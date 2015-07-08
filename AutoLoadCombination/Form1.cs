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

            double Rg_DL = 1.2;//恒载分项系数（不利）
            double Rgc_DL = 1.35;//恒载控制时分项系数
            double Rgn_DL = 1.0;//恒载有利时，不大于1.0

            double Rg_LL = 1.4;//可变荷载分项系数
            double Lamd_LL = 1.0;//活荷载使用年限调整系数

            double Fi_LL = 0.7;//活载组合值系数
            double Fi_WL = 0.6;//风载组合值系数
            double Fi_TL = 0.6;//温度作用组合值系数

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
                    BLCFactGroup lcf_DL = new BLCFactGroup(lc, Rg_DL);
                    LComb.AddLCFactGroup(lcf_DL);
                }

                int num_LL = c[i].Length;

                BLCFactGroup lcf_zLL = new BLCFactGroup(c[i][0], Rg_LL * Lamd_LL);
                LComb.AddLCFactGroup(lcf_zLL);//添加控制活荷载工况

                if (num_LL > 1)
                {
                    for (int j = 1; j < num_LL; j++)
                    {
                        BLCFactGroup lcf_LL = new BLCFactGroup(c[i][j], Rg_LL * Lamd_LL * Fi_LL);
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
            gridOut.Columns[1].Width = 200;
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
        }
    }
}
