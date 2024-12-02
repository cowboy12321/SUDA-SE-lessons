using GEC_LAB._04_Class;
using GEC_LAB._04_Class.Interface;
using GEC_LAB._04_Class.Models;
using GEC_LAB._04_Class.MoveControl;
using LiveChartsCore.SkiaSharpView;
using Newtonsoft.Json;
using System;
using System.Windows.Controls;
using LiveChartsCore.Defaults;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Threading;
using System.Collections.ObjectModel;

namespace GEC_LAB._03_UserControl.Component
{
    /// <summary>
    /// LogicAnalyzer.xaml 的交互逻辑
    /// </summary>
    public partial class LogicAnalyzer : UserControl,ISavedComponent
    {
        MouseController mouseController;
        LogicAnalyzerDataModel data;
        private readonly string LOG = "LogicAnalyzer";
        private readonly int DataInterval = 5;
        private int channelIndex = 0;
        private Thread? tickThead;
        private double time = 0;
        private bool recording = false;
        private readonly int maxLimitMin = 10;
        private DictionaryHelper<int, LogicAnalyzerChannel> channelMap = new();
        private Axis[] XAxes = LogicAnalyzerChannel.GenerateXAxis();

        public LogicAnalyzer()
        {
            InitializeComponent();
            data = new();
            mouseController = new(this);
            mouseController.setMovable();
            Init();
        }
        private void Init()
        {
            this.Height = 450;
            Width =750;

  
            btnAdd.Click += (s, e) => { addChannel(channelIndex++, new());ProjectHelper.Saved = false; };
            btnStart.Click += RecordStart;
            btnStop.Click += RecordStop;
            btnReset.Click += Reset;

            addChannel(channelIndex++, new());
            addChannel(channelIndex++, new());
            addChannel(channelIndex++, new());
            addChannel(channelIndex++, new());
        }

        private void Reset(object sender, System.Windows.RoutedEventArgs e)
        {
            if (recording) RecordStop(this, new());
            foreach (var item in channelMap.Values)
            {
                if (item.points.Count!=0)
                {
                    item.points.Clear();
                    item.points.Add(new ObservablePoint(0, 0));
                    item.points.Add(new ObservablePoint(0.00001, 0));
                    ProjectHelper.Saved = false;
                }
                XAxes[0].MaxLimit = maxLimitMin;
            }
        }

        private void RecordStop(object sender, System.Windows.RoutedEventArgs e)
        {
            recording = false;
            foreach (var item in channelPanel.Children)
            {
                if (item is LogicAnalyzerChannel lac)
                {
                    lac.Unlock();
                }
            }
            btnStop.Visibility = System.Windows.Visibility.Collapsed;
            btnStart.Visibility = System.Windows.Visibility.Visible;
        }

        private void RecordStart(object sender, System.Windows.RoutedEventArgs e)
        {
            foreach (var item in channelMap.Values) item.points.Clear();
            recording = true;
            time = 0;
            tickThead = new Thread(() => {
                while (recording)
                {
                    tick();
                    Thread.Sleep(DataInterval);
                }
            });
            tickThead.Start();
            XAxes[0].MaxLimit = 10;
            XAxes[0].MinLimit = 0;

            //锁定图形组件，防止用户瞎改
            foreach (var item in channelPanel.Children)
            {
                if (item is LogicAnalyzerChannel lac) {
                    lac.Lock();
                } 
            }

            btnStart.Visibility = System.Windows.Visibility.Collapsed;
            btnStop.Visibility = System.Windows.Visibility.Visible;
        }
        #region 4.事件监听
        public SavedComponent save()
        {
            data.channels.Clear();
            foreach (var item in channelPanel.Children)
            {
                if(item is LogicAnalyzerChannel lac)
                {
                    data.channels.Add(new LogicAnalyzerDataModel.Channel()
                    {
                        channelName = lac.ChannelName,
                        ChannelColor = lac.ChannelColor,
                        formula = lac.ChannelFomula,
                        points = lac.points.Select(x => new GPoint(x.X==null?0:(double)x.X, x.Y == null ? 0 : (double)x.Y)).ToList()
                    });
                }
            }
            return new SavedComponent(this.GetType(), data);
        }
        public void restore(object? obj)
        {
            LogicAnalyzerDataModel? t = null;
            string? s = obj?.ToString();
            if (s != null)
            {
                t = JsonConvert.DeserializeObject<LogicAnalyzerDataModel>(s);
            }

            if (t != null)
            {
                data = t;
                refresh();
            }
            else
            {
                Gobals.logger?.error(LOG, "restore() 数据恢复错误");
            }
        }
        private void tick() {
            time += DataInterval*1.0/1000;
            foreach(var item in channelMap.Values)
            {

                string? s = item.ChannelFomula;
                bool clacRes = false;
                
                if ( s!=null&&s!="" && DataRefreshCenter.Instance.tryCalculateLogic(s,out bool res)) clacRes = res;
                lock (item.Sync)
                {
                    double v = clacRes ? 0.5 : -0.5;
                    if (item.points.Count>3&&item.points[item.points.Count-1].Y==v && item.points[item.points.Count - 2].Y==v)
                        item.points.RemoveAt(item.points.Count-1);
                    item.points.Add(new ObservablePoint(time, v));
                }
                if (time > maxLimitMin)
                {
                    XAxes[0].MaxLimit = time;
                    XAxes[0].MinLimit = time - maxLimitMin;
                }
            }
            ProjectHelper.Saved = false;
        }
        public void refresh()
        {
            channelPanel.Children.Clear();
            channelIndex = 0;
            time=0;
            foreach(var item in data.channels) addChannel(channelIndex++, item);
        }

        public void addChannel(int index, LogicAnalyzerDataModel.Channel channel)
        {
            var collection = new ObservableCollection<ObservablePoint>(channel.points.Select(p => new ObservablePoint(p.x, p.y - 0.5)));
            //controller
            LogicAnalyzerChannel c = new LogicAnalyzerChannel(XAxes, collection, channel);
            channelMap.put(index, c);
            c.ChanelIndex = index;
            c.Width = Width;
            c.ChannelDeleteEvent += channelDelete;
            channelPanel.Children.Add(c);
            strechHeight();
        }
        public void channelDelete(object? s,EventArgs args)
        {
            if (s != null && s is LogicAnalyzerChannel lac)
            {
                channelMap.Remove(lac.ChanelIndex);
                channelPanel.Children.Remove(lac);
                ProjectHelper.Saved = false;
                strechHeight();
            }
        }
        public void strechHeight()
        {
            if (channelPanel.Children.Count < 12)
            {
                Height = channelPanel.Children.Count*79 + 75;
            }
        }
        #endregion

        public class LogicAnalyzerDataModel
        {
            public List<Channel> channels = new();
            public class Channel
            {
                public string? channelName="channel";
                public Color ChannelColor=Colors.Pink;
                public string? formula;
                public List<GPoint> points=new();
            }
        }

        private void MyScrollViewer_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }
    }
}
