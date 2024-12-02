using GEC_LAB._02_Window;
using GEC_LAB._04_Class;
using Panuon.WPF.UI;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace GEC_LAB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WindowX
    {
        #region 1.全局变量
        private Stack<Window> wStack = new Stack<Window>();
        #endregion
        #region 2.初始化
        private bool justTestOnceflag=true;
        public MainWindow(IRegionManager regionManager)
        {
            InitializeComponent();
            Trace.WriteLine("MainWindow initing");
            Gobals.main = this;
            controlBarShowBtn.Click += (s, e) => { 
                if(justTestOnceflag)
                {
                    controlBarBorder.Visibility = Visibility.Collapsed;
                    justTestOnceflag = false;
                }
                else
                {
                    controlBarBorder.Visibility = Visibility.Visible;
                    justTestOnceflag = true;
                }
            };
            this.DataContext  = new GecLabViewModels(regionManager);
        }

        #endregion
        #region 3. 开放函数
        DateTime toastDate = DateTime.Now;
        public void pushWindow(Window w)
        {
            wStack.Push(w);
        }
        public void gobalMessage(string message)
        {
            Dispatcher.Invoke(new Action(() => { 
                while(wStack.Count > 0 && wStack.Peek().DialogResult!=null) wStack.Pop();
                if(wStack.Count==0)MessageBoxX.Show(this, message,"错误",MessageBoxButton.OK,MessageBoxIcon.Error);
                else
                {
                    Window window = wStack.Peek();
                    MessageBoxX.Show(window, message, "错误", MessageBoxButton.OK, MessageBoxIcon.Error);
                }
            }));
        }
        public void gobalToast(string message)
        {
            if(DateTime.Now -toastDate>Gobals.ToastInterval) {
                Dispatcher.Invoke(new Action(() =>
                {
                    Toast(message);
                }));
                toastDate = DateTime.Now;
            }
        }
        public void gobalTips(string tips)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                TipsText.Text = tips;
            }));

        }
        public void gobalTitle(string projectName)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                this.Title = projectName + "  "+Gobals.softName+" v"+Gobals.softVersion;
            }));
        }
        public void gobalTitleUnSave()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                if (!this.Title.EndsWith("*")) this.Title=this.Title+"*";
            }));
        }
        public void gobalTitleSaved()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                if (this.Title.EndsWith("*")) this.Title = this.Title.Substring(0,Title.Length-1);
            }));
        }
        public void gobalStatus(string s)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                StatusHintBar.Content = s;
            }));
        }
        #endregion
        #region 4. 事件监听


        #endregion
    }
}
