using GEC_LAB._02_Window;
using GEC_LAB._03_UserControl;
using GEC_LAB._03_UserControl.body;
using GEC_LAB._03_UserControl.Body;
using GEC_LAB._04_Class;
using Prism.DryIoc;
using Prism.Ioc;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace GEC_LAB
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            //UI线程未捕获异常处理事件
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            //Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            //非UI线程未捕获异常处理事件
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Trace.WriteLine("we are in");
            return Container.Resolve<MainWindow>();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                if (Gobals.main != null)
                {
                    Gobals.main.gobalMessage("".Replace("{Exception}", $"{e.ExceptionObject}"));
                }
                else MessageBox.Show("".Replace("{Exception}", $"{e.ExceptionObject}"));
                Gobals.logger?.error("App", "".Replace("{Exception}", $"{e.ExceptionObject}"));
            }
            catch { }
        }

        private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            try
            {
                e.SetObserved();
                if (Gobals.main != null) Gobals.main.gobalMessage(e.Exception.Message);
                else MessageBox.Show(e.Exception.Message);
            }
            catch { }
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try{
                e.Handled = true;
                if (Gobals.main != null) Gobals.main.gobalMessage(e.Exception.Message);
                else MessageBox.Show(e.Exception.Message);
            }catch { }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<PageStep01, PageStep01ViewModel>(PrismNaviName.pageStep01);
            containerRegistry.RegisterForNavigation<PageStep02>(PrismNaviName.pageStep02);
            containerRegistry.RegisterForNavigation<PageStep03>(PrismNaviName.pageStep03);
            containerRegistry.RegisterForNavigation<PageStep04>(PrismNaviName.pageStep04);
            containerRegistry.RegisterForNavigation<BodyCanvas>(PrismNaviName.bodyCanvas);
            containerRegistry.RegisterForNavigation<BodyOverview, BodyOverviewViewModel>(PrismNaviName.bodyOverview);
            containerRegistry.RegisterForNavigation<BodyMcuOverview, BodyMcuOverviewViewModel>(PrismNaviName.bodyMcuOverview);
        }
    }
}
