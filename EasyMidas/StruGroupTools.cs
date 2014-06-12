using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MidasGenModel.model;

namespace EasyMidas
{
    public partial class StruGroupTools : Form
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

        #endregion
        #region 构造函数
        public StruGroupTools()
        {
            InitializeComponent();
        }
        #endregion

        #region 方法
        /// <summary>
        /// 窗体初始化
        /// </summary>
        public void InitForm()
        {
            SortedList<string, BSGroup> Gps = this.CurModle.StruGroups;
            lb_GroupA.Items.Clear();//清空所所项
            foreach (KeyValuePair<string, BSGroup> Gp in Gps)
            {
                lb_GroupA.Items.Add(Gp.Key);
            }
        }
        #endregion

        #region 按扭事件
        private void bt_TSFZ_Click(object sender, EventArgs e)
        {
            //结构组
            SortedList<string, BSGroup> Gps = this.CurModle.StruGroups;
            SortedList<int, Element> Eles = this.CurModle.elements;//单元表
            SortedList<int, Bnodes> Nodes = this.CurModle.nodes;//节点表

            List<int> NNs_up = new List<int>();//上弦节点组
            List<int> NNs_down = new List<int>();//下弦节点组
            List<int> EEs_up = new List<int>();//上弦单元组
            List<int> EEs_down = new List<int>();//下弦单元组
            List<int> EEs_unknown = new List<int>();//未分组的单位

            BSGroup g1 = Gps["临时腹杆"];
            BSGroup g2 = Gps["待分组上下弦杆"];

            //节点上下分组
            foreach (int ele in g1.EleList)
            {
                int i=Eles[ele].iNs[0];
                int j = Eles[ele].iNs[1];

                if (Nodes[i].Z > Nodes[j].Z)
                {
                    NNs_up.Add(i);
                    NNs_down.Add(j);
                }
                else
                {
                    NNs_up.Add(j);
                    NNs_down.Add(i);
                }
            }

            //根据节点分组单元
            foreach (int ele in g2.EleList)
            {
                int i = Eles[ele].iNs[0];
                int j = Eles[ele].iNs[1];

                if (NNs_up.Contains(i) || NNs_up.Contains(j))
                {
                    EEs_up.Add(ele);
                    continue;
                }
                else if (NNs_down.Contains(i) || NNs_down.Contains(j))
                {
                    EEs_down.Add(ele);
                    continue;
                }
                else
                {
                    EEs_unknown.Add(ele);
                }
            }

            tb_out.AppendText("上弦单元：");
            tb_out.AppendText(Environment.NewLine);
            //输出单元分组结果
            foreach (int ee in EEs_up)
            {
                tb_out.AppendText(ee.ToString() + " ");
            }

            tb_out.AppendText(Environment.NewLine+"下弦单元：");
            tb_out.AppendText(Environment.NewLine);
            //输出单元分组结果
            foreach (int ee in EEs_down)
            {
                tb_out.AppendText(ee.ToString() + " ");
            }
            tb_out.AppendText(Environment.NewLine + "未识别单元：");
            tb_out.AppendText(Environment.NewLine);
            //输出单元分组结果
            foreach (int ee in EEs_unknown)
            {
                tb_out.AppendText(ee.ToString() + " ");
            }

        }
        #endregion

    }
}
