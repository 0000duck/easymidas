namespace AutoLoadCombination
{
    partial class Form1
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.comboBox_DL = new System.Windows.Forms.ComboBox();
            this.tb_DL = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.comboBox_LL = new System.Windows.Forms.ComboBox();
            this.tb_LL = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBox_W = new System.Windows.Forms.ComboBox();
            this.tb_W1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.npd_WL = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tb_T1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.npd_TL = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.comboBox_Ev = new System.Windows.Forms.ComboBox();
            this.tb_Ez = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tb_E2 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.comboBox_Eh = new System.Windows.Forms.ComboBox();
            this.tb_E1 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.gridOut = new Xceed.Grid.GridControl();
            this.dataRowTemplate1 = new Xceed.Grid.DataRow();
            this.groupByRow1 = new Xceed.Grid.GroupByRow();
            this.columnManagerRow1 = new Xceed.Grid.ColumnManagerRow();
            this.cb_DLCtr = new System.Windows.Forms.CheckBox();
            this.cb_W = new System.Windows.Forms.CheckBox();
            this.cb_T = new System.Windows.Forms.CheckBox();
            this.cb_DLFavour = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.npd_WL)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.npd_TL)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataRowTemplate1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.columnManagerRow1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 523);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 25);
            this.button1.TabIndex = 0;
            this.button1.Text = "生成基本组合";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.comboBox_DL);
            this.groupBox1.Controls.Add(this.tb_DL);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(387, 48);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "恒载指定";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(219, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 9;
            this.label10.Text = "类型：";
            // 
            // comboBox_DL
            // 
            this.comboBox_DL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_DL.FormattingEnabled = true;
            this.comboBox_DL.Items.AddRange(new object[] {
            "静力(ST)",
            "组合(CB)"});
            this.comboBox_DL.Location = new System.Drawing.Point(266, 17);
            this.comboBox_DL.Name = "comboBox_DL";
            this.comboBox_DL.Size = new System.Drawing.Size(106, 20);
            this.comboBox_DL.TabIndex = 8;
            // 
            // tb_DL
            // 
            this.tb_DL.Location = new System.Drawing.Point(93, 17);
            this.tb_DL.Name = "tb_DL";
            this.tb_DL.Size = new System.Drawing.Size(100, 21);
            this.tb_DL.TabIndex = 1;
            this.tb_DL.Text = "DL";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "恒载名称：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.comboBox_LL);
            this.groupBox2.Controls.Add(this.tb_LL);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(12, 64);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(387, 47);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "活载指定";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(219, 20);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 12);
            this.label11.TabIndex = 9;
            this.label11.Text = "类型：";
            // 
            // comboBox_LL
            // 
            this.comboBox_LL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_LL.FormattingEnabled = true;
            this.comboBox_LL.Items.AddRange(new object[] {
            "静力(ST)",
            "组合(CB)"});
            this.comboBox_LL.Location = new System.Drawing.Point(266, 17);
            this.comboBox_LL.Name = "comboBox_LL";
            this.comboBox_LL.Size = new System.Drawing.Size(106, 20);
            this.comboBox_LL.TabIndex = 8;
            // 
            // tb_LL
            // 
            this.tb_LL.Location = new System.Drawing.Point(93, 17);
            this.tb_LL.Name = "tb_LL";
            this.tb_LL.Size = new System.Drawing.Size(100, 21);
            this.tb_LL.TabIndex = 1;
            this.tb_LL.Text = "LL";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "活载名称：";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cb_W);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.comboBox_W);
            this.groupBox3.Controls.Add(this.tb_W1);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.npd_WL);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(12, 116);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(387, 83);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(163, 26);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 7;
            this.label9.Text = "类型：";
            // 
            // comboBox_W
            // 
            this.comboBox_W.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_W.FormattingEnabled = true;
            this.comboBox_W.Items.AddRange(new object[] {
            "静力(ST)",
            "组合(CB)"});
            this.comboBox_W.Location = new System.Drawing.Point(210, 23);
            this.comboBox_W.Name = "comboBox_W";
            this.comboBox_W.Size = new System.Drawing.Size(106, 20);
            this.comboBox_W.TabIndex = 4;
            // 
            // tb_W1
            // 
            this.tb_W1.Location = new System.Drawing.Point(93, 53);
            this.tb_W1.Name = "tb_W1";
            this.tb_W1.Size = new System.Drawing.Size(39, 21);
            this.tb_W1.TabIndex = 6;
            this.tb_W1.Text = "W1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "工况名：";
            // 
            // npd_WL
            // 
            this.npd_WL.Location = new System.Drawing.Point(93, 22);
            this.npd_WL.Name = "npd_WL";
            this.npd_WL.Size = new System.Drawing.Size(39, 21);
            this.npd_WL.TabIndex = 4;
            this.npd_WL.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.npd_WL.ValueChanged += new System.EventHandler(this.npd_WL_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "风工况数：";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cb_T);
            this.groupBox4.Controls.Add(this.tb_T1);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.npd_TL);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Location = new System.Drawing.Point(12, 205);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(387, 79);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            // 
            // tb_T1
            // 
            this.tb_T1.Location = new System.Drawing.Point(93, 52);
            this.tb_T1.Name = "tb_T1";
            this.tb_T1.Size = new System.Drawing.Size(39, 21);
            this.tb_T1.TabIndex = 6;
            this.tb_T1.Text = "T1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "工况名：";
            // 
            // npd_TL
            // 
            this.npd_TL.Location = new System.Drawing.Point(93, 21);
            this.npd_TL.Name = "npd_TL";
            this.npd_TL.Size = new System.Drawing.Size(39, 21);
            this.npd_TL.TabIndex = 4;
            this.npd_TL.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.npd_TL.ValueChanged += new System.EventHandler(this.npd_TL_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "温度工况数：";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label14);
            this.groupBox5.Controls.Add(this.comboBox_Ev);
            this.groupBox5.Controls.Add(this.tb_Ez);
            this.groupBox5.Controls.Add(this.label13);
            this.groupBox5.Controls.Add(this.tb_E2);
            this.groupBox5.Controls.Add(this.label12);
            this.groupBox5.Controls.Add(this.comboBox_Eh);
            this.groupBox5.Controls.Add(this.tb_E1);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.numericUpDown3);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Enabled = false;
            this.groupBox5.Location = new System.Drawing.Point(12, 290);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(387, 116);
            this.groupBox5.TabIndex = 5;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "地震作用指定";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(138, 90);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(41, 12);
            this.label14.TabIndex = 14;
            this.label14.Text = "类型：";
            // 
            // comboBox_Ev
            // 
            this.comboBox_Ev.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Ev.FormattingEnabled = true;
            this.comboBox_Ev.Items.AddRange(new object[] {
            "静力(ST)",
            "组合(CB)",
            "反应谱(RS)",
            "偶然偏心反应谱(ES)"});
            this.comboBox_Ev.Location = new System.Drawing.Point(185, 87);
            this.comboBox_Ev.Name = "comboBox_Ev";
            this.comboBox_Ev.Size = new System.Drawing.Size(106, 20);
            this.comboBox_Ev.TabIndex = 13;
            // 
            // tb_Ez
            // 
            this.tb_Ez.Location = new System.Drawing.Point(93, 87);
            this.tb_Ez.Name = "tb_Ez";
            this.tb_Ez.Size = new System.Drawing.Size(39, 21);
            this.tb_Ez.TabIndex = 12;
            this.tb_Ez.Text = "Ez";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(22, 90);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 12);
            this.label13.TabIndex = 11;
            this.label13.Text = "竖向地震：";
            // 
            // tb_E2
            // 
            this.tb_E2.Location = new System.Drawing.Point(138, 59);
            this.tb_E2.Name = "tb_E2";
            this.tb_E2.Size = new System.Drawing.Size(39, 21);
            this.tb_E2.TabIndex = 10;
            this.tb_E2.Text = "E2";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(154, 30);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 12);
            this.label12.TabIndex = 9;
            this.label12.Text = "类型：";
            // 
            // comboBox_Eh
            // 
            this.comboBox_Eh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Eh.FormattingEnabled = true;
            this.comboBox_Eh.Items.AddRange(new object[] {
            "静力(ST)",
            "组合(CB)",
            "反应谱(RS)",
            "偶然偏心反应谱(ES)"});
            this.comboBox_Eh.Location = new System.Drawing.Point(201, 27);
            this.comboBox_Eh.Name = "comboBox_Eh";
            this.comboBox_Eh.Size = new System.Drawing.Size(106, 20);
            this.comboBox_Eh.TabIndex = 8;
            // 
            // tb_E1
            // 
            this.tb_E1.Location = new System.Drawing.Point(93, 59);
            this.tb_E1.Name = "tb_E1";
            this.tb_E1.Size = new System.Drawing.Size(39, 21);
            this.tb_E1.TabIndex = 6;
            this.tb_E1.Text = "E1";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 62);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 5;
            this.label7.Text = "工况名：";
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(93, 28);
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(39, 21);
            this.numericUpDown3.TabIndex = 4;
            this.numericUpDown3.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(22, 30);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "水平地震数：";
            // 
            // gridOut
            // 
            this.gridOut.DataRowTemplate = this.dataRowTemplate1;
            this.gridOut.FixedHeaderRows.Add(this.groupByRow1);
            this.gridOut.FixedHeaderRows.Add(this.columnManagerRow1);
            this.gridOut.Location = new System.Drawing.Point(414, 0);
            this.gridOut.Name = "gridOut";
            this.gridOut.Size = new System.Drawing.Size(487, 568);
            this.gridOut.TabIndex = 7;
            // 
            // cb_DLCtr
            // 
            this.cb_DLCtr.AutoSize = true;
            this.cb_DLCtr.Location = new System.Drawing.Point(20, 450);
            this.cb_DLCtr.Name = "cb_DLCtr";
            this.cb_DLCtr.Size = new System.Drawing.Size(132, 16);
            this.cb_DLCtr.TabIndex = 16;
            this.cb_DLCtr.Text = "包括由永久荷载控制";
            this.cb_DLCtr.UseVisualStyleBackColor = true;
            // 
            // cb_W
            // 
            this.cb_W.AutoSize = true;
            this.cb_W.Checked = true;
            this.cb_W.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_W.Location = new System.Drawing.Point(7, 1);
            this.cb_W.Name = "cb_W";
            this.cb_W.Size = new System.Drawing.Size(84, 16);
            this.cb_W.TabIndex = 8;
            this.cb_W.Text = "风荷载指定";
            this.cb_W.UseVisualStyleBackColor = true;
            this.cb_W.CheckedChanged += new System.EventHandler(this.cb_W_CheckedChanged);
            // 
            // cb_T
            // 
            this.cb_T.AutoSize = true;
            this.cb_T.Checked = true;
            this.cb_T.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_T.Location = new System.Drawing.Point(8, 1);
            this.cb_T.Name = "cb_T";
            this.cb_T.Size = new System.Drawing.Size(96, 16);
            this.cb_T.TabIndex = 9;
            this.cb_T.Text = "温度作用指定";
            this.cb_T.UseVisualStyleBackColor = true;
            this.cb_T.CheckedChanged += new System.EventHandler(this.cb_T_CheckedChanged);
            // 
            // cb_DLFavour
            // 
            this.cb_DLFavour.AutoSize = true;
            this.cb_DLFavour.Location = new System.Drawing.Point(20, 472);
            this.cb_DLFavour.Name = "cb_DLFavour";
            this.cb_DLFavour.Size = new System.Drawing.Size(120, 16);
            this.cb_DLFavour.TabIndex = 17;
            this.cb_DLFavour.Text = "永久荷载有利组合";
            this.cb_DLFavour.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(901, 568);
            this.Controls.Add(this.cb_DLFavour);
            this.Controls.Add(this.cb_DLCtr);
            this.Controls.Add(this.gridOut);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "荷载组合生成器";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.npd_WL)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.npd_TL)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataRowTemplate1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.columnManagerRow1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tb_DL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tb_LL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown npd_WL;
        private System.Windows.Forms.TextBox tb_W1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox tb_T1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown npd_TL;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox tb_E1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboBox_W;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox comboBox_DL;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox comboBox_LL;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox comboBox_Ev;
        private System.Windows.Forms.TextBox tb_Ez;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tb_E2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox comboBox_Eh;
        private Xceed.Grid.GridControl gridOut;
        private Xceed.Grid.DataRow dataRowTemplate1;
        private Xceed.Grid.GroupByRow groupByRow1;
        private Xceed.Grid.ColumnManagerRow columnManagerRow1;
        private System.Windows.Forms.CheckBox cb_DLCtr;
        private System.Windows.Forms.CheckBox cb_W;
        private System.Windows.Forms.CheckBox cb_T;
        private System.Windows.Forms.CheckBox cb_DLFavour;
    }
}

