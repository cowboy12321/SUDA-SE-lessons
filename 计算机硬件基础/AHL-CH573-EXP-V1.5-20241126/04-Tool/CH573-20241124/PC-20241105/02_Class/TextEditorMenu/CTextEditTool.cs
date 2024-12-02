
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pub
{
    public class CTextEditTool
    {

        //查找第一个
        public static void FindFirst(RichTextBox rich, string content)
        {
            int index = rich.Find(content, 0);
            if (index >= 0) ShowSelection(rich, index, content.Length);
        }

        //查找最后一个
        public static void FindLast(RichTextBox rich, string content)
        {
            int index = rich.Find(content, rich.Text.Length, RichTextBoxFinds.Reverse);
            if (index >= 0) ShowSelection(rich, index, content.Length);
        }

        /// <summary> 
        /// 自己定义查找方法.参数content是要查询的内容. 
        /// type是查询条件.如：从文件头部向下查询，从文件尾部向上查询. 
        /// 如type= RichTextBoxFinds.None 表示从文件头部向下查询. 
        /// </summary> 
        public static void FindText(RichTextBox rich, string content, RichTextBoxFinds options)
        {
            int startIndex;
            int endIndex;

            if ((options & RichTextBoxFinds.Reverse) == RichTextBoxFinds.Reverse)
            {
                startIndex = 0;
                endIndex = rich.SelectionStart;
            }
            else
            {
                startIndex = rich.SelectionStart + rich.SelectionLength;
                endIndex = rich.Text.Length;
            }

            int index = rich.Find(content, startIndex, endIndex, options);

            if (index >= 0) //如果找到
                ShowSelection(rich, index, content.Length);
            else
                MessageBox.Show("Not found!");
        }

        //选择搜索到的文本
        private static void ShowSelection(RichTextBox rich, int index, int length)
        {
            rich.SelectionStart = index;
            rich.SelectionLength = length;
            rich.SelectionColor = Color.Red;
            rich.Focus();
        }
    }
}