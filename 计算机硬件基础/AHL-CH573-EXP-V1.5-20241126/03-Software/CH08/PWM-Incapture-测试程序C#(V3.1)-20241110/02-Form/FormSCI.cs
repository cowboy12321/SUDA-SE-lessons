using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Threading;
using System.Linq;
using System.Drawing.Drawing2D;
using System.Windows.Forms.DataVisualization.Charting;

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
        private bool state_high_low = true;
        private Queue<int> Yqueue = new Queue<int>();
        private Queue<double> Xqueue = new Queue<double>();
        private int t_flag=0;//判断芯片温度输出异常地语音播报标志位
        private int queueLen = 300;
        int backtime;
        //System.Timers.Timer thigh = new System.Timers.Timer(1);//实例化Timer类，设置间隔时间为1毫秒；
        
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
            //thigh.Interval = 1;
            //thigh.Elapsed += new System.Timers.ElapsedEventHandler(OrderTimer_Tickhigh);//到时间的时候执行事件； 
            //thigh.AutoReset = true;//设置是执行一次（false）还是一直执行(true)； 
            //thigh.Enabled = false;//是否执行System.Timers.Timer.Elapsed事件；

            //初始化时,按钮显示"打开串口"
            this.BtnSCISwitch.Text = "打开串口";
            this.CbSCIBaud.Enabled = true;    //[波特率选择框]处于可用状态
            this.CbSCIComNum.Enabled = true; //[串口选择框]处于可用状态


            #region 数据表
            this.chart1.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
            this.chart1.ChartAreas[0].AxisX.ScaleView.Size = 50;


            #endregion



            //自动搜索串口,并将其加入到[串口选择框]中
            int i;
            string[] SCIPorts;
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
            //折线图初始化
            chartLoading();
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
                    // 打开定时器事件
                    timer.Start();
                    //this.thigh.Enabled = true;
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
                    //thigh.Enabled = false;
                    timer.Stop();
                    // 清空绘制数据，此处代码一定要有，不然第二次打开串口时程序必定崩溃
                    this.chart1.Series[0].Points.Clear();
                }
                else//串口关闭失败
                {
                    this.TSSLState.Text += "关闭"+PublicVar.g_SCIComNum+"失败!";
                }
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
            //如果正在执行关闭操作
            if (closing)
            {
                return;
            }
            //String str = String.Empty;
            //StringBuilder实现字符拼接的效率更高
            StringBuilder MyStringBuilder = new StringBuilder("", 500); //优化效率
            //设置允许跨线程文本框赋值
            Control.CheckForIllegalCrossThreadCalls = false;
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
                if (current_string.Contains("1"))
                {
                    state_high_low = true;
                }
                else if(current_string.Contains("0"))
                {
                    state_high_low = false;
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
            this.TSSLState.Text = "过程提示:清空接收文本框!";
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

        private void lbChac_Click(object sender, EventArgs e)
        {

        }

        private void SetSerialPort_Enter(object sender, EventArgs e)
        {

        }

        private void TbShowString_TextChanged(object sender, EventArgs e)
        {

        }


        private void sSSerialPortInfo_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

      

        /// <summary>
        /// 折线图初始化
        /// </summary>、
        private DateTime minValue, maxValue;    //横坐标最小和最大值
        private void chartLoading()
        {

            minValue = DateTime.Now;          //x轴最小刻度 
            maxValue = minValue.AddSeconds(10); //X轴最大刻度,比最小刻度大1秒
            //定义图表区域
            this.chart1.ChartAreas.Clear();
            ChartArea chartArea1 = new ChartArea("C1");
            this.chart1.ChartAreas.Add(chartArea1);
            //定义存储和显示点的容器
            this.chart1.Series.Clear();

            Series series1 = new Series("电平");
            series1.ChartArea = "C1";
            this.chart1.Series.Add(series1);

            //设置图表显示样式
            this.chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";         //毫秒格式： hh:mm:ss.fff ，后面几个f则保留几位毫秒小数，此时要注意轴的最大值和最小值不要差太大
            this.chart1.ChartAreas[0].AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Seconds;
            this.chart1.ChartAreas[0].AxisX.LabelStyle.Interval = 1;                //坐标值间隔1S
            this.chart1.ChartAreas[0].AxisX.LabelStyle.IsEndLabelVisible = false;   //防止X轴坐标跳跃
            this.chart1.ChartAreas[0].AxisX.MajorGrid.IntervalType = DateTimeIntervalType.Seconds;
            this.chart1.ChartAreas[0].AxisX.MajorGrid.Interval = 1;                 //网格间隔

            this.chart1.ChartAreas[0].AxisY.Minimum = 0;
            this.chart1.ChartAreas[0].AxisY.Maximum = 1;
            this.chart1.ChartAreas[0].AxisY.LabelStyle.Interval = 1;

            this.chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            this.chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

            chart1.ChartAreas[0].AxisX.Minimum = minValue.ToOADate();
            chart1.ChartAreas[0].AxisX.Maximum = maxValue.ToOADate();

            //设置图表显示样式
            this.chart1.Series[0].Color = Color.Red;
            this.chart1.Series[0].ChartType = SeriesChartType.Line;
            this.chart1.Series[0].BorderWidth = 1;
            this.chart1.Series[0].XValueType= ChartValueType.DateTime;

            this.chart1.Series[0].Points.Clear();
        }
        //public void OrderTimer_Tickhigh(object source, System.Timers.ElapsedEventArgs e)
        //{
        //    try
        //    {
        //        if (state_high_low == true)
        //        {
        //            this.chart1.Series[0].Points.AddXY(DateTime.Now.ToOADate(), 1);
        //        }
        //        else
        //        {
        //            this.chart1.Series[0].Points.AddXY(DateTime.Now.ToOADate(), 0);
        //        }
        //        double removeBefore = DateTime.Now.AddSeconds((double)(1) * (-1)).ToOADate();
        //        /*
        //        while(this.chart1.Series[0].Points[0].XValue< removeBefore)
        //        {
        //            this.chart1.Series[0].Points.RemoveAt(0);
        //        }
        //        */
        //        int a = 0;
        //        lock (this.chart1.Series[0].Points)
        //        {

        //            for (; ; )
        //            {
                        
        //                a++;
        //                if (this.chart1.Series[0].Points[0].XValue < removeBefore)
        //                {
                            
        //                    Console.WriteLine("Hello World!{0}",a);
        //                    this.chart1.Series[0].Points.RemoveAt(0);
        //                    Console.WriteLine("Hello World!");
        //                }
        //                else
        //                    break;

        //            }
        //        }
        //        this.chart1.ChartAreas[0].AxisX.Minimum = this.chart1.Series[0].Points[0].XValue;
        //        this.chart1.ChartAreas[0].AxisX.Maximum = DateTime.FromOADate(this.chart1.Series[0].Points[0].XValue).AddSeconds(1).ToOADate();
        //        this.chart1.Invalidate();
        //    }
        //    catch
        //    {
        //        int aaa;
        //        aaa = 10;
        //    }
        //}

        private void timer_Tick(object sender, EventArgs e)
        {
            systemTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            try
            {
                Xqueue.Enqueue(DateTime.Now.ToOADate());
                if (state_high_low == true)
                {
                    Yqueue.Enqueue(1);
                }
                else
                {
                    Yqueue.Enqueue(0);
                }

                if (Xqueue.Count() >= queueLen)
                {
                    Xqueue.Dequeue();
                    Yqueue.Dequeue();
                }

                //this.chart1.ChartAreas[0].AxisX.Minimum = this.chart1.Series[0].Points[0].XValue;
                //this.chart1.ChartAreas[0].AxisX.Maximum = DateTime.FromOADate(this.chart1.Series[0].Points[0].XValue).AddSeconds(1).ToOADate();
                this.chart1.ChartAreas[0].AxisX.Minimum = Xqueue.Peek();
                this.chart1.ChartAreas[0].AxisX.Maximum = Xqueue.ElementAt(Xqueue.Count() - 1);


                this.chart1.Series[0].Points.DataBindXY(Xqueue, Yqueue);
                double a = chart1.Series[0].Points[0].XValue;
                this.chart1.Invalidate();
            }
            catch
            {
                int aaa;
                aaa = 10;
            }
        }


        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void systemTime_Click(object sender, EventArgs e)
        {

        }
    }
}