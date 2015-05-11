using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MidasGenModel.Tools;
using MidasGenModel.Design;

namespace EasyMidas
{
    public partial class CheckElemForm : Form
    {
        public CheckElemForm()
        {
            InitializeComponent();
        }

        private void bt_GoCheck_Click(object sender, EventArgs e)
        {
            MainForm mmf = this.Owner as MainForm;//主窗口
            ModelForm1 mf = mmf.ModelForm;//模型窗口
            TextBox messagebox = mmf.MessageTool.Tb_out;
       
            MidasGenModel.model.Bmodel mm = mf.CurModel;
            //激活地震组合有问题
            mm.RSCombineActive(cb_isdizhen.Checked);//激活地震组合

            if (tb_Elems.Text.Length == 0)
            {
                lb_out.Text = "提示：没有指定单元号!";
            }

            List<int> eles = SelectCollection.StringToList(tb_Elems.Text);
            int num=eles.Count;//选择单元数
            int i=1;
            mmf.MessageTool.Tb_out.AppendText(Environment.NewLine+"************单元验算开始************");
            mmf.MessageTool.Tb_out.AppendText(Environment.NewLine+"  开始验算");
            foreach (int ele in eles)
            {
                mf.CheckTable.CheckElemByNum(ref mm, ele);
                //lb_out.Text = "提示："+i.ToString()+" of "+num.ToString()+ "验算完成" ;
                string info = "   "+i.ToString() + " of " + num.ToString() + " 验算完成!";
                ReplaceLastLine(ref messagebox,info);
                i++;
            }
            mmf.MessageTool.Tb_out.AppendText(Environment.NewLine+"************单元验算结束************");
        }

        /// <summary>
        /// 代替文本框中的最后一行
        /// </summary>
        /// <param name="TB">文件框，必须为多行</param>
        /// <param name="NewLine">新行的字符串</param>
        public void ReplaceLastLine(ref TextBox TB,string NewLine)
        {
            string[] ls = TB.Lines;
            if (ls.Length < 1)
                return;

            string lastline = ls[ls.Length - 1];

            int start = TB.GetFirstCharIndexFromLine(ls.Length-1);
            int end = lastline.Length;

            TB.Select(start, end);
            TB.SelectedText = "";

            TB.AppendText(NewLine);//追加新的字符
        }

        private void bt_CheckGo_Click(object sender, EventArgs e)
        {
            MainForm mmf = this.Owner as MainForm;//主窗口
            ModelForm1 mf = mmf.ModelForm;
            TextBox messagebox = mmf.MessageTool.Tb_out;

            MidasGenModel.model.Bmodel mm = mf.CurModel;
            if (tb_Elems.Text.Length == 0)
            {
                lb_out.Text = "提示：没有指定单元号!";
            }

            List<int> eles = SelectCollection.StringToList(tb_Elems.Text);
            int num = eles.Count;//选择单元数
            int i = 1;
            mmf.MessageTool.Tb_out.AppendText(Environment.NewLine + "************单元验算开始************");
            mmf.MessageTool.Tb_out.AppendText(Environment.NewLine + "  开始验算");
            foreach (int ele in eles)
            {
                //mf.CheckTable.CheckElemByNum(ref mm, ele);
                CodeCheck.RefreshDesignPara(ref mm, mf.CheckModel, ele);
                CodeCheck.CalDesignPara_lemda(ref mm, ele);
                CodeCheck.CalDesignPara_phi(ref mm,ele,1);//计算y向轴压稳定系数
                CodeCheck.CalDesignPara_phi(ref mm, ele, 2);//计算z向轴压稳定系数

                mf.CheckTable.CheckElemByNum_N(ref mm, ele, ref mf.CheckModel);
                string info = "   " + i.ToString() + " of " + num.ToString() + " 验算完成!";
                //ReplaceLastLine(ref messagebox, info);
                mmf.MessageTool.Tb_out.AppendText(Environment.NewLine+info);
                i++;
            }
            mmf.MessageTool.Tb_out.AppendText(Environment.NewLine + "************单元验算结束************");
           
        }
    }

}
