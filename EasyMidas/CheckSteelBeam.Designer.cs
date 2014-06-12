namespace EasyMidas
{
    partial class CheckSteelBeam
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
            this.bt_Go = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rtb_midasin = new System.Windows.Forms.RichTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rtb_midasout = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // bt_Go
            // 
            this.bt_Go.Location = new System.Drawing.Point(25, 328);
            this.bt_Go.Name = "bt_Go";
            this.bt_Go.Size = new System.Drawing.Size(75, 23);
            this.bt_Go.TabIndex = 0;
            this.bt_Go.Text = "开始转换";
            this.bt_Go.UseVisualStyleBackColor = true;
            this.bt_Go.Click += new System.EventHandler(this.bt_Go_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rtb_midasin);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(477, 87);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Midas选择集";
            // 
            // rtb_midasin
            // 
            this.rtb_midasin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_midasin.Location = new System.Drawing.Point(3, 17);
            this.rtb_midasin.Name = "rtb_midasin";
            this.rtb_midasin.Size = new System.Drawing.Size(471, 67);
            this.rtb_midasin.TabIndex = 0;
            this.rtb_midasin.Text = "11786 11788 12812to12843 12847to12849 17857 17859 18661to18692 18696to18698";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rtb_midasout);
            this.groupBox2.Location = new System.Drawing.Point(12, 105);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(477, 207);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "转换后的选择集";
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
            // CheckSteelBeam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 358);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.bt_Go);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CheckSteelBeam";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Midas选择集转换工具";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bt_Go;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox rtb_midasin;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox rtb_midasout;
    }
}