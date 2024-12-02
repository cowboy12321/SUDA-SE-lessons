using GEC_LAB._03_UserControl.BasicComponent;
using GEC_LAB._03_UserControl.Component;
using GEC_LAB._04_Class;
using GEC_LAB._04_Class.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GEC_LAB._02_Window
{
    /// <summary>
    /// SeqGeneratorEditor.xaml 的交互逻辑
    /// </summary>
    public partial class SeqGeneratorEditor : Window
    {
        public List<SeqGeneratorChannelDataModel> list=new ();
        CustomCompareList<ExpPin> pins = new CustomCompareList<ExpPin>(
                ProjectHelper.GetEnablePins()
                .Where(item => item is ExpPin p && p.pinMode == ExpPin.PinMode.DigitalOut)
                .Select(x => (ExpPin)x)
                .ToList<ExpPin>()
                , new MComparer());
        public SeqGeneratorEditor()
        {
            InitializeComponent();
            Init();
        }
        public SeqGeneratorEditor(Window? owner, List<SeqGeneratorChannelDataModel> list)
        {
            Owner = owner; 
            InitializeComponent();
            Init();

            list.ForEach(x => container.Children.Add(new SeqGeneratorEditorChannel(pins,x)));
        }

        public class MComparer : IComparer<ExpPin>
        {
            public int Compare(ExpPin? x, ExpPin? y)
            {
                if (x == null || y == null) return 0;
                if (x != null && y != null) return y.no - x.no;
                if (x == null) return 1;
                return -1;
            }

        }
        private void Init() {

            btnConfirm.Click += Button_Confirm;
            btnCanel.Click += Button_Cancel;
            btnAdd.Click += addChannel;
        }
        public new bool? ShowDialog() {
            Gobals.main?.pushWindow(this);
            return base.ShowDialog();
        }

        private void addChannel(object sender, RoutedEventArgs e)
        {
            container.Children.Add(new SeqGeneratorEditorChannel(pins));
        }

        private void Button_Confirm(object sender, RoutedEventArgs e)
        {
            list.Clear();
            HashSet<short> set = new HashSet<short>();
            foreach (var item in container.Children)
            {
                if(item is SeqGeneratorEditorChannel sge)
                {
                    if(sge.ChannelPin!=null )
                    {
                        if (set.Contains(sge.ChannelPin.no))
                        {
                            Gobals.main?.gobalMessage($"引脚 {sge.ChannelPin} 重复使用，请检查！");
                            return;
                        }else
                        {
                            set.Add(sge.ChannelPin.no);
                        }
                    }
                    list.Add(sge.DataModel);
                }
            }
            DialogResult = true;
        }
        private void Button_Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
