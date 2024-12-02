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
using System.Reflection;
using SerialPort;

namespace AHL_GEC
{
    public partial class frm_uartUpdate : Form
    {
        //定义使用的全局变量
        private FrmMain fmain;  //主页面变量
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


        private bool closing = false;      //标识是否正在关闭串口
        private bool listening = false;    //标识是否执行完invoke相关操作
        //方法一：使用Thread类
        ThreadStart threadStart= null;　
        Thread thread = null;

        #region 重新打开串口使用的变量
        public SCI sci;
        #endregion


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
        //函数名称：frm_uartUpdate
        //函数返回：frm_uartUpdate窗体构造函数
        //参数说明：无
        //功能概要：完成frm_uartUpdate窗体的初始化工作
        //======================================================================
        public frm_uartUpdate()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        //======================================================================
        //函数名称：frm_uartUpdate_Load
        //函数返回：无
        //参数说明：无
        //功能概要：窗体加载事件，窗体加载时自动调用
        //======================================================================
        private void frm_uartUpdate_Load(object sender, EventArgs e)
        {
            //（1）本窗体由frmMain创建，所以本窗体的父窗体为frmMain
            fmain = FrmMain.getInstance();
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

            //
            this.btn_autoupdate1.RectColor = Color.Maroon;
            this.infor_dispaly_button.RectColor = Color.Maroon;
            this.btnCloseUpdate.RectColor = Color.Maroon;

            //关闭Thread(用于统计1S内接收的消息条目数)-【20190507】 2/2  
            //(6)用Thread类
            threadStart = new ThreadStart(mes_count);//通过ThreadStart委托告诉子线程执行什么方法　　
            thread = new Thread(threadStart);
            thread.Start();//启动新线程
        }

        //======================================================================
        //函数名称：btn_uartcheck_Click
        //函数返回：无
        //参数说明：无
        //功能概要：“连接GEC”按钮点击事件，重新连接终端
        //======================================================================
        private void btn_uartcheck_Click(object sender, EventArgs e)
        {
            //【1】变量声明
            int ret;            //返回值
            string com = "";    //串口信息
            string sTemp = "";
            byte[] recv = null;//保存串口接收信息
            byte[] shake = { (byte)10, (byte)'s', (byte)'h', (byte)'a', (byte)'k', (byte)'e' }; //与终端握手帧数据


            //【2】清除一些可能余留信息
            this.lbl_uartstate.Text = "";       //右上角显示区
            fmain.lbl_mainstatus.Text = "运行状态：正在连接GEC...";     //底部提示
            this.lst_codeshow1.Items.Clear();   //左侧代码显示区
            this.txt_updateinfo1.Text = "";     //右侧更新提示区
            this.Refresh();                     //刷新显示 
            btn_uartcheck.Text = "重新连接GEC"; //更改显示文字            

            //【3】重新遍历串口，寻找终端
            if (emuart._Uartport != null)
            {
                //【20191024】  (3.1)让耗时操作做完，如果不做完耗时操作，无法进行下面的事件，且界面不响应
                Application.DoEvents();
                emuart._Uartport.Close();
                emuart._Uartport.Dispose();
                //【20200401】 姜家乐 增加延时函数
                Thread.Sleep(10);
            }
            //Thread.Sleep(50);   //等待串口关闭
            emuart.RawDataReceivedEvent -= new EMUART.RawDataReceived(DataRecv);//解除事件绑定的方法，加快握手速度
            if (emuart != null) emuart = null;
            
            emuart = EMUART.getInstance();    //每次“重新连接”重新实例化
            fmain.lbl_mainstatus.Text = "运行状态：正在连接GEC...正在查找匹配设备...";     //底部提示
            ret = emuart.findDevice(out com, 115200);  //寻找emuart设备
            this.lbl_uartstate.Text = com;       //右上角显示区
            fmain.lbl_mainstatus.Text = "运行状态：正在连接GEC...尝试与匹配设备建立通信...";     //底部提示

            //【4】根据寻找串口的返回值确定执行下列分支
            //【4.1】如果连接终端的emuart失败，退出函数
            if (ret == 1) goto btn_uartcheck_Click_EXIT1;

            //【4.2】找到串口，没有找到UE，退出函数
            else if (ret == 2) goto btn_uartcheck_Click_EXIT2;

            //【4.3】如果找到串口与UE,向设备发送握手帧
            fmain.lbl_mainstatus.Text = "运行状态：正在连接GEC...向该设备发送握手信息，准备接收设备返回消息...";     //底部提示
            emuart.bufferClear();   //清除接收数组缓冲区
            emuart.send(shake, out recv, 100, 3); //获得设备的信息在recv中
            this.btn_fileopen1.Enabled = true;     //允许选择文件功能
            fmain.lbl_mainstatus.Text = "运行状态：正在连接GEC...正在处理设备返回信息...";     //底部提示
            if (recv == null) goto btn_uartcheck_Click_EXIT3;//未收到数据，退出   
           

            //【4.4】发送握手帧后，若收到设备返回数据，处理之
            //【4.4.1】如果终端的更新程序版本为VA.10以后的版本（通用于所有GEC芯片）
            int length1 = Marshal.SizeOf(typeof(shakeData));
            int length = Marshal.SizeOf(typeof(newshakeData));

            if (recv.Length == Marshal.SizeOf(typeof(shakeData)))
            {
                fmain.lbl_mainstatus.Text = "运行状态：成功连接到GEC设备";     //底部提示
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
                fmain.lbl_mainstatus.Text = "运行状态：" + sTemp;   //底部提示
                fmain.lbl_protocal.Text = "协议：串口";    //底部中间协议类型
                fmain.lbl_location.Text = "协议信息：端口" + com + ",波特率115200";   //底部右侧协议信息
                this.txt_updateinfo1.Text = sTemp + "\r\n";     //右侧更新提示区     
                //若保留更新最大字节为0表示目前没有User程序，此时只能进行整体更新
                
                if (reserveNum == 0)
                {
                    txtShow1("可进行GEC中User程序更新\r\n");     //右侧更新提示区  
                }
                //如果存在串口，则允许接收调试信息
                if (this.emuart.haveUE)
                {
                    //绑定串口结束事件，处理串口调试信息
                    emuart.RawDataReceivedEvent += new EMUART.RawDataReceived(DataRecv);
                    fmain.lbl_mainstatus.Text = "过程提示:串口接收界面开启成功";   //底部提示
                }
            }

            //【4.4.2】如果终端的更新程序版本为VA.10及其以前的版本
            else if (System.Text.Encoding.Default.GetString(recv).Contains("shake:GEC-"))
            {
                softVersion = 1;   //表示终端更新程序为A.10及之前的版本
                sTemp = "MKL36Z64" + "(" + com + ")";//设备信息
                this.lbl_uartstate.Text = "成功连接" + sTemp;       //右上角显示区
                fmain.lbl_mainstatus.Text = "运行状态：正在连接GEC...成功连接" + sTemp;   //底部提示
                this.lbl_uartstate.Text = "找到终端设备" + sTemp;   //右上角显示区
                fmain.lbl_protocal.Text = "协议：串口";    //底部中间协议类型
                fmain.lbl_location.Text = "协议信息：端口" + com + ",波特率115200";   //底部右侧协议信息
                //如果存在串口，则允许接收调试信息
                if (fmain.mfrm_uartUpdate.emuart.haveUE)
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
                sTemp = com + "：" + " " + mcuType;  //设备信息
                //#region 记录设备信息，重新打开串口
                //PublicVar.g_SCIComNum = com;
                //#endregion
                //状态提示
                this.lbl_uartstate.Text = sTemp;     //右上角显示区
                this.lbl_uartstate.Text += " " + biosVersion;
                fmain.lbl_mainstatus.Text = "运行状态：" + sTemp;   //底部提示
                fmain.lbl_protocal.Text = "协议：串口";    //底部中间协议类型
                fmain.lbl_location.Text = "协议信息：端口" + com + ",波特率115200";   //底部右侧协议信息
                this.txt_updateinfo1.Text = sTemp + "\r\n";     //右侧更新提示区     
                //若保留更新最大字节为0表示目前没有User程序，此时只能进行整体更新
                if (reserveNum == 0)
                {
                    txtShow1("可进行GEC中User程序更新\r\n");     //右侧更新提示区  
                }
                //如果存在串口，则允许接收调试信息
                if (this.emuart.haveUE)
                {
                    //绑定串口结束事件，处理串口调试信息
                    emuart.RawDataReceivedEvent += new EMUART.RawDataReceived(DataRecv);
                    fmain.lbl_mainstatus.Text = "过程提示:串口接收界面开启成功";   //底部提示
                    btn_uartcheck.Symbol = 61475;
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
            this.btn_autoupdate1.Enabled = false;    //禁止串口更新操作
            this.btn_fileopen1.Enabled = false;      //禁止选择文件功能
            this.lbl_uartstate.Text = "当前不存在可用串口";       //右上角显示区
            fmain.lbl_mainstatus.Text = "运行状态：当前不存在可用串口";   //底部提示
            fmain.lbl_protocal.Text = "协议：";    //底部中间协议类型
            fmain.lbl_location.Text = "协议信息："; //底部右侧协议信息
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
            goto btn_uartcheck_Click_EXIT;

        //【5.3】存在串口，但不存在emuar设备
        btn_uartcheck_Click_EXIT2:
            emuart.terminate(115200); //发送数据给终端设备，让终端设备清空其数据缓冲区
            this.lbl_uartstate.Text = "已连接串口" + com + "但未找到设备";       //右上角显示区
            fmain.lbl_mainstatus.Text = "运行状态：已连接串口" + com + "但未找到设备";   //底部提示
            fmain.lbl_protocal.Text = "协议：";   //底部中间协议类型
            fmain.lbl_location.Text = "协议信息：";    //底部右侧协议信息
            fmain.lbl_mainstatus.Text = "运行状态：与设备建立连接失败，请尝试重新连接设备";   //底部提示
            //右侧更新提示区
            this.txt_updateinfo1.Text = "有USB串口，但未连上终端，可能原因如下：\r\n（1）USB串口驱动需更新\r\n（2）USB串口未连接终端\r\n（3）终端程序未运行\r\n";
            closing = true;     //正在关闭串口
            while (listening)
            {
                Application.DoEvents();
            }
            emuart._Uartport.Close();
            emuart._Uartport.Dispose();
            Thread.Sleep(10);
            closing = false;    //关闭完成
            goto btn_uartcheck_Click_EXIT;

        //【5.4】未收到返回信息或收到错误返回信息
        btn_uartcheck_Click_EXIT3:
            emuart.terminate(115200); //发送数据给终端设备，让终端设备清空其数据缓冲区
            fmain.lbl_mainstatus.Text = "运行状态：正在连接GEC...未收到返回信息";     //底部提示
            this.lbl_uartstate.Text = "找到GEC在" + com + "但握手失败，请再次单击[重新连接]按钮";  //右上角显示
            closing = true;     //正在关闭串口
            while (listening)
            {
                Application.DoEvents();
            }
            //【20191205 1/2】  程宏玉   解决无串口连接时单击连接GEC按钮，出现异常问题
            if (emuart._Uartport!=null)
            {
                try
                {
                    Application.DoEvents();
                    emuart._Uartport.Close();
                    emuart._Uartport.Dispose();
                    Thread.Sleep(10);
                }
                catch (Exception)
                {

                } 
            }
            closing = false;    //关闭完成
            goto btn_uartcheck_Click_EXIT;
     
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
                        fmain.lbl_mainstatus.Text = "运行状态：当前导入的hex文件无法烧入目标设备";
                        goto btn_fileopen1_Click_EXIT3;
                    }
                    //（2.6）状态提示
                    this.lbl_filename1.Text = fileName;
                    this.prg_update1.Value = 0;
                    this.prg_update1.Text = "";
                    this.btn_autoupdate1.Enabled = true;  //允许整体更新功能
                    txtShow1("导入" + fileName + "文件成功！\r\n");     //右侧更新提示区  
                    this.lst_codeshow1.Items.Clear();    //左侧代码显示区
                    foreach (string str in hex.getHexStrList())
                    {
                        this.lst_codeshow1.Items.Add(str);    //左侧代码显示区
                    }
                    fmain.lbl_mainstatus.Text = "运行状态：" + fileName + "文件选择成功，文件数据解析成功";   //底部提示
                    if (lbl_uartstate.Text.Contains("MSP432") == true)
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
                    fmain.lbl_mainstatus.Text = "运行状态：" + fileName + "文件导入成功";   //底部提示
                    //（3.3）获取文件数据，并显示在文件显示框
                    FileStream fs = new FileStream(filePath, FileMode.Open);
                    if (fs == null) goto btn_fileopen1_Click_EXIT1;    //文件打开失败
                    fmain.lbl_mainstatus.Text = "运行状态：" + fileName + "文件打开成功，正在读取文件数据...";   //底部提示
                    //（3.4）读取数据
                    StreamReader sr = new StreamReader(fs);  //读取数据流
                    hex_old.clear();  //先清空hex数据，准备接收hex数据
                    this.lst_codeshow1.Items.Clear();    //左侧代码显示区
                    //（3.5）遍历hex文件，读出到文本框中
                    while (true)
                    {
                        line = sr.ReadLine();  //读取1行数据
                        if (line == null) break;  //文件读取错误，退出
                        hex_old.addLine(line);  //获取hex文件行数据有效数据
                        this.lst_codeshow1.Items.Add(line);     //左侧代码显示区                  
                    }
                    sr.Close();  //关闭数据流
                    fs.Close();
                    list = hex_old.getHexList();
                    //（3.6）判断读取文件的合法性,如果不合法
                    if (list[0].address != 0x6800) goto btn_fileopen1_Click_EXIT3;
                    //（3.7）判断读取文件的合法性,如果合法               
                    this.lbl_filename1.Text = fileName;
                    this.prg_update1.Value = 0;
                    this.prg_update1.Text = "";
                    this.btn_autoupdate1.Enabled = true;   //允许整体更新功能
                    txtShow1("导入" + fileName + "文件成功！\r\n");   //右侧更新提示区  
                    fmain.lbl_mainstatus.Text = "运行状态：" + fileName + "文件选择成功，文件数据解析成功";   //底部提示             
                }
	        }
                
        //（4）退出区
        //（4.1）退出函数
        btn_fileopen1_Click_EXIT:  
            return;

        //（4.2）导入hex文件失败
        btn_fileopen1_Click_EXIT1: 
            this.btn_autoupdate1.Enabled = false;  //禁止整体更新功能
            fmain.lbl_mainstatus.Text = "运行状态：请重新选择文件";   //底部提示
            MessageBox.Show("Hex文件异常!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            goto btn_fileopen1_Click_EXIT;

        //（4.3）未选择Hex文件
        btn_fileopen1_Click_EXIT2:
            fmain.lbl_mainstatus.Text = "运行状态：未选择Hex文件";   //底部提示
            goto btn_fileopen1_Click_EXIT;

        //（4.4）导入的Hex文件不合法
        btn_fileopen1_Click_EXIT3:
            fmain.lbl_mainstatus.Text = "运行状态：请重新选择文件";   //底部提示
            MessageBox.Show("Hex文件异常!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            txtShow1("导入的" + fileName + "文件首地址不合法，请检查！\r\n");     //右侧更新提示区  
            this.btn_autoupdate1.Enabled = false;  //不允许整体更新功能
            fmain.lbl_mainstatus.Text = "运行状态：" + fileName + "文件首地址不合法，请检查！";   //底部提示
            goto btn_fileopen1_Click_EXIT;

        }

        //======================================================================
        //函数名称：btn_autoUpdate1_Click
        //函数返回：无
        //参数说明：无
        //功能概要：“一键自动更新”（整体更新）按钮点击事件，进行程序更新操作
        //======================================================================
        private void btn_autoupdate1_Click(object sender, EventArgs e)
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
            btn_autoupdate1.Enabled = false;

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
                    fmain.lbl_mainstatus.Text = "运行状态：整体更新开始";   //底部提示
                    txtShow1("运行状态：整体更新开始\r\n");     //右侧更新提示区  
                    emuart.RawDataReceivedEvent -= new EMUART.RawDataReceived(DataRecv);//解除事件绑定的方法，加快握手速度
                    infor_dispaly_button.Text = "暂停传输";
                    emuart.RawDataReceivedEvent += new EMUART.RawDataReceived(DataRecv);//串口事件绑定


                    //（2.2.1）从update类中取出待发送的数据帧，逐一进行发送
                    while ((senddata = update.getNextFrame()) != null)
                    {
                        cur = update.getNextIndex();
                        //非最后的更新命令帧，需返回数据
                        if (cur != sum - 1 )
                        {
                            //【20201004】 删除【V4.21】添加的测试功能，并进行界面优化
                            //【20201003】 对第1帧进行延时保护，防止出现Flash未写入现象
                            //第26帧做特殊处理，防止延迟
                            if (cur == 25)
                            {
                                int aaa = 10;
                            }
             
                            if (!emuart.send(senddata, out recvdata, 1000, 2)) goto btn_autoupdate1_Click_EXIT5;  //未接收到正确返回信息
                            if (update.updateRecv(recvdata) != 0) goto btn_autoupdate1_Click_EXIT6;  //更新返回帧异常
                            //当前帧数据操作成功，进度条显示更新进度
                            this.prg_update1.Value = (cur + 1) * 100 / sum;            //进度条显示
                            this.prg_update1.Text = (cur + 1) * 100 / sum + "%";  //进度百分比显示
                            this.prg_update1.Refresh();
                            //成功提示
                            txtShow1("当前第" + (cur + 1).ToString() + "/" + sum.ToString() + "帧 \r\n");     //右侧更新提示区


                        }
                        //最后一帧更新命令帧，仅需发送
                        else
                        {
                            txtShow1("当前第" + (cur + 1).ToString() + "/" + sum.ToString() + "帧 \r\n");     //右侧更新提示区
                            fmain.lbl_mainstatus.Text = "运行状态：程序整体更新成功";   //底部提示
                            txtShow1("程序整体更新成功\r\n");     //右侧更新提示区  
                            this.prg_update1.Value = 100;
                            this.prg_update1.Text = "100%";
                            this.prg_update1.Refresh();
                            emuart.send(senddata, out recvdata, 0, 1); //令发送等待时间为0 20190516（1/2） 【20210225】处理二次提示 
                            System.Threading.Thread.Sleep(100); //延时100毫秒
                            this.btn_autoupdate1.Enabled = true;
                            return;
                        }
                    }
                    //（2.3）更新成功 
                    goto btn_autoupdate1_Click_EXIT3;
                }
                catch (Exception ex)
                {
                    fmain.lbl_mainstatus.Text = "运行状态：系统异常退出";   //底部提示
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
                fmain.lbl_mainstatus.Text = "";   //左下提示区
                txtShow1("");     //右侧更新提示区  
                //（3.3）Hex文件选择成功、串口选择成功则进行自动更新操作//右下提示区
                update_old = new Update_old(hex_old.getHexList());    //新的更新数据列表
                try
                {                 
                    //（3.3.1）通过串口向MCU发送开始更新提示startUpdate,即11, update;
                    if (!(emuart.send(startUpdate, out bRet, 300, 2) && bRet != null
                        && Encoding.Default.GetString(bRet) == "Start Update")) goto btn_autoUpdate1_Click_EXIT8;
                    fmain.lbl_mainstatus.Text = "运行状态：收到UE握手信息，将开始程序更新操作...";      //底部提示
                    txtShow1("运行状态：收到UE握手信息，将开始程序更新操作...\r\n");                    //右侧更新提示区
                    emuart.RawDataReceivedEvent -= new EMUART.RawDataReceived(DataRecv);//解除事件绑定的方法，加快握手速度
                    infor_dispaly_button.Text = "暂停传输";
                    emuart.RawDataReceivedEvent += new EMUART.RawDataReceived(DataRecv);//解除事件绑定的方法，加快握手速度                   
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
                        if (bRet[0] == (byte)2 && bRet[1] != 0) goto   btn_autoUpdate1_Click_EXIT10;  //UE接收数据帧出错
                        if ((bRet[0] == 1) && ((int)bRet[2] * 256 + bRet[1] != i)) goto btn_autoUpdate1_Click_EXIT11;  //UE接收到的帧数有误
                        //（正确情况）进度条显示更新进度                      
                        this.prg_update1.Value = update_old.getSendCount() * 100 / sum;     //进度条显示       
                        this.prg_update1.Text = update_old.getSendCount() * 100 / sum + "%";//进度条百分比显示 
                        this.prg_update1.Refresh();
                        //成功提示
                        txtShow1("当前第" + (i + 1).ToString() + "/" + sum.ToString() + "帧  " + bRet[0].ToString() + "\r\n");     //右侧更新提示区  
                    }
                    //（3.4）程序更新成功
                    goto btn_autoupdate1_Click_EXIT4;
                }
                catch (Exception ex)
                {
                    fmain.lbl_mainstatus.Text = "运行状态：系统异常退出" + ex;   //底部提示
                    txtShow1("系统异常退出\r\n" + ex);     //右侧更新提示区  
                    goto btn_autoUpdate1_Click_EXIT7;
                }
            }

        //（4）退出区
        //（4.1）退出函数
        btn_autoupdate1_Click_EXIT:
            this.btn_autoupdate1.Enabled = true;
            return;

        //（4.2）Hex文件未选择
        btn_autoupdate1_Click_EXIT1:
            fmain.lbl_mainstatus.Text = "运行状态：Hex文件未选择";   //底部提示
            goto btn_autoupdate1_Click_EXIT;

            //（4.3）未连接设备
        btn_autoupdate1_Click_EXIT2:
            fmain.lbl_mainstatus.Text = "运行状态：未连接设备，请点击“重新连接”";   //底部提示
            goto btn_autoupdate1_Click_EXIT;

            //（4.4）程序整体更新成功
        btn_autoupdate1_Click_EXIT3:
            goto btn_autoupdate1_Click_EXIT;

            //（4.5）（VA.10之前）程序更新成功
        btn_autoupdate1_Click_EXIT4:
            fmain.lbl_mainstatus.Text = "运行状态：程序更新成功";   //底部提示
            txtShow1("程序更新成功！\r\n");     //右侧更新提示区  
            this.prg_update1.Value = 100;
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
            this.btn_autoupdate1.Enabled = true;
            this.btn_fileopen1.Enabled = true;
            if (flag != 1) this.btn_autoupdate1.Enabled = true;   
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
            if (infor_dispaly_button.Text == "暂停传输")
            {
                infor_dispaly_button.Text = "继续传输";
                infor_dispaly_button.Symbol = 61515;
                emuart.RawDataReceivedEvent -= new EMUART.RawDataReceived(DataRecv);//解除事件绑定的方法,暂停数据接收
            }

            else
            {
                infor_dispaly_button.Text = "暂停传输";
                infor_dispaly_button.Symbol = 61517;
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
                    infor_dispaly_button.Text = "继续传输";
                    emuart.RawDataReceivedEvent -= new EMUART.RawDataReceived(DataRecv);//解除事件绑定的方法,暂停数据接收
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
        private void frm_uartUpdate_FormClosed(object sender, FormClosedEventArgs e)
        {
            //【20201129】 修复串口更新烧录或连接后串口区卡死问题。 暂不清楚出现原因
            btnCloseUpdate_Click(sender,new EventArgs());
        }

        /// <summary>
        /// 退出串口更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCloseUpdate_Click(object sender, EventArgs e)
        {
            //【20201129】 修复串口更新烧录或连接后串口区卡死问题。 暂不清楚出现原因
            if (emuart._Uartport != null)
            {
                emuart.RawDataReceivedEvent -= new EMUART.RawDataReceived(DataRecv);
                closing = true;
                while (listening)
                {
                    Application.DoEvents();
                }
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
        private void GetAllFiles(string path,List<FileInfo> files)
        {
            if (path==null)
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
    }
}


