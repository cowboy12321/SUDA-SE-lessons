using GEC_LAB._04_Class;
using GEC_LAB._04_Class.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace GEC_LAB._03_UserControl
{
    /// <summary>
    /// pageStep04.xaml 的交互逻辑
    /// </summary>
    public partial class PageStep04 : UserControl
    {

        PageStep04ViewModel viewModel;
        public PageStep04()
        {
            InitializeComponent();
            this.DataContext = viewModel = new PageStep04ViewModel();
            viewModel.PageChange += (sender, e) => refreshController();
            ProjectHelper.OnReload += ()=> { controllerPanel.Children.Clear();refreshController(); };
            
        }

        #region 界面刷新
        private void refreshController() {
            Dictionary<int,ExpPin> map=new Dictionary<int,ExpPin>();
            foreach (ExpPin item in ProjectHelper.GetEnablePins())
            {
                if (item.enable && (item.pinMode==ExpPin.PinMode.AnalogOut||item.pinMode==ExpPin.PinMode.DigitalOut))
                {
                    map.Add(item.no, item);
                }
            }
            lock (controllerPanel.Children)
            {
                List<UIElement> remove = new List<UIElement>();
                foreach (UIElement item in controllerPanel.Children)
                {
                    if (item is AnalogController)
                    {
                        AnalogController analogController = (AnalogController)item;
                        if (!map.ContainsKey(analogController.pin.no) || map[analogController.pin.no].pinMode!=ExpPin.PinMode.AnalogOut)
                        {
                            remove.Add(item);
                        }
                        else
                        {
                            map.Remove(analogController.pin.no);
                        }
                    }
                    else if (item is DigitalController)
                    {
                        DigitalController digitalController = (DigitalController)item;
                        if (!map.ContainsKey(digitalController.pin.no) || map[digitalController.pin.no].pinMode != ExpPin.PinMode.DigitalOut)
                        {
                            remove.Add(item);
                        }
                        else
                        {
                            map.Remove(digitalController.pin.no);
                        }
                    }
                    else
                    {
                        remove.Add(item);
                    }
                }
                foreach (UIElement item in remove) {
                    controllerPanel.Children.Remove(item);
                }
            }
            foreach(ExpPin item in map.Values)
            {
                if (item.pinMode == ExpPin.PinMode.AnalogOut)
                    controllerPanel.Children.Add(new AnalogController(item, viewModel.analogControl));
                else
                    controllerPanel.Children.Add(new DigitalController(item, viewModel.digitalControl));
            }
            if (EmuartHandler.Instance.UserState)
            {
                var outPair = getOut();
                viewModel?.sendAllMsg(outPair.First,outPair.Second);
            }
        }
        private Pair<List<Pair<int, bool>>, List<Pair<int, double>>> getOut()
        {
            List<Pair<int, bool>> digitalOut = new List<Pair<int, bool>>();
            List<Pair<int, double>> analogOut = new List<Pair<int, double>>();
            Dispatcher.Invoke(new Action(() =>
            {
                foreach (var x in controllerPanel.Children)
                {
                    if (x is AnalogController)
                    {
                        analogOut.Add(new Pair<int, double>(((AnalogController)x).pin.no, ((AnalogController)x).Value));
                    }
                    else if (x is DigitalController)
                    {
                        digitalOut.Add(new Pair<int, bool>(((DigitalController)x).pin.no, ((DigitalController)x).Value));
                    }
                }
            }));

            return new( digitalOut, analogOut );
        }
        #endregion

        #region 事件监听
        private void btn_uartLink_Click(object sender, RoutedEventArgs e)
        {
            btnUnLink.Visibility = Visibility.Hidden;
            btnLinking.Visibility = Visibility.Visible;
            new Thread(linkDevice).Start();
        }
        public void linkDevice() {
            var outPair = getOut();
            viewModel?.linkDevice(outPair.First, outPair.Second);
            Dispatcher.Invoke(new Action(() =>
            {
                btnUnLink.Visibility = Visibility.Visible;
                btnUnLinkText.Text = "重新连接";
                btnLinking.Visibility = Visibility.Hidden;
            }));

        }
        #endregion
    }

}
