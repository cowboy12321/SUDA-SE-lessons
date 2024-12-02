using GEC_LAB._04_Class;
using System;
using System.Windows;

namespace GEC_LAB._02_Window
{
    /// <summary>
    /// Debug.xaml 的交互逻辑
    /// </summary>
    public partial class Debug : Window
    {
        public Debug(Window? owner)
        {
            InitializeComponent();
            Owner = owner;
            init();
        }
        bool debugOpen = true, userOpen = true;
        bool debugScroll = true, userScroll = true;

        public void init()
        {
            debugViewer.ScrollChanged += (s, e) =>  debugScroll = false;
            userViewer.ScrollChanged += (s, e) =>  userScroll = false;
            EmuartHandler.GetInstance().DebugRawDataReceiveEvent += str =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    if (debugOpen)
                    {
                        string s = debugDisplay.Text + '\n' + str;
                        if (s.Length > 2000) debugDisplay.Text = s.Substring(s.Length - 2000);
                        else debugDisplay.Text = s;
                        if (debugScroll) debugViewer.ScrollToEnd();
                    }
                }));
            };

            EmuartHandler.GetInstance().UserRawDataReceiveEvent += str =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    if (userOpen)
                    {
                        string s = userDisplay.Text + '\n' + str;
                        if (s.Length > 2000) userDisplay.Text = s.Substring(s.Length - 2000);
                        else userDisplay.Text = s;
                        if (userScroll) userViewer.ScrollToEnd();
                    }
                }));
            };

            debugButton.Click += (s, e) =>
            {
                if (debugButton.Content.ToString() == "暂停")
                {
                    debugButton.Content = "开始";
                    debugOpen = false;
                }
                else
                {
                    debugButton.Content = "暂停";
                    debugOpen = true;
                }
            };
            userButton.Click += (s, e) =>
            {
                if (userButton.Content.ToString() == "暂停")
                {
                    userButton.Content = "开始";
                    userOpen = false;
                }
                else
                {
                    userButton.Content = "暂停";
                    userOpen = true;
                }
            };
            debugClearButton.Click += (s, e) => debugDisplay.Text = ""; 
            userClearButton.Click += (s, e) => userDisplay.Text = ""; 
            debugGotoButtomBtn.Click += (s, e) => debugScroll = true;
            userGotoButtomBtn.Click += (s, e) => userScroll = true;
        }
    }
}
