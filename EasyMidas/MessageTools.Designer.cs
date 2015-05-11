namespace EasyMidas
{
    partial class MessageTools
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
            this.tb_out = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tb_out
            // 
            this.tb_out.AcceptsReturn = true;
            this.tb_out.AcceptsTab = true;
            this.tb_out.BackColor = System.Drawing.Color.White;
            this.tb_out.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_out.Location = new System.Drawing.Point(0, 0);
            this.tb_out.Multiline = true;
            this.tb_out.Name = "tb_out";
            this.tb_out.ReadOnly = true;
            this.tb_out.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_out.Size = new System.Drawing.Size(593, 100);
            this.tb_out.TabIndex = 1;
            this.tb_out.TextChanged += new System.EventHandler(this.tb_out_TextChanged);
            // 
            // MessageTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tb_out);
            this.Key = "MessageTool";
            this.Name = "MessageTools";
            this.Size = new System.Drawing.Size(593, 100);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_out;

    }
}
