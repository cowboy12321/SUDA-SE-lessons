namespace AHL_GEC._03_Form._03_02_FrmSCI
{
    partial class Frm_Monitor
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
            this.txtname = new System.Windows.Forms.TextBox();
            this.txtcompany = new System.Windows.Forms.TextBox();
            this.txttime = new System.Windows.Forms.TextBox();
            this.lbl_name = new System.Windows.Forms.Label();
            this.lbl_company = new System.Windows.Forms.Label();
            this.lbl_time = new System.Windows.Forms.Label();
            this.grp_log = new System.Windows.Forms.GroupBox();
            this.txt_log = new System.Windows.Forms.TextBox();
            this.lbl_lock_status = new System.Windows.Forms.Label();
            this.txtstate = new System.Windows.Forms.TextBox();
            this.SetSerialPort = new System.Windows.Forms.GroupBox();
            this.BtnSCISwitch = new System.Windows.Forms.Button();
            this.CbSCIBaud = new System.Windows.Forms.ComboBox();
            this.Baud = new System.Windows.Forms.Label();
            this.LbPortName = new System.Windows.Forms.Label();
            this.CbSCIComNum = new System.Windows.Forms.ComboBox();
            this.sSSerialPortInfo = new System.Windows.Forms.StatusStrip();
            this.TSSLState = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.SCIPort = new System.IO.Ports.SerialPort(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.txtdevicename = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtversion = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grp_log.SuspendLayout();
            this.SetSerialPort.SuspendLayout();
            this.sSSerialPortInfo.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtname
            // 
            this.txtname.Location = new System.Drawing.Point(21, 65);
            this.txtname.Name = "txtname";
            this.txtname.ReadOnly = true;
            this.txtname.Size = new System.Drawing.Size(100, 31);
            this.txtname.TabIndex = 0;
            // 
            // txtcompany
            // 
            this.txtcompany.Location = new System.Drawing.Point(127, 65);
            this.txtcompany.Name = "txtcompany";
            this.txtcompany.ReadOnly = true;
            this.txtcompany.Size = new System.Drawing.Size(100, 31);
            this.txtcompany.TabIndex = 1;
            // 
            // txttime
            // 
            this.txttime.Location = new System.Drawing.Point(233, 65);
            this.txttime.Name = "txttime";
            this.txttime.ReadOnly = true;
            this.txttime.Size = new System.Drawing.Size(100, 31);
            this.txttime.TabIndex = 2;
            // 
            // lbl_name
            // 
            this.lbl_name.AutoSize = true;
            this.lbl_name.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_name.Location = new System.Drawing.Point(50, 42);
            this.lbl_name.Name = "lbl_name";
            this.lbl_name.Size = new System.Drawing.Size(49, 20);
            this.lbl_name.TabIndex = 4;
            this.lbl_name.Text = "姓名";
            // 
            // lbl_company
            // 
            this.lbl_company.AutoSize = true;
            this.lbl_company.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_company.Location = new System.Drawing.Point(151, 42);
            this.lbl_company.Name = "lbl_company";
            this.lbl_company.Size = new System.Drawing.Size(49, 20);
            this.lbl_company.TabIndex = 5;
            this.lbl_company.Text = "公司";
            // 
            // lbl_time
            // 
            this.lbl_time.AutoSize = true;
            this.lbl_time.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_time.Location = new System.Drawing.Point(259, 42);
            this.lbl_time.Name = "lbl_time";
            this.lbl_time.Size = new System.Drawing.Size(49, 20);
            this.lbl_time.TabIndex = 6;
            this.lbl_time.Text = "时间";
            // 
            // grp_log
            // 
            this.grp_log.BackColor = System.Drawing.SystemColors.Control;
            this.grp_log.Controls.Add(this.txt_log);
            this.grp_log.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.grp_log.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.grp_log.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grp_log.Location = new System.Drawing.Point(135, 318);
            this.grp_log.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grp_log.Name = "grp_log";
            this.grp_log.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grp_log.Size = new System.Drawing.Size(613, 279);
            this.grp_log.TabIndex = 8;
            this.grp_log.TabStop = false;
            this.grp_log.Text = "柜子日志";
            // 
            // txt_log
            // 
            this.txt_log.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_log.Location = new System.Drawing.Point(6, 32);
            this.txt_log.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_log.Multiline = true;
            this.txt_log.Name = "txt_log";
            this.txt_log.ReadOnly = true;
            this.txt_log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_log.Size = new System.Drawing.Size(601, 239);
            this.txt_log.TabIndex = 1;
            // 
            // lbl_lock_status
            // 
            this.lbl_lock_status.AutoSize = true;
            this.lbl_lock_status.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_lock_status.Location = new System.Drawing.Point(335, 42);
            this.lbl_lock_status.Name = "lbl_lock_status";
            this.lbl_lock_status.Size = new System.Drawing.Size(109, 20);
            this.lbl_lock_status.TabIndex = 10;
            this.lbl_lock_status.Text = "锁开关状态";
            // 
            // txtstate
            // 
            this.txtstate.Location = new System.Drawing.Point(339, 65);
            this.txtstate.Name = "txtstate";
            this.txtstate.ReadOnly = true;
            this.txtstate.Size = new System.Drawing.Size(100, 31);
            this.txtstate.TabIndex = 9;
            // 
            // SetSerialPort
            // 
            this.SetSerialPort.Controls.Add(this.BtnSCISwitch);
            this.SetSerialPort.Controls.Add(this.CbSCIBaud);
            this.SetSerialPort.Controls.Add(this.Baud);
            this.SetSerialPort.Controls.Add(this.LbPortName);
            this.SetSerialPort.Controls.Add(this.CbSCIComNum);
            this.SetSerialPort.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.SetSerialPort.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SetSerialPort.ForeColor = System.Drawing.Color.Blue;
            this.SetSerialPort.Location = new System.Drawing.Point(30, 22);
            this.SetSerialPort.Margin = new System.Windows.Forms.Padding(4);
            this.SetSerialPort.Name = "SetSerialPort";
            this.SetSerialPort.Padding = new System.Windows.Forms.Padding(4);
            this.SetSerialPort.Size = new System.Drawing.Size(879, 81);
            this.SetSerialPort.TabIndex = 11;
            this.SetSerialPort.TabStop = false;
            this.SetSerialPort.Text = "串口设置";
            // 
            // BtnSCISwitch
            // 
            this.BtnSCISwitch.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnSCISwitch.ForeColor = System.Drawing.Color.Black;
            this.BtnSCISwitch.Location = new System.Drawing.Point(735, 24);
            this.BtnSCISwitch.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSCISwitch.Name = "BtnSCISwitch";
            this.BtnSCISwitch.Size = new System.Drawing.Size(136, 44);
            this.BtnSCISwitch.TabIndex = 4;
            this.BtnSCISwitch.Text = "打开串口";
            this.BtnSCISwitch.UseVisualStyleBackColor = true;
            this.BtnSCISwitch.Click += new System.EventHandler(this.BtnSCISwitch_Click);
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
            // 
            // sSSerialPortInfo
            // 
            this.sSSerialPortInfo.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.sSSerialPortInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSSLState,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.sSSerialPortInfo.Location = new System.Drawing.Point(0, 601);
            this.sSSerialPortInfo.Name = "sSSerialPortInfo";
            this.sSSerialPortInfo.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.sSSerialPortInfo.Size = new System.Drawing.Size(938, 24);
            this.sSSerialPortInfo.TabIndex = 12;
            this.sSSerialPortInfo.Text = "statusStrip1";
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(506, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 20);
            this.label1.TabIndex = 13;
            this.label1.Text = "设备号";
            // 
            // txtdevicename
            // 
            this.txtdevicename.Location = new System.Drawing.Point(445, 65);
            this.txtdevicename.Name = "txtdevicename";
            this.txtdevicename.ReadOnly = true;
            this.txtdevicename.Size = new System.Drawing.Size(189, 31);
            this.txtdevicename.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(651, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 20);
            this.label2.TabIndex = 16;
            this.label2.Text = "软件版本";
            // 
            // txtversion
            // 
            this.txtversion.Location = new System.Drawing.Point(640, 65);
            this.txtversion.Name = "txtversion";
            this.txtversion.ReadOnly = true;
            this.txtversion.Size = new System.Drawing.Size(100, 31);
            this.txtversion.TabIndex = 15;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtcompany);
            this.groupBox1.Controls.Add(this.txtversion);
            this.groupBox1.Controls.Add(this.txtname);
            this.groupBox1.Controls.Add(this.txtdevicename);
            this.groupBox1.Controls.Add(this.txttime);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lbl_name);
            this.groupBox1.Controls.Add(this.lbl_company);
            this.groupBox1.Controls.Add(this.lbl_time);
            this.groupBox1.Controls.Add(this.lbl_lock_status);
            this.groupBox1.Controls.Add(this.txtstate);
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(30, 127);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(879, 184);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "最新刷卡信息";
            // 
            // Frm_Monitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(938, 625);
            this.Controls.Add(this.sSSerialPortInfo);
            this.Controls.Add(this.SetSerialPort);
            this.Controls.Add(this.grp_log);
            this.Controls.Add(this.groupBox1);
            this.Name = "Frm_Monitor";
            this.Text = "刷卡日志";
            this.Load += new System.EventHandler(this.Frm_Monitor_Load);
            this.grp_log.ResumeLayout(false);
            this.grp_log.PerformLayout();
            this.SetSerialPort.ResumeLayout(false);
            this.SetSerialPort.PerformLayout();
            this.sSSerialPortInfo.ResumeLayout(false);
            this.sSSerialPortInfo.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtname;
        private System.Windows.Forms.TextBox txtcompany;
        private System.Windows.Forms.TextBox txttime;
        private System.Windows.Forms.Label lbl_name;
        private System.Windows.Forms.Label lbl_company;
        private System.Windows.Forms.Label lbl_time;
        private System.Windows.Forms.GroupBox grp_log;
        private System.Windows.Forms.Label lbl_lock_status;
        private System.Windows.Forms.TextBox txtstate;
        private System.Windows.Forms.TextBox txt_log;
        private System.Windows.Forms.GroupBox SetSerialPort;
        private System.Windows.Forms.Button BtnSCISwitch;
        private System.Windows.Forms.ComboBox CbSCIBaud;
        private System.Windows.Forms.Label Baud;
        private System.Windows.Forms.Label LbPortName;
        private System.Windows.Forms.ComboBox CbSCIComNum;
        private System.Windows.Forms.StatusStrip sSSerialPortInfo;
        private System.Windows.Forms.ToolStripStatusLabel TSSLState;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.IO.Ports.SerialPort SCIPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtdevicename;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtversion;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}