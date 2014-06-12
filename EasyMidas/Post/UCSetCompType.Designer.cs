namespace EasyMidas.Post
{
    partial class UCSetCompType
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
            this.rb_colu = new System.Windows.Forms.RadioButton();
            this.rb_truss = new System.Windows.Forms.RadioButton();
            this.rb_beam = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rb_del = new System.Windows.Forms.RadioButton();
            this.rb_add = new System.Windows.Forms.RadioButton();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bt_Close
            // 
            this.bt_Close.Location = new System.Drawing.Point(118, 223);
            this.bt_Close.Name = "bt_Close";
            this.bt_Close.Size = new System.Drawing.Size(55, 23);
            this.bt_Close.TabIndex = 14;
            this.bt_Close.Text = "关 闭";
            this.bt_Close.UseVisualStyleBackColor = true;
            this.bt_Close.Click += new System.EventHandler(this.bt_Close_Click);
            // 
            // bt_Accept
            // 
            this.bt_Accept.Location = new System.Drawing.Point(19, 223);
            this.bt_Accept.Name = "bt_Accept";
            this.bt_Accept.Size = new System.Drawing.Size(55, 23);
            this.bt_Accept.TabIndex = 13;
            this.bt_Accept.Text = "适 用";
            this.bt_Accept.UseVisualStyleBackColor = true;
            this.bt_Accept.Click += new System.EventHandler(this.bt_Accept_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rb_colu);
            this.groupBox2.Controls.Add(this.rb_truss);
            this.groupBox2.Controls.Add(this.rb_beam);
            this.groupBox2.Location = new System.Drawing.Point(2, 82);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(187, 109);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "构件类型";
            // 
            // rb_colu
            // 
            this.rb_colu.AutoSize = true;
            this.rb_colu.Location = new System.Drawing.Point(17, 79);
            this.rb_colu.Name = "rb_colu";
            this.rb_colu.Size = new System.Drawing.Size(143, 16);
            this.rb_colu.TabIndex = 2;
            this.rb_colu.Text = "柱（拉弯、压弯构件）";
            this.rb_colu.UseVisualStyleBackColor = true;
            this.rb_colu.CheckedChanged += new System.EventHandler(this.rb_type_CheckedChanged);
            // 
            // rb_truss
            // 
            this.rb_truss.AutoSize = true;
            this.rb_truss.Location = new System.Drawing.Point(17, 57);
            this.rb_truss.Name = "rb_truss";
            this.rb_truss.Size = new System.Drawing.Size(143, 16);
            this.rb_truss.TabIndex = 1;
            this.rb_truss.Text = "桁架（轴心受力构件）";
            this.rb_truss.UseVisualStyleBackColor = true;
            this.rb_truss.CheckedChanged += new System.EventHandler(this.rb_type_CheckedChanged);
            // 
            // rb_beam
            // 
            this.rb_beam.AutoSize = true;
            this.rb_beam.Checked = true;
            this.rb_beam.Location = new System.Drawing.Point(17, 35);
            this.rb_beam.Name = "rb_beam";
            this.rb_beam.Size = new System.Drawing.Size(107, 16);
            this.rb_beam.TabIndex = 0;
            this.rb_beam.TabStop = true;
            this.rb_beam.Text = "梁（受弯构件）";
            this.rb_beam.UseVisualStyleBackColor = true;
            this.rb_beam.CheckedChanged += new System.EventHandler(this.rb_type_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rb_del);
            this.groupBox1.Controls.Add(this.rb_add);
            this.groupBox1.Location = new System.Drawing.Point(2, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(187, 67);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选项";
            // 
            // rb_del
            // 
            this.rb_del.AutoSize = true;
            this.rb_del.Location = new System.Drawing.Point(100, 30);
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
            this.rb_add.Location = new System.Drawing.Point(17, 30);
            this.rb_add.Name = "rb_add";
            this.rb_add.Size = new System.Drawing.Size(77, 16);
            this.rb_add.TabIndex = 0;
            this.rb_add.TabStop = true;
            this.rb_add.Text = "添加/替换";
            this.rb_add.UseVisualStyleBackColor = true;
            // 
            // UCSetCompType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bt_Close);
            this.Controls.Add(this.bt_Accept);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "UCSetCompType";
            this.Size = new System.Drawing.Size(190, 276);
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
        private System.Windows.Forms.RadioButton rb_colu;
        private System.Windows.Forms.RadioButton rb_truss;
        private System.Windows.Forms.RadioButton rb_beam;
    }
}
