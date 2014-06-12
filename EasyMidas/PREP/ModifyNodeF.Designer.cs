namespace EasyMidas
{
    partial class ModifyNodeF
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
            this.tb_mag = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_LC = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // bt_Go
            // 
            this.bt_Go.Location = new System.Drawing.Point(27, 223);
            this.bt_Go.Name = "bt_Go";
            this.bt_Go.Size = new System.Drawing.Size(75, 23);
            this.bt_Go.TabIndex = 0;
            this.bt_Go.Text = "开始放大";
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
            this.groupBox1.Text = "节点选择集";
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
            this.groupBox2.Controls.Add(this.tb_mag);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cb_LC);
            this.groupBox2.Location = new System.Drawing.Point(12, 105);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(477, 112);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "选项";
            // 
            // tb_mag
            // 
            this.tb_mag.Location = new System.Drawing.Point(78, 67);
            this.tb_mag.Name = "tb_mag";
            this.tb_mag.Size = new System.Drawing.Size(100, 21);
            this.tb_mag.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "放大倍数:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "工况:";
            // 
            // cb_LC
            // 
            this.cb_LC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_LC.FormattingEnabled = true;
            this.cb_LC.Location = new System.Drawing.Point(76, 34);
            this.cb_LC.Name = "cb_LC";
            this.cb_LC.Size = new System.Drawing.Size(121, 20);
            this.cb_LC.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(121, 228);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "label3";
            // 
            // ModifyNodeF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 256);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.bt_Go);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModifyNodeF";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "节点荷载批量放大";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bt_Go;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox rtb_midasin;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cb_LC;
        private System.Windows.Forms.TextBox tb_mag;
        private System.Windows.Forms.Label label3;
    }
}