namespace EasyMidas.Post
{
    partial class NodeForceForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tb_Nodes = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tb_Elems = new System.Windows.Forms.TextBox();
            this.bt_CalForce = new System.Windows.Forms.Button();
            this.bt_Close = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.rb1 = new System.Windows.Forms.RadioButton();
            this.rb2 = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tb_Nodes);
            this.groupBox1.Location = new System.Drawing.Point(9, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(477, 87);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "节点号";
            // 
            // tb_Nodes
            // 
            this.tb_Nodes.Location = new System.Drawing.Point(8, 20);
            this.tb_Nodes.Multiline = true;
            this.tb_Nodes.Name = "tb_Nodes";
            this.tb_Nodes.Size = new System.Drawing.Size(463, 61);
            this.tb_Nodes.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tb_Elems);
            this.groupBox2.Location = new System.Drawing.Point(9, 106);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(477, 87);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "单元号（从此单元集中选择内力进行合成）";
            // 
            // tb_Elems
            // 
            this.tb_Elems.Location = new System.Drawing.Point(7, 18);
            this.tb_Elems.Multiline = true;
            this.tb_Elems.Name = "tb_Elems";
            this.tb_Elems.Size = new System.Drawing.Size(463, 61);
            this.tb_Elems.TabIndex = 1;
            // 
            // bt_CalForce
            // 
            this.bt_CalForce.Location = new System.Drawing.Point(281, 228);
            this.bt_CalForce.Name = "bt_CalForce";
            this.bt_CalForce.Size = new System.Drawing.Size(113, 28);
            this.bt_CalForce.TabIndex = 4;
            this.bt_CalForce.Text = "生成节点合力";
            this.bt_CalForce.UseVisualStyleBackColor = true;
            this.bt_CalForce.Click += new System.EventHandler(this.bt_CalForce_Click);
            // 
            // bt_Close
            // 
            this.bt_Close.Location = new System.Drawing.Point(414, 228);
            this.bt_Close.Name = "bt_Close";
            this.bt_Close.Size = new System.Drawing.Size(69, 28);
            this.bt_Close.TabIndex = 5;
            this.bt_Close.Text = "关 闭";
            this.bt_Close.UseVisualStyleBackColor = true;
            this.bt_Close.Click += new System.EventHandler(this.bt_Close_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 201);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "提示：执行此命令前，请先读取单元内力";
            // 
            // rb1
            // 
            this.rb1.AutoSize = true;
            this.rb1.Checked = true;
            this.rb1.Location = new System.Drawing.Point(17, 234);
            this.rb1.Name = "rb1";
            this.rb1.Size = new System.Drawing.Size(107, 16);
            this.rb1.TabIndex = 7;
            this.rb1.TabStop = true;
            this.rb1.Text = "只显示最大合力";
            this.rb1.UseVisualStyleBackColor = true;
            // 
            // rb2
            // 
            this.rb2.AutoSize = true;
            this.rb2.Location = new System.Drawing.Point(130, 234);
            this.rb2.Name = "rb2";
            this.rb2.Size = new System.Drawing.Size(119, 16);
            this.rb2.TabIndex = 8;
            this.rb2.Text = "显示各组合下合力";
            this.rb2.UseVisualStyleBackColor = true;
            // 
            // NodeForceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 267);
            this.Controls.Add(this.rb2);
            this.Controls.Add(this.rb1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bt_Close);
            this.Controls.Add(this.bt_CalForce);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NodeForceForm";
            this.ShowInTaskbar = false;
            this.Text = "节点内力合成工具";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button bt_CalForce;
        private System.Windows.Forms.Button bt_Close;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_Nodes;
        private System.Windows.Forms.TextBox tb_Elems;
        private System.Windows.Forms.RadioButton rb1;
        private System.Windows.Forms.RadioButton rb2;
    }
}