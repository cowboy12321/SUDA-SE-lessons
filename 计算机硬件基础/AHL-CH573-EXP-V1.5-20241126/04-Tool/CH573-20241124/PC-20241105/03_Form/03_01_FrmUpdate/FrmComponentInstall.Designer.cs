namespace AHL_GEC
{
    partial class frm_uartDynamic
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
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btn_fileopen1 = new System.Windows.Forms.Button();
            this.btnCloseUpdate = new System.Windows.Forms.Button();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btn_autoupdate1 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txt_updateinfo1 = new System.Windows.Forms.TextBox();
            this.grp_uartHint = new System.Windows.Forms.GroupBox();
            this.lbl_uartstate = new System.Windows.Forms.Label();
            this.btn_uartcheck = new System.Windows.Forms.Button();
            this.lbl_uartstyle = new System.Windows.Forms.Label();
            this.groupBox8.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.grp_uartHint.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.button2);
            this.groupBox8.Controls.Add(this.comboBox1);
            this.groupBox8.Controls.Add(this.btn_fileopen1);
            this.groupBox8.Location = new System.Drawing.Point(28, 104);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(476, 103);
            this.groupBox8.TabIndex = 15;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "选择文件";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(138, 45);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(109, 37);
            this.button2.TabIndex = 12;
            this.button2.Text = "添加构件";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(11, 45);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 25);
            this.comboBox1.TabIndex = 1;
            // 
            // btn_fileopen1
            // 
            this.btn_fileopen1.Location = new System.Drawing.Point(6, 0);
            this.btn_fileopen1.Name = "btn_fileopen1";
            this.btn_fileopen1.Size = new System.Drawing.Size(80, 29);
            this.btn_fileopen1.TabIndex = 0;
            this.btn_fileopen1.Text = "选择lst文件";
            this.btn_fileopen1.UseVisualStyleBackColor = true;
            this.btn_fileopen1.Click += new System.EventHandler(this.btn_fileopen1_Click_1);
            // 
            // btnCloseUpdate
            // 
            this.btnCloseUpdate.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCloseUpdate.Location = new System.Drawing.Point(803, 104);
            this.btnCloseUpdate.Name = "btnCloseUpdate";
            this.btnCloseUpdate.Size = new System.Drawing.Size(117, 37);
            this.btnCloseUpdate.TabIndex = 14;
            this.btnCloseUpdate.Text = "退出动态装载";
            this.btnCloseUpdate.UseVisualStyleBackColor = true;
            this.btnCloseUpdate.Click += new System.EventHandler(this.btnCloseUpdate_Click);
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.textBox1);
            this.groupBox9.Location = new System.Drawing.Point(28, 213);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(476, 245);
            this.groupBox9.TabIndex = 12;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "命令帧：";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(11, 17);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(459, 222);
            this.textBox1.TabIndex = 0;
            // 
            // btn_autoupdate1
            // 
            this.btn_autoupdate1.Enabled = false;
            this.btn_autoupdate1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_autoupdate1.Location = new System.Drawing.Point(551, 104);
            this.btn_autoupdate1.Name = "btn_autoupdate1";
            this.btn_autoupdate1.Size = new System.Drawing.Size(117, 37);
            this.btn_autoupdate1.TabIndex = 11;
            this.btn_autoupdate1.Text = "一键自动装载";
            this.btn_autoupdate1.UseVisualStyleBackColor = true;
            this.btn_autoupdate1.Click += new System.EventHandler(this.btn_autoupdate1_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txt_updateinfo1);
            this.groupBox3.Location = new System.Drawing.Point(510, 149);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(432, 308);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "装载提示";
            // 
            // txt_updateinfo1
            // 
            this.txt_updateinfo1.Location = new System.Drawing.Point(6, 38);
            this.txt_updateinfo1.Multiline = true;
            this.txt_updateinfo1.Name = "txt_updateinfo1";
            this.txt_updateinfo1.ReadOnly = true;
            this.txt_updateinfo1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_updateinfo1.Size = new System.Drawing.Size(420, 213);
            this.txt_updateinfo1.TabIndex = 0;
            // 
            // grp_uartHint
            // 
            this.grp_uartHint.BackColor = System.Drawing.Color.Linen;
            this.grp_uartHint.Controls.Add(this.lbl_uartstate);
            this.grp_uartHint.Controls.Add(this.btn_uartcheck);
            this.grp_uartHint.Controls.Add(this.lbl_uartstyle);
            this.grp_uartHint.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.grp_uartHint.Font = new System.Drawing.Font("黑体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grp_uartHint.ForeColor = System.Drawing.Color.Red;
            this.grp_uartHint.Location = new System.Drawing.Point(12, 20);
            this.grp_uartHint.Name = "grp_uartHint";
            this.grp_uartHint.Size = new System.Drawing.Size(940, 78);
            this.grp_uartHint.TabIndex = 10;
            this.grp_uartHint.TabStop = false;
            this.grp_uartHint.Text = "设备连接";
            // 
            // lbl_uartstate
            // 
            this.lbl_uartstate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_uartstate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_uartstate.ForeColor = System.Drawing.Color.Navy;
            this.lbl_uartstate.Location = new System.Drawing.Point(375, 27);
            this.lbl_uartstate.Name = "lbl_uartstate";
            this.lbl_uartstate.Size = new System.Drawing.Size(533, 39);
            this.lbl_uartstate.TabIndex = 0;
            this.lbl_uartstate.Text = "初始请单击连接【连接GEC】按钮";
            this.lbl_uartstate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btn_uartcheck
            // 
            this.btn_uartcheck.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btn_uartcheck.Location = new System.Drawing.Point(10, 28);
            this.btn_uartcheck.Name = "btn_uartcheck";
            this.btn_uartcheck.Size = new System.Drawing.Size(243, 37);
            this.btn_uartcheck.TabIndex = 1;
            this.btn_uartcheck.Text = "连接GEC并检测容量";
            this.btn_uartcheck.UseVisualStyleBackColor = true;
            this.btn_uartcheck.Click += new System.EventHandler(this.btn_uartcheck_Click);
            // 
            // lbl_uartstyle
            // 
            this.lbl_uartstyle.AutoSize = true;
            this.lbl_uartstyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_uartstyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lbl_uartstyle.Location = new System.Drawing.Point(259, 37);
            this.lbl_uartstyle.Name = "lbl_uartstyle";
            this.lbl_uartstyle.Size = new System.Drawing.Size(110, 24);
            this.lbl_uartstyle.TabIndex = 1;
            this.lbl_uartstyle.Text = "连接状态：";
            // 
            // frm_uartDynamic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Menu;
            this.ClientSize = new System.Drawing.Size(964, 478);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.btnCloseUpdate);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.btn_autoupdate1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.grp_uartHint);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_uartDynamic";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "构件固化";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_uartDynamic_FormClosed);
            this.Load += new System.EventHandler(this.frm_uartDynamic_Load);
            this.groupBox8.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.grp_uartHint.ResumeLayout(false);
            this.grp_uartHint.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btn_fileopen1;
        private System.Windows.Forms.Button btnCloseUpdate;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btn_autoupdate1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txt_updateinfo1;
        private System.Windows.Forms.GroupBox grp_uartHint;
        private System.Windows.Forms.Label lbl_uartstate;
        private System.Windows.Forms.Button btn_uartcheck;
        private System.Windows.Forms.Label lbl_uartstyle;

    }
}