using GEC_LAB._02_Window;
using GEC_LAB._04_Class;
using GEC_LAB._04_Class.Models;
using System.Windows;
using System.Windows.Controls;

namespace GEC_LAB._03_UserControl
{
    /// <summary>
    /// pageStep02.xaml 的交互逻辑
    /// </summary>
    public partial class PageStep02 : UserControl
    {
        private CheckBox[] analogInBoxs = new CheckBox[100];
        private CheckBox[] analogOutBoxs = new CheckBox[100];
        private CheckBox[] digitalInBoxs = new CheckBox[100];
        private CheckBox[] digitalOutBoxs = new CheckBox[100];
        private bool loading = false;// 状态标识，在loading时不要触发事件
        private readonly string LOG = "PageStep02";
        public PageStep02()
        {
            InitializeComponent();
            init();
        }
        public void init()
        {
            Gobals.logger?.info(LOG, "init");
            load();
            ProjectHelper.OnReload += load;
            ProjectHelper.McuReload += id => load();
            Gobals.logger?.info(LOG, "init done");
        }
        private void load()
        {
            loading = true;
            analogOutPanel.Children.Clear();
            analogInPanel.Children.Clear();
            digitalInPanel.Children.Clear();
            digitalOutPanel.Children.Clear();

            MCU? mcu = ProjectHelper.SelectedMcu;
            if (mcu != null)
            {
                McuHelper.getAnalogIn(mcu).ForEach(pin => analogInBoxs[pin.no] = addTo(analogInPanel, pin));
                McuHelper.getAnalogOut(mcu).ForEach(pin => analogOutBoxs[pin.no] = addTo(analogOutPanel, pin));
                McuHelper.getDigital(mcu).ForEach(pin => {
                    digitalOutBoxs[pin.no] = addTo(digitalOutPanel, pin);
                    digitalInBoxs[pin.no] = addTo(digitalInPanel, pin);
                });
            }
            ProjectHelper.GetPins().ForEach(pin => {
                if (analogInBoxs[pin.no] != null) check(pin, analogInBoxs[pin.no], ExpPin.PinMode.AnalogIn);
                if (analogOutBoxs[pin.no] != null) check(pin, analogOutBoxs[pin.no], ExpPin.PinMode.AnalogOut);
                if (digitalInBoxs[pin.no] != null) check(pin, digitalInBoxs[pin.no], ExpPin.PinMode.DigitalIn);
                if (digitalOutBoxs[pin.no] != null) check(pin, digitalOutBoxs[pin.no], ExpPin.PinMode.DigitalOut);
            });
            loading = false;
            ProjectHelper.PinStatusChangedEvent += no =>{
                ExpPin? pin = ProjectHelper.GetPin(no);
                if (pin != null)
                {
                    if (analogInBoxs[pin.no] != null) check(pin, analogInBoxs[pin.no], ExpPin.PinMode.AnalogIn);
                    if (analogOutBoxs[pin.no] != null) check(pin, analogOutBoxs[pin.no], ExpPin.PinMode.AnalogOut);
                    if (digitalInBoxs[pin.no] != null) check(pin, digitalInBoxs[pin.no], ExpPin.PinMode.DigitalIn);
                    if (digitalOutBoxs[pin.no] != null) check(pin, digitalOutBoxs[pin.no], ExpPin.PinMode.DigitalOut);
                }
            };
        }
        private void check(ExpPin pin,CheckBox box,ExpPin.PinMode targetPin)
        {
            box.Content = pin.Name;
            box.IsEnabled = !pin.enable || pin.pinMode == targetPin;
            box.IsChecked = pin.enable && pin.pinMode == targetPin;
        }
        private CheckBox addTo(Panel panel, ExpPin pin)
        {
            //右键菜单
            ContextMenu contextMenu = new ContextMenu();
            MenuItem newItem = new MenuItem();
            newItem.Header = "修改名称";
            newItem.Click += changeName_click; ;
            contextMenu.Items.Add(newItem);
            newItem.Tag = pin;

            //box
            CheckBox box = new CheckBox();
            box.Style = (Style)FindResource("pinController");
            box.Content = pin.Name;
            box.ContextMenu = contextMenu;
            box.Tag = pin;
            panel.Children.Add(box);
            box.Checked += Box_Checked;
            box.Unchecked += Box_Unchecked;
            return box;
        }

        private void Box_Unchecked(object sender, RoutedEventArgs e)
        {
            if (loading) return;
            CheckBox c = (CheckBox)sender;
            if (c.Tag is ExpPin)
            {
                ExpPin pin = (ExpPin)((Control)sender).Tag;
                ProjectHelper.ConfigPin(pin.no, pin.pinMode, false);
            }
        }

        private void Box_Checked(object sender, RoutedEventArgs e)
        {
            if (loading) return;
            CheckBox c = (CheckBox)sender;
            if (c.Tag is ExpPin)
            {
                ExpPin pin = (ExpPin)((Control)sender).Tag;

                ExpPin.PinMode pinMode = getModeByPanel(c);
                ProjectHelper.ConfigPin(pin.no, pinMode, true);
                pin.pinMode = pinMode;
            }
        }
        private ExpPin.PinMode getModeByPanel(CheckBox c)
        {
            if (c.Parent == digitalInPanel)
            {
                return ExpPin.PinMode.DigitalIn;
            }
            else if (c.Parent == digitalOutPanel)
            {
                return ExpPin.PinMode.DigitalOut;
            }
            else if (c.Parent == analogInPanel)
            {
                return ExpPin.PinMode.AnalogIn;
            }
            else if (c.Parent == analogOutPanel)
            {
                return ExpPin.PinMode.AnalogOut;
            }
            return ExpPin.PinMode.None;
        }

        private void changeName_click(object sender, RoutedEventArgs e)
        {
            Control c= (Control)sender;
            if (c.Tag is ExpPin)
            {
                ExpPin pin = (ExpPin)((Control)sender).Tag;
                ChangeName changeName = new ChangeName("修改名字", "为" + pin.ToString() + "改名", pin.Name);
                changeName.WindowStartupLocation = WindowStartupLocation.Manual;
                Point p=this.PointToScreen(new Point(0,40));
                changeName.Left = p.X;
                changeName.Top = p.Y;
                bool? v = changeName.ShowDialog();
                if (v != null && v == true)
                {
                    pin.Name = changeName.data;
                    if (analogInBoxs[pin.no] != null)
                    {
                        analogInBoxs[pin.no].Content = pin.Name;
                    }
                    if (analogOutBoxs[pin.no] != null)
                    {
                        analogOutBoxs[pin.no].Content = pin.Name;
                    }
                    if (digitalInBoxs[pin.no] != null && digitalOutBoxs[pin.no] != null)
                    {
                        digitalOutBoxs[pin.no].Content = digitalInBoxs[pin.no].Content = pin.Name;
                    }
                    ProjectHelper.ConfigPin(pin.no, pin.Name);
                }
            }
        }

    }
}
