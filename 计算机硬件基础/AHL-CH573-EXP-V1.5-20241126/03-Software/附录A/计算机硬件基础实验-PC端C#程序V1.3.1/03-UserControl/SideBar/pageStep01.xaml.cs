using System.Windows.Controls;

namespace GEC_LAB._03_UserControl
{
    /// <summary>
    /// pageStep01.xaml 的交互逻辑
    /// </summary>
    public partial class PageStep01 : UserControl
    {
        public PageStep01()
        {
            this.DataContext = new PageStep01ViewModel();
            InitializeComponent();
        }
    }
}
