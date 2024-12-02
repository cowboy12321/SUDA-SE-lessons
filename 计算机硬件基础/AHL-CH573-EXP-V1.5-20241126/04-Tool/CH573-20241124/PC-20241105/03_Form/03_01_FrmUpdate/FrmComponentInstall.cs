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
using System.Collections;
namespace AHL_GEC
{
    public partial class frm_uartDynamic : Form
    {
        //定义使用的全局变量
        private FrmMain fmain;  //主页面变量
        private Hex hex;  //Hex文件信息，整体更新用
        private SynchronizationContext m_SyncContext = null;    //用于安全地跨线程访问控件
        public EMUART emuart;     //串口通信类对象
        private uint softVersion; //保存终端更新程序版本，0表示更新版本不详，1表示更新版本为VA.10之前，2表示更新版本为VA.10之后
        private Hex_old hex_old;  //（VA.10版本之前）更新程序Hex对象
        private static UInt32 message_count;

        private string lstfile;//name of .lst file
        private int funcount;  //剩余指针数量
        private int funnum;  //下一个可使用的指针号
        private int funsize;  //剩余代码空间

        private string dname;//构件名
        private int dcount;  //构件函数数目
        private int[] dlength = new int[32];//构件函数长度

        private string dlocation;
        private string[] hfile = new string[32];
        

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

        //======================================================================
        //函数名称：frm_uartDynamic
        //函数返回：frm_uartDynamic窗体构造函数
        //参数说明：无
        //功能概要：完成frm_uartDynamic窗体的初始化工作
        //======================================================================
        public frm_uartDynamic()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        //======================================================================
        //函数名称：frm_uartDynamic_Load
        //函数返回：无
        //参数说明：无
        //功能概要：窗体加载事件，窗体加载时自动调用
        //======================================================================
        private void frm_uartDynamic_Load(object sender, EventArgs e)
        {
            //（1）本窗体由frmMain创建，所以本窗体的父窗体为frmMain
            fmain = (FrmMain)this.MdiParent;  //得到frmMain窗体变量
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
        }

        //======================================================================
        //函数名称：btn_uartcheck_Click
        //函数返回：无
        //参数说明：无
        //功能概要：“连接GEC”按钮点击事件，重新连接终端
        //======================================================================
        private void btn_uartcheck_Click(object sender, EventArgs e)
        {
            //（1）变量声明
            int ret;            //返回值
            string com = "";    //串口信息
            string sTemp = "";
            byte[] recv = null;//保存串口接收信息
            byte[] shake = { (byte)10, (byte)'s', (byte)'h', (byte)'a', (byte)'k', (byte)'e' }; //与终端握手帧数据
            //（2）清除一些可能余留信息
            this.lbl_uartstate.Text = "";       //右上角显示区
            fmain.lbl_mainstatus.Text = "运行状态：正在连接GEC...";     //底部提示
            this.txt_updateinfo1.Text = "";     //右侧更新提示区
            this.Refresh();                     //刷新显示 
            btn_uartcheck.Text = "再次连接GEC并检测容量"; //更改显示文字            

            //（3）重新遍历串口，寻找终端
            if (emuart._Uartport != null) emuart._Uartport.Close();
            Thread.Sleep(10);   //等待串口关闭
            emuart.RawDataReceivedEvent -= new EMUART.RawDataReceived(DataRecv);//解除事件绑定的方法，加快握手速度
            emuart = EMUART.getInstance();    //每次“重新连接”重新实例化
            fmain.lbl_mainstatus.Text = "运行状态：正在连接GEC...正在查找匹配设备...";     //底部提示
            //ret = emuart.findDevice(out com, 115200);  //寻找emuart设备
            this.lbl_uartstate.Text = com;       //右上角显示区
            fmain.lbl_mainstatus.Text = "运行状态：正在连接GEC...尝试与匹配设备建立通信...";     //底部提示

            //（4）根据寻找串口的返回值确定执行下列分支
            //（4.1）如果连接终端的emuart失败，退出函数
            //if (ret == 1) goto btn_uartcheck_Click_EXIT1;
            ////（4.2）找到串口，没有找到UE，退出函数
            //else if (ret == 2) goto btn_uartcheck_Click_EXIT2;
            //（4.3)如果找到串口与UE,向设备发送握手帧
            fmain.lbl_mainstatus.Text = "运行状态：正在连接GEC...向该设备发送握手信息，准备接收设备返回消息...";     //底部提示
            emuart.bufferClear();   //清除接收数组缓冲区
            emuart.send(shake, out recv, 100, 3); //获得设备的信息在recv中
            fmain.lbl_mainstatus.Text = "运行状态：正在连接GEC...正在处理设备返回信息...";     //底部提示
            if (recv == null) goto btn_uartcheck_Click_EXIT3;//未收到数据，退出              
            //（4.4)发送握手帧后，若收到设备返回数据，处理之
            //（4.4.1）如果终端的更新程序版本为VA.10以后的版本（通用于所有GEC芯片）
            if (recv.Length == Marshal.SizeOf(typeof(shakeData)))
            {
                fmain.lbl_mainstatus.Text = "运行状态：成功连接到GEC设备";     //底部提示
                softVersion = 2;  //表示终端更新程序为A.10之后的通用版本
                //byte数组转结构体
                shakedata = (shakeData)bytesToStruct(recv, typeof(shakeData));
                //获取握手帧数据
                uecomType = Encoding.Default.GetString(shakedata.uecomType).Replace("\0", "");
                mcuType = Encoding.Default.GetString(shakedata.mcuType).Replace("\0", "");
                uStartAds = shakedata.uStartAds;
                uCodeSize = shakedata.uCodeSize;
                replaceNum = shakedata.replaceNum;
                reserveNum = shakedata.reserveNum;
                //设置设备信息                   
                sTemp = com + "：" + uecomType + " " + mcuType;  //设备信息
                //状态提示
                this.lbl_uartstate.Text = sTemp;     //右上角显示区
                fmain.lbl_mainstatus.Text = "运行状态：" + sTemp;   //底部提示
                fmain.lbl_protocal.Text = "协议：串口";    //底部中间协议类型
                fmain.lbl_location.Text = "协议信息：端口" + com + ",波特率9600";   //底部右侧协议信息
                this.txt_updateinfo1.Text = sTemp + "\r\n";     //右侧更新提示区     
                //如果存在串口，则允许接收调试信息
                if (fmain.mfrm_uartDynamic.emuart.haveUE)
                {

                    //绑定串口结束事件，处理串口调试信息
                    emuart.RawDataReceivedEvent += new EMUART.RawDataReceived(DataRecv);
                    fmain.lbl_mainstatus.Text = "过程提示:串口接收界面开启成功";   //底部提示
                    byte[] info = { (byte)21, (byte)'i' };
                    emuart.bufferClear();   //清除接收数组缓冲区
                    emuart.send(info, out recv, 500, 3); //获得设备的信息在recv中
                    if (recv == null) goto btn_uartcheck_Click_EXIT4;
                    this.txt_updateinfo1.Text = "";     //右侧更新提示区   
                    this.txt_updateinfo1.Text += "动态函数列表的指针容量为" + ((int)(recv[3]<<8|recv[2]<<8|recv[1]<<8|recv[0])).ToString() + "个。";
                    this.txt_updateinfo1.Text += "已使用" + ((int)(recv[7] << 8 | recv[6] << 8 | recv[5] << 8 | recv[4])).ToString() + "个。\r\n";
                    funcount = (int)(recv[3] << 8 | recv[2] << 8 | recv[1] << 8 | recv[0]) - (int)(recv[7] << 8 | recv[6] << 8 | recv[5] << 8 | recv[4]);
                    funnum = ((int)(recv[7] << 8 | recv[6] << 8 | recv[5] << 8 | recv[4]));
                    this.txt_updateinfo1.Text +="动态函数代码区的容量为" + ((int)(recv[15] << 8 | recv[14] << 8 | recv[13] << 8 | recv[12]) - (int)(recv[11] << 8 | recv[10] << 8 | recv[9] << 8 | recv[8])).ToString() + "B。";
                    this.txt_updateinfo1.Text += "已使用" + ((int)(recv[19] << 8 | recv[18] << 8 | recv[17] << 8 | recv[16])).ToString() + "B。\r\n";
                    funsize = ((int)(recv[15] << 8 | recv[14] << 8 | recv[13] << 8 | recv[12]) - (int)(recv[11] << 8 | recv[10] << 8 | recv[9] << 8 | recv[8])) - ((int)(recv[19] << 8 | recv[18] << 8 | recv[17] << 8 | recv[16]));
                    this.txt_updateinfo1.Text +="动态函数列表的使用情况为:\r\n";
                    int i = 20;
                    while (i < recv.Length)
                    {
                        this.txt_updateinfo1.Text += ((char)recv[i + 3]).ToString() + ((char)recv[i + 2]).ToString() + ((char)recv[i + 1]).ToString() + ((char)recv[i]).ToString() + "构件使用" + ((int)(recv[i + 7] << 8 | recv[i + 6])+2).ToString() + "-" + ((int)(recv[i + 5] << 8 | recv[i + 4])).ToString() + "号指针\r\n";
                        i += 8;
                    }
                    emuart.bufferClear();   //清除接收数组缓冲区
                    btn_autoupdate1.Enabled = true;
                }


            }
            //（4.4.2）如果终端的更新程序版本为VA.10及其以前的版本
            else if (System.Text.Encoding.Default.GetString(recv).Contains("shake:GEC-"))
            {
                softVersion = 1;   //表示终端更新程序为A.10及之前的版本
                sTemp = "MKL36Z64" + "(" + com + ")";//设备信息
                this.lbl_uartstate.Text = "成功连接" + sTemp;       //右上角显示区
                fmain.lbl_mainstatus.Text = "运行状态：正在连接GEC...成功连接" + sTemp;   //底部提示
                this.lbl_uartstate.Text = "找到终端设备" + sTemp;   //右上角显示区
                fmain.lbl_protocal.Text = "协议：串口";    //底部中间协议类型
                fmain.lbl_location.Text = "协议信息：端口" + com + ",波特率9600";   //底部右侧协议信息
                //如果存在串口，则允许接收调试信息
                if (fmain.mfrm_uartDynamic.emuart.haveUE)
                {
                    //绑定串口结束事件，处理串口调试信息
                    emuart.RawDataReceivedEvent += new cn.edu.suda.sumcu.iot.EMUART.RawDataReceived(DataRecv);
                    fmain.lbl_mainstatus.Text = "过程提示:串口接收界面开启成功";   //底部提示
                }
            }
            else if (recv.Length == Marshal.SizeOf(typeof(newshakeData)))
            {
                fmain.lbl_mainstatus.Text = "运行状态：成功连接到GEC设备";     //底部提示
                softVersion = 2;  //表示终端更新程序为A.10之后的通用版本
                //byte数组转结构体
                newshakedata = (newshakeData)bytesToStruct(recv, typeof(newshakeData));
                //获取握手帧数据
                uecomType = Encoding.Default.GetString(newshakedata.uecomType).Replace("\0", "");
                mcuType = Encoding.Default.GetString(newshakedata.mcuType).Replace("\0", "");
                uStartAds = newshakedata.uStartAds;
                uCodeSize = newshakedata.uCodeSize;
                replaceNum = newshakedata.replaceNum;
                reserveNum = newshakedata.reserveNum;
                //设置设备信息                   
                sTemp = com + "：" + uecomType + " " + mcuType;  //设备信息
                //状态提示
                this.lbl_uartstate.Text = sTemp;     //右上角显示区
                fmain.lbl_mainstatus.Text = "运行状态：" + sTemp;   //底部提示
                fmain.lbl_protocal.Text = "协议：串口";    //底部中间协议类型
                fmain.lbl_location.Text = "协议信息：端口" + com + ",波特率9600";   //底部右侧协议信息
                this.txt_updateinfo1.Text = sTemp + "\r\n";     //右侧更新提示区     
                //如果存在串口，则允许接收调试信息
                if (fmain.mfrm_uartDynamic.emuart.haveUE)
                {

                    //绑定串口结束事件，处理串口调试信息
                    emuart.RawDataReceivedEvent += new EMUART.RawDataReceived(DataRecv);
                    fmain.lbl_mainstatus.Text = "过程提示:串口接收界面开启成功";   //底部提示
                    byte[] info = { (byte)21, (byte)'i' };
                    emuart.bufferClear();   //清除接收数组缓冲区
                    emuart.send(info, out recv, 500, 3); //获得设备的信息在recv中
                    if (recv == null) goto btn_uartcheck_Click_EXIT4;
                    this.txt_updateinfo1.Text = "";     //右侧更新提示区   
                    this.txt_updateinfo1.Text += "动态函数列表的指针容量为" + ((int)(recv[3] << 8 | recv[2] << 8 | recv[1] << 8 | recv[0])).ToString() + "个。";
                    this.txt_updateinfo1.Text += "已使用" + ((int)(recv[7] << 8 | recv[6] << 8 | recv[5] << 8 | recv[4])).ToString() + "个。\r\n";
                    funcount = (int)(recv[3] << 8 | recv[2] << 8 | recv[1] << 8 | recv[0]) - (int)(recv[7] << 8 | recv[6] << 8 | recv[5] << 8 | recv[4]);
                    funnum = ((int)(recv[7] << 8 | recv[6] << 8 | recv[5] << 8 | recv[4]));
                    this.txt_updateinfo1.Text += "动态函数代码区的容量为" + ((int)(recv[15] << 8 | recv[14] << 8 | recv[13] << 8 | recv[12]) - (int)(recv[11] << 8 | recv[10] << 8 | recv[9] << 8 | recv[8])).ToString() + "B。";
                    this.txt_updateinfo1.Text += "已使用" + ((int)(recv[19] << 8 | recv[18] << 8 | recv[17] << 8 | recv[16])).ToString() + "B。\r\n";
                    funsize = ((int)(recv[15] << 8 | recv[14] << 8 | recv[13] << 8 | recv[12]) - (int)(recv[11] << 8 | recv[10] << 8 | recv[9] << 8 | recv[8])) - ((int)(recv[19] << 8 | recv[18] << 8 | recv[17] << 8 | recv[16]));
                    this.txt_updateinfo1.Text += "动态函数列表的使用情况为:\r\n";
                    int i = 20;
                    while (i < recv.Length)
                    {
                        this.txt_updateinfo1.Text += ((char)recv[i + 3]).ToString() + ((char)recv[i + 2]).ToString() + ((char)recv[i + 1]).ToString() + ((char)recv[i]).ToString() + "构件使用" + ((int)(recv[i + 7] << 8 | recv[i + 6]) + 2).ToString() + "-" + ((int)(recv[i + 5] << 8 | recv[i + 4])).ToString() + "号指针\r\n";
                        i += 8;
                    }
                    emuart.bufferClear();   //清除接收数组缓冲区
                    btn_autoupdate1.Enabled = true;
                }


            }
            //（4.4.3）若收到错误返回信息，退出函数
            else goto btn_uartcheck_Click_EXIT3;

         //(5)退出区
        //(5.1)退出函数
        btn_uartcheck_Click_EXIT:
            return;

        //(5.2)不存在可用串口
        btn_uartcheck_Click_EXIT1:
            this.btn_autoupdate1.Enabled = false;    //禁止串口更新操作
            this.lbl_uartstate.Text = "当前不存在可用串口";       //右上角显示区
            fmain.lbl_mainstatus.Text = "运行状态：当前不存在可用串口";   //底部提示
            fmain.lbl_protocal.Text = "协议：";    //底部中间协议类型
            fmain.lbl_location.Text = "协议信息："; //底部右侧协议信息
            this.txt_updateinfo1.Text = "没有找到USB串口，可能原因如下：\r\n（1）USB串口未插上PC\r\n（2）PC未安装串口驱动\r\n";     //右侧更新提示区
            goto btn_uartcheck_Click_EXIT;

        //(5.3)存在串口，但不存在emuar设备
        btn_uartcheck_Click_EXIT2:
            emuart.terminate(9600); //发送数据给终端设备，让终端设备清空其数据缓冲区
            this.lbl_uartstate.Text = "已连接串口" + com + "但未找到设备";       //右上角显示区
            fmain.lbl_mainstatus.Text = "运行状态：已连接串口" + com + "但未找到设备";   //底部提示
            fmain.lbl_protocal.Text = "协议：";   //底部中间协议类型
            fmain.lbl_location.Text = "协议信息：";    //底部右侧协议信息
            fmain.lbl_mainstatus.Text = "运行状态：与设备建立连接失败，请尝试重新连接设备";   //底部提示
            //右侧更新提示区
            this.txt_updateinfo1.Text = "有USB串口，但未连上终端，可能原因如下：\r\n（1）USB串口驱动需更新\r\n（2）USB串口未连接终端\r\n（3）终端程序未运行\r\n";
            goto btn_uartcheck_Click_EXIT;

        //(5.4)未收到返回信息或收到错误返回信息
        btn_uartcheck_Click_EXIT3:
            emuart.terminate(9600); //发送数据给终端设备，让终端设备清空其数据缓冲区
            fmain.lbl_mainstatus.Text = "运行状态：正在连接GEC...未收到返回信息";     //底部提示
            this.lbl_uartstate.Text = "找到GEC在" + com + "但握手失败，请单击[再次连接GEC并检测容量]按钮";  //右上角显示
            goto btn_uartcheck_Click_EXIT;
        btn_uartcheck_Click_EXIT4:
            emuart.terminate(9600); //发送数据给终端设备，让终端设备清空其数据缓冲区
            fmain.lbl_mainstatus.Text = "运行状态：未收到返回容量信息";     //底部提示
            this.lbl_uartstate.Text = "找到GEC在" + com + "但未收到返回容量信息，请单击[再次连接GEC并检测容量]按钮";  //右上角显示
            goto btn_uartcheck_Click_EXIT;

        }

        //======================================================================
        //函数名称：btn_fileOpen1_Click
        //函数返回：无
        //参数说明：无
        //功能概要：导入待整体更新Hex文件并对该文件进行解析取出其有效数据
        //======================================================================

        //======================================================================
        //函数名称：btn_autoUpdate1_Click
        //函数返回：无
        //参数说明：无
        //功能概要：“一键自动更新”（整体更新）按钮点击事件，进行程序更新操作
        //======================================================================
        private void btn_autoupdate1_Click(object sender, EventArgs e)
        {
            byte[] recv = null;//保存串口接收信息
            string hex = this.textBox1.Text;
            hex = hex.ToUpper();
            int length = hex.Length / 2;
            char[] hexChars = hex.ToCharArray();
            byte[] d = new byte[length];
            int size = 0;
            for (int i = 0; i < hex.Length; i++)
            {
                if (hexChars[i] <= 57) hexChars[i] = (char)(hexChars[i] - 48);
                if (hexChars[i] >= 65) hexChars[i] = (char)(hexChars[i] - 55);                
            }
            for (int i = 0; i < length; i++)
            {
                int pos = i * 2;
                d[i] = (byte)((hexChars[pos] << 4) | (hexChars[pos + 1]));
            }
            byte[] data = new byte[10 + dcount * 2 + length];
            data[0] = (byte)21;
            data[1] = (byte)'a';
            data[2] = (byte)dname[0];
            data[3] = (byte)dname[1];
            data[4] = (byte)dname[2];
            data[5] = (byte)dname[3];
            data[6] = (byte)(funnum>>8);
            data[7] = (byte)(funnum&0x00FF);
            data[8] = (byte)((funnum + dcount+1)>>8);
            data[9] = (byte)((funnum + dcount + 1)&0x00FF);
            for (int i = 0; i < dcount * 2; i+=2)
            {
                data[10 + i] = (byte)(dlength[i / 2] >> 8);
                data[11 + i] = (byte)(dlength[i / 2] & 0x00FF);
                size += (data[10 + i] << 8 | data[11 + i]);
                if (size > funsize)
                {
                    this.txt_updateinfo1.Text= "机器码大小超过容量限制。\r\n";     //右侧更新提示区  
                    return;
                }
            }
            emuart.bufferClear();   //清除接收数组缓冲区
            Array.Copy(d, 0, data, 10 + dcount * 2, length);
            emuart.send(data, out recv, 500, 3); //获得设备的信息在recv中
            this.txt_updateinfo1.Text += "机器码发送成功。\r\n";     //右侧更新提示区  

            if (recv == null)
            { 
                this.txt_updateinfo1.Text = "未成功接收到返回信息，机器码可能发送失败。\r\n";
                return;
            }
            this.txt_updateinfo1.Text = "";     //右侧更新提示区   
            this.txt_updateinfo1.Text += "动态函数列表的指针容量为" + ((int)(recv[3] << 8 | recv[2] << 8 | recv[1] << 8 | recv[0])).ToString() + "个。";
            this.txt_updateinfo1.Text += "已使用" + ((int)(recv[7] << 8 | recv[6] << 8 | recv[5] << 8 | recv[4])).ToString() + "个。\r\n";
            funcount = (int)(recv[3] << 8 | recv[2] << 8 | recv[1] << 8 | recv[0]) - (int)(recv[7] << 8 | recv[6] << 8 | recv[5] << 8 | recv[4]);
            funnum = ((int)(recv[7] << 8 | recv[6] << 8 | recv[5] << 8 | recv[4]));
            this.txt_updateinfo1.Text += "动态函数代码区的容量为" + ((int)(recv[15] << 8 | recv[14] << 8 | recv[13] << 8 | recv[12]) - (int)(recv[11] << 8 | recv[10] << 8 | recv[9] << 8 | recv[8])).ToString() + "B。";
            this.txt_updateinfo1.Text += "已使用" + ((int)(recv[19] << 8 | recv[18] << 8 | recv[17] << 8 | recv[16])).ToString() + "B。\r\n";
            funsize = ((int)(recv[15] << 8 | recv[14] << 8 | recv[13] << 8 | recv[12]) - (int)(recv[11] << 8 | recv[10] << 8 | recv[9] << 8 | recv[8])) - ((int)(recv[19] << 8 | recv[18] << 8 | recv[17] << 8 | recv[16]));
            this.txt_updateinfo1.Text += "动态函数列表的使用情况为:\r\n";
            int j = 20;
            while (j< recv.Length)
            {
                this.txt_updateinfo1.Text += ((char)recv[j + 3]).ToString() + ((char)recv[j + 2]).ToString() + ((char)recv[j + 1]).ToString() + ((char)recv[j]).ToString() + "构件使用" + ((int)(recv[j + 7] << 8 | recv[j + 6]) + 2).ToString() + "-" + ((int)(recv[j + 5] << 8 | recv[j+ 4])).ToString() + "号指针\r\n";
                j += 8;
            }
            emuart.bufferClear();   //清除接收数组缓冲区




            ////create .h file
            //string newTxtPath = Environment.CurrentDirectory + @"\" + comboBox1.Text.Replace(" ", "") + ".txt";
            //StreamWriter sw = new StreamWriter(newTxtPath, false, Encoding.Default);//实例化StreamWriter
            //sw.WriteLine("//请注意：本文件仅给出构件内函数的声明语句。");
            //sw.WriteLine("//             本文件不可直接取代原本构件的头文件。");
            //sw.WriteLine("//             使用时请将原本构件的函数声明(包括静态函数)删除或注释。");
            //int count = uframe[2];
            //for (int i = 0; i < count; i++)
            //{
            //    if (hfile[i].Contains("static"))
            //        continue;
            //    string[] declare;
            //    declare = hfile[i].Split(' ');
            //    string retu = declare[0];
            //    declare = declare[1].Split('(');
            //    string name = declare[0];
            //    declare = System.Text.RegularExpressions.Regex.Split(hfile[i], name, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            //    sw.WriteLine("#define " + name + " ((" + retu + " (*)" + declare[1] + ")(bios_fun_point[" + (61 + i).ToString() + "]))");
            //}
            //sw.Flush();
            //sw.Close();
        }
        private static byte[] charToByte(char c)
        {
            byte[] b = new byte[2];
            b[0] = (byte)((c & 0xFF00) >> 8);
            b[1] = (byte)(c & 0xFF);
            return b;
        }

        //======================================================================
        //函数名称：txtShow1
        //函数返回：无
        //参数说明：str:在终端执行信息提示框追加显示的内容
        //功能概要：整体更新过程中，在终端执行信息提示框txt_uartinfo1中追加提示
        //          信息，并显示最新信息      
        //======================================================================
        byte lastCH = 0;    //【20190506】-2/3
        private void txtShow1(byte[] data)
        {
            int count = 0;
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
            //将字符串转为汉字
            string str = Encoding.GetEncoding("GBK").GetString(showData);
            //右侧更新提示区
            if (this.txt_updateinfo1.Lines.Length >= 220) this.txt_updateinfo1.Text = String.Empty; //长度过长则清空内容
            this.txt_updateinfo1.Text += str;
            this.txt_updateinfo1.Refresh();
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
            byte[] showData = data;
            if (this.txt_updateinfo1.InvokeRequired)
            {
                this.txt_updateinfo1.Invoke(new Action(() => { txtShow1(showData); }));
            }
            else
                txtShow1(showData);
            emuart.bufferClear();
            Application.DoEvents();
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
                    emuart.RawDataReceivedEvent -= new EMUART.RawDataReceived(DataRecv);//解除事件绑定的方法
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
        private void frm_uartDynamic_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (emuart._Uartport != null)
                emuart._Uartport.Dispose();
            this.Close();
        }

        /// <summary>
        /// 退出串口更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCloseUpdate_Click(object sender, EventArgs e)
        {
            if (emuart._Uartport != null)
                emuart._Uartport.Dispose();
            this.Close();
        }

        private void btn_fileopen1_Click_1(object sender, EventArgs e)
        {
            //（1）变量声明           
            string filePath;          //文件路径
            //（2.1）导入Hex文件
            OpenFileDialog ofd = new OpenFileDialog();  //打开文件对话框
            ofd.Filter = "hex file(*.lst)|*.lst";
            ofd.ShowDialog();
            //（2.2）获取文件名
            filePath = ofd.FileName;
            if (filePath == "")
            {
                txtShow1("导入失败！\r\n");     //右侧更新提示区  
                return;
            }
            lstfile = filePath;
            txtShow1("导入" + lstfile + "文件成功！\r\n");     //右侧更新提示区  
            if (File.Exists(filePath))
            {
                StreamReader sr = new StreamReader(filePath, Encoding.Default);
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains(">:"))
                    {
                        string[] name;
                        name = line.Split('<');
                        name = name[1].Split('>');
                        char[] c;
                        char[] c5 = { ' ', ' ', ' ', ' ', ' ' };
                        c = name[0].ToCharArray();
                        if (c[0] == '_' || ((c[0] >= 65) && (c[0] <= 90))) continue;//不允许构件名为下划线开头或者大写字母
                        for (int i = 0; i <= 4; i++)
                        {
                            if (i < c.Length && c[i] != '_')
                            {
                                c5[i] = c[i];
                            }
                            else
                                break;
                        }
                        string s = new string(c5);
                        if (!this.comboBox1.Items.Contains(s))
                            this.comboBox1.Items.Add(s);
                    }
                }
                ArrayList al = new ArrayList();
                for (int i = 0; i < comboBox1.Items.Count; i++)
                {
                    string a = comboBox1.Items[i].ToString();
                    al.Add(a);
                }
                al.Sort();
                comboBox1.Items.Clear();
                for (int i = 0; i < al.Count; i++)
                {
                    comboBox1.Items.Add(al[i]);
                }
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            string filePath = lstfile;
            if (File.Exists(filePath))
            {
                string head = "46";
                string length = "";
                string frame1 = "";
                string frame = "";
                int num = 0;
                for (int i = 1; i <= 15; i++)
                {
                    TextBox tb = (TextBox)this.Controls.Find("add_textBox" + i.ToString(), true)[0];
                    if (tb.Text.Contains(";") == false)
                        continue;
                    string[] name_num;
                    name_num = tb.Text.Split(';');
                    if (name_num[0] == "" || name_num[1] == "")
                    {
                        MessageBox.Show("添加函数格式错误！");
                        return;
                    }
                    num++;
                    //取函数名对应的机器码
                    StreamReader sr = new StreamReader(filePath, Encoding.Default);
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Contains("<" + name_num[0] + ">:"))
                        {
                            while ((line = sr.ReadLine()) != null)
                            {
                                if (line.Contains(":\t") && line.Contains(" \t"))
                                {
                                    string[] lst = line.Split('\t');
                                    lst[1] = lst[1].Replace(" ", "");
                                    if (!line.Contains("word"))
                                    {
                                        if (lst[1].Length == 4)
                                        {
                                            char[] temp1 = lst[1].ToCharArray();
                                            char[] temp2 = lst[1].ToCharArray();
                                            temp1[0] = temp2[2];
                                            temp1[1] = temp2[3];
                                            temp1[2] = temp2[0];
                                            temp1[3] = temp2[1];
                                            lst[1] = new string(temp1);
                                        }
                                        else
                                        {
                                            char[] temp1 = lst[1].ToCharArray();
                                            char[] temp2 = lst[1].ToCharArray();
                                            temp1[0] = temp2[2];
                                            temp1[1] = temp2[3];
                                            temp1[2] = temp2[0];
                                            temp1[3] = temp2[1];
                                            temp1[4] = temp2[6];
                                            temp1[5] = temp2[7];
                                            temp1[6] = temp2[4];
                                            temp1[7] = temp2[5];
                                            lst[1] = new string(temp1);
                                        }
                                    }
                                    else
                                    {
                                        char[] temp1 = lst[1].ToCharArray();
                                        char[] temp2 = lst[1].ToCharArray();
                                        temp1[0] = temp2[6];
                                        temp1[1] = temp2[7];
                                        temp1[2] = temp2[4];
                                        temp1[3] = temp2[5];
                                        temp1[4] = temp2[2];
                                        temp1[5] = temp2[3];
                                        temp1[6] = temp2[0];
                                        temp1[7] = temp2[1];
                                        lst[1] = new string(temp1);
                                    }
                                    frame1 += lst[1];
                                }
                                if (line.Contains(">:"))
                                    break;
                            }
                            break;
                        }
                    }
                    //加入frame
                    string h = Ten2Hex(name_num[1]);
                    string l = Ten2Hex((frame1.Length / 2).ToString());
                    if (l.Length == 1) l = "000" + l;
                    if (l.Length == 2) l = "00" + l;
                    if (l.Length == 3) l = "0" + l;
                    length += ((h.Length == 1) ? ("0" + h) : (h)) + l;
                    frame += frame1;
                    frame1 = "";
                }
                string num1 = Ten2Hex(num.ToString());
                head += ((num1.Length == 1) ? ("0" + num1) : (num1));
                frame = head + length + frame;
                frame = frame.ToUpper();
                textBox1.Text = frame;
            }
            else
            {
                MessageBox.Show("文件不存在！");
            }
        }

        /// <summary>
        /// 从十进制转换到十六进制
        /// </summary>
        /// <param name="ten"></param>
        /// <returns></returns>
        public static string Ten2Hex(string ten)
        {
            ulong tenValue = Convert.ToUInt64(ten);
            ulong divValue, resValue;
            string hex = "";
            do
            {
                //divValue = (ulong)Math.Floor(tenValue / 16);

                divValue = (ulong)Math.Floor((decimal)(tenValue / 16));

                resValue = tenValue % 16;
                hex = tenValue2Char(resValue) + hex;
                tenValue = divValue;
            }
            while (tenValue >= 16);
            if (tenValue != 0)
                hex = tenValue2Char(tenValue) + hex;
            return hex;
        }

        public static string tenValue2Char(ulong ten)
        {
            switch (ten)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    return ten.ToString();
                case 10:
                    return "A";
                case 11:
                    return "B";
                case 12:
                    return "C";
                case 13:
                    return "D";
                case 14:
                    return "E";
                case 15:
                    return "F";
                default:
                    return "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dname = comboBox1.Text.Replace(" ", "");
            int h = 0;
            string filePath = lstfile;
            if (File.Exists(filePath))
            {
                hfile = new string[32];
                StreamReader sr = new StreamReader(filePath, Encoding.Default);
                String line;
                int i=0;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains(">:") && line.Contains("<" + dname))
                    {
                        string[] name;
                        name = line.Split('<');
                        name = name[1].Split('>');
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.Contains(name[0] + "("))
                            {

                                hfile[h] = line;
                                h++;
                                i++;
                                break;
                            }
                        }
                        if (i > funcount)
                        {
                            MessageBox.Show("函数指针剩余数量不足！");
                            return;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("文件不存在！");
                return;
            }
            dcount = h;
            if (File.Exists(filePath))
            {
                string head = "46";
                string length = "";
                string frame1 = "";
                string frame = "";
                int num = 0;
                dlength=new int[32];
                for (int i = 0; i < hfile.Length; i++)
                {
                    if (hfile[i] == null)
                        continue;
                    string[] name;
                    name = hfile[i].Split('(');
                    name = name[0].Split(' ');
                    if (name[name.Length-1]=="")
                    {
                        MessageBox.Show("添加函数格式错误！");
                        return;
                    }
                    //取函数名对应的机器码
                    StreamReader sr = new StreamReader(filePath, Encoding.Default);
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Contains("<" + name[name.Length - 1] + ">:"))
                        {
                            while ((line = sr.ReadLine()) != null)
                            {
                                if (line.Contains(":\t") && line.Contains(" \t"))
                                {
                                    string[] lst = line.Split('\t');
                                    lst[1] = lst[1].Replace(" ", "");
                                    if (!line.Contains("word"))
                                    {
                                        if (lst[1].Length == 4)
                                        {
                                            char[] temp1 = lst[1].ToCharArray();
                                            char[] temp2 = lst[1].ToCharArray();
                                            temp1[0] = temp2[2];
                                            temp1[1] = temp2[3];
                                            temp1[2] = temp2[0];
                                            temp1[3] = temp2[1];
                                            lst[1] = new string(temp1);
                                        }
                                        else
                                        {
                                            char[] temp1 = lst[1].ToCharArray();
                                            char[] temp2 = lst[1].ToCharArray();
                                            temp1[0] = temp2[2];
                                            temp1[1] = temp2[3];
                                            temp1[2] = temp2[0];
                                            temp1[3] = temp2[1];
                                            temp1[4] = temp2[6];
                                            temp1[5] = temp2[7];
                                            temp1[6] = temp2[4];
                                            temp1[7] = temp2[5];
                                            lst[1] = new string(temp1);
                                        }
                                    }
                                    else
                                    {
                                        char[] temp1 = lst[1].ToCharArray();
                                        char[] temp2 = lst[1].ToCharArray();
                                        temp1[0] = temp2[6];
                                        temp1[1] = temp2[7];
                                        temp1[2] = temp2[4];
                                        temp1[3] = temp2[5];
                                        temp1[4] = temp2[2];
                                        temp1[5] = temp2[3];
                                        temp1[6] = temp2[0];
                                        temp1[7] = temp2[1];
                                        lst[1] = new string(temp1);
                                    }
                                    frame1 += lst[1];
                                }
                                if (line.Contains(">:"))
                                    break;
                            }
                            break;
                        }
                    }
                    //加入frame
                    dlength[i] = frame1.Length / 2;
                    frame += frame1;
                    frame1 = "";

                }
                frame = frame.ToUpper();        
                textBox1.Text = frame;
            }
            else
            {
                MessageBox.Show("文件不存在！");
            }
        }



























    }
}


