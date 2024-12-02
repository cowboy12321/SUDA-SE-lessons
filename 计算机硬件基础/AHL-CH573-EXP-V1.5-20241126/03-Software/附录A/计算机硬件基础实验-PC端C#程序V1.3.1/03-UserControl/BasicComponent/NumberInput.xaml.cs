using GEC_LAB._04_Class;
using System;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace GEC_LAB._03_UserControl.BasicComponent
{
    /// <summary>
    /// NumberInput.xaml 的交互逻辑
    /// </summary>
    public partial class NumberInput : UserControl
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value", typeof(double), typeof(NumberInput),
                new FrameworkPropertyMetadata(
          defaultValue: 0.0,
          flags: FrameworkPropertyMetadataOptions.AffectsRender,
          propertyChangedCallback: new PropertyChangedCallback(OnPropertyChanged)));
        static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((NumberInput)d).innerValueChanged.Invoke();
        }
        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.Register(  
                "Interval", typeof(double), typeof(NumberInput),  new PropertyMetadata());

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(
                "Maximum", typeof(double), typeof(NumberInput),  new PropertyMetadata());

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(
                "Minimum", typeof(double), typeof(NumberInput), new PropertyMetadata());

        public static readonly DependencyProperty RoundDigitProperty =DependencyProperty.Register(
            "RoundDigit", typeof(int), typeof(NumberInput), new PropertyMetadata());


        public bool EnableUpDown
        {
            get { return (bool)GetValue(EnableUpDownProperty); }
            set { SetValue(EnableUpDownProperty, value); }
        }

        // Using a DependencyProperty as the backing store for enableUpDown.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableUpDownProperty =
            DependencyProperty.Register("enableUpDown", typeof(bool), typeof(NumberInput), new PropertyMetadata(true,new PropertyChangedCallback((s, e) => ((NumberInput)s).upDownBtnChange())));



        public double Value
        {
            get { return Math.Round((double)GetValue(ValueProperty),RoundDigit); }
            set { SetValue(ValueProperty, value);}
        }

        public double Interval
        {
            get { return (double)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public int RoundDigit
        {
            get { return (int)GetValue(RoundDigitProperty); }
            set { SetValue(RoundDigitProperty, value); }
        }
        public delegate void ValueChangedFunc(object sender);
        public event ValueChangedFunc? ValueChanged;
        private event Action innerValueChanged;
        private readonly int MouseInterval = 100;
        private bool pressFlag=false;
        public NumberInput()
        {
            InitializeComponent();
            this.DataContext = this;
            //upBtn.Click += (s, e) => Value += Interval;
            //downBtn.Click += (s, e) => Value -= Interval;
            upBtn.MouseLeftButtonDown += (s, e) => { 
                pressFlag = true;
                upBtn.CaptureMouse();
                new Thread(() => { while (pressFlag) {
                        Dispatcher.Invoke(new Action(() => Value += Interval));
                        Thread.Sleep(MouseInterval); } }).Start();
            };

            downBtn.MouseDown += (s, e) => { 
                pressFlag = true;
                downBtn.CaptureMouse();
                new Thread(() => { while (pressFlag)
                    {
                        Dispatcher.Invoke(new Action(() => Value -= Interval));
                        Thread.Sleep(MouseInterval); } }).Start(); 
            };

            upBtn.MouseUp += (s, e) => { pressFlag = false; upBtn.ReleaseMouseCapture(); };
            downBtn.MouseUp += (s, e) => { pressFlag = false; downBtn.ReleaseMouseCapture(); };

            innerValueChanged += () => { 
                if (Value > Maximum) Value = Maximum;
                if (Value < Minimum) Value = Minimum;
                if (!(Double.TryParse(numberText.Text, out double x) && Math.Abs(x - Math.Round(Value, RoundDigit)) < 0.00001))
                {
                    numberText.Text = Math.Round(Value, RoundDigit).ToString();
                    numberText.SelectionStart = numberText.Text.Length;
                }
                ValueChanged?.Invoke(this);
            };
            numberText.TextChanged += (s, e) =>
            {
                if (Double.TryParse(numberText.Text, out double result))
                {
                    Value = result;
                }
                else
                {
                    Gobals.logger?.error("NumberInput", "value error!");
                }

            };

            numberText.PreviewTextInput += (s, e) => {
                int start = numberText.SelectionStart;
                int len = numberText.SelectionLength;
                string txt = numberText.Text.Substring(0,start) + e.Text + numberText.Text.Substring(start+len);
                
                if(Double.TryParse(txt, out double result))
                {
                    e.Handled = result < Minimum || result > Maximum;
                }
                else
                {
                    e.Handled = true;
                }
            };

            DataObject.AddPastingHandler(numberText, onPaste);
        }
        private void onPaste(object sender, DataObjectPastingEventArgs e)
        {
            var isText = e.SourceDataObject.GetDataPresent(DataFormats.UnicodeText, true);
            if (!isText) return;

            var s = e.SourceDataObject.GetData(DataFormats.UnicodeText) as string;

            if (string.IsNullOrEmpty(s)) return;
            StringBuilder ss = new();
            foreach (var ch in s)
            {
                if (char.IsDigit(ch)) ss.Append(ch);
            }
            string t = numberText.Text;
            int start = numberText.SelectionStart;
            int len = numberText.SelectionLength;
            numberText.Text = $"{t.Substring(0, start)}{ss}{t.Substring(start + len)}";
            numberText.SelectionStart = start;
            e.CancelCommand();
        }
        public void upDownBtnChange() { 


        }
    }
}
