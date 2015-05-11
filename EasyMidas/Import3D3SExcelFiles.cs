using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExcelLibrary.BinaryFileFormat;
using ExcelLibrary.SpreadSheet;
using MidasGenModel.model;
using MidasGenModel.Tools;
using System.IO;


namespace EasyMidas
{
    public partial class Import3D3SExcelFiles : Form
    {
        private double RaioUnit = 1;//长度单位调整系数

        public Import3D3SExcelFiles()
        {
            InitializeComponent();
        }

        //点击开始时
        private void bt_run_Click(object sender, EventArgs e)
        {
            string fileN = textBox1.Text;//节点文件路径
            string fileE = textBox2.Text;//单元文件路径

            RefreshUnitRatio();//更新长度单位系数

            MainForm mmf = this.Owner as MainForm;//主窗口
            ModelForm1 mf = mmf.ModelForm;
            TextBox messagebox = mmf.MessageTool.Tb_out;//信息窗口

            Bmodel MyModel=new Bmodel();//模型数据库

            Workbook bookN = Workbook.Load(fileN);//节点数据表
            Workbook bookE = Workbook.Load(fileE);//节点数据表


            #region 节点表信息读取及处理
            //节点表信息读取及处理
            foreach (Worksheet ws in bookN.Worksheets)
            {
                if (ws.Name == "节点")
                    ReadNode1(ws, ref MyModel);//读取“节点”表
                else if (ws.Name == "节点荷载"||ws.Name=="节点导荷载")
                    ReadNode2(ws,ref MyModel);//读取“节点荷载”表/“节点导荷载”表
            }
            messagebox.AppendText(Environment.NewLine + "读取节点表信息完成!");
            #endregion
            #region 单元表信息读取及处理
            //单元表信息读取及处理
            foreach (Worksheet ws in bookE.Worksheets)
            {
                if (ws.Name == "单元")
                    ReadElem1(ws, ref MyModel);
                else if (ws.Name == "单元荷载")
                    ReadElem2(ws, ref MyModel);
                else if (ws.Name == "杆件导荷载")
                    ReadElem2(ws, ref MyModel);
                else if (ws.Name == "计算长度")
                    ReadElem3(ws, ref MyModel);
            }
            messagebox.AppendText(Environment.NewLine + "读取单元表信息完成!");
            #endregion

            MyModel.UNIT.Force = "KN";//指定力为KN
            mf.CurModel = MyModel;//存储模型入数据库
            messagebox.AppendText(Environment.NewLine + 
                "导入模型："+MyModel.nodes.Count+"节点,"+MyModel.elements.Count+"单元");
            //this.Close();
        }

        /// <summary>
        /// 读取单元信息表1
        /// </summary>
        public void ReadElem1(Worksheet Esheet0,ref Bmodel MyModel)
        {
            int iFrow = Esheet0.Cells.FirstRowIndex;
            int iLrow = Esheet0.Cells.LastRowIndex;
            //材料信息读取
            for (int i = iFrow + 1; i <= iLrow; i++)
            {
                Row CurRow = Esheet0.Cells.GetRow(i);
                int imat = int.Parse(CurRow.GetCell(4).StringValue);//材料性质id
                string Cursec = CurRow.GetCell(7).StringValue;//截面名称

                int iele = int.Parse(CurRow.GetCell(0).StringValue);
                int ieleI = int.Parse(CurRow.GetCell(1).StringValue);
                int ieleJ = int.Parse(CurRow.GetCell(2).StringValue);
                int MaxProp = MyModel.sections.Count > 0 ? MyModel.sections.Keys.Max() : 0;//记录最大截面号
                int iProp = 1;//当前截面号
                bool hasSec = false;//指示是否有当前截面
                BMaterial mat = new BMaterial(imat, MatType.USER, "Mat_" + imat.ToString());
                foreach (KeyValuePair<int, BSections> ss in MyModel.sections)
                {
                    if (ss.Value.Name == Cursec)
                    {
                        iProp = ss.Key;
                        hasSec = true;//有当前截面
                        break;
                    }
                }
                //如果没有当前截面，则添加一个新的截面
                if (hasSec == false)
                {
                    iProp = MaxProp + 1;//新的截面号
                    SectionGeneral sec = new SectionGeneral(iProp, Cursec);
                    MyModel.AddSection(sec);//添加入库
                }

                MyModel.AddMat(mat);//添加料号入库
                //最后添加单元入数据库
                FrameElement ee = new FrameElement(iele, ElemType.BEAM, imat, iProp, ieleI, ieleJ);
                MyModel.AddElement(ee);
            }
        }
        /// <summary>
        /// 读取梁单元荷荷载信息表“单元荷载”、"杆单元导荷载"
        /// </summary>
        /// <param name="ws">工作表</param>
        /// <param name="mm">读入的数据库</param>
        public void ReadElem2(Worksheet ws,ref Bmodel mm)
        {
            int iFrow = ws.Cells.FirstRowIndex;//起始行号
            int iLrow = ws.Cells.LastRowIndex;//终止行号
            //进行逐行读取和处理
            for (int i = iFrow + 1; i <= iLrow; i++)
            {
                Row CurRow = ws.Cells.GetRow(i);//当前行
                int iele = int.Parse(CurRow.GetCell(0).StringValue);//单元号
                string lc = CurRow.GetCell(3).StringValue;//工况名称
                string lctype = CurRow.GetCell(4).StringValue;//荷载类型号3d3s
                double Q1 = double.Parse(CurRow.GetCell(6).StringValue);//荷载数据
                double Q2 = double.Parse(CurRow.GetCell(7).StringValue);
                double X1 = double.Parse(CurRow.GetCell(8).StringValue);
                double X2 = double.Parse(CurRow.GetCell(9).StringValue);
                double eleL = mm.getFrameLength(iele);//单元长度

                BBLoad bbl = new BBLoad();
                bbl.ELEM_num = iele;
                bbl.LC = lc;
                switch (lctype)
                {
                    case "1":
                        bbl.TYPE = BeamLoadType.UNILOAD;
                        if (X2 == 0 || Math.Abs(X2 - eleL) / eleL < 0.001 || X2 > eleL)
                            X2 = 1;
                        else
                            X2 = X2 / eleL;
                        bbl.setLoadData(X1, Q1, X2, Q2, 0, 0, 0, 0);
                        break;
                    case "5":
                        bbl.TYPE = BeamLoadType.UNILOAD;
                        if (X2 == 0&&X1==0)
                        {
                            X2 = 1;
                            bbl.setLoadData(X1, Q1, X2, Q2, 0, 0, 0, 0);
                        }
                        else if (X1 != 0 && X2 == 0&&Q2==0)
                        {
                            if (X1>=eleL)
                                bbl.setLoadData(0, 0, 1, Q1, 0, 0, 0, 0);
                            else
                                bbl.setLoadData(0, 0, X1/eleL, Q1, 1, 0, 0, 0);
                        }
                        else if (X1 == X2)
                        {
                            bbl.setLoadData(0, 0, 1, Q1, 0, 0, 0, 0);
                        }
                        else
                            bbl.setLoadData(0, 0, X1 / X2, Q1, 1, 0, 0, 0);
                        break;
                    case "6":
                        if (Q2 == 0)
                            Q2 = Q1;
                        bbl.TYPE = BeamLoadType.UNILOAD;
                        bbl.setLoadData(0, 0, X1 / eleL, Q1, X2/eleL, Q2, 1, 0);
                        break;
                    default :
                        bbl.TYPE = BeamLoadType.UNILOAD;
                        break;
                }
                
                bbl.setLoadDir("GZ");

                mm.AddElemLoad(bbl);//添加到数据库
            }
        }

        /// <summary>
        /// 读取单元信息表3:读取分层信息为单元分组
        /// </summary>
        public void ReadElem3(Worksheet Esheet0, ref Bmodel MyModel)
        {
            int iFrow = Esheet0.Cells.FirstRowIndex;
            int iLrow = Esheet0.Cells.LastRowIndex;
            //信息读取
            //材料信息读取
            for (int i = iFrow + 1; i <= iLrow; i++)
            {
                Row CurRow = Esheet0.Cells.GetRow(i);
                int iele = int.Parse(CurRow.GetCell(0).StringValue);
                string group = CurRow.GetCell(1).StringValue;//组名
                MyModel.AddElemToGroup(iele, group);
            }
        }
        /// <summary>
        /// 读取节点信息表“节点”
        /// </summary>
        /// <param name="ws">工作表</param>
        /// <param name="mm">读入的数据库</param>
        public void ReadNode1(Worksheet ws, ref Bmodel mm)
        {
            int iFrow = ws.Cells.FirstRowIndex;
            int iLrow = ws.Cells.LastRowIndex;

            for (int i = iFrow + 1; i <= iLrow; i++)
            {
                Row CurRow = ws.Cells.GetRow(i);
                int inode = int.Parse(CurRow.GetCell(0).StringValue);
                double x = double.Parse(CurRow.GetCell(1).StringValue);
                double y = double.Parse(CurRow.GetCell(2).StringValue);
                double z = double.Parse(CurRow.GetCell(3).StringValue);
                Bnodes nn = new Bnodes(inode, x/RaioUnit, y/RaioUnit, z/RaioUnit);//进行长度单位转换
                mm.AddNode(nn);
            }
        }

        /// <summary>
        /// 读取节点信息表“节点荷载”
        /// </summary>
        /// <param name="ws">工作表</param>
        /// <param name="mm">读入的数据库</param>
        public void ReadNode2(Worksheet ws, ref Bmodel mm)
        {
            int iFrow = ws.Cells.FirstRowIndex;//起始行
            int iLrow = ws.Cells.LastRowIndex;//终止行

            for (int i = iFrow + 1; i <= iLrow; i++)
            {
                Row CurRow = ws.Cells.GetRow(i);//当前行
                int inode = int.Parse(CurRow.GetCell(0).StringValue);
                string lc = CurRow.GetCell(3).StringValue;//工况名称
                double Px = double.Parse(CurRow.GetCell(4).StringValue);//荷载数据
                double Py = double.Parse(CurRow.GetCell(5).StringValue);
                double Pz = double.Parse(CurRow.GetCell(6).StringValue);
                double Mx = double.Parse(CurRow.GetCell(7).StringValue);
                double My = double.Parse(CurRow.GetCell(8).StringValue);
                double Mz = double.Parse(CurRow.GetCell(9).StringValue);

                BNLoad nld = new BNLoad(inode);
                nld.LC = lc;
                nld.SetLoadValue(Px, Py, Pz, Mx, My, Mz);
                mm.AddNodeLoad(nld);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog OPD = new OpenFileDialog();
            OPD.Title = "打开3d3s节点信息文件";
            OPD.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            OPD.Filter = "excel2003 文件(*.xls)|*.xls|All files (*.*)|*.*";

            OPD.RestoreDirectory = true;
            if (OPD.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = OPD.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog OPD = new OpenFileDialog();
            OPD.Title = "打开3d3s单元信息文件";
            OPD.InitialDirectory = Environment.GetFolderPath( Environment.SpecialFolder.MyComputer);
            OPD.Filter = "excel2003 文件(*.xls)|*.xls|All files (*.*)|*.*";

            OPD.RestoreDirectory = true;
            if (OPD.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = OPD.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //更新长度系数
        private void RefreshUnitRatio()
        {
            if (rb_mm.Checked)
                RaioUnit = 1000;
            else if (rb_cm.Checked)
                RaioUnit = 100;
            else if (rb_m.Checked)
                RaioUnit = 1;
        }
    }
}
