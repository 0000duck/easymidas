using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MidasGenModel.model;

namespace EasyMidas.Post
{
    public partial class QueryResults : Form
    {
        public QueryResults()
        {
            InitializeComponent();
            //InitPanels();//初始化面板
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
                MainForm mf = this.Owner as MainForm;
                return mf.ModelForm.CheckTable;
            }
        }

        /// <summary>
        /// 初始化对话框
        /// </summary>
        public void InitPanels()
        {
            
            cb_secs2.Items.Clear();
            if (CurModel.sections.Count > 0)
            {
                foreach (BSections sec in CurModel.sections.Values)
                {
                    cb_secs2.Items.Add(sec.Num + " " + sec.Name);
                }
            }
            else
            {
                cb_secs2.Items.Add("无");
            }
            cb_secs2.SelectedIndex = 0;
        }

        private void bt_findRaio_Click(object sender, EventArgs e)
        {
            double r1 = Convert.ToDouble(tb_R_min.Text);
            double r2 = Convert.ToDouble(tb_R_max.Text);
            string Cursec = cb_secs2.SelectedItem.ToString();
            int iSec = 5;
            if (Cursec.Contains(" "))
            {
                string temp = Cursec.Remove(Cursec.IndexOf(' '));
                iSec = Convert.ToInt32(temp);//取得截面号
            }

            List<int> Elems = CheckTable.GetElemsByRatio(r1, r2);//取得所有截面号

            string Res = "";
            //以下按截面进行过滤
            foreach (int ele in Elems)
            {
                if (CurModel.elements[ele].iPRO == iSec)
                {
                    Res = Res + " " + ele.ToString();
                }
            }
            rtb_Messagebox.Text = Res;
        }
    }
}
