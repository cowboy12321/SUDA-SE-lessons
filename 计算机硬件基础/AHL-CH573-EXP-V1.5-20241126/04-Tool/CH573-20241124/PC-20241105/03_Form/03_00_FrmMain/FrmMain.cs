using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Pub;
using Newtonsoft.Json;
using System.Threading;
using Microsoft.Win32;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using System.Xml;
using System.Xml.Linq;
using cn.edu.suda.sumcu.iot;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using AHL_GEC._03_Form._03_02_FrmSCI;
using cn.edu.suda.sumcu.iot.data;
using cn.edu.suda.sumcu.iot.util;

public struct OutSoft
{
    public string name;
    public string path;
    public string display;
    public string arguments;
};

namespace AHL_GEC
{
    public partial class FrmMain : Form
    {
       




        //定义委托
        public delegate void addRootNode(TreeNode treeNode);       //创建文件树根节点
        public delegate TreeNode addChildNode(TreeNode rootNode,string childPath,string childName);           //创建文件树子节点
        public delegate void deleteChildNode(string childPath, string childName);       //删除文件树子节点
        public delegate void deleteRootNode(string folderName);    //删除文件树根节点
        public delegate void closeTabPage();                   //关闭所有标签页
        public delegate void DelReadStdOutput(string result);
        public delegate void DelReadErrOutput(string result);
        public event DelReadStdOutput ReadStdOutput;
        public event DelReadErrOutput ReadErrOutput;

        //定义使用的局部变量
        public static bool isrunning = false;//程序是否正在运行状态
        public static bool isdebugmode = false;//是否处于串口调试模式
        public static bool isC = true;//是否是c语言
        public static bool isASM = false;//是否是汇编


        public static string[] bpfilepath={"","","","","",""};//断点所在文件路径
        public static int[] bplinenum = {0,0,0,0,0,0 };//断点所在的行数
        public static uint[] bpaddr = { 0,0,0,0,0,0};
        public EMUART emuart;     //串口通信类对象


        //用于重新加载窗体
        public static int Target1_s = 0;
        public static int Target1_f = 0;
        public static int Target2_s = 0;
        public static int Target2_f = 0;
        public static int Target3_s = 0;
        public static int Target3_f = 0;

        public static bool SerachOnce = false;

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
        private string uecomType;   //通信模组类型
        private string mcuType;     //MCU类型
        private bool closing = false;      //标识是否正在关闭串口
        private bool listening = false;    //标识是否执行完invoke相关操作
        private Hex hex;  //Hex文件信息，整体更新用
        private uint uStartAds;     //User程序起始地址
        private Update update;  //更新类，保存更新所使用的帧结构体与方法


        public frm_uartDynamic mfrm_uartDynamic;    //动态装载窗体
        public frm_UserUpdate mfrm_UserUpdate;
        public frm_UserUpdate_1 mfrm_UserUpdate_1;
        public frm_UserInfoUpdate mfrm_UserInfoUpdate;
        public Frm_Monitor mfrm_Monitor;

        public FormReadVideo formReadVideo;     //读视频文件
        public FormSendAndReceive FormSendAndReceive;   //选择文件并发送

        //【4.07 1/4】创建进程对象
        public Process SerialPort = null;

        public frm_uartUpdate mfrm_uartUpdate;    //串口更新窗体


        public static string projectPath;           //记录工程路径
        public string projectName;    //记录打开的工程名
        public bool isOpenCCS = false;    //新添加工程修改
        public bool isOpenKDS = false;
        public bool isOpenSTM32 = false;
        public bool isOpenS32KDS = false;
        public bool isOpenKeil = false;
        public bool isOpenCCTOGNU = false;
        public bool isOpenAhlPy = false;
        public  bool isOpenProject = false;    //当前开发环境中是否有工程打开
        private bool hasHidden = false;
        private List<string> insertText = new List<string>();      //记录编译错误、警告信息
        private List<string> errorReason = new List<string>();     //记录错误、警告原因
        private List<FileInfo> fileList = new List<FileInfo>();    //保存工程下所有文件
        private TreeNode root;
        public bool openList = false;    //标识是否打开lst文件
        public bool openHex = false;     //标识是否打开hex文件
        public bool openMap = false;     //标识是否打开map文件 
        private string tmpProjectName;    //记录去掉特殊符号后的CCS工程名
        private bool isCCToCNU = false;    //是否为CC编译器下工程转为GNU编译器下工程
        //工程模板路径（新添加工程修改）
        public string KDSmodelPath = Environment.CurrentDirectory + @"\..\..\04_Resource\Project\KDS\KL36Z64xxx4\Source";
        public string CCSmodelPath = Environment.CurrentDirectory + @"\..\..\04_Resource\Project\CCS\MSP432\Source";
        public string STM32modelPath = Environment.CurrentDirectory + @"\..\..\04_Resource\Project\STM32Cube\STM32L4xx\Source";
        public string S32KDSmodelPath = Environment.CurrentDirectory + @"\..\..\04_Resource\Project\S32DS\S32K144_NOS\Source";
        public string KeilmodelPath = Environment.CurrentDirectory + @"\..\..\04_Resource\Project\Keil\STM32L431RCT\Source";
        public string AhlPymodelPath = Environment.CurrentDirectory + @"\..\..\04_Resource\project\AhlPy\Source";
        public string makePrePath = string.Empty;    //makefile的上级文件目录


        public bool Template_flag = true;

        //【20200103】Python工程用
        FileInfo PyFile = null;              //记录Python文件路径

        //委托实例
        private addRootNode createNode;
        private deleteRootNode removeRootNode;
        private closeTabPage removeTabPage;
        private addChildNode createChildNode;
        private deleteChildNode removeChildNode;
        private Frm_Monitor realFrm;
        private TreeNode objNode;
        private TreeNode srccNode;
        private TreeNode srcsNode;

        //public LoadConfig loadConfig = new LoadConfig();
        //public List<string> mcuName = new List<string>();
        private static FrmMain mInstance;    //主窗体单例模式
        public string backupFindText = string.Empty;    //记录查询文本【20190826 1/1】

        //public OptLevel optLevel = OptLevel.O1;         //默认O1优化
        private bool isMakeClean = false;               //标识本次编译是否需要先清理工程（目前仅用于修改优化等级后的重新编译）
        public static FrmMain getInstance()
        {
            if (mInstance == null || mInstance.IsDisposed)
            {
                mInstance = new FrmMain();
            }
            return mInstance;
        }


        //======================================================================
        //函数名称：FrmMain
        //函数返回：FrmMain窗体构造函数
        //参数说明：无
        //功能概要：完成FrmMain窗体的初始化工作
        //======================================================================
        public FrmMain()
        {
            InitializeComponent();
            mInstance = this;
            this.dockPanel1.Hide();
            emuart = EMUART.getInstance();
            hex = new Hex();
            realFrm = new Frm_Monitor();
        }


        ///-----------------------------------------------------------------
        /// <summary>  
        /// 对象：窗体FrmMain（主窗体）
        /// 事件：Load事件（窗体加载事件，显示窗体是触发）                              
        /// 功能：主窗体加载时，显示该窗体，进行一系列初始化工作，见注释
        /// </summary>                                                      
        ///-----------------------------------------------------------------
        private void FrmMain_Load(object sender, EventArgs e)
        {
        }



        //======================================================================
        //函数名称：Add_Internal_Software
        //函数返回：无
        //参数说明：无
        //功能概要：添加内部外接软件
        //======================================================================
        public void Add_Internal_Software(ref Process SerialPort, string FileName)
        {
            //【20200529 1/5】 新增内部外接软件功能，方法见【4.07】。
            SerialPort = new Process();
            SerialPort.StartInfo.FileName = FileName;
            try
            {
                SerialPort.Start();
            }
            catch
            {
                MessageBox.Show(null, "未有该功能的独立功能软件，请按照readme中步骤操作", "内部外接软件警告");
                return;
            }
            finally
            {
                
            }
            
        }



        //======================================================================
        //函数名称：FrmMain_search_new   【20190905 1/1】 通过读取xml文件添加外界软件
        //函数返回：无
        //参数说明：无
        //功能概要：读取xml文件添加外接软件菜单
        //======================================================================
        //【20191120 1/6】由于修改了显示数据的数据结构，重新编写了读取xml文件添加外接软件菜单
        public List<OutSoft> FrmMain_search_new(List<OutSoft> outsoft)
        {

            List<string> name = new List<string> { };
            List<string> path = new List<string> { };
            List<string> display = new List<string> { };
            //【20200318 4/6】 姜家乐 新增参数结点，修改显示数据的数据结构
            List<string> arguments = new List<string> { };

            outsoft.Clear();

            OutSoft tempsoft;

            XmlDocument doc = new XmlDocument();
            //doc.Load("..//..//04_Resource//XMLFile1.xml");
            doc.Load("..//..//04_Resource//XMLFile1.xml");
            XmlElement rootElem = doc.DocumentElement;//获取根节点
            XmlNodeList fileNodes = rootElem.GetElementsByTagName("file");//获取file子节点集合
            foreach (XmlNode node in fileNodes)
            {
                string strName = ((XmlElement)node).GetAttribute("name");//获取name属性值
                name.Add(strName);

                XmlNodeList subpathNodes = ((XmlElement)node).GetElementsByTagName("path");  //获取path子XmlElement集合  
                if (subpathNodes.Count == 1)
                {
                    string strpath = subpathNodes[0].InnerText;
                    path.Add(strpath);//将path加入list
                }

                XmlNodeList subdisplayNodes = ((XmlElement)node).GetElementsByTagName("display");
                string strdisplay = subdisplayNodes[0].InnerText;
                display.Add(strdisplay);

                XmlNodeList subargumentsNodes = ((XmlElement)node).GetElementsByTagName("arguments");
                string strarguments;
                if(subargumentsNodes[0] == null)
                {
                    strarguments = "no args";
                }
                else
                {
                    strarguments = subargumentsNodes[0].InnerText;
                }
                arguments.Add(strarguments);
            }
            //【20191120 2/6】 更改了数据结点的数据结构，重新编写了提取xml结点的获取方式
            for (int i = 0; i < name.Count(); i++)
            {
                tempsoft.name = name[i];
                tempsoft.path = path[i];
                tempsoft.display = display[i];
                tempsoft.arguments = arguments[i];

                outsoft.Add(tempsoft);
            }
            return outsoft;
        }

        public void Re_load_Item(object sender, EventArgs e)
        {
            FrmMain_Load(sender, e);
        }

        #region tmr_tick_Tick
        //======================================================================
        //函数名称：tmr_tick_Tick
        //函数返回：无
        //参数说明：无
        //功能概要：每1秒执行一次，更新显示当前系统时间
        //======================================================================
        private void tmr_tick_Tick(object sender, EventArgs e)
        {
            lbl_curtime.Text = "当前时间：" + System.DateTime.Now.ToString();
            //将隐藏的dockpanel及停靠在上窗体自动显示（新添加工程修改）
            if ((isOpenCCS || isOpenKDS || isOpenSTM32 || isOpenS32KDS || isOpenKeil || isOpenCCTOGNU || isOpenAhlPy) && this.dockPanel1.Visible == false
                && (mfrm_UserUpdate == null || mfrm_UserUpdate.IsDisposed)
                && (mfrm_uartDynamic == null || mfrm_uartDynamic.IsDisposed))
            {
                //显示窗体
                this.dockPanel1.Visible = true;
                
                this.hasHidden = false;
                
            }
        } 
        #endregion





        public string all;

        #region 关闭窗体时，释放所有资源
        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                this.Dispose();
                this.Close();
                Application.Exit();
                System.Environment.Exit(System.Environment.ExitCode);
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        #endregion

        

        private void 程序更新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (mfrm_CreateDChexManual != null)
            //{
            //    if (mfrm_CreateDChexManual.emuart._Uartport != null)
            //        mfrm_CreateDChexManual.emuart._Uartport.Dispose();
            //    mfrm_CreateDChexManual.Close();
            //}
            //（1）判断串口界面是否已经打开
            //【4.07 2/4】关闭其他进程，防止资源浪费
            if (SerialPort != null)
            {
                SerialPort = null;
            }
           

            //（4）串口更新页面是否已打开
            if (mfrm_UserUpdate == null || mfrm_UserUpdate.IsDisposed)
            {
                //新建frm_update窗体
                mfrm_UserUpdate = new frm_UserUpdate();
                //设置mfrm_update的父窗体为本窗体
                mfrm_UserUpdate.MdiParent = this;
            }

            if (mfrm_UserUpdate_1 != null)
            {
                if (mfrm_UserUpdate_1.emuart._Uartport != null)
                    mfrm_UserUpdate_1.emuart._Uartport.Dispose();
                mfrm_UserUpdate_1.Close();
            }
            //（5）动态装载页面是否已打开
            if (mfrm_uartDynamic != null)
            {
                if (mfrm_uartDynamic.emuart._Uartport != null)
                    mfrm_uartDynamic.emuart._Uartport.Dispose();
                mfrm_uartDynamic.Close();
            }

            if (mfrm_UserInfoUpdate != null)
            {
                if (mfrm_UserInfoUpdate.emuart._Uartport != null)
                    mfrm_UserInfoUpdate.emuart._Uartport.Dispose();
                mfrm_UserInfoUpdate.Close();
            }

            //（5）判断与KDS工程相关窗体是否已经打开
            this.dockPanel1.Hide();
            

            //（6）显示串口更新页面
            mfrm_UserUpdate.Hide();  //置顶显示
            mfrm_UserUpdate.Show();
            //（7）提示
            lbl_mainstatus.Text = "运行状态：打开串口更新页面";
        }

        private void 设备信息更新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SerialPort != null)
            {
                SerialPort = null;
            }


            //（4）串口更新页面是否已打开
            if (mfrm_UserInfoUpdate == null || mfrm_UserInfoUpdate.IsDisposed)
            {
                //新建frm_update窗体
                mfrm_UserInfoUpdate = new frm_UserInfoUpdate();
                //设置mfrm_update的父窗体为本窗体
                mfrm_UserInfoUpdate.MdiParent = this;
            }
            //（5）动态装载页面是否已打开
            if (mfrm_UserUpdate != null)
            {
                if (frm_UserUpdate.emuart._Uartport != null)
                    frm_UserUpdate.emuart._Uartport.Dispose();
                mfrm_UserUpdate.Close();
            }
            if (mfrm_uartDynamic != null)
            {
                if (mfrm_uartDynamic.emuart._Uartport != null)
                    mfrm_uartDynamic.emuart._Uartport.Dispose();
                mfrm_uartDynamic.Close();
            }

            if (mfrm_UserUpdate_1 != null)
            {
                if (mfrm_UserUpdate_1.emuart._Uartport != null)
                    mfrm_UserUpdate_1.emuart._Uartport.Dispose();
                mfrm_UserUpdate_1.Close();
            }

            //（5）判断与KDS工程相关窗体是否已经打开
            this.dockPanel1.Hide();


            //（6）显示串口更新页面
            mfrm_UserInfoUpdate.Hide();  //置顶显示
            mfrm_UserInfoUpdate.Show();
            //（7）提示
            lbl_mainstatus.Text = "运行状态：打开串口更新页面";
        }

        private void 串口数据接收ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SerialPort != null)
            {
                SerialPort = null;
            }


            //（4）串口更新页面是否已打开
            if (mfrm_Monitor == null || mfrm_Monitor.IsDisposed)
            {
                //新建frm_Monitor窗体
                mfrm_Monitor = new Frm_Monitor();
                //设置frm_Monitor的父窗体为本窗体
                mfrm_Monitor.MdiParent = this;
            }
            //（5）动态装载页面是否已打开
            if (mfrm_UserUpdate != null)
            {
                if (frm_UserUpdate.emuart._Uartport != null)
                    frm_UserUpdate.emuart._Uartport.Dispose();
                mfrm_UserUpdate.Close();
            }

            if (mfrm_UserUpdate_1 != null)
            {
                if (mfrm_UserUpdate_1.emuart._Uartport != null)
                    mfrm_UserUpdate_1.emuart._Uartport.Dispose();
                mfrm_UserUpdate_1.Close();
            }

            if (mfrm_uartDynamic != null)
            {
                if (mfrm_uartDynamic.emuart._Uartport != null)
                    mfrm_uartDynamic.emuart._Uartport.Dispose();
                mfrm_uartDynamic.Close();
            }

            if (mfrm_UserInfoUpdate != null)
            {
                if (mfrm_UserInfoUpdate.emuart._Uartport != null)
                    mfrm_UserInfoUpdate.emuart._Uartport.Dispose();
                mfrm_UserInfoUpdate.Close();
            }

            //（5）判断与KDS工程相关窗体是否已经打开
            this.dockPanel1.Hide();


            //（6）显示串口更新页面
            mfrm_Monitor.Hide();  //置顶显示
            mfrm_Monitor.Show();
            //（7）提示
            lbl_mainstatus.Text = "运行状态：打开串口更新页面";
        }

        private void 单个设备程序更新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SerialPort != null)
            {
                SerialPort = null;
            }


            //（4）串口更新页面是否已打开
            if (mfrm_UserUpdate_1 == null || mfrm_UserUpdate_1.IsDisposed)
            {
                //新建frm_update窗体
                mfrm_UserUpdate_1 = new frm_UserUpdate_1();
                //设置mfrm_update的父窗体为本窗体
                mfrm_UserUpdate_1.MdiParent = this;
            }
            //（5）动态装载页面是否已打开
            if (mfrm_uartDynamic != null)
            {
                if (mfrm_uartDynamic.emuart._Uartport != null)
                    mfrm_uartDynamic.emuart._Uartport.Dispose();
                mfrm_uartDynamic.Close();
            }
            if (mfrm_UserUpdate != null)
            {
                if (frm_UserUpdate.emuart._Uartport != null)
                    frm_UserUpdate.emuart._Uartport.Dispose();
                mfrm_UserUpdate.Close();
            }
            if (mfrm_UserInfoUpdate != null)
            {
                if (mfrm_UserInfoUpdate.emuart._Uartport != null)
                    mfrm_UserInfoUpdate.emuart._Uartport.Dispose();
                mfrm_UserInfoUpdate.Close();
            }
            


            //（5）判断与KDS工程相关窗体是否已经打开
            this.dockPanel1.Hide();


            //（6）显示串口更新页面
            mfrm_UserUpdate_1.Hide();  //置顶显示
            mfrm_UserUpdate_1.Show();
            //（7）提示
            lbl_mainstatus.Text = "运行状态：打开串口更新页面";
        }

        private void 构件固化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SerialPort != null)
            {
                SerialPort = null;
            }

            //（1）动态构件固化页面是否已打开
            if (mfrm_uartDynamic == null || mfrm_uartDynamic.IsDisposed)
            {
                //新建frm_update窗体
                mfrm_uartDynamic = new frm_uartDynamic();
                //设置mfrm_update的父窗体为本窗体
                mfrm_uartDynamic.MdiParent = this;
            }

            //（2）程序更新页面是否已打开
            if (mfrm_UserUpdate != null)
            {
                if (frm_UserUpdate.emuart._Uartport != null)
                    frm_UserUpdate.emuart._Uartport.Dispose();
                mfrm_UserUpdate.Close();
            }

            if (mfrm_UserUpdate_1 != null)
            {
                if (mfrm_UserUpdate_1.emuart._Uartport != null)
                    mfrm_UserUpdate_1.emuart._Uartport.Dispose();
                mfrm_UserUpdate_1.Close();
            }

            //（3）串口工具页面是否打开
            if (mfrm_UserInfoUpdate != null)
            {
                if (mfrm_UserInfoUpdate.emuart._Uartport != null)
                    mfrm_UserInfoUpdate.emuart._Uartport.Dispose();
                mfrm_UserInfoUpdate.Close();
            }

            //（4）判断与KDS工程相关窗体是否已经打开
            this.dockPanel1.Hide();

            //（5）显示串口更新页面
            mfrm_uartDynamic.Hide();  //置顶显示
            mfrm_uartDynamic.Show();

            //（6）提示
            lbl_mainstatus.Text = "运行状态：打开动态构件固化页面";

        }

        private void ReadVideoFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (formReadVideo == null || formReadVideo.IsDisposed)
            {
                formReadVideo = new FormReadVideo();
            }
            formReadVideo.Show();

        }

        private void selectFileSendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormSendAndReceive == null || FormSendAndReceive.IsDisposed)
            {
                FormSendAndReceive = new FormSendAndReceive();
            }
            FormSendAndReceive.Show();
        }

        private void tsmi_uartupdate_Click(object sender, EventArgs e)
        {
            //（1）判断串口界面是否已经打开
            //【4.07 2/4】关闭其他进程，防止资源浪费
            if (SerialPort != null)
            {
                SerialPort = null;
            }
            if (mfrm_uartUpdate == null || mfrm_uartUpdate.IsDisposed)
            {
                //新建frm_update窗体
                mfrm_uartUpdate = new frm_uartUpdate();
                //设置mfrm_update的父窗体为本窗体
                mfrm_uartUpdate.MdiParent = this;
            }
            mfrm_uartUpdate.Hide();  //置顶显示
            mfrm_uartUpdate.Show();
            //（7）提示
            lbl_mainstatus.Text = "运行状态：打开串口更新页面";
        }
    }
}