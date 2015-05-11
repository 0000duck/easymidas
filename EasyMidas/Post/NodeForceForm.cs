using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MidasGenModel.Tools;
using MidasGenModel.model;
using MidasGenModel.Geometry3d;

namespace EasyMidas.Post
{
    public partial class NodeForceForm : Form
    {
        public NodeForceForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 当前模型
        /// </summary>
        public MidasGenModel.model.Bmodel MM
        {
            get
            {
                MainForm mf = this.Owner as MainForm;
                return mf.ModelForm.CurModel;
            }
        }

        /// <summary>
        /// 输入文本框
        /// </summary>
        public TextBox MessageOUT
        {
            get
            {
                MainForm mmf = this.Owner as MainForm;//主窗口
                return  mmf.MessageTool.Tb_out;
            }
        }

        private void bt_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bt_CalForce_Click(object sender, EventArgs e)
        {
            if (tb_Nodes.Text.Length == 0)
            {
                MessageBox.Show("未输入计算节点集！！");
                return;
            }

            if (tb_Elems.Text.Length==0)
            {
                MessageBox.Show("未输入单元集！！");
                return;
            }

            List<int> Nodes = SelectCollection.StringToList(tb_Nodes.Text);//节点集
            List<int> Elems = SelectCollection.StringToList(tb_Elems.Text);//单元集
            List<string> Coms = MM.LoadCombTable.ComSteel;

            MessageOUT.AppendText(Environment.NewLine+"  [节点号]\t[最大合力(kN)]\t[控制组合]\t[合力方向向量]");
            foreach (int nn in Nodes)
            {
                List<int> ees = FindNodeElems(nn, Elems);//与之相联的单元
                Vector3 MaxSumForce = new Vector3();//最大合力
                string MaxComName=null;//最大合力组合名称

                foreach (string com in Coms)
                {
                    Vector3 SumForce = new Vector3();//合力
                    foreach (int ee in ees)
                    {
                        Vector3 vectemp = GetNodeForceVec(nn, ee,
                            MM.LoadCombTable.getLoadComb(LCKind.STEEL,com));
                        SumForce = SumForce+vectemp;
                    }
                    //显示单个组合的合力
                    if (rb2.Checked == true)
                    {
                        MessageOUT.AppendText(Environment.NewLine + "  " + nn.ToString() +
                    "\t" + SumForce.Magnitude.ToString("0.0") + "\t" + com + "\t" + 
                    Vector3.Normalize(SumForce).ToString("0.000", null));
                    }

                    //比较记录最大值
                    if (SumForce.Magnitude>MaxSumForce.Magnitude)
                    {
                        MaxSumForce = SumForce;
                        MaxComName = com;
                    }
                }

                MessageOUT.AppendText(Environment.NewLine + "  " + nn.ToString()+
                    "\t" + MaxSumForce.Magnitude.ToString("0.0") + "\t" + MaxComName + "\t" + 
                    Vector3.Normalize(MaxSumForce).ToString("0.000",null)
                    +"\t***");
            }
        }

        /// <summary>
        /// 按节点号查得相交单元
        /// </summary>
        /// <param name="nn">节点号</param>
        /// <param name="Elems">单元集合</param>
        /// <returns>共节点的单元（从指定的单元集中选择）</returns>
        public List<int> FindNodeElems(int nn, List<int> Elems)
        {
            List<int> Res = new List<int>();

            foreach (int ee in Elems)
            {
                if ((MM.elements[ee] is FrameElement) == false)
                {
                    continue;
                }
                FrameElement fme = MM.elements[ee] as FrameElement;
                if (fme.iNs.Contains(nn))
                {
                    Res.Add(ee);
                }
            }

            return Res;
        }

        /// <summary>
        /// 计算节点相交处相应单元的轴力向量
        /// </summary>
        /// <param name="node">节点号</param>
        /// <param name="ele">单元号（必须为FrameElement）</param>
        /// <param name="com">荷载组合</param>
        /// <returns>轴力向量</returns>
        public Vector3 GetNodeForceVec(int node, int ele, BLoadComb com)
        {
            Vector3 Res = new Vector3();
            FrameElement fme=MM.elements[ele] as FrameElement;
            
            //如果节点不在单元上则返回0向量
            if (fme.iNs.Contains(node) == false)
            {
                return new Vector3();
            }
            ElemForce Force = MM.CalElemForceComb(com, ele);
            if (fme.I == node)
            {
                Res = MM.getFrameVec(ele);
                Res.Normalize();//归一化
                Res = Res * Force.Force_i.N;
            }
            else
            {
                Res = -MM.getFrameVec(ele);
                Res.Normalize();//归一化
                Res = Res * Force.Force_j.N;
            }
            return Res;
        }
    }
}
