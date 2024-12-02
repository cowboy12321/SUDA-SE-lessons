using GEC_LAB._04_Class;
using GEC_LAB._04_Class.Interface;
using GEC_LAB._04_Class.Models;
using GEC_LAB._04_Class.MoveControl;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace GEC_LAB._03_UserControl.Component
{
    /// <summary>
    /// VImeter.xaml 的交互逻辑
    /// </summary>
    public partial class VImeter : UserControl, ISavedComponent
    {
        private VImeterDataModel data=new VImeterDataModel("未命名仪表",3.3,"");
        public double MaxValue { get => data.maxValue; set { data.maxValue = value; refreshControl(); } }
        public double Value
        {
            get => _value; set
            {
                if (value < 0) value = 0;
                if (value != _value)
                {
                    valueChanged(_value, value);
                    _value = value;
                }
            }
        }
        public string Express { get => data.express; set { data.express = value; lblExpress.Text = data.express; } }
        private readonly int SCALE = 35;
        private readonly double DEADANGLE = 45;
        private MouseController mouseController;
        private double _value;
        private Path? lastPath;
        public VImeter()
        {
            InitializeComponent();
            DataRefreshCenter.Instance.ComponentTickEvent += timer_tick;
            Width = Height = 200;

            mouseController = new MouseController(this, new VImeterAttachment(data, refreshControl));
            mouseController.setMovable();

            this.SizeChanged += (s, e) => refreshControl() ;
            refreshControl();
            Value = 3.3;
        }
        #region 事件监听
        private void valueChanged(double oldValue, double newValue)
        {
            if(lastPath!=null &&cv.Children.Contains(lastPath)) cv.Children.Remove(lastPath);
            Path p = new Path();
            p.Stroke = Brushes.Teal;
            p.StrokeThickness = 4;
            lblValue.Text = Math.Round(newValue, 2).ToString();
            var converter = TypeDescriptor.GetConverter(typeof(Geometry));
#pragma warning disable 8600
            double r = cv.Width / 2;
            double angle = Math.Min(360 - DEADANGLE, DEADANGLE + newValue * (360 - DEADANGLE * 2) / data.maxValue);
            string d = string.Format("M {0},{1} A {2},{3} 0 {4} 1 {5},{6}", getX(r, DEADANGLE, 1), getY(r, DEADANGLE, 1), r, r, angle > 180 + DEADANGLE ? 1 : 0, getX(r, angle, 1), getY(r, angle, 1));
            p.Data = (Geometry)converter.ConvertFrom(d);
            
#pragma warning restore 8600
            cv.Children.Add(p);
            lastPath = p;
        }

        public SavedComponent save()
        {
            return new SavedComponent(this.GetType(), data);
        }

        public void restore(object? o)
        {
            string? s = o?.ToString();
            if (s != null)
            {
                VImeterDataModel? t = JsonConvert.DeserializeObject<VImeterDataModel>(s);
                if (t != null) { 
                    data = t;
                    refreshControl();
                    mouseController.setAttach(new VImeterAttachment(data,refreshControl));
                }
            }
        }

        private void refreshControl()
        {
            lblExpress.Text = data.name;
            cv.Children.Clear();
            double h = this.Width < this.Height ? Width : Height;
            double r = h / 2;
            cv.Width = h; cv.Height = h;

            if (h.ToString() != "NaN")
            {
                //外圈
                Ellipse ell1 = new Ellipse();
                Ellipse ell2 = new Ellipse();
                ell1.Height = ell1.Width = ell2.Height = ell2.Width = h;
                ell1.Stroke = new SolidColorBrush(Color.FromRgb(50, 69, 73));
                ell1.Effect = new BlurEffect() { Radius = 5, KernelType = KernelType.Box };
                ell2.Fill = Brushes.White;
                cv.Children.Add(ell1);
                cv.Children.Add(ell2);
                lblExpress.FontSize = h / 12;
                lblValue.FontSize = h / 4;
                for (int i = 0; i <= SCALE; ++i)
                {
                    double angle = DEADANGLE + i * (360 - DEADANGLE * 2) / SCALE;
                    if (i % 5 == 0)
                    {
                        Line l = new Line();
                        l.X1 = getX(r, angle, 0.99);
                        l.X2 = getX(r, angle, 0.9);
                        l.Y1 = getY(r, angle, 0.99);
                        l.Y2 = getY(r, angle, 0.9);
                        l.Stroke = Brushes.Red;
                        l.StrokeThickness = 2;
                        cv.Children.Add(l);

                        Label lbl = new Label();
                        lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
                        lbl.VerticalContentAlignment = VerticalAlignment.Center;
                        lbl.Width = h / 6;
                        lbl.Height = h / 9;
                        lbl.Padding = new Thickness(0);
                        lbl.Background = Brushes.Transparent;
                        lbl.Content = Math.Round(i * data.maxValue / SCALE, 2).ToString();
                        lbl.FontSize = h / 21;
                        Canvas.SetTop(lbl, getY(r, angle, 0.8) - lbl.Height / 2);
                        Canvas.SetLeft(lbl, getX(r, angle, 0.8) - lbl.Width / 2);
                        cv.Children.Add(lbl);
                    }
                    else
                    {
                        Line l = new Line();
                        l.X1 = getX(r, angle, 0.99);
                        l.X2 = getX(r, angle, 0.95);
                        l.Y1 = getY(r, angle, 0.99);
                        l.Y2 = getY(r, angle, 0.95);
                        l.Stroke = Brushes.Black;
                        l.StrokeThickness = 1;
                        cv.Children.Add(l);

                    }
                }
                valueChanged(0, _value);
            }
        }
        #endregion
        #region 辅助函数
        private double getX(double r, double angle, double zoom)
        {
            double v = r - r * zoom * Math.Sin(angle / 180 * Math.PI);
            return v;
        }
        private double getY(double r, double angle, double zoom)
        {
            double v = r + r * zoom * Math.Cos(angle / 180 * Math.PI);
            return v;

        }

        #endregion
        #region 5.定时处理
        public void timer_tick()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                if (DataRefreshCenter.GetInstance().tryCalculate(data.express, out double res))
                {
                    if(res>MaxValue) MaxValue= res;
                    Value = res;
                }
            }));
        }
        #endregion
    }
    public class VImeterDataModel
    {
        public string name;
        public double maxValue;
        public string express;
        public VImeterDataModel(string name,double m, string e)
        {
            this.name = name; maxValue = m; express = e;
        }
    }

}
