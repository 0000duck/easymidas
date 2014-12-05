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
        public Form1()
        {
            InitializeComponent();
            this.comboBox_W.SelectedIndex = 0;
            this.comboBox_DL.SelectedIndex = 0;
            this.comboBox_LL.SelectedIndex = 0;
            this.comboBox_Eh.SelectedIndex = 2;
            this.comboBox_Ev.SelectedIndex = 2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }
        /// <summary>
        /// 自动生成荷载组合
        /// </summary>
        private void CreatLoadComb()
        {
            double Rg_DL = 1.2;//恒载分项系数
            double Rgc_DL = 1.35;//恒载控制时分项系数

        }
    }
}
