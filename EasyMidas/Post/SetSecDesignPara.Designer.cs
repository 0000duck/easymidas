namespace EasyMidas.Post
{
    partial class SetSecDesignPara
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
            this.gb_Para = new System.Windows.Forms.GroupBox();
            this.tb_Gamay = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_Gamax = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cb_SecCat_y = new System.Windows.Forms.ComboBox();
            this.cb_Closed = new System.Windows.Forms.CheckBox();
            this.tb_ratioNet = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bt_Enter = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.cb_secs = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rb_dele = new System.Windows.Forms.RadioButton();
            this.rb_add = new System.Windows.Forms.RadioButton();
            this.tb_Close = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.cb_SecCat_z = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.gb_Para.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gb_Para
            // 
            this.gb_Para.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_Para.BackColor = System.Drawing.SystemColors.Control;
            this.gb_Para.Controls.Add(this.label7);
            this.gb_Para.Controls.Add(this.label6);
            this.gb_Para.Controls.Add(this.cb_SecCat_z);
            this.gb_Para.Controls.Add(this.tb_Gamay);
            this.gb_Para.Controls.Add(this.label4);
            this.gb_Para.Controls.Add(this.tb_Gamax);
            this.gb_Para.Controls.Add(this.label3);
            this.gb_Para.Controls.Add(this.label2);
            this.gb_Para.Controls.Add(this.cb_SecCat_y);
            this.gb_Para.Controls.Add(this.cb_Closed);
            this.gb_Para.Controls.Add(this.tb_ratioNet);
            this.gb_Para.Controls.Add(this.label1);
            this.gb_Para.Location = new System.Drawing.Point(12, 94);
            this.gb_Para.Name = "gb_Para";
            this.gb_Para.Size = new System.Drawing.Size(379, 107);
            this.gb_Para.TabIndex = 1;
            this.gb_Para.TabStop = false;
            this.gb_Para.Text = "截面设计相关参数";
            // 
            // tb_Gamay
            // 
            this.tb_Gamay.Location = new System.Drawing.Point(304, 72);
            this.tb_Gamay.Name = "tb_Gamay";
            this.tb_Gamay.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tb_Gamay.Size = new System.Drawing.Size(42, 21);
            this.tb_Gamay.TabIndex = 17;
            this.tb_Gamay.Text = "1.05";
            this.tb_Gamay.Leave += new System.EventHandler(this.ParaModified);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(191, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 12);
            this.label4.TabIndex = 16;
            this.label4.Text = "塑性发展系数-γy:";
            // 
            // tb_Gamax
            // 
            this.tb_Gamax.Location = new System.Drawing.Point(128, 72);
            this.tb_Gamax.Name = "tb_Gamax";
            this.tb_Gamax.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tb_Gamax.Size = new System.Drawing.Size(42, 21);
            this.tb_Gamax.TabIndex = 15;
            this.tb_Gamax.Text = "1.05";
            this.tb_Gamax.Leave += new System.EventHandler(this.ParaModified);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 12);
            this.label3.TabIndex = 14;
            this.label3.Text = "塑性发展系数-γx:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "截面类别:";
            // 
            // cb_SecCat_y
            // 
            this.cb_SecCat_y.DropDownHeight = 300;
            this.cb_SecCat_y.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_SecCat_y.FormattingEnabled = true;
            this.cb_SecCat_y.IntegralHeight = false;
            this.cb_SecCat_y.Items.AddRange(new object[] {
            "a类",
            "b类",
            "c类",
            "d类"});
            this.cb_SecCat_y.Location = new System.Drawing.Point(117, 46);
            this.cb_SecCat_y.Name = "cb_SecCat_y";
            this.cb_SecCat_y.Size = new System.Drawing.Size(55, 20);
            this.cb_SecCat_y.TabIndex = 13;
            this.cb_SecCat_y.Leave += new System.EventHandler(this.ParaModified);
            // 
            // cb_Closed
            // 
            this.cb_Closed.AutoSize = true;
            this.cb_Closed.Location = new System.Drawing.Point(278, 19);
            this.cb_Closed.Name = "cb_Closed";
            this.cb_Closed.Size = new System.Drawing.Size(72, 16);
            this.cb_Closed.TabIndex = 11;
            this.cb_Closed.Text = "闭口截面";
            this.cb_Closed.UseVisualStyleBackColor = true;
            this.cb_Closed.Leave += new System.EventHandler(this.ParaModified);
            // 
            // tb_ratioNet
            // 
            this.tb_ratioNet.Location = new System.Drawing.Point(92, 16);
            this.tb_ratioNet.Name = "tb_ratioNet";
            this.tb_ratioNet.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tb_ratioNet.Size = new System.Drawing.Size(78, 21);
            this.tb_ratioNet.TabIndex = 10;
            this.tb_ratioNet.Text = "0.9";
            this.tb_ratioNet.Leave += new System.EventHandler(this.ParaModified);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "净毛面积比:";
            // 
            // bt_Enter
            // 
            this.bt_Enter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Enter.Location = new System.Drawing.Point(219, 207);
            this.bt_Enter.Name = "bt_Enter";
            this.bt_Enter.Size = new System.Drawing.Size(75, 25);
            this.bt_Enter.TabIndex = 10;
            this.bt_Enter.Text = "适 用";
            this.bt_Enter.UseVisualStyleBackColor = true;
            this.bt_Enter.Click += new System.EventHandler(this.bt_Enter_Click);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(12, 71);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(59, 12);
            this.label20.TabIndex = 5;
            this.label20.Text = "截面类型:";
            // 
            // cb_secs
            // 
            this.cb_secs.DropDownHeight = 300;
            this.cb_secs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_secs.FormattingEnabled = true;
            this.cb_secs.IntegralHeight = false;
            this.cb_secs.Location = new System.Drawing.Point(77, 68);
            this.cb_secs.Name = "cb_secs";
            this.cb_secs.Size = new System.Drawing.Size(175, 20);
            this.cb_secs.TabIndex = 8;
            this.cb_secs.SelectedIndexChanged += new System.EventHandler(this.cb_secs_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.rb_dele);
            this.groupBox1.Controls.Add(this.rb_add);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(379, 45);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选项";
            // 
            // rb_dele
            // 
            this.rb_dele.AutoSize = true;
            this.rb_dele.Location = new System.Drawing.Point(151, 20);
            this.rb_dele.Name = "rb_dele";
            this.rb_dele.Size = new System.Drawing.Size(47, 16);
            this.rb_dele.TabIndex = 1;
            this.rb_dele.Text = "删除";
            this.rb_dele.UseVisualStyleBackColor = true;
            // 
            // rb_add
            // 
            this.rb_add.AutoSize = true;
            this.rb_add.Checked = true;
            this.rb_add.Location = new System.Drawing.Point(17, 20);
            this.rb_add.Name = "rb_add";
            this.rb_add.Size = new System.Drawing.Size(77, 16);
            this.rb_add.TabIndex = 0;
            this.rb_add.TabStop = true;
            this.rb_add.Text = "添加/替换";
            this.rb_add.UseVisualStyleBackColor = true;
            // 
            // tb_Close
            // 
            this.tb_Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_Close.Location = new System.Drawing.Point(314, 207);
            this.tb_Close.Name = "tb_Close";
            this.tb_Close.Size = new System.Drawing.Size(75, 25);
            this.tb_Close.TabIndex = 11;
            this.tb_Close.Text = "关 闭";
            this.tb_Close.UseVisualStyleBackColor = true;
            this.tb_Close.Click += new System.EventHandler(this.tb_Close_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 215);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(197, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "说明：本参数依据GB50017-2003设置";
            // 
            // cb_SecCat_z
            // 
            this.cb_SecCat_z.DropDownHeight = 300;
            this.cb_SecCat_z.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_SecCat_z.FormattingEnabled = true;
            this.cb_SecCat_z.IntegralHeight = false;
            this.cb_SecCat_z.Items.AddRange(new object[] {
            "a类",
            "b类",
            "c类",
            "d类"});
            this.cb_SecCat_z.Location = new System.Drawing.Point(235, 46);
            this.cb_SecCat_z.Name = "cb_SecCat_z";
            this.cb_SecCat_z.Size = new System.Drawing.Size(55, 20);
            this.cb_SecCat_z.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(78, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 12);
            this.label6.TabIndex = 18;
            this.label6.Text = "沿x轴";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(192, 51);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 12);
            this.label7.TabIndex = 19;
            this.label7.Text = "沿y轴";
            // 
            // SetSecDesignPara
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 239);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tb_Close);
            this.Controls.Add(this.bt_Enter);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gb_Para);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.cb_secs);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetSecDesignPara";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "指定查询截面设计参数";
            this.gb_Para.ResumeLayout(false);
            this.gb_Para.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gb_Para;
        private System.Windows.Forms.Button bt_Enter;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.ComboBox cb_secs;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rb_dele;
        private System.Windows.Forms.RadioButton rb_add;
        private System.Windows.Forms.Button tb_Close;
        private System.Windows.Forms.CheckBox cb_Closed;
        private System.Windows.Forms.TextBox tb_ratioNet;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cb_SecCat_y;
        private System.Windows.Forms.TextBox tb_Gamay;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_Gamax;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cb_SecCat_z;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;

    }
}