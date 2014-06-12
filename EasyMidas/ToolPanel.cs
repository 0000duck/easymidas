using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Xceed.DockingWindows;

namespace EasyMidas
{
    public partial class ToolPanel : ToolWindow
    {
        //设计参数工具对话框
        private DesignWindow dwTW;

        public ToolPanel()
        {
            InitializeComponent();
            dwTW = new DesignWindow();
            dwTW.Name = "DesgnWindow";//检索key
        }
        /// <summary>
        /// 切换菜单
        /// </summary>
        /// <param name="iMenu"></param>
        public void switchUC(int iMenu)
        {
            this.Controls["tabControlMain"].Hide();
            dwTW.SetMenuByid(iMenu);
            this.Controls.Add(dwTW);
        }
        /// <summary>
        /// 返回原始主菜单
        /// </summary>
        public void backTabControl()
        {
            this.Controls.RemoveByKey("DesgnWindow");
            this.Controls["tabControlMain"].Show();
        }
        /// <summary>
        /// 初始化菜单控件
        /// </summary>
        public void initCon()
        {
            this.Controls.Clear();
            //this.InitializeComponent();
        }
    }
}
