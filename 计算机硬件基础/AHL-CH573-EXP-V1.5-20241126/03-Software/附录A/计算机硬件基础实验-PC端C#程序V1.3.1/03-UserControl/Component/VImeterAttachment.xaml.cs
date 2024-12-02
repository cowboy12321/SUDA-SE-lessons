using GEC_LAB._02_Window;
using GEC_LAB._04_Class;
using System.Windows;
using System.Windows.Controls;

namespace GEC_LAB._03_UserControl.Component
{
    /// <summary>
    /// VImeterAttachment.xaml 的交互逻辑
    /// </summary>
    public partial class VImeterAttachment : UserControl
    {
        public delegate void Func();
        Func? DataChangeHandler;
        VImeterDataModel data;
        public VImeterAttachment(VImeterDataModel data, Func? dataChange)
        {
            InitializeComponent();
            this.data = data;
            DataChangeHandler += dataChange;
            init();
        }

        private void init()
        {
            Width = 240; Height = 110;
            txtName.Text = data.name;
            txtExpress.Text=data.express;
            numMax.Value = data.maxValue;

            txtName.TextChanged += (s, e) => { data.name = txtName.Text; DataChangeHandler?.Invoke(); ProjectHelper.Saved = false; };
            txtExpress.TextChanged += (s, e) => { data.express = txtExpress.Text; ProjectHelper.Saved = false; };
            txtExpress.PreviewMouseLeftButtonDown += gotFocus;
            numMax.ValueChanged += (s,e) => { data.maxValue = (double)numMax.Value; DataChangeHandler?.Invoke(); ProjectHelper.Saved = false; };
        }

        private void gotFocus(object sender, RoutedEventArgs e)
        {
            Control c = (Control)sender;
            ExpressEditor ee = new ExpressEditor(Gobals.main, txtExpress.Text, ProjectHelper.GetEnablePins());
            if (ee.ShowDialog() == true)
            {
                txtExpress.Text = ee.data;
                ((Panel)txtExpress.Parent).Focus();
            }
        }


    }
}
