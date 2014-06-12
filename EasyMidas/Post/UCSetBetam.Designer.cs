namespace EasyMidas.Post
{
    partial class UCSetBetam
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.bt_Close = new System.Windows.Forms.Button();
            this.bt_Accept = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rb_del = new System.Windows.Forms.RadioButton();
            this.rb_add = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_Betamy = new System.Windows.Forms.TextBox();
            this.tb_Betamz = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_Betaty = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_Betatz = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bt_Close
            // 
            this.bt_Close.Location = new System.Drawing.Point(118, 199);
            this.bt_Close.Name = "bt_Close";
            this.bt_Close.Size = new System.Drawing.Size(55, 23);
            this.bt_Close.TabIndex = 14;
            this.bt_Close.Text = "关 闭";
            this.bt_Close.UseVisualStyleBackColor = true;
            this.bt_Close.Click += new System.EventHandler(this.bt_Close_Click);
            // 
            // bt_Accept
            // 
            this.bt_Accept.Location = new System.Drawing.Point(19, 199);
            this.bt_Accept.Name = "bt_Accept";
            this.bt_Accept.Size = new System.Drawing.Size(55, 23);
            this.bt_Accept.TabIndex = 13;
            this.bt_Accept.Text = "适 用";
            this.bt_Accept.UseVisualStyleBackColor = true;
            this.bt_Accept.Click += new System.EventHandler(this.bt_Accept_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tb_Betatz);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.tb_Betaty);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.tb_Betamz);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.tb_Betamy);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(2, 58);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(187, 135);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "构件类型";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rb_del);
            this.groupBox1.Controls.Add(this.rb_add);
            this.groupBox1.Location = new System.Drawing.Point(2, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(187, 45);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选项";
            // 
            // rb_del
            // 
            this.rb_del.AutoSize = true;
            this.rb_del.Location = new System.Drawing.Point(100, 17);
            this.rb_del.Name = "rb_del";
            this.rb_del.Size = new System.Drawing.Size(47, 16);
            this.rb_del.TabIndex = 1;
            this.rb_del.Text = "删除";
            this.rb_del.UseVisualStyleBackColor = true;
            // 
            // rb_add
            // 
            this.rb_add.AutoSize = true;
            this.rb_add.Checked = true;
            this.rb_add.Location = new System.Drawing.Point(17, 17);
            this.rb_add.Name = "rb_add";
            this.rb_add.Size = new System.Drawing.Size(77, 16);
            this.rb_add.TabIndex = 0;
            this.rb_add.TabStop = true;
            this.rb_add.Text = "添加/替换";
            this.rb_add.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Beta_my:";
            // 
            // tb_Betamy
            // 
            this.tb_Betamy.Location = new System.Drawing.Point(74, 14);
            this.tb_Betamy.Name = "tb_Betamy";
            this.tb_Betamy.Size = new System.Drawing.Size(62, 21);
            this.tb_Betamy.TabIndex = 1;
            this.tb_Betamy.Text = "1.0";
            this.tb_Betamy.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tb_Betamz
            // 
            this.tb_Betamz.Location = new System.Drawing.Point(74, 40);
            this.tb_Betamz.Name = "tb_Betamz";
            this.tb_Betamz.Size = new System.Drawing.Size(62, 21);
            this.tb_Betamz.TabIndex = 3;
            this.tb_Betamz.Text = "1.0";
            this.tb_Betamz.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Beta_mz:";
            // 
            // tb_Betaty
            // 
            this.tb_Betaty.Location = new System.Drawing.Point(74, 65);
            this.tb_Betaty.Name = "tb_Betaty";
            this.tb_Betaty.Size = new System.Drawing.Size(62, 21);
            this.tb_Betaty.TabIndex = 5;
            this.tb_Betaty.Text = "1.0";
            this.tb_Betaty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "Beta_ty:";
            // 
            // tb_Betatz
            // 
            this.tb_Betatz.Location = new System.Drawing.Point(74, 92);
            this.tb_Betatz.Name = "tb_Betatz";
            this.tb_Betatz.Size = new System.Drawing.Size(62, 21);
            this.tb_Betatz.TabIndex = 7;
            this.tb_Betatz.Text = "1.0";
            this.tb_Betatz.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "Beta_tz:";
            // 
            // UCSetBetam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bt_Close);
            this.Controls.Add(this.bt_Accept);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "UCSetBetam";
            this.Size = new System.Drawing.Size(190, 243);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bt_Close;
        private System.Windows.Forms.Button bt_Accept;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rb_del;
        private System.Windows.Forms.RadioButton rb_add;
        private System.Windows.Forms.TextBox tb_Betatz;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_Betaty;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_Betamz;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_Betamy;
        private System.Windows.Forms.Label label1;
    }
}
