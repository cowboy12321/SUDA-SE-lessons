using GEC_LAB._04_Class.Models;
using GEC_LAB._04_Class.MoveControl;
using GEC_LAB._04_Class;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using GEC_LAB._02_Window;
using GEC_LAB._04_Class.Interface;
using GEC_LAB._03_UserControl.BasicComponent;
using System.Threading;
using System;
using System.Linq.Dynamic.Core;
using System.Linq;
using ImTools;

namespace GEC_LAB._03_UserControl.Component
{
    /// <summary>
    /// SeqGenerator.xaml 的交互逻辑
    /// </summary>
    public partial class SeqGenerator : UserControl,ISavedComponent
    {
        MouseController mouseController;
        private readonly string LOG = "SeqGenerator";
        private static readonly string PLAY_ONCE = "单次播放";
        private static readonly string PLAY_LOOP = "循环播放";
        private static readonly int minAnimationInvertal = 20;

        private readonly string[] playStrings = { PLAY_ONCE, PLAY_LOOP};
        private bool playing = false;
        MenuItem editMenuItem = new MenuItem() { Header = "修改序列" };
        private SeqGeneratorDataModel dataModel = new() {
            channels = new(){
                new SeqGeneratorChannelDataModel() { channelName = "通道1" },
                new SeqGeneratorChannelDataModel() { channelName = "通道2" },
                new SeqGeneratorChannelDataModel() { channelName = "通道3" },
                new SeqGeneratorChannelDataModel() { channelName = "通道4" },

            },
            interval = 500
        };
        public SeqGenerator()
        {
            InitializeComponent();
            mouseController = new MouseController(this);
            mouseController.setMovable();
            init();
        }
        #region  初始化
        private void init()
        {
            Height = 280;
            Width = 600;
            ContextMenu.Items.Clear();
            editMenuItem.Click += edit;
            ContextMenu.Items.Add(editMenuItem);
            MenuItem item = new MenuItem() { Header = "删除" };
            item.Click +=(s,e)=>mouseController.removeSelf();
            ContextMenu.Items.Add(item);
            btnEdit.Click += edit;
            btnStart.Click += start;
            btnStop.Click += stop;
            intervalNumber.ValueChanged += valueChanged;
            foreach(string s in playStrings)playerModeBox.Items.Add(s);
            refresh();
        }

        private void valueChanged(object sender, Panuon.WPF.SelectedValueChangedRoutedEventArgs<double?> e)
        {
            if (intervalNumber.Value != null)
            {
                dataModel.interval = (int)intervalNumber.Value;
                if (dataModel.interval < minAnimationInvertal)
                {
                    Gobals.main?.gobalToast("数据间隔小于"+ minAnimationInvertal+"ms时停用播放动画");
                }
            }
            
        }



        #endregion

        #region 3.内部函数
        private void enableEdit() {
            playerModeBox.IsEnabled = true;
            btnEdit.IsEnabled = true;
            editMenuItem.IsEnabled = true;
            intervalNumber.IsEnabled = true;
        }
        private void disableEdit()
        {
            playerModeBox.IsEnabled = false;
            btnEdit.IsEnabled = false;
            editMenuItem.IsEnabled = false;
            intervalNumber.IsEnabled = false;
        }
        #endregion

        #region 4.事件监听

        public void restore(object? obj)
        {
            SeqGeneratorDataModel? t = null;
            string? s = obj?.ToString();
            if (s != null)
            {
                t = JsonConvert.DeserializeObject<SeqGeneratorDataModel>(s);
            }

            if (t != null)
            {
                dataModel = t;
                refresh();
            }
            else
            {
                Gobals.logger?.error(LOG, "restore() 数据恢复错误");
            }
        }

        public SavedComponent save()
        {
            return new SavedComponent(GetType(), dataModel);
        }
        private void refresh()
        {
            btnStart.Visibility= Visibility.Visible;
            btnStop.Visibility= Visibility.Collapsed;
            channelContainer.Children.Clear();
            dataModel.channels.ForEach(c => channelContainer.Children.Add(new SeqGeneratorChannel(c)));
            intervalNumber.Value = dataModel.interval;
            playerModeBox.SelectedIndex = 0;
            enableEdit();
            strechHeight();
        }
        private void reset()
        {
            btnStart.Visibility = Visibility.Visible;
            btnStop.Visibility = Visibility.Collapsed;
            playing = false;
            foreach (var item in channelContainer.Children)
            {
                if(item is SeqGeneratorChannel sgc)
                {
                    sgc.Reset();
                }
            };
            enableEdit();
        }
        private void start(object sender, RoutedEventArgs e)
        {
            reset();
            playing = true;
            btnStart.Visibility = Visibility.Collapsed;
            btnStop.Visibility = Visibility.Visible;
            disableEdit();
            string? s = playerModeBox.SelectedItem.ToString();
            if (dataModel.interval < minAnimationInvertal)
            {
                Gobals.main?.gobalToast("数据间隔小于" + minAnimationInvertal + "ms时停用播放动画");
            }
            if (s!=null)
                new Thread(()=>thread_tick(s)).Start();
            else
                new Thread(() => thread_tick(PLAY_ONCE)).Start();
        }
        private void stop(object sender, RoutedEventArgs e)
        {
            playing = false;
            btnStart.Visibility = Visibility.Visible;
            btnStop.Visibility = Visibility.Collapsed;
            enableEdit();
        }
        private void edit(object sender, RoutedEventArgs e)
        {
            SeqGeneratorEditor editor= new SeqGeneratorEditor(Gobals.main,dataModel.channels);
            if (editor.ShowDialog() == true)
            {
                dataModel.channels = editor.list;
                ProjectHelper.Saved = false;
                refresh();
            }
        }
        public void strechHeight()
        {
            if (channelContainer.Children.Count > 4)
            {
                this.Height = channelContainer.Children.Count * 47 + 75;
            }
            else
            {
                this.Height = 280;
            }
        }

        #endregion

        #region 5.定时处理     
        private void resetAllChannel()
        {
            Dispatcher.Invoke(() =>
            {
                foreach (var item in channelContainer.Children)
                {
                    if (item is SeqGeneratorChannel sgc)
                    {
                        sgc.Reset();
                    }
                }
            });
        }
        private bool play_once(SeqGeneratorChannel.AheadFunction[] fs)
        {
            Command cmd = new Command();
            bool p = false;
            foreach(var f in fs)
            {
                p|=f(cmd);
            }
            if (p) cmd.sendWithoutLog();

            return p;
        }
        private void thread_tick(string play_mode)
        {
            resetAllChannel();
            SeqGeneratorChannel.AheadFunction[] fs;
            if (dataModel.interval < minAnimationInvertal)
            {
                fs = Dispatcher.Invoke(() => channelContainer.Children
                    .ToDynamicList()
                    .Where(i => i is SeqGeneratorChannel)
                    .Select(i => (SeqGeneratorChannel.AheadFunction)((SeqGeneratorChannel)i).AheadWithoutAnimation)
                    .ToArray()
                    );
            }
            else
            {
                fs = Dispatcher.Invoke(() => channelContainer.Children
                    .ToDynamicList()
                    .Where(i => i is SeqGeneratorChannel)
                    .Select(i => (SeqGeneratorChannel.AheadFunction)((SeqGeneratorChannel)i).Ahead)
                    .ToArray()
                    );
            }
            while (playing)
            {
                DateTime now= DateTime.Now;
                Dispatcher.Invoke(() =>
                {
                    if (!play_once(fs))
                    { 
                        if (play_mode == PLAY_ONCE)
                        {
                            reset();
                        }
                        else
                        {
                            foreach (var item in channelContainer.Children)
                            {
                                if (item is SeqGeneratorChannel sgc)
                                {
                                    sgc.ResetSeq();
                                }
                            }
                            if (!play_once(fs))
                            {
                                Gobals.main?.gobalToast("通道数据错误！");
                                reset();
                            }
                        }
                    }
                });

                DateTime now1 = DateTime.Now;
                Thread.Sleep(new TimeSpan(Math.Max(10000 * dataModel.interval - (now1 - now).Ticks -166, 1000)));
            }
        }
        #endregion
    }
    public class SeqGeneratorDataModel
    {
        public List<SeqGeneratorChannelDataModel> channels = new List<SeqGeneratorChannelDataModel>();
        public int interval = 500;
    }
    public class SeqGeneratorChannelDataModel
    {
        public string channelName="";
        public string channelSeq="";
        public ExpPin? pin;
    }
}
