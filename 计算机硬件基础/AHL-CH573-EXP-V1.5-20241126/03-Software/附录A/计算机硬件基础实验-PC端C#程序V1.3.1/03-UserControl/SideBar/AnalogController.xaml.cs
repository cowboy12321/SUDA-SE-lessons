using GEC_LAB._04_Class.Models;
using System;
using System.Windows.Controls;

namespace GEC_LAB._03_UserControl
{
    /// <summary>
    /// AnalogController.xaml 的交互逻辑
    /// </summary>
    public partial class AnalogController : UserControl
    {
        public ExpPin pin;
        public delegate void AnalogControlF(short no, double value);
        public double Value { get => numberInput.Value; }
        AnalogControlF analog;
        public AnalogController(ExpPin pin, AnalogControlF analog)
        {
            InitializeComponent();
            this.pin = pin;
            pin.NameChanged += nameChanged;
            this.analog = analog;
            init();
        }

        private void nameChanged()
        {
            displayName.Content = pin;
        }

        private void init() { 
            numberInput.ValueChanged += NumberInput_ValueChanged;
            displayName.Content = pin;
        }
        private void NumberInput_ValueChanged(object sender)
        {
            double? v = numberInput.Value;
            if (v != null) analog(pin.no, (double)v);
        }
    }
}
