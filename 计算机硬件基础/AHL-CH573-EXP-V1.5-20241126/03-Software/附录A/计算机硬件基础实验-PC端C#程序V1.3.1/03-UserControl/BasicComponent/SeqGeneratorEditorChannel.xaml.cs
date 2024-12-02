using GEC_LAB._03_UserControl.Component;
using GEC_LAB._04_Class;
using GEC_LAB._04_Class.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GEC_LAB._03_UserControl.BasicComponent
{
    /// <summary>
    /// SeqGeneratorChannel.xaml 的交互逻辑
    /// </summary>
    public partial class SeqGeneratorEditorChannel : UserControl
    {
        public string ChannelName
        {
            get => txtName.Text;
            set => txtName.Text = value;
        }

        public string ChannelSeq
        {
            get => txtSeq.Text;
            set => txtSeq.Text = value;
        }

        private CustomCompareList<ExpPin> pins;
        private ExpPin? pin;
        public ExpPin? ChannelPin
        {
            get => comboPin.SelectedIndex==-1? null:pins[comboPin.SelectedIndex];
            set
            {
                if (value == null || !pins.Contains(value)) comboPin.SelectedIndex = -1;
                else {
                    pin=value;
                    comboPin.SelectedIndex = comboPin.SelectedIndex=pins.IndexOf(value);
                }
            }
        }

        public SeqGeneratorChannelDataModel DataModel
        {
            get =>new SeqGeneratorChannelDataModel() { 
                channelName = ChannelName, 
                channelSeq = ChannelSeq, 
                pin = ChannelPin 
            };
        }

        private List<Control> controls = new();
        private Dictionary<Control, int> ids =new();
        private void Init() {

            txtSeq.PreviewTextInput += (s, e) =>
            {
                foreach(char ch in e.Text)
                {
                    if(ch != '0' && ch != '1')
                    {
                        e.Handled = true;
                        return;
                    }
                }
            };
            DataObject.AddPastingHandler(txtSeq, onPaste);


            btnDelete.Click += delete;
            controls.Add(txtName);
            controls.Add(txtSeq);
            controls.Add(comboPin);
            controls.Add(btnDelete);
            ids.Add(txtName, 0);
            ids.Add(txtSeq, 1);
            ids.Add(comboPin, 2);
            ids.Add(btnDelete, 3);
            txtName.PreviewKeyDown += ArrowDown;
            txtSeq.PreviewKeyDown += ArrowDown;
            comboPin.PreviewKeyDown += ArrowDown;
            btnDelete.PreviewKeyDown += ArrowDown;
        }

        private void ArrowDown(object sender, KeyEventArgs e)
        {
            int op;
            int u,v;
            switch (e.Key)
            {
                case Key.Left:
                    op = -1;
                    goto left_right;
                case Key.Right:
                    op = 1;
                left_right:
                    if(sender is TextBox t) {
                        if ((t.SelectionStart != 0 || op > 0) && (t.SelectionStart != t.Text.Length || op<0)) return;
                    }
                    u = ids[(Control)sender];
                    v = u + op;
                    if (v >= controls.Count) v = 0;
                    else if (v < 0) v = controls.Count - 1;
                    controls[v].Focus();
                    e.Handled = true;
                    if (controls[v] is TextBox t2)
                    {
                        if(op>0) t2.SelectionStart = 0;
                        else t2.SelectionStart = t2.Text.Length;
                    }
                    break;
                case Key.Up:
                op = -1;
                goto up_down;
                case Key.Down:
                op = 1;
                up_down:
                    if(sender is ComboBox c)
                    {
                        if(!((c.SelectedIndex + op<0)||(c.SelectedIndex + op==c.Items.Count))) return;
                    }
                if (Parent is Panel p)
                {
                    v = p.Children.IndexOf(this)+op;
                    if (v < 0) v = p.Children.Count - 1;
                    else if(v>=p.Children.Count) v = 0;
                        int start=0;
                        if(sender is TextBox)
                        {
                            start=((TextBox)sender).SelectionStart;
                        }
                    ((SeqGeneratorEditorChannel)p.Children[v]).locate(ids.GetValueOrDefault((Control)sender, 0), start);
                }
                e.Handled = true;
                break;

            }
        }
        public void locate(int index, int start)
        {
            if(index>=0 && index < controls.Count())
            {
                controls[index].Focus();
                if (controls[index] is TextBox t)
                {
                    t.SelectionStart= start;
                }
            }
        }

        private void onPaste(object sender, DataObjectPastingEventArgs e)
        {
            var isText = e.SourceDataObject.GetDataPresent(DataFormats.UnicodeText, true);
            if (!isText) return;

            var s = e.SourceDataObject.GetData(DataFormats.UnicodeText) as string;

            if (string.IsNullOrEmpty(s)) return;
            StringBuilder ss=new();
            foreach (var ch in s)
            {
                if (ch == '0' || ch == '1') ss.Append(ch);
            }
            string t = txtSeq.Text;
            int start = txtSeq.SelectionStart;
            int len = txtSeq.SelectionLength;
            txtSeq.Text = $"{t.Substring(0,start)}{ss}{t.Substring(start+len)}";
            txtSeq.SelectionStart = start;
            e.CancelCommand();
        }

        private void delete(object sender, RoutedEventArgs e)
        {
            ((Panel)Parent).Children.Remove(this);
        }
        public SeqGeneratorEditorChannel(CustomCompareList<ExpPin> expPins,SeqGeneratorChannelDataModel? model=null)
        {
            InitializeComponent();
            pins = expPins;
            pins.ForEach(x =>
            {
                Button btn = new();
                btn.Content = x.ToString();
                btn.Style = (Style)FindResource("ExpressEditorItem");
                btn.Click += (s, e) => ChannelPin = x;
                comboPin.Items.Add(x.ToString());
            });
            if (model != null)
            {
                ChannelName = model.channelName; 
                ChannelSeq = model.channelSeq; 
                ChannelPin = model.pin;
            }
            ChannelPin = pin;
            Init();
        }
    }
}
