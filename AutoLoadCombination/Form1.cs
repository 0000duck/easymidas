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
        public Form1()
        {
            InitializeComponent();
            this.comboBox_W.SelectedIndex = 0;
            this.comboBox_DL.SelectedIndex = 0;
            this.comboBox_LL.SelectedIndex = 0;
            this.comboBox_Eh.SelectedIndex = 2;
            this.comboBox_Ev.SelectedIndex = 2;

            //定义各工况名          
            BLoadCase LC = new BLoadCase("DL");
            LC.LCType = LCType.D;
            List_DL.Add(LC);

            LC = new BLoadCase("L1");
            LC.LCType = LCType.L;
            List_LL.Add(LC);

            LC = new BLoadCase("L2");
            LC.LCType = LCType.L;
            List_LL.Add(LC);

            //LC = new BLoadCase("L3");
            //LC.LCType = LCType.L;
            //List_LL.Add(LC);

            LC = new BLoadCase("W1");
            LC.LCType = LCType.W;
            List_WL.Add(LC);

            LC = new BLoadCase("W2");
            LC.LCType = LCType.W;
            List_WL.Add(LC);

            LC = new BLoadCase("T1");
            LC.LCType = LCType.T;
            List_TL.Add(LC);

            LC = new BLoadCase("T2");
            LC.LCType = LCType.T;
            List_TL.Add(LC);

            //结果表格显示初始化
            initGridOut();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreatLoadComb();
            //显示数据
            gridOut.DataSource = BLT.ComList_G;
        }
        /// <summary>
        /// todo:自动生成荷载组合
        /// </summary>
        private void CreatLoadComb()
        {
            //todo:清除BLT表中所有组合

            double Rg_DL = 1.2;//恒载分项系数（不利）
            double Rgc_DL = 1.35;//恒载控制时分项系数
            double Rgn_DL = 1.0;//恒载有利时，不大于1.0

            double Rg_LL = 1.4;//可变荷载分项系数
            double Lamd_LL = 1.0;//活荷载使用年限调整系数

            double Fi_LL = 0.7;//活载组合值系数
            double Fi_WL = 0.6;//风载组合值系数
            double Fi_TL = 0.6;//温度作用组合值系数

            //由活载控制的基本组合

            List<BLoadCase[]> c = new List<BLoadCase[]> ();
            for(int i=1;i<=List_LL.Count;i++)
            {
                List<BLoadCase[]> ci = PermutationAndCombination<BLoadCase>.GetCombination(List_LL.ToArray(), i);
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

            for (int i = 0; i < c.Count; i++)
            {
                BLoadCombG LComb = new BLoadCombG(LCType.L);
                LComb.NAME = "C1";
                LComb.KIND = LCKind.GEN;
                LComb.DESC = "活载控制";
                foreach (BLoadCase lc in List_DL)//恒
                {
                    BLCFactGroup lcf_DL = new BLCFactGroup(lc, Rg_DL);
                    LComb.AddLCFactGroup(lcf_DL);
                }

                int num_LL = c[i].Length;

                BLCFactGroup lcf_zLL = new BLCFactGroup(c[i][0], Rg_LL * Lamd_LL);
                LComb.AddLCFactGroup(lcf_zLL);//添加控制活荷载工况

                foreach (BLoadCase zlc in c[i])//活
                {
                    
                    //foreach (BLoadCase lc in List_LL)
                    //{
                    //    if (lc == zlc)
                    //        continue;
                    //    BLCFactGroup lcf_LL = new BLCFactGroup(lc, Rg_LL * Lamd_LL * Fi_LL);
                    //    LComb.AddLCFactGroup(lcf_LL);//添加组合活荷载
                    //}
                    
                }

                BLT.AddEnforce(LComb);//有问题
            }            
        }

        /// <summary>
        /// 初始化结果显示
        /// </summary>
        private void initGridOut()
        {
            gridOut.ReadOnly = true;
            //数据绑定
            gridOut.DataSource = BLT.ComList_G;
        }
    }
}
