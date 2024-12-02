using GEC_LAB._02_Window;
using GEC_LAB._04_Class;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GEC_LAB._03_UserControl.BasicComponent
{
    /// <summary>
    /// SeriesController.xaml 的交互逻辑
    /// </summary>
    public partial class SeriesController : UserControl
    {
        #region 静态绑定

        public string UUID
        {
            get { return (string)GetValue(UUIDProperty); }
            set { SetValue(UUIDProperty, value); }
        }

        public static readonly DependencyProperty UUIDProperty =
            DependencyProperty.Register("UUID", typeof(string), typeof(SeriesController), new PropertyMetadata(null));

        public string CurveName
        {
            get { return (string)GetValue(CurveNameProperty); }
            set { SetValue(CurveNameProperty, value); }
        }

        public static readonly DependencyProperty CurveNameProperty =
            DependencyProperty.Register("CurveName", typeof(string), typeof(SeriesController),
                new PropertyMetadata(new PropertyChangedCallback((s, e) =>  ((SeriesController)s).txtName.Text=(string)e.NewValue )));

        public string FormulaX
        {
            get { return (string)GetValue(FormulaXProperty); }
            set { SetValue(FormulaXProperty, value); }
        }

        public static readonly DependencyProperty FormulaXProperty =
            DependencyProperty.Register("FormulaX", typeof(string), typeof(SeriesController),
                new PropertyMetadata(new PropertyChangedCallback((s, e) =>  ((SeriesController)s).txtXFormula.Text=(string)e.NewValue )));

        public string FormulaY
        {
            get { return (string)GetValue(FormulaYProperty); }
            set { SetValue(FormulaYProperty, value);  }
        }

        public static readonly DependencyProperty FormulaYProperty =
            DependencyProperty.Register("FormulaY", typeof(string), typeof(SeriesController),
                new PropertyMetadata(new PropertyChangedCallback((s, e) => ((SeriesController)s).txtYFormula.Text = (string)e.NewValue)));

        public Color LineColor
        {
            get { return (Color)GetValue(LineColorProperty); }
            set { SetValue(LineColorProperty, value); }
        }

        public static readonly DependencyProperty LineColorProperty =
            DependencyProperty.Register("LineColor", typeof(Color), typeof(SeriesController),
                new PropertyMetadata(Colors.Black, new PropertyChangedCallback((s, e) => 
                    ((SeriesController)s).displayLine.Stroke= new SolidColorBrush((Color)e.NewValue)
                )));

        #endregion
        #region 全局变量
        public event RoutedEventHandler? copyFromThisEvent;
        public event RoutedEventHandler? deleteThisEvent;
        public event RoutedEventHandler? clearThisEvent;
        public event RoutedEventHandler? nameChangedEvent;
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

            txtYFormula.PreviewMouseLeftButtonDown += gotFocus;
            txtXFormula.PreviewMouseLeftButtonDown += gotFocus;


            txtName.TextChanged += (s, e) => {
                Name = txtName.Text;
                nameChangedEvent?.Invoke(this, null);
            };
            txtXFormula.TextChanged += (s, e) =>
            {
                FormulaX = txtXFormula.Text;
                formulaChangedEvent?.Invoke(this, null);
            };
            txtYFormula.TextChanged += (s, e) =>
            {
                FormulaY = txtYFormula.Text;
                formulaChangedEvent?.Invoke(this, null);
            };

        }


        public SeriesController()
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

            TextBox? handleControl = null;
            if (c.Name == "txtXFormula")
            {
                handleControl = txtXFormula;
            }
            else if (c.Name == "txtYFormula")
            {
                handleControl = txtYFormula;
            }
            if (handleControl != null)
            {
                ExpressEditor ee = new ExpressEditor(Gobals.main, handleControl.Text, ProjectHelper.GetEnablePins());
                if (ee.ShowDialog() == true)
                {
                    if (handleControl.Text != ee.data)
                    {
                        handleControl.Text = ee.data;
                    }
                    ((Panel)handleControl.Parent).Focus();
                }
            }
        }
        
        private void colorClick(object sender, RoutedEventArgs e)
        {
            colorChange(((Button)sender).Background);
        }



        #endregion
    }



}
