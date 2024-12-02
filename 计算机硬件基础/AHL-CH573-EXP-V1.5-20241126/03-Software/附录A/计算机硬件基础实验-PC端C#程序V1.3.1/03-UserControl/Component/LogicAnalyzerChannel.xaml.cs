using GEC_LAB._02_Window;
using GEC_LAB._04_Class;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using Prism.Mvvm;
using SkiaSharp;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Windows;
using static GEC_LAB._03_UserControl.Component.LogicAnalyzer;

namespace GEC_LAB._03_UserControl.Component
{
    /// <summary>
    /// LogicAnalyzerSeriesController.xaml 的交互逻辑
    /// </summary>
    public partial class LogicAnalyzerChannel : UserControl
    {
        public static readonly SKColor s_gray = new(230, 230, 230   );
        public static readonly SKColor s_gray1 = new(50, 50, 50);
        public static readonly SKColor s_gray2 = new(50, 50, 50);
        public static readonly SKColor s_dark3 = new(50, 50, 50);
        public static readonly float LineStroke = 2.5f;
        private LogicAnalyzerChannelViewModel data;
        public string? ChannelName
        {
            get => txtName.Text;
        }
        public int ChanelIndex
        {
            get {
                string s = StringUtils.nullToEmpty(btnColor.Content);
                if (int.TryParse(s, out int i))
                {
                    return i;
                }
                else return 0;
            }
            set => btnColor.Content = value;
        }
        public string? ChannelFomula
        {
            get => data.Formula;
        }
        public Color ChannelColor { get {
                if (btnColor.Background is SolidColorBrush scb)
                {
                    return scb.Color;
                }
                return Colors.White;
            }
        }

        private StepLineSeries<ObservablePoint> line;
        public ObservableCollection<ObservablePoint> points;
        public delegate void ColorChanged(Color c);
        public EventHandler? ChannelDeleteEvent;
        public object Sync { get; } = new object();
        public LogicAnalyzerChannel()
        {
            InitializeComponent();
            data = (LogicAnalyzerChannelViewModel)DataContext;
            line =
                new StepLineSeries<ObservablePoint>
                {
                    Stroke = new SolidColorPaint(brushToSK(btnColor.Background), LineStroke),
                    Fill = null,
                    GeometrySize = 0
                };
            points = new();
            Init();
        }
        public int count = 0;
        public LogicAnalyzerChannel(Axis[] XAxis, ObservableCollection<ObservablePoint> list, LogicAnalyzerDataModel.Channel channel)
        {
            InitializeComponent();
            data = (LogicAnalyzerChannelViewModel)DataContext;
            points = list;
            txtName.Text = channel.channelName;
            data.Formula = channel.formula;
            btnColor.Background=new SolidColorBrush(channel.ChannelColor);
            line = new StepLineSeries<ObservablePoint>
            {
                Values = points,
                Stroke = new SolidColorPaint(brushToSK(btnColor.Background), LineStroke),
                Fill = null,
                GeometrySize = 0
            };
            chart.Series = new ISeries[] { line };
            chart.XAxes = XAxis;
            Init();

        }
        public static SKColor brushToSK(Brush? brush)
        {
            if (brush != null && brush is SolidColorBrush b)
            {
                return new SKColor(b.Color.R, b.Color.G, b.Color.B);
            }
            return new SKColor(33, 150, 243);
        }
        private void Init() {
            chart.SyncContext = Sync;
            txtName.KeyUp += TxtName_KeyUp;
            txtFormula.KeyUp += TxtName_KeyUp;

            btnColor.Click += (s, e) => {
                LabColorSelecter labColorSelecter = new LabColorSelecter();
                labColorSelecter.Owner = Gobals.main;
                if (true == labColorSelecter.ShowDialog())
                {
                    btnColor.Background = new SolidColorBrush(labColorSelecter.SelectedColor);
                    line.Stroke = new SolidColorPaint(brushToSK(btnColor.Background), LineStroke);
                    ProjectHelper.Saved = false;
                }
            };
            btnDelete.Click += (s, e) => { ChannelDeleteEvent?.Invoke(this, new EventArgs()); };
            txtFormula.TextChanged+=(s,e) =>ProjectHelper.Saved = false;
            txtName.TextChanged+= (s, e) => ProjectHelper.Saved = false;
            chart.MouseDown += (sender, e) => e.Handled = true;
            chart.MouseUp += (s, e) => e.Handled = true;
            chart.MouseWheel += (s, e) => e.Handled = true;
            txtFormula.PreviewMouseLeftButtonDown += getFocus;
        }

        private void getFocus(object sender, RoutedEventArgs e)
        {
            ExpressEditor ee = new ExpressEditor(Gobals.main, data.Formula, ProjectHelper.GetEnablePins());
            if (ee.ShowDialog() == true)
            {
                if (ee.data != data.Formula)
                {
                    data.Formula = ee.data;
                }
                ((Panel)txtFormula.Parent).Focus();
            }
        }
        private void TxtName_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                txtFocus.Focus();
            }
        }

        public void Lock(){
            txtFocus.IsEnabled = false;
        }
        public void Unlock()
        {
            txtFocus.IsEnabled = true;
        }
        public static Axis[] GenerateXAxis() {
            return new Axis[]    {
            new Axis
            {
                TextSize = 12,
                Padding = new Padding(5, 15, 5, 5),
                LabelsPaint = new SolidColorPaint(s_gray),


                SubseparatorsCount = 5,
                ZeroPaint = new SolidColorPaint
                {
                    Color = s_gray1,
                    StrokeThickness = 2,
                    PathEffect = new DashEffect(new float[] { 3, 3 })
                },
                TicksPaint = new SolidColorPaint
                {
                    Color = s_gray,
                    StrokeThickness = 1.5f
                }
            }
};
        }
    }
    public class LogicAnalyzerChannelViewModel:BindableBase
    {
        private string? formula="";

        public string? Formula
        {
            get { return formula; }
            set { formula = value; RaisePropertyChanged(); }
        }


        public Margin Margin { get; set; } = new Margin(0);

        public Axis[] XAxes { get; set; } = LogicAnalyzerChannel.GenerateXAxis();

        public Axis[] YAxes { get; set; } =
        {
        new Axis
        {
            TextSize=0,
            Padding = new Padding(5, 0, 15, 0),
            MaxLimit=1,
            MinLimit=-1,
            LabelsPaint = new SolidColorPaint(LogicAnalyzerChannel.s_gray),
            SeparatorsPaint = new SolidColorPaint
            {
                Color = LogicAnalyzerChannel.s_gray,
                StrokeThickness = 0
            },
            SubseparatorsPaint = new SolidColorPaint
            {
                Color = LogicAnalyzerChannel.s_gray2,
                StrokeThickness = 0
            },
            ZeroPaint = new SolidColorPaint
            {
                Color = LogicAnalyzerChannel.s_gray1,
                StrokeThickness = 1,
                PathEffect = new DashEffect(new float[] { 3, 3 })
            }
        }
    };

        //public DrawMarginFrame Frame { get; set; } = new()
        //{
        //    Fill = new SolidColorPaint(LogicAnalyzerChannel.s_dark3),
        //    Stroke = new SolidColorPaint
        //    {
        //        Color = new(200,200,200),
        //        StrokeThickness = 2
        //    }
        //};
    }
}
