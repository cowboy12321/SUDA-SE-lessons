using ImTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace GEC_LAB._04_Class
{
    public class StringUtils
    {
        public static string nullToEmpty(object? obj)
        {
            string? s = obj?.ToString();
            return s == null ? "" : s;
        }
        public static bool nullOrEmpty(object? obj)
        {
            return obj == null || obj.ToString() == "";
        }
    }
    public class CommonUtils
    {
        //======================================================================
        //函数名称：bytesToStruct
        //函数返回：byte数组转换为对应的结构体
        //参数说明：bytes:字节数组;type:结构体类型
        //功能概要：将byte字节数组数据转换为对应的结构体数据
        //======================================================================
        public static object? bytesToStruct(byte[] bytes, Type type)
        {
            //（1）变量声明
            int size;
            object? obj;
            IntPtr structPtr;

            size = Marshal.SizeOf(type);
            //（2）判断字节长度
            if (size > bytes.Length) return null;
            //（3）分配结构体内存空间
            structPtr = Marshal.AllocHGlobal(size);
            //（4）将byte数组拷贝到分配好的内存空间
            Marshal.Copy(bytes, 0, structPtr, size);
            //（5）将内存空间转换为目标结构体
            obj = Marshal.PtrToStructure(structPtr, type);
            //（6）释放内存空间
            Marshal.FreeHGlobal(structPtr);

            return obj;
        }


        /// <summary>
        /// 中位值平均滤波法
        /// </summary>
        /// <param name="us">达到滤波个数的一组电压值</param>
        /// <returns></returns>
        public static double MidianAverageFileter(List<double> us)
        {
            int N = us.Count;
            us.Sort();

            double sum = 0F;
            int cnt = 0;
            for (int i = N / 3; i < (N - N / 3); i++)
            {
                sum += us[i];
                cnt++;
            }
            return (sum / cnt);
        }

        public static int? calcExpress(string express)
        {
            if(express == null||express=="") return null;
            int pc = 0;
            int or = -1,lsh=-1;
            for(int i=0; i <express.Length; i++)
            {
                switch (express[i])
                {
                    case '(': pc++;break;
                    case ')': pc--;break;
                    case '<': if (pc == 0 && i < express.Length - 1 && express[i + 1] == '<') lsh = i; break;
                    case '|': if (pc == 0) or = i;break;
                }
            }
            if (or != -1)
            {
                int? lhs = calcExpress(express.Substring(0, or));
                int? rhs = calcExpress(express.Substring((or + 1)));
                if (lhs != null && rhs != null) return lhs | rhs;
                else return null;
            }
            if (lsh != -1)
            {
                int? lhs = calcExpress(express.Substring(0, lsh));
                int? rhs = calcExpress(express.Substring((lsh + 2)));
                if (lhs != null && rhs != null) return lhs << rhs;
                else return null;

            }
            if (express.StartsWith("(") && express.EndsWith(")")) return calcExpress(express.Substring(1, express.Length - 2));

            if(long.TryParse(express, out var result))
            {
                return (int)result;
            }
            return null;
        }
        //======================================================================
        //函数名称：clacLogicExpress
        //函数返回：逻辑表达式的值
        //参数说明：express:表达式
        //功能概要：计算express对应的值
        //======================================================================
        public static bool clacLogicExpress(string express)
        {
            int index = express.IndexOf("||");
            if (index != -1)
            {
                return clacLogicExpress(express.Substring(0, index)) || clacLogicExpress(express.Substring(index + 2));
            }
            index = express.IndexOf("&&");
            if (index != -1)
            {
                return clacLogicExpress(express.Substring(0, index)) && clacLogicExpress(express.Substring(index + 2));
            }
            if (express[0] == '(' && express[express.Length - 1] == ')') return clacLogicExpress(express.Substring(1, express.Length - 2));

            if (express.Equals("true") || express.Equals("True"))
            {
                return true;
            }
            else if (express.Equals("false") || express.Equals("False"))
            {
                return false;
            }
            else
            {
                try
                {
                    return int.Parse(express) != 0;
                }
                catch (Exception)
                {
                    throw new ArithmeticException();
                }
            }

        }

        public static byte[] readFile(string fileName) {
            FileStream fileStream = new FileStream(fileName, FileMode.Open,
            FileAccess.Read, FileShare.Read);

            // 读取文件的 byte[]
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            return bytes;
        }


        //======================================================================
        //函数名称：DeepCopy
        //函数返回：深拷贝一个对象
        //======================================================================
        public static T DeepCopy<T>(T obj)
        {
            #pragma warning disable 8600,8603
            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer xml = new XmlSerializer(typeof(T));
                xml.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                retval = xml.Deserialize(ms);
                ms.Close();
            }
            return (T)retval;
            #pragma warning restore 8600, 8603
        }


        public static Color DarkenColor(Color color, double factor)
        {
            // factor 是一个 0 到 1 之间的值，用于决定变暗的程度。0 是完全黑色，1 是保持原色。
            factor = Math.Clamp(factor, 0, 1);

            byte r = (byte)(color.R * factor);
            byte g = (byte)(color.G * factor);
            byte b = (byte)(color.B * factor);

            return Color.FromArgb(color.A, r, g, b);
        }

        public static System.Windows.Media.Color DarkenColor(System.Windows.Media.Color color, double factor)
        {
            // factor 是一个 0 到 1 之间的值，用于决定变暗的程度。0 是完全黑色，1 是保持原色。
            factor = Math.Clamp(factor, 0, 1);

            byte r = (byte)(color.R * factor);
            byte g = (byte)(color.G * factor);
            byte b = (byte)(color.B * factor);

            return System.Windows.Media.Color.FromArgb(color.A, r, g, b);
        }

    }
    public class Pair<T1, T2> where T1 : notnull where T2 : notnull
    {
        public T1 First;
        public T2 Second;
        public Pair(T1 first, T2 second)
        {
            First = first;
            Second = second;
        }
    }
    public class DictionaryHelper<TKey, TValue> : Dictionary<TKey, TValue> where TKey : notnull
    {
        public DictionaryHelper() : base() { }
        public delegate TValue Maker();
        public TValue computeIfAbsent(TKey key, Maker func)
        {
            if (this.TryGetValue(key, out TValue? value))
            {
                return value;
            }
            else
            {
                value = func();
                this.Add(key, value);
                return value;
            }
        }
        public bool put(TKey key, TValue value)
        {
            if (TryAdd(key, value))
            {
                return true;
            }
            else
            {
                this[key] = value;
                return false;
            }
        }
    }
    public class CustomCompareList<T> : List<T> where T : notnull
    {
        IComparer<T > comparer; 
        public CustomCompareList(IComparer<T> comparer) : base() {
            this.comparer = comparer;
        }
        public CustomCompareList(List<T> list,IComparer<T> comparer) : base(list)
        {
            this.comparer = comparer;

        }
        public new bool Contains(T value)
        {
            foreach (T item in this)
            {
                if(comparer.Compare(item, value) == 0) return true;
            }
            return false;
        }
        public new int IndexOf(T value)
        {
            for (int i = 0; i < Count; i++)
            {
                if (comparer.Compare(this[i], value) == 0) return i;

            }
            return -1;
        }
    }
}
