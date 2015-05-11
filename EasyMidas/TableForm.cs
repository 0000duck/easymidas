using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MidasGenModel.model;
using MidasGenModel.Design;
using Xceed.DockingWindows;
using Xceed.Grid;
using Xceed.Grid.Exporting;

namespace EasyMidas
{
    public partial class TableForm : ToolWindow
    {
        public TableForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 写出单元设计组合内力表
        /// <param name="mm">模型数据</param>
        /// </summary>
        public  void writeDesignForce(ref Bmodel mm,ref BCheckModel cm,List<int> eles)
        {
            //表头
            gc_Table.Columns.Add(new Column("单元", typeof(int)));
            gc_Table.Columns.Add(new Column("位置", typeof(string)));
            gc_Table.Columns.Add(new Column("组合", typeof(string)));
            gc_Table.Columns.Add(new Column("Fx(kN)", typeof(double)));
            gc_Table.Columns.Add(new Column("Fy(kN)", typeof(double)));
            gc_Table.Columns.Add(new Column("Fz(kN)", typeof(double)));
            gc_Table.Columns.Add(new Column("Mx(kN*m)", typeof(double)));
            gc_Table.Columns.Add(new Column("My(kN*m)", typeof(double)));
            gc_Table.Columns.Add(new Column("Mz(kN*m)", typeof(double)));
            //数据写出
            foreach (int ele in eles)
            {
                printDesignForcebyEle(ele, 0, ref mm, ref cm);
                printDesignForcebyEle(ele, 4,ref mm,ref cm);
                printDesignForcebyEle(ele, 8, ref mm, ref cm);
            }
            
        }
        /// <summary>
        /// 输出指定单元指定截面的设计内力
        /// </summary>
        /// <param name="iEle">单元号</param>
        /// <param name="loc">截面位置</param>
        /// <param name="mm">模型数据</param>
        /// <param name="cm">设计数据</param>
        public void printDesignForcebyEle(int iEle, int loc, ref Bmodel mm, ref BCheckModel cm)
        {
            List<string> coms = mm.LoadCombTable.ComSteel;//钢结构设计组合表
            string sLoc = null;
            switch (loc)
            {
                case 0: sLoc = "I"; break;
                case 1: sLoc = "1/8"; break;
                case 2: sLoc = "2/8"; break;
                case 3: sLoc = "3/8"; break;
                case 4: sLoc = "4/8"; break;
                case 5: sLoc = "5/8"; break;
                case 6: sLoc = "6/8"; break;
                case 7: sLoc = "7/8"; break;
                case 8: sLoc = "J"; break;
            }
            //指定地震工况
            string LcEx = "SRSS3";
            string LcEy = "SRSS4";
            string LcEz = "Ez";
            List<string> LcEs = new List<string>() { LcEx,LcEy,LcEz};
            foreach (string com in coms)
            {
                BLoadComb curComb = mm.LoadCombTable.getLoadComb(LCKind.STEEL, com);//正常组合
                BLoadComb Comb_N = curComb.Clone() as BLoadComb;
                BLoadComb Comb_M = curComb.Clone() as BLoadComb;//弯矩调整组合
                BLoadComb Comb_V = curComb.Clone() as BLoadComb;//剪力调整组合
                if (!curComb.bACTIVE)
                    continue;

                ElemForce EFcom = mm.CalElemForceComb(curComb, iEle);
                Xceed.Grid.DataRow curRow = gc_Table.DataRows.AddNew();//添加数据行
                if (cm.QuakeAdjustFacors.ContainsKey(iEle)&curComb.hasLC(LcEs))//如果为地震组合且有放大
                {
                    BQuakeAdjustFactor qaf= cm.QuakeAdjustFacors[iEle];//放大系数
                    Comb_N.magnifyLc(LcEs,qaf.N_LC);//组合前放大
                    Comb_N.magnifyComb(qaf.N_COM);//组合后放大
                    Comb_M.magnifyLc(LcEs, qaf.M_LC);//组合前放大
                    Comb_M.magnifyComb(qaf.M_COM);//组合后放大
                    Comb_V.magnifyLc(LcEs, qaf.V_LC);//组合前放大
                    Comb_V.magnifyComb(qaf.V_COM);//组合后放大
                    curRow.BackColor = Color.AliceBlue;
                }
                ElemForce EFcom_N = mm.CalElemForceComb(Comb_N, iEle);
                ElemForce EFcom_M = mm.CalElemForceComb(Comb_M, iEle);
                ElemForce EFcom_V = mm.CalElemForceComb(Comb_V, iEle);

                curRow.Cells[0].Value = iEle;
                curRow.Cells[1].Value = sLoc;
                curRow.Cells[2].Value = com;
                curRow.Cells[3].Value = Math.Round( EFcom_N[loc].N / 1000,1);
                curRow.Cells[4].Value = Math.Round(EFcom_V[loc].Vy / 1000,1);
                curRow.Cells[5].Value = Math.Round(EFcom_V[loc].Vz / 1000,1);
                curRow.Cells[6].Value = Math.Round(EFcom[loc].T / 1000,1);
                curRow.Cells[7].Value = Math.Round(EFcom_M[loc].My / 1000,1);
                curRow.Cells[8].Value = Math.Round(EFcom_M[loc].Mz / 1000,1);
                curRow.EndEdit();//完成数据行编辑

                //if (cm.QuakeAdjustFacors.ContainsKey(iEle) & curComb.hasLC(LcEs))//如果为地震组合且有放大
                //{
                //    //调整前数据
                //    Xceed.Grid.DataRow curRowN = gc_Table.DataRows.AddNew();//添加数据
                //    curRowN.BackColor = Color.Red;
                //    curRowN.Cells[0].Value = iEle;
                //    curRowN.Cells[1].Value = sLoc;
                //    curRowN.Cells[2].Value = com;
                //    curRowN.Cells[3].Value = EFcom[loc].N / 1000;
                //    curRowN.Cells[4].Value = EFcom[loc].Vy / 1000;
                //    curRowN.Cells[5].Value = EFcom[loc].Vz / 1000;
                //    curRowN.Cells[6].Value = EFcom[loc].T / 1000;
                //    curRowN.Cells[7].Value = EFcom[loc].My / 1000;
                //    curRowN.Cells[8].Value = EFcom[loc].Mz / 1000;
                //    curRowN.EndEdit();//完成数据行编辑                 
                //}
            }
        }

        private void 导出ExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "请输入Excel文件存储位置";
            sfd.Filter = "xml 文件(*.xml)|*.xml|All files (*.*)|*.*";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                ExcelExporter ee = new ExcelExporter();
                ee.Export(gc_Table, sfd.FileName);

                MessageTools mt = this.DockLayoutManager.ToolWindows["MessageTool"] as MessageTools;
                string Sout = string.Format("**Excel文件保存至：{0}!", sfd.FileName);
                mt.Tb_out.AppendText(Environment.NewLine + Sout);
            }
        }
    }
}
