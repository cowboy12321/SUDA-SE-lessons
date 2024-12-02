using GEC_LAB._04_Class;
using GEC_LAB._04_Class.Interface;
using GEC_LAB._04_Class.Models;
using GEC_LAB._04_Class.MoveControl;
using Newtonsoft.Json;
using System;
using System.Windows.Controls;

namespace GEC_LAB._03_UserControl.Component
{
    /// <summary>
    /// Led.xaml 的交互逻辑
    /// </summary>
    public partial class Led : UserControl, ISavedComponent
    {
        LedDataModel data = new LedDataModel("数字监控","");
        bool _value;
        bool Value { get => _value;set { _value = value;refreshControl(); } }
        MouseController mouseController;
        public Led()
        {
            InitializeComponent();
            DataRefreshCenter.Instance.ComponentTickEvent += timer_tick;
            mouseController = new MouseController(this, new LedAttachment(data, refreshControl));
            init();
        }
        private void init() {
            this.Width = 60; this.Height = 90;
            mouseController.setMovable();
            Value = false;
        }

        private void refreshControl() {
            txtName.Content = data.name;
            if (Value) digitalDisplay.Source = ImgHelper.BitmapToBitmapImage(Properties.Resources.digital1);
            else digitalDisplay.Source = ImgHelper.BitmapToBitmapImage(Properties.Resources.digital0);
        }
        public void restore(object? d)
        {

            string? s = d?.ToString();
            if (s != null)
            {
                LedDataModel? t = JsonConvert.DeserializeObject<LedDataModel>(s);
                if (t != null)
                {
                    data = t;
                }
                refreshControl();
                mouseController.setAttach(new LedAttachment(data, refreshControl)); 
            }
        }

        public SavedComponent save()
        {
            return new SavedComponent(GetType(),data);
        }

        #region 5.定时处理
        public void timer_tick()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                if (DataRefreshCenter.GetInstance().tryCalculateLogic(data.express, out bool res))
                {
                    if(res) Value= true;
                    else Value= false;
                }
            }));
        }
        #endregion
    }
    public class LedDataModel {
        public string name;
        public string express;
        public LedDataModel(string name,string exp) { this.name = name; express = exp; }
    }
}
