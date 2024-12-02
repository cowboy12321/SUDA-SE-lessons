using System.Windows.Controls;

namespace GEC_LAB._03_UserControl.Component
{
    /// <summary>
    /// MultiCurveAttachment.xaml 的交互逻辑
    /// </summary>
    public partial class MultiCurveAttachment : UserControl
    {
        public MultiCurveAttachment()
        {
            InitializeComponent();
            init();
        }
        public void init()
        {
            Width = 250;
            Height = 400;
        }
    }
}
