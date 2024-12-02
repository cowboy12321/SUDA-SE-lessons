using GEC_LAB._04_Class;
using GEC_LAB._04_Class.Models;
using ImTools;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static GEC_LAB._04_Class.Models.ExpPin;

namespace GEC_LAB._03_UserControl.body
{
    public class BodyMcuOverviewViewModel: BindableBase
    {
        #region 变量
        #region 绑定变量
        private string mcuName= "项目没有MCU文件！请重新创建项目";


		public string McuName
		{
			get { return mcuName; }
			set { mcuName = value; RaisePropertyChanged(); }
		}

		private ObservableCollection<McuPinMode> leftPanelList =new ObservableCollection<McuPinMode>();

		public ObservableCollection<McuPinMode> LeftPanelList
        {
			get { return leftPanelList; }
			private set { leftPanelList = value;RaisePropertyChanged(); }
		}
        private ObservableCollection<McuPinMode> rightPanelList=new ObservableCollection<McuPinMode>();

        public ObservableCollection<McuPinMode> RightPanelList { 
            get { return rightPanelList; } 
            private set { rightPanelList = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<McuPinMode> leftPanelNoList = new ObservableCollection<McuPinMode>();

        public ObservableCollection<McuPinMode> LeftPanelNoList
        {
            get { return leftPanelNoList; }
            private set { leftPanelNoList = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<McuPinMode> rightPanelNoList = new ObservableCollection<McuPinMode>();

        public ObservableCollection<McuPinMode> RightPanelNoList
        {
            get { return rightPanelNoList; }
            private set { rightPanelNoList = value; RaisePropertyChanged(); }
        }
        #endregion
        #region
        DictionaryHelper<int, McuPinMode> mcuCache = new DictionaryHelper<int, McuPinMode>();
        private readonly string LOG = "BodyMcuOverviewViewModel";
        #endregion
        #endregion
        #region 初始化
        public BodyMcuOverviewViewModel() {
            Gobals.logger?.info(LOG, "init");
            MCU? mcu = ProjectHelper.SelectedMcu;
            if(mcu != null)
            {
                loadMcu(mcu);
            }
            ProjectHelper.PinStatusChangedEvent += no => {
                if (leftPanelList.Contains(mcuCache[no])) pinChanged(leftPanelList.FindFirst(x => x.No == no));
                if (rightPanelList.Contains(mcuCache[no])) pinChanged(rightPanelList.FindFirst(x => x.No == no));

            };
            ProjectHelper.McuReload += id =>
            {
                MCU? mcu = McuHelper.Instance.getMcuById(id);
                clear();
                if (mcu != null) loadMcu(mcu);
            };
            ProjectHelper.OnReload += () =>
            {
                MCU? mcu = ProjectHelper.SelectedMcu;
                clear();
                if (mcu != null) loadMcu(mcu);
            };
            Gobals.logger?.info(LOG, "done");
        }
        private void clear() {
            McuName = "项目没有MCU文件！请重新创建项目"; 
            mcuCache.Clear();
            LeftPanelList.Clear();
            RightPanelList.Clear();
        }
        private void pinChanged(McuPinMode pmode)
        {
            if (ProjectHelper.Pins.ContainsKey(pmode.No))
            {
                pmode.Enable = ProjectHelper.Pins[pmode.No].enable;
                pmode.PinMode = castMode(ProjectHelper.Pins[pmode.No].pinMode);
                pmode.Name = ProjectHelper.Pins[pmode.No].Name;
            }

        }
        private int castMode(PinMode mode)
        {
            return (int)mode;
        }
        private void loadMcu(MCU mcu)
        {

            Gobals.logger?.info(LOG, "load mcu");
            McuName = mcu.mcuName;

            foreach (var item in mcu.labelMap)
            {
                mcuCache.put(item.Key, new McuPinMode()
                {
                    Name = mcu.labelMap.GetValueOrDefault(item.Key, "undefined:mcuOverview"),
                    No = item.Key,
                    Enable = false,
                    Label = mcu.labelMap.GetValueOrDefault(item.Key, "undefined:mcuOverview")
                });
            }
            mcu.leftPanel.ForEach(x => { if (mcuCache.ContainsKey(x)) LeftPanelList.Add(mcuCache[x]); });
            mcu.rightPanel.ForEach(x => { if (mcuCache.ContainsKey(x)) RightPanelList.Add(mcuCache[x]); });
            foreach (ExpPin pin in ProjectHelper.Pins.Values)
            {
                if (mcuCache.ContainsKey(pin.no))
                {
                    McuPinMode pmode = mcuCache[pin.no];
                    mcuCache[pin.no].Enable= pin.enable;
                    if (pin.pinMode != PinMode.None) pmode.PinMode = castMode(pin.pinMode);
                    if (pin.Name != null)pmode.Name = pin.Name;
                }

            }
            LeftPanelNoList.Clear(); RightPanelNoList.Clear() ;
            foreach (var x in mcu.leftNoPanel) { LeftPanelNoList.Add(new McuPinMode() { Label=""+x}); }
            foreach (var x in mcu.rightNoPanel) { RightPanelNoList.Add(new McuPinMode() { Label = "" + x }); }
            Gobals.logger?.info(LOG, "load done");
        }
        #endregion
    }
}
