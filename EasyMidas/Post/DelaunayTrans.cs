using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MidasGenModel.Tools;
using MidasGenModel.DelaunayTriangulator;

namespace EasyMidas.Post
{
    public partial class DelaunayTrans : Form
    {
        static int  iMaxElem;//指示最大单元号
        public DelaunayTrans()
        {
            InitializeComponent();
        }

        private void bt_Run_Click(object sender, EventArgs e)
        {
            if (rtb_midasin.Text.Length == 0)
            {
                rtb_midasout.Text = "没有输入Midas格式的选择集";
                return;
            }
            List<int> nns = SelectCollection.StringToList(rtb_midasin.Text);

            MainForm mmf = this.Owner as MainForm;//主窗口
            ModelForm1 mf = mmf.ModelForm;

            MidasGenModel.model.Bmodel mm = mf.CurModel;

            List<Triangle> Tras = mm.getDelaunayTriangleByNodes(nns);
            rtb_midasout.Clear();//清屏
            rtb_midasout.AppendText("\n转换成功,共"+Tras.Count.ToString()+"个三角形");
            //进行三角形过滤
            if (cb_Con.Checked)
            {
                double Ang_con = double.Parse(tb_Ang.Text);
                List<Triangle> TransNew = new List<Triangle>();
                //畸形三角形过滤
                foreach (Triangle tt in Tras)
                {
                    if (tt.getMaxAngle() <= Ang_con)
                        TransNew.Add(tt);
                }
                Tras = TransNew;//更新新的三角形集合
                rtb_midasout.AppendText("\n进行△过滤后，共" + Tras.Count.ToString() + "个三角形");
            }

            //下面转换成板单元命令流
            int i =1;
            rtb_midasout.AppendText("\n*ELEMENT");
            foreach (Triangle ang in Tras)
            {
                MidasGenModel.DelaunayTriangulator.Point pt1 = ang.Vertex1;
                MidasGenModel.DelaunayTriangulator.Point pt2 = ang.Vertex2;
                MidasGenModel.DelaunayTriangulator.Point pt3 = ang.Vertex3;

                int n1 = mm.getNode(pt1.X, pt1.Y, pt1.Z);
                int n2 = mm.getNode(pt2.X, pt2.Y, pt2.Z);
                int n3 = mm.getNode(pt3.X, pt3.Y, pt3.Z);
                int iNum = i + iMaxElem;//单元号
                string trout="\n "+iNum.ToString()+", PLATE, 5, 2, "+n1+", "+n2+", "+n3;
                rtb_midasout.AppendText(trout);

                //提示信息输出
                label1.Text = "提示：板单元"+i.ToString()+"已生成...";
                this.Refresh();
                i++;
            }

            //更新最大单元号
            iMaxElem = iMaxElem + i;
        }

        private void cb_Con_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_Con.CheckState == CheckState.Checked)
            {
                tb_Ang.Enabled = true;
                lb_Ang.Enabled = true;
            }
            else
            {
                tb_Ang.Enabled = false;
                lb_Ang.Enabled = false;
            }
        }

        /// <summary>
        /// 初始化对话框参数
        /// </summary>
        public  void InitForm()
        {
            MainForm mmf = this.Owner as MainForm;//主窗口

            MidasGenModel.model.Bmodel mm = mmf.ModelForm.CurModel;
            iMaxElem = mm.MaxElem;//取得最大单元号
        }
    }
}
