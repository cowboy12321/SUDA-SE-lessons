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

namespace AHL_GEC
{

    public partial class frm_UserUpdate : Form
    {
        public static frm_UserUpdate f1 = new frm_UserUpdate();
        //定义使用的全局变量
        private Hex hex;  //Hex文件信息，整体更新用

        private Hex User1_hex;  //Hex文件信息，整体更新用
        private Hex User2_hex;  //Hex文件信息，整体更新用
        private Hex User3_hex;  //Hex文件信息，整体更新用

        public static bool buttonOpen = false;
        public static bool Code_Update = false;                                               //是否处于更新状态

        private Update update;  //更新类，保存更新所使用的帧结构体与方法
        private SynchronizationContext m_SyncContext = null;    //用于安全地跨线程访问控件
        public static EMUART emuart;     //串口通信类对象
        private uint softVersion; //保存终端更新程序版本，0表示更新版本不详，1表示更新版本为VA.10之前，2表示更新版本为VA.10之后
        private Hex_old hex_old;  //（VA.10版本之前）更新程序Hex对象
        private Update_old update_old;  //（VA.10版本之前）更新类对象，保存更新所使用的帧结构体与方法
        private static UInt32 message_count;


        private List<FileInfo> fileList;    //当前工程文件列表

        private List<FileInfo> fileList_User1;    //当前工程文件列表
        private List<FileInfo> fileList_User2;    //当前工程文件列表
        private List<FileInfo> fileList_User3;    //当前工程文件列表

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
        Thread threadA;
        int FirstAddress, LastAddress;
        int MAX_COUNT = 3;
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
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] mcuType;            //MCU类型
            public uint uStartAds;            //User程序起始地址
            public uint uCodeSize;            //User程序总代码大小
            public uint replaceNum;           //替换更新最大字节
            public uint reserveNum;           //保留更新最大字节（不等于0意味着有User程序）
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] BIOSVersion;            //BIOS版本号
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
        //【20201012】
        private uint startAdd;            //代码起始地址
        //private readonly string[] LC_Num = {"1-1","2-1", "3-1" , "4-1",
        //                                    "1-2","2-2", "3-2" , "4-2",
        //                                    "1-3","2-3", "3-3" , "4-3",
        //                                    "1-4","2-4", "3-4" , "4-4",
        //                                    "1-5","2-5", "3-5" , "4-5",
        //                                    "1-6","2-6", "3-6" , "4-6",
        //                                    "1-7","2-7", "3-7" , "4-7" };
        private readonly string[] LC_Num = {"1-1","1-2","1-3", "1-4", "1-5", "1-6", "1-7",
                                            "2-1","2-2","2-3", "2-4", "2-5", "2-6", "2-7",
                                            "3-1","3-2","3-3", "3-4", "3-5", "3-6", "3-7",
                                            "4-1","4-2","4-3", "4-4", "4-5", "4-6", "4-7"};
        //======================================================================
        //函数名称：frm_UserUpdate
        //函数返回：frm_UserUpdate窗体构造函数
        //参数说明：无
        //功能概要：完成frm_UserUpdate窗体的初始化工作
        //======================================================================
       
        public frm_UserUpdate()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            f1 = this;


        }

        //======================================================================
        //函数名称：frm_UserUpdate_Load
        //函数返回：无
        //参数说明：无
        //功能概要：窗体加载事件，窗体加载时自动调用
        //======================================================================
        public void frm_UserUpdate_Load(object sender, EventArgs e)
        {
            //（1）变量声明
            //bool con_flag;
            //bool sflag;
            //int FirstAddress, LastAddress;
            //int CodeFlag;                                            //更新程序序号
            //int count = 1;
            //int UpdateCount;

            //CodeFlag = 1;

            //int Link_Count = 3;




            int flag1, flag2, flag3;
            string filePath1, filePath2, filePath3;
            string fileName1, fileName2, fileName3;          //文件名
            //string fileName1, fileName2, fileName3;          //文件名
            //filePath1 = "..\\..\\bin\\Debug\\Hex\\User1.hex";
            //filePath2 = "..\\..\\bin\\Debug\\Hex\\User2.hex";
            //filePath3 = "..\\..\\bin\\Debug\\Hex\\User3.hex";
            //fmain = (FrmMain)this.MdiParent;  //得到frmMain窗体变量
            m_SyncContext = SynchronizationContext.Current;   //用于安全地跨线程访问控件
            overallStyle = 0;    //更新方式为默认为整体更新方式
            message_count = 0;   //数据接收条数为0
            //（2）初始化Hex变量
            hex = new Hex();
            User1_hex = new Hex();
            User2_hex = new Hex();
            User3_hex = new Hex();

            //（3）初始化串口
            emuart = EMUART.getInstance();
            //（4）初始化版本号
            softVersion = 0;  //表示版本号不明
            //（5）初始化原更新程序变量           
            hex_old = new Hex_old();

            //flag1 = User1_hex.loadFile(filePath1);

            //（2.4）获取导入的文件信息
            fileName1 = User1_hex.getFileName();
            fileName2 = User2_hex.getFileName();
            fileName3 = User3_hex.getFileName();

            fileList_User1 = new List<FileInfo>();
            fileList_User2 = new List<FileInfo>();
            fileList_User3 = new List<FileInfo>();

            //状态提示
            this.User_filename1.Text = fileName1;


            this.prg_update1.Value = 0;
            this.lbl_progressbar1.Text = "";

            txtShow1("导入" + fileName1 + "文件成功！\r\n");     //右侧更新提示区  

            this.lst_codeshow2.Items.Clear();    //左侧代码显示区

            foreach (string str in User1_hex.getHexStrList())
            {
                this.lst_codeshow2.Items.Add(str);    //左侧代码显示区
            }

            Thread.Sleep(500);   //延时



            //(6)用Thread类
            threadStart = new ThreadStart(Update);//通过ThreadStart委托告诉子线程执行什么方法　　
            thread = new Thread(threadStart);
            thread.Start();//启动新线程
            threadA = new Thread(Auto_Update);
            this.button1.Enabled = false;
            //buttonOpen = true;

            //f1.Show();

            //this.button1_Click(null,null);
        }


        //======================================================================
        //函数名称：btn_fileOpen1_Click
        //函数返回：无
        //参数说明：无
        //功能概要：导入待整体更新Hex文件并对该文件进行解析取出其有效数据
        //======================================================================
        //private void btn_fileopen1_Click(object sender, EventArgs e)
        //{
        //    //（1）变量声明           
        //    int flag;

        //    uint codesize;            //代码大小

        //    string filePath;          //文件路径
        //    string fileName;          //文件名
        //    string line;              //Hex文件行数据
        //    List<hexStruct_old> list; //更新程序对应Hex文件保存变量            
        //    //（2）如果终端更新程序版本为VA.10之后的版本
        //    //if (softVersion == 2)
        //    //{
        //        //（2.1）导入Hex文件

        //    OpenFileDialog ofd = new OpenFileDialog();  //打开文件对话框
        //    ofd.Filter = "hex file(*.hex)|*.hex";
        //    textBox2.Clear();
        //    if (ofd.ShowDialog(this) == DialogResult.OK)
        //    {
        //            //（2.2）获取文件名
        //            filePath = ofd.FileName;
        //            //（2.3）导入解析Hex文件
        //            flag = hex.loadFile(filePath);
        //            if (flag != 0) goto btn_fileopen1_Click_EXIT1;
        //            //（2.4）获取导入的文件信息
        //            fileName = hex.getFileName();
        //            startAdd = hex.getStartAddress();    //hex文件中读取的起始地址
        //            codesize = hex.getCodeSize();
        //            //lengthDiffer = uStartAds - startAdd;    //起始长度差


        //            string ldFilePath = string.Empty;
        //            string userStart = string.Empty;      //被替换的起始地址
        //            string strStartAdd = string.Empty;    //替换的起始地址
        //            StreamReader sr = null;
        //            fileList = new List<FileInfo>();
        //            //如果打开工程，则先修改link文件中RAM的起始地址

        //            //（2.5）判断导入的Hex文件首地址是否正确
        //            //if (startAdd != uStartAds)
        //            //{
        //            //    //MessageBox.Show("当前User程序与BIOS程序版本不匹配");
        //            //    goto btn_fileopen1_Click_EXIT3;
        //            //}
        //            //（2.6）状态提示
        //            this.lbl_filename1.Text = fileName;
        //            this.prg_update1.Value = 0;
        //            this.lbl_progressbar1.Text = "";
        //            this.button1.Enabled = true;  //允许整体更新功能
        //            txtShow1("导入" + fileName + "文件成功！\r\n");     //右侧更新提示区  
        //            this.lst_codeshow1.Items.Clear();    //左侧代码显示区
        //            foreach (string str in hex.getHexStrList())
        //            {
        //                this.lst_codeshow1.Items.Add(str);    //左侧代码显示区
        //            }
        //            if (lbl_uartstate.Text.Contains("MSP432") == true)
        //            {
        //                StreamReader ssr = File.OpenText(Environment.CurrentDirectory.Replace("\\bin\\Debug", "") + @"\04_Resource\DChex.txt");
        //                string nextLine1;
        //                while ((nextLine1 = ssr.ReadLine()) != null)
        //                {
        //                    if (nextLine1.Contains("[MSP432 P401R]"))
        //                    {
        //                        while ((nextLine1 = ssr.ReadLine()) != null)
        //                        {
        //                            if (nextLine1.Contains("#DC串口更新"))
        //                            {
        //                                button1.Enabled = true;
        //                                ssr.Close();
        //                                return;
        //                            }
        //                        }
        //                    }
        //                }
        //                ssr.Close();
        //            }

        //    }

        //    //（4）退出区
        //    //（4.1）退出函数
        //    btn_fileopen1_Click_EXIT:
        //        return;

        //    //（4.2）导入hex文件失败
        //    btn_fileopen1_Click_EXIT1:
        //        this.button1.Enabled = false;  //禁止整体更新功能
        //        MessageBox.Show("Hex文件异常!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        //        goto btn_fileopen1_Click_EXIT;

        //}


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
            MyStringBuilder.Append(Encoding.GetEncoding("GBK").GetString(showData));//【20200428】优化效率
            //右侧更新提示区
            //【20191024】删除原来超过220行自动清除文本框的功能，原本清除文本框功能会影响事件响应
            //this.txt_updateinfo1.AppendText(str);
            this.txt_updateinfo1.AppendText(MyStringBuilder.ToString());//【20200428】优化效率
            MyStringBuilder.Clear();


            this.txt_updateinfo1.SelectionStart = this.txt_updateinfo1.Text.Length;  //光标指向最后一位
            this.txt_updateinfo1.ScrollToCaret();   //移动到光标处

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
            if (this.txt_updateinfo1.Lines.Length >= 220) this.txt_updateinfo1.Text = String.Empty; //长度过长则清空内容
            this.txt_updateinfo1.Text += str;
            this.txt_updateinfo1.Refresh();
            this.txt_updateinfo1.SelectionStart = this.txt_updateinfo1.Text.Length;  //光标指向最后一位
            this.txt_updateinfo1.ScrollToCaret();   //移动到光标处

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
            if (infor_dispaly_button.Text == "暂停")
            {
                infor_dispaly_button.Text = "继续";
                emuart.RawDataReceivedEvent -= new EMUART.RawDataReceived(DataRecv);//解除事件绑定的方法,暂停数据接收
            }

            else
            {
                infor_dispaly_button.Text = "暂停";
                emuart.RawDataReceivedEvent += new EMUART.RawDataReceived(DataRecv);//解除事件绑定的方法,开始数据接收
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
                    infor_dispaly_button.Text = "继续";
                    emuart.RawDataReceivedEvent -= new EMUART.RawDataReceived(DataRecv);//解除事件绑定的方法,暂停数据接收
                }
                message_count = 0;
                Thread.Sleep(1000);
            }
        }

        public void Update()
        {
            Thread.Sleep(500);
            //button1_Click(null, null);
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frm_UserUpdate_FormClosed(object sender, FormClosedEventArgs e)
        {

            //infor_dispaly_button_Click(sender, e);
            btnCloseUpdate_Click(sender, new EventArgs());
            thread.Abort();
            this.Dispose();

        }

        /// <summary>
        /// 退出串口更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCloseUpdate_Click(object sender, EventArgs e)
        {
            //infor_dispaly_button_Click(sender, e);
            if (threadA.IsAlive) threadA.Abort();
            //Thread.Sleep(3000);
            if (emuart._Uartport != null)
            {
                emuart.DataReceivedEvent -= new EMUART.DataReceived(DataRecv);
                closing = true;
                while (listening)
                {
                    Application.DoEvents();
                }
                emuart._Uartport.Dispose();
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

        public void button1_Click(object sender, EventArgs e)
        {
            

            FirstAddress = int.Parse(textBox1.Text);
            LastAddress = int.Parse(textBox4.Text);
            //button1.Enabled = false;


            Thread.Sleep(10000);
            //目标节点的地址范围在1~10000之间
            if (FirstAddress < 1 || FirstAddress > 10000 || LastAddress < 1 || LastAddress > 10000 || FirstAddress > LastAddress)
                txtShow1("请检查输入的地址(1~10000)是否正确！");
            else
            {
                threadA = new Thread(Auto_Update);
                Application.DoEvents();     //【20191024】与重新连接部分原理相同
                textBox5.Text = "";
                if (threadA.IsAlive == false)
                    threadA.Start();
                else
                {
                    //MessageBox.Show("线程执行中");
                    threadA.Abort();
                    Thread.Sleep(3000);
                    threadA = new Thread(Auto_Update);
                    threadA.Start();
                    Application.DoEvents();     //【20191024】与重新连接部分原理相同
                }

            }
        }
       
       

        private bool Connect_PC()
        {

            //【1】变量声明
            int ret;            //返回值
            int count;
            int flag;
            string com = "";    //串口信息
            string sTemp = "";
            byte[] recv = null;//保存串口接收信息
            //byte[] shake = { (byte)10, (byte)'s', (byte)'h', (byte)'a', (byte)'k', (byte)'e' }; //与终端握手帧数据




            count = 1;
            flag = 0;
            //【2】清除一些可能余留信息
            this.lbl_uartstate.Text = "";       //右上角显示区
            //this.lst_codeshow1.Items.Clear();   //左侧代码显示区
            this.txt_updateinfo1.Text = "";     //右侧更新提示区
            this.Refresh();                     //刷新显示  

            try
            {
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
                //Thread.Sleep(100);   //等待串口关闭
                emuart.RawDataReceivedEvent -= new EMUART.RawDataReceived(DataRecv);//解除事件绑定的方法，加快握手速度
                if (emuart != null) emuart = null;
                emuart = EMUART.getInstance();    //每次“重新连接”重新实例化
                //byte[] imei = null;   //没用上啊，这到底干嘛的
                //byte[] imei = System.Text.Encoding.Default.GetBytes(textBox1.Text);
                txtShow1("正在连接PC-Node节点1.\r\n");
                ret = emuart.findDevice(out com, 115200);  //寻找emuart设备
                this.lbl_uartstate.Text = com;                  //右上角显示区
                                                                //【4】根据寻找串口的返回值确定执行下列分支
                if (ret != 0)
                {
                    txtShow1("未找到可用串口.\r\n");
                    return false;
                }



                //【4.3】如果找到串口与UE,向设备发送握手帧
                byte[] shake = { (byte)10, (byte)'u', (byte)'s', (byte)'e', (byte)'r', (byte)'s', (byte)'h', (byte)'a', (byte)'k', (byte)'e' }; //与终端握手帧数据
                emuart.bufferClear();   //清除接收数组缓冲区
                txtShow1("正在连接PC-Node节点2.\r\n");
                while (count++ < 3)
                {
                    txtShow1("数据发送\r\n");
                    if (!emuart.send1(shake, out recv, 100, 3))                    //发送数据至下位机
                    {
                        txtShow1("发送失败\r\n");

                        //count--;
                        continue;
                    }
                    System.Threading.Thread.Sleep(10); //延时10毫秒
                    if (recv == null)                                       //没收到返回
                    {

                        txtShow1("连接PC-Node节点失败\r\n");
                    }
                    else                                                   //收到返回
                    {

                        txtShow1("连接PC-Node节点成功.\r\n");
                        flag = 1;
                        break;
                    }
                }
                if (flag != 1)
                {
                    return false;
                }
                //if(!emuart.send(shake, out recv, 100, 3) && count++ < 3)
                //count
                //emuart.send(shake, out recv, 100, 3); //获得设备的信息在recv中
                ////this.btn_fileopen1.Enabled = true;     //允许选择文件功能
                //if (recv == null)
                //{
                //    txtShow1("连接PC-Node节点失败\r\n");
                //    return false;//未收到数据，退出   
                //}
                //else
                //{
                //    txtShow1("连接PC-Node节点成功.\r\n");
                //}
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
                    this.lbl_uartstate.Text = sTemp;     //右上角显示区
                    this.txt_updateinfo1.Text = sTemp + "\r\n";     //右侧更新提示区     
                                                                    //若保留更新最大字节为0表示目前没有User程序，此时只能进行整体更新

                    if (reserveNum == 0)
                    {
                        txtShow1("可进行GEC中User程序更新\r\n");     //右侧更新提示区  
                    }
                    //如果存在串口，则允许接收调试信息
                    if (emuart.haveUE)
                    {
                        //绑定串口结束事件，处理串口调试信息
                        emuart.RawDataReceivedEvent += new EMUART.RawDataReceived(DataRecv);
                    }
                }

                //【4.4.2】如果终端的更新程序版本为VA.10及其以前的版本
                else if (System.Text.Encoding.Default.GetString(recv).Contains("shake:GEC-"))
                {
                    softVersion = 1;   //表示终端更新程序为A.10及之前的版本
                    sTemp = "MKL36Z64" + "(" + com + ")";//设备信息
                    this.lbl_uartstate.Text = "成功连接" + sTemp;       //右上角显示区
                    this.lbl_uartstate.Text = "找到终端设备" + sTemp;   //右上角显示区

                }
                else if (recv.Length == Marshal.SizeOf(typeof(newshakeData)))
                {
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
                    //设置设备信息  
                    //【20200715 2/2】 将版本信息显示出来 
                    sTemp = com + "：" + " " + mcuType + biosVersion;   //设备信息
                                                                       //状态提示
                    this.lbl_uartstate.Text = sTemp + "\r\n";                    //左边显示区
                    this.lbl_uartstate.Refresh();

                    this.txt_updateinfo1.Text = sTemp + "\r\n";        //右侧更新提示区     
                                                                       //若保留更新最大字节为0表示目前没有User程序，此时只能进行整体更新
                    if (reserveNum == 0)
                    {
                        txtShow1("可进行GEC中User程序更新\r\n");     //右侧更新提示区  
                    }
                    //如果存在串口，则允许接收调试信息
                    if (emuart.haveUE)
                    {
                        //绑定串口结束事件，处理串口调试信息
                        emuart.RawDataReceivedEvent += new EMUART.RawDataReceived(DataRecv);
                    }
                }
                return true;
            }
            catch { return false; }
        }


        private bool Auto_connect(int i)
        {
            

                //【1】变量声明
                int ret;            //返回值
                string com = "";    //串口信息
                string sTemp = "";
                byte[] recv = null;//保存串口接收信息
                                   //byte[] shake = { (byte)10, (byte)'s', (byte)'h', (byte)'a', (byte)'k', (byte)'e' }; //与终端握手帧数据
                int Hardaddress;

                //【2】清除一些可能余留信息
                this.lbl_uartstate.Text = "";       //右上角显示区
                this.txt_updateinfo1.Text = "";     //右侧更新提示区
                this.Refresh();                     //刷新显示  
                try {

                    byte[] imei = BitConverter.GetBytes(i);

                    //向终端发送软件地址和版本号
                    byte[] TarData = new byte[11];
                    TarData[0] = (byte)12;
                    Array.Copy(imei, 0, TarData, 1, 2);
                    byte[] softver = System.Text.Encoding.Default.GetBytes(textBox3.Text);
                    Array.Copy(softver, 0, TarData, 3, 8);

                    emuart.bufferClear();   //清除接收数组缓冲区
                    emuart.send(TarData, out recv, 2000, 3);

                    if (recv != null && recv.Length == 4)
                    {
                        this.lbl_uartstate.Text = "握手被拒绝，终端软件版本已为最新版。";

                        return false;
                    }

                    Hardaddress = recv[10];
                    Hardaddress += recv[11] << 8;
                    Hardaddress += recv[12] << 16;
                    Hardaddress += recv[13] << 24;
                    //"该节点的硬件地址为：0x" + Convert.ToString(Hardaddress, 16);
                    this.txtShow1("硬件地址为：0x" + Convert.ToString(Hardaddress, 16) + "的PC节点正在搜索目标节点，请稍后...\r\n");     //右侧更新提示区
                                                                                                                    //【4.3】如果找到串口与UE,向设备发送握手帧
                    byte[] shake = { (byte)10, (byte)'u', (byte)'s', (byte)'e', (byte)'r', (byte)'s', (byte)'h', (byte)'a', (byte)'k', (byte)'e' }; //与终端握手帧数据
                    emuart.bufferClear();   //清除接收数组缓冲区
                    emuart.send(shake, out recv, 6000, 3); //获得设备的信息在recv中
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
                        this.lbl_uartstate.Text = sTemp;     //右上角显示区
                        this.txt_updateinfo1.Text = sTemp + "\r\n";     //右侧更新提示区     
                                                                        //若保留更新最大字节为0表示目前没有User程序，此时只能进行整体更新

                        if (reserveNum == 0)
                        {
                            txtShow1("可进行GEC中User程序更新111\r\n");     //右侧更新提示区  
                        }
                        //如果存在串口，则允许接收调试信息
                        if (emuart.haveUE)
                        {
                            //绑定串口结束事件，处理串口调试信息
                            emuart.RawDataReceivedEvent += new EMUART.RawDataReceived(DataRecv);
                        }
                    }

                    //【4.4.2】如果终端的更新程序版本为VA.10及其以前的版本
                    else if (System.Text.Encoding.Default.GetString(recv).Contains("shake:GEC-"))
                    {
                        softVersion = 1;   //表示终端更新程序为A.10及之前的版本
                        sTemp = "MKL36Z64" + "(" + com + ")";//设备信息
                        this.lbl_uartstate.Text = "成功连接" + sTemp;       //右上角显示区
                        this.lbl_uartstate.Text = "找到终端设备" + sTemp;   //右上角显示区

                    }
                    else if (recv.Length == Marshal.SizeOf(typeof(newshakeData)))
                    {
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
                            Array.Copy(imei, 0, TarData, 1, 2);


                            emuart.bufferClear();   //清除接收数组缓冲区
                            emuart.send(linkData, out recv, 2000, 1);
                            if (recv == null) goto btn_uartcheck_Click_EXIT4;
                            if (Encoding.Default.GetString(recv).Equals("linkSuccse"))
                            {
                                //设置设备信息  
                                //【20200715 2/2】 将版本信息显示出来 
                                sTemp = com + "：" + " " + mcuType + biosVersion;  //设备信息
                                                                                  //状态提示
                                this.lbl_uartstate.Text = sTemp;     //右上角显示区
                                this.txt_updateinfo1.Text = sTemp + "\r\n";     //右侧更新提示区     
                                txtShow1("可进行GEC中User程序更新\r\n");     //右侧更新提示区  
                                txtShow1("硬件地址为：0x" + Convert.ToString(Hardaddress, 16) + textBox1.Text + "节点连接成功，可进行远程更新\r\n");
                                return true;
                            }
                            else
                            {
                                goto btn_uartcheck_Click_EXIT4;
                            }
                            //if (recv == null) //未收到数据，退出  

                        }
                        //如果存在串口，则允许接收调试信息
                        if (emuart.haveUE)
                        {
                            //绑定串口结束事件，处理串口调试信息
                            emuart.RawDataReceivedEvent += new EMUART.RawDataReceived(DataRecv);
                        }
                    }
                    //【4.4.3】若收到错误返回信息，退出函数
                    else goto btn_uartcheck_Click_EXIT3;

                    //【5】退出区
                    // 【5.1】退出函数


                    //【5.2】不存在可用串口
                    btn_uartcheck_Click_EXIT1:
                    this.lbl_uartstate.Text = "当前不存在可用串口";       //右上角显示区
                    this.txt_updateinfo1.Text = "没有找到USB串口，可能原因如下：\r\n（1）USB串口未插上PC\r\n（2）PC未安装串口驱动\r\n";     //右侧更新提示区
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
                    return false;

                //【5.3】存在串口，但不存在emuar设备
                btn_uartcheck_Click_EXIT2:
                    //emuart.terminate(9600); //发送数据给终端设备，让终端设备清空其数据缓冲区
                    this.lbl_uartstate.Text = "已连接串口" + com + "但未找到设备";       //右上角显示区
                                                                              //右侧更新提示区
                    this.txt_updateinfo1.Text = "有USB串口，但未连上终端，可能原因如下：\r\n（1）USB串口驱动需更新\r\n（2）USB串口未连接终端\r\n（3）终端程序未运行\r\n";
                    closing = true;     //正在关闭串口
                    while (listening)
                    {
                        Application.DoEvents();
                    }
                    emuart._Uartport.Close();
                    closing = false;    //关闭完成
                    return false;

                //【5.4】未收到返回信息或收到错误返回信息
                btn_uartcheck_Click_EXIT3:
                    //emuart.terminate(9600); //发送数据给终端设备，让终端设备清空其数据缓冲区
                    this.lbl_uartstate.Text = "找到PC节点在" + com + "但握手失败，请再次单击[重新连接]按钮";  //右上角显示
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
                    return false;

                btn_uartcheck_Click_EXIT4:
                    //emuart.terminate(9600); //发送数据给终端设备，让终端设备清空其数据缓冲区
                    this.lbl_uartstate.Text = "找到PC节点在" + com + "且握手成功，但连接" + i.ToString() + "节点失败，请再次单击[重新连接]按钮";  //右上角显示
                    this.txt_updateinfo1.Text = "连接失败可能原因如下：\r\n（1）待更新节点软件地址错误\r\n（2）节点无线收发出错\r\n（3）终端程序未运行\r\n";
                    return false;
                
                }
                catch { return false; }

        }


        private bool start_Update(int i)
        {
            //（1）变量声明
            bool sflag = false;
            int sum;          //发送总帧数
            int cur;          //当前发送帧号
            byte[] senddata = null;
            byte[] recvdata = null;
            byte[] shake = { (byte)10, (byte)'s', (byte)'h', (byte)'a', (byte)'k', (byte)'e' };  //握手帧
            byte[] startUpdate = { (byte)11, (byte)'u', (byte)'p', (byte)'d', (byte)'a', (byte)'t', (byte)'e' }; //开始更新提示帧
            byte[] zero = new byte[500];
            byte[] reStartFrame = new byte[3];

            Application.DoEvents();     //【20191024】与重新连接部分原理相同


            Code_Update = true;                                          //当前处于程序更新中
            //（2）如果更新程序版本为VA.10之后的版本
            //if (softVersion == 2)
            //{
            //（2.1）若未导入Hex文件或串口连接失败则退出（防错用）



            if (User1_hex.getHexList().Count == 0) goto btn_autoupdate1_Click_EXIT1;//未导入Hex文件
            if (emuart == null || emuart.haveUE == false) goto btn_autoupdate1_Click_EXIT2;//串口连接失败
                                                                                           //（2.2）开始更新
            update = new Update(overallStyle, User1_hex.getHexList());  //update初始化
            sum = update.getFrameNum();
            try
            {
                txtShow1("运行状态：整体更新开始\r\n");     //右侧更新提示区  

                emuart.RawDataReceivedEvent -= new EMUART.RawDataReceived(DataRecv);//解除事件绑定的方法，加快握手速度
                infor_dispaly_button.Text = "暂停";
                emuart.RawDataReceivedEvent += new EMUART.RawDataReceived(DataRecv);//串口事件绑定
                System.Threading.Thread.Sleep(100); //延时100毫秒
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
                        this.prg_update1.Value = (cur + 1) * 100 / sum;            //进度条显示
                        this.lbl_progressbar1.Text = (cur + 1) * 100 / sum + "%";  //进度百分比显示
                        this.prg_update1.Refresh();
                        //成功提示
                        txtShow1("当前第" + (cur + 1).ToString() + "/" + sum.ToString() + "帧 \r\n");     //右侧更新提示区  

                    }
                    //最后一帧更新命令帧，仅需发送
                    else
                    {
                        txtShow1("当前第" + (cur + 1).ToString() + "/" + sum.ToString() + "帧 \r\n");     //右侧更新提示区
                        txtShow1("程序整体更新成功\r\n");     //右侧更新提示区  
                        this.prg_update1.Value = 100;
                        this.lbl_progressbar1.Text = "100%";
                        this.lbl_progressbar1.Refresh();

                        emuart.send(senddata, out recvdata, 0, 1); //令发送等待时间为0 20190516（1/2）
                        
                        txtShow1("终端地址为" + Convert.ToString(i) + "信息更新成功。\r\n");     //右侧更新提示区
                        sflag = true; 
                        System.Threading.Thread.Sleep(100); //延时100毫秒
                        return sflag;
                    }

                }
                //（2.3）更新成功 
                goto btn_autoupdate1_Click_EXIT3;
            }
            catch (Exception ex)
            {
                //txtShow1("系统异常退出\r\n" + ex + "\r\n");     //右侧更新提示区  
                goto btn_autoupdate1_Click_EXIT;
            }

        //（4）退出区
        //（4.1）退出函数
        btn_autoupdate1_Click_EXIT:
            return sflag;

        //（4.2）Hex文件未选择
        btn_autoupdate1_Click_EXIT1:
            goto btn_autoupdate1_Click_EXIT;

        //（4.3）未连接设备
        btn_autoupdate1_Click_EXIT2:
            goto btn_autoupdate1_Click_EXIT;

        //（4.4）程序整体更新成功
        btn_autoupdate1_Click_EXIT3:
            sflag = true;
            goto btn_autoupdate1_Click_EXIT;

        //（4.6）未接收到终端返回数据
        btn_autoupdate1_Click_EXIT5:
            txtShow1("错误提示：未接收到终端返回数据\r\n");     //右侧更新提示区  
            goto btn_autoupdate1_Click_EXIT;

        //（4.7）接收到的更新返回数据异常
        btn_autoupdate1_Click_EXIT6:
            txtShow1("错误提示：接收到的更新返回数据异常\r\n");     //右侧更新提示区  
            goto btn_autoupdate1_Click_EXIT;

        }

        //======================================================================
        //函数名称：txtShow3
        //函数返回：无
        //参数说明：str:在终端执行信息提示框追加显示的内容
        //功能概要：整体更新过程中，在终端连接状态提示框lbl_uartstate中追加提示
        //          信息，并显示最新信息      
        //======================================================================
        private void txtShow3(string str)
        {
            //右侧更新提示区
            //if (this.textBox2.Lines.Length >= 220) this.textBox2.Text = String.Empty; //长度过长则清空内容
            this.lbl_uartstate.Text += str;
            this.lbl_uartstate.Refresh();

        }

        //======================================================================
        //函数名称：txtShow2
        //函数返回：无
        //参数说明：str:在终端执行信息提示框追加显示的内容
        //功能概要：整体更新过程中，在终端执行信息提示框textBox2中追加提示
        //          信息，并显示最新信息      
        //======================================================================
        private void txtShow2(string str)
        {
            //右侧更新提示区
            //if (this.textBox2.Lines.Length >= 220) this.textBox2.Text = String.Empty; //长度过长则清空内容
            this.textBox2.Text += str;
            this.textBox2.Refresh();
            this.textBox2.SelectionStart = this.textBox2.Text.Length;  //光标指向最后一位
            this.textBox2.ScrollToCaret();   //移动到光标处

        }

        //======================================================================
        //函数名称：txtShow5
        //函数返回：无
        //参数说明：str:在终端执行信息提示框追加显示的内容
        //功能概要：整体更新过程中，在终端执行信息提示框textBox4中追加提示
        //          信息，并显示最新信息      
        //======================================================================
        private void txtShow5(string str)
        {
            //右侧更新提示区
            //if (this.textBox2.Lines.Length >= 220) this.textBox2.Text = String.Empty; //长度过长则清空内容
            this.textBox5.Text += str;
            this.textBox5.Refresh();
            this.textBox5.SelectionStart = this.textBox4.Text.Length;  //光标指向最后一位
            this.textBox5.ScrollToCaret();   //移动到光标处

        }

        private void grp_uartHint_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void lbl_uartstate_Click(object sender, EventArgs e)
        {

        }


        //======================================================================
        //函数名称：btn_fileopen2_Click
        //函数返回：无
        //参数说明：无
        //功能概要：导入待整体更新Hex文件并对该文件进行解析取出其有效数据
        //======================================================================
        private void btn_fileopen2_Click(object sender, EventArgs e)
        {
            //变量声明           
            int flag;

            uint codesize;            //代码大小

            string filePath;          //文件路径
            string fileName;          //文件名
            string line;              //Hex文件行数据
            List<hexStruct_old> list; //更新程序对应Hex文件保存变量       


            OpenFileDialog ofd = new OpenFileDialog();  //打开文件对话框
            ofd.Filter = "hex file(*.hex)|*.hex";
            textBox2.Clear();
            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                //获取文件名
                filePath = ofd.FileName;
                flag = User1_hex.loadFile(filePath);
                if (flag != 0) goto btn_fileopen2_Click_EXIT1;

                //（2.4）获取导入的文件信息
                fileName = User1_hex.getFileName();
                startAdd = User1_hex.getStartAddress();    //hex文件中读取的起始地址
                codesize = User1_hex.getCodeSize();

                string ldFilePath = string.Empty;
                string userStart = string.Empty;      //被替换的起始地址
                string strStartAdd = string.Empty;    //替换的起始地址
                StreamReader sr = null;
                fileList_User1 = new List<FileInfo>();

                //状态提示
                this.User_filename1.Text = fileName;
                this.prg_update1.Value = 0;
                this.lbl_progressbar1.Text = "";
                txtShow1("导入" + fileName + "文件成功！\r\n");     //右侧更新提示区  
                button1.Enabled = true;
                this.lst_codeshow2.Items.Clear();    //左侧代码显示区
                foreach (string str in User1_hex.getHexStrList())
                {
                    this.lst_codeshow2.Items.Add(str);    //左侧代码显示区
                }
            }
        btn_fileopen2_Click_EXIT:
            return;
        //退出函数
        btn_fileopen2_Click_EXIT1:
            this.button1.Enabled = false;  //禁止整体更新功能
            MessageBox.Show("Hex文件异常!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            goto btn_fileopen2_Click_EXIT;
        }


        //======================================================================
        //函数名称：btn_fileopen3_Click
        //函数返回：无
        //参数说明：无
        //功能概要：导入待整体更新Hex文件并对该文件进行解析取出其有效数据
        //======================================================================
        public void btn_fileopen3_Click(object sender, EventArgs e)
        {
            //变量声明           
            int flag;

            uint codesize;            //代码大小

            string filePath;          //文件路径
            string fileName;          //文件名
            string line;              //Hex文件行数据
            List<hexStruct_old> list; //更新程序对应Hex文件保存变量       


            OpenFileDialog ofd = new OpenFileDialog();  //打开文件对话框
            ofd.Filter = "hex file(*.hex)|*.hex";
            textBox2.Clear();
            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                //获取文件名
                filePath = ofd.FileName;
                flag = User2_hex.loadFile(filePath);
                if (flag != 0) goto btn_fileopen3_Click_EXIT1;

                //（2.4）获取导入的文件信息
                fileName = User2_hex.getFileName();
                startAdd = User2_hex.getStartAddress();    //hex文件中读取的起始地址
                codesize = User2_hex.getCodeSize();

                string ldFilePath = string.Empty;
                string userStart = string.Empty;      //被替换的起始地址
                string strStartAdd = string.Empty;    //替换的起始地址
                StreamReader sr = null;
                fileList_User2 = new List<FileInfo>();

                //状态提示
                //this.User_filename2.Text = fileName;
                this.prg_update1.Value = 0;
                this.lbl_progressbar1.Text = "";
                txtShow1("导入" + fileName + "文件成功！\r\n");     //右侧更新提示区  
                //this.lst_codeshow3.Items.Clear();    //左侧代码显示区
                foreach (string str in User2_hex.getHexStrList())
                {
                    //this.lst_codeshow3.Items.Add(str);    //左侧代码显示区
                }
            }
        btn_fileopen3_Click_EXIT:
            return;
        //退出函数
        btn_fileopen3_Click_EXIT1:
            this.button1.Enabled = false;  //禁止整体更新功能
            MessageBox.Show("Hex文件异常!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            goto btn_fileopen3_Click_EXIT;
        }

        //======================================================================
        //函数名称：btn_fileopen4_Click
        //函数返回：无
        //参数说明：无
        //功能概要：导入待整体更新Hex文件并对该文件进行解析取出其有效数据
        //======================================================================
        private void btn_fileopen4_Click(object sender, EventArgs e)
        {
            //变量声明           
            int flag;

            uint codesize;            //代码大小

            string filePath;          //文件路径
            string fileName;          //文件名
            string line;              //Hex文件行数据
            List<hexStruct_old> list; //更新程序对应Hex文件保存变量       
            OpenFileDialog ofd = new OpenFileDialog();  //打开文件对话框
            ofd.Filter = "hex file(*.hex)|*.hex";
            textBox2.Clear();
            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                //获取文件名
                filePath = ofd.FileName;
                flag = User3_hex.loadFile(filePath);
                if (flag != 0) goto btn_fileopen4_Click_EXIT1;

                //（2.4）获取导入的文件信息
                fileName = User3_hex.getFileName();
                startAdd = User3_hex.getStartAddress();    //hex文件中读取的起始地址
                codesize = User3_hex.getCodeSize();

                string ldFilePath = string.Empty;
                string userStart = string.Empty;      //被替换的起始地址
                string strStartAdd = string.Empty;    //替换的起始地址
                StreamReader sr = null;
                fileList_User3 = new List<FileInfo>();

                //状态提示
                //this.User_filename3.Text = fileName;
                this.prg_update1.Value = 0;
                this.lbl_progressbar1.Text = "";
                this.button1.Enabled = true;  //允许整体更新功能
                txtShow1("导入" + fileName + "文件成功！\r\n");     //右侧更新提示区  
                //this.lst_codeshow4.Items.Clear();    //左侧代码显示区
                foreach (string str in User3_hex.getHexStrList())
                {
                    //this.lst_codeshow4.Items.Add(str);    //左侧代码显示区
                }
            }
        btn_fileopen4_Click_EXIT:
            return;
        //退出函数
        btn_fileopen4_Click_EXIT1:
            this.button1.Enabled = false;  //禁止整体更新功能
            MessageBox.Show("Hex文件异常!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            goto btn_fileopen4_Click_EXIT;
        }

        private void txt_updateinfo1_TextChanged(object sender, EventArgs e)
        {

        }
       

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void frm_UserUpdate_FormClosing(object sender, FormClosingEventArgs e)
        {
            

        }

        private void button2_Click(object sender, EventArgs e)
        {

            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            path += @"\更新失败设备列表-" + DateTime.Now.ToString("yyyyMMdd") + @".txt";
            //System.IO.File.WriteAllText(path, string.Empty);
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                try
                {

                    sw.Write(System.DateTime.Now + "\r\n" + this.textBox5.Text + "\r\n-----------------------\r\n");
                    //MessageBox.Show("导出成功");
                }
                catch
                {
                    MessageBox.Show("导出列表失败");
                }
            }

        }

        private void Auto_Update()
        {
            //（1）变量声明
            bool con_flag = false;
            bool sflag;

            int CodeFlag;                                            //更新程序序号
            int count = 1;
            //int UpdateCount;

            CodeFlag = 1;

            int Link_Count = MAX_COUNT;
            //自动选择设备号并连接
            try
            {
                textBox2.Text = "";
                this.textBox2.Refresh();
                if (Connect_PC())                               //连接PC-Node节点
                {

                    //自动选择设备号并连接
                    for (int i = FirstAddress; i <= LastAddress; i++)
                    {
                        while (Link_Count-- > 0)
                        {
                            con_flag = Auto_connect(i);
                            if (con_flag)
                                break;
                            //txtShow2("重新进行连接Target节点\r\n");
                            txtShow1("重新进行连接Target节点\r\n");
                            Thread.Sleep(6000);
                        }
                        if (con_flag)
                        {
                            Link_Count = MAX_COUNT;
                            txtShow1("当前烧录的设备软件地址为：" + Convert.ToString(i) + "\r\n");
                            sflag = start_Update(CodeFlag);
                            if (sflag)
                            {
                                txtShow2("终端地址: " + Convert.ToString(i) + "节点程序更新完成\r\n");
                            }
                            else
                            {
                                txtShow5("终端地址: " + Convert.ToString(i) + "节点程序更新失败\r\n");

                            }

                            Thread.Sleep(6000);                 //更新一个节点延时3秒

                            Code_Update = false;                             //程序更新结束 

                        }
                        else
                        {
                            Link_Count = MAX_COUNT;
                            txtShow5("终端地址: " + Convert.ToString(i) + "节点程序更新失败\r\n");
                            Thread.Sleep(6000);
                            continue;
                        }
                    }
                }
                else
                {
                    count++;
                    if (count < 3)
                    {
                        txtShow2("连接失败，再次连接PC-Node\r\n");
                    }
                }
                count = 1;
                txtShow2("节点更新完毕，延时60秒进行下一次更新。\r\n");
                Thread.Sleep(60000);                                   //更新一次延时60秒
                button2.PerformClick();
                button1.Enabled = true; button2.Enabled = true;
                Thread.Sleep(1000);
                Code_Update = false;                             //程序更新结束 
                button1.PerformClick();       //自动开始下一次更新


            }
            catch (Exception ex)
            {
                txtShow1(ex.ToString());
                txtShow2("节点更新异常，延时60秒进行下一次更新。\r\n");
                Thread.Sleep(60000);                                   //更新一次延时60秒
                button2.PerformClick();
                button1.Enabled = true; button2.Enabled = true;
                Thread.Sleep(1000);
                Code_Update = false;                             //程序更新结束 
                button1.PerformClick();       //自动开始下一次更新
            }

        }
    }
}


