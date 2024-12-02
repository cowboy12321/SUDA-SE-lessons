using GEC_LAB._02_Window;
using GEC_LAB._04_Class;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GEC_LAB._03_UserControl.Component
{
    /// <summary>
    /// LedAttachment.xaml 的交互逻辑
    /// </summary>
    public partial class LedAttachment : UserControl
    {
        Action? DataChangeHandler;
        LedDataModel data;
        public LedAttachment(LedDataModel data, Action DataChanged)
        {
            InitializeComponent();
            DataChangeHandler += DataChanged;
            this.data = data;
            init();
        }
        public void init() {
            Height = 90; Width = 150;
            txtExpress.Text = data.express;
            txtName.Text = data.name;

            txtExpress.TextChanged += (s, e) => { data.express = txtExpress.Text; ProjectHelper.Saved =false; };
            txtName.TextChanged += (s, e) => { data.name = txtName.Text; DataChangeHandler?.Invoke(); ProjectHelper.Saved = false; };
            txtExpress.PreviewMouseLeftButtonDown += gotFocus;
        }

        private void gotFocus(object sender, RoutedEventArgs e)
        {
            ExpressEditor ee = new ExpressEditor(Gobals.main, txtExpress.Text, ProjectHelper.GetEnablePins());
            if (ee.ShowDialog() == true)
            {
                txtExpress.Text = ee.data;
                ((Panel)txtExpress.Parent).Focus();
            }
        }
    }
}
