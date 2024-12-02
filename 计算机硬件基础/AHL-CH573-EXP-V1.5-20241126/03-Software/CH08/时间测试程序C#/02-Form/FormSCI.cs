using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SerialPort
{
    ///----------------------------------------------------------------------
    ///FrmSCI  :串口测试工程                                                
    ///功能描述:测试串口通信是否正常                                        
    ///         发送时可以选择字符串、十进制、十六进制三种方式              
    ///         接收到的数据分别以字符串、十进制、十六进制三种方式显示      
    ///目    的:测试串口                                                    
    ///说    明:                                                           
    ///注    意:                                                           
    ///日    期:2010年3月31日                                              
    ///编 程 者:LCX-WYH                                                     
    ///-----------串口测试工程(苏州大学飞思卡尔嵌入式中心)----------------------
    public partial class FrmSCI : Form
    {
        //委托,将从串口接收到的数据显示到接收框里面
        delegate void handleinterfaceupdatedelegate(Object textbox,
                                                    string text);

        //串口默认情况
        private string msg = "    无校验,8位数据位,1位停止位";
        private string str = "串口号、波特率:";
        private bool listening = false;    //标识是否执行完invoke相关操作
        private bool closing = false;      //标识是否正在关闭串口
        SCI sci = new SCI();    //要调用SCI类中所定义的函数
        private string current_string;

        ///-----------------------------------------------------------------
        /// <summary>                                                       
        /// 函 数 名:FrmSCI:类FrmSCI的构造函数                              
        /// 功    能:完成窗体的初始化工作                                   
        /// 函数调用:InitializeComponent                                    
        /// </summary>                                                      
        ///-----------------------------------------------------------------
        public FrmSCI()
        {
            InitializeComponent();
        }

        ///-----------------------------------------------------------------
        /// <summary>                                                       
        /// 对    象:FrmSCI:窗体                                            
        /// 事    件:Load                                                   
        /// 功    能:执行加载窗体程序,自动获得串口号                        
        ///          同时在标签LblSCI中显示串口相关信息                     
        /// 函数调用:无                                                     
        /// </summary>                                                      
        /// <param name="sender"></param>                                   
        /// <param name="e"></param>                                        
        /// <remarks></remarks>                                             
        ///-----------------------------------------------------------------
        private void FrmSCI_Load(object sender, EventArgs e)
        {
            //初始化时,按钮显示"打开串口"

            this.BtnSCISwitch.Text = "打开串口";
            this.CbSCIBaud.Enabled = true;    //[波特率选择框]处于可用状态
            this.CbSCIComNum.Enabled = true;　//[串口选择框]处于可用状态
            
            //自动搜索串口,并将其加入到[串口选择框]中
            int i;
            string[] SCIPorts;
            SCIPorts = System.IO.Ports.SerialPort.GetPortNames();//获取电脑上可用串口
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
                this.CbSCISendType.SelectedIndex = 0;

                //设置初始的串口号与波特率
                PublicVar.g_SCIComNum = CbSCIComNum.Text;
                PublicVar.g_SCIBaudRate = int.Parse(this.CbSCIBaud.Text);
                //显示当前串口信与状态信息
                this.LblSCI.Text = str + PublicVar.g_SCIComNum + "、" +
                                   PublicVar.g_SCIBaudRate + msg;
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
        /// 对    象:CbSCIBaud                                              
        /// 事    件:SelectedIndexChanged                                   
        /// 功    能:改变当前串口波特率                                     
        /// 函数调用:无                                                     
        /// </summary>                                                      
        /// <param name="sender"></param>                                   
        /// <param name="e"></param>                                        
        ///-----------------------------------------------------------------
        private void CbSCIBaud_SelectedIndexChanged(object sender,
                                                    EventArgs e)
        {
            PublicVar.g_SCIBaudRate = int.Parse(this.CbSCIBaud.Text);
            this.TSSLState.Text = "过程提示:选择波特率";
        }

        ///-----------------------------------------------------------------
        /// <summary>                                                       
        /// 对    象:CbSCIComNum:串口选择框                                 
        /// 事    件:SelectedIndexChanged                                  
        /// 功    能:改变当前串口号                                         
        /// 函数调用:无                                                     
        /// </summary>                                                      
        /// <param name="sender"></param>                                   
        /// <param name="e"></param>                                        
        ///-----------------------------------------------------------------
        private void CbSCIComNum_SelectedIndexChanged(object sender,
                                                      EventArgs e)
        {
            PublicVar.g_SCIComNum = this.CbSCIComNum.Text;
            this.TSSLState.Text = "过程提示:选择串口号";
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
                    //显示打开串口相关信息
                    this.LblSCI.Text = str + PublicVar.g_SCIComNum +
                        "、" + PublicVar.g_SCIBaudRate + msg;

                    this.BtnSCISwitch.Text = "关闭串口";
                    //[串口选择框]处于禁用状态
                    this.CbSCIComNum.Enabled = false;
                    //[波特率选择框]处于禁用状态
                    this.CbSCIBaud.Enabled = false;
                    //状态上显示结果信息
                    this.TSSLState.Text = this.TSSLState.Text +
                                          "打开" + PublicVar.g_SCIComNum + "成功!" + "波特率选择：" + PublicVar.g_SCIBaudRate;
                    this.pictureBox1.Image = SerialPort.Properties.Resources.Run;
                    this.SCIPort.DiscardInBuffer();
                }
                else//串口打开失败
                {
                    this.TSSLState.Text = this.TSSLState.Text +
                                          "打开" + PublicVar.g_SCIComNum + "失败!";
                    this.pictureBox1.Image = SerialPort.Properties.Resources.Run_static;
                }
            }
            else if (this.BtnSCISwitch.Text == "关闭串口")
            {

                //提示当前操作
                this.TSSLState.Text = "过程提示:正在关闭串口...";
                closing = true;     //正在关闭串口
                while (listening)
                {
                    //使其能实时响应其他事件，避免系统出现假死现象
                    //在接受数据操作时进行串口关闭操作，会因为假死现象对关闭串口事件没有反应
                    //调用DoEvents能够使其即时响应关闭串口事件
                    Application.DoEvents();
                }
                //执行关闭串口操作,并用Flag返回结果
                Flag = sci.SCIClose(this.SCIPort);
                if (Flag == true)
                {
                    this.LblSCI.Text = str + PublicVar.g_SCIComNum
                             + "、" + PublicVar.g_SCIBaudRate + msg;
                    this.BtnSCISwitch.Text = "打开串口";
                    //[串口选择框]处于可用状态
                    this.CbSCIComNum.Enabled = true;
                    //[波特率选择框]处于可用状态
                    this.CbSCIBaud.Enabled = true;
                    this.TSSLState.Text += "关闭"+PublicVar.g_SCIComNum+"成功!";
                    this.pictureBox1.Image = SerialPort.Properties.Resources.Run_static;
                    closing = false;    //关闭完成
                }
                else//串口关闭失败
                {
                    this.TSSLState.Text += "关闭"+PublicVar.g_SCIComNum+"失败!";
                }
            }
            
        }

        ///-----------------------------------------------------------------
        /// <summary>                                                       
        /// 对    象:BtnSCISend                                             
        /// 事    件:Click                                                  
        /// 功    能:发送数据:选择字符串发送时,以字符串的形式发送出去,      
        ///          选择十进制发送时,以十进制的形式发送                    
        ///          选择十六进制发送时,以十六进制的形式发送出              
        /// 函数调用:SCISendData:通过串口发送数据                           
        /// </summary>                                                      
        /// <param name="sender"></param>                                   
        /// <param name="e"></param>                                        
        ///-----------------------------------------------------------------
        private void BtnSCISend_Click(object sender, EventArgs e)
        {
            this.TSSLState.Text = "过程提示: 执行发送数据...";

            bool Flag;//判断数据发送是否成功
            int i, count = 0;
            int len;  //len保存发送数据的长度

            //0表示选择是字符发送,1表示的是十进制发送,2表示十六进制发送
            int SendType;
            SendType = CbSCISendType.SelectedIndex;

            //定义一个ArrayList类的实例对象,实现一个数组,其大小在添加元
            //素时自动变化
            System.Collections.ArrayList SendData = new
                System.Collections.ArrayList();

            //如果串口没有打开
            if (!SCIPort.IsOpen)
            {
                //状态条进行提示
                this.TSSLState.Text += "请先打开串口!";
                return;
            }
            //如果发送数据为空
            if (this.TbSCISend.Text == string.Empty)
            {
                this.TSSLState.Text += "发送数据不得为空!";
                return;
            }

            if (SendType == 0)//选择的是以字符串方式发送
            {
                this.TSSLState.Text = "以字符串方式发送数据!";
                //将要发送的数据进行编码,并获取编码后的数据长度
                len =System.Text.Encoding.Default.GetBytes(this.TbSCISend.Text).Length;
                //sci.SCIReceInt(SCIPort,len);//设置产生接收中断的字节数  【2014-5-5 注释，否则会导致程序无响应】
                //动态分配len字节单元内容用来存放发送数据
                PublicVar.g_SendByteArray = new byte[len];
               //获取TbSCISend文本的码值
                PublicVar.g_SendByteArray =
                  System.Text.Encoding.Default.GetBytes(this.TbSCISend.Text); 
            }
            else //选择的是以十进制或者是十六进制发送数据
            {
                //sci.SCIReceInt(SCIPort, 1);//设置产生接收中断的字节数    【2014-5-5 注释，否则会导致程序无响应】
                foreach (string str in this.TbSCISend.Text.Split(','))
                {
                    //排除掉连续两个逗号,说明之间没有数
                    if (str != string.Empty)
                    {
                        if (SendType == 1)//选择的是以十进制方式发送
                        {
                            //将文本框中的数转化为十进制存入ArrayList类的实例对象
                            SendData.Add(Convert.ToByte(str, 10));
                            count++;//进行计数,统计有效的数据个数
                        }
                        else
                        {
                            //将文本框中的数转化为十六进制存入ArrayList类的实例对象
                            SendData.Add(Convert.ToByte(str, 16));
                            count++;//进行计数,统计有效的数据个数
                        }
                    }
                }
                //动态分配空间存放发送数据
                PublicVar.g_SendByteArray = new byte[count];

                //将已经转化后的数据放入到全局变量g_SendByteArray中
                for (i = 0; i < count; i++)
                    PublicVar.g_SendByteArray[i] = (byte)SendData[i];
            }

            //发送全局变量g_SendByteArray中的数据,并返回结果
            Flag = sci.SCISendData(this.SCIPort, ref PublicVar.g_SendByteArray);

            if (Flag == true)//数据发送成功
                this.TSSLState.Text += "数据发送成功!";
            else
                this.TSSLState.Text += "数据发送失败!";
        }

        ///-----------------------------------------------------------------
        /// <summary>                                                       
        /// 对    象:BtnSCIClearSend                                        
        /// 事    件:Click                                                  
        /// 功    能:清除发送框中的内容                                     
        /// 函数调用:无                                                     
        /// </summary>                                                      
        /// <param name="sender"></param>                                   
        /// <param name="e"></param>                                        
        ///-----------------------------------------------------------------
        private void BtnSCIClearSend_Click(object sender, EventArgs e)
        {
            this.TbSCISend.Text = "";
            this.TSSLState.Text = "过程提示:清空发送文本框!";
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

        public static int recordFlag1 = 0;
        public static int recordFlag2 = 0;
        public static int hour2 = 0;
        public static int min2 = 0;
        public static int sec2 = 0;


        private void SCIPort_DataReceived(object sender,
            System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            //如果正在执行关闭操作
            if (closing)
            {
                return;
            }
            //String str = String.Empty;
            //StringBuilder实现字符拼接的效率更高
            StringBuilder MyStringBuilder = new StringBuilder("", 500); //优化效率

            bool Flag;//标记串口接收数据是否成功
            int len;//标记接收的数据的长度
            byte[] ch2=new byte[2];
            listening = true;    //开始接收处理数据，可能会用到多线程
            //调用串口接收函数,并返回结果
            Flag = sci.SCIReceiveData(SCIPort, ref PublicVar.g_ReceiveByteArray);
            if (Flag == true)//串口接收数据成功
            {
                len = PublicVar.g_ReceiveByteArray.Length;
                //对于字符串形式,考虑到可能有汉字,
                //直接调用系统定义的函数,处理整个字符串
                //str = Encoding.Default.GetString(PublicVar.g_ReceiveByteArray);
                MyStringBuilder.Append(Encoding.GetEncoding("GBK").GetString(PublicVar.g_ReceiveByteArray));//优化效率
                //显示字符串接收内容                                    
                //SCIUpdateRevtxtbox(TbShowString, str);                
                SCIUpdateRevtxtbox(TbShowString, MyStringBuilder.ToString());
                current_string = MyStringBuilder.ToString();
                if (current_string.Contains("金葫芦提示"))//复位
                {
                    recordFlag1 = 0;
                    Control.CheckForIllegalCrossThreadCalls = false;
                    lblFlag.Text = "---";
                    lblM.Text = "0";
                    hour2 = 0;
                    min2 = 0;
                    sec2 = 0;
    }
                if (recordFlag1==0&& current_string.Contains(":"))
                {
                    stopwatch.Start(); //  开始监视代码运行时间
                    Control.CheckForIllegalCrossThreadCalls = false;
                    lblFlag.Text = "开始计时";
                    string[] arr = current_string.Split(new char[] { ':' });
                    int i = arr.Length;
                    int hour1 = Convert.ToInt32(arr[0]);
                    int min1 = Convert.ToInt32(arr[1]);
                    int sec1= Convert.ToInt32(arr[2]);
                   
                    sec2 = sec1 + 30;
                    min2 = min1;
                    
                    hour2 = hour1;
                    if (sec2>=60)
                    {
                        sec2 = sec2 - 60;
                        min2 = min1 + 1;
                        if(min2>=60)
                        {
                            min2 = 0;
                            hour2 = hour1 + 1;
                            if (hour2 >= 24)
                            {
                                hour2 = 0;
                            }
                        }
                        
                    }
                    recordFlag1 = 1;
                    recordFlag2 = 1;

                }
                if (recordFlag2 == 1)
                {
                    string sss = hour2.ToString() + ':' + min2.ToString() + ':' + sec2.ToString() + "\r\n";
                    if(current_string==sss)
                    {
                        stopwatch.Stop();  //  停止监视代码运行时间
                        Control.CheckForIllegalCrossThreadCalls = false;
                        lblFlag.Text = "停止计时";
                        recordFlag2 = 0;
                        
                        TimeSpan timespan = stopwatch.Elapsed; //  获取当前实例测量得出的总时间
                        double seconds = timespan.TotalSeconds;  //  总秒数
                        //防止复位计时值变化，最多7次
                        if (seconds > 215)
                        {
                            seconds = seconds - 210;
                            lblM.Text = seconds.ToString();
                        }
                        else if (seconds > 185)
                        {
                            seconds = seconds - 180;
                            lblM.Text = seconds.ToString();
                        }
                        else if (seconds > 155)
                        {
                            seconds = seconds - 150;
                            lblM.Text = seconds.ToString();
                        }
                        else if (seconds > 125)
                        {
                            seconds = seconds - 120;
                            lblM.Text = seconds.ToString();
                        }
                        else if (seconds > 95)
                        {
                            seconds = seconds - 90;
                            lblM.Text = seconds.ToString();
                        }
                        else if (seconds > 65)
                        {
                            seconds = seconds - 60;
                            lblM.Text = seconds.ToString();
                        }
                        else if (seconds > 35)
                        {
                            seconds = seconds - 30;
                            lblM.Text = seconds.ToString();
                        }
                        else
                            lblM.Text = timespan.TotalSeconds.ToString();
                    }

                }
               

                //MyStringBuilder.Clear();     //清空MyStringBuilder

                //十进制和十六进制形式按字节进行处理
                for (int i = 0; i < len; i++)
                {
                    //十进制都是按照三位来显示,字节之间有空格表示区分
                    //如果十进制显示被选中，则显示十进制接收内容
                    if (TbShowDec_checkBox.Checked)
                        SCIUpdateRevtxtbox(TbShowDec,
                                PublicVar.g_ReceiveByteArray[i].ToString("D3") + "  ");
                    //十六进制都是按照两位来显示,字节之间有空格表示区分
                    //如果十六进制显示被选中，则显示十六进制接收内容
                    if (TbShowHex_checkBox.Checked)
                        SCIUpdateRevtxtbox(TbShowHex,
                                PublicVar.g_ReceiveByteArray[i].ToString("X2") + "  ");
                    }

                   // sci.SCIReceInt(SCIPort, 1);//设置产生接收中断的字节数【2014-5-5 注释，否则会导致程序无响应】
                    this.TSSLState.Text = "过程提示:数据接收成功!";
            }
            //接收数据失败
            else
            {
                //sci.SCIReceInt(SCIPort, 1);//设置产生接收中断的字节数【2014-5-5 注释，否则会导致程序无响应】 
                this.TSSLState.Text = "过程提示:数据接收失败!";
            }
            listening = false;  //接收数据结束
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
        /// 对    象:BtnSCIClearRec                                         
        /// 事    件:Click                                                  
        /// 功    能:清除接收文本框                                         
        /// 函数调用:无                                                     
        /// </summary>                                                      
        /// <param name="sender"></param>                                   
        /// <param name="e"></param>                                        
        ///-----------------------------------------------------------------
        private void btnClearRec_Click(object sender, EventArgs e)
        {
            this.TbShowString.Text = string.Empty;
            this.TbShowDec.Text = string.Empty;
            this.TbShowHex.Text = string.Empty;
            this.TSSLState.Text = "过程提示:清空接收文本框!";
        }

        ///-----------------------------------------------------------------
        /// <summary>                                                       
        /// 对    象:TbSCISend                                              
        /// 事    件:KeyPress                                                 
        /// 功    能:限制发送框内输入数据的格式                             
        /// 函数调用:无                                                     
        /// </summary>                                                      
        /// <param name="sender"></param>                                   
        /// <param name="e"></param>                                        
        ///----------------------------------------------------------------- 
        private void TbSCISend_KeyPress(object sender, KeyPressEventArgs e)
        {
            int select = CbSCISendType.SelectedIndex;
            if (select == 1)//输入的是十进制的数
            {
                //除了数字、逗号和退格键,其他都不给输入
                if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == 0x08
                    || e.KeyChar == ',')
                {
                    //输入的是数字,可以任意输入逗号与退格符
                    if (e.KeyChar >= '0' && e.KeyChar <= '9')
                    {
                        //在文本框中没有逗号时,
                        //this.TbSCISend.Text.LastIndexOf(',')默认为-1
                        //逗号之后出现第三个数字时,
                        //才用得着考虑是否会大于255
                        if (this.TbSCISend.Text.Length -
                            this.TbSCISend.Text.LastIndexOf(',') >= 2)
                        {
                            //考虑如果输入的话,是否会超出255
                            if (int.Parse(
                                    this.TbSCISend.Text.Substring(
                                 TbSCISend.Text.LastIndexOf(',') + 1)) * 10
                                    + e.KeyChar - '0' > 255)
                            {
                                e.Handled = true;
                                this.TSSLState.Text = "输入数据不得大于255";
                            }
                            //默认情况下是允许输入的,即e.Handled = false
                        }
                    }
                }
                else
                {
                    e.Handled = true;//除了逗号、数字0~9,其他都不给输入
                    this.TSSLState.Text = "输入数据必须是0-9,或者逗号"
                                          + ",或者退格符";
                }
            }
            //十六进制的处理方式与十进制相同,只是判断是否大于255时不太一样
            else if (select == 2)
            {
                //除了数字、大写字母、小写字母、逗号和退格键,其他都不给输入
                if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar >= 'a'
                    && e.KeyChar <= 'f' || e.KeyChar >= 'A' && e.KeyChar <=
                    'F' || e.KeyChar == 0x08 || e.KeyChar == ',')
                {
                    //逗号和退格符可以任意输入,只考虑输入数字的情况
                    if (e.KeyChar >= '0' && e.KeyChar <= '9')
                    {
                        if (this.TbSCISend.Text.Length -
                             this.TbSCISend.Text.LastIndexOf(',') >= 2)
                        {
                            if (Convert.ToInt32(TbSCISend.Text.Substring(
                               TbSCISend.Text.LastIndexOf(',') + 1), 16) * 16
                                + (e.KeyChar - '0') > 255)
                            {
                                e.Handled = true;
                                this.TSSLState.Text = "输入数据不得大于255";
                            }
                        }
                    }
                    else if (e.KeyChar >= 'a' && e.KeyChar <= 'f'
                        || e.KeyChar >= 'A' && e.KeyChar <= 'F')
                    {
                        if (this.TbSCISend.Text.Length -
                             this.TbSCISend.Text.LastIndexOf(',') >= 2)
                        {
                            //无论是大写字母还是小写字母都转化成大写字母判断
                            if (Convert.ToInt32(TbSCISend.Text.Substring(
                               TbSCISend.Text.LastIndexOf(',') + 1), 16) * 16
                                + (Char.ToUpper(e.KeyChar) - 'A') > 255)
                            {
                                e.Handled = true;
                                this.TSSLState.Text = "输入数据不得大于255";
                            }
                        }
                    }
                }
                else
                {
                    e.Handled = true;
                    this.TSSLState.Text = "输入数据必须是0-9,a-f,A-F,或者逗号"
                                          + ",或者退格符";
                }
            }
        }

        ///-----------------------------------------------------------------
        /// <summary>                                                       
        /// /// 对    象:CbSCISendType                                          
        /// 事    件:SelectedIndexChanged                                   
        /// 功    能:当选泽发送方式时,提示用户输入数据的格式,并清空发送框   
        ///         (1)当选中"字符串"时,提示用户"请输入字符串"  
        ///         (2)当选中"十进制"时,提示用户"请输入十进制数
        ///            ,以逗号隔开,数据范围0-255,允许用退格键"              
        ///         (3)当选中"十六进制":,提示用户"请输入十六进制数, 
        ///            以逗号隔开,数据范围00-FF,允许用退格键"               
        /// 函数调用:无                                                     
        /// </summary>                                                      
        /// <param name="sender"></param>                                   
        /// <param name="e"></param>                                        
        ///-----------------------------------------------------------------
        private void CbSCISendType_SelectedIndexChanged(object sender,
                                                        EventArgs e)
        {
            this.TSSLState.Text = "过程提示:选择数据发送方式!";
            //选择某一种发送方式时,首先清空发送框
            this.TbSCISend.Text = string.Empty;

            //选择的是发送字符串
            if (CbSCISendType.SelectedIndex == 0)
            {
                this.LabNote.Text = "请输入字符串!";
                this.TSSLState.Text = "你选择了发送字符串!";
            }
            else if (CbSCISendType.SelectedIndex == 1)
            {
                this.LabNote.Text = "请输入十进制数,以逗号(英文输入法下)隔开"
                                    + ",数据范围0-255,允许用退格键";
                this.TSSLState.Text = "你选择了发送十进制数!";
            }
            else
            {
                this.LabNote.Text = "请输入十六进制数,以逗号(英文输入法下)隔"
                                    + "开,数据范围00-FF,允许用退格键";
                this.TSSLState.Text = "你选择了发送十六进制数! ";
            }
        }

        ///-----------------------------------------------------------------
        /// <summary>                                                       
        /// 对    象:BtnState                                               
        /// 事    件:Click                                                  
        /// 功    能:控制状态条显示或隐藏                                   
        /// 函数调用:无                                                     
        /// </summary>                                                      
        /// <param name="sender"></param>                                   
        /// <param name="e"></param>                                        
        ///-----------------------------------------------------------------
        private void BtnState_Click(object sender, EventArgs e)
        {
            //状态条是不可见的
            if (this.TSSLState.Visible == false)
            {
                this.sSSerialPortInfo.Visible = true;//状态条可见
                BtnState.Text = "隐藏状态条";
            }
            //当前状态条不可见
            else
            {
                this.sSSerialPortInfo.Visible = false;//状态条不可见
                BtnState.Text = "显示状态条";
            }
        }

        ///-----------------------------------------------------------------
        /// <summary>                                                       
        /// 对    象:FrmSCI                                                 
        /// 事    件:FormClosing                                               
        /// 功    能:关闭窗体时,确保串口已经关闭                            
        /// 函数调用:SCIClose                                               
        /// </summary>                                                      
        /// <param name="sender"></param>                                   
        /// <param name="e"></param>                                        
        ///-----------------------------------------------------------------
        private void FrmSCI_FormClosing(object sender,
            FormClosingEventArgs e)
        {
            try
            {
                closing = true;     //正在关闭串口
                while (listening)
                {
                    //使其能实时响应其他事件，避免系统出现假死现象
                    Application.DoEvents();
                }
                sci.SCIClose(SCIPort);
                closing = false;    //关闭完成
            }
            catch
            { }
        }

        private void SerialPortSend_Enter(object sender, EventArgs e)
        {

        }

        private void lbChac_Click(object sender, EventArgs e)
        {

        }

        private void SetSerialPort_Enter(object sender, EventArgs e)
        {

        }

        private void TbShowString_TextChanged(object sender, EventArgs e)
        {

        }

        private void TbSCISend_TextChanged(object sender, EventArgs e)
        {

        }

        private void sSSerialPortInfo_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void TbShowDec_TextChanged(object sender, EventArgs e)
        {

        }

       

        private void TbShowDec_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            //十进制形式被选中，判断十六进制形式的选择情况
            if (TbShowDec_checkBox.Checked)
            {
                //十六进制形式被选中
                if (TbShowHex_checkBox.Checked)
                {
                    //修改字符形式显示框位置和大小
                    TbShowString.Location = new Point(20, 60);
                    TbShowString.Width = 250;
                    //修改十进制形式显示框大小、位置和是否可见
                    TbShowDec.Location = new Point(286, 60);
                    TbShowDec.Width = 250;
                    TbShowDec.Visible = true;
                    //修改十六进制形式显示框大小、位置和是否可见
                    TbShowHex.Location = new Point(548, 60);
                    TbShowHex.Width = 250;
                    TbShowHex.Visible = true;
                }
                //十六进制显示未被选中
                else
                {
                    TbShowString.Location = new Point(20, 60);
                    TbShowString.Width = 390;
                    TbShowDec.Location = new Point(415, 60);
                    TbShowDec.Width = 390;
                    TbShowDec.Visible = true;
                    TbShowHex.Visible = false;
                }
            }
            //十进制显示未被选中，判断十六进制显示的选择状况
            else
            {
                //十六进制显示被选中
                if (TbShowHex_checkBox.Checked)
                {
                    TbShowString.Location = new Point(20, 60);
                    TbShowString.Width = 390;
                    TbShowDec.Visible = false;
                    TbShowHex.Location = new Point(415, 60);
                    TbShowHex.Width = 390;
                    TbShowHex.Visible = true;
                }
                //十六进制显示未被选中
                else
                {
                    TbShowString.Location = new Point(20, 60);
                    TbShowString.Width = 790;
                    TbShowDec.Visible = false;
                    TbShowHex.Visible = false;
                }
            }
        }

        private void TbShowHex_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (TbShowDec_checkBox.Checked)
            {
                if (TbShowHex_checkBox.Checked)
                {
                    TbShowString.Location = new Point(20, 60);
                    TbShowString.Width = 250;
                    TbShowDec.Location = new Point(286, 60);
                    TbShowDec.Width = 250;
                    TbShowDec.Visible = true;
                    TbShowHex.Location = new Point(548, 60);
                    TbShowHex.Width = 250;
                    TbShowHex.Visible = true;
                }
                else
                {
                    TbShowString.Location = new Point(20, 60);
                    TbShowString.Width = 390;
                    TbShowDec.Location = new Point(415, 60);
                    TbShowDec.Width = 390;
                    TbShowDec.Visible = true;
                    TbShowHex.Visible = false;
                }
            }
            else
            {
                if (TbShowHex_checkBox.Checked)
                {
                    TbShowString.Location = new Point(20, 60);
                    TbShowString.Width = 390;
                    TbShowDec.Visible = false;
                    TbShowHex.Location = new Point(415, 60);
                    TbShowHex.Width = 390;
                    TbShowHex.Visible = true;
                }
                else
                {
                    TbShowString.Location = new Point(20, 60);
                    TbShowString.Width = 790;
                    TbShowDec.Visible = false;
                    TbShowHex.Visible = false;
                }
            }
        }

        private void lbDec_Click(object sender, EventArgs e)
        {

        }
        public static int count = 1;
        public static int flag = 1;
       

     
        public static int timeFlag = 0;
        public static int resetTime = 0;

        public static int controlFlag = 0;
        public static int controlFlag1 = 0;
        System.Diagnostics.Stopwatch stopwatch = new Stopwatch();
        private void timer2_Tick(object sender, EventArgs e)
        {
            
        }
        public static int GetCountByAppointString(string sourceString, string appointString, string type)
        {
            if (sourceString == null || appointString == null)
                return -3;

            int int_count = 0;
            int int_AstringLength = appointString.Length;
            int int_SstringLength = sourceString.Length;

            if (int_SstringLength < int_AstringLength)
                return -1;
            if (int_SstringLength == 0 || int_AstringLength == 0)
                return -2;

            if (type == "for")
            {
                for (int i = 0; i < sourceString.Length - 2; i++)
                {
                    if (sourceString.Substring(i, int_AstringLength) == appointString)
                        int_count++;
                }
            }
            else if (type == "replace")
            {
                sourceString = sourceString.Replace(appointString, string.Empty);
                int_count = (int_SstringLength - sourceString.Length) / int_AstringLength;
            }
          
            return int_count;
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click_1(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void lblFlag_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now; // 声明一个DateTime的对象；
            // 获取当前的时间；
            string strTime = dt.ToLongTimeString().ToString();
            // 用控件显示出来；
            label5.Text =  strTime ;
        }
        //------------------------------------------------------------------
    }
}