using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using SerialPort;
using System.Threading;
using Light;


namespace AHL_GEC
{
    public partial class FormSendAndReceive : Form
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
        public FormSendAndReceive()
        {
            InitializeComponent();
            //处理线程间操作无效的问题
            CheckForIllegalCrossThreadCalls = false;
        }


        private void FrmSCI_Load(object sender, EventArgs e)
        {
            //初始化时,按钮显示"打开串口"

            this.BtnSCISwitch.Text = "打开串口";
            this.CbSCIBaud.Enabled = true;    //[波特率选择框]处于可用状态
            this.CbSCIComNum.Enabled = true; //[串口选择框]处于可用状态

            
            this.CbSCIComNum2.Enabled = true; //[串口选择框]处于可用状态

            //自动搜索串口,并将其加入到[串口选择框]中
            int i;
            string[] SCIPorts;
            SCIPorts = System.IO.Ports.SerialPort.GetPortNames();
            this.CbSCIComNum.Items.Clear();//首先将现有的项清除掉
            this.CbSCIComNum2.Items.Clear();//首先将现有的项清除掉
            for (i = 0; i < SCIPorts.Length; i++) { 
                //向[串口选择框]中添加搜索到的串口号
                this.CbSCIComNum.Items.Add(SCIPorts[i]);
                this.CbSCIComNum2.Items.Add(SCIPorts[i]);
            }
            //设置各组合框的初始显示值
            if (SCIPorts.Length != 0)
            {
                this.BtnSCISwitch.Enabled = true;
                this.CbSCIBaud.SelectedIndex = 0;
                this.CbSCIComNum.SelectedIndex = 0;
                this.CbSCIComNum2.SelectedIndex = 0;
                //this.CbSCISendType.SelectedIndex = 0;

                //设置初始的串口号与波特率
                PublicVar.g_SCIComNum = CbSCIComNum.Text;
                PublicVar.g_SCIComNum2 = CbSCIComNum2.Text;
                PublicVar.g_SCIBaudRate = int.Parse(this.CbSCIBaud.Text);
                //显示当前串口信与状态信息
                this.LblSCI.Text = str + PublicVar.g_SCIComNum + "、" +
                                   PublicVar.g_SCIBaudRate + msg+";"+ PublicVar.g_SCIComNum2 + "、" +
                                   PublicVar.g_SCIBaudRate;
                this.TSSLState.Text = "无操作,请先选择波特率与串口号,打开串口," +
                                 "然后发送数据";
            }
            else
            {
                this.TSSLState.Text = "没有可用的串口,请检查!";
                this.BtnSCISwitch.Enabled = false;
            }


        }

        private void CbSCIBaud_SelectedIndexChanged(object sender,
                                            EventArgs e)
        {
            PublicVar.g_SCIBaudRate = int.Parse(this.CbSCIBaud.Text);
            this.TSSLState.Text = "过程提示:选择波特率";
        }

        private void CbSCIComNum_SelectedIndexChanged(object sender,
                                              EventArgs e)
        {
            PublicVar.g_SCIComNum = this.CbSCIComNum.Text;
            this.TSSLState.Text = "过程提示:选择串口号";
        }
        private void CbSCIComNum2_SelectedIndexChanged(object sender,
                                      EventArgs e)
        {
            PublicVar.g_SCIComNum2 = this.CbSCIComNum2.Text;
            this.TSSLState.Text = "过程提示:选择串口号";
        }
        private void BtnSCISwitch_Click(object sender, EventArgs e)
        {
            bool Flag2;//
            bool Flag;//标记打开是否成功

            //根据按钮BtnSCISwitch显示内容执行打开或关闭串口操作
            if (this.BtnSCISwitch.Text.CompareTo("打开串口") == 0)
            {
                //提示当前正在执行打开串口操作
                if (PublicVar.g_SCIComNum.Equals(PublicVar.g_SCIComNum2))
                {
                    this.TSSLState.Text = "过程提示:不可以打开两个相同的串口";
                    return;
                }
                this.TSSLState.Text = "过程提示:正在打开串口...";
                //进行串口的初始化,并用Flag返回结果
                Flag = sci.SCIInit(SCIPort, PublicVar.g_SCIComNum,
                            PublicVar.g_SCIBaudRate);
                Flag2 = sci.SCIInit(SCIPort2, PublicVar.g_SCIComNum2,
                            PublicVar.g_SCIBaudRate);
                //截取字符串，得到串口号
                //string portNum = PublicVar.g_SCIComNum.Remove(0, 3);
                //int port2 = Convert.ToInt32(portNum) - 1;
                //Flag2 = sci.SCIInit(SCIPort2, "COM" + port2,
                //            PublicVar.g_SCIBaudRate);

                //Console.WriteLine(Flag2);
                if (Flag == true)//串口打开成功
                {
                    //显示打开串口相关信息
                    this.LblSCI.Text = str + PublicVar.g_SCIComNum + PublicVar.g_SCIComNum2 +
                        "、" +
                        "、" + PublicVar.g_SCIBaudRate + msg;

                    this.BtnSCISwitch.Text = "关闭串口";
                    //[串口选择框]处于禁用状态
                    this.CbSCIComNum.Enabled = false;
                    this.CbSCIComNum2.Enabled = false;
                    //[波特率选择框]处于禁用状态
                    this.CbSCIBaud.Enabled = false;
                    //状态上显示结果信息
                    this.TSSLState.Text = this.TSSLState.Text +
                                          "打开" + PublicVar.g_SCIComNum + "、" + PublicVar.g_SCIComNum2 + "成功!" + "波特率选择：" + PublicVar.g_SCIBaudRate;
                    this.pictureBox1.Image = AHL_GEC.Properties.Resources.Run;
                }
                else//串口打开失败
                {
                    this.TSSLState.Text = this.TSSLState.Text +
                                          "打开" + PublicVar.g_SCIComNum + "、" + PublicVar.g_SCIComNum2 + "失败!";
                    this.pictureBox1.Image = AHL_GEC.Properties.Resources.Run_static;
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
                Flag2 = sci.SCIClose(this.SCIPort2);
                if (Flag == true)
                {
                    this.LblSCI.Text = str + PublicVar.g_SCIComNum + "、" + PublicVar.g_SCIComNum2
                             + "、" + PublicVar.g_SCIBaudRate + msg;
                    this.BtnSCISwitch.Text = "打开串口";
                    //[串口选择框]处于可用状态
                    this.CbSCIComNum.Enabled = true;
                    this.CbSCIComNum2.Enabled = true;
                    //[波特率选择框]处于可用状态
                    this.CbSCIBaud.Enabled = true;
                    this.TSSLState.Text += "关闭" + PublicVar.g_SCIComNum + "、" + PublicVar.g_SCIComNum2 + "成功!";
                    this.pictureBox1.Image = AHL_GEC.Properties.Resources.Run_static;
                    closing = false;    //关闭完成
                }
                else//串口关闭失败
                {
                    this.TSSLState.Text += "关闭" + PublicVar.g_SCIComNum + "、" + PublicVar.g_SCIComNum2 + "失败!";
                }
            }
        }

        private void SCIPort_DataReceived(object sender,
            System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (closing)
            {
                return;
            }

            listening = true;

            bool Flag;
            //需要读回的数据
            String hardAdr = "";
            String softAdr = "";
            String time = "";

            StringBuilder MyStringBuilder = new StringBuilder("", 500); //优化效率

            //串口传回的数据存放在PublicVar.g_ReceiveByteArray中
            Flag = sci.SCIReceiveData(SCIPort, ref PublicVar.g_ReceiveByteArray);
            //byte[] haddr = BitConverter.GetBytes(Convert.ToInt32(this.TbSendHardAdr.Text, 16));
            byte[] hardAdrArr = new byte[4];

            hardAdrArr[0] = PublicVar.g_ReceiveByteArray[0];
            hardAdrArr[1] = PublicVar.g_ReceiveByteArray[1];
            hardAdrArr[2] = PublicVar.g_ReceiveByteArray[2];
            hardAdrArr[3] = PublicVar.g_ReceiveByteArray[3];
            Int32 hardAdr32 = (hardAdrArr[0] << 24) + (hardAdrArr[1]<< 16) + (hardAdrArr[2] << 8) + hardAdrArr[3];

            hardAdr ="0x" + Convert.ToString(hardAdr32, 16);

            Int16 softAdr1 = (Int16)(((PublicVar.g_ReceiveByteArray[4]) << 8) + PublicVar.g_ReceiveByteArray[5]);
            softAdr = Convert.ToString(softAdr1);


            byte[] byteTime = new byte[24];
            for (int i = 0; i < 24; i++)
            {
                byteTime[i] = PublicVar.g_ReceiveByteArray[i + 6];
            }

            MyStringBuilder.Append(Encoding.GetEncoding("GBK").GetString(byteTime));

            SCIUpdateRevtxtbox(TbShowHardAdr, hardAdr);
            SCIUpdateRevtxtbox(TbShowSoftAdr, softAdr);
            SCIUpdateRevtxtbox(TbShowTime, Convert.ToString(MyStringBuilder));
            MyStringBuilder.Clear();

            listening = false;

        }


        private void SCIPort2_DataReceived(object sender,
            System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            
            sci.SCIReceiveData(SCIPort2, ref PublicVar.g_ReceiveByteArray2);
            //string data = Convert.ToString(PublicVar.g_ReceiveByteArray2);
            string data = System.Text.Encoding.Default.GetString(PublicVar.g_ReceiveByteArray2);
            //Console.WriteLine(data);
            this.textBoxReceive.Text += data;
        }

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


        private void SetSerialPort_Enter(object sender, EventArgs e)
        {

        }


        private void btnChooseSend_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            //FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Title = "请选择文件路径";
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            string lightFilePath = dialog.FileName.Trim();

            FileStream fs = new FileStream(lightFilePath, FileMode.Open, FileAccess.Read);
            long length = fs.Length;//数据的长度
            //length += 10;
            byte[] sendData = new byte[length + 12];
            int i;
            bool Flag = false;
            int groupCount = 0;//拆成了多少组

            UInt32 lengthNum = (UInt32)length;//放在帧头
            sendData[0] = (byte)'u';
            sendData[1] = (byte)'p';
            sendData[2] = (byte)'d';
            sendData[3] = (byte)'a';
            sendData[4] = (byte)'t';
            sendData[5] = (byte)'e';

            sendData[6] = (byte)(lengthNum >> 24);
            sendData[7] = (byte)(lengthNum >> 16);
            sendData[8] = (byte)(lengthNum >> 8);
            sendData[9] = (byte)(lengthNum);

            string lightFileName = Path.GetFileNameWithoutExtension(lightFilePath);
            this.textBoxLightFile.Text = lightFileName + ".txt";
            this.textBoxCurrentState.Text = "选择文件成功";

            int softAdr = Convert.ToInt16(lightFileName.Remove(0, 5));
            sendData[10] = (byte)(softAdr>>8);
            sendData[11] = (byte)softAdr;
            //

            //软件地址改为灯的编号，10进制表示
            //this.textBoxSoftAdr.Text = "0x" + Convert.ToString(softAdr, 16);
            this.textBoxSoftAdr.Text = Convert.ToString(softAdr);
            for (i = 0; i < length; i++)
            {
                sendData[i + 12] = (byte)fs.ReadByte();
            }
            fs.Close();

            //所有的数据在sendData中
            //拆分进行发送，因为一次性发送太多的情况会漏掉数据
            //在测试过程中，720个字节+11个帧头的情况是能正确发送的，所有这里以720为一段
            if (length <= 720)
            {
                ///////////////////////
                Flag = sci.SCISendData(this.SCIPort, ref sendData);
            }
            else
            {
                //加上11个帧头
                byte[] sendData1 = new byte[732];
                for (i = 0; i < 732; i++)
                {
                    sendData1[i] = sendData[i];
                }
                ///////////////////////
                Flag = sci.SCISendData(this.SCIPort, ref sendData1);
                groupCount += 1;
                length -= 720;

                while (length > 0)
                {
                    //sleep放在循环内部。如果长度小于720不需要sleep,如果大于2个720,也可以正常sleep
                    Thread.Sleep(10);
                    byte[] sendData2 = new byte[length <= 720 ? length : 720];
                    for (i = 0; i < sendData2.Length; i++)
                    {
                        sendData2[i] = sendData[i + groupCount * 720 + 12];
                    }
                    ///////////////////////
                    Flag = sci.SCISendData(this.SCIPort, ref sendData2);
                    groupCount += 1;
                    length -= 720;

                }
            }



            //Array.Clear(sendData, 0, sendData.Length);


            if (Flag == true)
            {
                this.textBoxCurrentState.Text += "发送文件成功";
            }
            else
            {
                this.textBoxCurrentState.Text += ",发送文件失败";
            }
        }

        private void btnReadData_Click(object sender, EventArgs e)
        {
            this.TSSLState.Text = "过程提示: 执行读取数据...";
            bool Flag;

            if (!SCIPort.IsOpen)
            {
                this.TSSLState.Text += "请先打开串口!";
                return;
            }

            //清空之前的数据
            this.TbShowHardAdr.Text = "";
            this.TbShowSoftAdr.Text = "";
            //this.TbSendTemperature.Text = "";
            this.TbShowTime.Text = "";

            PublicVar.g_SendByteArray = new byte[1];
            PublicVar.g_SendByteArray = System.Text.Encoding.Default.GetBytes("s");
            Flag = sci.SCISendData(this.SCIPort, ref PublicVar.g_SendByteArray);

            //好像只能代表s请求发送成功，串口回发的数据是否成功也不一定
            if (Flag == true)
                this.TSSLState.Text += "数据读取成功!";
            else
                this.TSSLState.Text += "数据读取失败!";
        }

        private void btnSendStartTime_Click(object sender, EventArgs e)
        {
            int i;
            bool flag;
            //int relativeTimeNum;
            //获取要发送的数据
            string absoluteTime = this.textBoxAbsTime.Text;
            //string relativeTime = this.textBoxRelativeTime.Text;
            DataProcess dataProcess = new DataProcess();
            //回头写一个判断时间格式的函数
            //目前是空的
            if (dataProcess.judgeTimeFormat(absoluteTime))
            {
                byte[] sendDataAbs = new byte[absoluteTime.Length + 1];//发送数据=时间数据+帧头t
                byte[] absTimeData = new byte[absoluteTime.Length];//时间数据
                absTimeData = System.Text.Encoding.UTF8.GetBytes(absoluteTime);
                sendDataAbs[0] = (byte)'t';
                for (i = 0; i < absoluteTime.Length; i++)
                {
                    sendDataAbs[i + 1] = absTimeData[i];
                }
                flag = sci.SCISendData(this.SCIPort, ref sendDataAbs);

            }
        }

        private void BtnSCISend_Click(object sender, EventArgs e)
        {
            this.TSSLState.Text = "过程提示: 执行发送数据...";

            bool Flag;//判断数据发送是否成功
            int i;

            //0表示选择是字符发送,1表示的是十进制发送,2表示十六进制发送
            //SendType = CbSCISendType.SelectedIndex;

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
            //这里暂时只看了其中一个数据
            if ( this.TbSendSoftAdr.Text == string.Empty)
            {
                this.TSSLState.Text += "发送数据不得为空!";
                return;
            }


            //目前只发送两个数据

            PublicVar.g_SendByteArray = new byte[31];
            //len = System.Text.Encoding.Default.GetBytes(this.TbSendHardAdr.Text).Length;
            //len += System.Text.Encoding.Default.GetBytes(this.TbSendSoftAdr.Text).Length;
            //String sendData = 'r' + this.TbSendHardAdr.Text + this.TbSendSoftAdr.Text;

            //标志位，第一位为'r'表示下位机要接收数据，第一位为's'表示下位机要发送数据
            PublicVar.g_SendByteArray[0] = (byte)'r';
            //byte hardAdr, softAdr;
            //这里判断合不合法的写的不太全面，暂时先用着
            //if (this.TbSendSoftAdr.Text.Length > 2 || this.TbSendTime.Text.Length != 23)
            if ( this.TbSendTime.Text.Length != 23)
            {

                this.TbSendSoftAdr.Text = "";
                this.TbSendHardAdr.Text = "";
                this.TbSendTime.Text = "";
                this.TSSLState.Text += "地址长度不合法!";
            }
            else
            {
                Int32 HardAdr = Convert.ToInt32(this.TbSendHardAdr.Text, 16);
                byte[] haddr = BitConverter.GetBytes(Convert.ToInt32(this.TbSendHardAdr.Text, 16));
                //PublicVar.g_SendByteArray[1] = Convert.ToByte((byte)(HardAdr >> 24));
                //PublicVar.g_SendByteArray[2] = Convert.ToByte((byte)(HardAdr >> 16));
                //PublicVar.g_SendByteArray[3] = Convert.ToByte((byte)(HardAdr >> 8));
                //PublicVar.g_SendByteArray[4] = Convert.ToByte((byte)(HardAdr ));
                //PublicVar.g_SendByteArray[1] = Convert.ToByte("1", 16);
                PublicVar.g_SendByteArray[1] = haddr[0];
                PublicVar.g_SendByteArray[2] = haddr[1];
                PublicVar.g_SendByteArray[3] = haddr[2];
                PublicVar.g_SendByteArray[4] = haddr[3];
                //PublicVar.g_SendByteArray[5] = Convert.ToByte(this.TbSendSoftAdr.Text, 16);
                //byte[] saddr = BitConverter.GetBytes(Convert.ToInt32(this.TbSendSoftAdr.Text, 16));
                byte[] saddr1 = BitConverter.GetBytes(int.Parse(TbSendSoftAdr.Text));
                PublicVar.g_SendByteArray[5] = saddr1[1];
                PublicVar.g_SendByteArray[6] = saddr1[0];
                String timeString = Convert.ToString(this.TbSendTime.Text);
                String[] timeArray;
                //for(i = 0; i < timeString.Length; i++)
                //{
                //    Console.WriteLine(timeString[i]);
                //}
                for (i = 0; i < 23; i++)
                {
                    PublicVar.g_SendByteArray[i + 7] = Convert.ToByte(timeString[i]);
                }
                PublicVar.g_SendByteArray[i + 7] = Convert.ToByte('\0');
                //PublicVar.g_SendByteArray = System.Text.Encoding.Default.GetBytes(sendData);

                //发送全局变量g_SendByteArray中的数据,并返回结果
                Flag = sci.SCISendData(this.SCIPort, ref PublicVar.g_SendByteArray);

                if (Flag == true)//数据发送成功
                    this.TSSLState.Text += "数据发送成功!";
                else
                    this.TSSLState.Text += "数据发送失败!";
            }
        }

        private void BtnSCIClearSend_Click(object sender, EventArgs e)
        {
            this.TbShowHardAdr.Text = string.Empty;
            this.TbShowSoftAdr.Text = string.Empty;
            this.TbShowTime.Text = string.Empty;
            this.TSSLState.Text = "过程提示:清空接收文本框!";
        }

        private void btnClearRead_Click(object sender, EventArgs e)
        {
            this.TbSendTime.Text = "";
            this.TbSendHardAdr.Text = "";
            this.TbSendSoftAdr.Text = "";
            this.TSSLState.Text = "过程提示:清空发送文本框!";
        }

        /// <summary>
        /// 批量发送
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendAll_Click(object sender, EventArgs e)
        {
            //获取用户选择的文件夹和要发送的数量
            //string fileDir = this.textBoxFileDir.Text;
            string textNum;
            if ((textNum = this.textBoxLightNum.Text) != "")
            {
                int num = Convert.ToInt32(textNum);
            }
            else
            {
                this.textBoxCurrentState.Text = "";
                this.textBoxCurrentState.Text = "请输入灯的总数";
                return;
            }

            string videoPath;

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select a folder to add in Folder as Workspace panel";
            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                videoPath = fbd.DirectoryPath;    //要打开工程的路径
                this.textBoxFileDir.Text = videoPath;
                //Console.WriteLine(projectPath);
            }
            else
            {
                //可能没有选择文件夹
                this.textBoxCurrentState.Text = "";
                this.textBoxCurrentState.Text = "选择文件夹失败，重新选择";
                return;
            }

            DirectoryInfo root = new DirectoryInfo(videoPath);
            FileInfo[] files = root.GetFiles();
            foreach(var file in files)
            {
                if (file.Name != "All.txt")
                {
                    Console.WriteLine("当前发送"+file.Name);
                    Thread.Sleep(10000);
                    sendLightFile(file.FullName);
                }
                
            }
        }

        private void sendLightFile(string lightFilePath)
        {
            FileStream fs = new FileStream(lightFilePath, FileMode.Open, FileAccess.Read);
            long length = fs.Length;//数据的长度
            //length += 10;
            byte[] sendData = new byte[length + 11];
            int i;
            bool Flag = false;
            int groupCount = 0;//拆成了多少组

            UInt32 lengthNum = (UInt32)length;//放在帧头
            sendData[0] = (byte)'u';
            sendData[1] = (byte)'p';
            sendData[2] = (byte)'d';
            sendData[3] = (byte)'a';
            sendData[4] = (byte)'t';
            sendData[5] = (byte)'e';

            sendData[6] = (byte)(lengthNum >> 24);
            sendData[7] = (byte)(lengthNum >> 16);
            sendData[8] = (byte)(lengthNum >> 8);
            sendData[9] = (byte)(lengthNum);

            string lightFileName = Path.GetFileNameWithoutExtension(lightFilePath);

            int softAdr = Convert.ToInt16(lightFileName.Remove(0, 5));
            sendData[10] = (byte)softAdr;


            for (i = 0; i < length; i++)
            {
                sendData[i + 11] = (byte)fs.ReadByte();
            }
            fs.Close();

            //所有的数据在sendData中
            //拆分进行发送，因为一次性发送太多的情况会漏掉数据
            //在测试过程中，720个字节+11个帧头的情况是能正确发送的，所有这里以720为一段
            if (length <= 720)
            {
                ///////////////////////
                Flag = sci.SCISendData(this.SCIPort, ref sendData);
            }
            else
            {
                //加上11个帧头
                byte[] sendData1 = new byte[731];
                for (i = 0; i < 731; i++)
                {
                    sendData1[i] = sendData[i];
                }
                ///////////////////////
                Flag = sci.SCISendData(this.SCIPort, ref sendData1);
                groupCount += 1;
                length -= 720;

                while (length > 0)
                {
                    //sleep放在循环内部。如果长度小于720不需要sleep,如果大于2个720,也可以正常sleep
                    Thread.Sleep(10);
                    byte[] sendData2 = new byte[length <= 720 ? length : 720];
                    for (i = 0; i < sendData2.Length; i++)
                    {
                        sendData2[i] = sendData[i + groupCount * 720 + 11];
                    }
                    ///////////////////////
                    Flag = sci.SCISendData(this.SCIPort, ref sendData2);
                    groupCount += 1;
                    length -= 720;

                }
            }
            if(Flag == true)
            {
                this.textBoxCurrentState.Text += softAdr.ToString() + "号灯已发送完成\r\n" ;
            }
            else
            {
                this.textBoxCurrentState.Text += softAdr.ToString() + "号灯发送失败\r\n";
            }
        }

        private void btnClearReceive_Click(object sender, EventArgs e)
        {
            this.textBoxReceive.Text = "";
        }

        private void textBoxReceive_TextChanged(object sender, EventArgs e)
        {
            this.textBoxReceive.SelectionStart = this.textBoxReceive.TextLength;
            this.textBoxReceive.ScrollToCaret();
        }
    }//class
}//namespace
