using System;
using System.Windows;

namespace GEC_LAB._02_Window
{
    /// <summary>
    /// ChangeName.xaml 的交互逻辑
    /// </summary>
    public partial class ChangeName : Window
    {
        public string? Hint {
            get => lblHint.Content.ToString(); 
            set => lblHint.Content = value;
        }
        public string? Text { 
            get => textBox.Text;
            set => textBox.Text = value;
        }
        public string data="";
        public ChangeName()
        {
            InitializeComponent();
        }
        public ChangeName(string Title, string Hint, string Value)
        {
            InitializeComponent();
            this.Title = Title;
            this.Hint = Hint;   
            this.Text = Value;
        }

        private void Button_Confirm(object sender, RoutedEventArgs e)
        {
            data = textBox.Text;
            this.DialogResult = true;
        }
        private void Button_Cancel(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
