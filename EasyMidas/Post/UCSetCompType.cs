using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MidasGenModel.Tools;
using MidasGenModel.Design;

namespace EasyMidas.Post
{
    public partial class UCSetCompType : UserControl
    {
        private CompForceType _CompType;//指示当前构件类型
        public UCSetCompType()
        {
            InitializeComponent();
            _CompType = CompForceType.beam;
        }

        private void bt_Close_Click(object sender, EventArgs e)
        {
            MainForm mm = this.ParentForm as MainForm;
            mm.ToolPanel.backTabControl();
        }

        private void bt_Accept_Click(object sender, EventArgs e)
        {
            MainForm mf = this.ParentForm as MainForm;
            ModelForm1 cf = mf.ModelForm;
            List<int> eles = mf.SelectElems;
            #region 输入错误判断
            if (eles.Count == 0)
            {
                mf.MessageTool.tb_out.AppendText(Environment.NewLine + "*Error*:未选择单元!");
                return;
            }
            if (cf == null || cf.IsDisposed)
            {
                MessageBox.Show("请先新建模型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            #endregion
           
            if (rb_add.Checked)//添加数据
            {
                foreach (int ele in eles)
                {
                    cf.CheckModel.assignCompType(ele, _CompType);
                }
            }
            else if (rb_del.Checked)
            {
                foreach (int ele in eles)
                {
                    cf.CheckModel.deleCompType(ele);
                }
            }
        }

        private void rb_type_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_beam.Checked)
                _CompType = CompForceType.beam;
            else if (rb_truss.Checked)
                _CompType = CompForceType.truss;
            else
                _CompType = CompForceType.column;
        }
    }
}
