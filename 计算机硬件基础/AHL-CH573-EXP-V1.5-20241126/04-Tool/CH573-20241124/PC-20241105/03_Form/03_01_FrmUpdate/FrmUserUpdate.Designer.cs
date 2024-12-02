namespace AHL_GEC
{
    partial class frm_UserUpdate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_UserUpdate));
            this.grp_uartHint = new System.Windows.Forms.GroupBox();
            this.lbl_uartstate = new System.Windows.Forms.Label();
            this.lbl_uartstyle = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.btnCloseUpdate = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.prg_update1 = new System.Windows.Forms.ProgressBar();
            this.lbl_progressbar1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.infor_dispaly_button = new System.Windows.Forms.Button();
            this.txt_updateinfo1 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.User_filename1 = new System.Windows.Forms.Label();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.lst_codeshow2 = new System.Windows.Forms.ListBox();
            this.btn_fileopen2 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.grp_uartHint.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // grp_uartHint
            // 
            this.grp_uartHint.BackColor = System.Drawing.SystemColors.Control;
            this.grp_uartHint.Controls.Add(this.lbl_uartstate);
            this.grp_uartHint.Controls.Add(this.lbl_uartstyle);
            this.grp_uartHint.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.grp_uartHint.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.grp_uartHint.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grp_uartHint.Location = new System.Drawing.Point(12, 296);
            this.grp_uartHint.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grp_uartHint.Name = "grp_uartHint";
            this.grp_uartHint.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grp_uartHint.Size = new System.Drawing.Size(465, 114);
            this.grp_uartHint.TabIndex = 0;
            this.grp_uartHint.TabStop = false;
            this.grp_uartHint.Text = "2.设备连接状态";
            this.grp_uartHint.Enter += new System.EventHandler(this.grp_uartHint_Enter);
            // 
            // lbl_uartstate
            // 
            this.lbl_uartstate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_uartstate.Font = new System.Drawing.Font("华文楷体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_uartstate.ForeColor = System.Drawing.Color.Navy;
            this.lbl_uartstate.Location = new System.Drawing.Point(11, 29);
            this.lbl_uartstate.Name = "lbl_uartstate";
            this.lbl_uartstate.Size = new System.Drawing.Size(431, 71);
            this.lbl_uartstate.TabIndex = 0;
            this.lbl_uartstate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbl_uartstate.Click += new System.EventHandler(this.lbl_uartstate_Click);
            // 
            // lbl_uartstyle
            // 
            this.lbl_uartstyle.AutoSize = true;
            this.lbl_uartstyle.Font = new System.Drawing.Font("华文楷体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_uartstyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lbl_uartstyle.Location = new System.Drawing.Point(180, 63);
            this.lbl_uartstyle.Name = "lbl_uartstyle";
            this.lbl_uartstyle.Size = new System.Drawing.Size(137, 27);
            this.lbl_uartstyle.TabIndex = 1;
            this.lbl_uartstyle.Text = "连接状态：";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(181, 165);
            this.textBox1.MaxLength = 13;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(76, 31);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "02";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(3, 165);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(212, 31);
            this.label1.TabIndex = 4;
            this.label1.Text = "节点软件地址范围:";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.btnCloseUpdate);
            this.groupBox8.Controls.Add(this.button1);
            this.groupBox8.Location = new System.Drawing.Point(17, 27);
            this.groupBox8.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox8.Size = new System.Drawing.Size(550, 50);
            this.groupBox8.TabIndex = 0;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "一键更新";
            // 
            // btnCloseUpdate
            // 
            this.btnCloseUpdate.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.btnCloseUpdate.Location = new System.Drawing.Point(453, 0);
            this.btnCloseUpdate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCloseUpdate.Name = "btnCloseUpdate";
            this.btnCloseUpdate.Size = new System.Drawing.Size(91, 33);
            this.btnCloseUpdate.TabIndex = 8;
            this.btnCloseUpdate.Text = "退出更新";
            this.btnCloseUpdate.UseVisualStyleBackColor = true;
            this.btnCloseUpdate.Click += new System.EventHandler(this.btnCloseUpdate_Click);
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.button1.Location = new System.Drawing.Point(231, -1);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(91, 34);
            this.button1.TabIndex = 9;
            this.button1.Text = "一键更新";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // prg_update1
            // 
            this.prg_update1.Location = new System.Drawing.Point(97, 108);
            this.prg_update1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.prg_update1.Name = "prg_update1";
            this.prg_update1.Size = new System.Drawing.Size(402, 11);
            this.prg_update1.TabIndex = 0;
            // 
            // lbl_progressbar1
            // 
            this.lbl_progressbar1.AutoSize = true;
            this.lbl_progressbar1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_progressbar1.Location = new System.Drawing.Point(518, 100);
            this.lbl_progressbar1.Name = "lbl_progressbar1";
            this.lbl_progressbar1.Size = new System.Drawing.Size(51, 32);
            this.lbl_progressbar1.TabIndex = 1;
            this.lbl_progressbar1.Text = "0%";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.infor_dispaly_button);
            this.groupBox3.Controls.Add(this.txt_updateinfo1);
            this.groupBox3.Location = new System.Drawing.Point(503, 178);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox3.Size = new System.Drawing.Size(574, 652);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "5.更新提示";
            // 
            // infor_dispaly_button
            // 
            this.infor_dispaly_button.Location = new System.Drawing.Point(481, 8);
            this.infor_dispaly_button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.infor_dispaly_button.Name = "infor_dispaly_button";
            this.infor_dispaly_button.Size = new System.Drawing.Size(86, 34);
            this.infor_dispaly_button.TabIndex = 9;
            this.infor_dispaly_button.Text = "暂停";
            this.infor_dispaly_button.UseVisualStyleBackColor = true;
            this.infor_dispaly_button.Click += new System.EventHandler(this.infor_dispaly_button_Click);
            // 
            // txt_updateinfo1
            // 
            this.txt_updateinfo1.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_updateinfo1.Location = new System.Drawing.Point(6, 42);
            this.txt_updateinfo1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_updateinfo1.Multiline = true;
            this.txt_updateinfo1.Name = "txt_updateinfo1";
            this.txt_updateinfo1.ReadOnly = true;
            this.txt_updateinfo1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_updateinfo1.Size = new System.Drawing.Size(561, 594);
            this.txt_updateinfo1.TabIndex = 0;
            this.txt_updateinfo1.TextChanged += new System.EventHandler(this.txt_updateinfo1_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lbl_progressbar1);
            this.groupBox1.Controls.Add(this.prg_update1);
            this.groupBox1.Controls.Add(this.groupBox8);
            this.groupBox1.Location = new System.Drawing.Point(503, 13);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(574, 148);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "4.更新程序";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(17, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 27);
            this.label3.TabIndex = 10;
            this.label3.Text = "更新进度";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.textBox4);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.textBox3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBox2.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.groupBox2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox2.Location = new System.Drawing.Point(12, 13);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Size = new System.Drawing.Size(465, 275);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "1.设备信息填写";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(257, 165);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 31);
            this.label5.TabIndex = 14;
            this.label5.Text = "~~~~~";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(345, 165);
            this.textBox4.MaxLength = 13;
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(76, 31);
            this.textBox4.TabIndex = 13;
            this.textBox4.Text = "02";
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Font = new System.Drawing.Font("华文楷体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Navy;
            this.label2.Location = new System.Drawing.Point(6, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(415, 92);
            this.label2.TabIndex = 7;
            this.label2.Text = "1.请正确填写软件地址和软件版本号。\r\n2.如有修改需求，请使用设备信息更新功能。\r\n3.如目标节点的软件版本与当前节点的软件版本一致，则握手会被拒绝。\r\n";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(129, 209);
            this.textBox3.MaxLength = 8;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(291, 31);
            this.textBox3.TabIndex = 12;
            this.textBox3.Text = "20200430";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(5, 210);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(140, 31);
            this.label4.TabIndex = 11;
            this.label4.Text = "软件版本号:";
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox4.Controls.Add(this.textBox2);
            this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBox4.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.groupBox4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox4.Location = new System.Drawing.Point(12, 418);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox4.Size = new System.Drawing.Size(186, 114);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "已成功烧录设备列表";
            this.groupBox4.Enter += new System.EventHandler(this.groupBox4_Enter);
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox2.Location = new System.Drawing.Point(6, 23);
            this.textBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox2.Size = new System.Drawing.Size(171, 83);
            this.textBox2.TabIndex = 1;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.groupBox6);
            this.groupBox5.Location = new System.Drawing.Point(18, 539);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(459, 291);
            this.groupBox5.TabIndex = 8;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "3.导入Hex文件";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.User_filename1);
            this.groupBox6.Controls.Add(this.groupBox11);
            this.groupBox6.Controls.Add(this.btn_fileopen2);
            this.groupBox6.Location = new System.Drawing.Point(6, 22);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(447, 260);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "User程序1";
            // 
            // User_filename1
            // 
            this.User_filename1.AutoSize = true;
            this.User_filename1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.User_filename1.Location = new System.Drawing.Point(103, 33);
            this.User_filename1.Name = "User_filename1";
            this.User_filename1.Size = new System.Drawing.Size(126, 27);
            this.User_filename1.TabIndex = 7;
            this.User_filename1.Text = "(机器码文件)";
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.lst_codeshow2);
            this.groupBox11.Location = new System.Drawing.Point(6, 68);
            this.groupBox11.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox11.Size = new System.Drawing.Size(488, 185);
            this.groupBox11.TabIndex = 6;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "文件内容";
            // 
            // lst_codeshow2
            // 
            this.lst_codeshow2.FormattingEnabled = true;
            this.lst_codeshow2.ItemHeight = 23;
            this.lst_codeshow2.Location = new System.Drawing.Point(10, 27);
            this.lst_codeshow2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lst_codeshow2.Name = "lst_codeshow2";
            this.lst_codeshow2.Size = new System.Drawing.Size(472, 142);
            this.lst_codeshow2.TabIndex = 0;
            // 
            // btn_fileopen2
            // 
            this.btn_fileopen2.Location = new System.Drawing.Point(6, 26);
            this.btn_fileopen2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_fileopen2.Name = "btn_fileopen2";
            this.btn_fileopen2.Size = new System.Drawing.Size(91, 34);
            this.btn_fileopen2.TabIndex = 1;
            this.btn_fileopen2.Text = "选择文件";
            this.btn_fileopen2.UseVisualStyleBackColor = true;
            this.btn_fileopen2.Click += new System.EventHandler(this.btn_fileopen2_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox7
            // 
            this.groupBox7.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox7.Controls.Add(this.textBox5);
            this.groupBox7.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBox7.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.groupBox7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox7.Location = new System.Drawing.Point(204, 418);
            this.groupBox7.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox7.Size = new System.Drawing.Size(186, 114);
            this.groupBox7.TabIndex = 3;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "烧录失败的设备列表";
            // 
            // textBox5
            // 
            this.textBox5.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox5.Location = new System.Drawing.Point(6, 23);
            this.textBox5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox5.Multiline = true;
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox5.Size = new System.Drawing.Size(171, 83);
            this.textBox5.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(396, 474);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(58, 32);
            this.button2.TabIndex = 9;
            this.button2.Text = "导出";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // frm_UserUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Menu;
            this.ClientSize = new System.Drawing.Size(1089, 836);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grp_uartHint);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_UserUpdate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "金葫芦AHL-GEC-UserUpdate：多用户程序更新";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_UserUpdate_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_UserUpdate_FormClosed);
            this.Load += new System.EventHandler(this.frm_UserUpdate_Load);
            this.grp_uartHint.ResumeLayout(false);
            this.grp_uartHint.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grp_uartHint;
        private System.Windows.Forms.Label lbl_uartstyle;
        private System.Windows.Forms.Label lbl_uartstate;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.ProgressBar prg_update1;
        private System.Windows.Forms.Label lbl_progressbar1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button infor_dispaly_button;
        private System.Windows.Forms.Button btnCloseUpdate;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.ListBox lst_codeshow2;
        private System.Windows.Forms.Button btn_fileopen2;
        private System.Windows.Forms.Label User_filename1;
        public System.Windows.Forms.TextBox txt_updateinfo1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Button button2;
    }
}