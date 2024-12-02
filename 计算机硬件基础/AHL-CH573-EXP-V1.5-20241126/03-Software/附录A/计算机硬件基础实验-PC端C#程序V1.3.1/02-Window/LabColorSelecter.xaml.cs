using System.Windows;
using System.Windows.Media;

namespace GEC_LAB._02_Window
{
    /// <summary>
    /// LabColorSelecter.xaml 的交互逻辑
    /// </summary>
    public partial class LabColorSelecter : Window
    {
        public Color SelectedColor = Colors.Black;
        public LabColorSelecter()
        {
            InitializeComponent();
            btnConfirm.Click += Button_Confirm;
            btnCanel.Click += Button_Cancel;
        }

        private void Button_Confirm(object sender, RoutedEventArgs e)
        {
            SelectedColor = selector.SelectedColor;
            DialogResult = true;
        }
        private void Button_Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
