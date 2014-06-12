namespace EasyMidas
{
    partial class DesignWindow
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
            this.cb_DesignMenu = new System.Windows.Forms.ComboBox();
            this.Subpanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // cb_DesignMenu
            // 
            this.cb_DesignMenu.AutoCompleteCustomSource.AddRange(new string[] {
            "自由长度(L,Lb)",
            "编辑构件类型"});
            this.cb_DesignMenu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_DesignMenu.FormattingEnabled = true;
            this.cb_DesignMenu.Items.AddRange(new object[] {
            "自由长度(L,Lb)",
            "编辑构件类型",
            "编辑等效弯矩系数",
            "地震作用放大系数"});
            this.cb_DesignMenu.Location = new System.Drawing.Point(3, 3);
            this.cb_DesignMenu.Name = "cb_DesignMenu";
            this.cb_DesignMenu.Size = new System.Drawing.Size(182, 20);
            this.cb_DesignMenu.TabIndex = 0;
            this.cb_DesignMenu.SelectedIndexChanged += new System.EventHandler(this.cb_DesignMenu_SelectedIndexChanged);
            // 
            // Subpanel
            // 
            this.Subpanel.Location = new System.Drawing.Point(0, 29);
            this.Subpanel.Name = "Subpanel";
            this.Subpanel.Size = new System.Drawing.Size(190, 465);
            this.Subpanel.TabIndex = 6;
            // 
            // DesignWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Subpanel);
            this.Controls.Add(this.cb_DesignMenu);
            this.Name = "DesignWindow";
            this.Size = new System.Drawing.Size(193, 512);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cb_DesignMenu;
        private System.Windows.Forms.Panel Subpanel;

    }
}
