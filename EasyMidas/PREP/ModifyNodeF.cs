using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MidasGenModel.model;
using MidasGenModel.Tools;
using System.Collections;

namespace EasyMidas
{
    public partial class ModifyNodeF : Form
    {
        #region 属性
        /// <summary>
        /// 当前模型
        /// </summary>
        public Bmodel CurModle
        {
            get
            {
                MainForm mf = this.Owner as MainForm;
                return mf.ModelForm.CurModel;
            }
        }
        /// <summary>
        /// 数据提示窗
        /// </summary>
        public MessageTools MessTool
        {
            get
            {
                MainForm mf = this.Owner as MainForm;
                return mf.MessageTool;
            }
        }

        #endregion
        public ModifyNodeF()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 初始化对话框
        /// </summary>
        public void InitForm()
        {
            rtb_midasin.Text = "3847";
            //荷载工况表显示
            cb_LC.Items.Clear();
            foreach (BLoadCase lc in this.CurModle.STLDCASE)
            {
                cb_LC.Items.Add(lc.LCName);             
            }
            if (cb_LC.Items.Count>0)
                cb_LC.SelectedIndex = 0;
            cb_LC.DropDownStyle = ComboBoxStyle.DropDownList;

            //放大系数
            tb_mag.BackColor = Color.AliceBlue;
            tb_mag.Text = "1.1";
            //提示
            label3.Text = "提示:节点荷载一经放大，不可恢复，请慎重操作！";
            label3.ForeColor = Color.Red;
        }

        private void bt_Go_Click(object sender, EventArgs e)
        {
            string LcN = cb_LC.SelectedItem.ToString();//选择的工况
            double Mag = double.Parse(tb_mag.Text);//放大倍数
            //选择的节点集
            List<int> nodes = SelectCollection.StringToList(rtb_midasin.Text);
            int Num_modified = 0;//记录修改的节点数
            //修改荷载
            foreach (int nn in nodes)
            {
                SortedList NLD = 
                    this.CurModle.LoadTable.NLoadData[LcN] as SortedList;
                if (NLD.ContainsKey(nn))
                {
                    this.CurModle.LoadTable.ModifyNodeLoad(nn, LcN, Mag);
                    Num_modified++;
                }
            }

            //输出命令行
            string t1 = string.Format("{0}{1}个节点荷载已经进行了放大!", 
                Environment.NewLine,Num_modified);
            this.MessTool.tb_out.AppendText(t1);
            string t2 = string.Format("{0}下面输出3d3s10.1版本文本文件:",Environment.NewLine);
            this.MessTool.tb_out.AppendText(t2);
            int nodeloadlibN = 4599;//目前节点荷载库中节点荷载序号
            string lctype="0";//临时变量：工况类型0,1,2
            foreach (int nn in nodes)
            {
                SortedList NLD = 
                    this.CurModle.LoadTable.NLoadData[LcN] as SortedList;
                if (NLD.ContainsKey(nn))
                {
                    BNLoad bnl = NLD[nn] as BNLoad;
                    string s1 = string.Format("{0}PL {1} {2} {3} {4} {5} {6} {7} {8}",
                        Environment.NewLine,lctype,LcN,
                        bnl.FX,bnl.FY,bnl.FZ,bnl.MX,bnl.MY,bnl.MZ);
                    string s2 = string.Format("{0}APL {1} {2}",Environment.NewLine,
                        nn,nodeloadlibN++);
                    this.MessTool.tb_out.AppendText(s1);
                    this.MessTool.tb_out.AppendText(s2);
                }

            }
        }

    }
}
