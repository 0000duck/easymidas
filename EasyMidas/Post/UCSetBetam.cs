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
    public partial class UCSetBetam : UserControl
    {
        public UCSetBetam()
        {
            InitializeComponent();
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
                mf.MessageTool.Tb_out.AppendText(Environment.NewLine + "*Error*:未选择单元!");
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
                double b_my = double.Parse(tb_Betamy.Text);
                double b_mz = double.Parse(tb_Betamz.Text);
                double b_ty = double.Parse(tb_Betaty.Text);
                double b_tz = double.Parse(tb_Betatz.Text);
                foreach (int ele in eles)
                {
                    BEquivalentCoeff NewEc = new BEquivalentCoeff(ele, 
                        b_my, b_mz, b_ty, b_tz);
                    cf.CheckModel.assignEquCoeff(ele,NewEc);
                }
            }
            else if (rb_del.Checked)
            {
                foreach (int ele in eles)
                {
                    cf.CheckModel.deldEquCoeff(ele);
                }
            }
        }
    }
}
