using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MidasGenModel.model;

namespace AutoLoadCombination
{
    public partial class Form1 : Form
    {
        List<BLoadCase> List_DL = new List<BLoadCase>();//恒载工况列表
        List<BLoadCase> List_LL = new List<BLoadCase>();//活载工况列表
        List<BLoadCase> List_WL = new List<BLoadCase>();//风载工部列表
        List<BLoadCase> List_TL = new List<BLoadCase>();//温度作用列表

        BLoadCombTable BLT = new BLoadCombTable();//荷载组合结果表

        int NumWL = 1;//风工况数量
        int NumTL = 1;//温度作用数量

        public Form1()
        {
            InitializeComponent();
            initForm();//初始化控件显示
            
           //定义各工况名          
            updateInput();

            //结果表格显示初始化
            initGridOut();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreatLoadComb();
            //显示数据
           
            gridOut.DataSource = BLT.getComTable_G();
        }
        /// <summary>
        /// todo:自动生成荷载组合
        /// </summary>
        private void CreatLoadComb()
        {
            //清除BLT表中所有组合
            BLT.ClearComb(LCKind.GEN);

            LCFactor FF = new LCFactor();//默认组合系数表

            //由活载控制的基本组合
            List<BLoadCase> List_V = new List<BLoadCase>();//所有可变荷载工况列表
            List_V.AddRange(List_LL);//活
            List_V.AddRange(List_WL);//风
            List_V.AddRange(List_TL);//温

            List<BLoadCase[]> c = new List<BLoadCase[]> ();//可变荷载组合列表
            for(int i=1;i<=List_V.Count;i++)
            {
                List<BLoadCase[]> ci = PermutationAndCombination<BLoadCase>.GetCombination(List_V.ToArray(), i);
                if (i == 1)
                { 
                    c.AddRange(ci);
                    continue;
                }               
                //按主控制工况进行办换顺序
                foreach(BLoadCase[] cc in ci)
                {
                    List<BLoadCase[]> cn = PermutationAndCombination<BLoadCase>.GetPermutationOne(cc);
                    c.AddRange(cn);
                }                
            }
            //生成完整组合
            for (int i = 0; i < c.Count; i++)
            {
                LCType ctrT= c[i][0].LCType;//控制工况类型
                BLoadCombG LComb = new BLoadCombG(ctrT);
                LComb.NAME = "C1";//临时取个名
                LComb.KIND = LCKind.GEN;
                foreach (BLoadCase lc in List_DL)//恒
                {
                    BLCFactGroup lcf_DL = new BLCFactGroup(lc, FF.Rg_DL);
                    LComb.AddLCFactGroup(lcf_DL);
                }

                int num_LL = c[i].Length;

                BLCFactGroup lcf_zLL = new BLCFactGroup(c[i][0],
                    FF.getPartialF_ctr(c[i][0].LCType) * FF.getLamd_LL(c[i][0].LCType));
                LComb.AddLCFactGroup(lcf_zLL);//添加控制活荷载工况

                if (num_LL > 1)
                {
                    for (int j = 1; j < num_LL; j++)
                    {
                        LCType lct = c[i][j].LCType;
                        BLCFactGroup lcf_LL = new BLCFactGroup(c[i][j],
                            FF.getPartialF_ctr(lct) * FF.getLamd_LL(lct) * 
                            FF.getCombinationF(lct));
                        LComb.AddLCFactGroup(lcf_LL);//添加组合活荷载
                    }
                }

                LComb.DESC = LComb.ToString();//指定组合描述
                BLT.AddEnforce(LComb);
            }            
        }

        /// <summary>
        /// 初始化结果显示
        /// </summary>
        private void initGridOut()
        {
            gridOut.ReadOnly = true;
            //数据绑定
            gridOut.DataSource = BLT.getComTable_G();
            gridOut.Columns[1].Width = 250;
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        private void initForm()
        {
            this.comboBox_W.SelectedIndex = 0;
            this.comboBox_DL.SelectedIndex = 0;
            this.comboBox_LL.SelectedIndex = 0;
            this.comboBox_Eh.SelectedIndex = 2;
            this.comboBox_Ev.SelectedIndex = 2;
        }
        /// <summary>
        /// 更新输入数据
        /// </summary>
        private void updateInput()
        {
            BLoadCase LC = new BLoadCase(tb_DL.Text);
            LC.ANALType = (comboBox_DL.SelectedIndex == 0) ? ANAL.ST : ANAL.CB;
            LC.LCType = LCType.D;
            List_DL.Add(LC);

            LC = new BLoadCase(tb_LL.Text);
            LC.ANALType = (comboBox_LL.SelectedIndex == 0) ? ANAL.ST : ANAL.CB;
            LC.LCType = LCType.L;
            List_LL.Add(LC);

            LC = new BLoadCase(tb_W1.Text);
            LC.ANALType = (comboBox_W.SelectedIndex==0) ? ANAL.ST : ANAL.CB;
            LC.LCType = LCType.W;
            List_WL.Add(LC);

            LC = new BLoadCase(tb_T1.Text);
            LC.LCType = LCType.T;
            List_TL.Add(LC);
        }

        private void npd_WL_ValueChanged(object sender, EventArgs e)
        {
            NumWL = (int)npd_WL.Value;//WL数量修改
            if (NumWL <= 1)
                return;

            int os_x = tb_W1.Width + 6;//偏移量           
            Point p1 = tb_W1.Location;//第一个控件位置
            for (int i = 2; i <= NumWL; i++)
            {
                TextBox tbn = new TextBox();
                tbn.Name = string.Format("tb_W{0}", i);
                p1.Offset(os_x,0);
                tbn.Location = p1;
                tbn.Text = string.Format("W{0}",i);
                tbn.Width = tb_W1.Width;
                tbn.Height = tb_W1.Height;
                this.groupBox3.Controls.Add(tbn);
            }
        }
    }
}
