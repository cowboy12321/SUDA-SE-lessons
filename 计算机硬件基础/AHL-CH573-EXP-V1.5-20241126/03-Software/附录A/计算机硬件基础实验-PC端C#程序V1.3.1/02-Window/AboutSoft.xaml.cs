using GEC_LAB._04_Class;
using System.Windows;

namespace GEC_LAB._02_Window
{
    /// <summary>
    /// AboutSoft.xaml 的交互逻辑
    /// </summary>
    public partial class AboutSoft : Window
    {
        public AboutSoft(Window? owner)
        {
            InitializeComponent();
            Owner = owner;
            init();
        }
        private void init() {
            btnConfirm.Click += (s, e) => Close();
            lblSoftName.Content = Gobals.softName+" v"+Gobals.softVersion;
            lblBuildTime.Content = "构建时间：" + Gobals.createDate;
        }
    }
}
