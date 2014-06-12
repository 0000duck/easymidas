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
    public partial class UCSetQuakeAdjustFator : UserControl
    {
        public UCSetQuakeAdjustFator()
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
                double Nlc = double.Parse(tb_N_LC.Text);
                double Mlc = double.Parse(tb_M_LC.Text);
                double Vlc = double.Parse(tb_V_LC.Text);

                double Ncom = double.Parse(tb_N_Com.Text);
                double Mcom = double.Parse(tb_M_Com.Text);
                double Vcom = double.Parse(tb_V_Com.Text);
                foreach (int ele in eles)
                {
                    BQuakeAdjustFactor Qaf = new BQuakeAdjustFactor(ele);
                    Qaf.N_LC = Nlc; Qaf.M_LC = Mlc; Qaf.V_LC = Vlc;
                    Qaf.N_COM = Ncom; Qaf.M_COM = Mcom; Qaf.V_COM = Vcom;
                    cf.CheckModel.assignQuakeAdjustFactor(ele,Qaf);
                }
            }
            else if (rb_del.Checked)
            {
                foreach (int ele in eles)
                {
                    cf.CheckModel.deleQuakeAdjustFactor(ele);
                }
            }
        }
    }
}
