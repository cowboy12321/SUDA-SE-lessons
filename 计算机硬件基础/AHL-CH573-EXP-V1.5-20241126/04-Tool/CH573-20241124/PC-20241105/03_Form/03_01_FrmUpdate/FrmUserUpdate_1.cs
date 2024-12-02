using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using cn.edu.suda.sumcu.iot;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Diagnostics;
using Sunny.UI.Win32;

namespace AHL_GEC
{
    public partial class frm_UserUpdate_1 : Form
    {
        //定义使用的全局变量
        private Hex hex;  //Hex文件信息，整体更新用
        private Update update;  //更新类，保存更新所使用的帧结构体与方法
        private SynchronizationContext m_SyncContext = null;    //用于安全地跨线程访问控件
        public EMUART emuart;     //串口通信类对象
        private uint softVersion; //保存终端更新程序版本，0表示更新版本不详，1表示更新版本为VA.10之前，2表示更新版本为VA.10之后
        private Hex_old hex_old;  //（VA.10版本之前）更新程序Hex对象
        private Update_old update_old;  //（VA.10版本之前）更新类对象，保存更新所使用的帧结构体与方法
        private static UInt32 message_count;
        private List<FileInfo> fileList;    //当前工程文件列表
        string modulePath = string.Empty;    //模板工程路径
        public static UInt32[] crcTable =
        {
          0x00000000, 0x04c11db7, 0x09823b6e, 0x0d4326d9, 0x130476dc, 0x17c56b6b, 0x1a864db2, 0x1e475005,
          0x2608edb8, 0x22c9f00f, 0x2f8ad6d6, 0x2b4bcb61, 0x350c9b64, 0x31cd86d3, 0x3c8ea00a, 0x384fbdbd,
          0x4c11db70, 0x48d0c6c7, 0x4593e01e, 0x4152fda9, 0x5f15adac, 0x5bd4b01b, 0x569796c2, 0x52568b75,
          0x6a1936c8, 0x6ed82b7f, 0x639b0da6, 0x675a1011, 0x791d4014, 0x7ddc5da3, 0x709f7b7a, 0x745e66cd,
          0x9823b6e0, 0x9ce2ab57, 0x91a18d8e, 0x95609039, 0x8b27c03c, 0x8fe6dd8b, 0x82a5fb52, 0x8664e6e5,
          0xbe2b5b58, 0xbaea46ef, 0xb7a96036, 0xb3687d81, 0xad2f2d84, 0xa9ee3033, 0xa4ad16ea, 0xa06c0b5d,
          0xd4326d90, 0xd0f37027, 0xddb056fe, 0xd9714b49, 0xc7361b4c, 0xc3f706fb, 0xceb42022, 0xca753d95,
          0xf23a8028, 0xf6fb9d9f, 0xfbb8bb46, 0xff79a6f1, 0xe13ef6f4, 0xe5ffeb43, 0xe8bccd9a, 0xec7dd02d,
          0x34867077, 0x30476dc0, 0x3d044b19, 0x39c556ae, 0x278206ab, 0x23431b1c, 0x2e003dc5, 0x2ac12072,
          0x128e9dcf, 0x164f8078, 0x1b0ca6a1, 0x1fcdbb16, 0x018aeb13, 0x054bf6a4, 0x0808d07d, 0x0cc9cdca,
          0x7897ab07, 0x7c56b6b0, 0x71159069, 0x75d48dde, 0x6b93dddb, 0x6f52c06c, 0x6211e6b5, 0x66d0fb02,
          0x5e9f46bf, 0x5a5e5b08, 0x571d7dd1, 0x53dc6066, 0x4d9b3063, 0x495a2dd4, 0x44190b0d, 0x40d816ba,
          0xaca5c697, 0xa864db20, 0xa527fdf9, 0xa1e6e04e, 0xbfa1b04b, 0xbb60adfc, 0xb6238b25, 0xb2e29692,
          0x8aad2b2f, 0x8e6c3698, 0x832f1041, 0x87ee0df6, 0x99a95df3, 0x9d684044, 0x902b669d, 0x94ea7b2a,
          0xe0b41de7, 0xe4750050, 0xe9362689, 0xedf73b3e, 0xf3b06b3b, 0xf771768c, 0xfa325055, 0xfef34de2,
          0xc6bcf05f, 0xc27dede8, 0xcf3ecb31, 0xcbffd686, 0xd5b88683, 0xd1799b34, 0xdc3abded, 0xd8fba05a,
          0x690ce0ee, 0x6dcdfd59, 0x608edb80, 0x644fc637, 0x7a089632, 0x7ec98b85, 0x738aad5c, 0x774bb0eb,
          0x4f040d56, 0x4bc510e1, 0x46863638, 0x42472b8f, 0x5c007b8a, 0x58c1663d, 0x558240e4, 0x51435d53,
          0x251d3b9e, 0x21dc2629, 0x2c9f00f0, 0x285e1d47, 0x36194d42, 0x32d850f5, 0x3f9b762c, 0x3b5a6b9b,
          0x0315d626, 0x07d4cb91, 0x0a97ed48, 0x0e56f0ff, 0x1011a0fa, 0x14d0bd4d, 0x19939b94, 0x1d528623,
          0xf12f560e, 0xf5ee4bb9, 0xf8ad6d60, 0xfc6c70d7, 0xe22b20d2, 0xe6ea3d65, 0xeba91bbc, 0xef68060b,
          0xd727bbb6, 0xd3e6a601, 0xdea580d8, 0xda649d6f, 0xc423cd6a, 0xc0e2d0dd, 0xcda1f604, 0xc960ebb3,
          0xbd3e8d7e, 0xb9ff90c9, 0xb4bcb610, 0xb07daba7, 0xae3afba2, 0xaafbe615, 0xa7b8c0cc, 0xa379dd7b,
          0x9b3660c6, 0x9ff77d71, 0x92b45ba8, 0x9675461f, 0x8832161a, 0x8cf30bad, 0x81b02d74, 0x857130c3,
          0x5d8a9099, 0x594b8d2e, 0x5408abf7, 0x50c9b640, 0x4e8ee645, 0x4a4ffbf2, 0x470cdd2b, 0x43cdc09c,
          0x7b827d21, 0x7f436096, 0x7200464f, 0x76c15bf8, 0x68860bfd, 0x6c47164a, 0x61043093, 0x65c52d24,
          0x119b4be9, 0x155a565e, 0x18197087, 0x1cd86d30, 0x029f3d35, 0x065e2082, 0x0b1d065b, 0x0fdc1bec,
          0x3793a651, 0x3352bbe6, 0x3e119d3f, 0x3ad08088, 0x2497d08d, 0x2056cd3a, 0x2d15ebe3, 0x29d4f654,
          0xc5a92679, 0xc1683bce, 0xcc2b1d17, 0xc8ea00a0, 0xd6ad50a5, 0xd26c4d12, 0xdf2f6bcb, 0xdbee767c,
          0xe3a1cbc1, 0xe760d676, 0xea23f0af, 0xeee2ed18, 0xf0a5bd1d, 0xf464a0aa, 0xf9278673, 0xfde69bc4,
          0x89b8fd09, 0x8d79e0be, 0x803ac667, 0x84fbdbd0, 0x9abc8bd5, 0x9e7d9662, 0x933eb0bb, 0x97ffad0c,
          0xafb010b1, 0xab710d06, 0xa6322bdf, 0xa2f33668, 0xbcb4666d, 0xb8757bda, 0xb5365d03, 0xb1f740b4
        };

        private bool closing = false;      //标识是否正在关闭串口
        private bool listening = false;    //标识是否执行完invoke相关操作
        //方法一：使用Thread类
        ThreadStart threadStart = null;
        Thread thread = null;

        private string LinkSlave_COM = null;    //当前连接的从机串口
        private bool LinkSlave = false;         //从机正常连接标识

        //握手帧
        // LayoutKind.Sequential用于强制将成员按其出现的顺序进行顺序布局,字符串转换成ANSI字符串，Pack表示1字节对齐
        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct shakeData
        {
            // SizeConst用来定义数组大小
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
            public byte[] uecomType;          //通信模组类型
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] mcuType;            //MCU类型
            public uint uStartAds;            //User程序起始地址
            public uint uCodeSize;            //User程序总代码大小
            public uint replaceNum;           //替换更新最大字节
            public uint reserveNum;           //保留更新最大字节（不等于0意味着有User程序）
        }
        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct newshakeData
        {
            // SizeConst用来定义数组大小
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
            public byte[] uecomType;          //通信模组类型
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
            public byte[] mcuType;            //MCU类型
            public uint uStartAds;            //User程序起始地址
            public uint uCodeSize;            //User程序总代码大小
            public uint replaceNum;           //替换更新最大字节
            public uint reserveNum;           //保留更新最大字节（不等于0意味着有User程序）
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
            public byte[] BIOSVersion;            //BIOS版本号
            public uint RAMStart;               //RAM起始地址
            public uint RAMLength;              //RAM大小
            public uint FlashStart;             //Flash起始地址
            public uint FlashLength;            //Flash大小
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] coreType;             //内核类型
            public uint mcuSectSize;            //扇区大小
            public uint RTOSSize;               //操作系统空间大小
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] RTType;               //操作系统版本
            public uint RAMUserStart;           //RAM空间User的起始地址
            public uint SystemClock;            //主频大小
        }
        private newshakeData newshakedata;
        private shakeData shakedata;
        private string uecomType;   //通信模组类型
        private string mcuType;     //MCU类型
        private uint uStartAds;     //User程序起始地址
        private uint uCodeSize;     //User程序总代码大小
        private uint replaceNum;    //替换更新最大字节
        private uint reserveNum;    //保留更新最大字节（不等于0意味着有User程序）
        private byte overallStyle;  //整体更新方式
        private string biosVersion; //BIOS版本号 
        private uint sectorStart;

        //======================================================================
        //函数名称：frm_UserUpdate
        //函数返回：frm_UserUpdate窗体构造函数
        //参数说明：无
        //功能概要：完成frm_UserUpdate窗体的初始化工作
        //======================================================================
        public frm_UserUpdate_1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        //======================================================================
        //函数名称：frm_UserUpdate_Load
        //函数返回：无
        //参数说明：无
        //功能概要：窗体加载事件，窗体加载时自动调用
        //======================================================================
        private void frm_UserUpdate_Load(object sender, EventArgs e)
        {

            //fmain = (FrmMain)this.MdiParent;  //得到frmMain窗体变量
            m_SyncContext = SynchronizationContext.Current;   //用于安全地跨线程访问控件
            overallStyle = 0;    //更新方式为默认为整体更新方式
            message_count = 0;   //数据接收条数为0
            //（2）初始化Hex变量
            hex = new Hex();
            //（3）初始化串口
            emuart = EMUART.getInstance();
            //（4）初始化版本号
            softVersion = 0;  //表示版本号不明
            //（5）初始化原更新程序变量           
            hex_old = new Hex_old();

            //关闭Thread(用于统计1S内接收的消息条目数)-【20190507】 2/2  
            //(6)用Thread类
            threadStart = new ThreadStart(mes_count);//通过ThreadStart委托告诉子线程执行什么方法　　
            thread = new Thread(threadStart);
            thread.Start();//启动新线程

            //(7)更新combox控件
            int i;
            string[] SCIPorts;
            SCIPorts = System.IO.Ports.SerialPort.GetPortNames();
            this.comboBox1.Items.Clear();//首先将现有的项清除掉
            this.comboBox2.Items.Clear();//首先将现有的项清除掉
            this.comboBox1.Items.Add("自动搜索");
            this.comboBox2.Items.Add("自动搜索");
            for (i = 0; i < SCIPorts.Length; i++)
            {
                //向[串口选择框]中添加搜索到的串口号
                this.comboBox1.Items.Add(SCIPorts[i]);
                this.comboBox2.Items.Add(SCIPorts[i]);
            }
            this.comboBox1.SelectedIndex = 0;
            this.comboBox2.SelectedIndex = 0;
        }


        //======================================================================
        //函数名称：btn_fileOpen1_Click
        //函数返回：无
        //参数说明：无
        //功能概要：导入待整体更新Hex文件并对该文件进行解析取出其有效数据
        //======================================================================
        private void btn_fileopen1_Click(object sender, EventArgs e)
        {
            //（1）变量声明           
            int flag;
            uint startAdd;            //代码起始地址
            uint codesize;            //代码大小

            string filePath;          //文件路径
            string fileName;          //文件名
            string line;              //Hex文件行数据
            List<hexStruct_old> list; //更新程序对应Hex文件保存变量            
            //（2）如果终端更新程序版本为VA.10之后的版本
            if (softVersion == 2)
            {
                //（2.1）导入Hex文件

                OpenFileDialog ofd = new OpenFileDialog();  //打开文件对话框
                ofd.Filter = "hex file(*.hex)|*.hex";
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    //（2.2）获取文件名
                    filePath = ofd.FileName;
                    //（2.3）导入解析Hex文件
                    flag = hex.loadFile(filePath);
                    if (flag != 0) goto btn_fileopen1_Click_EXIT1;
                    //（2.4）获取导入的文件信息
                    fileName = hex.getFileName();
                    startAdd = hex.getStartAddress();    //hex文件中读取的起始地址
                    codesize = hex.getCodeSize();
                    //lengthDiffer = uStartAds - startAdd;    //起始长度差


                    string ldFilePath = string.Empty;
                    string userStart = string.Empty;      //被替换的起始地址
                    string strStartAdd = string.Empty;    //替换的起始地址
                    StreamReader sr = null;
                    fileList = new List<FileInfo>();
                    //如果打开工程，则先修改link文件中RAM的起始地址

                    //（2.5）判断导入的Hex文件首地址是否正确
                    if (startAdd != uStartAds)
                    {
                        //MessageBox.Show("当前User程序与BIOS程序版本不匹配");
                        goto btn_fileopen1_Click_EXIT3;
                    }
                    //（2.6）状态提示
                    this.lbl_filename1_1.Text = fileName;
                    this.prg_update1_1.Value = 0;
                    this.lbl_progressbar1.Text = "";
                    this.button1_1.Enabled = true;  //允许整体更新功能
                    txtShow1("导入" + fileName + "文件成功！\r\n");     //右侧更新提示区  
                    this.lst_codeshow1_1.Items.Clear();    //左侧代码显示区
                    foreach (string str in hex.getHexStrList())
                    {
                        this.lst_codeshow1_1.Items.Add(str);    //左侧代码显示区
                    }
                    if (lbl_uartstate_1.Text.Contains("MSP432") == true)
                    {
                        StreamReader ssr = File.OpenText(Environment.CurrentDirectory.Replace("\\bin\\Debug", "") + @"\04_Resource\DChex.txt");
                        string nextLine1;
                        while ((nextLine1 = ssr.ReadLine()) != null)
                        {
                            if (nextLine1.Contains("[MSP432 P401R]"))
                            {
                                while ((nextLine1 = ssr.ReadLine()) != null)
                                {
                                    if (nextLine1.Contains("#DC串口更新"))
                                    {
                                        button1_1.Enabled = true;
                                        ssr.Close();
                                        return;
                                    }
                                }
                            }
                        }
                        ssr.Close();
                    }
                }
                //（3）如果终端更新程序版本为VA.10及其之前的版本
                else if (softVersion == 1)
                {
                    //（3.1）导入Hex文件
                    OpenFileDialog ofdHex = new OpenFileDialog();  //打开文件对话框
                    ofdHex.Filter = "hex file(*.hex)|*.hex";
                    ofdHex.ShowDialog();
                    //（3.2）获取文件名
                    filePath = ofdHex.FileName;
                    fileName = filePath.Substring(filePath.LastIndexOf("\\") + 1);//最后一个\\后是文件名
                    if (fileName == "") goto btn_fileopen1_Click_EXIT2;//未选择Hex文件
                    //（3.3）获取文件数据，并显示在文件显示框
                    FileStream fs = new FileStream(filePath, FileMode.Open);
                    if (fs == null) goto btn_fileopen1_Click_EXIT1;    //文件打开失败
                    //（3.4）读取数据
                    StreamReader sr = new StreamReader(fs);  //读取数据流
                    hex_old.clear();  //先清空hex数据，准备接收hex数据
                    this.lst_codeshow1_1.Items.Clear();    //左侧代码显示区
                    //（3.5）遍历hex文件，读出到文本框中
                    while (true)
                    {
                        line = sr.ReadLine();  //读取1行数据
                        if (line == null) break;  //文件读取错误，退出
                        hex_old.addLine(line);  //获取hex文件行数据有效数据
                        this.lst_codeshow1_1.Items.Add(line);     //左侧代码显示区                  
                    }
                    sr.Close();  //关闭数据流
                    fs.Close();
                    list = hex_old.getHexList();
                    //（3.6）判断读取文件的合法性,如果不合法
                    if (list[0].address != 0x6800) goto btn_fileopen1_Click_EXIT3;
                    //（3.7）判断读取文件的合法性,如果合法               
                    this.lbl_filename1_1.Text = fileName;
                    this.prg_update1_1.Value = 0;
                    this.lbl_progressbar1.Text = "";
                    this.button1_1.Enabled = true;   //允许整体更新功能
                    txtShow1("导入" + fileName + "文件成功！\r\n");   //右侧更新提示区              
                }
            }

        //（4）退出区
        //（4.1）退出函数
        btn_fileopen1_Click_EXIT:
            return;

        //（4.2）导入hex文件失败
        btn_fileopen1_Click_EXIT1:
            this.button1_1.Enabled = false;  //禁止整体更新功能
            MessageBox.Show("Hex文件异常!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            goto btn_fileopen1_Click_EXIT;

        //（4.3）未选择Hex文件
        btn_fileopen1_Click_EXIT2:
            goto btn_fileopen1_Click_EXIT;

        //（4.4）导入的Hex文件不合法
        btn_fileopen1_Click_EXIT3:
            MessageBox.Show("Hex文件异常!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            txtShow1("导入的" + fileName + "文件首地址不合法，请检查！\r\n");     //右侧更新提示区  
            this.button1_1.Enabled = false;  //不允许整体更新功能
            goto btn_fileopen1_Click_EXIT;

        }


        //======================================================================
        //函数名称：txtShow_rece_data
        //函数返回：无
        //参数说明：str:在终端执行信息提示框追加显示的内容
        //功能概要：整体更新过程中，在终端执行信息提示框txt_uartinfo1中追加提示
        //          信息，并显示最新信息      
        //======================================================================
        byte lastCH = 0;    //【20190506】-2/3
        StringBuilder MyStringBuilder = new StringBuilder("", 500); //【20200428】优化效率
        private void txtShow_rece_data(byte[] data)
        {
            int count = 0;
            int i;
            int j;
            byte[] showData = data;   //临时数组

            //【20190506】-3/3 ---解决汉字显示乱码问题
            //如果上一次发送出现汉字断码
            if (lastCH != 0)
            {
                showData = new byte[data.Length + 1];   //临时数组增1一个字节
                showData[0] = lastCH;                   //上次保留的字节填入前端
                Array.Copy(data, 0, showData, 1, data.Length);  //其他数据填充后部
                lastCH = 0;                                     //lastCH清0
                data = showData;                                //指针等同                    
            }
            //从后向前统计，
            while ((count < data.Length) && (data[data.Length - 1 - count] > 128))
            {
                count++;
            }
            //如果连续大于128的字节数为奇数，则将最后一位保存，与下一次的数据一起组合
            if (count % 2 == 1)
            {
                showData = new byte[data.Length - 1];
                Array.Copy(data, 0, showData, 0, data.Length - 1);
                lastCH = data[data.Length - 1];
            }


            //【20191022】-1/1
            /*
            //不显示每次重启，串口多发送的数据0xF8，那个“？”
            int flag = 0;
            j = showData.Length;
            for (i = 0; i < showData.Length - 1; i++)
            {
                if (showData[i] == 248)
                {
                    j = i;
                }
                if (showData[i + 1] == 0xF0 && showData[i] == 0xBD)
                    flag = 1;
            }
            if (flag == 1)
            {
                for (i = j; i < showData.Length - 1; i++)
                {
                    showData[i] = showData[i + 1];
                }
            }
            */

            //将字符串转为汉字
            //string str = Encoding.GetEncoding("GBK").GetString(showData);
            string v = Encoding.GetEncoding("GBK").GetString(showData);
            MyStringBuilder.Append(v);//【20200428】优化效率
            //右侧更新提示区
            //【20191024】删除原来超过220行自动清除文本框的功能，原本清除文本框功能会影响事件响应
            //this.txt_updateinfo1.AppendText(str);
            this.txt_updateinfo1_1.AppendText(MyStringBuilder.ToString());//【20200428】优化效率
            MyStringBuilder.Clear();

            if(v== "主从机连接已断开，请重新操作\r\n")
                   button1_1.Enabled = false;

            this.txt_updateinfo1_1.SelectionStart = this.txt_updateinfo1_1.Text.Length;  //光标指向最后一位
            this.txt_updateinfo1_1.ScrollToCaret();   //移动到光标处

        }

        //======================================================================
        //函数名称：txtShow1
        //函数返回：无
        //参数说明：str:在终端执行信息提示框追加显示的内容
        //功能概要：整体更新过程中，在终端执行信息提示框txt_uartinfo1中追加提示
        //          信息，并显示最新信息      
        //======================================================================
        private void txtShow1(string str)
        {
            //右侧更新提示区
            if (this.txt_updateinfo1_1.Lines.Length >= 220) this.txt_updateinfo1_1.Text = String.Empty; //长度过长则清空内容
            this.txt_updateinfo1_1.Text += str;
            this.txt_updateinfo1_1.Refresh();
            this.txt_updateinfo1_1.SelectionStart = this.txt_updateinfo1_1.Text.Length;  //光标指向最后一位
            this.txt_updateinfo1_1.ScrollToCaret();   //移动到光标处

        }

        //======================================================================
        //函数名称：bytesToStruct
        //函数返回：byte数组转换为对应的结构体
        //参数说明：bytes:字节数组;type:结构体类型
        //功能概要：将byte字节数组数据转换为对应的结构体数据
        //======================================================================
        private object bytesToStruct(byte[] bytes, Type type)
        {
            //（1）变量声明
            int size;
            object obj;
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

        ////======================================================================
        ////函数名称：SetTextSafePost
        ////函数返回：无
        ////参数说明：data:待显示数据的二进制形式
        ////功能概要：将接收到的调试数据显示到文本控件上的异步操作
        ////======================================================================
        //private void SetTextSafePost(object data)
        //{
        //    //显示调试信息
        //    if (infor_dispaly_button.Text == "暂停")
        //        //txtShow1(Encoding.Default.GetString((byte[])data)); //右侧更新提示区
        //        txtShow1(Encoding.GetEncoding("ANSI").GetString((byte[])data)); //右侧更新提示区
        //    message_count++;
        //    Application.DoEvents();
        //}

        //======================================================================
        //函数名称：DataRecv
        //函数返回：无
        //参数说明：data:待显示数据的二进制形式
        //功能概要：执行异步操作，将接收到的调试数据显示到文本控件上
        //======================================================================
        private void DataRecv(byte[] data)
        {
            //如果正在执行关闭操作
            if (closing)
            {
                return;
            }
            try
            {

                byte[] showData = data;

                message_count++;
                listening = true;    //开始处理数据，可能会用到多线程
                /*[20200505]
                if (this.txt_updateinfo1.InvokeRequired)
                {
                    this.txt_updateinfo1.Invoke(new Action(() => { txtShow_rece_data(showData); }));
                }
                else
                */

                txtShow_rece_data(showData);

                emuart.bufferClear();
                Application.DoEvents();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                listening = false;
            }
        }

        //======================================================================
        //函数名称：infor_dispaly_button_Click
        //函数返回：无
        //参数说明：无
        //功能概要：暂停或启动右侧提示区的数据显示
        //======================================================================
        private void infor_dispaly_button_Click(object sender, EventArgs e)
        {
            if (infor_dispaly_button_1.Text == "暂停")
            {
                infor_dispaly_button_1.Text = "继续";
                emuart.DataReceivedEvent -= new EMUART.DataReceived(DataRecv);//解除事件绑定的方法,暂停数据接收
            }

            else
            {
                infor_dispaly_button_1.Text = "暂停";
                emuart.DataReceivedEvent += new EMUART.DataReceived(DataRecv);//解除事件绑定的方法,开始数据接收
            }

        }

        //======================================================================
        //函数名称：infor_dispaly_button_Click
        //函数返回：无
        //参数说明：无
        //功能概要：1秒钟超过50条数据显示则自动停止右侧提示区的显示
        //======================================================================
        public void mes_count()
        {
            while (true)
            {
                if (message_count >= 100)
                {
                    infor_dispaly_button_1.Text = "继续";
                    emuart.DataReceivedEvent -= new EMUART.DataReceived(DataRecv);//解除事件绑定的方法,暂停数据接收
                }
                message_count = 0;
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frm_UserUpdate_FormClosed(object sender, FormClosedEventArgs e)
        {
            infor_dispaly_button_Click(sender, e);
            btnCloseUpdate_Click(sender, new EventArgs());
        }

        /// <summary>
        /// 退出串口更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCloseUpdate_Click(object sender, EventArgs e)
        {
            infor_dispaly_button_Click(sender, e);
            if (emuart._Uartport != null)
            {
                emuart.DataReceivedEvent -= new EMUART.DataReceived(DataRecv);
                closing = true;
                while (listening)
                {
                    Application.DoEvents();
                }
                // emuart._Uartport.Dispose();
                emuart._Uartport.Close();

            }
            this.Close();
            Thread.Sleep(10);
            closing = false;
        }

        /// <summary>
        /// 获取工程下所有文件
        /// </summary>
        /// <param name="path">工程路径</param>
        private void GetAllFiles(string path, List<FileInfo> files)
        {
            if (path == null)
            {
                return;
            }
            DirectoryInfo folder = new DirectoryInfo(path);
            FileInfo[] childFiles = folder.GetFiles("*.*");    //文件列表
            foreach (FileInfo childFile in childFiles)
            {
                files.Add(childFile);
            }
            DirectoryInfo[] chldFolders = folder.GetDirectories();    //文件夹列表
            foreach (DirectoryInfo chldFolder in chldFolders)
            {
                GetAllFiles(chldFolder.FullName, files);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //（1）变量声明
            int sum;          //发送总帧数
            int cur;          //当前发送帧号
            byte[] senddata = null;
            byte[] recvdata = null;
            int flag;         //（VA.10之前）更新标志位
            byte[] shake = { (byte)10, (byte)'s', (byte)'h', (byte)'a', (byte)'k', (byte)'e' };  //握手帧
            byte[] startUpdate = { (byte)11, (byte)'u', (byte)'p', (byte)'d', (byte)'a', (byte)'t', (byte)'e' }; //开始更新提示帧
            byte[] zero = new byte[500];
            byte[] bRet = null;
            byte[] data = null;
            byte[] reStartFrame = new byte[3];

            Application.DoEvents();     //【20191024】与重新连接部分原理相同

            button1_1.Enabled = false;

            //（2）如果更新程序版本为VA.10之后的版本
            if (softVersion == 2)
            {
                //（2.1）若未导入Hex文件或串口连接失败则退出（防错用）
                if (hex.getHexList().Count == 0) goto btn_autoupdate1_Click_EXIT1;//未导入Hex文件
                if (emuart == null || emuart.haveUE == false) goto btn_autoupdate1_Click_EXIT2;//串口连接失败
                //（2.2）开始更新
                update = new Update(overallStyle, hex.getHexList());  //update初始化
                sum = update.getFrameNum();
                try
                {
                    txtShow1("运行状态：整体更新开始\r\n");     //右侧更新提示区  
                    emuart.DataReceivedEvent -= new EMUART.DataReceived(DataRecv);//解除事件绑定的方法，加快握手速度
                    infor_dispaly_button_1.Text = "暂停";
                    emuart.DataReceivedEvent += new EMUART.DataReceived(DataRecv);//串口事件绑定
                    //（2.2.1）从update类中取出待发送的数据帧，逐一进行发送
                    while ((senddata = update.getNextFrame()) != null)
                    {
                        cur = update.getNextIndex();
                        //非最后的更新命令帧，需返回数据
                        if (cur != sum - 1)
                        {
                            //第26帧做特殊处理，防止延迟
                            if (cur == 25)
                            {
                                int aaa = 10;
                            }
                            if (!emuart.send(senddata, out recvdata, 10000, 3)) goto btn_autoupdate1_Click_EXIT5;  //未接收到正确返回信息
                            if (update.updateRecv(recvdata) != 0) goto btn_autoupdate1_Click_EXIT6;  //更新返回帧异常
                            //当前帧数据操作成功，进度条显示更新进度
                            this.prg_update1_1.Value = (cur + 1) * 100 / sum;            //进度条显示
                            this.lbl_progressbar1.Text = (cur + 1) * 100 / sum + "%";  //进度百分比显示
                            this.prg_update1_1.Refresh();
                            //成功提示
                            txtShow1("当前第" + (cur + 1).ToString() + "/" + sum.ToString() + "帧 \r\n");     //右侧更新提示区  

                        }
                        //最后一帧更新命令帧，仅需发送
                        else
                        {
                            txtShow1("当前第" + (cur + 1).ToString() + "/" + sum.ToString() + "帧 \r\n");     //右侧更新提示区
                            txtShow1("程序整体更新成功\r\n");     //右侧更新提示区  
                            this.prg_update1_1.Value = 100;
                            this.lbl_progressbar1.Text = "100%";
                            this.lbl_progressbar1.Refresh();

                            emuart.send(senddata, out recvdata, 0, 1); //令发送等待时间为0 20190516（1/2）

                            
                            System.Threading.Thread.Sleep(100); //延时100毫秒
                            txtShow1("主机接受更新程序成功。\r\n");     //右侧更新提示区  
                            txtShow1("主机开始向从机发送更新数据。\r\n");
                            this.button1_1.Enabled = true;
                            return;
                        }
                    }
                    //（2.3）更新成功 
                    goto btn_autoupdate1_Click_EXIT3;
                }
                catch (Exception ex)
                {
                    txtShow1("系统异常退出\r\n" + ex + "\r\n");     //右侧更新提示区  
                    goto btn_autoupdate1_Click_EXIT;
                }
            }
            //（3）如果更新程序版本为VA.10及其之前的版本
            else if (softVersion == 1)
            {
                //（3.1）若未导入Hex文件或串口连接失败则退出（防错用）
                flag = 0;
                if (hex_old.getHexList() == null) goto btn_autoupdate1_Click_EXIT1;    //未导入Hex文件
                if (emuart == null || emuart.haveUE == false) goto btn_autoupdate1_Click_EXIT2;    //串口连接失败
                //（3.2）显示区初始化
                txtShow1("");     //右侧更新提示区  
                //（3.3）Hex文件选择成功、串口选择成功则进行自动更新操作//右下提示区
                update_old = new Update_old(hex_old.getHexList());    //新的更新数据列表
                try
                {
                    //（3.3.1）通过串口向MCU发送开始更新提示startUpdate,即11, update;
                    if (!(emuart.send(startUpdate, out bRet, 300, 2) && bRet != null
                        && Encoding.Default.GetString(bRet) == "Start Update")) goto btn_autoUpdate1_Click_EXIT8;
                    txtShow1("运行状态：收到UE握手信息，将开始程序更新操作...\r\n");                    //右侧更新提示区
                    emuart.DataReceivedEvent -= new EMUART.DataReceived(DataRecv);//解除事件绑定的方法，加快握手速度
                    infor_dispaly_button_1.Text = "暂停";
                    emuart.DataReceivedEvent += new EMUART.DataReceived(DataRecv);//解除事件绑定的方法，加快握手速度                   
                    //（3.3.2）更新操作初始化
                    sum = update_old.getFrameNum();      //sum←待发送的总帧数
                    reStartFrame[0] = 0x04;          //返回帧的帧头reStartFrame[0]←0x04
                    //（3.3.3）发送第一帧数据，需等待较长时间
                    for (int i = update_old.getCurCount(); i < sum; i++)
                    {
                        data = update_old.getNextFrame();    //data←一帧数据
                        if (i == 0) { Array.Copy(data, 1, reStartFrame, 1, 2); }; // 若是第0帧data变动
                        //第26帧做特殊处理，防止延迟
                        if (i == 26)
                        {
                            int aaa = 10;
                        }
                        //发送数据，并等待接收返回信息
                        //(几种错误情况)
                        if (!emuart.send(data, out bRet, 500, 2)) goto btn_autoUpdate1_Click_EXIT9;   //未正确返回信息
                        if (bRet[0] == (byte)2 && bRet[1] != 0) goto btn_autoUpdate1_Click_EXIT10;  //UE接收数据帧出错
                        if ((bRet[0] == 1) && ((int)bRet[2] * 256 + bRet[1] != i)) goto btn_autoUpdate1_Click_EXIT11;  //UE接收到的帧数有误
                        //（正确情况）进度条显示更新进度                      
                        this.prg_update1_1.Value = update_old.getSendCount() * 100 / sum;     //进度条显示       
                        this.lbl_progressbar1.Text = update_old.getSendCount() * 100 / sum + "%";//进度条百分比显示 
                        this.lbl_progressbar1.Refresh();
                        //成功提示
                        txtShow1("当前第" + (i + 1).ToString() + "/" + sum.ToString() + "帧  " + bRet[0].ToString() + "\r\n");     //右侧更新提示区  

                    }
                    //（3.4）程序更新成功
                    goto btn_autoupdate1_Click_EXIT4;
                }
                catch (Exception ex)
                {
                    txtShow1("系统异常退出\r\n" + ex);     //右侧更新提示区  
                    goto btn_autoUpdate1_Click_EXIT7;
                }
            }


        //（4）退出区
        //（4.1）退出函数
        btn_autoupdate1_Click_EXIT:
            return;

        //（4.2）Hex文件未选择
        btn_autoupdate1_Click_EXIT1:
            goto btn_autoupdate1_Click_EXIT;

            //（4.3）未连接设备
        btn_autoupdate1_Click_EXIT2:
            goto btn_autoupdate1_Click_EXIT;

            //（4.4）程序整体更新成功
        btn_autoupdate1_Click_EXIT3:
            goto btn_autoupdate1_Click_EXIT;

            //（4.5）（VA.10之前）程序更新成功
        btn_autoupdate1_Click_EXIT4:
            txtShow1("程序更新成功！,请等待节点系统重启后，绿色电源指示灯亮后再断电\r\n");     //右侧更新提示区  
            this.prg_update1_1.Value = 100;
            goto btn_autoUpdate1_Click_EXIT7;

         //（4.6）未接收到终端返回数据
        btn_autoupdate1_Click_EXIT5:
            txtShow1("错误提示：未接收到终端返回数据\r\n");     //右侧更新提示区  
            goto btn_autoupdate1_Click_EXIT;

         //（4.7）接收到的更新返回数据异常
        btn_autoupdate1_Click_EXIT6:
            txtShow1("错误提示：接收到的更新返回数据异常\r\n");     //右侧更新提示区  
            goto btn_autoupdate1_Click_EXIT;

        //（4.8）（VA.10之前）程序更新操作结束，重新允许导入新的Hex文件、切换更新方式、重连串口等操作
        btn_autoUpdate1_Click_EXIT7:
            this.btn_fileopen1_1.Enabled = true;
            return;

        //以下都是VA.10版本之前的更新错误处理
        //（4.9）与UE握手失败
        btn_autoUpdate1_Click_EXIT8:
            txtShow1("错误提示1：与UE握手失败！\r\n");     //右侧更新提示区  
            flag = 1;    //更新失败
            goto btn_autoUpdate1_Click_EXIT7;

        //（4.10）更新过程未收到返回信息
        btn_autoUpdate1_Click_EXIT9:
            txtShow1("错误提示2：更新过程未收到返回信息！\r\n");     //右侧更新提示区  
            flag = 1;    //更新失败
            goto btn_autoUpdate1_Click_EXIT7;

        //（4.11）UE告知接收数据帧有误
        btn_autoUpdate1_Click_EXIT10:
            txtShow1("错误提示3：UE告知接收数据帧有误！\r\n");     //右侧更新提示区  
            flag = 1;    //更新失败
            goto btn_autoUpdate1_Click_EXIT7;

        //（4.12）UE返回的帧数不对
        btn_autoUpdate1_Click_EXIT11:
            txtShow1("错误提示4：UE返回的帧数不对！(" + ((int)bRet[2] * 256 + bRet[1]).ToString() + ")\r\n");//右侧更新提示区
            flag = 1;    //更新失败
            goto btn_autoUpdate1_Click_EXIT7;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {

            //【1】变量声明
            int ret;            //返回值
            string com = "";    //串口信息
            string sTemp = "";
            byte[] recv = null;//保存串口接收信息
            byte[] host_link = { (byte)12, (byte)'h', (byte)'o', (byte)'s', (byte)'t' };
            //byte[] shake = { (byte)10, (byte)'s', (byte)'h', (byte)'a', (byte)'k', (byte)'e' }; //与终端握手帧数据
            int Hardaddress;
            string SciInfo = comboBox2.Text;    //获取选中的串口：自动更新/具体的串口号
            byte[] imei = BitConverter.GetBytes(int.Parse(textBox2.Text));      //目标从机的软件地址

            //【2】清除一些可能余留信息
            this.lbl_uartstate_1.Text = "";       //右上角显示区
            this.lst_codeshow1_1.Items.Clear();   //左侧代码显示区
            this.txt_updateinfo1_1.Text = "";     //右侧更新提示区
            this.Refresh();                     //刷新显示  

            //txtShow1("PC节点正在搜索目标节点，请稍后...\r\n");     //右侧更新提示区  
            //【3】重新遍历串口，寻找终端
            if (emuart._Uartport != null)
            {
                //emuart._Uartport.Dispose();
                //【20191024】  (3.1)让耗时操作做完，如果不做完耗时操作，无法进行下面的事件，且界面不响应
                Application.DoEvents();
                emuart._Uartport.Close();
                //【20200401】 姜家乐 增加延时函数
                //Thread.Sleep(20);
            }
            Thread.Sleep(100);   //等待串口关闭
            emuart.DataReceivedEvent -= new EMUART.DataReceived(DataRecv);//解除事件绑定的方法，加快握手速度
            emuart = EMUART.getInstance();    //每次“重新连接”重新实例化

            if (SciInfo == "自动搜索")
            {
                bool search_com_auto = false;       //自动搜索串口成功标识
                bool ret_flag1 = true, ret_flag2 = true;
                Debug.Print(comboBox2.Items.Count + "");
                for (int i = 1; i < comboBox2.Items.Count; i++)      //每个串口轮询找过去，相当于一个个测试下面那个“手动选择”
                {
                    emuart = EMUART.getInstance();    //每次“重新连接”重新实例化
                    ret = emuart.LinkDevice(comboBox2.Items[i].ToString(), 115200);  //寻找emuart设备
                    Debug.Print(ret + "");
                    //if (ret == 1) { label2_1.Text = "未找到串口\r\n"; return; }
                    //if (ret == 2) { label2_1.Text = "找到串口，但是未找到设备\r\n"; return; }

                    if (ret == 1) { ret_flag1 = true; continue; }
                    ret_flag1 = false;
                    if (ret == 2) { ret_flag2 = true; continue; }
                    ret_flag2 = false;

                    com = comboBox2.Items[i].ToString();
                    this.lbl_uartstate_1.Text = com + "\r\n";       //右上角显示区

                    //向主机终端发送软件地址和版本号
                    byte[] host_link_data = new byte[7];
                    Array.Copy(host_link, 0, host_link_data, 0, 5);
                    Array.Copy(imei, 0, host_link_data, 5, 2);

                    emuart.bufferClear();   //清除接收数组缓冲区
                    emuart.send(host_link_data, out recv, 100, 3);

                    if (recv == null)
                    {
                        Debug.Print("test1");
                        continue;
                        //this.label2_1.Text += "未收到返回数据\r\n";
                        //goto btn_uartcheck_Click_EXIT;//未收到数据，退出 
                    }

                    if (recv != null && recv.Length == 13)
                    {
                        Debug.Print("test2");
                        continue;
                    }

                    if (recv.Length == 14 && recv != null)
                    {
                        Debug.Print("test5");
                        Hardaddress = recv[10];
                        Hardaddress += recv[11] << 8;
                        Hardaddress += recv[12] << 16;
                        Hardaddress += recv[13] << 24;
                        //"该节点的硬件地址为：0x" + Convert.ToString(Hardaddress, 16);
                        this.txtShow1("硬件地址为：0x" + Convert.ToString(Hardaddress, 16) + "的PC节点正在搜索目标节点，请稍后...\r\n");     //右侧更新提示区
                        search_com_auto = true;
                        break;
                    }
                }
                Debug.Print("11111111");
                if (!search_com_auto)
                {
                    Debug.Print("22222");
                    //this.lbl_uartstate_1.Text = "连接失败";
                    if (ret_flag1) goto btn_uartcheck_Click_EXIT1;
                    if (ret_flag2) goto btn_uartcheck_Click_EXIT2;
                }
                if (search_com_auto)
                    Debug.Print("33333");
            }
            else    //手动选择串口
            {
                //Debug.Print("手动选择");
                emuart = EMUART.getInstance();    //每次“重新连接”重新实例化
                ret = emuart.LinkDevice(SciInfo, 115200);  //寻找emuart设备

                if (ret == 1) { LinkSlave = false; goto btn_uartcheck_Click_EXIT1; }
                if (ret == 2) { LinkSlave = false; goto btn_uartcheck_Click_EXIT2; }

                com = SciInfo;
                this.lbl_uartstate_1.Text = com + "\r\n";       //右上角显示区

                //向主机终端发送软件地址和版本号
                byte[] host_link_data = new byte[7];
                Array.Copy(host_link, 0, host_link_data, 0, 5);
                Array.Copy(imei, 0, host_link_data, 5, 2);

                emuart.bufferClear();   //清除接收数组缓冲区
                emuart.send(host_link_data, out recv, 100, 3);

                if (recv == null)
                {
                    Debug.Print("test1");
                    this.label2_1.Text += "未收到返回数据\r\n";
                    goto btn_uartcheck_Click_EXIT;//未收到数据，退出 
                }

                if (recv != null && recv.Length == 13)
                {
                    Debug.Print("test2");
                    if (recv[0] == '0')
                    {
                        Debug.Print("test3");
                        this.lbl_uartstate_1.Text += "该设备为从机，请选择主机连接";
                        return;
                    }
                    else if (recv[0] == '1')
                    {
                        Debug.Print("test4");
                        this.lbl_uartstate_1.Text += "该设备为类型错误，请重新设置";
                        return;
                    }
                }

                if (recv.Length == 14 && recv != null)
                {
                    Debug.Print("test5");
                    Hardaddress = recv[10];
                    Hardaddress += recv[11] << 8;
                    Hardaddress += recv[12] << 16;
                    Hardaddress += recv[13] << 24;
                    //"该节点的硬件地址为：0x" + Convert.ToString(Hardaddress, 16);
                    this.txtShow1("硬件地址为：0x" + Convert.ToString(Hardaddress, 16) + "的PC节点正在搜索目标节点，请稍后...\r\n");     //右侧更新提示区

                }
                //对返回信息进行判断
            }

            //while (true) ;     //打个断点先，这边现在主从机那边测试一下能不能连接上

            //验证输入的节点软件地址
            ////if(int.Parse(textBox1_1.Text) < 0 || int.Parse(textBox1_1.Text) > 10000)
            ////{
            ////    this.lbl_uartstate_1.Text = "输入的节点软件地址有误！";
            ////    return;
            ////}

            ////byte[] imei = BitConverter.GetBytes(int.Parse(textBox1_1.Text));

            //ret = emuart.findDevice(out com, 115200);       //寻找emuart设备
            //this.lbl_uartstate_1.Text = com;                    //右上角显示区

            //【4】根据寻找串口的返回值确定执行下列分支
            //【4.1】如果连接终端的emuart失败，退出函数
            //if (ret == 1) goto btn_uartcheck_Click_EXIT1;

            //【4.2】找到串口，没有找到UE，退出函数
            //else if (ret == 2) goto btn_uartcheck_Click_EXIT2;


            //向终端发送软件地址和版本号
            //byte[] TarData = new byte[11];
            //TarData[0] = (byte)12;
            ////Array.Copy(imei, 0, TarData, 1, 2);
            ////byte[] softver = System.Text.Encoding.Default.GetBytes(textBox3_1.Text);
            ////Array.Copy(softver, 0, TarData, 3, 8);

            //emuart.bufferClear();   //清除接收数组缓冲区
            //emuart.send(TarData, out recv, 1000, 3);

            //if (recv != null && recv.Length == 4)
            //{
            //    this.lbl_uartstate_1.Text = "握手被拒绝，终端软件版本已为最新版。";

            //    return;
            //}

            //Hardaddress = recv[10];
            //Hardaddress += recv[11] << 8;
            //Hardaddress += recv[12] << 16;
            //Hardaddress += recv[13] << 24;
            //"该节点的硬件地址为：0x" + Convert.ToString(Hardaddress, 16);
            //this.txtShow1("硬件地址为：0x" + Convert.ToString(Hardaddress, 16) +"的PC节点正在搜索目标节点，请稍后...\r\n");     //右侧更新提示区 

            //【4.3】如果找到串口与UE,向设备发送握手帧
            byte[] shake = { (byte)10, (byte)'s', (byte)'h', (byte)'a', (byte)'k', (byte)'e' }; //与终端握手帧数据
            emuart.bufferClear();   //清除接收数组缓冲区
            emuart.send(shake, out recv, 500, 3); //获得设备的信息在recv中
            this.btn_fileopen1_1.Enabled = true;     //允许选择文件功能
            if (recv == null) goto btn_uartcheck_Click_EXIT3;//未收到数据，退出   


            //【4.4】发送握手帧后，若收到设备返回数据，处理之
            //【4.4.1】如果终端的更新程序版本为VA.10以后的版本（通用于所有GEC芯片）
            int length1 = Marshal.SizeOf(typeof(shakeData));
            int length = Marshal.SizeOf(typeof(newshakeData));

            if (recv.Length == Marshal.SizeOf(typeof(shakeData)))
            {
                softVersion = 2;  //表示终端更新程序为A.10之后的通用版本
                //byte数组转结构体
                try
                {
                    shakedata = (shakeData)bytesToStruct(recv, typeof(shakeData));
                    //获取握手帧数据
                    uecomType = Encoding.Default.GetString(shakedata.uecomType).Replace("\0", "");
                    mcuType = Encoding.Default.GetString(shakedata.mcuType).Replace("\0", "");
                    uStartAds = shakedata.uStartAds;    //User程序起始地址
                    uCodeSize = shakedata.uCodeSize;
                    replaceNum = shakedata.replaceNum;
                    reserveNum = shakedata.reserveNum;
                    //sectorStart = uStartAds / uCodeSize;    //BIOS 中读取的用户程序扇区号
                }
                catch (Exception)
                {
                    throw;
                }
                //设置设备信息                   
                sTemp = com + "：" + uecomType + " " + mcuType;  //设备信息
                //状态提示
                this.lbl_uartstate_1.Text = sTemp;     //右上角显示区
                this.txt_updateinfo1_1.Text = sTemp + "\r\n";     //右侧更新提示区     
                //若保留更新最大字节为0表示目前没有User程序，此时只能进行整体更新

                if (reserveNum == 0)
                {
                    txtShow1("可进行GEC中User程序更新\r\n");     //右侧更新提示区  
                }
                //如果存在串口，则允许接收调试信息
                if (this.emuart.haveUE)
                {
                    //绑定串口结束事件，处理串口调试信息
                    emuart.DataReceivedEvent += new EMUART.DataReceived(DataRecv);
                }
            }

            //【4.4.2】如果终端的更新程序版本为VA.10及其以前的版本
            else if (System.Text.Encoding.Default.GetString(recv).Contains("shake:GEC-"))
            {
                softVersion = 1;   //表示终端更新程序为A.10及之前的版本
                sTemp = "MKL36Z64" + "(" + com + ")";//设备信息
                this.lbl_uartstate_1.Text = "成功连接" + sTemp;       //右上角显示区
                this.lbl_uartstate_1.Text = "找到终端设备" + sTemp;   //右上角显示区

            }
            else if (recv.Length == Marshal.SizeOf(typeof(newshakeData)))
            {
                Debug.Print("newShakeData");
                softVersion = 2;  //表示终端更新程序为A.10之后的通用版本
                //byte数组转结构体
                try
                {
                    newshakedata = (newshakeData)bytesToStruct(recv, typeof(newshakeData));
                    //获取握手帧数据
                    uecomType = Encoding.Default.GetString(newshakedata.uecomType).Replace("\0", "");
                    mcuType = Encoding.Default.GetString(newshakedata.mcuType);
                    uStartAds = newshakedata.uStartAds;    //User程序起始地址
                    uCodeSize = newshakedata.uCodeSize;
                    replaceNum = newshakedata.replaceNum;
                    reserveNum = newshakedata.reserveNum;
                    //【20200715 1/2】 获取结构体中的BIOS版本信息
                    biosVersion = Encoding.Default.GetString(newshakedata.BIOSVersion);
                    //sectorStart = uStartAds / uCodeSize;    //BIOS 中读取的用户程序扇区号
                }
                catch (Exception)
                {
                    throw;
                }

                //若保留更新最大字节为0表示目前没有User程序，此时只能进行整体更新
                if (reserveNum == 0)
                {

                    byte[] linkData = new byte[1];
                    linkData[0] = (byte)13;
                    ////Array.Copy(imei, 0, TarData, 1, 2);


                    emuart.bufferClear();   //清除接收数组缓冲区
                    emuart.send(linkData, out recv, 500, 8);
                    if (recv == null) goto btn_uartcheck_Click_EXIT4;
                    if (Encoding.Default.GetString(recv).Equals("linkSuccess"))
                    {
                        //设置设备信息  
                        //【20200715 2/2】 将版本信息显示出来 
                        sTemp = com + "：" + " " + mcuType + biosVersion;  //设备信息
                                                                          //状态提示
                        this.lbl_uartstate_1.Text = sTemp;     //右上角显示区
                        this.txt_updateinfo1_1.Text = sTemp + "\r\n";     //右侧更新提示区     
                        txtShow1("可进行GEC中User程序更新\r\n");     //右侧更新提示区  
                        txtShow1("硬件地址为：" + textBox1.Text + "软件地址为：" + textBox2.Text + "节点连接成功，可进行远程更新\r\n");
                    }
                    else
                    {
                        goto btn_uartcheck_Click_EXIT4;
                    }
                    //if (recv == null) //未收到数据，退出  
                }
                //如果存在串口，则允许接收调试信息
                if (this.emuart.haveUE)
                {
                    //绑定串口结束事件，处理串口调试信息
                    emuart.DataReceivedEvent += new EMUART.DataReceived(DataRecv);
                }
            }

            //【4.4.3】若收到错误返回信息，退出函数
            else goto btn_uartcheck_Click_EXIT3;

         //【5】退出区
        // 【5.1】退出函数
        btn_uartcheck_Click_EXIT:
            return;

        //【5.2】不存在可用串口
        btn_uartcheck_Click_EXIT1:
            this.btn_fileopen1_1.Enabled = false;      //禁止选择文件功能
            this.lbl_uartstate_1.Text = "当前不存在可用串口";       //右上角显示区
            this.txt_updateinfo1_1.Text = "没有找到USB串口，可能原因如下：\r\n（1）USB串口未插上PC\r\n（2）PC未安装串口驱动\r\n";     //右侧更新提示区
            closing = true;     //正在关闭串口
            while (listening)
            {
                Application.DoEvents();
            }
            if (emuart._Uartport != null)
            {
                emuart._Uartport.Close();
            }
            closing = false;    //关闭完成
            goto btn_uartcheck_Click_EXIT;

        //【5.3】存在串口，但不存在emuar设备
        btn_uartcheck_Click_EXIT2:
            emuart.terminate(9600); //发送数据给终端设备，让终端设备清空其数据缓冲区
            this.lbl_uartstate_1.Text = "已连接串口" + com + "但未找到设备";       //右上角显示区
            //右侧更新提示区
            this.txt_updateinfo1_1.Text = "有USB串口，但未连上终端，可能原因如下：\r\n（1）USB串口驱动需更新\r\n（2）USB串口未连接终端\r\n（3）终端程序未运行\r\n";
            closing = true;     //正在关闭串口
            while (listening)
            {
                Application.DoEvents();
            }
            emuart._Uartport.Close();
            closing = false;    //关闭完成
            goto btn_uartcheck_Click_EXIT;

        //【5.4】未收到返回信息或收到错误返回信息
        btn_uartcheck_Click_EXIT3:
            //emuart.terminate(9600); //发送数据给终端设备，让终端设备清空其数据缓冲区
            this.lbl_uartstate_1.Text = "找到PC节点在" + com + "但握手失败，请再次单击[重新连接]按钮";  //右上角显示
            closing = true;     //正在关闭串口
            while (listening)
            {
                Application.DoEvents();
            }
            //【20191205 1/2】  程宏玉   解决无串口连接时单击连接GEC按钮，出现异常问题
            if (emuart._Uartport != null)
            {
                try
                {
                    emuart._Uartport.Close();
                }
                catch (Exception)
                {

                }
            }
            closing = false;    //关闭完成
            goto btn_uartcheck_Click_EXIT;

         btn_uartcheck_Click_EXIT4:
            this.btn_fileopen1_1.Enabled = false;
            //emuart.terminate(9600); //发送数据给终端设备，让终端设备清空其数据缓冲区
            ////this.lbl_uartstate_1.Text = "找到PC节点在" + com + "且握手成功，但连接"+ textBox1_1.Text + "节点失败，请再次单击[重新连接]按钮";  //右上角显示
            this.txt_updateinfo1_1.Text = "连接失败可能原因如下：\r\n（1）待更新节点软件地址错误\r\n（2）节点无线收发出错\r\n（3）终端程序未运行\r\n";
            //closing = true;     //正在关闭串口
            //while (listening)
            //{
            //    Application.DoEvents();
            //}
            ////【20191205 1/2】  程宏玉   解决无串口连接时单击连接GEC按钮，出现异常问题
            //if (emuart._Uartport != null)
            //{
            //    try
            //    {
            //        emuart._Uartport.Close();
            //    }
            //    catch (Exception)
            //    {

            //    }
            //}
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //【1】变量声明
            int ret;            //返回值
            //int Softaddress;
            //int Hardaddress;
            string com = "";    //串口信息
            //string sTemp = "";  //更新用的MCU信息
            byte[] recv = null;//保存串口接收信息
            byte[] shake = { (byte)10, (byte)'s', (byte)'l', (byte)'a', (byte)'v', (byte)'e', (byte)'-', (byte)'b', (byte)'l', (byte)'e', (byte)'-', (byte)'s', (byte)'h', (byte)'a', (byte)'k', (byte)'e' }; //与终端握手帧数据
            //byte[] askaddress = { (byte)11, (byte)'A', (byte)'s', (byte)'k', (byte)'A', (byte)'d', (byte)'d', (byte)'r', (byte)'e', (byte)'s', (byte)'s' };
            //byte[] imei = null;   //这个变量好像定义了没用到过
            string SciInfo = comboBox1.Text;     //获取选中的串口：自动更新/具体的串口号
            //【2】清除一些可能余留信息
            this.label2_1.Text = "";       //右上角显示区
            this.Refresh();                     //刷新显示 

            Regex regex = new System.Text.RegularExpressions.Regex("^0x[0-9A-Fa-f]{1,8}$");
            bool b = regex.IsMatch(textBox1.Text);

            //20210511 WTC
            //验证输入的节点硬件地址
            if (b == false)
            {
                this.label2_1.Text = "输入的节点硬件地址有误！要求输入十六进制数\r\n";
                return;
            }
            //验证输入的节点软件地址
            if (int.Parse(textBox2.Text) < 0 || int.Parse(textBox2.Text) > 10000)
            {
                this.label2_1.Text = "输入的节点软件地址有误！\r\n";
                return;
            }

            //【3】重新遍历串口，寻找终端
            if (emuart != null && emuart._Uartport != null)
            {
                //emuart._Uartport.Dispose();
                //【20191024】  (3.1)让耗时操作做完，如果不做完耗时操作，无法进行下面的事件，且界面不响应
                Application.DoEvents();
                emuart._Uartport.Close();
                //【20200401】 姜家乐 增加延时函数

            }
            //Thread.Sleep(100);   //等待串口关闭
            emuart.DataReceivedEvent -= new EMUART.DataReceived(DataRecv);//解除事件绑定的方法，加快握手速度
            if (emuart != null) emuart = null;

            if (SciInfo == "自动搜索")
            {
                bool search_com_auto = false;       //自动搜索串口成功标识
                Debug.Print(comboBox1.Items.Count + "");
                for(int i = 1; i < comboBox1.Items.Count; i++)      //每个串口轮询找过去，相当于一个个测试下面那个“手动选择”
                {   
                    //Debug.Print("自动连接");
                    emuart = EMUART.getInstance();    //每次“重新连接”重新实例化
                    ret = emuart.LinkDevice(comboBox1.Items[i].ToString(), 115200);  //寻找emuart设备,这个finddDevice1干嘛的，功能不适合findDecvice一样
                    //this.label2_1.Text = comboBox1.Items[i].ToString();       //右上角显示区
                    //Debug.Print(comboBox1.Items[i].ToString());
                    //Debug.Print("手动选择");
                    emuart = EMUART.getInstance();    //每次“重新连接”重新实例化
                    ret = emuart.LinkDevice(comboBox1.Items[i].ToString(), 115200);  //寻找emuart设备
                    //Debug.Print(ret + "");
                    //if (ret == 1) { label2_1.Text = "未找到串口\r\n"; return; }
                    //if (ret == 2) { label2_1.Text = "找到串口，但是未找到设备\r\n"; return; }

                    if (ret == 1) continue;
                    if (ret == 2) continue;

                    com = SciInfo;
                    this.label2_1.Text = com + "\r\n";       //右上角显示区

                    byte[] TarData = new byte[24];  //发送给下位机-从机的数据  “shake-ble(硬件地址)(软件地址)(主机从机)”
                    byte[] haddr = BitConverter.GetBytes(Convert.ToInt32(textBox1.Text, 16));
                    byte[] saddr = BitConverter.GetBytes(int.Parse(textBox2.Text));
                    Array.Copy(shake, 0, TarData, 0, 16);        //“shake-ble”
                    Array.Copy(haddr, 0, TarData, 16, 4);        //“12345678”
                    Array.Copy(saddr, 0, TarData, 20, 2);       //“01”
                    TarData[22] = (byte)0;                      //从机标识
                    TarData[23] = (byte)1;
                    emuart.bufferClear();   //清除接收数组缓冲区
                    emuart.send(TarData, out recv, 100, 3);
                    //Debug.Print("test1");
                    if (recv == null)
                    {
                        //Debug.Print("test2");
                        //this.label2_1.Text += "未收到返回数据\r\n";
                        //goto btn_uartcheck_Click_EXIT;//未收到数据，退出 
                        continue;
                    }

                    if (recv.Length == 9 && recv != null)
                    {
                        //Debug.Print("recv_length_1\r\n");
                        //Debug.Print(recv.Length + "");
                        //Debug.Print("recv_length_2\r\n");
                        //Debug.Print("test3");
                        if (recv[0] == '1')
                        {
                            //Debug.Print("test4");
                            //this.label2_1.Text += "硬件地址不匹配，请重新确认设备硬件地址！\r\n";
                            //return;
                            continue;
                        }
                        else if (recv[0] == '2')
                        {
                            //Debug.Print("test5");
                            //this.label2_1.Text += "软件地址不匹配，请重新确认设备软件地址！\r\n";
                            //return;
                            continue;
                        }
                        else if (recv[0] == '3')
                        {
                            //Debug.Print("test6");
                            //this.label2_1.Text += "目标设备并非从机，请重新确认设备类型！\r\n";
                            //return;
                            continue;
                        }
                        else if (recv[0] == '0')
                        {
                            search_com_auto = true;
                            LinkSlave_COM = comboBox1.Items[i].ToString();
                            LinkSlave = true;
                            //Debug.Print("test7");
                            this.label2_1.Text = LinkSlave_COM + "连接成功\r\n";
                            this.label2_1.Text += "该从机硬件地址为：" + textBox1.Text + "  ，软件地址为：" + textBox2.Text + "\r\n";
                            //LinkSlave_COM = SciInfo;
                            //Debug.Print(LinkSlave_COM);
                            //return;
                            break;
                        }

                    }
                }
                if (!search_com_auto)
                {
                    this.label2_1.Text = "连接失败";
                    LinkSlave = false;
                    return; 
                }
                return;
            }
            else    //手动选择串口
            {
                //Debug.Print("手动选择");
                emuart = EMUART.getInstance();    //每次“重新连接”重新实例化
                ret = emuart.LinkDevice(SciInfo, 115200);  //寻找emuart设备

                if (ret == 1) { label2_1.Text = "未找到串口\r\n"; LinkSlave = false; return; }
                if (ret == 2) { label2_1.Text = "找到串口，但是未找到设备\r\n"; LinkSlave = false; return; }

                com = SciInfo;
                this.label2_1.Text = com + "\r\n";       //右上角显示区

                byte[] TarData = new byte[24];  //发送给下位机-从机的数据  “shake-ble(硬件地址)(软件地址)(主机从机)”
                byte[] haddr = BitConverter.GetBytes(Convert.ToInt32(textBox1.Text, 16));
                byte[] saddr = BitConverter.GetBytes(int.Parse(textBox2.Text));
                Array.Copy(shake, 0, TarData, 0, 16);        //“shake-ble”
                Array.Copy(haddr, 0, TarData, 16, 4);        //“12345678”
                Array.Copy(saddr, 0, TarData, 20, 2);       //“01”
                TarData[22] = (byte)0;                      //从机标识
                TarData[23] = (byte)1;
                emuart.bufferClear();   //清除接收数组缓冲区
                emuart.send(TarData, out recv, 100, 3);
                //Debug.Print("test1");
                if (recv == null)
                {
                    //Debug.Print("test2");
                    this.label2_1.Text += "未收到返回数据\r\n";
                    LinkSlave = false;
                    goto btn_uartcheck_Click_EXIT;//未收到数据，退出 
                }

                if (recv.Length == 9 && recv != null)
                {
                    //Debug.Print("recv_length_1\r\n");
                    //Debug.Print(recv.Length + "");
                    //Debug.Print("recv_length_2\r\n");
                    //Debug.Print("test3");
                    if (recv[0]=='1')
                    {
                        //Debug.Print("test4");
                        this.label2_1.Text += "硬件地址不匹配，请重新确认设备硬件地址！\r\n";
                        LinkSlave = false;
                        return ;
                    }
                    else if (recv[0] == '2')
                    {
                        //Debug.Print("test5");
                        this.label2_1.Text += "软件地址不匹配，请重新确认设备软件地址！\r\n";
                        LinkSlave = false;
                        return;
                    }
                    else if (recv[0] == '3')
                    {
                        //Debug.Print("test6");
                        this.label2_1.Text += "目标设备并非从机，请重新确认设备类型！\r\n";
                        LinkSlave = false;
                        return;
                    }
                    else if (recv[0]=='0')
                    {
                        //Debug.Print("test7");
                        this.label2_1.Text  = com + "连接成功\r\n";
                        this.label2_1.Text += "该从机硬件地址为：" + textBox1.Text + "  ，软件地址为：" + textBox2.Text + "\r\n";
                        LinkSlave_COM = SciInfo;
                        LinkSlave = true;
                        //Debug.Print(LinkSlave_COM);
                        return;
                    }

                }
                //对返回信息进行判断
            }

            emuart.bufferClear();   //清除接收数组缓冲区
            
        btn_uartcheck_Click_EXIT:
            this.label2_1.Text += "连接失败";
            LinkSlave = false;
            return;
        }

        private void comboBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int i;
            string[] SCIPorts;
            SCIPorts = System.IO.Ports.SerialPort.GetPortNames();
            this.comboBox1.Items.Clear();//首先清除现有的项
            this.comboBox1.Items.Add("自动搜索");
            for (i = 0; i < SCIPorts.Length; i++)
            {
                //向[串口选择框]中添加搜索到的串口号
                int add_index = this.comboBox1.Items.Add(SCIPorts[i]);
            }
            this.comboBox1.SelectedIndex = 0;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (!LinkSlave) return;

            //【1】变量声明
            int ret;            //返回值
            byte[] shake = { (byte)11, (byte)'w', (byte)'a', (byte)'i', (byte)'t', (byte)'-', (byte)'u', (byte)'p', (byte)'d', (byte)'a', (byte)'t', (byte)'e' }; //与终端握手帧数据
            byte[] recv = null;//保存串口接收信息

            Debug.Print(LinkSlave_COM);
            emuart = EMUART.getInstance();    //每次“重新连接”重新实例化
            ret = emuart.LinkDevice(LinkSlave_COM, 115200);  //寻找emuart设备

            if (ret == 1) { label2_1.Text = "未找到串口\r\n"; return; }
            if (ret == 2) { label2_1.Text = "找到串口，但是未找到设备\r\n"; return; }

            emuart.bufferClear();   //清除接收数组缓冲区
            emuart.send(shake, out recv, 500, 3);
            //Debug.Print("test1");
            if (recv == null)
            {
                //Debug.Print("test2");
                this.label2_1.Text += "未收到返回数据\r\n";
                goto btn_wait_update_Click_EXIT;//未收到数据，退出 
            }

            if (recv.Length == 2 && recv != null)
            {
                if (recv[0]=='O' && recv[1]=='K')
                {
                    emuart.bufferClear();   //清除接收数组缓冲区
                    this.label2_1.Text += LinkSlave_COM + "进入更新等待\r\n";
                    return;
                }
                else 
                    goto btn_wait_update_Click_EXIT;
            }
            //对返回信息进行判断
        btn_wait_update_Click_EXIT:
            emuart.bufferClear();   //清除接收数组缓冲区
            this.label2_1.Text += "发生错误，请重新连接\r\n";
            return;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!LinkSlave) return;

            //【1】变量声明
            int ret;            //返回值
            byte[] shake = { (byte)11, (byte)'e', (byte)'n', (byte)'d', (byte)'-', (byte)'w', (byte)'a', (byte)'i', (byte)'t' }; //与终端握手帧数据
            byte[] recv = null;//保存串口接收信息

            Debug.Print(LinkSlave_COM);
            emuart = EMUART.getInstance();    //每次“重新连接”重新实例化
            ret = emuart.LinkDevice(LinkSlave_COM, 115200);  //寻找emuart设备

            if (ret == 1) { label2_1.Text = "未找到串口\r\n"; return; }
            if (ret == 2) { label2_1.Text = "找到串口，但是未找到设备\r\n"; return; }

            emuart.bufferClear();   //清除接收数组缓冲区
            emuart.send(shake, out recv, 100, 3);
            //Debug.Print("test1");
            if (recv == null)
            {
                //Debug.Print("test2");
                this.label2_1.Text += "未收到返回数据\r\n";
                goto btn_wait_update_Click_EXIT;//未收到数据，退出 
            }

            if (recv.Length == 2 && recv != null)
            {
                if (recv[0] == 'O' && recv[1] == 'K')
                {
                    emuart.bufferClear();   //清除接收数组缓冲区
                    this.label2_1.Text = LinkSlave_COM + "退出更新等待\r\n";
                    return;
                }
                else
                    goto btn_wait_update_Click_EXIT;
            }
        //对返回信息进行判断
        btn_wait_update_Click_EXIT:
            emuart.bufferClear();   //清除接收数组缓冲区
            this.label2_1.Text += "发生错误，请重新连接\r\n";
            return;
        }

        private void comboBox2_MouseClick(object sender, MouseEventArgs e)
        {
            int i;
            string[] SCIPorts;
            SCIPorts = System.IO.Ports.SerialPort.GetPortNames();
            this.comboBox1.Items.Clear();//首先清除现有的项
            this.comboBox1.Items.Add("自动搜索");
            for (i = 0; i < SCIPorts.Length; i++)
            {
                //向[串口选择框]中添加搜索到的串口号
                int add_index = this.comboBox1.Items.Add(SCIPorts[i]);
            }
            this.comboBox1.SelectedIndex = 0;
        }
    }
}


