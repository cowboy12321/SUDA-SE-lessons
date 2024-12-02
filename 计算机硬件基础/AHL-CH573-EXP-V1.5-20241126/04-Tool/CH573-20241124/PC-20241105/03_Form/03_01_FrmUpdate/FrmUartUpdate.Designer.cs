namespace AHL_GEC
{
    partial class frm_uartUpdate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_uartUpdate));
            this.uiGroupBox1 = new Sunny.UI.UIGroupBox();
            this.uiLabel3 = new Sunny.UI.UILabel();
            this.lbl_filename1 = new Sunny.UI.UILabel();
            this.btn_fileopen1 = new Sunny.UI.UISymbolButton();
            this.lst_codeshow1 = new Sunny.UI.UIListBox();
            this.uiGroupBox2 = new Sunny.UI.UIGroupBox();
            this.btn_uartcheck = new Sunny.UI.UISymbolButton();
            this.lbl_uartstate = new Sunny.UI.UILabel();
            this.prg_update1 = new Sunny.UI.UIProcessBar();
            this.btnCloseUpdate = new Sunny.UI.UISymbolButton();
            this.infor_dispaly_button = new Sunny.UI.UISymbolButton();
            this.btn_autoupdate1 = new Sunny.UI.UISymbolButton();
            this.uiGroupBox3 = new Sunny.UI.UIGroupBox();
            this.txt_updateinfo1 = new Sunny.UI.UITextBox();
            this.uiGroupBox4 = new Sunny.UI.UIGroupBox();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.uiGroupBox1.SuspendLayout();
            this.uiGroupBox2.SuspendLayout();
            this.uiGroupBox3.SuspendLayout();
            this.uiGroupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.Controls.Add(this.uiLabel1);
            this.uiGroupBox1.Controls.Add(this.uiLabel3);
            this.uiGroupBox1.Controls.Add(this.lbl_filename1);
            this.uiGroupBox1.Controls.Add(this.btn_fileopen1);
            this.uiGroupBox1.Controls.Add(this.lst_codeshow1);
            this.uiGroupBox1.Font = new System.Drawing.Font("黑体", 15F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox1.ForeColor = System.Drawing.Color.Navy;
            this.uiGroupBox1.Location = new System.Drawing.Point(15, 140);
            this.uiGroupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox1.RectColor = System.Drawing.Color.Navy;
            this.uiGroupBox1.Size = new System.Drawing.Size(502, 393);
            this.uiGroupBox1.Style = Sunny.UI.UIStyle.Custom;
            this.uiGroupBox1.TabIndex = 10;
            this.uiGroupBox1.Text = "导入机器码.Hex文件";
            // 
            // uiLabel3
            // 
            this.uiLabel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.uiLabel3.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiLabel3.Location = new System.Drawing.Point(18, 69);
            this.uiLabel3.Name = "uiLabel3";
            this.uiLabel3.Size = new System.Drawing.Size(138, 31);
            this.uiLabel3.Style = Sunny.UI.UIStyle.Custom;
            this.uiLabel3.TabIndex = 5;
            this.uiLabel3.Text = "机器码文件：";
            this.uiLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_filename1
            // 
            this.lbl_filename1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.lbl_filename1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_filename1.Location = new System.Drawing.Point(162, 69);
            this.lbl_filename1.Name = "lbl_filename1";
            this.lbl_filename1.Size = new System.Drawing.Size(337, 31);
            this.lbl_filename1.Style = Sunny.UI.UIStyle.Custom;
            this.lbl_filename1.TabIndex = 4;
            this.lbl_filename1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btn_fileopen1
            // 
            this.btn_fileopen1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_fileopen1.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_fileopen1.Location = new System.Drawing.Point(306, 30);
            this.btn_fileopen1.MinimumSize = new System.Drawing.Size(1, 1);
            this.btn_fileopen1.Name = "btn_fileopen1";
            this.btn_fileopen1.RectColor = System.Drawing.Color.Maroon;
            this.btn_fileopen1.Size = new System.Drawing.Size(173, 35);
            this.btn_fileopen1.Style = Sunny.UI.UIStyle.Custom;
            this.btn_fileopen1.Symbol = 61462;
            this.btn_fileopen1.SymbolSize = 25;
            this.btn_fileopen1.TabIndex = 3;
            this.btn_fileopen1.Text = "选择文件";
            this.btn_fileopen1.Click += new System.EventHandler(this.btn_fileopen1_Click);
            // 
            // lst_codeshow1
            // 
            this.lst_codeshow1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.lst_codeshow1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(254)))));
            this.lst_codeshow1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lst_codeshow1.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.lst_codeshow1.Location = new System.Drawing.Point(23, 134);
            this.lst_codeshow1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lst_codeshow1.MinimumSize = new System.Drawing.Size(1, 1);
            this.lst_codeshow1.Name = "lst_codeshow1";
            this.lst_codeshow1.Padding = new System.Windows.Forms.Padding(2);
            this.lst_codeshow1.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.lst_codeshow1.Size = new System.Drawing.Size(456, 247);
            this.lst_codeshow1.Style = Sunny.UI.UIStyle.Custom;
            this.lst_codeshow1.TabIndex = 1;
            this.lst_codeshow1.Text = null;
            // 
            // uiGroupBox2
            // 
            this.uiGroupBox2.Controls.Add(this.btn_uartcheck);
            this.uiGroupBox2.Controls.Add(this.lbl_uartstate);
            this.uiGroupBox2.Font = new System.Drawing.Font("黑体", 15F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox2.ForeColor = System.Drawing.Color.Navy;
            this.uiGroupBox2.Location = new System.Drawing.Point(15, 14);
            this.uiGroupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox2.Name = "uiGroupBox2";
            this.uiGroupBox2.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox2.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.uiGroupBox2.RectDisableColor = System.Drawing.Color.Silver;
            this.uiGroupBox2.Size = new System.Drawing.Size(502, 97);
            this.uiGroupBox2.Style = Sunny.UI.UIStyle.Custom;
            this.uiGroupBox2.TabIndex = 11;
            this.uiGroupBox2.Text = "设备连接";
            // 
            // btn_uartcheck
            // 
            this.btn_uartcheck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_uartcheck.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_uartcheck.Location = new System.Drawing.Point(11, 40);
            this.btn_uartcheck.MinimumSize = new System.Drawing.Size(1, 1);
            this.btn_uartcheck.Name = "btn_uartcheck";
            this.btn_uartcheck.RectColor = System.Drawing.Color.Maroon;
            this.btn_uartcheck.Size = new System.Drawing.Size(145, 35);
            this.btn_uartcheck.Style = Sunny.UI.UIStyle.Custom;
            this.btn_uartcheck.Symbol = 61758;
            this.btn_uartcheck.TabIndex = 1;
            this.btn_uartcheck.Text = "连接GEC";
            this.btn_uartcheck.Click += new System.EventHandler(this.btn_uartcheck_Click);
            // 
            // lbl_uartstate
            // 
            this.lbl_uartstate.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_uartstate.Location = new System.Drawing.Point(179, 40);
            this.lbl_uartstate.Name = "lbl_uartstate";
            this.lbl_uartstate.Size = new System.Drawing.Size(320, 35);
            this.lbl_uartstate.Style = Sunny.UI.UIStyle.Custom;
            this.lbl_uartstate.TabIndex = 0;
            this.lbl_uartstate.Text = "初始请单击连接【连接GEC】按钮";
            this.lbl_uartstate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // prg_update1
            // 
            this.prg_update1.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.prg_update1.Location = new System.Drawing.Point(27, 86);
            this.prg_update1.MinimumSize = new System.Drawing.Size(70, 23);
            this.prg_update1.Name = "prg_update1";
            this.prg_update1.Size = new System.Drawing.Size(747, 24);
            this.prg_update1.Style = Sunny.UI.UIStyle.Custom;
            this.prg_update1.TabIndex = 12;
            this.prg_update1.Text = "0.0%";
            // 
            // btnCloseUpdate
            // 
            this.btnCloseUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnCloseUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCloseUpdate.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCloseUpdate.Location = new System.Drawing.Point(558, 35);
            this.btnCloseUpdate.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnCloseUpdate.Name = "btnCloseUpdate";
            this.btnCloseUpdate.RectColor = System.Drawing.Color.Maroon;
            this.btnCloseUpdate.Size = new System.Drawing.Size(216, 40);
            this.btnCloseUpdate.Style = Sunny.UI.UIStyle.Custom;
            this.btnCloseUpdate.Symbol = 61579;
            this.btnCloseUpdate.TabIndex = 14;
            this.btnCloseUpdate.Text = "退出串口更新";
            this.btnCloseUpdate.Click += new System.EventHandler(this.btnCloseUpdate_Click);
            // 
            // infor_dispaly_button
            // 
            this.infor_dispaly_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.infor_dispaly_button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.infor_dispaly_button.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.infor_dispaly_button.Location = new System.Drawing.Point(309, 35);
            this.infor_dispaly_button.MinimumSize = new System.Drawing.Size(1, 1);
            this.infor_dispaly_button.Name = "infor_dispaly_button";
            this.infor_dispaly_button.RectColor = System.Drawing.Color.Red;
            this.infor_dispaly_button.Size = new System.Drawing.Size(162, 40);
            this.infor_dispaly_button.Style = Sunny.UI.UIStyle.Custom;
            this.infor_dispaly_button.Symbol = 61517;
            this.infor_dispaly_button.TabIndex = 15;
            this.infor_dispaly_button.Text = "暂停传输";
            this.infor_dispaly_button.Click += new System.EventHandler(this.infor_dispaly_button_Click);
            // 
            // btn_autoupdate1
            // 
            this.btn_autoupdate1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btn_autoupdate1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_autoupdate1.Enabled = false;
            this.btn_autoupdate1.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_autoupdate1.Location = new System.Drawing.Point(27, 35);
            this.btn_autoupdate1.MinimumSize = new System.Drawing.Size(1, 1);
            this.btn_autoupdate1.Name = "btn_autoupdate1";
            this.btn_autoupdate1.RectColor = System.Drawing.Color.Red;
            this.btn_autoupdate1.Size = new System.Drawing.Size(185, 40);
            this.btn_autoupdate1.Style = Sunny.UI.UIStyle.Custom;
            this.btn_autoupdate1.Symbol = 61473;
            this.btn_autoupdate1.TabIndex = 16;
            this.btn_autoupdate1.Text = "一键自动更新";
            this.btn_autoupdate1.Click += new System.EventHandler(this.btn_autoupdate1_Click);
            // 
            // uiGroupBox3
            // 
            this.uiGroupBox3.Controls.Add(this.txt_updateinfo1);
            this.uiGroupBox3.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.uiGroupBox3.Font = new System.Drawing.Font("黑体", 15F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox3.ForeColor = System.Drawing.Color.Navy;
            this.uiGroupBox3.Location = new System.Drawing.Point(563, 140);
            this.uiGroupBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox3.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox3.Name = "uiGroupBox3";
            this.uiGroupBox3.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox3.RectColor = System.Drawing.Color.Navy;
            this.uiGroupBox3.Size = new System.Drawing.Size(807, 393);
            this.uiGroupBox3.Style = Sunny.UI.UIStyle.Custom;
            this.uiGroupBox3.TabIndex = 17;
            this.uiGroupBox3.Text = "更新与运行提示信息";
            // 
            // txt_updateinfo1
            // 
            this.txt_updateinfo1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txt_updateinfo1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.txt_updateinfo1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txt_updateinfo1.Location = new System.Drawing.Point(45, 37);
            this.txt_updateinfo1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txt_updateinfo1.Maximum = 2147483647D;
            this.txt_updateinfo1.Minimum = -2147483648D;
            this.txt_updateinfo1.MinimumSize = new System.Drawing.Size(1, 1);
            this.txt_updateinfo1.Multiline = true;
            this.txt_updateinfo1.Name = "txt_updateinfo1";
            this.txt_updateinfo1.Padding = new System.Windows.Forms.Padding(5);
            this.txt_updateinfo1.Radius = 10;
            this.txt_updateinfo1.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.txt_updateinfo1.Size = new System.Drawing.Size(733, 344);
            this.txt_updateinfo1.Style = Sunny.UI.UIStyle.Custom;
            this.txt_updateinfo1.TabIndex = 0;
            // 
            // uiGroupBox4
            // 
            this.uiGroupBox4.Controls.Add(this.prg_update1);
            this.uiGroupBox4.Controls.Add(this.btnCloseUpdate);
            this.uiGroupBox4.Controls.Add(this.infor_dispaly_button);
            this.uiGroupBox4.Controls.Add(this.btn_autoupdate1);
            this.uiGroupBox4.Font = new System.Drawing.Font("黑体", 15F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox4.ForeColor = System.Drawing.Color.Navy;
            this.uiGroupBox4.Location = new System.Drawing.Point(567, 14);
            this.uiGroupBox4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox4.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox4.Name = "uiGroupBox4";
            this.uiGroupBox4.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox4.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.uiGroupBox4.RectDisableColor = System.Drawing.Color.Silver;
            this.uiGroupBox4.Size = new System.Drawing.Size(799, 116);
            this.uiGroupBox4.Style = Sunny.UI.UIStyle.Custom;
            this.uiGroupBox4.TabIndex = 19;
            this.uiGroupBox4.Text = "更新操作与进度提示";
            // 
            // uiLabel1
            // 
            this.uiLabel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.uiLabel1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiLabel1.Location = new System.Drawing.Point(18, 32);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(216, 31);
            this.uiLabel1.Style = Sunny.UI.UIStyle.Custom;
            this.uiLabel1.TabIndex = 6;
            this.uiLabel1.Text = "请选择机器码Hex文件";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frm_uartUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(254)))));
            this.ClientSize = new System.Drawing.Size(1407, 547);
            this.Controls.Add(this.uiGroupBox4);
            this.Controls.Add(this.uiGroupBox3);
            this.Controls.Add(this.uiGroupBox2);
            this.Controls.Add(this.uiGroupBox1);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_uartUpdate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "金葫芦AHL-GEC-IDE：串口更新（程序下载与运行）";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_uartUpdate_FormClosed);
            this.Load += new System.EventHandler(this.frm_uartUpdate_Load);
            this.uiGroupBox1.ResumeLayout(false);
            this.uiGroupBox2.ResumeLayout(false);
            this.uiGroupBox3.ResumeLayout(false);
            this.uiGroupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Sunny.UI.UIGroupBox uiGroupBox1;
        private Sunny.UI.UISymbolButton btn_fileopen1;
        private Sunny.UI.UIListBox lst_codeshow1;
        private Sunny.UI.UILabel uiLabel3;
        private Sunny.UI.UILabel lbl_filename1;
        private Sunny.UI.UIGroupBox uiGroupBox2;
        private Sunny.UI.UISymbolButton btn_uartcheck;
        private Sunny.UI.UILabel lbl_uartstate;
        private Sunny.UI.UIProcessBar prg_update1;
        private Sunny.UI.UISymbolButton btnCloseUpdate;
        private Sunny.UI.UISymbolButton infor_dispaly_button;
        private Sunny.UI.UISymbolButton btn_autoupdate1;
        private Sunny.UI.UIGroupBox uiGroupBox3;
        private Sunny.UI.UITextBox txt_updateinfo1;
        private Sunny.UI.UIGroupBox uiGroupBox4;
        private Sunny.UI.UILabel uiLabel1;
    }
}