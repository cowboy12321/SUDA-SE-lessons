using System;
using System.Collections.Generic;
using System.Windows;

namespace GEC_LAB._02_Window
{
    /// <summary>
    /// ChangeName.xaml 的交互逻辑
    /// </summary>
    public partial class SelectBox : Window
    {
        public int selected;
        public SelectBox(Window? owner, string title,List<object> list,int selectIndex)
        {
            InitializeComponent();
            Title = title;
            Owner = owner;
            foreach(var item in list) {
                combox.Items.Add(item);
            }
            if(combox.Items.Count > 0)
            {
                combox.SelectedIndex = Math.Min(selectIndex,combox.Items.Count-1);
            }
        }
        private void Button_Confirm(object sender, RoutedEventArgs e)
        {
            selected = combox.SelectedIndex;
            DialogResult = true;
        }
        private void Button_Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
