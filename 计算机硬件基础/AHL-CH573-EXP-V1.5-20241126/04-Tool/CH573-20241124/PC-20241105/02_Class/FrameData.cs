using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

//数据封装类放在cn.edu.suda.sumcu.iot空间下的data子空间中
namespace cn.edu.suda.sumcu.iot.data
{


    ///---------------------------------------------------------------------
    /// <summary>  【根据实际需要增删】                                                         
    /// 类          ：FrameData                                  
    /// 类   功   能：指定帧数据格式，提供字节数组与结构体互相转换的接口
    /// 类中接口包含：
    ///             (1)byteToStruct：     字节数组转结构体 
    ///             (2)structToByte：     结构体转字节数组
    ///             (3)addParameter       增加参数
    /// </summary>                                                                                                       
    /// --------------------------------------------------------------------
    [Serializable]
    public class FrameData
    {
        //用于存放新增参数的信息
        //注意：参数的值转为string之后保存，取出的时候可根据type属性转为
        //需要的值。传进来时，只需转为string类型即可。方便使用。
        [Serializable]
        
        public class ParameterInfo   //
        {
            public string type;      //新增参数的类型
            public string value;     //新增参数的值 
            public string name;      //新增参数的名字
            public int size;         //新增参数占据的字节数
            public string otherName; //新增参数的别名
            public string wr;        //新增参数的读写属性

        }
        //存放数据的List数组。为了安全，设为私有，但是
        //提供了get和set属性
        private List<ParameterInfo> parameter;
        public List<ParameterInfo> Parameter
        {
            get { return parameter; }
            set { parameter = value; }
        }
        /// -------------------------------------------------------------------
        ///  <summary> 【不动】
        ///  方法名称：FrameData
        ///  功能概要：初始化类
        ///  <summary> 
        /// -------------------------------------------------------------------
        public FrameData()
        {
            this.parameter = new List<ParameterInfo>();
        }
        ////////////////////////////////////////////////////////////////
        /// -------------------------------------------------------------------
        ///  <summary> 【不动】
        ///  方法名称：Clone
        ///  输入参数：无
        ///  方法返回：拷贝后的对象
        ///  功能概要：将本对象拷贝一份并返回
        ///  </summary> 
        /// -------------------------------------------------------------------
        public FrameData Clone()
        {
            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                //序列化成流
                bf.Serialize(ms,this as object);
                ms.Seek(0, SeekOrigin.Begin);
                //反序列化成对象
                retval = bf.Deserialize(ms);
                ms.Close();
            }
            return retval as FrameData;
        }
        /// -------------------------------------------------------------------
        ///  <summary> 【不动】
        ///  方法名称：addParameter
        ///  输入参数：type：类型；name：英文名字；
        ///            otherName：别名（中文名）；wr：读写属性
        ///  方法返回：true：添加成功；false：添加失败；
        ///  功能概要：向结构体中新增参数。(value赋值为空)
        ///  备注：可添加的数据类型包括：byte、sbyte、short、ushort、int、uint、
        ///        long、ulong、float、double、byte数组。
        ///  </summary> 
        /// -------------------------------------------------------------------
        public bool addParameter(string type, string name, string otherName, string wr)
        {
            ParameterInfo data = new ParameterInfo();
            type = type.ToLower();    //为了不区分大小写，将所有的字母转为小写
            try
            {
                switch (type)         //根据type的类型初始化新增变量的类型
                {
                    case "byte":
                    case "uint_8":
                        data.size = sizeof(byte); break;
                    case "sbyte":
                    case "int_8":
                        data.size = sizeof(sbyte); break;
                    case "short":
                    case "int_16":
                        data.size = sizeof(short); break;
                    case "ushort":
                    case "uint_16":
                        data.size = sizeof(ushort); break;
                    case "int":
                    case "int_32":
                        data.size = sizeof(int); break;
                    case "uint":
                    case "uint_32":
                        data.size = sizeof(uint); break;
                    case "long":
                    case "int_64":
                        data.size = sizeof(long); break;
                    case "ulong":
                    case "uint_64":
                        data.size = sizeof(ulong); break;
                    case "float":
                        data.size = sizeof(float); break;
                    case "double":
                        data.size = sizeof(double); break;
                    default:
                        if (type.Contains("byte[") || type.Contains("uint_8["))
                        {
                            string[] s = type.Split(new char[2] { '[', ']' });
                            data.size = Convert.ToInt32(s[1]);
                        }
                        else
                        {
                            return false;
                        }
                        break;
                }
                data.type = type;
                data.name = name;
                data.value = "";
                data.otherName = otherName;
                data.wr = wr;
                this.parameter.Add(data);   //加入
            }
            catch { }
            return true;
        }
                


        /// -------------------------------------------------------------------
        ///  <summary> 【不动】
        ///  方法名称：byteToStruct
        ///  传入参数：data：待转换的字节数组
        ///  方法返回：true：转换成功；false：转换失败
        ///  功能概要：将字节数组类型的数据存入本实例的成员变量parameter(value)中
        ///  </summary> 
        /// -------------------------------------------------------------------
        public bool byteToStruct(byte[] data)
        {
            int read;       //已读字节数
            read = 0;       //初值
            //遍历成员parameter中所有的item，并根据类型依次从data数组中取出数据
            for (int i = 0;i<this.parameter.Count; i++)
            {
                try
                {
                    switch (this.parameter[i].type)
                    {
                        case "byte":
                        case "uint_8":
                        case "sbyte":     //将字节类型数据转为string类型并存入结构体
                        case "int_8":
                            this.parameter[i].value =     //将字节类型转为string类型
                                data[read].ToString();
                            read++;    break;            //根据数据类型增加已读字节数
                        case "short":    //将short类型数据转为string类型并存入结构体
                        case "int_16":
                            this.parameter[i].value =     //将字节类型转为string类型
                                BitConverter.ToInt16(data, read).ToString();
                            read += sizeof(short); break;   //根据类型增加已读字节数
                        case "ushort":  //将ushort类型数据转为string类型并存入结构体
                        case "uint_16":
                            this.parameter[i].value =     //将字节类型转为string类型
                                BitConverter.ToUInt16(data, read).ToString();
                            read += sizeof(ushort); break;  //根据类型增加已读字节数
                        case "int":        //将int类型数据转为string类型并存入结构体
                        case "int_32":
                            this.parameter[i].value =     //将字节类型转为string类型
                                BitConverter.ToInt32(data, read).ToString();
                            read += sizeof(int); break;     //根据类型增加已读字节数 
                        case "uint":      //将uint类型数据转为string类型并存入结构体
                        case "uint_32":
                            this.parameter[i].value =     //将字节类型转为string类型
                                BitConverter.ToUInt32(data, read).ToString();
                            read += sizeof(uint); break;    //根据类型增加已读字节数
                        case "long":      //将long类型数据转为string类型并存入结构体
                        case "int_64":
                            this.parameter[i].value =     //将字节类型转为string类型
                                BitConverter.ToInt64(data, read).ToString();
                            read += sizeof(long); break;    //根据类型增加已读字节数
                        case "ulong":    //将ulong类型数据转为string类型并存入结构体
                        case "uint_64":
                            this.parameter[i].value =     //将字节类型转为string类型
                                BitConverter.ToUInt64(data, read).ToString();
                            read += sizeof(ulong); break;   //根据类型增加已读字节数
                        case "float":    //将float类型数据转为string类型并存入结构体
                            this.parameter[i].value =     //将字节类型转为string类型
                                BitConverter.ToSingle(data, read).ToString();
                            read += sizeof(float); break;   //根据类型增加已读字节数
                        case "double":  //将double类型数据转为string类型并存入结构体
                            this.parameter[i].value =     //将字节类型转为string类型
                                BitConverter.ToDouble(data, read).ToString();
                            read += sizeof(double); break;  //根据类型增加已读字节数
                        default:
                            if (this.parameter[i].type.Contains("byte[") || this.parameter[i].type.Contains("uint_8["))
                            {
                                //创建该item要求大小的字节数组
                                byte[] temp1 = new byte[this.parameter[i].size];
                                //从data字节数组中拷贝该item要求大小的字节数据
                                Array.Copy(data, read, temp1, 0, this.parameter[i].size);
                                //将该字节数组转为string类型
                                string sss = System.Text.Encoding.Default.GetString(temp1);
                                //将该字符串保存到该item的value中
                                this.parameter[i].value = sss;
                                //根据item的size参数长度增加已读字节数
                                read += this.parameter[i].size; break;
                            }
                            else
                            {
                                return false; //找不到该数据类型，输入参数错误
                            }
                    }
                }
                catch
                {

                }
            }
            return true;                              //至此，转换完成，返回正确
        }

        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：structToByte
        ///  传入参数：无
        ///  方法返回：结构体转换出来的字节数组
        ///  功能概要：将结构体类型的数据(value)转化为对应字节数组
        ///  内部调用：无
        ///  <summary> 
        /// -------------------------------------------------------------------
        public byte[] structToByte()
        {
            int read = 0;                                          //已读字节数
            int length = 0;                                    //转换后的字节数
            byte[] temp;
            int i;
            //通过遍历parameter成员的item的size，计算占据的总字节数
            for (i = 0; i < this.parameter.Count; i++)
            {
                length += this.parameter[i].size;
            }
            byte[] data = new byte[length];             //新建存放转换完成的数组
            Array.Clear(data,0,length);                             //清空该数组
            //遍历parameter成员的item，并把item的value转换成正确类型之后放入数组
            for (i = 0; i < this.parameter.Count; i++)
            {
                try
                {
                    switch (this.parameter[i].type)
                    {
                        case "byte":
                        case "uint_8":
                        case "sbyte":
                        case "int_8":
                            data[read] =  //将value转为指定类型，并赋给字节数组data
                                Convert.ToByte(this.parameter[i].value);
                            read++; break;
                        case "short":
                        case "int_16":
                            temp = BitConverter.GetBytes      //将value转为字节数组
                                (Convert.ToInt16(this.parameter[i].value));
                            //将转换后的字节数组拷贝到data数组中
                            Array.Copy(temp, 0, data, read, temp.Length);
                            read += sizeof(short); break;  //根据类型增加已读字节数
                        case "ushort":
                        case "uint_16":
                            temp = BitConverter.GetBytes
                                (Convert.ToUInt16(this.parameter[i].value));
                            Array.Copy(temp, 0, data, read, temp.Length);
                            read += sizeof(ushort); break;
                        case "int":
                        case "int_32":
                            temp = BitConverter.GetBytes
                                (Convert.ToInt32(this.parameter[i].value));
                            Array.Copy(temp, 0, data, read, temp.Length);
                            read += sizeof(int); break;
                        case "uint":
                        case "uint_32":
                            temp = BitConverter.GetBytes
                                (Convert.ToUInt32(this.parameter[i].value));
                            Array.Copy(temp, 0, data, read, temp.Length);
                            read += sizeof(uint); break;
                        case "long":
                        case "int_64":
                            temp = BitConverter.GetBytes
                                (Convert.ToInt64(this.parameter[i].value));
                            Array.Copy(temp, 0, data, read, temp.Length);
                            read += sizeof(long); break;
                        case "ulong":
                        case "uint_64":
                            temp = BitConverter.GetBytes
                                (Convert.ToUInt64(this.parameter[i].value));
                            Array.Copy(temp, 0, data, read, temp.Length);
                            read += sizeof(ulong); break;
                        case "float":
                            temp = BitConverter.GetBytes
                                (Convert.ToSingle(this.parameter[i].value));
                            Array.Copy(temp, 0, data, read, temp.Length);
                            read += sizeof(float); break;
                        case "double":
                            temp = BitConverter.GetBytes
                                (Convert.ToDouble(this.parameter[i].value));
                            Array.Copy(temp, 0, data, read, temp.Length);
                            read += sizeof(double); break;
                        default:
                            if (this.parameter[i].type.Contains("byte[") || this.parameter[i].type.Contains("uint_8["))
                            {
                                temp = System.Text.Encoding.Default.GetBytes
                                    (this.parameter[i].value);
                                Array.Copy(temp, 0, data, read, temp.Length);
                                read += this.parameter[i].size; break;
                            }
                            else
                            {
                                return null;//找不到该数据类型，输入参数错误
                            }
                    }
                }
                catch
                {

                }
            }
            return data;
        }
        
        /// -------------------------------------------------------------------
        ///  <summary> 【不动】
        ///  方法名称：dataRowToStruct
        ///  功能概要：将一行数据转存入本实例的成员变量parameter(value)中
        ///  内部调用：无
        ///  <summary> 
        ///  <param name="fd">包含插入帧中数据的结构体</param>
        ///  <returns>数据库表受改变的行数</returns>
        /// -------------------------------------------------------------------
        public bool dataRowToStruct(DataRow data)
        {

            int i;
            for (i = 0; i < this.parameter.Count;i++ )
            {
                if( this.parameter[i].name.ToLower()== "sendtime")
                {
                    System.DateTime startTime =           //获取时间基准
                           TimeZone.CurrentTimeZone.ToLocalTime
                           (new System.DateTime(1970, 1, 1));
                    ulong temp = (ulong)
                        (System.DateTime.Now.AddHours(8) - startTime).TotalSeconds;
                    this.Parameter[i].value =
                        temp.ToString();   //更新当前时间与基准时间的差值
                    continue;
                }
                this.parameter[i].value = 
                    data[this.parameter[i].name].ToString();
            }
            return true; ;
        }
    }
}