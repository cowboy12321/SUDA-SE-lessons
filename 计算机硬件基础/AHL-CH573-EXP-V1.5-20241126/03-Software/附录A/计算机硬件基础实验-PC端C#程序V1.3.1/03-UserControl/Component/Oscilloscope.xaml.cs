using GEC_LAB._04_Class;
using GEC_LAB._04_Class.Interface;
using GEC_LAB._04_Class.Models;
using GEC_LAB._04_Class.MoveControl;
using LiveChartsCore.Defaults;
using Newtonsoft.Json;
using ScottPlot;
using ScottPlot.Plottables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace GEC_LAB._03_UserControl.Component
{
    /// <summary>
    /// Oscilloscope.xaml 的交互逻辑
    /// </summary>
    public partial class Oscilloscope : UserControl , ISavedComponent
    {
        MouseController mouseController;
        OscilloscopeViewModel viewModel;
        private readonly string LOG = "Oscilloscope";
        private long startTime = 0;
        private bool recording = false;
        public Oscilloscope()
        {
            InitializeComponent();
            viewModel = new();
            OscilloscopeAttachment oscilloscopeAttachment = new OscilloscopeAttachment();
            mouseController =new(this,oscilloscopeAttachment);
            oscilloscopeAttachment.DataContext = viewModel;
            DataContext = viewModel;
            mouseController.setMovable();
            Init();
        }

        private Dictionary<OscilChannel, DisplayScatter> records = new();
        private class DisplayScatter
        {
            public Scatter scatter;
            public HorizontalLine horizontalLine;
            public List<double> xs;
            public List<double> ys;
            public DisplayScatter(Scatter scatter, HorizontalLine horizontalLine, List<double> xs, List<double> ys)
            {
                this.horizontalLine = horizontalLine;
                this.scatter = scatter;
                this.xs = xs;
                this.ys = ys;
            }
        }

        private void Init() {
            Plot plot = forms.Plot;
            forms.Plot.Font.Set("微软雅黑");
            this.Height = 500;
            this.Width = 600;
            forms.MouseDown += (sender, e) => e.Handled = true;
            forms.MouseUp += (s, e) => e.Handled = true;

            ScottPlot.Control.InputBindings customInputBindings = new()
            {
                DragPanButton = ScottPlot.Control.MouseButton.Left,
                ZoomInWheelDirection = ScottPlot.Control.MouseWheelDirection.Up,
                DragZoomButton = ScottPlot.Control.MouseButton.Right,
                ZoomOutWheelDirection = ScottPlot.Control.MouseWheelDirection.Down,
                ClickContextMenuButton = ScottPlot.Control.MouseButton.Right
            };
            ScottPlot.Control.Interaction inter = new ScottPlot.Control.Interaction(forms)
            {
                Inputs = customInputBindings
            };
            forms.Menu?.Clear();
            inter.Actions.ShowContextMenu += (s, e) => { DeployCustomMenu(null, null); };
            forms.Interaction = inter;
            // change figure colors
            plot.FigureBackground.Color = Color.FromHex("#181818");
            plot.DataBackground.Color = Color.FromHex("#1f1f1f");

            // change axis and grid colors
            plot.Axes.Color(Color.FromHex("#d7d7d7"));
            plot.Grid.MajorLineColor = Color.FromHex("#404040");

            // change legend colors
            plot.Legend.BackgroundColor = Color.FromHex("#404040");
            plot.Legend.FontColor = Color.FromHex("#d7d7d7");
            plot.Legend.OutlineColor = Color.FromHex("#d7d7d7");
            forms.PreviewMouseDown += (s, e) =>
            {
                mouseController.topping();
                mouseController.showAttach();
            };



            viewModel.seriesListChangedEvent += (s, e) => seriesChanged();
            viewModel.clearSeriesEvent += clearSeries;
            viewModel.colorChangedEvent += colorChanged;
            viewModel.nameChangedEvent += nameChanged;
            viewModel.offsetChangedEvent += offsetChanged;

            btnStart.Click += RecordStart;
            btnStop.Click += RecordStop;
            btnReset.Click += Reset;

        }

        #region 3.内部函数
        private void Reset(object sender, RoutedEventArgs e)
        {
            if (recording) RecordStop(this, new());
            foreach (var series in viewModel.SeriesList)
            {
                if(
                records.TryGetValue(series,out var ds))
                {
                    ds.xs.Clear();
                    ds.ys.Clear();
                }
                series.points.Clear();
            }
            forms.Refresh();
            ProjectHelper.Saved = false;
        }

        private void RecordStop(object sender, RoutedEventArgs e)
        {
            recording = false;
            btnStop.Visibility = Visibility.Collapsed;
            btnStart.Visibility = Visibility.Visible;
        }

        private void RecordStart(object sender, RoutedEventArgs e)
        {
            foreach (var series in viewModel.SeriesList)
            {
                
                if (records.TryGetValue(series, out var ds))
                {
                    ds.xs.Clear();
                    ds.ys.Clear();
                }
                series.points.Clear();
            }
            if (viewModel.Rolling)
            {
                forms.Plot.Axes.SetLimitsX(0,viewModel.RollingTime);
            }
            forms.Refresh();
            recording = true;
            startTime = DateTime.Now.Ticks;
            DataRefreshCenter.Instance.DataReceiveEvent += tick;


            btnStart.Visibility = Visibility.Collapsed;
            btnStop.Visibility = Visibility.Visible;
        }
        
        public void tick()
        {
            if (!recording)
            {
                DataRefreshCenter.Instance.DataReceiveEvent -= tick;
                return;
            }
            Dispatcher.Invoke(new Action(() =>
            {
                double x=(DateTime.Now.Ticks - startTime)/10000000.0, y;
                foreach (OscilChannel series in viewModel.SeriesList)
                {
                    if (records.TryGetValue(series, out DisplayScatter? ds))
                    {
                        if (ds != null &&
                            DataRefreshCenter.GetInstance().tryCalculate(series.Formula, out y))
                        {
                            
                            addPoint(ds, x, y);
                            series.points.Add(new GPoint(x, y));
                        }
                        else
                        {
                            lblHint.Content = "数据绘制失败!";
                        }
                    }
                }
            }
            ));
        }
        
        private void addPoint(DisplayScatter ds, double x, double y)
        {
            Plot plot = forms.Plot;

            ds.xs.Add(x); ds.ys.Add(y);
            plot.Axes.SetLimitsX(Math.Max( 0,x-viewModel.RollingTime),Math.Max(x,viewModel.RollingTime));
            ProjectHelper.Saved = false;
            forms.Refresh();
        }
        
        private void seriesChanged()
        {
            Plot plot = forms.Plot;
            Collection<OscilChannel> temp = new Collection<OscilChannel>();
            foreach (var item in viewModel.SeriesList)
            {
                temp.Add(item);
                if (!records.ContainsKey(item))
                {
                    List<double> xs = item.points.Select(p => p.x).ToList();
                    List<double> ys = item.points.Select(p => p.y).ToList();
                    Scatter scatter = plot.Add.ScatterLine(xs, ys);
                    HorizontalLine horizontalLine = plot.Add.HorizontalLine(item.Offset);
                    System.Windows.Media.Color color = CommonUtils.DarkenColor( item.LineColor,0.8);
                    horizontalLine.Color = new Color(color.R, color.G, color.B, color.A);
                    horizontalLine.LinePattern = LinePattern.Dotted;
                    horizontalLine.LineWidth=1;
                    records.Add(item, new(scatter,horizontalLine, xs, ys));
                    scatter.OffsetY = item.Offset;
                    scatter.Smooth = true;
                    scatter.Color = new Color(item.LineColor.R, item.LineColor.G, item.LineColor.B, item.LineColor.A);
                    scatter.LegendText = item.Name;
                    plot.ShowLegend();
                    forms.Refresh();
                    ProjectHelper.Saved = false;
                }
            }
            foreach (var item in records)
            {
                if (!temp.Contains(item.Key))
                {
                    records.Remove(item.Key);
                    plot.Remove(item.Value.scatter);
                    plot.Remove(item.Value.horizontalLine);
                    forms.Refresh();
                    ProjectHelper.Saved = false;
                }
            }
        }
        
        private void clearSeries(object sender, RoutedEventArgs e)
        {
            handleChanged(sender, (lb, ds) =>
            {
                ds.xs.Clear();
                ds.ys.Clear();
                
                forms.Refresh();
                ProjectHelper.Saved = false;
            });
        }

        private void nameChanged(object sender, RoutedEventArgs e)
        {
            handleChanged(sender, (lb, ds) =>
            {
                ds.scatter.LegendText = lb.Name;
                forms.Refresh();
                ProjectHelper.Saved = false;
            });
        }

        private void offsetChanged(object sender, RoutedEventArgs e)
        {
            handleChanged(sender, (lb, ds) =>
            {
                ds.scatter.OffsetY = lb.Offset;
                ds.horizontalLine.Y= lb.Offset;
                forms.Refresh();
                ProjectHelper.Saved = false;
            });
        }

        private void colorChanged(object sender, RoutedEventArgs e)
        {
            handleChanged(sender, (lb, ds) =>
            {
                System.Windows.Media.Color c = lb.LineColor;
                ds.scatter.Color = new Color(c.R, c.G, c.B, c.A);

                System.Windows.Media.Color color = CommonUtils.DarkenColor(c, 0.8);
                ds.horizontalLine.Color = new Color(color.R, color.G, color.B, color.A);
                forms.Refresh();
                ProjectHelper.Saved = false;
            });
        }
        
        private delegate void Func(OscilChannel lb, DisplayScatter ds);

        private void handleChanged(object sender, Func f)
        {
            if (sender is OscilChannel ls)
            {
                if (records.TryGetValue(ls, out DisplayScatter? value))
                {
                    f(ls, value);
                }
                else
                {
                    Gobals.logger?.error(LOG, "获取LabSeries失败");
                }
            }
            else
            {
                Gobals.logger?.error(LOG, "传入参数不是LabSeries");
            }
        }
        #endregion

        #region 4.事件监听

        //scottPlot的右键菜单
        private void DeployCustomMenu(object? sender, EventArgs? e)
        {
            MenuItem item;
            var cm = new ContextMenu();

            item = new() { Header = "缩放至合适" };
            item.Click += (o, e) => { forms.Plot.Axes.AutoScale(); forms.Refresh(); };
            cm.Items.Add(item);

            cm.Items.Add(new Separator());


            item = new() { Header = "删除" };
            item.Click += (o, e) => {
                mouseController.removeSelf();

            };
            cm.Items.Add(item);
            cm.IsOpen = true;
        }

        public void restore(object? obj)
        {
            OscilloscopeDataModel? t = null;
            string? s = obj?.ToString();
            if (s != null)
            {
                t = JsonConvert.DeserializeObject<OscilloscopeDataModel>(s);
            }

            if (t != null)
            {
                viewModel.restore(t);
                seriesChanged();
            }
            else
            {
                Gobals.logger?.error(LOG, "restore() 数据恢复错误");
            }
        }

        public SavedComponent save()
        {
            return new SavedComponent(this.GetType(), viewModel.save());
        }
        #endregion
    }
    public class OscilloscopeDataModel
    {
        public string name = "未命名示波器";
        public string YName = "Y";
        public bool Rolling = true;
        public double RollingTime = 1;
        public List<OscilChannel> series = new();
    }
}
