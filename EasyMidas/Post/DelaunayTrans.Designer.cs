namespace EasyMidas.Post
{
    partial class DelaunayTrans
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rtb_midasout = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rtb_midasin = new System.Windows.Forms.RichTextBox();
            this.bt_Run = new System.Windows.Forms.Button();
            this.tb_Ang = new System.Windows.Forms.TextBox();
            this.cb_Con = new System.Windows.Forms.CheckBox();
            this.lb_Ang = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rtb_midasout);
            this.groupBox2.Location = new System.Drawing.Point(18, 100);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(477, 207);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "结果";
            // 
            // rtb_midasout
            // 
            this.rtb_midasout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_midasout.Location = new System.Drawing.Point(3, 17);
            this.rtb_midasout.Name = "rtb_midasout";
            this.rtb_midasout.Size = new System.Drawing.Size(471, 187);
            this.rtb_midasout.TabIndex = 0;
            this.rtb_midasout.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rtb_midasin);
            this.groupBox1.Location = new System.Drawing.Point(18, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(477, 87);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Midas 节点集合";
            // 
            // rtb_midasin
            // 
            this.rtb_midasin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_midasin.Location = new System.Drawing.Point(3, 17);
            this.rtb_midasin.Name = "rtb_midasin";
            this.rtb_midasin.Size = new System.Drawing.Size(471, 67);
            this.rtb_midasin.TabIndex = 0;
            this.rtb_midasin.Text = "";
            // 
            // bt_Run
            // 
            this.bt_Run.Location = new System.Drawing.Point(403, 351);
            this.bt_Run.Name = "bt_Run";
            this.bt_Run.Size = new System.Drawing.Size(75, 23);
            this.bt_Run.TabIndex = 5;
            this.bt_Run.Text = "开  始";
            this.bt_Run.UseVisualStyleBackColor = true;
            this.bt_Run.Click += new System.EventHandler(this.bt_Run_Click);
            // 
            // tb_Ang
            // 
            this.tb_Ang.Enabled = false;
            this.tb_Ang.Location = new System.Drawing.Point(64, 337);
            this.tb_Ang.Name = "tb_Ang";
            this.tb_Ang.Size = new System.Drawing.Size(47, 21);
            this.tb_Ang.TabIndex = 6;
            this.tb_Ang.Text = "160";
            this.tb_Ang.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cb_Con
            // 
            this.cb_Con.AutoSize = true;
            this.cb_Con.Location = new System.Drawing.Point(21, 315);
            this.cb_Con.Name = "cb_Con";
            this.cb_Con.Size = new System.Drawing.Size(192, 16);
            this.cb_Con.TabIndex = 8;
            this.cb_Con.Text = "△最大内角超过以下角度时过滤";
            this.cb_Con.UseVisualStyleBackColor = true;
            this.cb_Con.CheckedChanged += new System.EventHandler(this.cb_Con_CheckedChanged);
            // 
            // lb_Ang
            // 
            this.lb_Ang.AutoSize = true;
            this.lb_Ang.Enabled = false;
            this.lb_Ang.Location = new System.Drawing.Point(119, 341);
            this.lb_Ang.Name = "lb_Ang";
            this.lb_Ang.Size = new System.Drawing.Size(17, 12);
            this.lb_Ang.TabIndex = 9;
            this.lb_Ang.Text = "度";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 365);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "提示：";
            // 
            // DelaunayTrans
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 386);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lb_Ang);
            this.Controls.Add(this.cb_Con);
            this.Controls.Add(this.tb_Ang);
            this.Controls.Add(this.bt_Run);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "DelaunayTrans";
            this.Text = "由节点转换三角形板单元";
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox rtb_midasout;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox rtb_midasin;
        private System.Windows.Forms.Button bt_Run;
        private System.Windows.Forms.TextBox tb_Ang;
        private System.Windows.Forms.CheckBox cb_Con;
        private System.Windows.Forms.Label lb_Ang;
        private System.Windows.Forms.Label label1;
    }
}