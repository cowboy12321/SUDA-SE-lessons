using GEC_LAB._03_UserControl.Component.ViewModels;
using GEC_LAB._04_Class;
using GEC_LAB._04_Class.Interface;
using GEC_LAB._04_Class.Models;
using GEC_LAB._04_Class.MoveControl;
using Newtonsoft.Json;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
namespace GEC_LAB._03_UserControl.Component
{
    /// <summary>
    /// Curve.xaml 的交互逻辑
    /// </summary>
    public partial class Curve : UserControl, ISavedComponent
    {
        public CurveDataModel curveModel;
        private CurveViewModel ViewModel { get => (CurveViewModel)attach.DataContext; }
        CurveAttachment attach;
        private double lastX, lastY;
        //代表记录了多少个点
        private int dataCount = 0;
        public MouseEventHandler? mouseEvent;
        MouseController mouseController;
        List<double> xs = new() { };
        List<double> ys = new() { };
        ScottPlot.Plottables.Scatter? mainScatter = null;

        #region 2.初始化和析构
        public Curve()
        {
            Gobals.logger?.info("Curve", "init");
            InitializeComponent();
            curveModel = new CurveDataModel();
            mouseController = new MouseController(this, attach = new CurveAttachment(curveModel, attachDataChanged, attachColorChanged));
            attach.MaxChangeHandler += MaxChanged;
            DataRefreshCenter.Instance.ComponentTickEvent+=timer_tick;
            init();
            Gobals.logger?.info("Curve", "done");
        }
        private void init()
        {
            Plot plot = forms.Plot;
            mouseController.setMovable();
            Height = 400;
            Width = 600;
            forms.MouseDown += (sender, e) => e.Handled = true;
            forms.MouseUp += (s, e) => e.Handled= true;
            
            ScottPlot.Control.InputBindings customInputBindings = new() {
                DragPanButton = ScottPlot.Control.MouseButton.Left,
                ZoomInWheelDirection = ScottPlot.Control.MouseWheelDirection.Up,
                DragZoomButton = ScottPlot.Control.MouseButton.Right,
                ZoomOutWheelDirection = ScottPlot.Control.MouseWheelDirection.Down,
                ClickContextMenuButton = ScottPlot.Control.MouseButton.Right
            };
            ScottPlot.Control.Interaction inter = new ScottPlot.Control.Interaction(forms) { 
                Inputs = customInputBindings
            };
            forms.Menu?.Clear();
            forms.PreviewMouseDown += (s, e) =>
            {
                mouseController.topping();
                mouseController.showAttach();
            };
            inter.Actions.ShowContextMenu += (s, e) => { DeployCustomMenu(null,null); };
            forms.Interaction = inter;
            btnReset.Click += (s, e) => ResetChart();
            refreshChart();
        }
        #endregion

        #region 3.函数

        ScottPlot.Plottables.Scatter? lastPoint = null;

        public void addPoint(double x, double y)
        {
            Plot plot = forms.Plot;
            
            xs.Add(x); ys.Add(y);
            lastX = x; lastY = y;
            ++dataCount;
            lblHint.Content = "绘制数据中:" + dataCount;
            if (lastPoint != null) plot.Remove(lastPoint);
            lastPoint = plot.Add.ScatterPoints(new double[] { x }, new double[] { y });
            if (ViewModel.AutoScale) plot.Axes.AutoScale();
            forms.Refresh();
        }

        public void ResetChart()
        {
            Plot plot = forms.Plot;
            plot.Clear();
            if (curveModel.points.Count != 0)
            {
                ProjectHelper.Saved = false;
                curveModel.points.Clear();
            }
            lastX = lastY = 0;
            xs.Clear(); ys.Clear(); 
            resetMainSactter();
        }
        
        public void refreshChart()
        {
            Plot plot = forms.Plot;
            plot.Clear();
            xs = curveModel.points.Select(x => x.x).ToList();
            ys = curveModel.points.Select(y => y.y).ToList();
            resetMainSactter();
        }
        private void resetMainSactter()
        {
            xsLbl.Content = curveModel.XName;
            ysLbl.Content = curveModel.YName;
            titleLbl.Content = curveModel.name;
            lblHint.Content = "";
            dataCount = 0;

            Plot plot = forms.Plot;
            if (mainScatter != null) plot.Remove(mainScatter);
            if (lastPoint != null) plot.Remove(lastPoint);
            lastX = lastY = 0;
            lastPoint = null;
            mainScatter = plot.Add.ScatterLine(xs, ys);
            if (ViewModel.AutoScale) plot.Axes.AutoScale();
            else plot.Axes.SetLimits(0,ViewModel.XMax,0,ViewModel.YMax);
            mainScatter.Smooth = true;
            mainScatter.Color = new(curveModel.color.R, curveModel.color.G, curveModel.color.B, curveModel.color.A);
            forms.Refresh();
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
            item.Click += (o, e) => { mouseController.removeSelf(); };
            cm.Items.Add(item);
            cm.IsOpen = true;
        }

        private void MaxChanged() {
            if (!ViewModel.AutoScale)
            {
                forms.Plot.Axes.SetLimits(0, ViewModel.XMax, 0, ViewModel.YMax);
                forms.Refresh();
            }
        }

        private void mouseClick(object sender, MouseEventArgs e)
        {
            mouseEvent?.Invoke(this, e);
        }

        public SavedComponent save()
        {
            curveModel.autoScale = ViewModel.AutoScale;
            curveModel.xMax = ViewModel.XMax;curveModel.yMax = ViewModel.YMax;  
            return new SavedComponent(GetType(), curveModel);
        }

        public void restore(object? data)
        {
            string? s = data?.ToString();
            if (s != null)
            {
                CurveDataModel? t = JsonConvert.DeserializeObject<CurveDataModel>(s);
                if (t!=null)
                {
                    curveModel = t;
                }
            }
            refreshChart();

            
            mouseController.setAttach(attach = new CurveAttachment(curveModel,attachDataChanged,attachColorChanged)); 
            attach.MaxChangeHandler += MaxChanged;
            ViewModel.AutoScale = curveModel.autoScale;
            ViewModel.XMax = curveModel.xMax;
            ViewModel.YMax = curveModel.yMax;
        }

        public void attachDataChanged()
        {
            ScottPlot.Plot plot = forms.Plot;
            xsLbl.Content=curveModel.XName;
            ysLbl.Content = curveModel.YName;
            titleLbl.Content = curveModel.name;
            if (ViewModel.AutoScale) plot.Axes.AutoScale();
            forms.Refresh();
        }
        
        private void attachColorChanged()
        {
            ScottPlot.Plot plot = forms.Plot;
            if(mainScatter!=null)mainScatter.Color = new(curveModel.color.R, curveModel.color.G, curveModel.color.B, curveModel.color.A);
            forms.Refresh();
        }
        #endregion

        #region 5.定时处理
        public void timer_tick() {
            Dispatcher.Invoke(new Action(() =>
            {
                double x, y;
                if (DataRefreshCenter.GetInstance().tryCalculate(curveModel.formulaX, out x) &&
                    DataRefreshCenter.GetInstance().tryCalculate(curveModel.formulaY, out y))
                {
                    if (Math.Abs(x - lastX) > 0.0999 || Math.Abs(lastY - y) > 0.0999)
                    {
                        addPoint(x, y);
                        curveModel.points.Add(new GPoint(x, y));
                        lastY = y;
                        lastX = x;
                    }
                }
            }));
        }
        #endregion

    }
    public class CurveDataModel {
        public string name = "未命名曲线图";
        public string formulaX = "";
        public string formulaY = "";
        public string XName = "X";
        public string YName = "Y";
        public bool autoScale = true;
        public double xMax = 0;
        public double yMax = 0; 
        public System.Windows.Media.Color color= System.Windows.Media.Color.FromRgb(0,0,0);
        public List<GPoint> points=new List<GPoint>();
        public CurveDataModel() { }
        public CurveDataModel(CurveDataModel curve)
        {
            copy(curve);
        }
        public void copy(CurveDataModel curve)
        {
            name = curve.name;
            formulaX = curve.formulaX;
            formulaY = curve.formulaY;
            autoScale = curve.autoScale;
            XName = curve.XName;
            YName = curve.YName;
            xMax = curve.xMax;
            yMax = curve.yMax;
        }
    }
}
