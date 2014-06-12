namespace EasyMidas.Post
{
    partial class QueryResults
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
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.bt_findRaio = new System.Windows.Forms.Button();
            this.rtb_Messagebox = new System.Windows.Forms.RichTextBox();
            this.tb_R_max = new System.Windows.Forms.TextBox();
            this.tb_R_min = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.cb_secs2 = new System.Windows.Forms.ComboBox();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.bt_findRaio);
            this.groupBox5.Controls.Add(this.rtb_Messagebox);
            this.groupBox5.Controls.Add(this.tb_R_max);
            this.groupBox5.Controls.Add(this.tb_R_min);
            this.groupBox5.Controls.Add(this.label20);
            this.groupBox5.Controls.Add(this.label19);
            this.groupBox5.Controls.Add(this.cb_secs2);
            this.groupBox5.Location = new System.Drawing.Point(12, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(267, 406);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "验算结果查询";
            // 
            // bt_findRaio
            // 
            this.bt_findRaio.Location = new System.Drawing.Point(30, 96);
            this.bt_findRaio.Name = "bt_findRaio";
            this.bt_findRaio.Size = new System.Drawing.Size(75, 23);
            this.bt_findRaio.TabIndex = 10;
            this.bt_findRaio.Text = "开始查询";
            this.bt_findRaio.UseVisualStyleBackColor = true;
            this.bt_findRaio.Click += new System.EventHandler(this.bt_findRaio_Click);
            // 
            // rtb_Messagebox
            // 
            this.rtb_Messagebox.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.rtb_Messagebox.Location = new System.Drawing.Point(6, 133);
            this.rtb_Messagebox.Name = "rtb_Messagebox";
            this.rtb_Messagebox.Size = new System.Drawing.Size(255, 262);
            this.rtb_Messagebox.TabIndex = 9;
            this.rtb_Messagebox.Text = "";
            // 
            // tb_R_max
            // 
            this.tb_R_max.Location = new System.Drawing.Point(177, 28);
            this.tb_R_max.Name = "tb_R_max";
            this.tb_R_max.Size = new System.Drawing.Size(42, 21);
            this.tb_R_max.TabIndex = 0;
            this.tb_R_max.Text = "1.5";
            // 
            // tb_R_min
            // 
            this.tb_R_min.Location = new System.Drawing.Point(109, 28);
            this.tb_R_min.Name = "tb_R_min";
            this.tb_R_min.Size = new System.Drawing.Size(42, 21);
            this.tb_R_min.TabIndex = 0;
            this.tb_R_min.Text = "0.9";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(28, 62);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(53, 12);
            this.label20.TabIndex = 5;
            this.label20.Text = "截面类型";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(28, 31);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(65, 12);
            this.label19.TabIndex = 5;
            this.label19.Text = "应力比范围";
            // 
            // cb_secs2
            // 
            this.cb_secs2.DropDownHeight = 300;
            this.cb_secs2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_secs2.FormattingEnabled = true;
            this.cb_secs2.IntegralHeight = false;
            this.cb_secs2.Location = new System.Drawing.Point(109, 59);
            this.cb_secs2.Name = "cb_secs2";
            this.cb_secs2.Size = new System.Drawing.Size(121, 20);
            this.cb_secs2.TabIndex = 8;
            // 
            // QueryResults
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 431);
            this.Controls.Add(this.groupBox5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QueryResults";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "单位验算结果查询";
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button bt_findRaio;
        private System.Windows.Forms.RichTextBox rtb_Messagebox;
        private System.Windows.Forms.TextBox tb_R_max;
        private System.Windows.Forms.TextBox tb_R_min;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.ComboBox cb_secs2;

    }
}