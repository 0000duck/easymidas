using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EasyMidas.Post
{
    public partial class UCSetLength : UserControl
    {
        public UCSetLength()
        {
            InitializeComponent();
        }

        private void bt_close_Click(object sender, EventArgs e)
        {
            MainForm mm = this.ParentForm as MainForm;
            mm.ToolPanel.backTabControl();
        }
    }
}
