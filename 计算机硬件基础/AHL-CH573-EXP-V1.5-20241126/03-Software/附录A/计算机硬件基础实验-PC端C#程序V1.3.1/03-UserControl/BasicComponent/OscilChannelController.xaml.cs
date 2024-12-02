using GEC_LAB._02_Window;
using GEC_LAB._04_Class;
using Prism.Mvvm;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GEC_LAB._03_UserControl.BasicComponent
{
    /// <summary>
    /// SeriesController.xaml 的交互逻辑
    /// </summary>
    public partial class OscilChannelController : UserControl
    {
        #region 静态绑定

        public string UUID
        {
            get { return (string)GetValue(UUIDProperty); }
            set { SetValue(UUIDProperty, value); }
        }

        public static readonly DependencyProperty UUIDProperty =
            DependencyProperty.Register("UUID", typeof(string), typeof(OscilChannelController), new PropertyMetadata(null));

        public string ChannelName
        {
            get { return (string)GetValue(ChannelNameProperty); }
            set { SetValue(ChannelNameProperty, value); }
        }

        public static readonly DependencyProperty ChannelNameProperty =
            DependencyProperty.Register("ChannelName", typeof(string), typeof(OscilChannelController),
                new PropertyMetadata("", new PropertyChangedCallback((s, e) => 
                    ((OscilChannelController)s).txtName.Text = (string)e.NewValue
                )));

        public string Formula
        {
            get { return (string)GetValue(FormulaProperty); }
            set { SetValue(FormulaProperty, value); }
        }

        public static readonly DependencyProperty FormulaProperty =
            DependencyProperty.Register("Formula", typeof(string), typeof(OscilChannelController),
                new PropertyMetadata("", new PropertyChangedCallback((s, e) =>
                    ((OscilChannelController)s).txtFormula.Text = (string)e.NewValue)));

        public Color LineColor
        {
            get { return (Color)GetValue(LineColorProperty); }
            set { SetValue(LineColorProperty, value); }
        }

        public static readonly DependencyProperty LineColorProperty =
            DependencyProperty.Register("LineColor", typeof(Color), typeof(OscilChannelController),
                new PropertyMetadata(Colors.Pink, new PropertyChangedCallback((s, e) => {
                    ((OscilChannelController)s). displayLine.Stroke = new SolidColorBrush((Color)e.NewValue);
                })));


        public double Offset
        {
            get { return (double )GetValue(OffsetProperty); }
            set { SetValue(OffsetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Offset.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OffsetProperty =
            DependencyProperty.Register("Offset", typeof(double ), typeof(OscilChannelController),
                new PropertyMetadata(0.0,new PropertyChangedCallback((s,e) =>
                    ((OscilChannelController)s).numberOffset.Value = (double)e.NewValue )));



        #endregion
        #region 全局变量
        public event RoutedEventHandler? copyFromThisEvent;
        public event RoutedEventHandler? deleteThisEvent;
        public event RoutedEventHandler? clearThisEvent;
        public event RoutedEventHandler? nameChangedEvent;
        public event RoutedEventHandler? offsetChangedEvent;
        public event RoutedEventHandler? colorChangedEvent;
        public event RoutedEventHandler? formulaChangedEvent;

        #endregion
        #region 初始化
        private void Init()
        {
            btnColor1.Click += colorClick;
            btnColor2.Click += colorClick;
            btnColor3.Click += colorClick;
            btnColor4.Click += colorClick;
            btnColorPixel.Click += (s, e) => {
                LabColorSelecter labColorSelecter = new LabColorSelecter();
                labColorSelecter.Owner = Gobals.main;
                if (true == labColorSelecter.ShowDialog())
                {
                    colorChange(new SolidColorBrush(labColorSelecter.SelectedColor));
                }
            };
            btnClear.Click += (s, e) => clearThisEvent?.Invoke(this, new());
            btnCopy.Click += (s, e) => copyFromThisEvent?.Invoke(this, new RoutedEventArgs());
            btnDelete.Click += (s, e) =>
            {
                deleteThisEvent?.Invoke(this, new RoutedEventArgs());
            };


            txtFormula.PreviewMouseLeftButtonDown += gotFocus;
            txtName.TextChanged+=(s,e) => {
                ChannelName=txtName.Text;
                nameChangedEvent?.Invoke(this, null);
            } ;
            txtFormula.TextChanged += (s, e) =>
            {
                Formula=txtFormula.Text;
                formulaChangedEvent?.Invoke(this, null);
            };
            numberOffset.ValueChanged += (s, e) =>
            {
                Offset = numberOffset.Value == null ? 0 : (double)numberOffset.Value;
                offsetChangedEvent?.Invoke(this, new RoutedEventArgs());
            };
        }


        public OscilChannelController()
        {
            InitializeComponent();
            Init();
        }
        #endregion

        #region 开放

        private void colorChange(Brush brush)
        {
            if (brush is SolidColorBrush scb)
            {
                LineColor = scb.Color;
            }
            else
            {
                LineColor = Colors.Black;
            }
            colorChangedEvent?.Invoke(this,null);
        }
        #endregion
        #region 事件监听
        private void gotFocus(object sender, RoutedEventArgs e)
        {
            Control c = (Control)sender;

            ExpressEditor ee = new ExpressEditor(Gobals.main, txtFormula.Text, ProjectHelper.GetEnablePins());
            if (ee.ShowDialog() == true)
            {
                if (txtFormula.Text != ee.data)
                {
                    txtFormula.Text = ee.data;
                }
                ((Panel)txtFormula.Parent).Focus();
            }
        }
        
        private void colorClick(object sender, RoutedEventArgs e)
        {
            colorChange(((Button)sender).Background);
        }

        #endregion
    }
}
