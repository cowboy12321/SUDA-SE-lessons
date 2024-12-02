
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.StripLine stripLine1 = new System.Windows.Forms.DataVisualization.Charting.StripLine();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.LbPortName = new System.Windows.Forms.Label();
            this.CbSCIComNum = new System.Windows.Forms.ComboBox();
            this.SetSerialPort = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.BtnSCISwitch = new System.Windows.Forms.Button();
            this.LblSCI = new System.Windows.Forms.Label();
            this.CbSCIBaud = new System.Windows.Forms.ComboBox();
            this.Baud = new System.Windows.Forms.Label();
            this.SerialPortReceive = new System.Windows.Forms.GroupBox();
            this.TbShowString = new System.Windows.Forms.TextBox();
            this.BtnSCIClearRec = new System.Windows.Forms.Button();
            this.lbChac = new System.Windows.Forms.Label();
            this.BtnState = new System.Windows.Forms.Button();
            this.sSSerialPortInfo = new System.Windows.Forms.StatusStrip();
            this.TSSLState = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.SCIPort = new System.IO.Ports.SerialPort(this.components);
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.systemTime = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SetSerialPort.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SerialPortReceive.SuspendLayout();
            this.sSSerialPortInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LbPortName
            // 
            this.LbPortName.AutoSize = true;
            this.LbPortName.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LbPortName.ForeColor = System.Drawing.Color.Black;
            this.LbPortName.Location = new System.Drawing.Point(87, 41);
            this.LbPortName.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.LbPortName.Name = "LbPortName";
            this.LbPortName.Size = new System.Drawing.Size(124, 28);
            this.LbPortName.TabIndex = 0;
            this.LbPortName.Text = "串口选择";
            // 
            // CbSCIComNum
            // 
            this.CbSCIComNum.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CbSCIComNum.FormattingEnabled = true;
            this.CbSCIComNum.Location = new System.Drawing.Point(235, 42);
            this.CbSCIComNum.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.CbSCIComNum.Name = "CbSCIComNum";
            this.CbSCIComNum.Size = new System.Drawing.Size(266, 36);
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
            this.SetSerialPort.Location = new System.Drawing.Point(69, 25);
            this.SetSerialPort.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.SetSerialPort.Name = "SetSerialPort";
            this.SetSerialPort.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.SetSerialPort.Size = new System.Drawing.Size(2081, 147);
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
            this.pictureBox1.Location = new System.Drawing.Point(1643, 31);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(254, 77);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // BtnSCISwitch
            // 
            this.BtnSCISwitch.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnSCISwitch.ForeColor = System.Drawing.Color.Black;
            this.BtnSCISwitch.Location = new System.Drawing.Point(1258, 38);
            this.BtnSCISwitch.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.BtnSCISwitch.Name = "BtnSCISwitch";
            this.BtnSCISwitch.Size = new System.Drawing.Size(204, 70);
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
            this.LblSCI.Location = new System.Drawing.Point(44, 109);
            this.LblSCI.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.LblSCI.Name = "LblSCI";
            this.LblSCI.Size = new System.Drawing.Size(152, 28);
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
            this.CbSCIBaud.Location = new System.Drawing.Point(864, 46);
            this.CbSCIBaud.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.CbSCIBaud.Name = "CbSCIBaud";
            this.CbSCIBaud.Size = new System.Drawing.Size(237, 36);
            this.CbSCIBaud.TabIndex = 3;
            this.CbSCIBaud.SelectedIndexChanged += new System.EventHandler(this.CbSCIBaud_SelectedIndexChanged);
            // 
            // Baud
            // 
            this.Baud.AutoSize = true;
            this.Baud.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Baud.ForeColor = System.Drawing.Color.Black;
            this.Baud.Location = new System.Drawing.Point(680, 42);
            this.Baud.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.Baud.Name = "Baud";
            this.Baud.Size = new System.Drawing.Size(152, 28);
            this.Baud.TabIndex = 2;
            this.Baud.Text = "波特率选择";
            // 
            // SerialPortReceive
            // 
            this.SerialPortReceive.Controls.Add(this.TbShowString);
            this.SerialPortReceive.Controls.Add(this.BtnSCIClearRec);
            this.SerialPortReceive.Controls.Add(this.lbChac);
            this.SerialPortReceive.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SerialPortReceive.ForeColor = System.Drawing.Color.Blue;
            this.SerialPortReceive.Location = new System.Drawing.Point(51, 184);
            this.SerialPortReceive.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.SerialPortReceive.Name = "SerialPortReceive";
            this.SerialPortReceive.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.SerialPortReceive.Size = new System.Drawing.Size(2129, 390);
            this.SerialPortReceive.TabIndex = 7;
            this.SerialPortReceive.TabStop = false;
            this.SerialPortReceive.Text = "接收数据设置";
            // 
            // TbShowString
            // 
            this.TbShowString.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TbShowString.Location = new System.Drawing.Point(40, 120);
            this.TbShowString.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.TbShowString.Multiline = true;
            this.TbShowString.Name = "TbShowString";
            this.TbShowString.ReadOnly = true;
            this.TbShowString.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TbShowString.Size = new System.Drawing.Size(2059, 217);
            this.TbShowString.TabIndex = 16;
            this.TbShowString.TextChanged += new System.EventHandler(this.TbShowString_TextChanged);
            // 
            // BtnSCIClearRec
            // 
            this.BtnSCIClearRec.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnSCIClearRec.ForeColor = System.Drawing.Color.Black;
            this.BtnSCIClearRec.Location = new System.Drawing.Point(238, 0);
            this.BtnSCIClearRec.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.BtnSCIClearRec.Name = "BtnSCIClearRec";
            this.BtnSCIClearRec.Size = new System.Drawing.Size(229, 46);
            this.BtnSCIClearRec.TabIndex = 8;
            this.BtnSCIClearRec.Text = "清空接收框";
            this.BtnSCIClearRec.UseVisualStyleBackColor = true;
            this.BtnSCIClearRec.Click += new System.EventHandler(this.btnClearRec_Click);
            // 
            // lbChac
            // 
            this.lbChac.AutoSize = true;
            this.lbChac.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbChac.ForeColor = System.Drawing.Color.Black;
            this.lbChac.Location = new System.Drawing.Point(12, 64);
            this.lbChac.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.lbChac.Name = "lbChac";
            this.lbChac.Size = new System.Drawing.Size(124, 28);
            this.lbChac.TabIndex = 7;
            this.lbChac.Text = "字符形式";
            this.lbChac.Click += new System.EventHandler(this.lbChac_Click);
            // 
            // BtnState
            // 
            this.BtnState.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnState.ForeColor = System.Drawing.Color.Black;
            this.BtnState.Location = new System.Drawing.Point(1992, 1075);
            this.BtnState.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.BtnState.Name = "BtnState";
            this.BtnState.Size = new System.Drawing.Size(271, 46);
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
            this.sSSerialPortInfo.Location = new System.Drawing.Point(0, 1127);
            this.sSSerialPortInfo.Name = "sSSerialPortInfo";
            this.sSSerialPortInfo.Padding = new System.Windows.Forms.Padding(2, 0, 28, 0);
            this.sSSerialPortInfo.Size = new System.Drawing.Size(2520, 38);
            this.sSSerialPortInfo.TabIndex = 8;
            this.sSSerialPortInfo.Text = "statusStrip1";
            this.sSSerialPortInfo.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.sSSerialPortInfo_ItemClicked);
            // 
            // TSSLState
            // 
            this.TSSLState.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TSSLState.Name = "TSSLState";
            this.TSSLState.Size = new System.Drawing.Size(138, 28);
            this.TSSLState.Text = "没有操作!";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 28);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 28);
            // 
            // SCIPort
            // 
            this.SCIPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SCIPort_DataReceived);
            // 
            // timer
            // 
            this.timer.Interval = 1;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // chart1
            // 
            chartArea1.AxisX.InterlacedColor = System.Drawing.Color.White;
            chartArea1.AxisX.LabelStyle.Enabled = false;
            chartArea1.AxisX.LineColor = System.Drawing.Color.Sienna;
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.LightSeaGreen;
            chartArea1.AxisX.MajorTickMark.Enabled = false;
            chartArea1.AxisX.StripLines.Add(stripLine1);
            chartArea1.AxisX.Title = "时间轴";
            chartArea1.AxisY.LineColor = System.Drawing.Color.Sienna;
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.LightSeaGreen;
            chartArea1.AxisY.Title = "电平1/0";
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(38, 64);
            this.chart1.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.chart1.Name = "chart1";
            series1.BorderWidth = 2;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Color = System.Drawing.Color.OrangeRed;
            series1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            series1.Legend = "Legend1";
            series1.Name = "S1";
            series1.ShadowColor = System.Drawing.Color.OrangeRed;
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(2061, 300);
            this.chart1.TabIndex = 26;
            this.chart1.Text = "chart1";
            this.chart1.Click += new System.EventHandler(this.chart1_Click);
            // 
            // systemTime
            // 
            this.systemTime.AutoSize = true;
            this.systemTime.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.systemTime.ForeColor = System.Drawing.Color.SaddleBrown;
            this.systemTime.Location = new System.Drawing.Point(568, 387);
            this.systemTime.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.systemTime.Name = "systemTime";
            this.systemTime.Size = new System.Drawing.Size(111, 33);
            this.systemTime.TabIndex = 28;
            this.systemTime.Text = "2021年";
            this.systemTime.Click += new System.EventHandler(this.systemTime_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chart1);
            this.groupBox1.Controls.Add(this.systemTime);
            this.groupBox1.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.ForeColor = System.Drawing.Color.Blue;
            this.groupBox1.Location = new System.Drawing.Point(51, 599);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.groupBox1.Size = new System.Drawing.Size(2129, 428);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "高低电平显示";
            // 
            // FrmSCI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(2520, 1165);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.sSSerialPortInfo);
            this.Controls.Add(this.BtnState);
            this.Controls.Add(this.SerialPortReceive);
            this.Controls.Add(this.SetSerialPort);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.Name = "FrmSCI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PWM-Incapture-测试程序C#（V3.1)  20241110     苏大嵌入式荣誉出品";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmSCI_FormClosing);
            this.Load += new System.EventHandler(this.FrmSCI_Load);
            this.SetSerialPort.ResumeLayout(false);
            this.SetSerialPort.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.SerialPortReceive.ResumeLayout(false);
            this.SerialPortReceive.PerformLayout();
            this.sSSerialPortInfo.ResumeLayout(false);
            this.sSSerialPortInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
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
        private System.Windows.Forms.GroupBox SerialPortReceive;
        private System.Windows.Forms.StatusStrip sSSerialPortInfo;
        private System.Windows.Forms.Label lbChac;
        private System.Windows.Forms.Button BtnSCIClearRec;
        private System.IO.Ports.SerialPort SCIPort;
        private System.Windows.Forms.ToolStripStatusLabel TSSLState;
        private System.Windows.Forms.Button BtnState;
        private System.Windows.Forms.TextBox TbShowString;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Label systemTime;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

