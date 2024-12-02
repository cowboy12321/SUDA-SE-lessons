using GEC_LAB._03_UserControl.Component;
using GEC_LAB._04_Class;
using GEC_LAB._04_Class.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GEC_LAB._03_UserControl
{
    /// <summary>
    /// pageStep03.xaml 的交互逻辑
    /// </summary>
    public partial class PageStep03 : UserControl
    {
        public PageStep03()
        {
            InitializeComponent();
            foreach (var m in Gobals.componentItemModels) {
                ComponentItem componentItem = new ComponentItem();
                if(m.Source!=null)componentItem.DisplaySource = m.Source;
                if(m.Text!=null)componentItem.DisplayName = m.Text;
                componentItem.Width = 70;
                componentItem.Height = 90;
                componentItem.Margin = new Thickness(7);
                componentItem.MouseDown += ComponentItem_MouseDown;
                componentItem.Tag = m;
                ComponentContainer.Children.Add(componentItem);
            }
        }

        private void ComponentItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ComponentItem c = (ComponentItem)sender;
            ComponentItemModel m =(ComponentItemModel) c.Tag;
            if (e.LeftButton == MouseButtonState.Pressed) {
                DragDrop.DoDragDrop(c, m, DragDropEffects.Move);
            }
        }


    }
}
