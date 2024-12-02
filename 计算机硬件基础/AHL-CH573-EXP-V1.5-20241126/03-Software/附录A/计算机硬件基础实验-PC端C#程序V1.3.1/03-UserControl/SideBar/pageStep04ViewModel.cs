using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Linq;
using System.Data;
using GEC_LAB._04_Class;
using GEC_LAB._04_Class.Models;
using System.Collections.Generic;
using Prism.Commands;
using System.Threading;

namespace GEC_LAB._03_UserControl
{
    internal class PageStep04ViewModel:BindableBase,INavigationAware
    {
        #region 变量绑定
        public EventHandler? PageChange;
        private string linkStatus;

        public string LinkStatus
        {
            get { return linkStatus; }
            set { linkStatus = value; RaisePropertyChanged(); }
        }

        public DelegateCommand DeLinkCommand { get; private set; }
        #endregion

        #region 全局变量
        private readonly string LOG = "PageStep04ViewModel";
        #endregion

        #region 初始化
        public PageStep04ViewModel()
        {
            Gobals.logger?.info(LOG, "init");
            linkStatus = LinkStatus = "点击连接按钮连接设备";
            DataRefreshCenter.Instance.WatchDogHungry += () =>
            {
                bool f1 = EmuartHandler.GetInstance().UserState;
                bool f2 = EmuartHandler.GetInstance().DebugState;
                //20240918debug口连接丢失后系统串口的isopen属性依然是true?应该是个bug，所以两个连接口有一个连不上就算失败
                if (!EmuartHandler.GetInstance().UserState || !EmuartHandler.GetInstance().DebugState)
                {
                    hint("连接已丢失，请检查线路！");
                    EmuartHandler.Instance.stop();
                    return false;
                }
                else
                {
                    return true;
                }
            };
            DeLinkCommand = new DelegateCommand(() =>
            {
                if (EmuartHandler.GetInstance().UserState)
                {
                    DataRefreshCenter.Instance.enableDog = false;
                    EmuartHandler.GetInstance().stop();
                    LinkStatus = "点击按钮连接设备";
                    Gobals.main?.gobalStatus("已断开");
                    Gobals.main?.gobalToast("已断开");
                }
            });
            Gobals.logger?.info(LOG, "init done");
        }
        #endregion
        
        #region 信息输出
        public void hint(string s)
        {
            LinkStatus = s;     //底部提示
            Gobals.main?.gobalStatus(s);
        }
        #endregion

        #region 通信
        public void linkDevice(List<Pair<int,bool>> digitalOut,List<Pair<int,double>> analogOut) {
            DataRefreshCenter.Instance.EnableDog = false;
            EmuartHandler.GetInstance().linkDebugDevice(hint);
            if (EmuartHandler.GetInstance().DebugInfo != "")
            {
                EmuartHandler.GetInstance().linkUserDevice(hint);
                if (EmuartHandler.GetInstance().UserState)
                {
                    LinkStatus = EmuartHandler.GetInstance().DebugInfo;
                    Gobals.main?.gobalStatus("连接成功！");
                    MCU? mcu = ProjectHelper.SelectedMcu;
                    LinkStatus += "   McuVersion:" + EmuartHandler.GetInstance().UserVersion + "   LVersion:" + EmuartHandler.GetInstance().LittleUserVersion;
                    if (mcu == null)
                    {
                        Gobals.main?.gobalStatus("连接成功，但缺少mcu信息");
                    }
                    else if (EmuartHandler.GetInstance().McuId != mcu.mcuIdentifier)
                    {
                        Gobals.main?.gobalMessage("连接成功,芯片型号检查有误，但请检查连接的设备是否为所选择的MCU");
                    }else{
                        int userV = EmuartHandler.GetInstance().UserVersion << 18 | EmuartHandler.GetInstance().LittleUserVersion;
                        int targetV = Gobals.targetMcuVersion << 18 | mcu.targetMcuVersion;
                        string msg = "下位机程序版本："+
                            EmuartHandler.GetInstance().UserVersion + "." + EmuartHandler.GetInstance().LittleUserVersion + 
                            "。上位机程序版本：" +Gobals.targetMcuVersion + "." + mcu.targetMcuVersion;
                        if (userV < targetV)
                        {
                            Gobals.main?.gobalMessage("请更新MCU程序！" +msg);
                        } else if (userV > targetV)
                        {
                            Gobals.main?.gobalMessage("请更新本程序！" + msg);
                        }
                        else
                        {
                            Gobals.main?.gobalToast("连接成功");
                        }
                    }

                    sendAllMsg(digitalOut,analogOut);
                    DataRefreshCenter.Instance.EnableDog = true;
                    DataRefreshCenter.Instance.EnableComponentTick = true;
                }
                else
                {
                    EmuartHandler.GetInstance().stop();
                    Gobals.main?.gobalStatus("连接失败，请检查后重试！");
                    Gobals.main?.gobalToast("连接失败");
                    LinkStatus = "user连接失败，请尝试复位下位机后连接\n,bios信息：" +
                            EmuartHandler.GetInstance().DebugInfo;
                    EmuartHandler.GetInstance().stop();
                }
            }
            else
            {
                Gobals.main?.gobalStatus("连接失败，请检查后重试！");
                Gobals.main?.gobalToast("连接失败");
                LinkStatus = "bios连接失败，请尝试复位下位机后连接";
                EmuartHandler.GetInstance().stop();
            }
        }
        public void sendAllMsg(List<Pair<int, bool>> digitalOut, List<Pair<int, double>> analogOut)
        {
            openIn();
            Thread.Sleep(20);
            Command cmd = new Command();
            digitalOut.ForEach(x => cmd.digitalControl((short)x.First, x.Second ? 1 : 0));
            analogOut.ForEach(x => cmd.analogControl((short)x.First, x.Second));
            cmd.send();
            Thread.Sleep(10);

        }
        private void generalControl(Action action)
        {
            if (!EmuartHandler.GetInstance().UserState)
            {
                Gobals.main?.gobalToast("请先连接MCU");
            }
            if (DataRefreshCenter.Instance.EnableDog&&DataRefreshCenter.Instance.DogStatus==false)
            {
                Gobals.main?.gobalToast("MCU连接已断开，请重新连接");
            }
            else
            {
                action.Invoke();
            }

        }
        public void digitalControl(short no,bool value)
        {
            generalControl(() =>new Command().digitalControl(no,value? 1 :0).send());
        }
        public void analogControl(short no, double value)
        {
            generalControl(() =>new Command().analogControl(no, value).send());
        }

        private void openIn()
        {
            Command cmd = new Command();

            ProjectHelper.Pins.Values
                .Where(x => x.pinMode == ExpPin.PinMode.DigitalIn && x.enable)
                .ToList().ForEach(x=>cmd.openGPIO(x.no));

            ProjectHelper.Pins.Values
                .Where(x => x.pinMode == ExpPin.PinMode.AnalogIn && x.enable)
                .ToList().ForEach(x => cmd.openAnalog(x.no));
            cmd.send();
        }
        #endregion

        #region 事件监听
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Gobals.logger?.info(LOG, "OnNavigatedTo");
            if (EmuartHandler.GetInstance().UserState)
            {
                openIn();
            }
            PageChange?.Invoke(null,new EventArgs());
            Gobals.logger?.info(LOG, "OnNavigatedTo end");
            Gobals.main?.gobalTips("出现连接失败请复位下位机重新连接。务必等到蓝灯亮起再重新连接！");
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
        #endregion

    }
}
