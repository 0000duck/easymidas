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
    public partial class SecEditForm : Form
    {
        public SecEditForm()
        {
            InitializeComponent();
        }

        private void bt_esc_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 更新当前对话框显示的截面信息
        /// </summary>
        /// <param name="SecId">截面号</param>
        public void UpdateCurentSec(int SecId)
        {
            MainForm mmf = this.Owner as MainForm;
            ModelForm1 mf = mmf.ModelForm;
            Bmodel mm = mf.CurModel;
            BSections curSec=mm.sections[SecId];
            string len=mm.unit.Length;//长度单位
            //更新截面信息
            tb_SecID.Text = SecId.ToString();
            tb_SecName.Text = curSec.Name;


            //更新截面特性
            Dgv_SecProp.Rows.Clear();//清除行
            Dgv_SecProp.RowHeadersVisible = false;
            DataAddRow(ref Dgv_SecProp, "A", curSec.Area.ToString(), len + "^2");
            DataAddRow(ref Dgv_SecProp, "Ixx", curSec.Ixx.ToString(), len + "^4");
            DataAddRow(ref Dgv_SecProp, "Iyy", curSec.Iyy.ToString(), len + "^4");
            DataAddRow(ref Dgv_SecProp, "Izz", curSec.Izz.ToString(), len + "^4");
            DataAddRow (ref Dgv_SecProp,"Iw",curSec.Iw.ToString(),len+"^6");

            DataAddRow(ref Dgv_SecProp, "Cent:y", curSec.Cy.ToString(), len);
            DataAddRow(ref Dgv_SecProp, "Cent:z", curSec.Cz.ToString(), len);
            DataAddRow(ref Dgv_SecProp, "Shear:y", curSec.Sy.ToString(), len);
            DataAddRow(ref Dgv_SecProp, "Shear:z", curSec.Sz.ToString(), len);

            DataAddRow(ref Dgv_SecProp, "CyM", curSec.CyM.ToString(), len);
            DataAddRow(ref Dgv_SecProp, "CyP", curSec.CyP.ToString(), len);
            DataAddRow(ref Dgv_SecProp, "CzM", curSec.CzM.ToString(), len);
            DataAddRow(ref Dgv_SecProp, "CzP", curSec.CzP.ToString(), len);

            MakeEditable(ref Dgv_SecProp, 1);//使可编辑
        }

        /// <summary>
        /// 给数据表添加行数据
        /// </summary>
        /// <param name="dgv">表控件</param>
        /// <param name="head">行标题</param>
        /// <param name="val">值</param>
        /// <param name="unit">单元</param>
        public void DataAddRow(ref DataGridView dgv, string head,
            string val, string unit)
        {
            dgv.ColumnCount = 3;
    
            DataGridViewRow row = dgv.Rows[dgv.Rows.Add()];
            //row.HeaderCell.Value = head;
            row.Cells[0].Value = head;
            row.Cells[1].Value = val;
            row.Cells[2].Value = unit;
        }
        /// <summary>
        /// 设置数据表指定列为可编辑
        /// </summary>
        /// <param name="dgv">数据表</param>
        /// <param name="iCl">列号</param>
        public void MakeEditable(ref DataGridView dgv,int iCl)
        {
            
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.Cells[iCl].ReadOnly = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainForm mmf = this.Owner as MainForm;
            ModelForm1 mf = mmf.ModelForm;
            Bmodel mm = mf.CurModel;
            int secid=Convert.ToInt32( tb_SecID.Text);
            BSections curSec = mm.sections[secid];

            foreach (DataGridViewRow row in Dgv_SecProp.Rows)
            {
                string Prop=row.Cells[0].Value.ToString();
                double valu=Convert.ToDouble( row.Cells[1].Value.ToString());
                switch (Prop)
                {
                    case "A": curSec.Area = valu; break;
                    case "Iw": curSec.Iw = valu; break;
                    case "Ixx": curSec.Ixx = valu; break;
                    case "Iyy": curSec.Iyy = valu; break;
                    case "Izz": curSec.Izz = valu; break;
                    case "Cent:y": curSec.Cy = valu; break;
                    case "Cent:z": curSec.Cz = valu; break;
                    case "Shear:y": curSec.Sy = valu; break;
                    case "Shear:z": curSec.Sz = valu; break;
                    case "CyM": curSec.CyM = valu; break;
                    case "CyP": curSec.CyP = valu; break;
                    case "CzM": curSec.CzM = valu; break;
                    case "CzP": curSec.CzP = valu; break;
                    default: break;
                }
            }

            this.Close();
        }
    }
}
