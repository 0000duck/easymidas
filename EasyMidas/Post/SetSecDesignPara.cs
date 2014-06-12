using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MidasGenModel.model;
using MidasGenModel.Design;

namespace EasyMidas.Post
{
    public partial class SetSecDesignPara : Form
    {
        private BSecDesignPara _CurSecPara;

        public SetSecDesignPara()
        {
            InitializeComponent();
            _CurSecPara = new BSecDesignPara(1);
        }
        /// <summary>
        /// 当前模型
        /// </summary>
        public MidasGenModel.model.Bmodel CurModel
        {
            get
            {
                MainForm mf = this.Owner as MainForm;
                return mf.ModelForm.CurModel;
            }
        }

        /// <summary>
        /// 验算结果表
        /// </summary>
        public MidasGenModel.Design.CheckRes CheckTable
        {
            get
            {
                MainForm mmf = this.Owner as MainForm;
                return mmf.ModelForm.CheckTable;
            }
        }

        /// <summary>
        /// 初始化对话框
        /// </summary>
        public void InitPanels()
        {
            #region 初始化截面下拉列表
            cb_secs.Items.Clear();
            if (CurModel.sections.Count > 0)
            {
                foreach (BSections sec in CurModel.sections.Values)
                {
                    cb_secs.Items.Add(sec.Num + ":" + sec.Name);
                }
            }
            else
            {
                cb_secs.Items.Add("无");
            }
            cb_secs.SelectedIndex = 0;
            //截面类别
            string strT = cb_secs.SelectedItem.ToString();
            if (strT.Contains(":"))
            {
                string num = strT.Remove(strT.IndexOf(':'));
                _CurSecPara.iSec = int.Parse(num);
                //刷新控件
                freshParaControl(int.Parse(num));
            }
            else
            {
                cb_SecCat_y.SelectedIndex = 1;
                cb_SecCat_z.SelectedIndex = 1;
            }
            #endregion 
        }

        private void tb_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bt_Enter_Click(object sender, EventArgs e)
        {
            MainForm mmf = this.Owner as MainForm;
            ModelForm1 cf = mmf.ModelForm;
            if (rb_add.Checked)
            {
                //重新取得bsp对像数据
                ///重要提示：对话框中设置局部的_CurSecPara对像由于生命周期问题，不适用于连续数据修改，建议心后直接获取对话框控件的状态
                BSecDesignPara bsp = new BSecDesignPara (_CurSecPara.iSec);
                bsp.IsClosed = cb_Closed.Checked;
                bsp.RatioNet = double.Parse(tb_ratioNet.Text);
                bsp.SecCat_y = (SecCategory)cb_SecCat_y.SelectedIndex;
                bsp.SecCat_z = (SecCategory)cb_SecCat_z.SelectedIndex;
                bsp.Gama_x = double.Parse(tb_Gamax.Text);
                bsp.Gama_y = double.Parse(tb_Gamay.Text);
                cf.CheckModel.assignSecDP(bsp.iSec, bsp);
            }
            else if (rb_dele.Checked)
            {
                cf.CheckModel.deleSecTP(_CurSecPara.iSec);
            }
        }
        //更改当前设计截面时发生
        private void cb_secs_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strT = cb_secs.SelectedItem.ToString();
            if (strT.Contains(":"))
            {
                string num = strT.Remove(strT.IndexOf(':'));
                _CurSecPara.iSec = int.Parse(num);
                //刷新控件
                freshParaControl(int.Parse(num));
            }           
        }
        /// <summary>
        /// 当控件参数修改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParaModified(object sender, EventArgs e)
        {
            _CurSecPara.RatioNet = double.Parse(tb_ratioNet.Text);
            _CurSecPara.IsClosed = cb_Closed.Checked;
            _CurSecPara.SecCat_y =(SecCategory)cb_SecCat_y.SelectedIndex;
            _CurSecPara.SecCat_z = (SecCategory)cb_SecCat_z.SelectedIndex;
            _CurSecPara.Gama_x = double.Parse(tb_Gamax.Text);
            _CurSecPara.Gama_y = double.Parse(tb_Gamay.Text);
            gb_Para.BackColor = Color.FromKnownColor(KnownColor.Control);
        }
        /// <summary>
        /// 查询现在设计数据库，并在控件上显示
        /// </summary>
        private void freshParaControl(int num)
        {
            MainForm mmf = this.Owner as MainForm;
            BCheckModel cm = mmf.ModelForm.CheckModel;
            if (cm.SecDP.ContainsKey(num))
            {
                double r1 = cm.SecDP[num].RatioNet;
                _CurSecPara.RatioNet = r1;
                tb_ratioNet.Text = r1.ToString();
                bool r2 = cm.SecDP[num].IsClosed;
                _CurSecPara.IsClosed = r2;
                cb_Closed.Checked = r2;
                SecCategory r3 = cm.SecDP[num].SecCat_y;
                _CurSecPara.SecCat_y = r3;
                cb_SecCat_y.SelectedIndex = (int)r3;
                SecCategory r33 = cm.SecDP[num].SecCat_z;
                _CurSecPara.SecCat_z = r33;
                cb_SecCat_z.SelectedIndex = (int)r33;
                double r4 = cm.SecDP[num].Gama_x;
                double r5 = cm.SecDP[num].Gama_y;
                _CurSecPara.Gama_x = r4;
                _CurSecPara.Gama_y = r5;
                tb_Gamax.Text = r4.ToString();
                tb_Gamay.Text = r5.ToString();
                gb_Para.BackColor = Color.PowderBlue;
            }
            else
            {
                gb_Para.BackColor = Color.FromKnownColor(KnownColor.Control);
                cb_SecCat_y.SelectedIndex = 1;
                cb_SecCat_z.SelectedIndex = 2;
            }
        }
    }
}
