using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MidasGenModel.model;
using Xceed.Grid;
using Xceed.Grid.Editors;

namespace EasyMidas
{
    public partial class LoadComForm : Form
    {
        public LoadComForm()
        {
            InitializeComponent();
        }

        //初始化荷载组合表
        public void InitLoadComForm()
        {
            MainForm mf = this.Owner as MainForm;
            Bmodel mm = mf.ModelForm.CurModel;
            BLoadCombTable lct = mm.LoadCombTable;//荷载组合表

            gv_LC.FixedColumnSplitter.Visible = false;
            gv_LC.Columns.Add(new Column("名称", typeof(string)));
            gv_LC.Columns.Add(new Column("激活",typeof(string)));
            gv_LC.Columns.Add(new Column("地震组合", typeof(string)));
            gv_LC.Columns.Add(new Column("说明", typeof(string)));

            List<string> GenCom = lct.ComGen;

            for (int i = 0; i < GenCom.Count; i++)
            {
                BLoadComb curCom = lct.getLoadComb(LCKind.GEN, GenCom[i]);//取得荷载组合
                Xceed.Grid.DataRow Row = gv_LC.DataRows.AddNew();
                Row.Cells[0].Value = GenCom[i];
                Row.Cells[1].Value = curCom.bACTIVE ? "激活":"钝化";
                Row.Cells[2].Value = curCom.bES ? "是":"否";
                Row.Cells[3].Value = curCom.DESC;
                Row.BackColor = System.Drawing.Color.DarkSeaGreen;

                Row.EndEdit();
            }
            //gv_LC.ReadOnly = false;
            //gv_LC.Columns[2].CellEditorManager = new CheckBoxEditor();
            //gv_LC.Columns[2].CellEditorDisplayConditions = CellEditorDisplayConditions.Always;
        }
    }
}
