using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AHL_GEC
{
    public class CSharpFormatHelper
    {
        //remove empty lines from string
        public static string RemoveEmptyLines(string lines)
        {
            return Regex.Replace(lines, @"^\s*$\n|\r", "", RegexOptions.Multiline).TrimEnd();
        }
        //Indent String with Spaces 缩进
        public static string Indent(int count)
        {
            if (count <= 0)
            {
                return "";
            }
            else
            {
                return "    ".PadLeft(count * 2);
            }
        }

        /// <summary>
        /// 格式化C#代码
        /// </summary>
        /// <param name="code">待格式化的代码</param>
        /// <returns>格式化后的代码</returns>
        public static string FormatCSharpCode(string code)
        {
            //去除空白行
            //code = RemoveEmptyLines(code);
            StringBuilder sb = new StringBuilder();
            int count = 2;
            int times = 0;
            string[] lines = code.Split('\n');
            foreach (var line in lines)
            {
                //每遇“{”空4格，遇“}”退4格
                if (line.TrimStart().StartsWith("{") || line.TrimEnd().EndsWith("{"))
                {
                    sb.Append(Indent(count * times) + line.TrimStart() + "\n");
                    times++;
                }
                else if (line.TrimStart().StartsWith("}"))
                {
                    times--;
                    if (times <= 0)
                    {
                        times = 0;
                    }
                    sb.Append(Indent(count * times) + line.TrimStart() + "\n");
                }
                else
                {
                    sb.Append(Indent(count * times) + line.TrimStart() + "\n");
                }
            }
            return sb.ToString();
        }


        /// <summary>
        /// 格式化汇编代码
        /// </summary>
        /// <param name="code">待格式化的代码</param>
        /// <returns>格式化后的代码</returns>
        public static string FormatAssemblyCode(string code)
        {
            //【20200501 2/4】汇编代码对齐功能函数，用于处理注释，#，:，以及其他代码的对齐方法
            StringBuilder sb = new StringBuilder();
            string[] lines = code.Split('\n');
            foreach (var line in lines)
            {
                //【1】//注释或#顶格
                if (line.TrimStart().StartsWith("//") || line.TrimStart().StartsWith("#")) sb.Append(line.TrimStart() + "\n");
                //【2】:标号开头的顶格
                else if(line.TrimEnd().EndsWith(":"))sb.Append(line.TrimStart() + "\n");
                //【3】/*或*注释的顶格
                else if(line.TrimStart().StartsWith("/*") || line.TrimStart().StartsWith("*"))sb.Append(line.TrimStart() + "\n");
                //【4】除此以外的所有代码退后一格
                else if (!line.StartsWith("    ") && !line.StartsWith("\t"))sb.Append("\t" + line.TrimStart() + "\n");
                //【5】与【4】一样
                else if (line.StartsWith("     "))sb.Append("\t" + line.TrimStart() + "\n");
                //【6】其他格式保留原样
                else sb.Append(line + "\n");
            }
            return sb.ToString();
        }
    }
}
