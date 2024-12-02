using GEC_LAB._03_UserControl.Component;
using GEC_LAB._04_Class;
using GEC_LAB._04_Class.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GEC_LAB._03_UserControl.BasicComponent
{
    /// <summary>
    /// SeqGeneratorChannel.xaml 的交互逻辑
    /// </summary>
    public partial class SeqGeneratorChannel : UserControl
    {
        public string ChannelName
        {
            get => txtName.Text;
            set=> txtName.Text = value;
        }

        public string channelSeq="";
        public string ChannelSeq
        {
            get => channelSeq;
            set => channelSeq = value;
        }

        private ExpPin? pin;
        public ExpPin? ChannelPin
        {
            get => pin;
            set
            {
                pin = value; 
                txtPin.Text = pin?.ToString();
            }
        }
        public delegate bool AheadFunction(Command cmd);

        public SeqGeneratorChannel(SeqGeneratorChannelDataModel? model=null)
        {
            InitializeComponent();
            if(model!= null)
            {
                ChannelPin=model.pin; ChannelName=model.channelName; ChannelSeq=model.channelSeq;
            }
            Reset();
        }

        public void Reset() {
            imgBefore.Source = ImgHelper.BitmapToBitmapImage(Properties.Resources.digital0);
            ResetSeq();
        }
        public void ResetSeq()
        {
            withoutAnimationPoint = 0;
            parseChannelSeq();
        }
        public bool Ahead() { 
            Command cmd= new Command();
            if(!Ahead(cmd)) return false;
            cmd.send();
            return true;
        }
        private void parseChannelSeq()
        {
            imgQueue.Children.Clear();
            foreach (char ch in ChannelSeq)
            {
                Image image = new Image();
                if (ch == '1')
                {
                    image.Source = ImgHelper.BitmapToBitmapImage(Properties.Resources.digital1);
                    image.Tag = 1;
                }
                else
                {
                    image.Source = ImgHelper.BitmapToBitmapImage(Properties.Resources.digital0);
                    image.Tag = 0;
                }
                image.Width = 35;
                imgQueue.Children.Add(image);
            }
        }
        
        public bool Ahead(Command cmd)
        {
            if (imgQueue.Children.Count == 0) return false;
            Image c = (Image)imgQueue.Children[0];
            if (c.Tag is int b && b == 1)
            {
                imgBefore.Source = ImgHelper.BitmapToBitmapImage(Properties.Resources.digital1);
                if (pin != null) cmd.digitalControl(pin.no, 1);
            }
            else
            {
                imgBefore.Source = ImgHelper.BitmapToBitmapImage(Properties.Resources.digital0);
                if (pin != null) cmd.digitalControl(pin.no, 0);
            }
            imgQueue.Children.RemoveAt(0);
            //if(imgQueue.Children.Count == 0 && loop)
            //{
            //    parseChannelSeq();
            //}

            return true;
        }
        private int withoutAnimationPoint = 0;
        public bool AheadWithoutAnimation(Command cmd)
        {
            if (imgQueue.Children.Count != channelSeq.Length) Reset();
            if (withoutAnimationPoint >= channelSeq.Length) return false;
            if (channelSeq[withoutAnimationPoint] == '0')
            {
                if(pin!=null)cmd.digitalControl(pin.no, 0);
            }
            else
            {
                if (pin != null) cmd.digitalControl(pin.no, 1);
            }
            withoutAnimationPoint++;
            //if(withoutAnimationPoint >= channelSeq.Length && loop) withoutAnimationPoint=0;

            return true;

        }
    }
}
