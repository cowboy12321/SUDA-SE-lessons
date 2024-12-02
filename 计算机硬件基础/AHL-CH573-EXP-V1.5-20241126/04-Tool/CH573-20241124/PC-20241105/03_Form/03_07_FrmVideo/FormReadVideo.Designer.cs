
namespace AHL_GEC
{
    partial class FormReadVideo
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
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxArmName = new System.Windows.Forms.TextBox();
            this.btnReadArm = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxGenerateDataPath = new System.Windows.Forms.TextBox();
            this.textBoxCurrentState = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(213, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 14);
            this.label4.TabIndex = 32;
            this.label4.Text = "读取的.arm文件名";
            // 
            // textBoxArmName
            // 
            this.textBoxArmName.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxArmName.Location = new System.Drawing.Point(214, 100);
            this.textBoxArmName.Multiline = true;
            this.textBoxArmName.Name = "textBoxArmName";
            this.textBoxArmName.ReadOnly = true;
            this.textBoxArmName.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxArmName.Size = new System.Drawing.Size(491, 26);
            this.textBoxArmName.TabIndex = 31;
            // 
            // btnReadArm
            // 
            this.btnReadArm.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReadArm.Location = new System.Drawing.Point(40, 79);
            this.btnReadArm.Name = "btnReadArm";
            this.btnReadArm.Size = new System.Drawing.Size(116, 47);
            this.btnReadArm.TabIndex = 30;
            this.btnReadArm.Text = "读取.arm文件";
            this.btnReadArm.UseVisualStyleBackColor = true;
            this.btnReadArm.Click += new System.EventHandler(this.btnReadArm_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(213, 164);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(133, 14);
            this.label5.TabIndex = 36;
            this.label5.Text = "生成数据存放的路径";
            // 
            // textBoxGenerateDataPath
            // 
            this.textBoxGenerateDataPath.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxGenerateDataPath.Location = new System.Drawing.Point(214, 198);
            this.textBoxGenerateDataPath.Multiline = true;
            this.textBoxGenerateDataPath.Name = "textBoxGenerateDataPath";
            this.textBoxGenerateDataPath.ReadOnly = true;
            this.textBoxGenerateDataPath.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxGenerateDataPath.Size = new System.Drawing.Size(491, 51);
            this.textBoxGenerateDataPath.TabIndex = 35;
            // 
            // textBoxCurrentState
            // 
            this.textBoxCurrentState.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxCurrentState.Location = new System.Drawing.Point(214, 288);
            this.textBoxCurrentState.Multiline = true;
            this.textBoxCurrentState.Name = "textBoxCurrentState";
            this.textBoxCurrentState.ReadOnly = true;
            this.textBoxCurrentState.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxCurrentState.Size = new System.Drawing.Size(491, 72);
            this.textBoxCurrentState.TabIndex = 38;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("黑体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.Color.Blue;
            this.label8.Location = new System.Drawing.Point(67, 315);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 19);
            this.label8.TabIndex = 37;
            this.label8.Text = "当前状态";
            // 
            // FormReadVideo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(788, 418);
            this.Controls.Add(this.textBoxCurrentState);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxGenerateDataPath);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxArmName);
            this.Controls.Add(this.btnReadArm);
            this.Name = "FormReadVideo";
            this.Text = "灯效文件分割";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxArmName;
        private System.Windows.Forms.Button btnReadArm;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxGenerateDataPath;
        private System.Windows.Forms.TextBox textBoxCurrentState;
        private System.Windows.Forms.Label label8;
    }
}