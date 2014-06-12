namespace EasyMidas
{
    partial class Import3D3SExcelFiles
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
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.bt_run = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.rb_m = new System.Windows.Forms.RadioButton();
            this.rb_cm = new System.Windows.Forms.RadioButton();
            this.rb_mm = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(316, 39);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "指定文件";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.ForeColor = System.Drawing.Color.Purple;
            this.textBox1.Location = new System.Drawing.Point(14, 39);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(296, 21);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "在此输入文件绝对路径";
            // 
            // textBox2
            // 
            this.textBox2.ForeColor = System.Drawing.Color.Violet;
            this.textBox2.Location = new System.Drawing.Point(14, 85);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(296, 21);
            this.textBox2.TabIndex = 2;
            this.textBox2.Text = "在此输入文件绝对路径";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(316, 85);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "指定文件";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // bt_run
            // 
            this.bt_run.Location = new System.Drawing.Point(14, 142);
            this.bt_run.Name = "bt_run";
            this.bt_run.Size = new System.Drawing.Size(75, 23);
            this.bt_run.TabIndex = 4;
            this.bt_run.Text = "开始读取";
            this.bt_run.UseVisualStyleBackColor = true;
            this.bt_run.Click += new System.EventHandler(this.bt_run_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "节点信息文件(*.xsl)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "单元信息文件(*.xsl)";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(316, 142);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 7;
            this.button3.Text = "关 闭";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // rb_m
            // 
            this.rb_m.AutoSize = true;
            this.rb_m.Location = new System.Drawing.Point(87, 116);
            this.rb_m.Name = "rb_m";
            this.rb_m.Size = new System.Drawing.Size(35, 16);
            this.rb_m.TabIndex = 8;
            this.rb_m.Text = "米";
            this.rb_m.UseVisualStyleBackColor = true;
            // 
            // rb_cm
            // 
            this.rb_cm.AutoSize = true;
            this.rb_cm.Location = new System.Drawing.Point(152, 116);
            this.rb_cm.Name = "rb_cm";
            this.rb_cm.Size = new System.Drawing.Size(47, 16);
            this.rb_cm.TabIndex = 9;
            this.rb_cm.Text = "厘米";
            this.rb_cm.UseVisualStyleBackColor = true;
            // 
            // rb_mm
            // 
            this.rb_mm.AutoSize = true;
            this.rb_mm.Checked = true;
            this.rb_mm.Location = new System.Drawing.Point(227, 116);
            this.rb_mm.Name = "rb_mm";
            this.rb_mm.Size = new System.Drawing.Size(47, 16);
            this.rb_mm.TabIndex = 10;
            this.rb_mm.TabStop = true;
            this.rb_mm.Text = "毫米";
            this.rb_mm.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "长度单位:";
            // 
            // Import3D3SExcelFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 179);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.rb_mm);
            this.Controls.Add(this.rb_cm);
            this.Controls.Add(this.rb_m);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bt_run);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Import3D3SExcelFiles";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "读取3d3s V10.1模型信息";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button bt_run;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.RadioButton rb_m;
        private System.Windows.Forms.RadioButton rb_cm;
        private System.Windows.Forms.RadioButton rb_mm;
        private System.Windows.Forms.Label label3;
    }
}