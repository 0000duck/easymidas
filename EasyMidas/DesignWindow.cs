using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EasyMidas.Post;

namespace EasyMidas
{
    public partial class DesignWindow : UserControl
    {
        public DesignWindow()
        {
            InitializeComponent();
            cb_DesignMenu.SelectedIndex = 0;
        }

        private void cb_DesignMenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cb_DesignMenu.SelectedIndex)
            {
                case 0:
                    Subpanel.Controls.Clear();
                    UCSetLength UCsl = new UCSetLength();
                    Subpanel.Controls.Add(UCsl);
                    break;
                case 1:
                    Subpanel.Controls.Clear();
                    UCSetCompType UCsct = new UCSetCompType();
                    Subpanel.Controls.Add(UCsct);
                    break;
                case 2:
                    Subpanel.Controls.Clear();
                    UCSetBetam UCsbeta = new UCSetBetam();
                    Subpanel.Controls.Add(UCsbeta);
                    break;
                case 3:
                    Subpanel.Controls.Clear();
                    UCSetQuakeAdjustFator UCSqaf = new UCSetQuakeAdjustFator();
                    Subpanel.Controls.Add(UCSqaf);
                    break;
            }
        }
        /// <summary>
        /// 切换下拉列表菜单
        /// </summary>
        /// <param name="id">菜单id</param>
        public void SetMenuByid(int id)
        {
            cb_DesignMenu.SelectedIndex = id;
        }
    }
}
