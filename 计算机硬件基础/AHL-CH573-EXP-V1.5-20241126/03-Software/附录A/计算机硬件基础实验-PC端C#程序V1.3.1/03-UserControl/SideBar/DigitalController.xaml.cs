using GEC_LAB._03_UserControl.BasicComponent;
using GEC_LAB._04_Class;
using GEC_LAB._04_Class.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GEC_LAB._03_UserControl
{
    /// <summary>
    /// DigitalController.xaml 的交互逻辑
    /// </summary>
    public partial class DigitalController : UserControl
    {
        public delegate void DigitalControlF(short no, bool value);
        public ExpPin pin;
        public bool Value { get => switcher.IsChecked == true; } 
        DigitalControlF digital;
        public DigitalController(ExpPin pin, DigitalControlF digital )
        {
            InitializeComponent();
            this.digital = digital;
            this.pin = pin;
            init();
        }

        private void init()
        {
            switcher.Checked += Checked; 
            switcher.Unchecked += Unchecked; 
            displayName.Content = pin.ToString();
            pin.NameChanged += nameChanged;
            Command.DigitalControled += changed;
        }

        private void nameChanged()
        {
            displayName.Content = pin.ToString();
        }

        private void Unchecked(object sender, RoutedEventArgs e)
        {
            digital(pin.no, false);
        }

        private void Checked(object sender, RoutedEventArgs e)
        {
            digital(pin.no, true);
        }

        private void changed(short no, double val)
        {
            if (no == pin.no)
            {
                Dispatcher.Invoke(() =>
                {
                    if (val != 0)
                    {
                        switcher.Checked -= Checked;
                        switcher.IsChecked = true;
                        switcher.Checked += Checked;
                    }
                    else
                    {
                        switcher.Unchecked -= Unchecked;
                        switcher.IsChecked = false;
                        switcher.Unchecked += Unchecked;

                    }
                });
            }
        }
    }
}
