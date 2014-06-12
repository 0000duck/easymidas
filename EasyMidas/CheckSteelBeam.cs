using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EasyMidas
{
    public partial class CheckSteelBeam : Form
    {
        public CheckSteelBeam()
        {
            InitializeComponent();
        }

        private void bt_Go_Click(object sender, EventArgs e)
        {
            if (rtb_midasin.Text.Length == 0)
            {
                rtb_midasout.Text = "没有输入Midas格式的选择集";
                return;
            }
            else
            {
                string res = null;//结果字符串
                int j = 0;
                string[] strIns=rtb_midasin.Lines;
                foreach (string ss in strIns)
                {
                    if (ss.Length == 0)
                        continue;
                    string[] temp = ss.Split(' ');
                    foreach (string str in temp)
                    {
                        if (str.Contains("to") == false)
                        {
                            res += "\n" + str;
                            j++;
                        }
                        else if (str.Contains("by") == false)
                        {
                            string from = str.Remove(str.IndexOf('t'));
                            string to = str.TrimStart(from.ToCharArray());
                            to = to.TrimStart("to".ToCharArray());

                            int from_num = Convert.ToInt32(from);
                            int to_num = Convert.ToInt32(to);

                            for (int i = from_num; i <= to_num; i++)
                            {
                                res += "\n" + i.ToString();
                                j++;
                            }
                        }
                        else
                        {
                            string[] tt = str.Split(new string[] { "to", "by" }, StringSplitOptions.RemoveEmptyEntries);
                            string from = tt[0];
                            string to = tt[1];
                            string by = tt[2];

                            int from_num = Convert.ToInt32(from);
                            int to_num = Convert.ToInt32(to);
                            int by_num = Convert.ToInt32(by);

                            for (int i = from_num; i <= to_num; i+=by_num)
                            {
                                res += "\n" + i.ToString();
                                j++;
                            }
                        }
                    }
                }

                rtb_midasout.Text = j.ToString()+res;
            }
        }
    }
}
