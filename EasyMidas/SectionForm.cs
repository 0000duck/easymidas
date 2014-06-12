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
    public partial class SectionForm : Form
    {
        public SectionForm()
        {
            InitializeComponent();
        }

        //初始化截面表控制件
        public void InitSectList()
        {
            MainForm mf = this.Owner as MainForm;
            Bmodel mm = mf.ModelForm.CurModel;

            Dgv_sec.RowCount=mm.sections.Count;
            Dgv_sec.ColumnCount=3;
            //不可调整表头高度
            Dgv_sec.ColumnHeadersHeightSizeMode = 
                DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            //禁止列排序
            Dgv_sec.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            Dgv_sec.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            Dgv_sec.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            int i=0;

            foreach (BSections sec in mm.sections.Values)
            {
                Dgv_sec.CurrentCell = Dgv_sec[0, i];
                Dgv_sec.CurrentCell.Value = sec.Num.ToString();
                Dgv_sec.CurrentCell = Dgv_sec[1, i];
                Dgv_sec.CurrentCell.Value = sec.Name;
                Dgv_sec.CurrentCell = Dgv_sec[2, i];
                Dgv_sec.CurrentCell.Value = sec.TYPE.ToString();
                Dgv_sec.CurrentRow.Height = 18;//设置行高
                i++;
            }
            if (Dgv_sec.Rows.Count > 0)
            {
                Dgv_sec.CurrentCell = Dgv_sec[0, 0];//选择第一行
            }
        }

        private void bt_EditSec_Click(object sender, EventArgs e)
        {
            SecEditForm sef = new SecEditForm();
            sef.Owner = this.Owner;
            //取得当前选择号
            string ss = Dgv_sec.SelectedRows[0].Cells[0].Value.ToString();
            int CurId=Convert.ToInt16(ss);
            sef.UpdateCurentSec(CurId);
            sef.ShowDialog();//显示模态对话框
        }
    }
}
