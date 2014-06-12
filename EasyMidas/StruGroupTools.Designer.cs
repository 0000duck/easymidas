namespace EasyMidas
{
    partial class StruGroupTools
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
            this.label1 = new System.Windows.Forms.Label();
            this.lb_GroupA = new System.Windows.Forms.ListBox();
            this.bt_TSFZ = new System.Windows.Forms.Button();
            this.tb_out = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "模型中的结构组:";
            // 
            // lb_GroupA
            // 
            this.lb_GroupA.FormattingEnabled = true;
            this.lb_GroupA.ItemHeight = 12;
            this.lb_GroupA.Items.AddRange(new object[] {
            "hkh",
            "ljl",
            "ljl",
            "lkmklm",
            ",mklm",
            "202",
            "lk",
            ";kl;l",
            "lklk"});
            this.lb_GroupA.Location = new System.Drawing.Point(12, 29);
            this.lb_GroupA.Name = "lb_GroupA";
            this.lb_GroupA.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lb_GroupA.Size = new System.Drawing.Size(144, 184);
            this.lb_GroupA.TabIndex = 3;
            // 
            // bt_TSFZ
            // 
            this.bt_TSFZ.Location = new System.Drawing.Point(262, 236);
            this.bt_TSFZ.Name = "bt_TSFZ";
            this.bt_TSFZ.Size = new System.Drawing.Size(85, 26);
            this.bt_TSFZ.TabIndex = 5;
            this.bt_TSFZ.Text = "特殊分组";
            this.bt_TSFZ.UseVisualStyleBackColor = true;
            this.bt_TSFZ.Click += new System.EventHandler(this.bt_TSFZ_Click);
            // 
            // tb_out
            // 
            this.tb_out.Location = new System.Drawing.Point(162, 44);
            this.tb_out.Multiline = true;
            this.tb_out.Name = "tb_out";
            this.tb_out.ReadOnly = true;
            this.tb_out.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_out.Size = new System.Drawing.Size(413, 169);
            this.tb_out.TabIndex = 6;
            // 
            // StruGroupTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 274);
            this.Controls.Add(this.tb_out);
            this.Controls.Add(this.bt_TSFZ);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lb_GroupA);
            this.Name = "StruGroupTools";
            this.Text = "结构组处理工具箱";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lb_GroupA;
        private System.Windows.Forms.Button bt_TSFZ;
        private System.Windows.Forms.TextBox tb_out;
    }
}