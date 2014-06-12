namespace EasyMidas
{
    partial class SecEditForm
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
            this.Dgv_SecProp = new System.Windows.Forms.DataGridView();
            this.tb_SecID = new System.Windows.Forms.TextBox();
            this.tb_SecName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.bt_esc = new System.Windows.Forms.Button();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Dgv_SecProp)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Dgv_SecProp);
            this.groupBox1.Location = new System.Drawing.Point(120, 39);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(323, 476);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "截面特性列表";
            // 
            // Dgv_SecProp
            // 
            this.Dgv_SecProp.AllowUserToAddRows = false;
            this.Dgv_SecProp.AllowUserToDeleteRows = false;
            this.Dgv_SecProp.AllowUserToResizeRows = false;
            this.Dgv_SecProp.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.Dgv_SecProp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Dgv_SecProp.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.Dgv_SecProp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Dgv_SecProp.Location = new System.Drawing.Point(3, 17);
            this.Dgv_SecProp.Name = "Dgv_SecProp";
            this.Dgv_SecProp.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.Dgv_SecProp.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            this.Dgv_SecProp.RowTemplate.Height = 23;
            this.Dgv_SecProp.Size = new System.Drawing.Size(317, 456);
            this.Dgv_SecProp.TabIndex = 0;
            // 
            // tb_SecID
            // 
            this.tb_SecID.Location = new System.Drawing.Point(73, 12);
            this.tb_SecID.Name = "tb_SecID";
            this.tb_SecID.Size = new System.Drawing.Size(55, 21);
            this.tb_SecID.TabIndex = 1;
            // 
            // tb_SecName
            // 
            this.tb_SecName.Location = new System.Drawing.Point(205, 12);
            this.tb_SecName.Name = "tb_SecName";
            this.tb_SecName.Size = new System.Drawing.Size(148, 21);
            this.tb_SecName.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "截面号";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(143, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "截面名称";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(252, 528);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "确  定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // bt_esc
            // 
            this.bt_esc.Location = new System.Drawing.Point(365, 528);
            this.bt_esc.Name = "bt_esc";
            this.bt_esc.Size = new System.Drawing.Size(75, 23);
            this.bt_esc.TabIndex = 6;
            this.bt_esc.Text = "取  消";
            this.bt_esc.UseVisualStyleBackColor = true;
            this.bt_esc.Click += new System.EventHandler(this.bt_esc_Click);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "项目";
            this.Column1.Name = "Column1";
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column1.Width = 35;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "值";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column2.Width = 23;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "单位";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column3.Width = 35;
            // 
            // SecEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 563);
            this.Controls.Add(this.bt_esc);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_SecName);
            this.Controls.Add(this.tb_SecID);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SecEditForm";
            this.ShowInTaskbar = false;
            this.Text = "SecEditForm";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Dgv_SecProp)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tb_SecID;
        private System.Windows.Forms.TextBox tb_SecName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView Dgv_SecProp;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button bt_esc;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
    }
}