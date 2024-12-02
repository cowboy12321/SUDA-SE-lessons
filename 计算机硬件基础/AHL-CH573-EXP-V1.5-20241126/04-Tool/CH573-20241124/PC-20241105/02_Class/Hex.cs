using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AHL_GEC
{
    public struct hexStruct
    {     //hex文本行有效数据格式
        public byte len;          //数据长度
        public uint address;      //数据地址
        public byte type;         //数据类型
        public byte[] data;       //数据
        public byte check;        //校验和
    };
    class Hex
    {
        private uint offsetAddress;//扩展段基础地址
        private string hexFileName;   //Hex文件名
        private List<hexStruct> hexList;  //Hex行有效数据链表
        private List<string> hexStrList;  //存储hex文件中每行字符串数据

        //======================================================================
        //函数名称：Hex构造函数
        //函数返回：无
        //参数说明：无
        //功能概要：Hex构造函数，初始化Hex文件链表
        //======================================================================
        public Hex()
        {
            offsetAddress = 0;  //默认为非扩展地址
            hexFileName = "";
            hexList = new List<hexStruct>();
            hexStrList = new List<string>();
        }

        //======================================================================
        //函数名称：loadFile
        //函数返回：0：Hex文件导入并解析成功；
        //          1：传入的文件路径错误；2：文件打开失败
        //参数说明：filePath：hex文件路径
        //功能概要：根据传入的hex文件路径，对该文件进行解析取出其有效数据
        //======================================================================
        public int loadFile(string filePath)
        {
            //（1）变量声明
            int rv;                //返回值
            string fileName;       //文件名
            string fileSuffix;     //文件后缀名
            string line;           //Hex文件行数据

            rv = 0;  //默认Hex文件数据导入并解析数据成功
            //（2）获取文件名
            fileName = filePath.Substring(filePath.LastIndexOf("\\") + 1);//最后一个\\后是文件名
            fileSuffix = filePath.Substring(filePath.LastIndexOf(".") + 1);//最后一个.后是文件后缀名
            if (fileName == "" || fileSuffix != "hex")  //传入的文件错误
            {
                rv = 1;
                goto loadFile_EXIT;
            }
            //（3）获取文件数据
            //（3.1）打开文件
            FileStream fs = new FileStream(filePath, FileMode.Open);
            if (fs == null)  //文件打开失败
            {
                rv = 2;
                goto loadFile_EXIT;
            }
            //（3.2）读取数据
            this.clear();  //读取Hex数据前先清空数据
            StreamReader sr = new StreamReader(fs);  //读取数据流
            while (true)
            {
                line = sr.ReadLine(); //读取1行数据
                if (line == null) break;  //文件读取错误，退出
                this.addLine(line);  //获取hex文件行数据有效数据，添加至数据链表hexList
                this.hexStrList.Add(line);  //添加行数据至链表
            }
            sr.Close();  //关闭数据流
            fs.Close();

            //（4）至此，Hex文件数据获取成功
            this.hexFileName = fileName;  //文件名赋值

            loadFile_EXIT:
            return rv;
        }

        //======================================================================
        //函数名称：getHexList
        //函数返回：List<hexStruct>：Hex文件链表有效数据（byte数组形式）
        //参数说明：无
        //功能概要：获得Hex文件链表数据
        //======================================================================
        public List<hexStruct> getHexList()
        {
            return this.hexList;
        }

        //======================================================================
        //函数名称：getHexStrList
        //函数返回：List<string>：Hex文件内容（字符串形式）
        //参数说明：无
        //功能概要：获得Hex文件字符串内容
        //======================================================================
        public List<string> getHexStrList()
        {
            return this.hexStrList;
        }

        //======================================================================
        //函数名称：getFileName
        //函数返回：string：Hex文件名
        //参数说明：无
        //功能概要：获得Hex文件名
        //======================================================================
        public string getFileName()
        {
            return this.hexFileName;
        }

        //======================================================================
        //函数名称：getStartAddress
        //函数返回：uint：Hex文件首地址
        //参数说明：无
        //功能概要：获得Hex文件首地址
        //======================================================================
        public uint getStartAddress()
        {
            return this.hexList[0].address;
        }

        //======================================================================
        //函数名称：getCodeSize
        //函数返回：uint：Hex文件代码大小
        //参数说明：无
        //功能概要：获得导入的Hex代码大小
        //======================================================================
        public uint getCodeSize()
        {
            //（1）变量声明
            uint codesize;
            int count;
            //（2）计算代码大小
            count = this.hexList.Count;
            codesize = this.hexList[count - 1].address + this.hexList[count - 1].len - this.hexList[0].address;

            return codesize;
        }

        //======================================================================
        //函数名称：clear
        //函数返回：无
        //参数说明：无
        //功能概要：清空hex所有数据
        //======================================================================
        public void clear()
        {
            this.offsetAddress = 0;
            this.hexFileName = "";
            this.hexList.Clear();
            this.hexStrList.Clear();
        }

        //==============================内部函数================================

        //======================================================================
        //函数名称：addLine
        //函数返回：0：添加hex文件行数据成功；1：起始标识错误；
        //          2：hex行数据长度错误；3：数据类型错误；
        //          4：校验和错误；5：命令信息行
        //参数说明：line：hex文件行数据
        //功能概要：根据传入的hex文件行数据，获取各个有效数据存入链表
        //======================================================================
        private int addLine(string line)
        {
            //（1）变量定义
            int i;
            int rv;              //返回值
            byte dataLen;        //数据长度
            uint dataAddress;  //起始地址
            byte dataType;       //数据类型
            byte[] data;         //有效数据
            byte dataCheck;      //数据校验和   
            byte hexSum;         //数据累加和
            hexStruct hs;        //hex文本行数据

            rv = 0;  //默认成功，返回值为0
            dataAddress = 0;  //默认地址为
            //（2）起始标识判断
            if (line[0] != ':')
            {
                rv = 1;     //若起始标识不为“：”，返回错误
                goto hexLineCheck_EXIT;
            }

            //（3）数据长度判断
            //Hex文件一行总长=1(:)+2(数据长度)+4(起始地址)+2(数据类型)+xx(数据)+2(检验和)
            //               =11+xx
            dataLen = byte.Parse(line.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);//获取数据长度
            if (line.Length != 2 * dataLen + 11 || dataLen == 0)  //hex结束行为:00000001FF，不采集该行数据
            {
                rv = 2;     //Hex行数据长度不满足，返回错误
                goto hexLineCheck_EXIT;
            }
            hs.len = dataLen;  //赋值

            //（4）数据类型判断
            dataType = byte.Parse(line.Substring(7, 2), System.Globalization.NumberStyles.HexNumber);//获取数据类型
            if (dataType != 0 && dataType != 1 && dataType != 2 && dataType != 4)  //有效数据类型为0-5，但在此处只获取类型为0、1、2、4数据
            {
                rv = 3;     //数据类型只为0或1，不满足时返回错误
                goto hexLineCheck_EXIT;
            }
            if (dataType == 2)
            {
                offsetAddress = uint.Parse(line.Substring(9, 4), System.Globalization.NumberStyles.HexNumber);   //获取扩展地址的基础地址
                offsetAddress = offsetAddress << 4;  //按照02数据类型对段地址进行左移4位的操作
                rv = 5;
                goto hexLineCheck_EXIT;
            }
            else if (dataType == 4)
            {
                offsetAddress = uint.Parse(line.Substring(9, 4), System.Globalization.NumberStyles.HexNumber);   //获取扩展地址的基础地址
                offsetAddress = offsetAddress << 16;  //按照04数据类型对段地址进行左移16位的操作
                rv = 5;
                goto hexLineCheck_EXIT;
            }
            hs.type = dataType;  //赋值

            //（5）获取本行数据起始地址
            dataAddress = ushort.Parse(line.Substring(3, 4), System.Globalization.NumberStyles.HexNumber);//获取起始地址
            hs.address = dataAddress + offsetAddress;  //赋值

            //（6）有效数据获取
            data = new byte[dataLen];
            for (i = 0; i < dataLen; i++)  //循环获取有效数据，每次得到一个byte数据
                data[i] = byte.Parse(line.Substring(2 * i + 9, 2), System.Globalization.NumberStyles.HexNumber);
            hs.data = data;  //赋值

            //（7）检验和判断
            dataCheck = byte.Parse(line.Substring(line.Length - 2, 2), System.Globalization.NumberStyles.HexNumber);//获取数据校验和
            hexSum = 0;
            for (i = 1; i < line.Length - 2; i += 2)  //计算校验和之前的所有数据的累加和
            {
                hexSum += byte.Parse(line.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
            }
            if (dataCheck != (byte)(0x100 - hexSum))  //判断检验和是否等于0x100-累加和
            {
                rv = 4;  //不满足校验和，返回错误
                goto hexLineCheck_EXIT;
            }
            hs.check = dataCheck;  //赋值
            //至此，一行hex文件数据校验完毕，准备添加如链表
            //（8）将hex文件行数据添加进链表
            this.hexList.Add(hs);

        hexLineCheck_EXIT:
            return rv;
        }

    }
}
