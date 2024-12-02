
namespace AHL_GEC
{
    partial class FormSendAndReceive
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSendAndReceive));
            this.groupBoxReadData = new System.Windows.Forms.GroupBox();
            this.btnClearRead = new System.Windows.Forms.Button();
            this.btnReadData = new System.Windows.Forms.Button();
            this.TbShowSoftAdr = new System.Windows.Forms.TextBox();
            this.LabSoftAdr = new System.Windows.Forms.Label();
            this.TbShowHardAdr = new System.Windows.Forms.TextBox();
            this.LabHardAdr = new System.Windows.Forms.Label();
            this.TbShowTime = new System.Windows.Forms.TextBox();
            this.LabTime = new System.Windows.Forms.Label();
            this.groupBoxSendData = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxCurrentState = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxSoftAdr = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxLightFile = new System.Windows.Forms.TextBox();
            this.btnChooseSend = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxLightNum = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxFileDir = new System.Windows.Forms.TextBox();
            this.btnSendAll = new System.Windows.Forms.Button();
            this.SetSerialPort = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.CbSCIComNum2 = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.BtnSCISwitch = new System.Windows.Forms.Button();
            this.LblSCI = new System.Windows.Forms.Label();
            this.CbSCIBaud = new System.Windows.Forms.ComboBox();
            this.Baud = new System.Windows.Forms.Label();
            this.LbPortName = new System.Windows.Forms.Label();
            this.CbSCIComNum = new System.Windows.Forms.ComboBox();
            this.sSSerialPortInfo = new System.Windows.Forms.StatusStrip();
            this.TSSLState = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBoxWriteData = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.TbSendSoftAdr = new System.Windows.Forms.TextBox();
            this.TbSendHardAdr = new System.Windows.Forms.TextBox();
            this.TbSendTime = new System.Windows.Forms.TextBox();
            this.BtnSCISend = new System.Windows.Forms.Button();
            this.BtnSCIClearSend = new System.Windows.Forms.Button();
            this.groupBoxSetPlayTime = new System.Windows.Forms.GroupBox();
            this.btnSendStartTime = new System.Windows.Forms.Button();
            this.textBoxAbsTime = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.SCIPort = new System.IO.Ports.SerialPort(this.components);
            this.groupBoxReceive = new System.Windows.Forms.GroupBox();
            this.btnClearReceive = new System.Windows.Forms.Button();
            this.textBoxReceive = new System.Windows.Forms.TextBox();
            this.SCIPort2 = new System.IO.Ports.SerialPort(this.components);
            this.groupBoxReadData.SuspendLayout();
            this.groupBoxSendData.SuspendLayout();
            this.SetSerialPort.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.sSSerialPortInfo.SuspendLayout();
            this.groupBoxWriteData.SuspendLayout();
            this.groupBoxSetPlayTime.SuspendLayout();
            this.groupBoxReceive.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxReadData
            // 
            this.groupBoxReadData.Controls.Add(this.btnClearRead);
            this.groupBoxReadData.Controls.Add(this.btnReadData);
            this.groupBoxReadData.Controls.Add(this.TbShowSoftAdr);
            this.groupBoxReadData.Controls.Add(this.LabSoftAdr);
            this.groupBoxReadData.Controls.Add(this.TbShowHardAdr);
            this.groupBoxReadData.Controls.Add(this.LabHardAdr);
            this.groupBoxReadData.Controls.Add(this.TbShowTime);
            this.groupBoxReadData.Controls.Add(this.LabTime);
            this.groupBoxReadData.Location = new System.Drawing.Point(656, 118);
            this.groupBoxReadData.Name = "groupBoxReadData";
            this.groupBoxReadData.Size = new System.Drawing.Size(556, 193);
            this.groupBoxReadData.TabIndex = 41;
            this.groupBoxReadData.TabStop = false;
            this.groupBoxReadData.Text = "读回数据";
            // 
            // btnClearRead
            // 
            this.btnClearRead.Location = new System.Drawing.Point(42, 117);
            this.btnClearRead.Name = "btnClearRead";
            this.btnClearRead.Size = new System.Drawing.Size(89, 56);
            this.btnClearRead.TabIndex = 32;
            this.btnClearRead.Text = "清空发送框";
            this.btnClearRead.UseVisualStyleBackColor = true;
            this.btnClearRead.Click += new System.EventHandler(this.btnClearRead_Click);
            // 
            // btnReadData
            // 
            this.btnReadData.Location = new System.Drawing.Point(42, 40);
            this.btnReadData.Name = "btnReadData";
            this.btnReadData.Size = new System.Drawing.Size(89, 56);
            this.btnReadData.TabIndex = 31;
            this.btnReadData.Text = "读回数据";
            this.btnReadData.UseVisualStyleBackColor = true;
            this.btnReadData.Click += new System.EventHandler(this.btnReadData_Click);
            // 
            // TbShowSoftAdr
            // 
            this.TbShowSoftAdr.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TbShowSoftAdr.Location = new System.Drawing.Point(266, 130);
            this.TbShowSoftAdr.Multiline = true;
            this.TbShowSoftAdr.Name = "TbShowSoftAdr";
            this.TbShowSoftAdr.ReadOnly = true;
            this.TbShowSoftAdr.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TbShowSoftAdr.Size = new System.Drawing.Size(241, 26);
            this.TbShowSoftAdr.TabIndex = 30;
            // 
            // LabSoftAdr
            // 
            this.LabSoftAdr.AutoSize = true;
            this.LabSoftAdr.Location = new System.Drawing.Point(162, 135);
            this.LabSoftAdr.Name = "LabSoftAdr";
            this.LabSoftAdr.Size = new System.Drawing.Size(53, 12);
            this.LabSoftAdr.TabIndex = 29;
            this.LabSoftAdr.Text = "软件地址";
            // 
            // TbShowHardAdr
            // 
            this.TbShowHardAdr.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TbShowHardAdr.Location = new System.Drawing.Point(266, 86);
            this.TbShowHardAdr.Multiline = true;
            this.TbShowHardAdr.Name = "TbShowHardAdr";
            this.TbShowHardAdr.ReadOnly = true;
            this.TbShowHardAdr.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TbShowHardAdr.Size = new System.Drawing.Size(241, 26);
            this.TbShowHardAdr.TabIndex = 28;
            // 
            // LabHardAdr
            // 
            this.LabHardAdr.AutoSize = true;
            this.LabHardAdr.Location = new System.Drawing.Point(162, 91);
            this.LabHardAdr.Name = "LabHardAdr";
            this.LabHardAdr.Size = new System.Drawing.Size(53, 12);
            this.LabHardAdr.TabIndex = 27;
            this.LabHardAdr.Text = "硬件地址";
            // 
            // TbShowTime
            // 
            this.TbShowTime.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TbShowTime.Location = new System.Drawing.Point(266, 44);
            this.TbShowTime.Multiline = true;
            this.TbShowTime.Name = "TbShowTime";
            this.TbShowTime.ReadOnly = true;
            this.TbShowTime.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TbShowTime.Size = new System.Drawing.Size(241, 26);
            this.TbShowTime.TabIndex = 26;
            // 
            // LabTime
            // 
            this.LabTime.AutoSize = true;
            this.LabTime.Location = new System.Drawing.Point(176, 49);
            this.LabTime.Name = "LabTime";
            this.LabTime.Size = new System.Drawing.Size(29, 12);
            this.LabTime.TabIndex = 25;
            this.LabTime.Text = "时间";
            // 
            // groupBoxSendData
            // 
            this.groupBoxSendData.Controls.Add(this.label3);
            this.groupBoxSendData.Controls.Add(this.label2);
            this.groupBoxSendData.Controls.Add(this.textBoxCurrentState);
            this.groupBoxSendData.Controls.Add(this.label8);
            this.groupBoxSendData.Controls.Add(this.label7);
            this.groupBoxSendData.Controls.Add(this.textBoxSoftAdr);
            this.groupBoxSendData.Controls.Add(this.label6);
            this.groupBoxSendData.Controls.Add(this.textBoxLightFile);
            this.groupBoxSendData.Controls.Add(this.btnChooseSend);
            this.groupBoxSendData.Controls.Add(this.label10);
            this.groupBoxSendData.Controls.Add(this.textBoxLightNum);
            this.groupBoxSendData.Controls.Add(this.label1);
            this.groupBoxSendData.Controls.Add(this.textBoxFileDir);
            this.groupBoxSendData.Controls.Add(this.btnSendAll);
            this.groupBoxSendData.Location = new System.Drawing.Point(57, 118);
            this.groupBoxSendData.Name = "groupBoxSendData";
            this.groupBoxSendData.Size = new System.Drawing.Size(549, 455);
            this.groupBoxSendData.TabIndex = 42;
            this.groupBoxSendData.TabStop = false;
            this.groupBoxSendData.Text = "发送视频数据";
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Location = new System.Drawing.Point(20, 319);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(507, 2);
            this.label3.TabIndex = 52;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(20, 157);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(507, 2);
            this.label2.TabIndex = 51;
            // 
            // textBoxCurrentState
            // 
            this.textBoxCurrentState.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxCurrentState.Location = new System.Drawing.Point(201, 347);
            this.textBoxCurrentState.Multiline = true;
            this.textBoxCurrentState.Name = "textBoxCurrentState";
            this.textBoxCurrentState.ReadOnly = true;
            this.textBoxCurrentState.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxCurrentState.Size = new System.Drawing.Size(296, 72);
            this.textBoxCurrentState.TabIndex = 47;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("黑体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.Color.Blue;
            this.label8.Location = new System.Drawing.Point(54, 368);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 19);
            this.label8.TabIndex = 46;
            this.label8.Text = "当前状态";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(200, 85);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 14);
            this.label7.TabIndex = 45;
            this.label7.Text = "灯编号";
            // 
            // textBoxSoftAdr
            // 
            this.textBoxSoftAdr.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxSoftAdr.Location = new System.Drawing.Point(201, 112);
            this.textBoxSoftAdr.Multiline = true;
            this.textBoxSoftAdr.Name = "textBoxSoftAdr";
            this.textBoxSoftAdr.ReadOnly = true;
            this.textBoxSoftAdr.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxSoftAdr.Size = new System.Drawing.Size(296, 26);
            this.textBoxSoftAdr.TabIndex = 44;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(200, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 14);
            this.label6.TabIndex = 43;
            this.label6.Text = "选择的灯光文件";
            // 
            // textBoxLightFile
            // 
            this.textBoxLightFile.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxLightFile.Location = new System.Drawing.Point(201, 47);
            this.textBoxLightFile.Multiline = true;
            this.textBoxLightFile.Name = "textBoxLightFile";
            this.textBoxLightFile.ReadOnly = true;
            this.textBoxLightFile.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxLightFile.Size = new System.Drawing.Size(296, 26);
            this.textBoxLightFile.TabIndex = 42;
            // 
            // btnChooseSend
            // 
            this.btnChooseSend.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnChooseSend.Location = new System.Drawing.Point(27, 35);
            this.btnChooseSend.Name = "btnChooseSend";
            this.btnChooseSend.Size = new System.Drawing.Size(116, 47);
            this.btnChooseSend.TabIndex = 41;
            this.btnChooseSend.Text = "选择并发送";
            this.btnChooseSend.UseVisualStyleBackColor = true;
            this.btnChooseSend.Click += new System.EventHandler(this.btnChooseSend_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(200, 241);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(126, 14);
            this.label10.TabIndex = 54;
            this.label10.Text = "灯的总数（1-128）";
            // 
            // textBoxLightNum
            // 
            this.textBoxLightNum.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxLightNum.Location = new System.Drawing.Point(201, 271);
            this.textBoxLightNum.Multiline = true;
            this.textBoxLightNum.Name = "textBoxLightNum";
            this.textBoxLightNum.Size = new System.Drawing.Size(241, 28);
            this.textBoxLightNum.TabIndex = 53;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(200, 172);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 14);
            this.label1.TabIndex = 50;
            this.label1.Text = "选择的文件夹";
            // 
            // textBoxFileDir
            // 
            this.textBoxFileDir.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxFileDir.Location = new System.Drawing.Point(201, 201);
            this.textBoxFileDir.Multiline = true;
            this.textBoxFileDir.Name = "textBoxFileDir";
            this.textBoxFileDir.ReadOnly = true;
            this.textBoxFileDir.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxFileDir.Size = new System.Drawing.Size(296, 26);
            this.textBoxFileDir.TabIndex = 49;
            // 
            // btnSendAll
            // 
            this.btnSendAll.Enabled = false;
            this.btnSendAll.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSendAll.Location = new System.Drawing.Point(27, 208);
            this.btnSendAll.Name = "btnSendAll";
            this.btnSendAll.Size = new System.Drawing.Size(116, 47);
            this.btnSendAll.TabIndex = 48;
            this.btnSendAll.Text = "批量发送";
            this.btnSendAll.UseVisualStyleBackColor = true;
            this.btnSendAll.Click += new System.EventHandler(this.btnSendAll_Click);
            // 
            // SetSerialPort
            // 
            this.SetSerialPort.Controls.Add(this.label12);
            this.SetSerialPort.Controls.Add(this.CbSCIComNum2);
            this.SetSerialPort.Controls.Add(this.pictureBox1);
            this.SetSerialPort.Controls.Add(this.BtnSCISwitch);
            this.SetSerialPort.Controls.Add(this.LblSCI);
            this.SetSerialPort.Controls.Add(this.CbSCIBaud);
            this.SetSerialPort.Controls.Add(this.Baud);
            this.SetSerialPort.Controls.Add(this.LbPortName);
            this.SetSerialPort.Controls.Add(this.CbSCIComNum);
            this.SetSerialPort.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.SetSerialPort.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SetSerialPort.ForeColor = System.Drawing.Color.Blue;
            this.SetSerialPort.Location = new System.Drawing.Point(57, 18);
            this.SetSerialPort.Name = "SetSerialPort";
            this.SetSerialPort.Size = new System.Drawing.Size(1357, 74);
            this.SetSerialPort.TabIndex = 43;
            this.SetSerialPort.TabStop = false;
            this.SetSerialPort.Text = "串口设置";
            this.SetSerialPort.Enter += new System.EventHandler(this.SetSerialPort_Enter);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Location = new System.Drawing.Point(273, 22);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(70, 14);
            this.label12.TabIndex = 6;
            this.label12.Text = "串口2选择";
            // 
            // CbSCIComNum2
            // 
            this.CbSCIComNum2.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CbSCIComNum2.FormattingEnabled = true;
            this.CbSCIComNum2.Location = new System.Drawing.Point(349, 19);
            this.CbSCIComNum2.Name = "CbSCIComNum2";
            this.CbSCIComNum2.Size = new System.Drawing.Size(135, 22);
            this.CbSCIComNum2.TabIndex = 7;
            this.CbSCIComNum2.SelectedIndexChanged += new System.EventHandler(this.CbSCIComNum2_SelectedIndexChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(1028, 19);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(127, 38);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // BtnSCISwitch
            // 
            this.BtnSCISwitch.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnSCISwitch.ForeColor = System.Drawing.Color.Black;
            this.BtnSCISwitch.Location = new System.Drawing.Point(865, 22);
            this.BtnSCISwitch.Name = "BtnSCISwitch";
            this.BtnSCISwitch.Size = new System.Drawing.Size(102, 35);
            this.BtnSCISwitch.TabIndex = 4;
            this.BtnSCISwitch.Text = "打开串口";
            this.BtnSCISwitch.UseVisualStyleBackColor = true;
            this.BtnSCISwitch.Click += new System.EventHandler(this.BtnSCISwitch_Click);
            // 
            // LblSCI
            // 
            this.LblSCI.AutoSize = true;
            this.LblSCI.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LblSCI.ForeColor = System.Drawing.Color.Black;
            this.LblSCI.Location = new System.Drawing.Point(22, 54);
            this.LblSCI.Name = "LblSCI";
            this.LblSCI.Size = new System.Drawing.Size(77, 14);
            this.LblSCI.TabIndex = 3;
            this.LblSCI.Text = "LblSCI标签";
            // 
            // CbSCIBaud
            // 
            this.CbSCIBaud.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CbSCIBaud.FormattingEnabled = true;
            this.CbSCIBaud.Items.AddRange(new object[] {
            "115200",
            "38400",
            "19200",
            "9600"});
            this.CbSCIBaud.Location = new System.Drawing.Point(629, 22);
            this.CbSCIBaud.Name = "CbSCIBaud";
            this.CbSCIBaud.Size = new System.Drawing.Size(121, 22);
            this.CbSCIBaud.TabIndex = 3;
            this.CbSCIBaud.SelectedIndexChanged += new System.EventHandler(this.CbSCIBaud_SelectedIndexChanged);
            // 
            // Baud
            // 
            this.Baud.AutoSize = true;
            this.Baud.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Baud.ForeColor = System.Drawing.Color.Black;
            this.Baud.Location = new System.Drawing.Point(517, 25);
            this.Baud.Name = "Baud";
            this.Baud.Size = new System.Drawing.Size(77, 14);
            this.Baud.TabIndex = 2;
            this.Baud.Text = "波特率选择";
            // 
            // LbPortName
            // 
            this.LbPortName.AutoSize = true;
            this.LbPortName.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LbPortName.ForeColor = System.Drawing.Color.Black;
            this.LbPortName.Location = new System.Drawing.Point(17, 22);
            this.LbPortName.Name = "LbPortName";
            this.LbPortName.Size = new System.Drawing.Size(63, 14);
            this.LbPortName.TabIndex = 0;
            this.LbPortName.Text = "串口选择";
            // 
            // CbSCIComNum
            // 
            this.CbSCIComNum.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CbSCIComNum.FormattingEnabled = true;
            this.CbSCIComNum.Location = new System.Drawing.Point(86, 19);
            this.CbSCIComNum.Name = "CbSCIComNum";
            this.CbSCIComNum.Size = new System.Drawing.Size(135, 22);
            this.CbSCIComNum.TabIndex = 1;
            this.CbSCIComNum.SelectedIndexChanged += new System.EventHandler(this.CbSCIComNum_SelectedIndexChanged);
            // 
            // sSSerialPortInfo
            // 
            this.sSSerialPortInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSSLState,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.sSSerialPortInfo.Location = new System.Drawing.Point(0, 664);
            this.sSSerialPortInfo.Name = "sSSerialPortInfo";
            this.sSSerialPortInfo.Size = new System.Drawing.Size(1748, 22);
            this.sSSerialPortInfo.TabIndex = 44;
            this.sSSerialPortInfo.Text = "statusStrip1";
            // 
            // TSSLState
            // 
            this.TSSLState.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TSSLState.Name = "TSSLState";
            this.TSSLState.Size = new System.Drawing.Size(70, 17);
            this.TSSLState.Text = "没有操作!";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 17);
            // 
            // groupBoxWriteData
            // 
            this.groupBoxWriteData.Controls.Add(this.label4);
            this.groupBoxWriteData.Controls.Add(this.label5);
            this.groupBoxWriteData.Controls.Add(this.label9);
            this.groupBoxWriteData.Controls.Add(this.TbSendSoftAdr);
            this.groupBoxWriteData.Controls.Add(this.TbSendHardAdr);
            this.groupBoxWriteData.Controls.Add(this.TbSendTime);
            this.groupBoxWriteData.Controls.Add(this.BtnSCISend);
            this.groupBoxWriteData.Controls.Add(this.BtnSCIClearSend);
            this.groupBoxWriteData.Location = new System.Drawing.Point(656, 342);
            this.groupBoxWriteData.Name = "groupBoxWriteData";
            this.groupBoxWriteData.Size = new System.Drawing.Size(556, 231);
            this.groupBoxWriteData.TabIndex = 45;
            this.groupBoxWriteData.TabStop = false;
            this.groupBoxWriteData.Text = "修改数据";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(174, 149);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 34;
            this.label4.Text = "软件地址";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(174, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 33;
            this.label5.Text = "硬件地址";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(188, 63);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 32;
            this.label9.Text = "时间";
            // 
            // TbSendSoftAdr
            // 
            this.TbSendSoftAdr.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TbSendSoftAdr.Location = new System.Drawing.Point(273, 150);
            this.TbSendSoftAdr.Multiline = true;
            this.TbSendSoftAdr.Name = "TbSendSoftAdr";
            this.TbSendSoftAdr.Size = new System.Drawing.Size(241, 28);
            this.TbSendSoftAdr.TabIndex = 31;
            // 
            // TbSendHardAdr
            // 
            this.TbSendHardAdr.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TbSendHardAdr.Location = new System.Drawing.Point(273, 103);
            this.TbSendHardAdr.Multiline = true;
            this.TbSendHardAdr.Name = "TbSendHardAdr";
            this.TbSendHardAdr.Size = new System.Drawing.Size(241, 28);
            this.TbSendHardAdr.TabIndex = 30;
            // 
            // TbSendTime
            // 
            this.TbSendTime.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TbSendTime.Location = new System.Drawing.Point(273, 62);
            this.TbSendTime.Multiline = true;
            this.TbSendTime.Name = "TbSendTime";
            this.TbSendTime.Size = new System.Drawing.Size(241, 28);
            this.TbSendTime.TabIndex = 29;
            // 
            // BtnSCISend
            // 
            this.BtnSCISend.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnSCISend.ForeColor = System.Drawing.Color.Black;
            this.BtnSCISend.Location = new System.Drawing.Point(42, 52);
            this.BtnSCISend.Name = "BtnSCISend";
            this.BtnSCISend.Size = new System.Drawing.Size(97, 52);
            this.BtnSCISend.TabIndex = 28;
            this.BtnSCISend.Text = "发送数据";
            this.BtnSCISend.UseVisualStyleBackColor = true;
            this.BtnSCISend.Click += new System.EventHandler(this.BtnSCISend_Click);
            // 
            // BtnSCIClearSend
            // 
            this.BtnSCIClearSend.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnSCIClearSend.ForeColor = System.Drawing.Color.Black;
            this.BtnSCIClearSend.Location = new System.Drawing.Point(42, 128);
            this.BtnSCIClearSend.Name = "BtnSCIClearSend";
            this.BtnSCIClearSend.Size = new System.Drawing.Size(97, 50);
            this.BtnSCIClearSend.TabIndex = 27;
            this.BtnSCIClearSend.Text = "清空发送框";
            this.BtnSCIClearSend.UseVisualStyleBackColor = true;
            this.BtnSCIClearSend.Click += new System.EventHandler(this.BtnSCIClearSend_Click);
            // 
            // groupBoxSetPlayTime
            // 
            this.groupBoxSetPlayTime.Controls.Add(this.btnSendStartTime);
            this.groupBoxSetPlayTime.Controls.Add(this.textBoxAbsTime);
            this.groupBoxSetPlayTime.Controls.Add(this.label11);
            this.groupBoxSetPlayTime.Location = new System.Drawing.Point(57, 589);
            this.groupBoxSetPlayTime.Name = "groupBoxSetPlayTime";
            this.groupBoxSetPlayTime.Size = new System.Drawing.Size(1155, 57);
            this.groupBoxSetPlayTime.TabIndex = 46;
            this.groupBoxSetPlayTime.TabStop = false;
            // 
            // btnSendStartTime
            // 
            this.btnSendStartTime.Location = new System.Drawing.Point(706, 14);
            this.btnSendStartTime.Name = "btnSendStartTime";
            this.btnSendStartTime.Size = new System.Drawing.Size(108, 37);
            this.btnSendStartTime.TabIndex = 45;
            this.btnSendStartTime.Text = "发送播放时间";
            this.btnSendStartTime.UseVisualStyleBackColor = true;
            this.btnSendStartTime.Click += new System.EventHandler(this.btnSendStartTime_Click);
            // 
            // textBoxAbsTime
            // 
            this.textBoxAbsTime.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxAbsTime.Location = new System.Drawing.Point(400, 19);
            this.textBoxAbsTime.Multiline = true;
            this.textBoxAbsTime.Name = "textBoxAbsTime";
            this.textBoxAbsTime.Size = new System.Drawing.Size(252, 28);
            this.textBoxAbsTime.TabIndex = 44;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.ForeColor = System.Drawing.Color.Blue;
            this.label11.Location = new System.Drawing.Point(250, 22);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(104, 16);
            this.label11.TabIndex = 42;
            this.label11.Text = "设置播放时间";
            // 
            // SCIPort
            // 
            this.SCIPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SCIPort_DataReceived);
            // 
            // groupBoxReceive
            // 
            this.groupBoxReceive.Controls.Add(this.btnClearReceive);
            this.groupBoxReceive.Controls.Add(this.textBoxReceive);
            this.groupBoxReceive.Location = new System.Drawing.Point(1258, 118);
            this.groupBoxReceive.Name = "groupBoxReceive";
            this.groupBoxReceive.Size = new System.Drawing.Size(450, 455);
            this.groupBoxReceive.TabIndex = 47;
            this.groupBoxReceive.TabStop = false;
            this.groupBoxReceive.Text = "回发数据";
            // 
            // btnClearReceive
            // 
            this.btnClearReceive.Location = new System.Drawing.Point(355, 416);
            this.btnClearReceive.Name = "btnClearReceive";
            this.btnClearReceive.Size = new System.Drawing.Size(89, 33);
            this.btnClearReceive.TabIndex = 49;
            this.btnClearReceive.Text = "清空发送框";
            this.btnClearReceive.UseVisualStyleBackColor = true;
            this.btnClearReceive.Click += new System.EventHandler(this.btnClearReceive_Click);
            // 
            // textBoxReceive
            // 
            this.textBoxReceive.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxReceive.Location = new System.Drawing.Point(6, 20);
            this.textBoxReceive.Multiline = true;
            this.textBoxReceive.Name = "textBoxReceive";
            this.textBoxReceive.ReadOnly = true;
            this.textBoxReceive.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxReceive.Size = new System.Drawing.Size(444, 390);
            this.textBoxReceive.TabIndex = 48;
            this.textBoxReceive.TextChanged += new System.EventHandler(this.textBoxReceive_TextChanged);
            // 
            // SCIPort2
            // 
            this.SCIPort2.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SCIPort2_DataReceived);
            // 
            // FormSendAndReceive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1748, 686);
            this.Controls.Add(this.groupBoxReceive);
            this.Controls.Add(this.groupBoxSetPlayTime);
            this.Controls.Add(this.groupBoxWriteData);
            this.Controls.Add(this.sSSerialPortInfo);
            this.Controls.Add(this.SetSerialPort);
            this.Controls.Add(this.groupBoxSendData);
            this.Controls.Add(this.groupBoxReadData);
            this.Name = "FormSendAndReceive";
            this.Text = "选择文件并发送";
            this.Load += new System.EventHandler(this.FrmSCI_Load);
            this.groupBoxReadData.ResumeLayout(false);
            this.groupBoxReadData.PerformLayout();
            this.groupBoxSendData.ResumeLayout(false);
            this.groupBoxSendData.PerformLayout();
            this.SetSerialPort.ResumeLayout(false);
            this.SetSerialPort.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.sSSerialPortInfo.ResumeLayout(false);
            this.sSSerialPortInfo.PerformLayout();
            this.groupBoxWriteData.ResumeLayout(false);
            this.groupBoxWriteData.PerformLayout();
            this.groupBoxSetPlayTime.ResumeLayout(false);
            this.groupBoxSetPlayTime.PerformLayout();
            this.groupBoxReceive.ResumeLayout(false);
            this.groupBoxReceive.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxReadData;
        private System.Windows.Forms.GroupBox groupBoxSendData;
        private System.Windows.Forms.TextBox textBoxCurrentState;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxSoftAdr;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxLightFile;
        private System.Windows.Forms.Button btnChooseSend;
        private System.Windows.Forms.GroupBox SetSerialPort;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button BtnSCISwitch;
        private System.Windows.Forms.Label LblSCI;
        private System.Windows.Forms.ComboBox CbSCIBaud;
        private System.Windows.Forms.Label Baud;
        private System.Windows.Forms.Label LbPortName;
        private System.Windows.Forms.ComboBox CbSCIComNum;
        private System.Windows.Forms.StatusStrip sSSerialPortInfo;
        private System.Windows.Forms.ToolStripStatusLabel TSSLState;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxFileDir;
        private System.Windows.Forms.Button btnSendAll;
        private System.Windows.Forms.Button btnReadData;
        private System.Windows.Forms.TextBox TbShowSoftAdr;
        private System.Windows.Forms.Label LabSoftAdr;
        private System.Windows.Forms.TextBox TbShowHardAdr;
        private System.Windows.Forms.Label LabHardAdr;
        private System.Windows.Forms.TextBox TbShowTime;
        private System.Windows.Forms.Label LabTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBoxWriteData;
        private System.Windows.Forms.Button btnClearRead;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox TbSendSoftAdr;
        private System.Windows.Forms.TextBox TbSendHardAdr;
        private System.Windows.Forms.TextBox TbSendTime;
        private System.Windows.Forms.Button BtnSCISend;
        private System.Windows.Forms.Button BtnSCIClearSend;
        private System.Windows.Forms.GroupBox groupBoxSetPlayTime;
        private System.Windows.Forms.Button btnSendStartTime;
        private System.Windows.Forms.TextBox textBoxAbsTime;
        private System.Windows.Forms.Label label11;
        private System.IO.Ports.SerialPort SCIPort;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxLightNum;
        private System.Windows.Forms.GroupBox groupBoxReceive;
        private System.Windows.Forms.TextBox textBoxReceive;
        /////////
        private System.IO.Ports.SerialPort SCIPort2;
        private System.Windows.Forms.Button btnClearReceive;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox CbSCIComNum2;
    }
}