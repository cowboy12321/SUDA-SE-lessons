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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.lblM = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblFlag = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SetSerialPort.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SerialPortSend.SuspendLayout();
            this.SerialPortReceive.SuspendLayout();
            this.sSSerialPortInfo.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LbPortName
            // 
            this.LbPortName.AutoSize = true;
            this.LbPortName.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LbPortName.ForeColor = System.Drawing.Color.Black;
            this.LbPortName.Location = new System.Drawing.Point(23, 28);
            this.LbPortName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LbPortName.Name = "LbPortName";
            this.LbPortName.Size = new System.Drawing.Size(80, 18);
            this.LbPortName.TabIndex = 0;
            this.LbPortName.Text = "串口选择";
            // 
            // CbSCIComNum
            // 
            this.CbSCIComNum.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CbSCIComNum.FormattingEnabled = true;
            this.CbSCIComNum.Location = new System.Drawing.Point(115, 24);
            this.CbSCIComNum.Margin = new System.Windows.Forms.Padding(4);
            this.CbSCIComNum.Name = "CbSCIComNum";
            this.CbSCIComNum.Size = new System.Drawing.Size(179, 25);
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
            this.SetSerialPort.Location = new System.Drawing.Point(32, 15);
            this.SetSerialPort.Margin = new System.Windows.Forms.Padding(4);
            this.SetSerialPort.Name = "SetSerialPort";
            this.SetSerialPort.Padding = new System.Windows.Forms.Padding(4);
            this.SetSerialPort.Size = new System.Drawing.Size(1087, 92);
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
            this.pictureBox1.Location = new System.Drawing.Point(895, 20);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(169, 48);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // BtnSCISwitch
            // 
            this.BtnSCISwitch.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnSCISwitch.ForeColor = System.Drawing.Color.Black;
            this.BtnSCISwitch.Location = new System.Drawing.Point(725, 24);
            this.BtnSCISwitch.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSCISwitch.Name = "BtnSCISwitch";
            this.BtnSCISwitch.Size = new System.Drawing.Size(136, 44);
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
            this.LblSCI.Location = new System.Drawing.Point(29, 68);
            this.LblSCI.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblSCI.Name = "LblSCI";
            this.LblSCI.Size = new System.Drawing.Size(98, 18);
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
            this.CbSCIBaud.Location = new System.Drawing.Point(483, 24);
            this.CbSCIBaud.Margin = new System.Windows.Forms.Padding(4);
            this.CbSCIBaud.Name = "CbSCIBaud";
            this.CbSCIBaud.Size = new System.Drawing.Size(160, 25);
            this.CbSCIBaud.TabIndex = 3;
            this.CbSCIBaud.SelectedIndexChanged += new System.EventHandler(this.CbSCIBaud_SelectedIndexChanged);
            // 
            // Baud
            // 
            this.Baud.AutoSize = true;
            this.Baud.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Baud.ForeColor = System.Drawing.Color.Black;
            this.Baud.Location = new System.Drawing.Point(333, 28);
            this.Baud.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Baud.Name = "Baud";
            this.Baud.Size = new System.Drawing.Size(98, 18);
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
            this.SerialPortSend.Location = new System.Drawing.Point(32, 115);
            this.SerialPortSend.Margin = new System.Windows.Forms.Padding(4);
            this.SerialPortSend.Name = "SerialPortSend";
            this.SerialPortSend.Padding = new System.Windows.Forms.Padding(4);
            this.SerialPortSend.Size = new System.Drawing.Size(1087, 138);
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
            this.LabNote.Location = new System.Drawing.Point(317, 31);
            this.LabNote.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LabNote.Name = "LabNote";
            this.LabNote.Size = new System.Drawing.Size(107, 18);
            this.LabNote.TabIndex = 7;
            this.LabNote.Text = "LabNote标签";
            // 
            // BtnSCISend
            // 
            this.BtnSCISend.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnSCISend.ForeColor = System.Drawing.Color.Black;
            this.BtnSCISend.Location = new System.Drawing.Point(56, 66);
            this.BtnSCISend.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSCISend.Name = "BtnSCISend";
            this.BtnSCISend.Size = new System.Drawing.Size(239, 45);
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
            this.CbSCISendType.Location = new System.Drawing.Point(185, 31);
            this.CbSCISendType.Margin = new System.Windows.Forms.Padding(4);
            this.CbSCISendType.Name = "CbSCISendType";
            this.CbSCISendType.Size = new System.Drawing.Size(108, 25);
            this.CbSCISendType.TabIndex = 1;
            this.CbSCISendType.SelectedIndexChanged += new System.EventHandler(this.CbSCISendType_SelectedIndexChanged);
            // 
            // BtnSCIClearSend
            // 
            this.BtnSCIClearSend.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnSCIClearSend.ForeColor = System.Drawing.Color.Black;
            this.BtnSCIClearSend.Location = new System.Drawing.Point(939, 54);
            this.BtnSCIClearSend.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSCIClearSend.Name = "BtnSCIClearSend";
            this.BtnSCIClearSend.Size = new System.Drawing.Size(140, 62);
            this.BtnSCIClearSend.TabIndex = 4;
            this.BtnSCIClearSend.Text = "清空发送框";
            this.BtnSCIClearSend.UseVisualStyleBackColor = true;
            this.BtnSCIClearSend.Click += new System.EventHandler(this.BtnSCIClearSend_Click);
            // 
            // TbSCISend
            // 
            this.TbSCISend.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TbSCISend.Location = new System.Drawing.Point(320, 54);
            this.TbSCISend.Margin = new System.Windows.Forms.Padding(4);
            this.TbSCISend.Multiline = true;
            this.TbSCISend.Name = "TbSCISend";
            this.TbSCISend.Size = new System.Drawing.Size(609, 62);
            this.TbSCISend.TabIndex = 6;
            this.TbSCISend.TextChanged += new System.EventHandler(this.TbSCISend_TextChanged);
            this.TbSCISend.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TbSCISend_KeyPress);
            // 
            // lbType
            // 
            this.lbType.AutoSize = true;
            this.lbType.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbType.ForeColor = System.Drawing.Color.Black;
            this.lbType.Location = new System.Drawing.Point(45, 35);
            this.lbType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbType.Name = "lbType";
            this.lbType.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbType.Size = new System.Drawing.Size(116, 18);
            this.lbType.TabIndex = 0;
            this.lbType.Text = "选择发送方式";
            this.lbType.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // SerialPortReceive
            // 
            this.SerialPortReceive.Controls.Add(this.label2);
            this.SerialPortReceive.Controls.Add(this.label1);
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
            this.SerialPortReceive.Location = new System.Drawing.Point(32, 260);
            this.SerialPortReceive.Margin = new System.Windows.Forms.Padding(4);
            this.SerialPortReceive.Name = "SerialPortReceive";
            this.SerialPortReceive.Padding = new System.Windows.Forms.Padding(4);
            this.SerialPortReceive.Size = new System.Drawing.Size(1087, 400);
            this.SerialPortReceive.TabIndex = 7;
            this.SerialPortReceive.TabStop = false;
            this.SerialPortReceive.Text = "接收数据设置";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(939, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 20);
            this.label2.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(913, 24);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 20);
            this.label1.TabIndex = 19;
            // 
            // TbShowHex_checkBox
            // 
            this.TbShowHex_checkBox.AutoSize = true;
            this.TbShowHex_checkBox.Location = new System.Drawing.Point(759, 40);
            this.TbShowHex_checkBox.Margin = new System.Windows.Forms.Padding(4);
            this.TbShowHex_checkBox.Name = "TbShowHex_checkBox";
            this.TbShowHex_checkBox.Size = new System.Drawing.Size(18, 17);
            this.TbShowHex_checkBox.TabIndex = 16;
            this.TbShowHex_checkBox.UseVisualStyleBackColor = true;
            this.TbShowHex_checkBox.CheckedChanged += new System.EventHandler(this.TbShowHex_checkBox_CheckedChanged);
            // 
            // TbShowDec_checkBox
            // 
            this.TbShowDec_checkBox.AutoSize = true;
            this.TbShowDec_checkBox.Location = new System.Drawing.Point(396, 40);
            this.TbShowDec_checkBox.Margin = new System.Windows.Forms.Padding(4);
            this.TbShowDec_checkBox.Name = "TbShowDec_checkBox";
            this.TbShowDec_checkBox.Size = new System.Drawing.Size(18, 17);
            this.TbShowDec_checkBox.TabIndex = 18;
            this.TbShowDec_checkBox.UseVisualStyleBackColor = true;
            this.TbShowDec_checkBox.CheckedChanged += new System.EventHandler(this.TbShowDec_checkBox_CheckedChanged);
            // 
            // TbShowString
            // 
            this.TbShowString.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TbShowString.Location = new System.Drawing.Point(27, 75);
            this.TbShowString.Margin = new System.Windows.Forms.Padding(4);
            this.TbShowString.Multiline = true;
            this.TbShowString.Name = "TbShowString";
            this.TbShowString.ReadOnly = true;
            this.TbShowString.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TbShowString.Size = new System.Drawing.Size(1036, 296);
            this.TbShowString.TabIndex = 16;
            this.TbShowString.TextChanged += new System.EventHandler(this.TbShowString_TextChanged);
            // 
            // lbHex
            // 
            this.lbHex.AutoSize = true;
            this.lbHex.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbHex.ForeColor = System.Drawing.Color.Black;
            this.lbHex.Location = new System.Drawing.Point(775, 40);
            this.lbHex.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbHex.Name = "lbHex";
            this.lbHex.Size = new System.Drawing.Size(116, 18);
            this.lbHex.TabIndex = 14;
            this.lbHex.Text = "十六进制形式";
            // 
            // BtnSCIClearRec
            // 
            this.BtnSCIClearRec.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnSCIClearRec.ForeColor = System.Drawing.Color.Black;
            this.BtnSCIClearRec.Location = new System.Drawing.Point(159, 0);
            this.BtnSCIClearRec.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSCIClearRec.Name = "BtnSCIClearRec";
            this.BtnSCIClearRec.Size = new System.Drawing.Size(191, 29);
            this.BtnSCIClearRec.TabIndex = 8;
            this.BtnSCIClearRec.Text = "清空接收框";
            this.BtnSCIClearRec.UseVisualStyleBackColor = true;
            this.BtnSCIClearRec.Click += new System.EventHandler(this.btnClearRec_Click);
            // 
            // TbShowHex
            // 
            this.TbShowHex.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TbShowHex.Location = new System.Drawing.Point(731, 75);
            this.TbShowHex.Margin = new System.Windows.Forms.Padding(4);
            this.TbShowHex.Multiline = true;
            this.TbShowHex.Name = "TbShowHex";
            this.TbShowHex.ReadOnly = true;
            this.TbShowHex.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TbShowHex.Size = new System.Drawing.Size(332, 296);
            this.TbShowHex.TabIndex = 13;
            // 
            // lbDec
            // 
            this.lbDec.AutoSize = true;
            this.lbDec.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbDec.ForeColor = System.Drawing.Color.Black;
            this.lbDec.Location = new System.Drawing.Point(412, 40);
            this.lbDec.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbDec.Name = "lbDec";
            this.lbDec.Size = new System.Drawing.Size(98, 18);
            this.lbDec.TabIndex = 10;
            this.lbDec.Text = "十进制形式";
            this.lbDec.Click += new System.EventHandler(this.lbDec_Click);
            // 
            // TbShowDec
            // 
            this.TbShowDec.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TbShowDec.Location = new System.Drawing.Point(381, 75);
            this.TbShowDec.Margin = new System.Windows.Forms.Padding(4);
            this.TbShowDec.Multiline = true;
            this.TbShowDec.Name = "TbShowDec";
            this.TbShowDec.ReadOnly = true;
            this.TbShowDec.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TbShowDec.Size = new System.Drawing.Size(332, 296);
            this.TbShowDec.TabIndex = 9;
            this.TbShowDec.TextChanged += new System.EventHandler(this.TbShowDec_TextChanged);
            // 
            // lbChac
            // 
            this.lbChac.AutoSize = true;
            this.lbChac.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbChac.ForeColor = System.Drawing.Color.Black;
            this.lbChac.Location = new System.Drawing.Point(8, 40);
            this.lbChac.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbChac.Name = "lbChac";
            this.lbChac.Size = new System.Drawing.Size(80, 18);
            this.lbChac.TabIndex = 7;
            this.lbChac.Text = "字符形式";
            this.lbChac.Click += new System.EventHandler(this.lbChac_Click);
            // 
            // BtnState
            // 
            this.BtnState.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnState.ForeColor = System.Drawing.Color.Black;
            this.BtnState.Location = new System.Drawing.Point(931, 695);
            this.BtnState.Margin = new System.Windows.Forms.Padding(4);
            this.BtnState.Name = "BtnState";
            this.BtnState.Size = new System.Drawing.Size(180, 29);
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
            this.sSSerialPortInfo.Location = new System.Drawing.Point(0, 767);
            this.sSSerialPortInfo.Name = "sSSerialPortInfo";
            this.sSSerialPortInfo.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.sSSerialPortInfo.Size = new System.Drawing.Size(1152, 24);
            this.sSSerialPortInfo.TabIndex = 8;
            this.sSSerialPortInfo.Text = "statusStrip1";
            this.sSSerialPortInfo.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.sSSerialPortInfo_ItemClicked);
            // 
            // TSSLState
            // 
            this.TSSLState.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TSSLState.Name = "TSSLState";
            this.TSSLState.Size = new System.Drawing.Size(89, 18);
            this.TSSLState.Text = "没有操作!";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 18);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 18);
            // 
            // SCIPort
            // 
            this.SCIPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SCIPort_DataReceived);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 15000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // lblM
            // 
            this.lblM.AutoSize = true;
            this.lblM.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblM.ForeColor = System.Drawing.Color.Black;
            this.lblM.Location = new System.Drawing.Point(739, 40);
            this.lblM.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblM.Name = "lblM";
            this.lblM.Size = new System.Drawing.Size(19, 19);
            this.lblM.TabIndex = 23;
            this.lblM.Text = "0";
            this.lblM.Click += new System.EventHandler(this.label6_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(532, 40);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(181, 19);
            this.label7.TabIndex = 24;
            this.label7.Text = "时间间隔（30秒）：";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // lblFlag
            // 
            this.lblFlag.AutoSize = true;
            this.lblFlag.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblFlag.ForeColor = System.Drawing.Color.Black;
            this.lblFlag.Location = new System.Drawing.Point(392, 40);
            this.lblFlag.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFlag.Name = "lblFlag";
            this.lblFlag.Size = new System.Drawing.Size(39, 19);
            this.lblFlag.TabIndex = 25;
            this.lblFlag.Text = "---";
            this.lblFlag.Click += new System.EventHandler(this.lblFlag_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(274, 40);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 19);
            this.label4.TabIndex = 27;
            this.label4.Text = "检测状态：";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.lblFlag);
            this.groupBox1.Controls.Add(this.lblM);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.ForeColor = System.Drawing.Color.Blue;
            this.groupBox1.Location = new System.Drawing.Point(32, 665);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(880, 80);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "计时器检测";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(155, 40);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 19);
            this.label5.TabIndex = 29;
            this.label5.Text = "---";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(28, 40);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 19);
            this.label3.TabIndex = 28;
            this.label3.Text = "PC机时间：";
            // 
            // FrmSCI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1152, 791);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.sSSerialPortInfo);
            this.Controls.Add(this.BtnState);
            this.Controls.Add(this.SerialPortReceive);
            this.Controls.Add(this.SerialPortSend);
            this.Controls.Add(this.SetSerialPort);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmSCI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "串口测试工程（V2.0)  202004     SD-Arm出品";
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
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Label lblM;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblFlag;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
    }
}

