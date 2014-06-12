namespace EasyMidas
{
    partial class CheckElemForm
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
            this.tb_Elems = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_isdizhen = new System.Windows.Forms.CheckBox();
            this.bt_GoCheck = new System.Windows.Forms.Button();
            this.lb_out = new System.Windows.Forms.Label();
            this.bt_CheckGo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tb_Elems
            // 
            this.tb_Elems.Location = new System.Drawing.Point(72, 12);
            this.tb_Elems.Name = "tb_Elems";
            this.tb_Elems.Size = new System.Drawing.Size(298, 21);
            this.tb_Elems.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "单元：";
            // 
            // cb_isdizhen
            // 
            this.cb_isdizhen.AutoSize = true;
            this.cb_isdizhen.Location = new System.Drawing.Point(72, 58);
            this.cb_isdizhen.Name = "cb_isdizhen";
            this.cb_isdizhen.Size = new System.Drawing.Size(96, 16);
            this.cb_isdizhen.TabIndex = 2;
            this.cb_isdizhen.Text = "验算抗震组合";
            this.cb_isdizhen.UseVisualStyleBackColor = true;
            // 
            // bt_GoCheck
            // 
            this.bt_GoCheck.Location = new System.Drawing.Point(72, 89);
            this.bt_GoCheck.Name = "bt_GoCheck";
            this.bt_GoCheck.Size = new System.Drawing.Size(75, 23);
            this.bt_GoCheck.TabIndex = 3;
            this.bt_GoCheck.Text = "开始验算";
            this.bt_GoCheck.UseVisualStyleBackColor = true;
            this.bt_GoCheck.Click += new System.EventHandler(this.bt_GoCheck_Click);
            // 
            // lb_out
            // 
            this.lb_out.AutoSize = true;
            this.lb_out.Location = new System.Drawing.Point(25, 130);
            this.lb_out.Name = "lb_out";
            this.lb_out.Size = new System.Drawing.Size(41, 12);
            this.lb_out.TabIndex = 4;
            this.lb_out.Text = "提示：";
            // 
            // bt_CheckGo
            // 
            this.bt_CheckGo.Location = new System.Drawing.Point(179, 88);
            this.bt_CheckGo.Name = "bt_CheckGo";
            this.bt_CheckGo.Size = new System.Drawing.Size(133, 23);
            this.bt_CheckGo.TabIndex = 5;
            this.bt_CheckGo.Text = "测试：新的验算";
            this.bt_CheckGo.UseVisualStyleBackColor = true;
            this.bt_CheckGo.Click += new System.EventHandler(this.bt_CheckGo_Click);
            // 
            // CheckElemForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 151);
            this.Controls.Add(this.bt_CheckGo);
            this.Controls.Add(this.lb_out);
            this.Controls.Add(this.bt_GoCheck);
            this.Controls.Add(this.cb_isdizhen);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_Elems);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CheckElemForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "验算指定单元";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_Elems;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cb_isdizhen;
        private System.Windows.Forms.Button bt_GoCheck;
        private System.Windows.Forms.Label lb_out;
        private System.Windows.Forms.Button bt_CheckGo;
    }
}