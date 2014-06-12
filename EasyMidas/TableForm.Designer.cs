namespace EasyMidas
{
    partial class TableForm
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
            this.components = new System.ComponentModel.Container();
            this.gc_Table = new Xceed.Grid.GridControl();
            this.dataRowTemplate1 = new Xceed.Grid.DataRow();
            this.groupByRow1 = new Xceed.Grid.GroupByRow();
            this.columnManagerRow1 = new Xceed.Grid.ColumnManagerRow();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.导出ExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.gc_Table)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataRowTemplate1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.columnManagerRow1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gc_Table
            // 
            this.gc_Table.ContextMenuStrip = this.contextMenuStrip1;
            this.gc_Table.DataRowTemplate = this.dataRowTemplate1;
            this.gc_Table.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gc_Table.FixedHeaderRows.Add(this.groupByRow1);
            this.gc_Table.FixedHeaderRows.Add(this.columnManagerRow1);
            this.gc_Table.Location = new System.Drawing.Point(0, 0);
            this.gc_Table.Name = "gc_Table";
            this.gc_Table.ReadOnly = true;
            this.gc_Table.Size = new System.Drawing.Size(646, 399);
            this.gc_Table.TabIndex = 0;
            // 
            // groupByRow1
            // 
            this.groupByRow1.Visible = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导出ExcelToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 48);
            // 
            // 导出ExcelToolStripMenuItem
            // 
            this.导出ExcelToolStripMenuItem.Name = "导出ExcelToolStripMenuItem";
            this.导出ExcelToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.导出ExcelToolStripMenuItem.Text = "导出Excel...";
            this.导出ExcelToolStripMenuItem.Click += new System.EventHandler(this.导出ExcelToolStripMenuItem_Click);
            // 
            // TableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gc_Table);
            this.Name = "TableForm";
            this.Size = new System.Drawing.Size(646, 399);
            ((System.ComponentModel.ISupportInitialize)(this.gc_Table)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataRowTemplate1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.columnManagerRow1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Xceed.Grid.DataRow dataRowTemplate1;
        private Xceed.Grid.GroupByRow groupByRow1;
        private Xceed.Grid.ColumnManagerRow columnManagerRow1;
        public Xceed.Grid.GridControl gc_Table;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 导出ExcelToolStripMenuItem;
    }
}
