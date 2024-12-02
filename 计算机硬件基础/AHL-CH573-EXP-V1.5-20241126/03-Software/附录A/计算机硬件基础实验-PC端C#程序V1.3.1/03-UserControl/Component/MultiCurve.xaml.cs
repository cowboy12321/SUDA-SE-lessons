using GEC_LAB._04_Class;
using GEC_LAB._04_Class.Interface;
using GEC_LAB._04_Class.Models;
using GEC_LAB._04_Class.MoveControl;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GEC_LAB._03_UserControl.Component
{
    /// <summary>
    /// MultiCurve.xaml 的交互逻辑
    /// </summary>
    public partial class MultiCurve : UserControl, ISavedComponent
    {
        MouseController mouseController;
        MultiCurveViewModel viewModel;
        private readonly string LOG = "MultiCurve";
        public MultiCurve()
        {
            InitializeComponent();
            viewModel = new();
            MultiCurveAttachment attach = new MultiCurveAttachment();
            attach.DataContext = viewModel;
            this.DataContext = viewModel;
            mouseController = new MouseController(this, attach);
            mouseController.setMovable();
            init();
        }
        private Dictionary<LabSeries, DisplayScatter> records=new ();
        private class DisplayScatter
        {
            public ScottPlot.Plottables.Scatter scatter;
            public double lastX, lastY;
            public List<double> xs;
            public List<double> ys;
            public DisplayScatter(ScottPlot.Plottables.Scatter scatter, List<double> xs, List<double> ys)
            {
                this.scatter = scatter;
                this.xs = xs;
                this.ys = ys;
            }
        }
        #region  初始化
        private void init()
        {
            ScottPlot.Plot plot = forms.Plot;
            forms.Plot.Font.Set("微软雅黑");
            this.Height = 400;
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
            forms.PreviewMouseDown += (s, e) =>
            {
                mouseController.topping();
                mouseController.showAttach();
            };
            ScottPlot.Control.Interaction inter = new ScottPlot.Control.Interaction(forms)
            {
                Inputs = customInputBindings
            };
            forms.Menu?.Clear();
            inter.Actions.ShowContextMenu += (s, e) => { DeployCustomMenu(null, null); };
            forms.Interaction = inter;
            btnReset.Click += (s, e) => ResetChart();

            DataRefreshCenter.Instance.ComponentTickEvent += timer_tick;
            viewModel.seriesListChangedEvent += (s, e) => seriesChanged();
            viewModel.clearSeriesEvent += clearSeries;
            viewModel.colorChangedEvent += colorChanged;
            viewModel.nameChangedEvent += nameChanged;
        }

        #endregion

        #region 3.内部函数
        private void seriesChanged()
        {
            ScottPlot.Plot plot = forms.Plot;
            Collection<LabSeries> temp = new Collection<LabSeries>();
            foreach (var item in viewModel.SeriesList)
            {
                temp.Add(item);
                if (!records.ContainsKey(item))
                {
                    List<double> xs = item.points.Select(p => p.x).ToList();
                    List<double> ys = item.points.Select(p => p.y).ToList();
                    ScottPlot.Plottables.Scatter scatter = plot.Add.ScatterLine(xs,ys);
                    records.Add(item, new (scatter,xs,ys));
                    if (viewModel.AutoScale) plot.Axes.AutoScale();
                    scatter.Smooth = true;
                    scatter.Color = new ScottPlot.Color(item.LineColor.R, item.LineColor.G, item.LineColor.B, item.LineColor.A);
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
                ds.lastX = 0;
                ds.lastY = 0;
                forms.Refresh();
                ProjectHelper.Saved = false;
            });
        }
        
        private void nameChanged(object sender, RoutedEventArgs e)
        {
            handleChanged(sender, (lb, ds) =>
            {
                ds.scatter.LegendText=lb.Name;
                forms.Refresh();
                ProjectHelper.Saved = false;
            });
        }

        private void colorChanged(object sender, RoutedEventArgs e)
        {
            handleChanged(sender, (lb, ds) =>
            {
                 System.Windows.Media.Color c = lb.LineColor;
                ds.scatter.Color = new ScottPlot.Color(c.R, c.G, c.B, c.A);
                forms.Refresh();
                ProjectHelper.Saved = false;
            });
        }
        private delegate void Func(LabSeries lb, DisplayScatter ds);
        private void  handleChanged(object sender,Func f)
        {
            if (sender is LabSeries ls)
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
        public void ResetChart()
        {
            ScottPlot.Plot plot = forms.Plot;
            foreach(var ds in records.Values)
            {
                ds.xs.Clear();
                ds.ys.Clear();
                ds.lastX = ds.lastY = 0;
                ProjectHelper.Saved = false;
            }
            foreach (var ls in records.Keys)
            {
                ls.points.Clear();
                ProjectHelper.Saved = false;
            }
            forms.Refresh();
        }
        #endregion

        #region 4.事件监听
        public void restore(object? obj)
        {
            MultiCurveDataModel? t = null;
            string? s = obj?.ToString();
            if (s != null)
            {
                t = JsonConvert.DeserializeObject<MultiCurveDataModel>(s);
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

        #endregion

        #region 5.定时处理        
        ScottPlot.Plottables.Scatter? lastPoint = null;

        private void addPoint(DisplayScatter ds, double x, double y)
        {
            ScottPlot.Plot plot = forms.Plot;

            ds.xs.Add(x); ds.ys.Add(y);
            ds.lastX = x; ds.lastY = y;
            if (lastPoint != null) plot.Remove(lastPoint);
            lastPoint = plot.Add.ScatterPoints(new double[] { x }, new double[] { y });
            if(viewModel.AutoScale) plot.Axes.AutoScale();
            ProjectHelper.Saved = false;
            forms.Refresh();
        }
        public void timer_tick()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                int? i = viewModel.SelectSeriesIndex;
                if (i!=null&&i >= 0 &&i < viewModel.SeriesList.Count)
                {
                    double x, y;
                    LabSeries series = viewModel.SeriesList[(int)i];
                    if(records.TryGetValue(series,out DisplayScatter? ds))
                    {
                        if (ds!=null&&
                            DataRefreshCenter.GetInstance().tryCalculate(series.FormulaX, out x) &&
                            DataRefreshCenter.GetInstance().tryCalculate(series.FormulaY, out y))
                        {
                            if (Math.Abs(x - ds.lastX) > 0.0499 || Math.Abs(ds.lastY - y) > 0.0499)
                            {
                                addPoint(ds,x, y);
                                series.points.Add(new GPoint(x, y));
                            }
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
        #endregion
    }
    public class MultiCurveDataModel
    {
        public string name = "未命名曲线图";
        public string XName = "X";
        public string YName = "Y";
        public bool autoScale = true;
        public int? selectIndex = 0;
        public List<LabSeries> series = new List<LabSeries>();

    }
}
