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
using System.Diagnostics;
using Newtonsoft.Json;
using System.Net.Security;

namespace AHL_GEC
{
    public partial class frm_UserInfoUpdate : Form
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
        private string selected_com;         //选中的COM口


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
        byte[] oldimei = null;
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
        //======================================================================
        //函数名称：frm_UserUpdate
        //函数返回：frm_UserUpdate窗体构造函数
        //参数说明：无
        //功能概要：完成frm_UserUpdate窗体的初始化工作
        //======================================================================
        public frm_UserInfoUpdate()
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
        private void frm_UserInfoUpdate_Load(object sender, EventArgs e)
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

        }


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
            this.textBox2.AppendText(MyStringBuilder.ToString());//【20200428】优化效率
            MyStringBuilder.Clear();


            this.textBox2.SelectionStart = this.textBox2.Text.Length;  //光标指向最后一位
            this.textBox2.ScrollToCaret();   //移动到光标处

        }

        private void button2_Click(object sender, EventArgs e)
        {

            //【1】变量声明
            int ret;            //返回值
            int Softaddress;
            int Hardaddress;
            string com = "";    //串口信息
            string sTemp = "";
            byte[] recv = null;//保存串口接收信息
            byte[] shake = { (byte)10, (byte)'s', (byte)'h', (byte)'a', (byte)'k', (byte)'e' }; //与终端握手帧数据
            byte[] askaddress = { (byte)11, (byte)'A', (byte)'s', (byte)'k', (byte)'A', (byte)'d', (byte)'d', (byte)'r', (byte)'e', (byte)'s', (byte)'s' };
            //byte[] imei = null;   //这个变量好像定义了没用到过
            string SciInfo = comboBoxUart.Text;     //获取选中的串口：自动更新/具体的串口号
            //【2】清除一些可能余留信息
            this.lbl_uartstate.Text = "";       //右上角显示区
            this.textBox2.Text = "";
            this.Refresh();                     //刷新显示 

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
            emuart.RawDataReceivedEvent -= new EMUART.RawDataReceived(DataRecv);//解除事件绑定的方法，加快握手速度
            if (emuart != null) emuart = null;

            if(SciInfo == "自动搜索")
            {
                //Debug.Print("自动连接");
                emuart = EMUART.getInstance();    //每次“重新连接”重新实例化
                ret = emuart.findDevice(out com, 115200);  //寻找emuart设备,这个finddDevice1干嘛的，功能不适合findDecvice一样
                this.lbl_uartstate.Text = com;       //右上角显示区
            }
            else
            {
                //Debug.Print("手动选择");
                emuart = EMUART.getInstance();    //每次“重新连接”重新实例化
                ret = emuart.LinkDevice(SciInfo, 115200);  //寻找emuart设备
                com = SciInfo;
                this.lbl_uartstate.Text = com;       //右上角显示区
            }
            selected_com = com;
            //emuart = EMUART.getInstance();    //每次“重新连接”重新实例化
            
            //fmain.lbl_mainstatus.Text = "运行状态：正在连接GEC...正在查找匹配设备...";     //底部提示
            //ret = emuart.findDevice1(out com, 115200);  //寻找emuart设备

            //this.lbl_uartstate.Text = com;       //右上角显示区
            //fmain.lbl_mainstatus.Text = "运行状态：正在连接GEC...尝试与匹配设备建立通信...";     //底部提示

            //【4】根据寻找串口的返回值确定执行下列分支
            //【4.1】如果连接终端的emuart失败，退出函数
            if (ret == 1) goto btn_uartcheck_Click_EXIT;

            //【4.2】找到串口，没有找到UE，退出函数
            else if (ret == 2) goto btn_uartcheck_Click_EXIT;
            
            emuart.bufferClear();   //清除接收数组缓冲区
            emuart.send(shake, out recv, 100, 3); //获得设备的信息在recv中
            if (recv == null) goto btn_uartcheck_Click_EXIT;//未收到数据，退出   

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

            //设置设备信息  
            //【20200715 2/2】 将版本信息显示出来 
            sTemp = com + "：" + " " + mcuType;  //设备信息
                                                //状态提示
            this.lbl_uartstate.Text = sTemp;     //右上角显示区
            this.lbl_uartstate.Text += " " + biosVersion;
            this.lbl_uartstate.Refresh();


            emuart.bufferClear();   //清除接收数组缓冲区
            emuart.send(askaddress, out recv, 100, 3); //获得设备的信息在recv中
            if (recv == null) goto btn_uartcheck_Click_EXIT;//未收到数据，退出   

            if (recv != null && recv.Length == 1)
                this.textBox2.Text += "该节点未进行软件地址或者硬件地址的设置，请先进行地址设置\r\n";
            
            if(recv != null && recv.Length == 8)
            {
                Hardaddress = recv[0];
                Hardaddress += recv[1] << 8;
                Hardaddress += recv[2] << 16;
                Hardaddress += recv[3] << 24;

                Softaddress = recv[4];
                Softaddress += recv[5] << 8;
                this.textBox2.Text += "该节点的硬件地址为：0x" + Convert.ToString(Hardaddress,16) + ",软件地址为："+ Convert.ToString(Softaddress)  + "\r\n" ;
                this.textBox2.Text += "设备类型为：" + 
                    (
                     ((recv[6] == 0x0) && (recv[7] == 0x1)) ? "从机" :
                     ((recv[6] == 0x1) && (recv[7] == 0x0)) ? "主机" :
                                                              "设备类型错误，请重新设置设备信息"
                    ) + "\r\n";
            }
            return;



        btn_uartcheck_Click_EXIT:
                this.textBox2.Text = "连接失败";
            return;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int ret;            //返回值
            byte[] shake = { (byte)12, (byte)'a', (byte)'d', (byte)'d', (byte)'r', (byte)'e', (byte)'s', (byte)'s' }; //与终端握手帧数据
            byte[] recv = null;//保存串口接收信息
            string com = "";    //串口信息
            textBox2.Clear();       //清空提示框
            Regex regex = new System.Text.RegularExpressions.Regex("^0x[0-9A-Fa-f]{1,8}$");
            bool b = regex.IsMatch(textBox3.Text);

            //20210511 WTC
            //验证输入的节点硬件地址
            if (b == false)
            {
                this.textBox2.Text = "输入的节点硬件地址有误！要求输入十六进制数\r\n";
                return;
            }
            //验证输入的节点软件地址
            if (int.Parse(textBox1.Text) < 0 || int.Parse(textBox1.Text) > 10000)
            {
                this.textBox2.Text = "输入的节点软件地址有误！\r\n";
                return;
            }
            //如果当前实际连接的COM和手动选择的COM不一致
            //即选中新的COM后没有再次连接GEC，则拒绝修改
            com = this.selected_com;
            if (com != comboBoxUart.Text && comboBoxUart.Text != "自动搜索")
            {
                Debug.Print(com);
                Debug.Print(comboBoxUart.Text);
                textBox2.Text = "选中新的COM后没有重新连接GEC，请点击“连接GEC”后在进行地址的更改！\r\n";
                return ;
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
            emuart.RawDataReceivedEvent -= new EMUART.RawDataReceived(DataRecv);//解除事件绑定的方法，加快握手速度
            if (emuart != null) emuart = null;

            emuart = EMUART.getInstance();    //每次“重新连接”重新实例化
            ret = emuart.LinkDevice(com, 115200);  //寻找emuart设备
            this.lbl_uartstate.Text = com;       //右上角显示区

            //【4】根据寻找串口的返回值确定执行下列分支
            //【4.1】如果连接终端的emuart失败，退出函数
            if (ret == 1) goto btn_uartcheck_Click_EXIT;

            //【4.2】找到串口，没有找到UE，退出函数
            else if (ret == 2) goto btn_uartcheck_Click_EXIT;

            byte[] haddr = BitConverter.GetBytes(Convert.ToInt32(textBox3.Text, 16));
            byte[] saddr = BitConverter.GetBytes(int.Parse(textBox1.Text));
            //byte[] chose_host  = BitConverter.GetBytes(radioButton1.Checked);
            //byte[] chose_slave = BitConverter.GetBytes(radioButton2.Checked);
            //Debug.Print(radioButton1.Checked ? "1" : "0");
            //Debug.Print(radioButton2.Checked ? "1" : "0");
            //向终端发送软件地址和版本号，以及主从机信息
            byte[] TarData = new byte[16];
            Array.Copy(shake, 0, TarData, 0, 8);
            Array.Copy(haddr, 0, TarData, 8, 4);
            Array.Copy(saddr, 0, TarData, 12, 2);
            TarData[14] = radioButton1.Checked ? (byte)1 : (byte)0;
            TarData[15] = radioButton2.Checked ? (byte)1 : (byte)0;

            emuart.bufferClear();   //清除接收数组缓冲区
            emuart.send(TarData, out recv, 200, 3);

            if (recv != null && recv.Length == 2)
            {
                this.textBox2.Text =  "更改成功。\r\n";
                this.textBox2.Text += "该节点的硬件地址为：0x" + textBox3.Text + ",软件地址为：" + textBox1.Text + "\r\n"; ;
                this.textBox2.Text += "设备类型为：" +
                    (
                     (radioButton1.Checked) ? "主机" :
                     (radioButton2.Checked) ? "从机" :
                                              "设备类型错误，请重新设置设备信息"
                    ) + "\r\n";
            }
            else
            {
                this.textBox2.Text = "更改失败。\r\n";
            }
            return ;

        btn_uartcheck_Click_EXIT:
            this.textBox2.Text = "连接失败.\r\n";
            return ;
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

        //======================================================================
        //函数名称：infor_dispaly_button_Click
        //函数返回：无
        //参数说明：无
        //功能概要：1秒钟超过50条数据显示则自动停止右侧提示区的显示
        //======================================================================
        //public void mes_count()
        //{
        //    while (true)
        //    {
        //        if (message_count >= 100)
        //        {
        //            infor_dispaly_button_1.Text = "继续";
        //            emuart.RawDataReceivedEvent -= new EMUART.RawDataReceived(DataRecv);//解除事件绑定的方法,暂停数据接收
        //        }
        //        message_count = 0;
        //        Thread.Sleep(1000);
        //    }
        //}

        private void frm_UserInfoUpdate_Load_1(object sender, EventArgs e)
        {
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
            //threadStart = new ThreadStart(mes_count);//通过ThreadStart委托告诉子线程执行什么方法　　
            //thread = new Thread(threadStart);
            //thread.Start();//启动新线程
            //(7)更新combox控件
            int i;
            string[] SCIPorts;
            SCIPorts = System.IO.Ports.SerialPort.GetPortNames();
            this.comboBoxUart.Items.Clear();//首先将现有的项清除掉
            this.comboBoxUart.Items.Add("自动搜索");
            for (i = 0; i < SCIPorts.Length; i++)
            {
                //向[串口选择框]中添加搜索到的串口号
                this.comboBoxUart.Items.Add(SCIPorts[i]);
            }
            this.comboBoxUart.SelectedIndex = 0;
        }

        private void comboBoxUart_MouseClick(object sender, MouseEventArgs e)
        {
            int i;
            string[] SCIPorts;
            SCIPorts = System.IO.Ports.SerialPort.GetPortNames();
            this.comboBoxUart.Items.Clear();//首先清除现有的项
            this.comboBoxUart.Items.Add("自动搜索");
            for(i =0; i < SCIPorts.Length; i++)
            {
                //向[串口选择框]中添加搜索到的串口号
                this.comboBoxUart.Items.Add(SCIPorts[i]);
            }
            this.comboBoxUart.SelectedIndex = 0;
        }

    }
}




        

        