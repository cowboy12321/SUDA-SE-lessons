using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GEC_LAB._03_UserControl
{
    /// <summary>
    /// ComponentItem.xaml 的交互逻辑
    /// </summary>
    public partial class ComponentItem : UserControl
    {
        public static readonly DependencyProperty DisplaySourceProperty =
            DependencyProperty.Register(
              name: "DisplaySource",
              propertyType: typeof(ImageSource),
              ownerType: typeof(ComponentItem),
              typeMetadata: new FrameworkPropertyMetadata()
        );
        // Using a DependencyProperty as the backing store for DisplayName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayNameProperty =
            DependencyProperty.Register(
                "DisplayName", 
                typeof(string), 
                typeof(ComponentItem),
                new PropertyMetadata());

        public ImageSource DisplaySource
        {
            get => (ImageSource)GetValue(DisplaySourceProperty);
            set => SetValue(DisplaySourceProperty, value);
        }

        public string DisplayName
        {
            get { return (string)GetValue(DisplayNameProperty); }
            set { SetValue(DisplayNameProperty, value); }
        }

        public ComponentItem()
        { 
            this.DataContext = this;
            InitializeComponent();
        }
    }
}
