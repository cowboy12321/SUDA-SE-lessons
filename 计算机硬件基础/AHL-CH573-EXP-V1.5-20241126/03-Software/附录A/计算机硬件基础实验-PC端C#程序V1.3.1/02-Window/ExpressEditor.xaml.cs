using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GEC_LAB._02_Window
{
    /// <summary>
    /// ExpressEditor.xaml 的交互逻辑
    /// </summary>
    public partial class ExpressEditor : Window
    {
        #region 全局变量
        List<object> list = new List<object>();
        public string data = "";
        #endregion
        #region 初始化
        public ExpressEditor()
        {
            InitializeComponent();
            init();
        }
        public ExpressEditor(Window? owner, string? express, List<object> l)
        {
            if(owner!=null)Owner= owner;
            InitializeComponent();
            init();
            setList(l);
            txtExpress.Text = express;
        }

        private void init()
        {
            btnConfirm.Click += (s,e)=> { data = txtExpress.Text; DialogResult = true; };
            btnCanel.Click += (s,e)=> DialogResult = false;
            btnBackSpace.Click += (s, e) => performBack() ;
            KeyDown += ExpressEditor_KeyDown;
        }

        #endregion
        #region 开放的函数
        public void setList(List<object> l)
        {
            list.Clear();
            list.AddRange(l);
            refreshControl();
        }
        public void refreshControl()
        {
            foreach (object o in list) addItem(o);
        }
        #endregion
        #region 界面操作函数
        private void addItem(object o)
        {
            Button btn = new Button();
            btn.Content = o.ToString();
            btn.Style = (Style)FindResource("ExpressEditorItem");
            btn.Click += (s,e) => addTextToExpress("[" + ((Control)s).Tag.ToString() + "]");
            btn.Tag = o;
            bodyContainer.Children.Add(btn);
        }
        private void addTextToExpress(string ins)
        {
            string text = txtExpress.Text;
            int index = txtExpress.SelectionStart;
            txtExpress.Text = text.Insert(index, ins);
            txtExpress.Focus();
            txtExpress.SelectionStart = index + ins.Length;
        }
        private void performBack() {
            string text = txtExpress.Text;
            int index = txtExpress.SelectionStart;
            int len = txtExpress.SelectionLength;
            txtExpress.Focus();
            if (len > 0)
            {
                txtExpress.Text = text.Substring(0, index) + text.Substring(index + len);
                txtExpress.SelectionStart = index;
            }
            else if (index > 0 && index <= text.Length && text[index - 1] == ']')
            {
                int pos = text.Substring(0, index).LastIndexOf("[");
                if (pos == -1) pos = 0;
                txtExpress.SelectionStart = pos;
                txtExpress.SelectionLength = index - pos;
            }
            else if (index > 0)
            {
                txtExpress.Text = text.Remove(index - 1, 1);
                txtExpress.SelectionStart = index - 1;
            }
        }
        #endregion
        #region 事件监听
        private void ExpressEditor_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key) {
                case Key.Escape:
                    DialogResult = false;
                    break;
                case Key.Back:
                    performBack();
                    break;

            }
        }

        #endregion
    }
}
