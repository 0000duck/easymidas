﻿namespace EasyMidas.Post
{
    partial class UCSetQuakeAdjustFator
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
            this.tb_V_LC = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_M_LC = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_N_LC = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rb_del = new System.Windows.Forms.RadioButton();
            this.rb_add = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_V_Com = new System.Windows.Forms.TextBox();
            this.tb_N_Com = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_M_Com = new System.Windows.Forms.TextBox();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // bt_Close
            // 
            this.bt_Close.Location = new System.Drawing.Point(118, 319);
            this.bt_Close.Name = "bt_Close";
            this.bt_Close.Size = new System.Drawing.Size(55, 23);
            this.bt_Close.TabIndex = 14;
            this.bt_Close.Text = "关 闭";
            this.bt_Close.UseVisualStyleBackColor = true;
            this.bt_Close.Click += new System.EventHandler(this.bt_Close_Click);
            // 
            // bt_Accept
            // 
            this.bt_Accept.Location = new System.Drawing.Point(19, 319);
            this.bt_Accept.Name = "bt_Accept";
            this.bt_Accept.Size = new System.Drawing.Size(55, 23);
            this.bt_Accept.TabIndex = 13;
            this.bt_Accept.Text = "适 用";
            this.bt_Accept.UseVisualStyleBackColor = true;
            this.bt_Accept.Click += new System.EventHandler(this.bt_Accept_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Location = new System.Drawing.Point(2, 58);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(187, 251);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "地震作用调整系数";
            // 
            // tb_V_LC
            // 
            this.tb_V_LC.Location = new System.Drawing.Point(77, 73);
            this.tb_V_LC.Name = "tb_V_LC";
            this.tb_V_LC.Size = new System.Drawing.Size(62, 21);
            this.tb_V_LC.TabIndex = 5;
            this.tb_V_LC.Text = "1.0";
            this.tb_V_LC.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "剪力:";
            // 
            // tb_M_LC
            // 
            this.tb_M_LC.Location = new System.Drawing.Point(77, 48);
            this.tb_M_LC.Name = "tb_M_LC";
            this.tb_M_LC.Size = new System.Drawing.Size(62, 21);
            this.tb_M_LC.TabIndex = 3;
            this.tb_M_LC.Text = "1.0";
            this.tb_M_LC.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "弯矩:";
            // 
            // tb_N_LC
            // 
            this.tb_N_LC.Location = new System.Drawing.Point(77, 22);
            this.tb_N_LC.Name = "tb_N_LC";
            this.tb_N_LC.Size = new System.Drawing.Size(62, 21);
            this.tb_N_LC.TabIndex = 1;
            this.tb_N_LC.Text = "1.0";
            this.tb_N_LC.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "轴力:";
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
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.tb_V_LC);
            this.groupBox3.Controls.Add(this.tb_N_LC);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.tb_M_LC);
            this.groupBox3.Location = new System.Drawing.Point(6, 20);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(175, 109);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "用于地震作用工况【组合前】";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.tb_V_Com);
            this.groupBox4.Controls.Add(this.tb_N_Com);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.tb_M_Com);
            this.groupBox4.Location = new System.Drawing.Point(6, 135);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(175, 109);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "用于地震组合【组合后】";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "轴力:";
            // 
            // tb_V_Com
            // 
            this.tb_V_Com.Location = new System.Drawing.Point(77, 73);
            this.tb_V_Com.Name = "tb_V_Com";
            this.tb_V_Com.Size = new System.Drawing.Size(62, 21);
            this.tb_V_Com.TabIndex = 5;
            this.tb_V_Com.Text = "1.0";
            this.tb_V_Com.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tb_N_Com
            // 
            this.tb_N_Com.Location = new System.Drawing.Point(77, 22);
            this.tb_N_Com.Name = "tb_N_Com";
            this.tb_N_Com.Size = new System.Drawing.Size(62, 21);
            this.tb_N_Com.TabIndex = 1;
            this.tb_N_Com.Text = "1.0";
            this.tb_N_Com.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "剪力:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 51);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "弯矩:";
            // 
            // tb_M_Com
            // 
            this.tb_M_Com.Location = new System.Drawing.Point(77, 48);
            this.tb_M_Com.Name = "tb_M_Com";
            this.tb_M_Com.Size = new System.Drawing.Size(62, 21);
            this.tb_M_Com.TabIndex = 3;
            this.tb_M_Com.Text = "1.0";
            this.tb_M_Com.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // UCSetQuakeAdjustFator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bt_Close);
            this.Controls.Add(this.bt_Accept);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "UCSetQuakeAdjustFator";
            this.Size = new System.Drawing.Size(190, 347);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bt_Close;
        private System.Windows.Forms.Button bt_Accept;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rb_del;
        private System.Windows.Forms.RadioButton rb_add;
        private System.Windows.Forms.TextBox tb_V_LC;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_M_LC;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_N_LC;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_V_Com;
        private System.Windows.Forms.TextBox tb_N_Com;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_M_Com;
        private System.Windows.Forms.GroupBox groupBox3;
    }
}
