
namespace SerialPort
{
    partial class FrmSCI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSCI));
            this.LbPortName = new System.Windows.Forms.Label();
            this.CbSCIComNum = new System.Windows.Forms.ComboBox();
            this.SetSerialPort = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.BtnSCISwitch = new System.Windows.Forms.Button();
            this.LblSCI = new System.Windows.Forms.Label();
            this.CbSCIBaud = new System.Windows.Forms.ComboBox();
            this.Baud = new System.Windows.Forms.Label();
            this.SerialPortSend = new System.Windows.Forms.GroupBox();
            this.LabNote = new System.Windows.Forms.Label();
            this.BtnSCISend = new System.Windows.Forms.Button();
            this.CbSCISendType = new System.Windows.Forms.ComboBox();
            this.BtnSCIClearSend = new System.Windows.Forms.Button();
            this.TbSCISend = new System.Windows.Forms.TextBox();
            this.lbType = new System.Windows.Forms.Label();
            this.SerialPortReceive = new System.Windows.Forms.GroupBox();
            this.TbShowHex_checkBox = new System.Windows.Forms.CheckBox();
            this.TbShowDec_checkBox = new System.Windows.Forms.CheckBox();
            this.TbShowString = new System.Windows.Forms.TextBox();
            this.lbHex = new System.Windows.Forms.Label();
            this.BtnSCIClearRec = new System.Windows.Forms.Button();
            this.TbShowHex = new System.Windows.Forms.TextBox();
            this.lbDec = new System.Windows.Forms.Label();
            this.TbShowDec = new System.Windows.Forms.TextBox();
            this.lbChac = new System.Windows.Forms.Label();
            this.BtnState = new System.Windows.Forms.Button();
            this.sSSerialPortInfo = new System.Windows.Forms.StatusStrip();
            this.TSSLState = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.SCIPort = new System.IO.Ports.SerialPort(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.led1 = new LED.LED();
            this.label3 = new System.Windows.Forms.Label();
            this.ucledNum1 = new HZH_Controls.Controls.UCLEDNum();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.ucThermometer1 = new HZH_Controls.Controls.UCThermometer();
            this.label1 = new System.Windows.Forms.Label();
            this.SetSerialPort.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SerialPortSend.SuspendLayout();
            this.SerialPortReceive.SuspendLayout();
            this.sSSerialPortInfo.SuspendLayout();
            this.SuspendLayout();
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
            // SetSerialPort
            // 
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
            this.SetSerialPort.Location = new System.Drawing.Point(24, 12);
            this.SetSerialPort.Name = "SetSerialPort";
            this.SetSerialPort.Size = new System.Drawing.Size(815, 74);
            this.SetSerialPort.TabIndex = 2;
            this.SetSerialPort.TabStop = false;
            this.SetSerialPort.Text = "串口设置";
            this.SetSerialPort.Enter += new System.EventHandler(this.SetSerialPort_Enter);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(671, 16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(127, 38);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // BtnSCISwitch
            // 
            this.BtnSCISwitch.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnSCISwitch.ForeColor = System.Drawing.Color.Black;
            this.BtnSCISwitch.Location = new System.Drawing.Point(544, 19);
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
            this.CbSCIBaud.Location = new System.Drawing.Point(362, 19);
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
            this.Baud.Location = new System.Drawing.Point(250, 22);
            this.Baud.Name = "Baud";
            this.Baud.Size = new System.Drawing.Size(77, 14);
            this.Baud.TabIndex = 2;
            this.Baud.Text = "波特率选择";
            // 
            // SerialPortSend
            // 
            this.SerialPortSend.Controls.Add(this.LabNote);
            this.SerialPortSend.Controls.Add(this.BtnSCISend);
            this.SerialPortSend.Controls.Add(this.CbSCISendType);
            this.SerialPortSend.Controls.Add(this.BtnSCIClearSend);
            this.SerialPortSend.Controls.Add(this.TbSCISend);
            this.SerialPortSend.Controls.Add(this.lbType);
            this.SerialPortSend.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SerialPortSend.ForeColor = System.Drawing.Color.Blue;
            this.SerialPortSend.Location = new System.Drawing.Point(24, 92);
            this.SerialPortSend.Name = "SerialPortSend";
            this.SerialPortSend.Size = new System.Drawing.Size(815, 110);
            this.SerialPortSend.TabIndex = 3;
            this.SerialPortSend.TabStop = false;
            this.SerialPortSend.Text = "发送数据设置";
            this.SerialPortSend.Enter += new System.EventHandler(this.SerialPortSend_Enter);
            // 
            // LabNote
            // 
            this.LabNote.AutoSize = true;
            this.LabNote.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LabNote.ForeColor = System.Drawing.Color.DarkRed;
            this.LabNote.Location = new System.Drawing.Point(238, 25);
            this.LabNote.Name = "LabNote";
            this.LabNote.Size = new System.Drawing.Size(84, 14);
            this.LabNote.TabIndex = 7;
            this.LabNote.Text = "LabNote标签";
            // 
            // BtnSCISend
            // 
            this.BtnSCISend.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnSCISend.ForeColor = System.Drawing.Color.Black;
            this.BtnSCISend.Location = new System.Drawing.Point(42, 53);
            this.BtnSCISend.Name = "BtnSCISend";
            this.BtnSCISend.Size = new System.Drawing.Size(179, 36);
            this.BtnSCISend.TabIndex = 5;
            this.BtnSCISend.Text = "发送数据";
            this.BtnSCISend.UseVisualStyleBackColor = true;
            this.BtnSCISend.Click += new System.EventHandler(this.BtnSCISend_Click);
            // 
            // CbSCISendType
            // 
            this.CbSCISendType.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CbSCISendType.FormattingEnabled = true;
            this.CbSCISendType.Items.AddRange(new object[] {
            "字符串",
            "十进制",
            "十六进制"});
            this.CbSCISendType.Location = new System.Drawing.Point(139, 25);
            this.CbSCISendType.Name = "CbSCISendType";
            this.CbSCISendType.Size = new System.Drawing.Size(82, 22);
            this.CbSCISendType.TabIndex = 1;
            this.CbSCISendType.SelectedIndexChanged += new System.EventHandler(this.CbSCISendType_SelectedIndexChanged);
            // 
            // BtnSCIClearSend
            // 
            this.BtnSCIClearSend.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnSCIClearSend.ForeColor = System.Drawing.Color.Black;
            this.BtnSCIClearSend.Location = new System.Drawing.Point(704, 43);
            this.BtnSCIClearSend.Name = "BtnSCIClearSend";
            this.BtnSCIClearSend.Size = new System.Drawing.Size(105, 50);
            this.BtnSCIClearSend.TabIndex = 4;
            this.BtnSCIClearSend.Text = "清空发送框";
            this.BtnSCIClearSend.UseVisualStyleBackColor = true;
            this.BtnSCIClearSend.Click += new System.EventHandler(this.BtnSCIClearSend_Click);
            // 
            // TbSCISend
            // 
            this.TbSCISend.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TbSCISend.Location = new System.Drawing.Point(240, 43);
            this.TbSCISend.Multiline = true;
            this.TbSCISend.Name = "TbSCISend";
            this.TbSCISend.Size = new System.Drawing.Size(458, 50);
            this.TbSCISend.TabIndex = 6;
            this.TbSCISend.TextChanged += new System.EventHandler(this.TbSCISend_TextChanged);
            this.TbSCISend.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TbSCISend_KeyPress);
            // 
            // lbType
            // 
            this.lbType.AutoSize = true;
            this.lbType.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbType.ForeColor = System.Drawing.Color.Black;
            this.lbType.Location = new System.Drawing.Point(34, 28);
            this.lbType.Name = "lbType";
            this.lbType.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbType.Size = new System.Drawing.Size(91, 14);
            this.lbType.TabIndex = 0;
            this.lbType.Text = "选择发送方式";
            this.lbType.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // SerialPortReceive
            // 
            this.SerialPortReceive.Controls.Add(this.TbShowHex_checkBox);
            this.SerialPortReceive.Controls.Add(this.TbShowDec_checkBox);
            this.SerialPortReceive.Controls.Add(this.TbShowString);
            this.SerialPortReceive.Controls.Add(this.lbHex);
            this.SerialPortReceive.Controls.Add(this.BtnSCIClearRec);
            this.SerialPortReceive.Controls.Add(this.TbShowHex);
            this.SerialPortReceive.Controls.Add(this.lbDec);
            this.SerialPortReceive.Controls.Add(this.TbShowDec);
            this.SerialPortReceive.Controls.Add(this.lbChac);
            this.SerialPortReceive.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SerialPortReceive.ForeColor = System.Drawing.Color.Blue;
            this.SerialPortReceive.Location = new System.Drawing.Point(24, 208);
            this.SerialPortReceive.Name = "SerialPortReceive";
            this.SerialPortReceive.Size = new System.Drawing.Size(815, 320);
            this.SerialPortReceive.TabIndex = 7;
            this.SerialPortReceive.TabStop = false;
            this.SerialPortReceive.Text = "接收数据设置";
            // 
            // TbShowHex_checkBox
            // 
            this.TbShowHex_checkBox.AutoSize = true;
            this.TbShowHex_checkBox.Location = new System.Drawing.Point(569, 32);
            this.TbShowHex_checkBox.Name = "TbShowHex_checkBox";
            this.TbShowHex_checkBox.Size = new System.Drawing.Size(15, 14);
            this.TbShowHex_checkBox.TabIndex = 16;
            this.TbShowHex_checkBox.UseVisualStyleBackColor = true;
            this.TbShowHex_checkBox.CheckedChanged += new System.EventHandler(this.TbShowHex_checkBox_CheckedChanged);
            // 
            // TbShowDec_checkBox
            // 
            this.TbShowDec_checkBox.AutoSize = true;
            this.TbShowDec_checkBox.Location = new System.Drawing.Point(297, 32);
            this.TbShowDec_checkBox.Name = "TbShowDec_checkBox";
            this.TbShowDec_checkBox.Size = new System.Drawing.Size(15, 14);
            this.TbShowDec_checkBox.TabIndex = 18;
            this.TbShowDec_checkBox.UseVisualStyleBackColor = true;
            this.TbShowDec_checkBox.CheckedChanged += new System.EventHandler(this.TbShowDec_checkBox_CheckedChanged);
            // 
            // TbShowString
            // 
            this.TbShowString.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TbShowString.Location = new System.Drawing.Point(20, 60);
            this.TbShowString.Multiline = true;
            this.TbShowString.Name = "TbShowString";
            this.TbShowString.ReadOnly = true;
            this.TbShowString.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TbShowString.Size = new System.Drawing.Size(778, 238);
            this.TbShowString.TabIndex = 16;
            this.TbShowString.TextChanged += new System.EventHandler(this.TbShowString_TextChanged);
            // 
            // lbHex
            // 
            this.lbHex.AutoSize = true;
            this.lbHex.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbHex.ForeColor = System.Drawing.Color.Black;
            this.lbHex.Location = new System.Drawing.Point(590, 32);
            this.lbHex.Name = "lbHex";
            this.lbHex.Size = new System.Drawing.Size(91, 14);
            this.lbHex.TabIndex = 14;
            this.lbHex.Text = "十六进制形式";
            // 
            // BtnSCIClearRec
            // 
            this.BtnSCIClearRec.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnSCIClearRec.ForeColor = System.Drawing.Color.Black;
            this.BtnSCIClearRec.Location = new System.Drawing.Point(119, 0);
            this.BtnSCIClearRec.Name = "BtnSCIClearRec";
            this.BtnSCIClearRec.Size = new System.Drawing.Size(143, 23);
            this.BtnSCIClearRec.TabIndex = 8;
            this.BtnSCIClearRec.Text = "清空接收框";
            this.BtnSCIClearRec.UseVisualStyleBackColor = true;
            this.BtnSCIClearRec.Click += new System.EventHandler(this.btnClearRec_Click);
            // 
            // TbShowHex
            // 
            this.TbShowHex.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TbShowHex.Location = new System.Drawing.Point(548, 60);
            this.TbShowHex.Multiline = true;
            this.TbShowHex.Name = "TbShowHex";
            this.TbShowHex.ReadOnly = true;
            this.TbShowHex.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TbShowHex.Size = new System.Drawing.Size(250, 238);
            this.TbShowHex.TabIndex = 13;
            // 
            // lbDec
            // 
            this.lbDec.AutoSize = true;
            this.lbDec.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbDec.ForeColor = System.Drawing.Color.Black;
            this.lbDec.Location = new System.Drawing.Point(309, 32);
            this.lbDec.Name = "lbDec";
            this.lbDec.Size = new System.Drawing.Size(77, 14);
            this.lbDec.TabIndex = 10;
            this.lbDec.Text = "十进制形式";
            // 
            // TbShowDec
            // 
            this.TbShowDec.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TbShowDec.Location = new System.Drawing.Point(286, 60);
            this.TbShowDec.Multiline = true;
            this.TbShowDec.Name = "TbShowDec";
            this.TbShowDec.ReadOnly = true;
            this.TbShowDec.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TbShowDec.Size = new System.Drawing.Size(250, 238);
            this.TbShowDec.TabIndex = 9;
            this.TbShowDec.TextChanged += new System.EventHandler(this.TbShowDec_TextChanged);
            // 
            // lbChac
            // 
            this.lbChac.AutoSize = true;
            this.lbChac.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbChac.ForeColor = System.Drawing.Color.Black;
            this.lbChac.Location = new System.Drawing.Point(6, 32);
            this.lbChac.Name = "lbChac";
            this.lbChac.Size = new System.Drawing.Size(63, 14);
            this.lbChac.TabIndex = 7;
            this.lbChac.Text = "字符形式";
            this.lbChac.Click += new System.EventHandler(this.lbChac_Click);
            // 
            // BtnState
            // 
            this.BtnState.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnState.ForeColor = System.Drawing.Color.Black;
            this.BtnState.Location = new System.Drawing.Point(709, 534);
            this.BtnState.Name = "BtnState";
            this.BtnState.Size = new System.Drawing.Size(135, 23);
            this.BtnState.TabIndex = 15;
            this.BtnState.Text = "隐藏状态条";
            this.BtnState.UseVisualStyleBackColor = true;
            this.BtnState.Click += new System.EventHandler(this.BtnState_Click);
            // 
            // sSSerialPortInfo
            // 
            this.sSSerialPortInfo.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.sSSerialPortInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSSLState,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.sSSerialPortInfo.Location = new System.Drawing.Point(0, 585);
            this.sSSerialPortInfo.Name = "sSSerialPortInfo";
            this.sSSerialPortInfo.Size = new System.Drawing.Size(1034, 22);
            this.sSSerialPortInfo.TabIndex = 8;
            this.sSSerialPortInfo.Text = "statusStrip1";
            this.sSSerialPortInfo.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.sSSerialPortInfo_ItemClicked);
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
            // SCIPort
            // 
            this.SCIPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SCIPort_DataReceived);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(890, 514);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(107, 44);
            this.button2.TabIndex = 16;
            this.button2.Text = "语音播报";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // led1
            // 
            this.led1.BorderColor = System.Drawing.Color.LightGray;
            this.led1.BorderWidth = 4;
            this.led1.CenterColor = System.Drawing.Color.White;
            this.led1.Distance = 8;
            this.led1.GridentColor = System.Drawing.Color.Gray;
            this.led1.Location = new System.Drawing.Point(908, 394);
            this.led1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.led1.Name = "led1";
            this.led1.Size = new System.Drawing.Size(89, 91);
            this.led1.TabIndex = 19;
            this.led1.Load += new System.EventHandler(this.led1_Load);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F);
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(845, 359);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 16);
            this.label3.TabIndex = 20;
            this.label3.Text = "MCU指示灯";
            // 
            // ucledNum1
            // 
            this.ucledNum1.LineWidth = 8;
            this.ucledNum1.Location = new System.Drawing.Point(858, 410);
            this.ucledNum1.Name = "ucledNum1";
            this.ucledNum1.Size = new System.Drawing.Size(35, 60);
            this.ucledNum1.TabIndex = 21;
            this.ucledNum1.Value = '0';
            this.ucledNum1.ValueChar = HZH_Controls.Controls.LEDNumChar.CHAR_0;
            this.ucledNum1.Load += new System.EventHandler(this.ucledNum1_Load);
            // 
            // timer1
            // 
            this.timer1.Interval = 960;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ucThermometer1
            // 
            this.ucThermometer1.BackColor = System.Drawing.Color.SkyBlue;
            this.ucThermometer1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ucThermometer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ucThermometer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.ucThermometer1.Font = new System.Drawing.Font("宋体", 9F);
            this.ucThermometer1.GlassTubeColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(211)))), ((int)(((byte)(211)))));
            this.ucThermometer1.LeftTemperatureUnit = HZH_Controls.Controls.TemperatureUnit.C;
            this.ucThermometer1.Location = new System.Drawing.Point(872, 12);
            this.ucThermometer1.MaxValue = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.ucThermometer1.MercuryColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.ucThermometer1.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ucThermometer1.Name = "ucThermometer1";
            this.ucThermometer1.RightTemperatureUnit = HZH_Controls.Controls.TemperatureUnit.C;
            this.ucThermometer1.Size = new System.Drawing.Size(82, 275);
            this.ucThermometer1.SplitCount = 5;
            this.ucThermometer1.TabIndex = 22;
            this.ucThermometer1.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ucThermometer1.Load += new System.EventHandler(this.ucThermometer1_Load);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F);
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(886, 290);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 16);
            this.label1.TabIndex = 24;
            this.label1.Text = "芯片温度";
            this.label1.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // FrmSCI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1034, 607);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ucThermometer1);
            this.Controls.Add(this.ucledNum1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.led1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.sSSerialPortInfo);
            this.Controls.Add(this.BtnState);
            this.Controls.Add(this.SerialPortReceive);
            this.Controls.Add(this.SerialPortSend);
            this.Controls.Add(this.SetSerialPort);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmSCI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "芯片基本功能串口测试工程（V3.0)    （For AHL-CH573)  202408   苏大嵌入式出品";
            this.Load += new System.EventHandler(this.FrmSCI_Load);
            this.SetSerialPort.ResumeLayout(false);
            this.SetSerialPort.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.SerialPortSend.ResumeLayout(false);
            this.SerialPortSend.PerformLayout();
            this.SerialPortReceive.ResumeLayout(false);
            this.SerialPortReceive.PerformLayout();
            this.sSSerialPortInfo.ResumeLayout(false);
            this.sSSerialPortInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LbPortName;
        private System.Windows.Forms.ComboBox CbSCIComNum;
        private System.Windows.Forms.GroupBox SetSerialPort;
        private System.Windows.Forms.Label Baud;
        private System.Windows.Forms.ComboBox CbSCIBaud;
        private System.Windows.Forms.Label LblSCI;
        private System.Windows.Forms.Button BtnSCISwitch;
        private System.Windows.Forms.GroupBox SerialPortSend;
        private System.Windows.Forms.TextBox TbSCISend;
        private System.Windows.Forms.GroupBox SerialPortReceive;
        private System.Windows.Forms.StatusStrip sSSerialPortInfo;
        private System.Windows.Forms.Button BtnSCISend;
        private System.Windows.Forms.ComboBox CbSCISendType;
        private System.Windows.Forms.Button BtnSCIClearSend;
        private System.Windows.Forms.Label lbType;
        private System.Windows.Forms.Label lbChac;
        private System.Windows.Forms.Button BtnSCIClearRec;
        private System.IO.Ports.SerialPort SCIPort;
        private System.Windows.Forms.ToolStripStatusLabel TSSLState;
        private System.Windows.Forms.TextBox TbShowDec;
        private System.Windows.Forms.Label lbDec;
        private System.Windows.Forms.TextBox TbShowHex;
        private System.Windows.Forms.Label lbHex;
        private System.Windows.Forms.Label LabNote;
        private System.Windows.Forms.Button BtnState;
        private System.Windows.Forms.TextBox TbShowString;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.CheckBox TbShowHex_checkBox;
        private System.Windows.Forms.CheckBox TbShowDec_checkBox;
        private System.Windows.Forms.Button button2;
        private LED.LED led1;
        private System.Windows.Forms.Label label3;
        private HZH_Controls.Controls.UCLEDNum ucledNum1;
        private System.Windows.Forms.Timer timer1;
        private HZH_Controls.Controls.UCThermometer ucThermometer1;
        private System.Windows.Forms.Label label1;
    }
}

