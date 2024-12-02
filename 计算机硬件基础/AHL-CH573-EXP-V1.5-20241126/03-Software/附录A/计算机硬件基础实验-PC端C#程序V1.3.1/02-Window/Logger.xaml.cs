using GEC_LAB._04_Class;
using ImTools;
using Microsoft.Win32;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace GEC_LAB._02_Window
{
    /// <summary>
    /// Logger.xaml 的交互逻辑
    /// </summary>
    public partial class Logger : Window 
    {
        #region 绑定以及变量定义
        enum LoggerLevel {
            DEBUG = 0,
            INFO = 1,
            WARN = 2,
            ERROR = 3
        }

        private Dictionary<int, SolidColorBrush> colorMap = new Dictionary<int, SolidColorBrush>() {
            {0,Brushes.Blue },
            {1,Brushes.Green },
            {2,Brushes.DarkGoldenrod },
            {3,Brushes.Red },
            {999,Brushes.DarkCyan }  };
        class LoggerItem {
            public DateTime time;
            public LoggerLevel level;
            public string threadName="";
            public string name="";
            public string msg= "";
        }

        private ConcurrentQueue<LoggerItem> items = new ConcurrentQueue<LoggerItem>();
        private Queue<Pair<LoggerItem,Paragraph>> displayItems = new Queue<Pair<LoggerItem, Paragraph>>();
        private LoggerLevel loggerLevel=LoggerLevel.INFO;
        private bool autoScroll=true;
        private int displaySize=50;
        private readonly int maxSize = 2000;
        private string datePattern = "yyyy-MM-dd HH:mm:ss.fff ";
        private bool saving=false;
        #endregion
        #region 初始化
        public Logger()
        {
            InitializeComponent();
            init();
        }
        private void init()
        {
            comboBox.ItemsSource = Enum.GetValues(typeof(LoggerLevel));
            comboBox.SelectedIndex = 1;
            comboBox.SelectionChanged += (s, e) => loggerLevel = (LoggerLevel)comboBox.SelectedIndex;
            displayScroll.ScrollChanged += (s, e) => {
                if (displayScroll.ScrollableHeight-4.99 <= displayScroll.VerticalOffset ) {
                    autoScroll = true;
                }
                else
                {
                    autoScroll = false;
                }
            };
            toTopBtn.Click += (s, e) => displayScroll.ScrollToTop();
            toEndBtn.Click += (s, e) =>displayScroll.ScrollToEnd();
            wrapBtn.Click += (s, e) => {
                if (displayScroll.HorizontalScrollBarVisibility == System.Windows.Controls.ScrollBarVisibility.Auto)
                {
                    displayScroll.HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Disabled;
                    display.MinWidth = 0;
                }
                else {
                    displayScroll.HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;
                    strechWidth();
                }
            };
            clearBtn.Click += (s, e) => clear();
            saveBtn.Click += (s, e) => save();

            display.Document.Blocks.Clear();

            comboBox.SelectionChanged += (s, e) =>
            {
                loggerLevel = (LoggerLevel)comboBox.SelectedIndex;
                refresh();
            };
            numberInput.Value = displaySize;
            numberInput.ValueChanged += (s, e) => {
                displaySize = (int)numberInput.Value;
                refresh();
            };
            Closing += (s, e) => {
                Hide();
                e.Cancel = true;
            };
            Gobals.logger = this;
        }

        #endregion
        #region 开放函数
        public void debug(string name, string msg) {

            add(LoggerLevel.DEBUG, name, msg,DateTime.Now);
        }
        public void info(string name, string msg)
        {
            add(LoggerLevel.INFO, name, msg, DateTime.Now);
        }
        public void warn(string name, string msg)
        {
            add(LoggerLevel.WARN, name, msg, DateTime.Now);
        }
        public void error(string name, string msg)
        {
            add(LoggerLevel.ERROR, name, msg, DateTime.Now);    
        }

        #endregion
        #region 内部函数
        
        private void save() {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog()
                {
                    InitialDirectory = "C:\\Users\\desktop",
                    FileName = "GEC_log-" + DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss"),
                    Filter="文本文件|*.txt",
                    Title = "另存为"
                };
                sfd.ShowDialog();
                if (sfd.FileName != null && sfd.FileName.Length > 0)
                {
                    List<string> list = new List<string>();
                    saving = true;
                    foreach (var item in items) {
                        list.Add(item.time.ToString(datePattern) +
                            item.level.ToString().PadLeft(5) + " [" +
                            item.threadName.PadLeft(threadNameSpace) + "] " +
                            item.name.PadRight(nameSpace) + " : " + item.msg);
                    };
                    saving = false;
                    File.WriteAllLines(sfd.FileName, list);
                };
            }
            catch (Exception ex)
            {
                Gobals.main?.gobalMessage("保存错误："+ex.Message);
            }
        }
        private void refresh()
        {
            clearDisplay();
            Stack<LoggerItem> stack = new Stack<LoggerItem>();
            foreach (var item in items)
            {
                if (item.level >= loggerLevel) stack.Push(item);
                if (stack.Count >= displaySize) break;

            }
            while (stack.Count > 0)
            {
                displayAdd(stack.Pop());
            }

        }
        private void strechWidth()
        {
            display.MinWidth = (threadNameSpace + nameSpace + msgSpace + 70) * 7;
        }
        private void add(LoggerLevel level,string name,string msg,DateTime time)
        {
            string? threadName = Thread.CurrentThread.Name;
            if (threadName == null) threadName = Thread.CurrentThread.ManagedThreadId.ToString();
            LoggerItem item = new LoggerItem() { time = time, level=level,threadName= threadName!=null? threadName:"",name=name,msg=msg };
            items.Enqueue(item);
            while (items.Count > maxSize && !saving ) items.TryDequeue(out LoggerItem? _);
            if (item.level >= loggerLevel) displayAdd(item);
        }
        private void displayAdd(LoggerItem item)
        {
            Dispatcher.Invoke(new Action(() => {
                {
                    Pair<LoggerItem, Paragraph> pair = new Pair<LoggerItem, Paragraph>(item, make(item));
                    if (displayItems.Count >= numberInput.Value)
                    {
                        display.Document.Blocks.Remove(displayItems.Dequeue().Second);
                    }
                    display.Document.Blocks.Add(pair.Second);
                    displayItems.Enqueue(pair);
                    strechWidth();
                    if (autoScroll) display.ScrollToEnd();
                }
            }));
        }
        private int threadNameSpace=0;
        private int nameSpace=0;
        private int msgSpace = 0;
        private void clear()
        {
            items.Clear();
            clearDisplay();
        }
        private void clearDisplay()
        {
            threadNameSpace = nameSpace = msgSpace = 0;
            displayItems.Clear();
            display.Document.Blocks.Clear();

        }
        private Paragraph make(LoggerItem item)
        {
            
            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(new Run(item.time.ToString(datePattern)) { Foreground = Brushes.Black });
            paragraph.Inlines.Add(new Run(item.level.ToString().PadLeft (5)) { Foreground = colorMap.GetValueOrDefault((int)item.level,Brushes.Green) });
            if (item.threadName.Length > threadNameSpace) threadNameSpace = item.threadName.Length;
            paragraph.Inlines.Add(new Run(" ["+ item.threadName.PadLeft(threadNameSpace) + "] ") { Foreground = Brushes.Black });
            if (item.name.Length > nameSpace ) nameSpace = item.name.Length;
            paragraph.Inlines.Add(new Run(item.name.PadRight(nameSpace) ){ Foreground = colorMap[999] });
            if(item.msg.Length>msgSpace ) msgSpace = item.msg.Length;
            paragraph.Inlines.Add(new Run(" : "+item.msg) { Foreground = Brushes.Black });
            return paragraph;
        }
        #endregion
    }
}
