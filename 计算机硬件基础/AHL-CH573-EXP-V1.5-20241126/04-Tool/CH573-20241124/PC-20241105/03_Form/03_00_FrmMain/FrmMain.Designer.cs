namespace AHL_GEC
{
    partial class FrmMain
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
            WeifenLuo.WinFormsUI.Docking.DockPanelSkin dockPanelSkin1 = new WeifenLuo.WinFormsUI.Docking.DockPanelSkin();
            WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin autoHideStripSkin1 = new WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient1 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin dockPaneStripSkin1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient dockPaneStripGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient2 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient2 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient3 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient4 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient5 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient3 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient6 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient7 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.tmr_tick = new System.Windows.Forms.Timer(this.components);
            this.mns_main = new System.Windows.Forms.MenuStrip();
            this.用户功能ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.单个设备程序更新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.程序更新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设备信息更新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.串口数据接收ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.构件固化ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.生产商功能ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bIOS更新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.VideoFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ReadVideoFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectFileSendToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm_update = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_uartupdate = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_remoteupdate = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_uartdynamic = new System.Windows.Forms.ToolStripMenuItem();
            this.dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.lbl_mainstatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbl_protocal = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbl_location = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbl_curtime = new System.Windows.Forms.ToolStripStatusLabel();
            this.ssr_state = new System.Windows.Forms.StatusStrip();
            this.mns_main.SuspendLayout();
            this.ssr_state.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmr_tick
            // 
            this.tmr_tick.Enabled = true;
            this.tmr_tick.Interval = 1000;
            this.tmr_tick.Tick += new System.EventHandler(this.tmr_tick_Tick);
            // 
            // mns_main
            // 
            this.mns_main.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.mns_main.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mns_main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.用户功能ToolStripMenuItem,
            this.生产商功能ToolStripMenuItem,
            this.VideoFileToolStripMenuItem,
            this.tsm_update});
            this.mns_main.Location = new System.Drawing.Point(0, 0);
            this.mns_main.Name = "mns_main";
            this.mns_main.Size = new System.Drawing.Size(993, 32);
            this.mns_main.TabIndex = 0;
            this.mns_main.Text = "menuStrip1";
            // 
            // 用户功能ToolStripMenuItem
            // 
            this.用户功能ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.单个设备程序更新ToolStripMenuItem,
            this.程序更新ToolStripMenuItem,
            this.设备信息更新ToolStripMenuItem,
            this.串口数据接收ToolStripMenuItem,
            this.构件固化ToolStripMenuItem});
            this.用户功能ToolStripMenuItem.Name = "用户功能ToolStripMenuItem";
            this.用户功能ToolStripMenuItem.Size = new System.Drawing.Size(83, 28);
            this.用户功能ToolStripMenuItem.Text = "用户功能";
            // 
            // 单个设备程序更新ToolStripMenuItem
            // 
            this.单个设备程序更新ToolStripMenuItem.Name = "单个设备程序更新ToolStripMenuItem";
            this.单个设备程序更新ToolStripMenuItem.Size = new System.Drawing.Size(212, 26);
            this.单个设备程序更新ToolStripMenuItem.Text = "单个设备程序更新";
            this.单个设备程序更新ToolStripMenuItem.Click += new System.EventHandler(this.单个设备程序更新ToolStripMenuItem_Click);
            // 
            // 程序更新ToolStripMenuItem
            // 
            this.程序更新ToolStripMenuItem.Name = "程序更新ToolStripMenuItem";
            this.程序更新ToolStripMenuItem.Size = new System.Drawing.Size(212, 26);
            this.程序更新ToolStripMenuItem.Text = "多个设备程序更新";
            this.程序更新ToolStripMenuItem.Click += new System.EventHandler(this.程序更新ToolStripMenuItem_Click);
            // 
            // 设备信息更新ToolStripMenuItem
            // 
            this.设备信息更新ToolStripMenuItem.Name = "设备信息更新ToolStripMenuItem";
            this.设备信息更新ToolStripMenuItem.Size = new System.Drawing.Size(212, 26);
            this.设备信息更新ToolStripMenuItem.Text = "设备信息更新";
            this.设备信息更新ToolStripMenuItem.Click += new System.EventHandler(this.设备信息更新ToolStripMenuItem_Click);
            // 
            // 串口数据接收ToolStripMenuItem
            // 
            this.串口数据接收ToolStripMenuItem.Name = "串口数据接收ToolStripMenuItem";
            this.串口数据接收ToolStripMenuItem.Size = new System.Drawing.Size(212, 26);
            this.串口数据接收ToolStripMenuItem.Text = "串口数据接收";
            this.串口数据接收ToolStripMenuItem.Click += new System.EventHandler(this.串口数据接收ToolStripMenuItem_Click);
            // 
            // 构件固化ToolStripMenuItem
            // 
            this.构件固化ToolStripMenuItem.Name = "构件固化ToolStripMenuItem";
            this.构件固化ToolStripMenuItem.Size = new System.Drawing.Size(212, 26);
            this.构件固化ToolStripMenuItem.Text = "构件固化";
            this.构件固化ToolStripMenuItem.Click += new System.EventHandler(this.构件固化ToolStripMenuItem_Click);
            // 
            // 生产商功能ToolStripMenuItem
            // 
            this.生产商功能ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bIOS更新ToolStripMenuItem});
            this.生产商功能ToolStripMenuItem.Name = "生产商功能ToolStripMenuItem";
            this.生产商功能ToolStripMenuItem.Size = new System.Drawing.Size(98, 28);
            this.生产商功能ToolStripMenuItem.Text = "生产商功能";
            // 
            // bIOS更新ToolStripMenuItem
            // 
            this.bIOS更新ToolStripMenuItem.Name = "bIOS更新ToolStripMenuItem";
            this.bIOS更新ToolStripMenuItem.Size = new System.Drawing.Size(156, 26);
            this.bIOS更新ToolStripMenuItem.Text = "BIOS更新";
            // 
            // VideoFileToolStripMenuItem
            // 
            this.VideoFileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ReadVideoFileToolStripMenuItem,
            this.selectFileSendToolStripMenuItem});
            this.VideoFileToolStripMenuItem.Name = "VideoFileToolStripMenuItem";
            this.VideoFileToolStripMenuItem.Size = new System.Drawing.Size(83, 28);
            this.VideoFileToolStripMenuItem.Text = "视频文件";
            // 
            // ReadVideoFileToolStripMenuItem
            // 
            this.ReadVideoFileToolStripMenuItem.Name = "ReadVideoFileToolStripMenuItem";
            this.ReadVideoFileToolStripMenuItem.Size = new System.Drawing.Size(227, 26);
            this.ReadVideoFileToolStripMenuItem.Text = "读取并拆分视频文件";
            this.ReadVideoFileToolStripMenuItem.Click += new System.EventHandler(this.ReadVideoFileToolStripMenuItem_Click);
            // 
            // selectFileSendToolStripMenuItem
            // 
            this.selectFileSendToolStripMenuItem.Name = "selectFileSendToolStripMenuItem";
            this.selectFileSendToolStripMenuItem.Size = new System.Drawing.Size(227, 26);
            this.selectFileSendToolStripMenuItem.Text = "选择文件并发送";
            this.selectFileSendToolStripMenuItem.Click += new System.EventHandler(this.selectFileSendToolStripMenuItem_Click);
            // 
            // tsm_update
            // 
            this.tsm_update.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_uartupdate,
            this.tsmi_remoteupdate,
            this.tsmi_uartdynamic});
            this.tsm_update.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tsm_update.Name = "tsm_update";
            this.tsm_update.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.tsm_update.Size = new System.Drawing.Size(60, 28);
            this.tsm_update.Text = "下载";
            // 
            // tsmi_uartupdate
            // 
            this.tsmi_uartupdate.Name = "tsmi_uartupdate";
            this.tsmi_uartupdate.Size = new System.Drawing.Size(166, 28);
            this.tsmi_uartupdate.Text = "串口更新";
            this.tsmi_uartupdate.Click += new System.EventHandler(this.tsmi_uartupdate_Click);
            // 
            // tsmi_remoteupdate
            // 
            this.tsmi_remoteupdate.Enabled = false;
            this.tsmi_remoteupdate.Name = "tsmi_remoteupdate";
            this.tsmi_remoteupdate.Size = new System.Drawing.Size(166, 28);
            this.tsmi_remoteupdate.Text = "远程更新";
            this.tsmi_remoteupdate.Visible = false;
            // 
            // tsmi_uartdynamic
            // 
            this.tsmi_uartdynamic.Enabled = false;
            this.tsmi_uartdynamic.Name = "tsmi_uartdynamic";
            this.tsmi_uartdynamic.Size = new System.Drawing.Size(166, 28);
            this.tsmi_uartdynamic.Text = "动态装载";
            this.tsmi_uartdynamic.Visible = false;
            // 
            // dockPanel1
            // 
            this.dockPanel1.ActiveAutoHideContent = null;
            this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel1.DockBackColor = System.Drawing.SystemColors.Control;
            this.dockPanel1.Location = new System.Drawing.Point(0, 32);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Size = new System.Drawing.Size(993, 541);
            dockPanelGradient1.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient1.StartColor = System.Drawing.SystemColors.ControlLight;
            autoHideStripSkin1.DockStripGradient = dockPanelGradient1;
            tabGradient1.EndColor = System.Drawing.SystemColors.Control;
            tabGradient1.StartColor = System.Drawing.SystemColors.Control;
            tabGradient1.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            autoHideStripSkin1.TabGradient = tabGradient1;
            dockPanelSkin1.AutoHideStripSkin = autoHideStripSkin1;
            tabGradient2.EndColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient2.StartColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient2.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient1.ActiveTabGradient = tabGradient2;
            dockPanelGradient2.EndColor = System.Drawing.SystemColors.Control;
            dockPanelGradient2.StartColor = System.Drawing.SystemColors.Control;
            dockPaneStripGradient1.DockStripGradient = dockPanelGradient2;
            tabGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
            tabGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
            tabGradient3.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient1.InactiveTabGradient = tabGradient3;
            dockPaneStripSkin1.DocumentGradient = dockPaneStripGradient1;
            tabGradient4.EndColor = System.Drawing.SystemColors.ActiveCaption;
            tabGradient4.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient4.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
            tabGradient4.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
            dockPaneStripToolWindowGradient1.ActiveCaptionGradient = tabGradient4;
            tabGradient5.EndColor = System.Drawing.SystemColors.Control;
            tabGradient5.StartColor = System.Drawing.SystemColors.Control;
            tabGradient5.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient1.ActiveTabGradient = tabGradient5;
            dockPanelGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
            dockPaneStripToolWindowGradient1.DockStripGradient = dockPanelGradient3;
            tabGradient6.EndColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient6.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient6.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient6.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient1.InactiveCaptionGradient = tabGradient6;
            tabGradient7.EndColor = System.Drawing.Color.Transparent;
            tabGradient7.StartColor = System.Drawing.Color.Transparent;
            tabGradient7.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            dockPaneStripToolWindowGradient1.InactiveTabGradient = tabGradient7;
            dockPaneStripSkin1.ToolWindowGradient = dockPaneStripToolWindowGradient1;
            dockPanelSkin1.DockPaneStripSkin = dockPaneStripSkin1;
            this.dockPanel1.Skin = dockPanelSkin1;
            this.dockPanel1.TabIndex = 12;
            // 
            // lbl_mainstatus
            // 
            this.lbl_mainstatus.AutoSize = false;
            this.lbl_mainstatus.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lbl_mainstatus.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lbl_mainstatus.Name = "lbl_mainstatus";
            this.lbl_mainstatus.Size = new System.Drawing.Size(380, 20);
            this.lbl_mainstatus.Text = "运行状态：无操作";
            this.lbl_mainstatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_protocal
            // 
            this.lbl_protocal.AutoSize = false;
            this.lbl_protocal.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lbl_protocal.Name = "lbl_protocal";
            this.lbl_protocal.Size = new System.Drawing.Size(199, 20);
            this.lbl_protocal.Spring = true;
            this.lbl_protocal.Text = "协议：";
            this.lbl_protocal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoSize = false;
            this.toolStripStatusLabel1.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(199, 20);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "协议信息：";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_location
            // 
            this.lbl_location.AutoSize = false;
            this.lbl_location.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lbl_location.Name = "lbl_location";
            this.lbl_location.Size = new System.Drawing.Size(199, 20);
            this.lbl_location.Spring = true;
            this.lbl_location.Text = "位置信息：";
            this.lbl_location.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_curtime
            // 
            this.lbl_curtime.AutoSize = false;
            this.lbl_curtime.BackColor = System.Drawing.SystemColors.HotTrack;
            this.lbl_curtime.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lbl_curtime.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.lbl_curtime.Name = "lbl_curtime";
            this.lbl_curtime.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbl_curtime.Size = new System.Drawing.Size(178, 20);
            this.lbl_curtime.Spring = true;
            this.lbl_curtime.Text = "当前时间：";
            this.lbl_curtime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ssr_state
            // 
            this.ssr_state.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ssr_state.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbl_mainstatus,
            this.lbl_protocal,
            this.toolStripStatusLabel1,
            this.lbl_location,
            this.lbl_curtime});
            this.ssr_state.Location = new System.Drawing.Point(0, 573);
            this.ssr_state.Name = "ssr_state";
            this.ssr_state.Size = new System.Drawing.Size(993, 26);
            this.ssr_state.TabIndex = 11;
            this.ssr_state.Text = "statusStrip1";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(993, 599);
            this.Controls.Add(this.dockPanel1);
            this.Controls.Add(this.ssr_state);
            this.Controls.Add(this.mns_main);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.SystemColors.WindowText;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Location = new System.Drawing.Point(250, 50);
            this.MainMenuStrip = this.mns_main;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "AHL-CH573代码更新系统上位机  2024年11月";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMain_FormClosed);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.mns_main.ResumeLayout(false);
            this.mns_main.PerformLayout();
            this.ssr_state.ResumeLayout(false);
            this.ssr_state.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tmr_tick;
        private System.Windows.Forms.MenuStrip mns_main;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
        private System.Windows.Forms.ToolStripMenuItem 用户功能ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 程序更新ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 生产商功能ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设备信息更新ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bIOS更新ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 串口数据接收ToolStripMenuItem;
        public System.Windows.Forms.ToolStripStatusLabel lbl_mainstatus;
        public System.Windows.Forms.ToolStripStatusLabel lbl_protocal;
        public System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        public System.Windows.Forms.ToolStripStatusLabel lbl_location;
        private System.Windows.Forms.ToolStripStatusLabel lbl_curtime;
        private System.Windows.Forms.StatusStrip ssr_state;
        private System.Windows.Forms.ToolStripMenuItem 单个设备程序更新ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 构件固化ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem VideoFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ReadVideoFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectFileSendToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsm_update;
        private System.Windows.Forms.ToolStripMenuItem tsmi_uartupdate;
        private System.Windows.Forms.ToolStripMenuItem tsmi_remoteupdate;
        private System.Windows.Forms.ToolStripMenuItem tsmi_uartdynamic;
    }
}