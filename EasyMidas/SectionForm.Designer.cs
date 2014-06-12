namespace EasyMidas
{
    partial class SectionForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.bt_EditSec = new System.Windows.Forms.Button();
            this.Dgv_sec = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Dgv_sec)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(513, 450);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.bt_EditSec);
            this.tabPage1.Controls.Add(this.Dgv_sec);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(505, 425);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "截面表";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(394, 341);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "截面特性";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // bt_EditSec
            // 
            this.bt_EditSec.Location = new System.Drawing.Point(394, 312);
            this.bt_EditSec.Name = "bt_EditSec";
            this.bt_EditSec.Size = new System.Drawing.Size(75, 23);
            this.bt_EditSec.TabIndex = 4;
            this.bt_EditSec.Text = "编 辑";
            this.bt_EditSec.UseVisualStyleBackColor = true;
            this.bt_EditSec.Click += new System.EventHandler(this.bt_EditSec_Click);
            // 
            // Dgv_sec
            // 
            this.Dgv_sec.AllowUserToAddRows = false;
            this.Dgv_sec.AllowUserToDeleteRows = false;
            this.Dgv_sec.AllowUserToResizeRows = false;
            this.Dgv_sec.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.Dgv_sec.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.Dgv_sec.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.Dgv_sec.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.Dgv_sec.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.Dgv_sec.EnableHeadersVisualStyles = false;
            this.Dgv_sec.Location = new System.Drawing.Point(6, 6);
            this.Dgv_sec.MultiSelect = false;
            this.Dgv_sec.Name = "Dgv_sec";
            this.Dgv_sec.ReadOnly = true;
            this.Dgv_sec.RowHeadersVisible = false;
            this.Dgv_sec.RowTemplate.Height = 23;
            this.Dgv_sec.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Dgv_sec.Size = new System.Drawing.Size(337, 384);
            this.Dgv_sec.TabIndex = 3;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "截面号";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "截面名称";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "截面类型";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // SectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 504);
            this.Controls.Add(this.tabControl1);
            this.Name = "SectionForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "SectionForm";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Dgv_sec)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView Dgv_sec;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.Button bt_EditSec;
        private System.Windows.Forms.Button button2;

    }
}