using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SerialPort;
using cn.edu.suda.sumcu.iot.data;
using System.Xml;
using AHL_GEC;
using System.Threading;
using cn.edu.suda.sumcu.iot.util;
using System.Text.RegularExpressions;

namespace AHL_GEC._03_Form._03_02_FrmSCI
{
    public partial class Frm_Monitor : Form
    {
        //（1）定义使用的全局变量
        public string[] g_IMSI = null;    //侦听的imsi号数组
        public string g_target;           //连接的目标地址（如：123.456.789:12345）
        public string g_wsTarget;         //WebSocket服务器地址（如：ws://0.0.0.0:38867）
        public string g_wsDirection;      //WebSocket二级目录（如：/wsServices/）
        public ulong g_TimeSec;           //程序已经运行的时间（秒）
        public static string myhardaddr = "";
        //（2）FrameData为存储帧结构中的变量名、变量类型、别名等信息的结构体
        public FrameData g_frmStruct1 = new FrameData();//用户信息帧（包含用户信息的所有字段）
        public FrameData g_frmStruct0 = new FrameData();//配置信息帧（存入终端flash中的数据）
        private List<string> g_ListCommandsField = new List<string>();   //存储命令对应的帧格式（如A0命令对应的内容）
        private List<string> g_ListCommands = new List<string>();        //存储命令（A0,A1,A2……）
        public Dictionary<string, FrameData> g_commandsFrame =
                                  new Dictionary<string, FrameData>();  //存储命令对应的帧（命令和FrameData对应）

        //（3）定义数据表的操作对象
        public SQLCommand sQLDevice;      //记录帧结构的变量名、类型和别名
        public SQLCommand sQLDown;        //下行帧记录表数据库操作对象
        public SQLCommand sQLUp;          //上行帧记录表数据库操作对象

        SynchronizationContext m_SyncContext = null;    //定义一个同步上下文变量
        private FrmMain frmMain;
        private static Frm_Monitor mInstance;
        public SCI sci = new SCI();    //用于发送数据的通讯对象
        //委托,将从串口接收到的数据显示到接收框里面
        delegate void handleinterfaceupdatedelegate(Object textbox,
                                                    string text);
        public Frm_Monitor()
        {
            InitializeComponent();
        }

        public Frm_Monitor(FrmMain frmMain) : this()
        {
            this.frmMain = frmMain;
            m_SyncContext = SynchronizationContext.Current;    //用于跨线程创建标签
        }


        public static Frm_Monitor getInstance()
        {
            if (mInstance == null || mInstance.IsDisposed)
            {
                mInstance = new Frm_Monitor(new FrmMain());  //创建一个实时数据窗体实例
            }
            return mInstance;
        }
        ///-----------------------------------------------------------------
        /// <summary>                                                       
        /// 函 数 名：Frm_Monitor_Load:页面加载函数                              
        /// 功    能：完成页面的初始化工作  
        /// 修    改：【20201022】ZRQ
        /// </summary>      
        ///-----------------------------------------------------------------
        private void Frm_Monitor_Load(object sender, EventArgs e)
        {

            //自动搜索串口,并将其加入到[串口选择框]中
            //（1）定义并初始化变量
            int i, j, k;   //临时变量
            string[] SCIPorts;
            try
            {
                
                XmlNode xNode;               //xml文件的根节点
                string vName = "";
                string vType = "";
                string votherName = "";
                string vWR = "";
                g_TimeSec = 0;              //程序已经运行的时间（秒）
                //（2）设置允许跨线程文本框赋值
                Control.CheckForIllegalCrossThreadCalls = false;
                //（3）加载“AHL.xml”文档并读取出IMSI号、目标地址等信息
                #region
                try
                {
                    //加载“AHL.xml”文件，进行解析
                    XmlDocument xmlDoc = new XmlDocument();
                    string configPath = Application.StartupPath + "/../../04_Resource/AHL.xml";  //【20200811】
                    xmlDoc.Load(configPath);    //读取AHL.xml文件到内存
                    //解析“AHL.xml”文件
                    xNode = xmlDoc.DocumentElement;    //获取根节点
                    xmlDoc.RemoveAll();                //释放“AHL.xml”文档
                    //遍历一级节点
                    foreach (XmlNode node in xNode.ChildNodes)
                    {
                        //找到appSetting节点
                        if (node.Name == "appSettings")
                        {
                            //遍历二级节点，自上而下开始读取
                            foreach (XmlNode node2 in node.ChildNodes)
                            {
                                //读取本窗体名并显示
                                if (node2.Name == "formName")
                                    this.Text = node2.InnerText;
                                //读取要连接的目标地址HCIComTarget→target
                                else if (node2.Name == "HCIComTarget")
                                    g_target = node2.InnerText;
                                else if (node2.Name == "WebSocketTarget")
                                    g_wsTarget = node2.InnerText;
                                else if (node2.Name == "WebSocketDirection")
                                    g_wsDirection = node2.InnerText;
                                //读取要侦听的终端UE的IMSI号→g_IMSI数组
                                else if (node2.Name == "IMSI")
                                {
                                    //以“;”为间隔符将imsiList中的数据解析到g_IMSI数组中
                                    g_IMSI = node2.InnerText.ToString().Trim().Split(';');
                                    //去除长度不等于15的号码（IMSI号的长度确定为15）
                                    List<string> list = g_IMSI.ToList();
                                    for (i = 0; i < list.Count; i++)
                                    {
                                        //【20181004】去除IMSI号中的空格、换行、回车和制表符
                                        list[i] = list[i].Replace(" ", "").Replace("\r", "").Replace("\n", "").Replace("\t", "");
                                        if (list[i].Length != 15)
                                        {
                                            list.RemoveAt(i);
                                            i--;
                                        }
                                    }
                                    g_IMSI = list.ToArray();  //正确的IMSI号→g_IMSI数组
                                }
                                else if (node2.Name == "commands")
                                {
                                    //遍历三级节点
                                    foreach (XmlNode node3 in node2.ChildNodes)
                                    {
                                        g_ListCommands.Add(node3.Name);
                                        g_ListCommandsField.Add("command," + node3.InnerText);
                                    }
                                }
                                else if (node2.Name == "frames")
                                {
                                    //遍历三级节点
                                    foreach (XmlNode node3 in node2.ChildNodes)
                                    {
                                        if (node3.Name == "frame0")//若为MCU配置相关变量
                                        {
                                            //遍历四级节点
                                            foreach (XmlNode node4 in node3.ChildNodes)
                                            {
                                                //遍历五级节点
                                                foreach (XmlNode node5 in node4.ChildNodes)
                                                {
                                                    if (node5.Name == "name")    //数据类型type
                                                        vName = node5.InnerText;
                                                    else if (node5.Name == "type")    //数据类型type
                                                        vType = node5.InnerText;
                                                    else if (node5.Name == "otherName") //中文名称
                                                        votherName = node5.InnerText;
                                                    else if (node5.Name == "wr")   //读写属性
                                                    {
                                                        vWR = node5.InnerText;
                                                        //存入g_frmStruct中
                                                        g_frmStruct0.addParameter(vType, vName, votherName, vWR);
                                                    }
                                                }//遍历五级节点
                                            }//遍历四级节点
                                        }
                                        else if (node3.Name == "frame1")//若为MCU通信相关变量
                                        {
                                            //遍历四级节点
                                            foreach (XmlNode node4 in node3.ChildNodes)
                                            {
                                                //遍历五级节点
                                                foreach (XmlNode node5 in node4.ChildNodes)
                                                {
                                                    if (node5.Name == "name")    //数据类型type
                                                        vName = node5.InnerText;
                                                    else if (node5.Name == "type")    //数据类型type
                                                        vType = node5.InnerText;
                                                    else if (node5.Name == "otherName") //中文名称
                                                        votherName = node5.InnerText;
                                                    else if (node5.Name == "wr")   //读写属性
                                                    {
                                                        vWR = node5.InnerText;
                                                        //存入g_frmStruct中
                                                        g_frmStruct1.addParameter(vType, vName, votherName, vWR);
                                                    }
                                                }//遍历五级节点
                                            }//遍历四级节点
                                        }
                                    }
                                }
                            }  //遍历二级节点
                        }  //if (node.Name == "appSettings")
                    }   //遍历一级节点
                }   //try
                catch
                {
                    //显示错误信息
                    MessageBox.Show("【主窗体加载时,读取配置文件AHL.xml错误】"
                        , "金葫芦IoT友情提示：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //退出程序
                    Application.Exit();
                }
                #endregion

                this.Show();       //把主窗体显示出来                

                //（5）数据库操作
                #region
                //（5.1）获取App.config文件中AppSettings域的数据库连接字符串
                string connectionString = System.Configuration.
                    ConfigurationManager.AppSettings["connectionString"];
                string myconnectionString = System.Configuration.
                    ConfigurationManager.AppSettings["myconnectionString"];
                //this.toolStripUserOper.Text = connectionString;  //状态栏显示
                this.Refresh();                                  //显示刷新
                //（5.2）初始化数据库中的表（3张表）
                sQLDevice = new SQLCommand(connectionString, "Device");       //设备信息记录表
                sQLDown = new SQLCommand(connectionString, "Down");           //下行帧记录表
                sQLUp = new SQLCommand(connectionString, "Up");               //上行帧记录表
                //（5.3）判断数据库是否能够正确连接，并删除多余数据
                int counts = sQLUp.count();
                if (counts < 0)
                {
                    MessageBox.Show("未能成功连接数据库，请检查：（1）DataBase文件夹位置是否正确；" +
                        "（2）DataBase文件夹中的文件是否对user用户有完全控制权限；（3）VS中是否有SQL Server Data Tools",
                        "金葫芦友情提示（加载主窗体时）：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }

                //（5.4）根据g_frmStruct1更新Up表、Down表和Command表的字段结构
                UpdateDbColumns(sQLUp, g_frmStruct1);
                UpdateDbColumns(sQLDown, g_frmStruct1);
                UpdateDbColumns(sQLDevice, g_frmStruct0);
                //（5.5）根据数据结构g_ListCommandsField、g_ListCommands和g_frmStruct1更新字典g_commandsFrame
                FrameData frame = null;
                for (i = 0; i < g_ListCommands.Count(); i++)
                {
                    frame = new FrameData();
                    string[] attr = Regex.Split(g_ListCommandsField[i].ToString(), ",", RegexOptions.IgnoreCase);
                    for (j = 0; j < attr.Count(); j++)
                    {
                        for (k = 0; k < g_frmStruct1.Parameter.Count(); k++)
                        {
                            if (attr[j] == g_frmStruct1.Parameter[k].name)
                            {
                                string variable = g_frmStruct1.Parameter[k].name;
                                string type = g_frmStruct1.Parameter[k].type;
                                string otherName = g_frmStruct1.Parameter[k].otherName;
                                string wr = g_frmStruct1.Parameter[k].wr;
                                frame.addParameter(type, variable, otherName, wr);
                                break;
                            }
                        }
                    }
                    g_commandsFrame.Add(g_ListCommands[i].ToString(), frame);
                }
                #endregion
                //（6）初始化全局变量
                //g_FieldCnt = g_frmStruct1.Parameter.Count; //初始化参数的个数 
                //（7）加载实时数据窗体运行
                //mnuRealTimeData_Click(sender, e);
                //mnuProBasePara_Click(sender, e);
                //frmDeviceConfig.Hide();
            }
            catch (Exception ee)  //主窗体加载系统错误
            {
                //显示错误信息
                MessageBox.Show("主窗体加载系统错误\r\n" + ee.Message,
                    "金葫芦友情提示：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //退出程序
                System.Environment.Exit(0);
            }






            //初始化时,按钮显示"打开串口"

            this.BtnSCISwitch.Text = "打开串口";
            this.CbSCIBaud.Enabled = true;    //[波特率选择框]处于可用状态
            this.CbSCIComNum.Enabled = true; //[串口选择框]处于可用状态


            SCIPorts = System.IO.Ports.SerialPort.GetPortNames();
            this.CbSCIComNum.Items.Clear();//首先将现有的项清除掉
            for (i = 0; i < SCIPorts.Length; i++)
                //向[串口选择框]中添加搜索到的串口号
                this.CbSCIComNum.Items.Add(SCIPorts[i]);

            //设置各组合框的初始显示值
            if (SCIPorts.Length != 0)
            {
                this.BtnSCISwitch.Enabled = true;
                this.CbSCIBaud.SelectedIndex = 0;
                this.CbSCIComNum.SelectedIndex = 0;

                //设置初始的串口号与波特率
                PublicVar.g_SCIComNum = CbSCIComNum.Text;
                PublicVar.g_SCIBaudRate = int.Parse(this.CbSCIBaud.Text);
                //显示当前串口信与状态信息
                this.TSSLState.Text = "无操作,请先选择波特率与串口号,打开串口," +
                                 "然后发送数据";
            }
            else
            {
                this.TSSLState.Text = "没有可用的串口,请检查!";
                this.BtnSCISwitch.Enabled = false;
            }

        }

        ///-----------------------------------------------------------------
        /// <summary>                                                       
        /// 对    象:SCIPort                                                
        /// 事    件:DataReceived                                           
        /// 功    能:串口接收数据                                           
        /// 函数调用:(1)SCIReceiveData,串口接收函数                         
        ///          (2)SCIUpdateRevtxtbox,更新文本框中的内容               
        /// </summary>                                                      
        /// <param name="sender"></param>                                   
        /// <param name="e"></param>                                        
        ///-----------------------------------------------------------------
        private void SCIPort_DataReceived(object sender,
            System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string log = string.Empty;
            int rowNum;
            FrameData tmpFrmStruct = null;
            String str = String.Empty;
            bool Flag;//标记串口接收数据是否成功
            int len;//标记接收的数据的长度
            byte[] ch2 = new byte[2];
            try
            {
                this.SCIPort.DataReceived -= new System.IO.Ports.SerialDataReceivedEventHandler(this.SCIPort_DataReceived);
                //调用串口接收函数,并返回结果
                Flag = sci.SCIReceiveData(SCIPort, ref PublicVar.g_ReceiveByteArray);
                if (Flag == true)//串口接收数据成功
                {
                    len = PublicVar.g_ReceiveByteArray.Length;
                    //对于字符串形式,考虑到可能有汉字,
                    //直接调用系统定义的函数,处理整个字符串
                    str = Encoding.Default.GetString(PublicVar.g_ReceiveByteArray);



                    if (PublicVar.g_ReceiveByteArray[0] == 'A' || PublicVar.g_ReceiveByteArray[0] == 'B') goto IoT_recv_exit;  //若收到的为配置命令
                    string command = Encoding.Default.GetString(PublicVar.g_ReceiveByteArray, 0, 2);
                    if (g_commandsFrame.ContainsKey(command))
                    {
                        tmpFrmStruct = g_commandsFrame[command];
                    }

                    //把接收到的字节数组类型的数据转为结构体的成员变量
                    if (tmpFrmStruct == null) goto IoT_recv_exit;
                    if (tmpFrmStruct.byteToStruct(PublicVar.g_ReceiveByteArray) == false) goto IoT_recv_exit;
                    ////【20200608】文本框显示
                    //Text_update(ref tmpFrmStruct, i, ref isDataOkFlag, ref inTheCyc);
                    //（2.4) 解析接收到的数据并显示在文本框中
                    tmpFrmStruct.byteToStruct(PublicVar.g_ReceiveByteArray);//把接收到的字节数组类型的数据转为结构体的成员变量

                    for (int i = 0; i < tmpFrmStruct.Parameter.Count; i++)
                    {
                        if (tmpFrmStruct.Parameter[i].name == "currentTime")
                        {
                            tmpFrmStruct.Parameter[i].value = System.DateTime.Now.ToString("yyyyMMddHHmm");
                            txttime.Text = tmpFrmStruct.Parameter[i].value;
                            log = log + tmpFrmStruct.Parameter[i].value.Replace("\0", "") + ":";
                        }
                        else if (tmpFrmStruct.Parameter[i].name == "name")
                        {
                            txtname.Text = tmpFrmStruct.Parameter[i].value;
                            log = log + tmpFrmStruct.Parameter[i].value.Replace("\0", "") + ",";
                        }
                        else if (tmpFrmStruct.Parameter[i].name == "company")
                        {
                            txtcompany.Text = tmpFrmStruct.Parameter[i].value;
                            log = log + tmpFrmStruct.Parameter[i].value.Replace("\0", "") + ",";
                        }
                        else if (tmpFrmStruct.Parameter[i].name == "lock_State")
                        {
                            if(tmpFrmStruct.Parameter[i].value == "189")
                            {
                                goto IoT_recv_exit;
                            }
                            txtstate.Text = tmpFrmStruct.Parameter[i].value;
                            log = log + tmpFrmStruct.Parameter[i].value.Replace("\0", "") + ";";

                        }
                        else if (tmpFrmStruct.Parameter[i].name == "devicename")
                        {
                            txtdevicename.Text = tmpFrmStruct.Parameter[i].value;
                        }
                        else if (tmpFrmStruct.Parameter[i].name == "softversion")
                        {
                            txtversion.Text = tmpFrmStruct.Parameter[i].value;
                        }

                    }

                    //显示字符串接收内容                                    
                    //SCIUpdateRevtxtbox(txt_log, str.Replace("\0", ""));
                    this.txt_log.Text += log + "\r\n";
                    this.txt_log.Focus();//获取焦点
                    this.txt_log.Select(this.txt_log.TextLength, 0);//光标定位到文本最后
                    this.txt_log.ScrollToCaret();//滚动到光标处
                    //数据入库
                    rowNum = this.sQLUp.insertFrame(tmpFrmStruct);
                    this.TSSLState.Text = "过程提示:数据接收成功!";
                }
                //接收数据失败
                else
                {
                    goto IoT_recv_exit;

                }


            IoT_recv_exit:
                tmpFrmStruct = null;
                this.SCIPort.DataReceived -= new System.IO.Ports.SerialDataReceivedEventHandler(this.SCIPort_DataReceived);
                this.SCIPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SCIPort_DataReceived);
                return;
            }
            catch(Exception ex)
            {
                tmpFrmStruct = null;
                this.SCIPort.DataReceived -= new System.IO.Ports.SerialDataReceivedEventHandler(this.SCIPort_DataReceived);
                this.SCIPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SCIPort_DataReceived);
                return;
            }
            
        }




        ///-----------------------------------------------------------------
        /// <summary>                                                       
        /// 函数名:SCIUpdateRevtxtbox                                       
        /// 参  数:(1)textbox,Object类型,接收数据要放入的文本框    (文本框）             
        ///        (2)text,string类型,要放入文本框的数据    （数据）                   
        /// 功  能:若串行接收与Object不在同一线程中运行，那么通过invoke     
        ///        跨线程用串口接收到的数据来更新接收文本框中的数据         
        /// 返  回:无                                                       
        /// </summary>                                                      
        /// <param name="textbox"></param>                                  
        /// <param name="str"></param>                                      
        ///-----------------------------------------------------------------
        private void SCIUpdateRevtxtbox(Object textbox, string text)
        {
            //textbox显示文本与串口执行不在同一个线程中
            if (((TextBox)textbox).InvokeRequired)
            {
                handleinterfaceupdatedelegate InterFaceUpdate = new
                    handleinterfaceupdatedelegate(SCIUpdateRevtxtbox);
                this.Invoke(InterFaceUpdate, new object[] { textbox, text });
            }
            else
            {
                ((TextBox)textbox).Text += text;
                //把光标放在最后一行
                ((TextBox)textbox).SelectionStart =
                                           ((TextBox)textbox).Text.Length;
                //将文本框中的内容调整到当前插入符号位置
                ((TextBox)textbox).ScrollToCaret();
            }
        }

        ///-----------------------------------------------------------------
        /// <summary>                                                       
        /// 对    象:BtnSCISwitch                                           
        /// 事    件:Click                                                  
        /// 功    能:(1)当开关显示为打开串口,则单击时执行打开串口 
        ///          操作,并在标签LblSCI中显示选择的串口号与波特率,并在     
        ///          状态条文本TSSLState中显示当前操作                      
        ///          (2)当开关显示为关闭串口,则单击时执行关闭串口
        ///          操作,并在标签LblSCI中显示关闭的串口号与波特率,并在     
        ///          状态条文本TSSLState中显示当前操作                      
        /// 函数调用:(1)SCIInit:串口初始化                                  
        ///          (2)SCIClose:关闭串口                                   
        /// </summary>                                                      
        /// <param name="sender"></param>                                   
        /// <param name="e"></param>                                        
        ///-----------------------------------------------------------------
        private void BtnSCISwitch_Click(object sender, EventArgs e)
        {
            bool Flag;//标记打开是否成功
            //根据按钮BtnSCISwitch显示内容执行打开或关闭串口操作
            if (this.BtnSCISwitch.Text.CompareTo("打开串口") == 0)
            {
                //提示当前正在执行打开串口操作
                this.TSSLState.Text = "过程提示:正在打开串口...";
                //进行串口的初始化,并用Flag返回结果
                Flag = sci.SCIInit(SCIPort, PublicVar.g_SCIComNum,
                            PublicVar.g_SCIBaudRate);

                if (Flag == true)//串口打开成功
                {

                    this.BtnSCISwitch.Text = "关闭串口";
                    //[串口选择框]处于禁用状态
                    this.CbSCIComNum.Enabled = false;
                    //[波特率选择框]处于禁用状态
                    this.CbSCIBaud.Enabled = false;
                    //状态上显示结果信息
                    this.TSSLState.Text = this.TSSLState.Text +
                                          "打开" + PublicVar.g_SCIComNum + "成功!" + "波特率选择：" + PublicVar.g_SCIBaudRate;
                }
                else//串口打开失败
                {
                    this.TSSLState.Text = this.TSSLState.Text +
                                          "打开" + PublicVar.g_SCIComNum + "失败!";
                }
            }
            else if (this.BtnSCISwitch.Text == "关闭串口")
            {
                //提示当前操作
                this.TSSLState.Text = "过程提示:正在关闭串口...";
                //执行关闭串口操作,并用Flag返回结果
                Flag = sci.SCIClose(this.SCIPort);
                if (Flag == true)
                {
                    this.BtnSCISwitch.Text = "打开串口";
                    //[串口选择框]处于可用状态
                    this.CbSCIComNum.Enabled = true;
                    //[波特率选择框]处于可用状态
                    this.CbSCIBaud.Enabled = true;
                    this.TSSLState.Text += "关闭" + PublicVar.g_SCIComNum + "成功!";
                }
                else//串口关闭失败
                {
                    this.TSSLState.Text += "关闭" + PublicVar.g_SCIComNum + "失败!";
                }
            }
        }


        private void UpdateDbColumns(SQLCommand sqlTable, FrameData frame)
        {
            //1.维持数据库表结构与帧结构一致
            for (int i = 0; i < frame.Parameter.Count; i++)
            {
                //申请10倍的空间
                sqlTable.insertColumn(frame.Parameter[i].name, "nvarchar(" + frame.Parameter[i].size + "0)");
            }
        }


    }
}
