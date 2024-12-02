using GEC_LAB._04_Class;
using GEC_LAB._04_Class.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;

namespace GEC_LAB._02_Window
{
    public class GecLabViewModels:BindableBase
    {
        #region 1.关联变量
        private string statusHint;

        public string StatusHint
        {
            get { return statusHint; }
            set { statusHint = value; RaisePropertyChanged(); }
        }
        private string tips="";

        public string Tips
        {
            get { return tips; }
            set { tips = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<SideMenuItem> sideMenuItems;

        public ObservableCollection<SideMenuItem> SideMenuItems
        {
            get { return sideMenuItems; }
            set { sideMenuItems = value; RaisePropertyChanged(); }
        }
        private string mcuName="";

        public string McuName
        {
            get { return mcuName; }
            set { mcuName =value; RaisePropertyChanged(); }
        }
        private string gecTitle = Gobals.softName+" V"+Gobals.softVersion;

        public string GecTitle
        {
            get { return gecTitle; }
            set { gecTitle = value; RaisePropertyChanged(); }
        }
        private string softName = "关于 " + Gobals.softNameEng;

        public string SoftName
        {
            get { return softName; }
            set { softName  = value; }
        }



        #endregion
        #region 2.命令
        public DelegateCommand<SideMenuItem> NavigateCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand SaveAsCommand { get; private set; }
        public DelegateCommand OpenCommand { get; private set; }
        public DelegateCommand NewCommand { get; private set; }
        public DelegateCommand AboutCommand { get; private set; }
        public DelegateCommand OpenDebugWindowCommand { get; private set; }
        public DelegateCommand OpenLoggerWindowCommand { get; private set; } 
        public DelegateCommand ChangeMcuCommand { get; private set; }

        public DelegateCommand LoadedCommand { get; private set; }

        #endregion
        #region 3.全局变量
        public Logger logger;
        private readonly string LOG = "GEC_LAB";
        #endregion
        #region 4.初始化
        public GecLabViewModels(IRegionManager regionManager)
        {
            logger = new Logger();
            Gobals.logger = logger;
            Thread.CurrentThread.Name = "main";
            logger.info(LOG,"system init , "+Gobals.softName+" V"+Gobals.softVersion);
            ProjectHelper.Init();

            statusHint = StatusHint = "初始化成功！";
            Tips = "等待下位机蓝灯亮起后连接";
            sideMenuItems = SideMenuItems = new ObservableCollection<SideMenuItem>
            {
                new SideMenuItem() { Text = "基本信息" , Page=PrismNaviName.pageStep01,BodyPage=PrismNaviName.bodyOverview},
                new SideMenuItem() { Text = "配置引脚" , Page=PrismNaviName.pageStep02,BodyPage=PrismNaviName.bodyMcuOverview},
                new SideMenuItem() { Text = "设置图表" , Page=PrismNaviName.pageStep03,BodyPage=PrismNaviName.bodyCanvas},
                new SideMenuItem() { Text = "开始控制" , Page=PrismNaviName.pageStep04,BodyPage=PrismNaviName.bodyCanvas}
            };
            NavigateCommand = new DelegateCommand<SideMenuItem>(s => {
                if (s != null)
                {
                    Gobals.logger?.info(LOG, "Navigate sideBarRegion to " + s.Page);
                    regionManager.Regions[PrismRegions.sideBarRegion].RequestNavigate(s.Page);
                    Gobals.logger?.info(LOG, "Navigate bodyRegion to " + s.BodyPage);
                    regionManager.Regions[PrismRegions.bodyRegion].RequestNavigate(s.BodyPage);
                }
            });
            SaveCommand = new DelegateCommand(() => ProjectHelper.SaveProject());
            SaveAsCommand = new DelegateCommand(() => ProjectHelper.SaveAsProject());
            OpenCommand = new DelegateCommand(() => { ProjectHelper.OpenProject(); });
            NewCommand = new DelegateCommand(() => { ProjectHelper.NewProject(); });
            AboutCommand = new DelegateCommand(() => {
                AboutSoft aboutSoft = new (Gobals.main);
                aboutSoft.Title = SoftName;
                aboutSoft.ShowDialog(); });
            OpenDebugWindowCommand = new DelegateCommand(() => { new Debug(Gobals.main).Show(); });
            OpenLoggerWindowCommand = new DelegateCommand(() => { logger.Owner = Gobals.main; logger.Show(); }); 
            ChangeMcuCommand = new DelegateCommand(changeMcu);
            LoadedCommand = new DelegateCommand(() =>
            {
                regionManager.Regions[PrismRegions.sideBarRegion].RequestNavigate(PrismNaviName.pageStep01);
                regionManager.Regions[PrismRegions.bodyRegion].RequestNavigate(PrismNaviName.bodyOverview);
            });

            Application.Current.MainWindow.Closing+= (s, e) =>
            {
                if (!ProjectHelper.CheckAndSave()) e.Cancel = true;
            };
            Application.Current.MainWindow.Closed+= (s, e) =>
            {
                ProjectHelper.SavePath();
                EmuartHandler.GetInstance().stop();
                System.Environment.Exit(0);
            };
            ProjectHelper.OnReload += () =>
            {
                Gobals.main?.gobalTitleSaved();
                Gobals.main?.gobalStatus("初始化成功！");
            };
            ProjectHelper.SaveStatusChanged += () => { 
                if (ProjectHelper.Saved) Gobals.main?.gobalTitleSaved(); 
                else Gobals.main?.gobalTitleUnSave(); };
            ProjectHelper.McuReload += mcuId =>
            {
                string? m1 = McuHelper.Instance.getMcuById(mcuId)?.mcuName;
                if (m1 == null) McuName = "未找到mcu";
                else McuName = "当前MCU：" + m1;
            };
            string? m1 = ProjectHelper.SelectedMcu?.mcuName;
            if (m1 == null) McuName = "未找到mcu";
            else McuName = "当前MCU："+m1;
            logger.info(LOG, "init done! mcu:"+m1);
            Gobals.main?.gobalStatus("初始化成功！");
        }   
        #endregion
        #region 5.函数
        private void changeMcu()
        {
            List<MCU> mcus = McuHelper.Instance.GetMcuList();
            List<object> list = mcus.Select(mcu => (object)mcu.mcuName).ToList();
            SelectBox selectBox = new SelectBox(Gobals.main,"选择芯片", list,0);
            if (selectBox.ShowDialog() == true)
            {
                int selected = selectBox.selected;
                ProjectHelper.ChangeMcu(mcus[selected].mcuIdentifier);
            }
        }
        #endregion
    }
}
