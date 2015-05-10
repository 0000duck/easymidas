using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data;
using MidasGenModel.Design;
using MidasGenModel.Geometry3d;
using MidasGenModel.DelaunayTriangulator;
using System.Data.Common;

namespace MidasGenModel.model
{
    /// <summary>
    /// 模型类：封装所有数据信息
    /// </summary>
    [Serializable]
    public class Bmodel : Object
    {
        #region 成员
        /// <summary>
        /// 单位系统
        /// </summary>
        public BUNIT unit;
        /// <summary>
        /// 节点信息列表
        /// </summary>
        public SortedList<int, Bnodes> nodes;
        /// <summary>
        /// 单元信息列表
        /// </summary>
        public SortedList<int, Element> elements;
        /// <summary>
        /// 截面信息列表
        /// </summary>
        public SortedList<int, BSections> sections;
        /// <summary>
        /// 板单元厚度表
        /// </summary>
        public SortedList<int, BThickness> thickness;

        /// <summary>
        /// 约束信息
        /// </summary>
        public SortedList<int, BConstraint> constraint;
        //public List<BConstraint> constraint;

        /// <summary>
        /// 荷载工况列表
        /// </summary>
        public List<BLoadCase> STLDCASE;

        /// <summary>
        /// 荷载组合列表
        /// </summary>
        private BLoadCombTable _LoadCombTable;

        //荷载表
        private BLoadTable _LoadTable;
        /// <summary>
        /// 节点荷载链表----计划用_LoadTable替代
        /// </summary>
        public SortedList<int, BNLoad> conloads;

        /// <summary>
        /// 梁单元荷载链表----计划用_LoadTable替代
        /// </summary>
        public SortedList<int, BBLoad> beamloads;



        /// <summary>
        /// 自重荷载信息链表
        /// </summary>
        public SortedList<string, BWeight> selfweight;

        /// <summary>
        /// 材料信息链表
        /// </summary>
        public SortedList<int, BMaterial> mats;

        /// <summary>
        /// 单元内力链表
        /// </summary>
        public SortedList<int, BElemForceTable> elemforce;
        #endregion

        #region 荷载边界相关成员
        private SortedList<int, BETLoad> _EleTempLoads;//单元温度荷载
        private SortedList<string, BSGroup> _StruGroups;//结构组链表
        private SortedList<int, BRigidLink> _RigidLinks;//刚性连接表
        #endregion

        #region 属性
        /// <summary>
        /// 荷载组合字典
        /// </summary>
        public BLoadCombTable LoadCombTable
        {
            get { return _LoadCombTable; }
        }
        /// <summary>
        /// 荷载表字典
        /// </summary>
        public BLoadTable LoadTable
        {
            get { return _LoadTable; }
        }
        /// <summary>
        /// 组构组链表
        /// </summary>
        public SortedList<string, BSGroup> StruGroups
        {
            get { return _StruGroups; }
        }
        /// <summary>
        /// 刚性连接链表
        /// </summary>
        public SortedList<int, BRigidLink> RigidLinks
        {
            get { return _RigidLinks; }
        }

        /// <summary>
        /// 模型的最大节点号
        /// </summary>
        public int MaxNode
        {
            get
            {
                return nodes.Keys[nodes.Keys.Count - 1];
            }
        }
        /// <summary>
        /// 模型最大单元号
        /// </summary>
        public int MaxElem
        {
            get
            {
              return  elements.Keys[elements.Keys.Count-1];
            }
        }

        /// <summary>
        /// 模型的单位系统
        /// </summary>
        public BUNIT UNIT
        {
            get { return unit; }
        }
        #endregion
        #region 设计相关成员与属性
        private SortedList<int, Bk_Factor> _K_Factors;//计算长度系数链表
        /// <summary>
        /// 计算长度系数链表
        /// </summary>
        public SortedList<int, Bk_Factor> K_Factors
        {
            get { return _K_Factors; }
        }
        private SortedList<int, BUnsupportedLen> _Lengths;//计算长度链表
        /// <summary>
        /// 计算长度链表
        /// </summary>
        public SortedList<int, BUnsupportedLen> Lengths
        {
            get { return _Lengths; }
        }
        private SortedList<int, BLimitsRatio> _LimitsRatios;//极限长细比链表
        /// <summary>
        /// 极限长细比链表
        /// </summary>
        public SortedList<int, BLimitsRatio> LimitsRatios
        {
            get { return _LimitsRatios; }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public Bmodel()
        {
            unit = new BUNIT();

            nodes = new SortedList<int, Bnodes>();
            elements = new SortedList<int, Element>();
            sections = new SortedList<int, BSections>();
            thickness = new SortedList<int, BThickness>();

            constraint = new SortedList<int,BConstraint> ();

            STLDCASE = new List<BLoadCase>();
            _LoadCombTable = new BLoadCombTable();//荷载组合表
            _LoadTable = new BLoadTable();//所有荷载表
            conloads = new SortedList<int, BNLoad>(new RepeatedKeySort());//节点荷载
            beamloads = new SortedList<int, BBLoad>(new RepeatedKeySort());//梁单元荷载
            _EleTempLoads = new SortedList<int, BETLoad>(new RepeatedKeySort());//单元温度荷载
            selfweight = new SortedList<string, BWeight>();//自重信息
            mats = new SortedList<int, BMaterial>();//材料信息

            elemforce = new SortedList<int, BElemForceTable>();//单元内力表

            _StruGroups = new SortedList<string, BSGroup>();//结构组表
            _RigidLinks = new SortedList<int, BRigidLink>();//刚性连接表
            #region 设计参数
            _K_Factors = new SortedList<int, Bk_Factor>();
            _Lengths = new SortedList<int, BUnsupportedLen>();
            _LimitsRatios = new SortedList<int, BLimitsRatio>();
            #endregion
        }
        #endregion

        #region 模型处理方法
        /// <summary>
        /// 转化梁单元关键点信息
        /// </summary>
        public void GenBeamKpoint()
        {
            int Nnodes = 99999;//模型节点数基数，用于方向点的起始编号
            int i = 1;
            foreach (Element elee in this.elements.Values)
            {
                FrameElement ele = elee as FrameElement;
                if (ele != null && ele.beta != 0 && ele.iNs.Count < 3)
                {
                    Bnodes ndi = this.nodes[(int)ele.iNs[0]];//得到节点i
                    Bnodes ndj = this.nodes[(int)ele.iNs[1]];//得到节点j

                    if (ndi.X == ndj.X && ndi.Y == ndj.Y)
                    {
                        //如果单元坐标系x轴平行于全局坐标系z轴
                        Point3d pto = new Point3d(ndi.X + 10000, ndi.Y, ndi.Z);
                        Point3d pt1 = new Point3d(ndi.X, ndi.Y, ndi.Z);
                        Point3d pt2 = new Point3d(ndj.X, ndj.Y, ndj.Z);

                        Point3d ptk = RotNodebyAxis(pto, pt1, pt2, -ele.beta * Math.PI / 180);//求得方向点

                        Bnodes ndk = new Bnodes(Nnodes + i, ptk.X, ptk.Y, ptk.Z);
                        nodes.Add(Nnodes + i, ndk);//添加方向节点进模型数据库

                        ele.iNs.Add(Nnodes + i);// 添加方向节点号到单元数据
                    }
                    else
                    {
                        //如果单元坐标系x轴不平行于全局坐标系z轴
                        Point3d pto = new Point3d(ndi.X, ndi.Y, ndi.Z + 10000);
                        Point3d pt1 = new Point3d(ndi.X, ndi.Y, ndi.Z);
                        Point3d pt2 = new Point3d(ndj.X, ndj.Y, ndj.Z);

                        Point3d ptk = RotNodebyAxis(pto, pt1, pt2, -ele.beta * Math.PI / 180);//求得方向点

                        Bnodes ndk = new Bnodes(Nnodes + i, ptk.X, ptk.Y, ptk.Z);
                        nodes.Add(Nnodes + i, ndk);//添加方向节点进模型数据库

                        ele.iNs.Add(Nnodes + i);// 添加方向节点号到单元数据
                    }

                    i++;
                }
            }
        }
        /// <summary>
        /// 计算空间点绕任意轴旋转任意角度后的结果
        /// </summary>
        /// <param name="pt_original">原始节点</param>
        /// <param name="pt1_Axis">转轴起点</param>
        /// <param name="pt2_Axis">转轴终点</param>
        /// <param name="A">转角,以弧度计量</param>
        /// <returns>旋转后的节点###结果为转轴方向面向观察者，顺时针转去得到</returns>
        public static Point3d RotNodebyAxis(Point3d pt_original, Point3d pt1_Axis, Point3d pt2_Axis, double A)
        {
            //轴向量
            double x0 = pt2_Axis.X - pt1_Axis.X;
            double y0 = pt2_Axis.Y - pt1_Axis.Y;
            double z0 = pt2_Axis.Z - pt1_Axis.Z;
            Vector3 vf = Vector3.Normalize(new Vector3(x0, y0, z0));//单位化向量
            Vector3 vr = vf.CrossProduct(new Vector3(1, 0, 0));
            Vector3 vup = vf.CrossProduct(vr);

            RtwMatrix m = new RtwMatrix(3, 3);
            m[0, 0] = (float)vr.X;
            m[1, 0] = (float)vr.Y;
            m[2, 0] = (float)vr.Z;

            m[0, 1] = (float)vup.X;
            m[1, 1] = (float)vup.Y;
            m[2, 1] = (float)vup.Z;

            m[0, 2] = (float)vf.X;
            m[1, 2] = (float)vf.Y;
            m[2, 2] = (float)vf.Z;

            RtwMatrix im = new RtwMatrix(3, 3);
            im = ~m;//矩阵转置

            RtwMatrix zrot = new RtwMatrix(3, 3);
            zrot[2, 2] = 1;
            zrot[0, 0] = (float)Math.Cos(A);
            zrot[0, 1] = (float)Math.Sin(A);
            zrot[1, 0] = (float)-Math.Sin(A);
            zrot[1, 1] = (float)Math.Cos(A);

            RtwMatrix M, mtemp = new RtwMatrix(3, 3);//变换矩阵
            mtemp = m * zrot;
            M = mtemp * im;

            //Tools.WriteMessage("\nM:\n"+M.ToString());
            Point3d pttemp = new Point3d(pt_original.X - pt1_Axis.X, pt_original.Y - pt1_Axis.Y, pt_original.Z - pt1_Axis.Z);

            double x1 = M[0, 0] * pttemp[0] + M[0, 1] * pttemp[1] + M[0, 2] * pttemp[2];
            double y1 = M[1, 0] * pttemp[0] + M[1, 1] * pttemp[1] + M[1, 2] * pttemp[2];
            double z1 = M[2, 0] * pttemp[0] + M[2, 1] * pttemp[1] + M[2, 2] * pttemp[2];

            x1 = x1 + pt1_Axis.X;
            y1 = y1 + pt1_Axis.Y;
            z1 = z1 + pt1_Axis.Z;
            Point3d pt_out = new Point3d(x1, y1, z1);

            //Tools.WriteMessage("计算点坐标"+pt_out.ToString());
            return pt_out;//反回坐标点
        }

        /// <summary>
        /// 刷新单元局部坐标系
        /// 2010.05.25
        /// </summary>
        public void RefreshESC()
        {
            foreach (Element elee in this.elements.Values)
            {
                if (elee is FrameElement)//梁单元
                {
                    CoordinateSystem cs = new CoordinateSystem();
                    FrameElement ele = elee as FrameElement;
                    int iNum = ele.iNs[0];
                    int jNum = ele.iNs[1];

                    Point3d pti = new Point3d(nodes[iNum].X, nodes[iNum].Y, nodes[iNum].Z);
                    Point3d ptj = new Point3d(nodes[jNum].X, nodes[jNum].Y, nodes[jNum].Z);
                    Vector3 Vecx = pti.GetVectorTo(ptj);
                    Vecx.Normalize();//归一化
                    Vector3 Vz = new Vector3();//方向向量
                    if (Vecx.X == 0 && Vecx.Y == 0)
                    {
                        Vz = Vector3.xAxis;
                    }
                    else
                    {
                        Vz = Vector3.zAxis;
                    }

                    //方向向量按β角旋转
                    RtwMatrix Mz = MatrixFactory.GetRotationMartrix(Vecx, ele.beta) * Vz.ToMatrix();
                    Vz = new Vector3(Mz[0, 0], Mz[1, 0], Mz[2, 0]);//更新旋转后的结果
                    Vector3 Vy = Vz.CrossProduct(Vecx);
                    Vy.Normalize();//归一化

                    cs.Origin = pti;
                    cs.AxisX = Vecx;
                    cs.AxisY = Vy;
                    ele.ECS = cs;//更新单元坐标系
                }
                else if (elee is PlanarElement)//平面单元
                {
                    //to do:
                }
            }
        }

        /// <summary>
        ///对模型数据进行标准化处理
        /// </summary>
        public void Normalize()
        {
            //截面类型标准化
            foreach (BSections sec in sections.Values)
            {
                //解决箱形截面当用户没有输入tf2时对截面数据进行标准化
                if (sec is SectionDBuser)
                {
                    if (sec.SSHAPE == SecShape.B && (double)sec.SEC_Data[6] == 0
                    && (double)sec.SEC_Data[4] != 0)
                    {
                        sec.SEC_Data[6] = sec.SEC_Data[4];
                    }
                    //解决槽钢截面当用户没有输入tf2时对截面数据进行标准化
                    else if (sec.SSHAPE == SecShape.C && (double)sec.SEC_Data[5] == 0
                        && (double)sec.SEC_Data[2] != 0)
                    {
                        sec.SEC_Data[5] = sec.SEC_Data[2];
                        sec.SEC_Data[6] = sec.SEC_Data[4];
                    }
                }
                sec.CalculateSecProp();//计算截面特性
                //todo:当输入截面为数据库截面时，进行截面参数转化
            }

            //标准化材料特性
            foreach (BMaterial mat in mats.Values)
            {
                mat.NormalizeProp();
            }

            //更新单元局部坐标系
            RefreshESC();

            //更新最新的荷载表数据
            _LoadTable.UpdateNodeLoadList(this.STLDCASE, this.conloads);
            _LoadTable.UpdateElemLoadList(this.STLDCASE, this.beamloads);
        }

        /// <summary>
        /// 添加荷载组合入模型
        /// </summary>
        /// <param name="com"></param>
        public void AddLoadComb(BLoadComb com)
        {
            _LoadCombTable.Add(com);
        }

        /// <summary>
        /// 计算指定单元的单元组合内力
        /// （组合形式为包络和ABS时计算功能未实现)
        /// </summary>
        /// <param name="com">组合名称</param>
        /// <param name="iElem">单元号</param>
        /// <returns>单元内力</returns>
        public ElemForce CalElemForceComb(BLoadComb com, int iElem)
        {
            ElemForce res = new ElemForce();//要返回的结果

            List<BLCFactGroup> comdata = com.LoadCombData;
            if (com.iTYPE == 0)//如果为线性组合
            {
                foreach (BLCFactGroup lfg in comdata)
                {
                    ElemForce ef = new ElemForce();
                    if (lfg.ANAL == ANAL.CB || lfg.ANAL == ANAL.CBS)
                    {
                        ef = this.CalElemForceComb(
                            _LoadCombTable.getLoadComb(com.KIND,lfg.LCNAME), 
                            iElem);//迭归组合
                    }
                    else if (lfg.ANAL == ANAL.RS)
                    {
                        ef = this.elemforce[iElem].LCForces[lfg.LCNAME + "(RS)"];
                    }
                    else
                    {
                        ef = elemforce[iElem].LCForces[lfg.LCNAME];//当前组合单元力
                    }
                    res = res + ef.Mutiplyby(lfg.FACT);
                }
            }
            else if (com.iTYPE == 1)//如果为包络
            {
                
            }
            else if (com.iTYPE == 2)//如果为ABS
            {

            }
            else if (com.iTYPE == 3)//如果为平方开根号
            {
                foreach (BLCFactGroup lfg in comdata)
                {
                    ElemForce ef = new ElemForce();
                    if (lfg.ANAL == ANAL.CB || lfg.ANAL == ANAL.CBS)
                    {
                        ef = this.CalElemForceComb(
                            _LoadCombTable.getLoadComb(com.KIND,lfg.LCNAME),
                            iElem);//迭归组合
                    }
                    else if (lfg.ANAL == ANAL.RS)
                    {
                        ef = this.elemforce[iElem].LCForces[lfg.LCNAME + "(RS)"];
                    }
                    else
                    {
                        ef = elemforce[iElem].LCForces[lfg.LCNAME];//当前组合单元力
                    }
                    res = res + (ef.Mutiplyby(lfg.FACT)).Pow(2);//平方和
                }
                res = res.Pow(0.5);//开根号
            }
            return res;
        }

        /// <summary>
        /// 清理所有模型数据
        /// </summary>
        public void Reset()
        {
            unit = new BUNIT();

            nodes = new SortedList<int, Bnodes>();
            elements = new SortedList<int, Element>();
            sections = new SortedList<int, BSections>();
            thickness = new SortedList<int, BThickness>();

            constraint = new SortedList<int,BConstraint> ();

            STLDCASE = new List<BLoadCase>();
            _LoadCombTable = new BLoadCombTable();//荷载组合表
            conloads = new SortedList<int, BNLoad>(new RepeatedKeySort());//节点荷载
            beamloads = new SortedList<int, BBLoad>(new RepeatedKeySort());//梁单元荷载
            selfweight = new SortedList<string, BWeight>();//自重信息
            mats = new SortedList<int, BMaterial>();//材料信息

            elemforce = new SortedList<int, BElemForceTable>();//单元内力表
        }

        /// <summary>
        /// 取得线单元的长度
        /// </summary>
        /// <param name="iEle">线单元号</param>
        /// <returns></returns>
        public double getFrameLength(int iEle)
        {
            double res = 0;

            if (this.elements[iEle] is FrameElement)
            {
                FrameElement ele = this.elements[iEle] as FrameElement;
                res = this.nodes[ele.iNs[0]].DistanceTo(this.nodes[ele.iNs[1]]);
            }
            return res;
        }

        /// <summary>
        /// 取得线单元的方向向量（单位向量）：由I到J
        /// </summary>
        /// <param name="iEle">单元号</param>
        /// <returns>单位方向向量</returns>
        public Vector3 getFrameVec(int iEle)
        {
            Vector3 Res = new Vector3();
            if (this.elements[iEle] is FrameElement)
            {
                FrameElement fme = this.elements[iEle] as FrameElement;
                Res = this.nodes[fme.I].VectorTo(this.nodes[fme.J]);
            }
            Res.Normalize();//单位向量化
            return Res;
        }

        /// <summary>
        /// 由截面号返回单元号列表
        /// </summary>
        /// <param name="iSec">截面号</param>
        /// <returns>单元号列表</returns>
        public List<int> getElemBySec(int iSec)
        {
            List<int> Res = new List<int>();
            foreach (Element ele in this.elements.Values)
            {
                if (ele.iPRO == iSec && ele is FrameElement)
                {
                    Res.Add(ele.iEL);
                }
            }
            return Res;
        }

        /// <summary>
        /// 地震反应谱组合和静力组合激活相互切换
        /// </summary>
        /// <param name="bActive">是否激活地震组合</param>
        public void RSCombineActive(bool bActive)
        {
            List<string> coms = LoadCombTable.ComSteel;//钢结构设计组合
            if (bActive == true)
            {//激活地震组合
                foreach (string com in coms)
                {
                    BLoadComb lc = LoadCombTable.getLoadComb(LCKind.STEEL,com);
                    if (lc.hasLC_ANAL(ANAL.RS) || lc.hasLC_ANAL(ANAL.ES))
                    {
                        LoadCombTable.setActive(LCKind.STEEL, com, true);
                    }
                    else
                    {
                        LoadCombTable.setActive(LCKind.STEEL, com, false);
                    }
                }
            }
            else
            {//激活非地震组合
                foreach (string com in coms)
                {
                    BLoadComb lc = LoadCombTable.getLoadComb(LCKind.STEEL,com);
                    if (lc.hasLC_ANAL(ANAL.RS) || lc.hasLC_ANAL(ANAL.ES))
                    {
                        LoadCombTable.setActive(LCKind.STEEL, com, false);
                    }
                    else
                    {
                        LoadCombTable.setActive(LCKind.STEEL, com, true);
                    }
                }
            }
        }

        /// <summary>
        /// 由三个坐标求得与(x,y,z)最近的节点号
        /// 2011.05.27
        /// </summary>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        /// <param name="z">z坐标</param>
        /// <returns></returns>
        public int getNode(double x, double y, double z)
        {
            int res;
            Bnodes nc = new Bnodes(0, x, y, z);//当前节点号
            if (nodes.Count < 1)
                return -1;//如果节点总数为0，则反回-1
            double NodeDis = nc.DistanceTo(this.nodes.Values[0]);
            res = this.nodes.Values[0].num;
            foreach (Bnodes nd in this.nodes.Values)
            {
                double temp = nd.DistanceTo(nc);
                if (NodeDis > temp)
                {
                    NodeDis = temp;
                    res = nd.num;
                }
            }
            return res;
        }
        /// <summary>
        /// 取得某个单元的计算长度（单位m）
        /// </summary>
        /// <param name="iElem">单元号</param>
        /// <param name="ly">y轴计算长度</param>
        /// <param name="lz">z轴计算长度</param>
        /// <returns>指示是否长度进行了人为指定，不指定返回将是单元的物理长度</returns>
        public bool getEleLyLz(int iElem,out double ly, out double lz)
        {
            bool res = false;
            double eleLeng = this.getFrameLength(iElem);//单元长度
            ly = eleLeng;//默认取单元自身长度
            lz = eleLeng;

            //如果指定的单元计算长度，则按单元计算长度
            if (this.Lengths.ContainsKey(iElem))
            {
                ly = this.Lengths[iElem].Ly;
                lz = this.Lengths[iElem].Lz;
                res = true;
            }
            else if (this.K_Factors.ContainsKey(iElem))
            {
                ly = eleLeng * this.K_Factors[iElem].Ky;
                lz = eleLeng * this.K_Factors[iElem].Kz;
                res = true;
            }
            return res;//指示是否进行了人为指定计算长度
        }

        /// <summary>
        /// 由节点集合生成Delaunay三角形分网
        /// 2011.5.30
        /// </summary>
        /// <param name="nodes">节点号集合</param>
        /// <returns>几何三角形集合</returns>
        public List<Triangle> getDelaunayTriangleByNodes(List<int> Lnode)
        {
            if (Lnode.Count<3)
                throw new ArgumentException("Can not triangulate less than three nodes!");
            // The triangle list
            List<Triangle> triangles = new List<Triangle>();
            List<Point> Pts = new List<Point>();
            foreach (int iNode in Lnode)
            {
                Pts.Add(new Point(nodes[iNode].X,nodes[iNode].Y,nodes[iNode].Z));
            }
            triangles=DelaunayTriangulation2d.Triangulate(Pts);

            return triangles;
        }

        /// <summary>
        /// 向模型中添加新的节点
        /// 2011.8.22
        /// </summary>
        /// <param name="nn">节点对象</param>
        public void AddNode(Bnodes nn)
        {
            //如果模型中已包含节点则不做处理
            if (this.nodes.ContainsKey(nn.num))
                return;
            else
                this.nodes.Add(nn.num, nn);
        }
        /// <summary>
        /// 向模型中添加新的材料
        /// 2011.08.23
        /// </summary>
        /// <param name="newmat">新的材料数据</param>
        public void AddMat(BMaterial newmat)
        {
            //如果模型中已包含节点则不做处理
            if (this.mats.ContainsKey(newmat.iMAT))
                return;
            else
                this.mats.Add(newmat.iMAT,newmat);
        }
        /// <summary>
        /// 向模型中添加新的截面
        /// 2011.08.23
        /// </summary>
        /// <param name="newsec">新的截面</param>
        public void AddSection(BSections newsec)
        {
            //如果模型中已包含节点则不做处理
            if (this.sections.ContainsKey(newsec.Num))
                return;
            else
                this.sections.Add(newsec.Num,newsec);
        }
        /// <summary>
        /// 向模型添加梁单元
        /// 2011.08.23
        /// </summary>
        /// <param name="fe">新的梁单元</param>
        public void AddElement(FrameElement fe)
        {
            if (this.elements.ContainsKey(fe.iEL))
                return;
            else
                this.elements.Add(fe.iEL,fe);
        }
        /// <summary>
        /// 向模型添加梁单元荷载
        /// </summary>
        /// <param name="bbl">梁单元荷载信息</param>
        public void AddElemLoad(BBLoad bbl)
        {
            //把工况名字加入到工况列表
            BLoadCase nLC = new BLoadCase(bbl.LC);
            if (!this.hasLC(bbl.LC))
                STLDCASE.Add(nLC);
            _LoadTable.AddElemLoad(bbl);
        }
        /// <summary>
        /// 向模型添加节点荷载
        /// </summary>
        /// <param name="bnl">节点荷载数据</param>
        public void AddNodeLoad(BNLoad bnl)
        {
            //把工况名字加入到工况列表
            BLoadCase nLC = new BLoadCase(bnl.LC);
            if (!this.hasLC(bnl.LC))
                STLDCASE.Add(nLC);

            _LoadTable.AddNodeLoad(bnl);
        }
        /// <summary>
        /// 将单元指定为相应的结构组
        /// </summary>
        /// <param name="iEle">单元号</param>
        /// <param name="GroupName">组名</param>
        public void AddElemToGroup(int iEle,string GroupName)
        {
            List<int> elel = new List<int>();
            elel.Add(iEle);//创建单元列表
            if (_StruGroups.ContainsKey(GroupName))
            {
                _StruGroups[GroupName].AddElemList(elel);
            }
            else
            {
                BSGroup gg=new BSGroup (GroupName);
                gg.AddElemList(elel);
                _StruGroups.Add(GroupName,gg);
            }
        }
        /// <summary>
        /// 查询荷载工况表中是否含有指定名称的工况
        /// 2014.05.27
        /// </summary>
        /// <param name="lcN">工况名</param>
        /// <returns>是或否</returns>
        public bool hasLC(string lcN)
        {
            bool res = false;
            foreach (BLoadCase blc in STLDCASE)
            {
                if (blc.LCName == lcN)
                {
                    res = true; 
                    break;
                }
            }
            return res;
        }
        #endregion

        #region model类输入接口方法
        /// <summary>
        /// 读取mgt文件信息
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        public void ReadFromMgt(string FilePath)
        {
            string currentdata = "notype";//指定当前数据类型
            string curLoadCase = "notype";//指定当前荷载工况

            int curSecNUM = 0;//当前截面号
            string curSecPOLY = null;//指示当前截面点对
            int IPOLY_num = 0;//指示当前截面的内轮廓线数量

            //初始化模型信息数据
            //model = new Bmodel();
            //临时变量
            string[] temp, temp1 = null;
            int tempInt = 0;
            double tempDoublt1, tempDoublt2, tempDoublt3 = 0;
            int tempInt1, tempInt2, tempInt3, tempInt4, tempInt5, tempInt6, tempInt7,tempInt8 = 0;

            FileStream stream = File.Open(FilePath, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream, System.Text.Encoding.Default);
            for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
            {
                //本行数据类型判断
                if (line.StartsWith(";") == true)
                {
                    //当前行为注释
                }
                else if (line.StartsWith("*USE-STLD") == true && line.Contains(","))
                {
                    if (line.Contains(";"))
                    {
                        line = line.Remove(line.IndexOf(';'));//去掉注释
                    }
                    curLoadCase = line;//当前荷载工况
                    currentdata = line;//得到当前数据内容
                }

                #region 自重荷载读取
                else if (line.StartsWith("*SELFWEIGHT") && curLoadCase != "notype")
                {
                    temp = line.Split(new char[] { ',', ' ' },
                        StringSplitOptions.RemoveEmptyEntries);
                    temp1 = curLoadCase.Split(new char[] { ',', ' ' },
                        StringSplitOptions.RemoveEmptyEntries);//荷载工况
                    try
                    {
                        BWeight weightdata = new BWeight();
                        weightdata.Gx = double.Parse(temp[1]);
                        weightdata.Gy = double.Parse(temp[2]);
                        weightdata.Gz = double.Parse(temp[3]);

                        weightdata.LC = temp1[1];//自重工况
                        string str_group = line.Substring(line.LastIndexOf(',') + 1);
                        if (str_group != " ")
                        {
                            weightdata.Group = str_group.Trim();//装入组名
                        }

                        selfweight.Add(weightdata.LC, weightdata);
                    }
                    catch
                    {
                        MessageBox.Show("解析自重信息出错！\n我晕，我再晕...", "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                #endregion

                else if (line.StartsWith("*") == true)
                {
                    //如果命令行包含空格的话：
                    if (line.IndexOf(' ') > 0)
                    {
                        line = line.Remove(line.IndexOf(' '));
                    }
                    currentdata = line;//得到当前数据内容
                }
                #region 模型信息读取
                else if (line.StartsWith(" ") == true && currentdata == "*UNIT")
                {
                    line.Trim();//修剪开头空格
                    temp = line.Split(',');
                    try
                    {
                        unit.Force = temp[0].Trim().ToUpper();
                        unit.Length = temp[1].Trim().ToUpper();
                        unit.Heat = temp[2].Trim().ToUpper();
                        unit.Temper = temp[3].Trim().ToUpper();
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                }
                #endregion
                #region 节点数据读取
                else if (line.StartsWith(" ") == true && currentdata == "*NODE")
                {
                    //进行节点数据读取
                    line.Trim();//修剪开头空格
                    temp = line.Split(',');
                    try
                    {
                        tempInt = int.Parse(temp[0], System.Globalization.NumberStyles.Number);
                        tempDoublt1 = double.Parse(temp[1], System.Globalization.NumberStyles.Float);
                        tempDoublt2 = double.Parse(temp[2], System.Globalization.NumberStyles.Float);
                        tempDoublt3 = double.Parse(temp[3], System.Globalization.NumberStyles.Float);
                        nodes.Add(tempInt, new Bnodes(tempInt, tempDoublt1, tempDoublt2
                            , tempDoublt3));
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                        //MessageBox.Show("解析节点数据字符串出错!");
                    }
                }
                #endregion
                #region 单元数据读取
                else if (line.StartsWith(" ") == true && currentdata == "*ELEMENT")
                {
                    //进行单元数据读取
                    temp = line.Split(',');
                    try
                    {
                        tempInt = int.Parse(temp[0], System.Globalization.NumberStyles.Number);
                        tempInt1 = int.Parse(temp[2], System.Globalization.NumberStyles.Number);
                        tempInt2 = int.Parse(temp[3], System.Globalization.NumberStyles.Number);
                        tempInt3 = int.Parse(temp[4], System.Globalization.NumberStyles.Number);
                        tempInt4 = int.Parse(temp[5], System.Globalization.NumberStyles.Number);

                        ElemType et = (ElemType)Enum.Parse(typeof(ElemType), temp[1].Trim(), true);

                        switch (et)
                        {
                            case ElemType.BEAM:
                                tempDoublt1 = double.Parse(temp[6], System.Globalization.NumberStyles.Float);
                                FrameElement elemdata = new FrameElement(
                                    tempInt, et, tempInt1, tempInt2, tempInt3, tempInt4);
                                if (Math.Abs(tempDoublt1) <= 0.0001)//如果读入角度非常小，近似认为方向角为0
                                    tempDoublt1 = 0;
                                elemdata.beta = tempDoublt1;//记录单元方向角
                                elements.Add(tempInt, elemdata);
                                break;
                            case ElemType.TRUSS:
                                goto case ElemType.BEAM;
                            case ElemType.PLATE:
                                tempInt5 = int.Parse(temp[6], System.Globalization.NumberStyles.Integer);
                                tempInt6 = int.Parse(temp[7], System.Globalization.NumberStyles.Integer);
                                tempInt7 = int.Parse(temp[8], System.Globalization.NumberStyles.Integer);
                                PlanarElement elemdata_P = new PlanarElement(
                                    tempInt, et, tempInt1, tempInt2, tempInt3, tempInt4, tempInt5, tempInt6);
                                elemdata_P.iSUB = tempInt7;
                                elements.Add(tempInt, elemdata_P);
                                break;
                            case ElemType.WALL:
                                tempInt5 = int.Parse(temp[6], System.Globalization.NumberStyles.Integer);
                                tempInt6 = int.Parse(temp[7], System.Globalization.NumberStyles.Integer);
                                tempInt7 = int.Parse(temp[8], System.Globalization.NumberStyles.Integer);
                                tempInt8 = int.Parse(temp[9], System.Globalization.NumberStyles.Integer);
                                PlanarElement elemdata_W = new PlanarElement(
                                    tempInt, et, tempInt1, tempInt2, tempInt3, tempInt4, tempInt5, tempInt6);
                                elemdata_W.iSUB = tempInt7;
                                elemdata_W.iWID = tempInt8;//墙号
                                elements.Add(tempInt, elemdata_W);
                                break;
                            case ElemType.TENSTR:
                                tempDoublt2 = double.Parse(temp[6], System.Globalization.NumberStyles.Number);
                                tempInt5 = int.Parse(temp[7], System.Globalization.NumberStyles.Integer);
                                tempDoublt3 = double.Parse(temp[8], System.Globalization.NumberStyles.Integer);
                                FrameElement elemdata_T = new FrameElement(
                                    tempInt, et, tempInt1, tempInt2, tempInt3, tempInt4);
                                elemdata_T.beta = tempDoublt2;//单元方向角
                                elemdata_T.iSUB = tempInt5;
                                elemdata_T.EXVAL = tempDoublt3;
                                elements.Add(tempInt, elemdata_T);
                                break;
                            default:
                                break;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("解析单元信息出错!");
                    }
                }
                #endregion
                #region 材料信息读取
                else if (line.StartsWith(" ") && currentdata == "*MATERIAL")
                {
                    //进行材料信息的读取
                    string MatMGT = line;
                    //判断是否有续行
                    if (MatMGT.EndsWith("\\"))
                    {
                        line = reader.ReadLine();
                        MatMGT=MatMGT.TrimEnd('\\');
                        MatMGT = MatMGT + line.Trim();
                    }
                    temp = MatMGT.Split(',');
                    try
                    {
                        tempInt = int.Parse(temp[0], System.Globalization.NumberStyles.Integer);//材料编号
                        MatType tp = (MatType)Enum.Parse(typeof(MatType), temp[1].Trim(), true);//材料类型
                        BMaterial mat = new BMaterial(tempInt, tp, temp[2].Trim());
                        mat.addMGTdata(MatMGT.Trim());//存储原始数据

                        switch (tp)
                        {
                            case MatType.STEEL:
                                mat.setProp(2.06e11, 0.3, 1.2e-5, 7850);
                                break;
                            case MatType.CONC:
                                mat.setProp(3e10, 0.2, 1e-5, 2549.5);//目前按C30输入
                                break;
                            case MatType.SRC:
                                mat.setProp(3e10, 0.2, 1e-5, 2549.5);//目前按C30输入
                                break;
                            case MatType.USER:
                                //mat.setProp(2.06e11, 0.3, 1.2e-5, 7850);
                                break;
                            default:
                                break;
                        }

                        mats.Add(tempInt, mat);//存储数据
                    }
                    catch
                    {
                        MessageBox.Show("解析材料信息出错！\n你的MIDAS模型用的什么鬼材料？？", "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                #endregion
                #region 截面数据读取
                else if (line.StartsWith(" ") && currentdata == "*SECTION")//一般截面
                {
                    //进行截面属性读取
                    temp = line.Split(',');
                    try
                    {
                        tempInt = int.Parse(temp[0], System.Globalization.NumberStyles.Number);//截面编号
                        SecType tt = (SecType)Enum.Parse(typeof(SecType), temp[1].Trim(), true);//截面类型(枚举解析)     

                        #region 分类进行截面数据读取
                        switch (tt)
                        {
                            case SecType.DBUSER:
                                BSections secdata = new SectionDBuser();
                                secdata.Num = tempInt;
                                secdata.TYPE = tt;//截面类型(枚举解析)                        
                                secdata.Name = temp[2].Trim();//截面名称

                                secdata.OFFSET[0] = temp[3];//截面偏心
                                for (int i = 1; i < 7; i++)
                                    secdata.OFFSET[i] = double.Parse(temp[i + 3], System.Globalization.NumberStyles.Number);

                                secdata.bsd = temp[10].Trim() == "YES" ? true : false;
                                secdata.SSHAPE = (SecShape)Enum.Parse(typeof(SecShape), temp[11].Trim(), true);//截面形状
                                secdata.SEC_Data.Clear();
                                secdata.SEC_Data.Add(int.Parse(temp[12], System.Globalization.NumberStyles.Number));//截面数据
                                if ((int)secdata.SEC_Data[0] == 2)
                                {
                                    for (int j = 1; j < 11; j++)
                                    {
                                        secdata.SEC_Data.Add(double.Parse(temp[j + 12],
                                            System.Globalization.NumberStyles.Number));
                                    }
                                }
                                else if ((int)secdata.SEC_Data[0] == 1)
                                {
                                    secdata.SEC_Data.Add(temp[13]);
                                    secdata.SEC_Data.Add(temp[14]);
                                }

                                sections.Add(tempInt, secdata);//输出变量
                                break;
                            case SecType.TAPERED:
                                SectionTapered secTapered = new SectionTapered();
                                secTapered.Num = tempInt;
                                secTapered.TYPE = tt;//截面类型(枚举解析)                        
                                secTapered.Name = temp[2].Trim();//截面名称
                                secTapered.OFFSET.Clear();
                                secTapered.OFFSET.Add(temp[3].Trim());//截面偏心
                                for (int i = 1; i < 9; i++)
                                    secTapered.OFFSET.Add(double.Parse(temp[i + 3], System.Globalization.NumberStyles.Number));

                                secTapered.bsd = temp[12].Trim() == "YES" ? true : false;
                                secTapered.SSHAPE = (SecShape)Enum.Parse(typeof(SecShape), temp[13].Trim(), true);//截面形状符号
                                secTapered.iyVAR = (iVAR)Enum.Parse(typeof(iVAR), temp[14], true);
                                secTapered.izVAR = (iVAR)Enum.Parse(typeof(iVAR), temp[15], true);
                                secTapered.SubTYPE = (STYPE)Enum.Parse(typeof(STYPE), temp[16], true);

                                line = reader.ReadLine();
                                secTapered.SEC_Data.Clear();
                                secTapered.SEC_Data.Add(line.Trim());

                                sections.Add(tempInt, secTapered);//存储截面
                                break;

                            case SecType.SRC:
                                BSections secSRC = new SectionSRC();
                                secSRC.Num = tempInt;
                                secSRC.TYPE = tt;//截面类型(枚举解析)                        
                                secSRC.Name = temp[2].Trim();//截面名称
                                line = reader.ReadLine();//下一行跳过

                                sections.Add(tempInt, secSRC);//存储截面
                                break;
                            default:
                                break;
                        }
                        #endregion

                    }
                    catch
                    {
                        MessageBox.Show("解析常规截面属性出错！\n是否选用了不支持的截面数据类型？？", "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                #region 自定义截面读取
                else if (line.StartsWith(" ") && currentdata == "*SECT-GENERAL")//自定义截面
                {
                    if (line.Contains("SECT="))
                    {
                        SectionGeneral secGEN = new SectionGeneral();
                        temp = line.Trim().Remove(0, 5).Split(',');
                        tempInt = Convert.ToInt16(temp[0].Trim());//截面编号
                        SecType tt = (SecType)Enum.Parse(typeof(SecType), temp[1].Trim(), true);//截面类型(枚举解析)
                        secGEN.Num = tempInt;
                        curSecNUM = tempInt;//指定当前截面号，用于截面其它信息读取
                        curSecPOLY = null;//指定当前截面轮廓符号
                        IPOLY_num = 0;//指针指向零
                        secGEN.TYPE = tt;
                        secGEN.Name = temp[2].Trim();
                        secGEN.OFFSET.Add(temp[3].Trim());
                        for (int i = 1; i < 7; i++)
                            secGEN.OFFSET.Add(double.Parse(temp[i + 3], System.Globalization.NumberStyles.Number));

                        secGEN.bsd = temp[10].Trim() == "YES" ? true : false;
                        secGEN.SSHAPE = (SecShape)Enum.Parse(typeof(SecShape), temp[11].Trim(), true);//截面形状符号
                        secGEN.bBU = temp[12].Trim() == "YES" ? true : false;
                        secGEN.bEQ = temp[13].Trim() == "YES" ? true : false;

                        line = reader.ReadLine();//第二行
                        temp = line.Split(',');
                        secGEN.setSecProp1(double.Parse(temp[0]), double.Parse(temp[1]), double.Parse(temp[2]),
                            double.Parse(temp[3]), double.Parse(temp[4]), double.Parse(temp[5]));
                        line = reader.ReadLine();//第三行
                        temp = line.Split(',');
                        secGEN.setSecProp2(double.Parse(temp[0]), double.Parse(temp[1]), double.Parse(temp[2]),
                            double.Parse(temp[3]), double.Parse(temp[4]), double.Parse(temp[5]),
                            double.Parse(temp[6]), double.Parse(temp[7]), double.Parse(temp[8]),
                            double.Parse(temp[9]));
                        line = reader.ReadLine();//第四行
                        temp = line.Split(',');
                        secGEN.setSecProp3(double.Parse(temp[0]), double.Parse(temp[1]), double.Parse(temp[2]),
                            double.Parse(temp[3]), double.Parse(temp[4]), double.Parse(temp[5]),
                            double.Parse(temp[6]), double.Parse(temp[7]));

                        sections.Add(tempInt, secGEN);//将截面添加入模型对象
                    }
                    else if (line.Contains("OPOLY="))//含有OPOLY=的行
                    {
                        curSecPOLY = "OPOLY";//指示当前点对符号
                        temp = line.Trim().Remove(0, 6).Split(',');
                        if (sections[curSecNUM] is SectionGeneral)
                        {
                            SectionGeneral SGtemp = sections[curSecNUM] as SectionGeneral;
                            for (int i = 0; i < temp.Length; i = i + 2)
                            {
                                double d1 = Convert.ToDouble(temp[i]);
                                double d2 = Convert.ToDouble(temp[i + 1]);
                                SGtemp.addtoOPOLY(new Point2d(d1, d2));
                            }
                            sections.Remove(curSecNUM);//删除现有截面
                            sections.Add(curSecNUM, SGtemp);//增加新的截面
                        }
                    }
                    else if (line.Contains("IPOLY="))//含有IPOLY=的行
                    {
                        curSecPOLY = "IPOLY";//指示当前点对符号
                        IPOLY_num++;
                        temp = line.Trim().Remove(0, 6).Split(',');
                        if (sections[curSecNUM] is SectionGeneral)
                        {
                            SectionGeneral SGtemp = sections[curSecNUM] as SectionGeneral;
                            for (int i = 0; i < temp.Length; i = i + 2)
                            {
                                double d1 = Convert.ToDouble(temp[i]);
                                double d2 = Convert.ToDouble(temp[i + 1]);
                                SGtemp.addtoIPOLY(IPOLY_num - 1, new Point2d(d1, d2));
                            }
                            sections.Remove(curSecNUM);//删除现有截面
                            sections.Add(curSecNUM, SGtemp);//增加新的截面
                        }
                    }
                    else if (curSecPOLY == "OPOLY")//不含有OPOLY=的行
                    {
                        temp = line.Trim().Split(',');
                        if (sections[curSecNUM] is SectionGeneral)
                        {
                            SectionGeneral SGtemp = sections[curSecNUM] as SectionGeneral;
                            for (int i = 0; i < temp.Length; i = i + 2)
                            {
                                double d1 = Convert.ToDouble(temp[i]);
                                double d2 = Convert.ToDouble(temp[i + 1]);
                                SGtemp.addtoOPOLY(new Point2d(d1, d2));
                            }
                            sections.Remove(curSecNUM);//删除现有截面
                            sections.Add(curSecNUM, SGtemp);//增加新的截面
                        }
                    }
                    else if (curSecPOLY == "IPOLY")//不含有IPOLY=的行
                    {
                        temp = line.Trim().Split(',');
                        if (sections[curSecNUM] is SectionGeneral)
                        {
                            SectionGeneral SGtemp = sections[curSecNUM] as SectionGeneral;
                            for (int i = 0; i < temp.Length; i = i + 2)
                            {
                                double d1 = Convert.ToDouble(temp[i]);
                                double d2 = Convert.ToDouble(temp[i + 1]);
                                SGtemp.addtoIPOLY(IPOLY_num - 1, new Point2d(d1, d2));
                            }
                            sections.Remove(curSecNUM);//删除现有截面
                            sections.Add(curSecNUM, SGtemp);//增加新的截面
                        }
                    }
                }
                #endregion
                #endregion
                #region 板单元厚度数据读取
                else if (line.StartsWith(" ") == true && currentdata == "*THICKNESS")
                {
                    temp = line.Split(',');
                    try
                    {
                        tempInt = int.Parse(temp[0], System.Globalization.NumberStyles.Number);//厚度编号
                        BThickness thidata = new BThickness();
                        thidata.iTHK = tempInt;
                        thidata.TYPE = temp[1].Trim();
                        thidata.bSAME = true;
                        if (temp[2].Trim() == "NO")
                            thidata.bSAME = false;
                        thidata.THIK_IN = double.Parse(temp[3].Trim(), System.Globalization.NumberStyles.Float);
                        thidata.THIK_OUT = double.Parse(temp[4].Trim(), System.Globalization.NumberStyles.Float);

                        thickness.Add(tempInt, thidata);//输出变量
                    }
                    catch
                    {
                        MessageBox.Show("解析截面厚度出错！", "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                #endregion
                #region 边界条件数据读取
                else if (line.StartsWith(" ") && currentdata == "*CONSTRAINT")
                {
                    //进行边界条件读取
                    BConstraint support = new BConstraint();
                    List<int> nodes = new List<int>();
                    temp = line.Split(',');
                    //当前行指示节点集合
                    nodes = Tools.SelectCollection.StringToList(temp[0].Trim());
                                        
                    //读取约束情况
                    for (int i = 0; i < temp[1].Trim().Length; i++)
                    {
                        if (temp[1].Trim()[i] == '1')
                        {
                            switch (i)
                            {
                                case 0:
                                    support.UX = true;
                                    break;
                                case 1:
                                    support.UY = true;
                                    break;
                                case 2:
                                    support.UZ = true;
                                    break;
                                case 3:
                                    support.RX = true;
                                    break;
                                case 4:
                                    support.RY = true;
                                    break;
                                case 5:
                                    support.RZ = true;
                                    break;
                            }
                        }
                    }
                    //添加的数据库
                    if (nodes.Count >0)
                    {
                        foreach (int nn in nodes)
                        {
                            BConstraint bs = new BConstraint();
                            bs.copySupports(support);
                            bs.Node = nn;
                            this.constraint.Add(nn, bs);
                        }
                    }
                    else 
                    {
                        //未处理
                    }

                }
                #endregion
                #region 工况列表数据读取
                else if (line.StartsWith(" ") == true && currentdata == "*STLDCASE")
                {
                    //拆分字符串
                    temp = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    try
                    {
                        
                        string nLcName = temp[0].Trim();
                        BLoadCase lcdata = new BLoadCase(nLcName);

                        switch (temp[1])
                        {
                            case "D":
                                lcdata.LCType = LCType.D;
                                break;
                            case "L":
                                lcdata.LCType = LCType.L;
                                break;
                            case "W":
                                lcdata.LCType = LCType.W;
                                break;
                            case "E":
                                lcdata.LCType = LCType.E;
                                break;
                            case "LR":
                                lcdata.LCType = LCType.LR;
                                break;
                            case "S":
                                lcdata.LCType = LCType.S;
                                break;
                            case "T":
                                lcdata.LCType = LCType.T;
                                break;
                            case "PS":
                                lcdata.LCType = LCType.PS;
                                break;
                            default:
                                lcdata.LCType = LCType.USER;
                                break;
                        }

                        STLDCASE.Add(lcdata);//添加到模型数据库
                    }
                    catch
                    {
                        MessageBox.Show("解析工况列表出错！\n我晕，我狂晕...", "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                #endregion

                #region 节点荷载列表读取
                else if (line.StartsWith(" ") == true && currentdata == "*CONLOAD")
                {
                    //拆分字符
                    temp = curLoadCase.Split(new char[] { ',', ' ' },
                        StringSplitOptions.RemoveEmptyEntries);//temp[1]为工况名
                    temp1 = line.Split(new char[] { ',', ' ' },
                        StringSplitOptions.RemoveEmptyEntries);//temp1为数据组

                    try
                    {
                        BNLoad BNLoaddata = new BNLoad(int.Parse(temp1[0]));
                        BNLoaddata.FX = double.Parse(temp1[1]);
                        BNLoaddata.FY = double.Parse(temp1[2]);
                        BNLoaddata.FZ = double.Parse(temp1[3]);
                        BNLoaddata.MX = double.Parse(temp1[4]);
                        BNLoaddata.MY = double.Parse(temp1[5]);
                        BNLoaddata.MZ = double.Parse(temp1[6]);

                        BNLoaddata.LC = temp[1];//工况名称
                        string str_group = line.Substring(line.LastIndexOf(',') + 1);
                        if (str_group != " ")
                        {
                            BNLoaddata.Group = str_group.Trim();//装入组名
                        }

                        conloads.Add(BNLoaddata.iNode, BNLoaddata);//加入模型数据库
                    }
                    catch
                    {
                        MessageBox.Show("解析节点荷载列表出错！\n我晕，我狂晕...", "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                #endregion
                #region 梁单元荷载列表读取
                else if (line.StartsWith(" ") == true && currentdata == "*BEAMLOAD")
                {
                    //拆分字符
                    temp = curLoadCase.Split(new char[] { ',', ' ' },
                        StringSplitOptions.RemoveEmptyEntries);//temp[1]为工况名
                    temp1 = line.Split(new char[] { ',' },
                        StringSplitOptions.RemoveEmptyEntries);//temp1为数据组
                    try
                    {
                        BBLoad BeamLoadData = new BBLoad();
                        BeamLoadData.ELEM_num = int.Parse(temp1[0].Trim());
                        BeamLoadData.CMD = temp1[1].Trim();
                        BeamLoadData.TYPE = (BeamLoadType)Enum.Parse(typeof(BeamLoadType), temp1[2].Trim());//荷载类型
                        BeamLoadData.setLoadDir(temp1[3].Trim());
                        if (temp1[4].Trim() == "YES")
                            BeamLoadData.bPROJ = true;

                        //更新偏心数据
                        BeamLoadData.readEccenDataMgt(line);

                        double dd1 = double.Parse(temp1[10].Trim());
                        double pp1 = double.Parse(temp1[11].Trim());
                        double dd2 = double.Parse(temp1[12].Trim());
                        double pp2 = double.Parse(temp1[13].Trim());
                        double dd3 = double.Parse(temp1[14].Trim());
                        double pp3 = double.Parse(temp1[15].Trim());
                        double dd4 = double.Parse(temp1[16].Trim());
                        double pp4 = double.Parse(temp1[17].Trim());
                        BeamLoadData.setLoadData(dd1, pp1, dd2, pp2, dd3, pp3, dd4, pp4);//更新荷载数据

                        BeamLoadData.LC = temp[1];//工况
                        if (temp1[18] != " ")
                        {
                            BeamLoadData.Group = temp1[18].Trim();//装入组名
                        }

                        beamloads.Add(BeamLoadData.ELEM_num, BeamLoadData);//加入模型数据库
                    }
                    catch
                    {
                        MessageBox.Show("解析梁单元荷载列表出错！\n我晕，我狂晕...", "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                #endregion

                #region 温度荷载读取
                else if (line.StartsWith(" ") == true && currentdata == "*ELTEMPER")
                {
                    //拆分字符
                    temp = curLoadCase.Split(new char[] { ',', ' ' },
                        StringSplitOptions.RemoveEmptyEntries);//temp[1]为工况名
                    temp1 = line.Split(new char[] { ',' },
                        StringSplitOptions.RemoveEmptyEntries);//temp1为数据组
                    try
                    {
                        BETLoad TempLoad = new BETLoad();
                        TempLoad.Elem_Num = Convert.ToInt32(temp1[0].Trim());//单元号
                        TempLoad.Temp = Convert.ToDouble(temp1[1].Trim());//单元温度
                        TempLoad.LC = temp[1];//工况
                        if (temp1[2] != " ")
                        {
                            TempLoad.Group = temp1[2].Trim();//装入组名
                        }
                        _EleTempLoads.Add(TempLoad.Elem_Num, TempLoad);//加入模型数据库
                    }
                    catch
                    {
                        MessageBox.Show("解析单元温度荷载表出错！\n", "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                #endregion
            }
            reader.Close();
            #region 再次打开文件并读取荷载组合
            FileStream str = File.Open(FilePath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(str, Encoding.Default);//用系统默认编码打开
            ReadLoadComb(ref sr);   //读取荷载组合
            sr.Close();
            #endregion

            #region 再次打开文件并读取结构组
            str = File.Open(FilePath, FileMode.Open, FileAccess.Read);
            sr = new StreamReader(str, Encoding.Default);//用系统默认编码打开
            ReadStruGroups(ref sr); //读取结构组
            sr.Close();
            #endregion

            #region 再次打开文件并读取刚性连接数据
            str = File.Open(FilePath, FileMode.Open, FileAccess.Read);
            sr = new StreamReader(str, Encoding.Default);//用系统默认编码打开
            ReadRigidLinks(ref sr);
            sr.Close();
            #endregion

            #region 再次打开文件并读取设计参数
            str = File.Open(FilePath, FileMode.Open, FileAccess.Read);
            sr = new StreamReader(str, Encoding.Default);//用系统默认编码打开
            ReadK_Ractor(ref sr);//读取计算长度系数
            ReadLengths(ref sr);//读取计算长度
            ReadLimitsRatio(ref sr);//读取极限长度 
            sr.Close();
            #endregion

            Normalize();//模型标准化处理
            GenBeamKpoint();//计算模型中梁单元的节点方向点信息
        }

        /// <summary>
        /// 读取荷载组合列表
        /// </summary>
        /// <param name="srt">文件流</param>
        public void ReadLoadComb(ref StreamReader srt)
        {
            /* 1、准备*/
            bool bRead = false;                     //是否可以读取
            String strText = null;                  //当前行文本
            String strStartFlag = "*LOADCOMB";      //数据开始标志
            String strEndFlag = "";                 //数据结束标志
            char szSplit = ',';                     //数据分隔符

            /* 2、循环读取*/
            string curName = null;                  //当前荷载组合名称
            BLoadComb curCom = null;//当前荷载组合
            for (strText = srt.ReadLine(); strText != null; strText = srt.ReadLine())
            {
                /* 2.1、判断是否读到数据。若读到，设置标志，进入下一轮循环开始读取；若没有读到，继续进入下一轮判断。*/
                /* 2.2、bRead=true，表示已经可以读数据了。读的时候要判断是否已经读完数据。*/
                if (!bRead)
                {
                    if (strText.StartsWith(strStartFlag))
                    {
                        bRead = true;
                    }
                    continue;
                }
                else if (strText.StartsWith(";"))//如果为注释则忽略
                    continue;
                else if (strText.CompareTo(strEndFlag) == 0)
                    return;
                else if (strText.Trim().StartsWith("NAME"))
                {
                    /*进入当前荷载组合基本数据读取*/
                    string[] sArrayCur = strText.Trim().Split(szSplit);
                    string sName = sArrayCur[0].Substring(sArrayCur[0].IndexOf('=') + 1).Trim();//组合名称
                    LCKind kind = LCKind.GEN;//组合类型
                    bool isActive = true;//是否激活
                    bool bEs = false;
                    switch (sArrayCur[1].Trim())
                    {
                        case "GEN": kind = LCKind.GEN; break;
                        case "STEEL": kind = LCKind.STEEL; break;
                        case "CONC": kind = LCKind.CONC; break;
                        case "SRC": kind = LCKind.SRC; break;
                        case "FDN": kind = LCKind.FDN; break;
                        default: kind = LCKind.GEN; break;
                    }
                    if (sArrayCur[2].Trim() == "INACTIVE")
                    {
                        isActive = false;
                    }
                    if (sArrayCur[3].Trim() != "0")
                        bEs = true;
                    int Type = Convert.ToInt16(sArrayCur[4].Trim());
                    curCom = new BLoadComb();
                    curCom.SetData1(sName, kind, isActive, bEs, Type, sArrayCur[5].Trim());
                    curName = sName;//记录当前名称
                    _LoadCombTable.Add(curCom);//添加数据库
                    continue;
                }
                else if (strText.StartsWith(" ") && strText.Contains(szSplit.ToString()))
                {
                    /*进入当前荷载组合工况对添加*/
                    //BLoadComb tempBLC = _LoadCombTable[curName];//取出当前组合
                    ////LOADCOMBS.Remove(curName);
                    //_LoadCombTable.Remove(curName);//从组合表中删除
                    string[] sArrayCur = strText.Trim().Split(szSplit);
                    for (int i = 0; i < sArrayCur.Length; i = i + 3)
                    {
                        BLCFactGroup lcfg = new BLCFactGroup();
                        switch (sArrayCur[i].Trim())
                        {
                            case "TH": lcfg.ANAL = ANAL.TH; break;
                            case "SM": lcfg.ANAL = ANAL.SM; break;
                            case "RS": lcfg.ANAL = ANAL.RS; break;
                            case "MV": lcfg.ANAL = ANAL.MV; break;
                            case "ST": lcfg.ANAL = ANAL.ST; break;
                            case "CB": lcfg.ANAL = ANAL.CB; break;
                            case "CBS": lcfg.ANAL = ANAL.CBS; break;
                            case "ES": lcfg.ANAL = ANAL.ES; break;
                            default: lcfg.ANAL = ANAL.ST; break;
                        }
                        lcfg.LCNAME = sArrayCur[i + 1].Trim();
                        lcfg.FACT = Convert.ToDouble(sArrayCur[i + 2]);
                        curCom.AddLCFactGroup(lcfg);
                    }
                    _LoadCombTable.Add(curCom);//再更新到组合表中
                }
                else
                    continue;
            }
        }

        /// <summary>
        /// 读取结构组数据表
        /// </summary>
        /// <param name="str">文件流</param>
        public void ReadStruGroups(ref StreamReader srt)
        {
            /* 1、准备*/
            bool bRead = false;                     //是否可以读取
            String strText = null;                  //当前行文本
            String strStartFlag = "*GROUP";      //数据开始标志
            String strEndFlag = "";                 //数据结束标志
            char szSplit = ',';                     //数据分隔符
            int iGroupFlag = 0;                  //组数据标志：0-新组,1-节点数据,2-单元数据

            /* 2、循环读取*/
            BSGroup Group = null;                   //组
            for (strText = srt.ReadLine(); strText != null; strText = srt.ReadLine())
            {
                /* 2.1、判断是否读到数据。若读到，设置标志，进入下一轮循环开始读取；若没有读到，继续进入下一轮判断。*/
                /* 2.2、bRead=true，表示已经可以读数据了。读的时候要判断是否已经读完数据。*/
                if (!bRead)
                {
                    if (strText.StartsWith(strStartFlag))
                    {
                        bRead = true;
                    }
                    continue;
                }
                else if (strText.StartsWith(";"))//如果为注释则忽略
                    continue;
                else if (strText.CompareTo(strEndFlag) == 0)//如果读取数据结尾则返回
                    return;
                else
                {
                    #region 读取数据
                    string[] sCurs = strText.Trim().Split(szSplit);
                    if (sCurs.Length == 3 && iGroupFlag == 0)//第一行有两个分隔符时
                    {
                        Group = new BSGroup(sCurs[0].Trim());//创建新组
                        List<int> nodes = Tools.SelectCollection.StringToList(sCurs[1].Trim());
                        List<int> elems = new List<int>();
                        if (sCurs[2].EndsWith("\\"))
                        {
                            iGroupFlag = 2;      //指定下一行为旧数据
                            elems = Tools.SelectCollection.StringToList(sCurs[2].TrimEnd('\\'));
                        }
                        else
                        {
                            iGroupFlag = 0;     //指定下一行为新组
                            elems = Tools.SelectCollection.StringToList(sCurs[2]);
                        }
                        Group.AddNodeList(nodes);//添加节点表
                        Group.AddElemList(elems);//添加单元表

                        //添加入模型数据库
                        if (strText.EndsWith("\\") == false)
                        {
                            this.StruGroups.Add(Group.GroupName, Group);
                        }
                    }
                    else if (sCurs.Length == 2 && iGroupFlag == 0)//第一行有一个分隔符时
                    {
                        Group = new BSGroup(sCurs[0].Trim());//创建新组
                        List<int> nodes = new List<int>();
                        if (sCurs[1].EndsWith("\\"))
                        {
                            iGroupFlag = 1;      //指定下一行为旧数据
                            nodes = Tools.SelectCollection.StringToList(sCurs[1].TrimEnd('\\'));
                        }
                        else
                        {
                            iGroupFlag = 0;     //指定下一行为新组
                            nodes = Tools.SelectCollection.StringToList(sCurs[1]);
                        }
                        Group.AddNodeList(nodes);//添加节点表

                        //添加入模型数据库
                        if (strText.EndsWith("\\") == false)
                        {
                            this.StruGroups.Add(Group.GroupName, Group);
                        }
                    }
                    else if (sCurs.Length == 1 && iGroupFlag == 1)//节点数据行
                    {
                        List<int> nodes = new List<int>();
                        string temp = strText;
                        if (strText.EndsWith("\\") == false)
                        {
                            iGroupFlag = 0;//新的组
                        }
                        else
                        {
                            temp = temp.TrimEnd('\\');
                        }
                        nodes = Tools.SelectCollection.StringToList(temp);

                        Group.AddNodeList(nodes);

                        //添加入模型数据库
                        if (strText.EndsWith("\\") == false)
                        {
                            this.StruGroups.Add(Group.GroupName, Group);
                        }
                    }
                    else if (sCurs.Length == 1 && iGroupFlag == 2)//单元数据行
                    {
                        List<int> elems = new List<int>();
                        string temp = strText;
                        if (strText.EndsWith("\\") == false)
                        {
                            iGroupFlag = 0;//新的组
                        }
                        else
                        {
                            temp = temp.TrimEnd('\\');
                        }
                        elems = Tools.SelectCollection.StringToList(temp);

                        Group.AddElemList(elems);

                        //添加入模型数据库
                        if (strText.EndsWith("\\") == false)
                        {
                            this.StruGroups.Add(Group.GroupName, Group);
                        }
                    }
                    else if (sCurs.Length == 2 && iGroupFlag == 1)//同时有节点数据和单元数据
                    {
                        List<int> nodes = new List<int>();
                        List<int> elems = new List<int>();
                        string temp1 = sCurs[0];
                        string temp2 = sCurs[1];
                        if (temp2.EndsWith("\\") == false)
                        {
                            iGroupFlag = 0;//新的组
                        }
                        else
                        {
                            temp2 = temp2.TrimEnd('\\');
                            iGroupFlag = 2;//更新数据标志
                        }

                        nodes = Tools.SelectCollection.StringToList(temp1);
                        elems = Tools.SelectCollection.StringToList(temp2);

                        Group.AddNodeList(nodes);
                        Group.AddElemList(elems);

                        //添加入模型数据库
                        if (strText.EndsWith("\\") == false)
                        {
                            this.StruGroups.Add(Group.GroupName, Group);
                        }
                    }
                    else
                        continue;
                    #endregion
                }
            }
        }

        /// <summary>
        /// 读取刚性连接数据
        /// </summary>
        /// <param name="srt">mgt文件流</param>
        public void ReadRigidLinks(ref StreamReader srt)
        {
            /* 1、准备*/
            bool bRead = false;                     //是否可以读取
            String strText = null;                  //当前行文本
            String strStartFlag = "*RIGIDLINK";      //数据开始标志
            String strEndFlag = "";                 //数据结束标志
            char szSplit = ',';                     //数据分隔符
            int iGroupFlag = 0;                  //组数据标志：0-新组,1-节点数据

            /* 2、循环读取*/
            BRigidLink RigidLink = null;                   //组
            for (strText = srt.ReadLine(); strText != null; strText = srt.ReadLine())
            {
                /* 2.1、判断是否读到数据。若读到，设置标志，进入下一轮循环开始读取；若没有读到，继续进入下一轮判断。*/
                /* 2.2、bRead=true，表示已经可以读数据了。读的时候要判断是否已经读完数据。*/
                if (!bRead)
                {
                    if (strText.StartsWith(strStartFlag))
                    {
                        bRead = true;
                    }
                    continue;
                }
                else if (strText.StartsWith(";"))//如果为注释则忽略
                    continue;
                else if (strText.CompareTo(strEndFlag) == 0)//如果读取数据结尾则返回
                    return;
                else
                {
                    #region 读取数据开始
                    string[] sCurs = strText.Trim().Split(szSplit);
                    if (sCurs.Length == 4 && iGroupFlag == 0)//所有数据均在一行时
                    {
                        RigidLink = new BRigidLink();//创建新的刚性连接
                        List<int> nodes = new List<int>();
                        nodes = Tools.SelectCollection.StringToList(sCurs[2]);
                        RigidLink.MNode = Convert.ToInt32(sCurs[0].Trim());//主节点号
                        RigidLink.SetUxyzRxyz(sCurs[1].Trim());//设置约束自由度
                        RigidLink.AddSNodesList(nodes);//添加从属节点表
                        RigidLink.Group = sCurs[3];//边界组名

                        //添加入模型数据库
                        if (strText.EndsWith("\\") == false)
                        {
                            this._RigidLinks.Add(RigidLink.MNode, RigidLink);
                        }
                        else
                        {
                            iGroupFlag = 1;      //指定下一行为旧数据
                        }
                    }
                    else if (sCurs.Length == 3 && iGroupFlag == 0)//第一行有两个分隔符时
                    {
                        RigidLink = new BRigidLink();//创建新的刚性连接
                        List<int> elems = new List<int>();
                        if (sCurs[2].EndsWith("\\"))
                        {
                            iGroupFlag = 1;      //指定下一行为旧数据
                            elems = Tools.SelectCollection.StringToList(sCurs[2].TrimEnd('\\'));
                        }
                        else
                        {
                            iGroupFlag = 0;     //指定下一行为新组
                            elems = Tools.SelectCollection.StringToList(sCurs[2]);
                        }
                        RigidLink.MNode = Convert.ToInt32(sCurs[0].Trim());//主节点号
                        RigidLink.SetUxyzRxyz(sCurs[1].Trim());//设置约束自由度
                        RigidLink.AddSNodesList(elems);//添加从属节点表

                        //添加入模型数据库
                        if (strText.EndsWith("\\") == false)
                        {
                            this._RigidLinks.Add(RigidLink.MNode, RigidLink);
                        }
                    }
                    else if (sCurs.Length == 1 && iGroupFlag == 1)//旧数据行
                    {
                        List<int> nodes = new List<int>();
                        string temp = strText;
                        if (strText.EndsWith("\\") == false)
                        {
                            iGroupFlag = 0;//新的组
                        }
                        else
                        {
                            temp = temp.TrimEnd('\\');
                        }
                        nodes = Tools.SelectCollection.StringToList(temp);

                        RigidLink.AddSNodesList(nodes);

                        //添加入模型数据库
                        if (strText.EndsWith("\\") == false)
                        {
                            this._RigidLinks.Add(RigidLink.MNode, RigidLink);
                        }
                    }
                    else if (sCurs.Length == 2 && iGroupFlag == 1)//读到行尾
                    {
                        List<int> nodes = new List<int>();
                        string temp1 = sCurs[0].Trim();
                        string temp2 = sCurs[1].Trim();
                        if (temp2.EndsWith("\\") == false)
                        {
                            iGroupFlag = 0;//新的组
                        }
                        else
                        {
                            temp2 = temp2.TrimEnd('\\');
                            iGroupFlag = 2;//更新数据标志
                        }
                        nodes = Tools.SelectCollection.StringToList(temp1);

                        RigidLink.AddSNodesList(nodes);
                        RigidLink.Group = temp2;//边界组名

                        //添加入模型数据库
                        if (strText.EndsWith("\\") == false)
                        {
                            this._RigidLinks.Add(RigidLink.MNode, RigidLink);
                        }
                    }
                    else
                        continue;
                    #endregion
                }
            }
        }
        /// <summary>
        /// 读取计算长度系数数据
        /// </summary>
        /// <param name="srt">mgt文件流</param>
        public void ReadK_Ractor(ref StreamReader srt)
        {
            /* 1、准备*/
            bool bRead = false;                     //是否可以读取
            String strText = null;                  //当前行文本
            String strStartFlag = "*K-FACTOR";      //数据开始标志
            String strEndFlag = "";                 //数据结束标志
            char szSplit = ',';                     //数据分隔符
            //int iGroupFlag = 0;                  //组数据标志

            /* 2、循环读取*/
            Bk_Factor KFactor = null;//计算长度系数
            //BSGroup Group = null;                   //组
            for (strText = srt.ReadLine(); strText != null; strText = srt.ReadLine())
            {
                /* 2.1、判断是否读到数据。若读到，设置标志，进入下一轮循环开始读取；若没有读到，继续进入下一轮判断。*/
                /* 2.2、bRead=true，表示已经可以读数据了。读的时候要判断是否已经读完数据。*/
                if (!bRead)
                {
                    if (strText.StartsWith(strStartFlag))
                    {
                        bRead = true;
                    }
                    continue;
                }
                else if (strText.StartsWith(";"))//如果为注释则忽略
                    continue;
                else if (strText.CompareTo(strEndFlag) == 0)//如果读取数据结尾则返回
                    return;
                else
                {
                    #region 读取数据
                    string[] sCurs = strText.Trim().Split(szSplit);
                    if (sCurs.Length == 3)
                    {
                        List<int> elems = Tools.SelectCollection.StringToList(sCurs[0].Trim());
                        double kky = double.Parse(sCurs[1].Trim());
                        double kkz = double.Parse(sCurs[2].Trim());
                        foreach (int ele in elems)
                        {
                            KFactor = new Bk_Factor(ele,kky,kkz);
                            this.K_Factors.Add(ele, KFactor);//添加到数据模型中
                        }
                    }
                    else
                        continue;
                    #endregion
                }
            }
        }
        /// <summary>
        /// 读取计算长度数据
        /// </summary>
        /// <param name="srt">mgt文件流</param>
        public void ReadLengths(ref StreamReader srt)
        {
            /* 1、准备*/
            bool bRead = false;                     //是否可以读取
            String strText = null;                  //当前行文本
            String strStartFlag = "*LENGTH";      //数据开始标志
            String strEndFlag = "";                 //数据结束标志
            char szSplit = ',';                     //数据分隔符
            //int iGroupFlag = 0;                  //组数据标志

            /* 2、循环读取*/
            BUnsupportedLen bul = null;//计算长度
            for (strText = srt.ReadLine(); strText != null; strText = srt.ReadLine())
            {
                /* 2.1、判断是否读到数据。若读到，设置标志，进入下一轮循环开始读取；若没有读到，继续进入下一轮判断。*/
                /* 2.2、bRead=true，表示已经可以读数据了。读的时候要判断是否已经读完数据。*/
                if (!bRead)
                {
                    if (strText.StartsWith(strStartFlag))
                    {
                        bRead = true;
                    }
                    continue;
                }
                else if (strText.StartsWith(";"))//如果为注释则忽略
                    continue;
                else if (strText.CompareTo(strEndFlag) == 0)//如果读取数据结尾则返回
                    return;
                else
                {
                    #region 读取数据
                    string[] sCurs = strText.Trim().Split(szSplit);
                    if (sCurs.Length == 6)
                    {
                        List<int> elems = Tools.SelectCollection.StringToList(sCurs[0].Trim());
                        double lly = double.Parse(sCurs[1].Trim());
                        double llz = double.Parse(sCurs[2].Trim());
                        bool bUSE = true;
                        if (sCurs[3].Trim() != "NO")
                            bUSE = false;
                        double llb = double.Parse(sCurs[4].Trim());
                        foreach (int ele in elems)
                        {
                            bul = new BUnsupportedLen(ele, lly, llz);
                            bul.IsCheckLb = bUSE;
                            bul.Lb = llb;
                            this._Lengths.Add(ele,bul);//添加到数据模型中
                        }
                    }
                    else
                        continue;
                    #endregion
                }
            }
        }

        /// <summary>
        /// 读取极限长细比数据
        /// </summary>
        /// <param name="srt">mgt文件流</param>
        public void ReadLimitsRatio(ref StreamReader srt)
        {
            /* 1、准备*/
            bool bRead = false;                     //是否可以读取
            String strText = null;                  //当前行文本
            String strStartFlag = "*LIMITSRATIO";      //数据开始标志
            String strEndFlag = "";                 //数据结束标志
            char szSplit = ',';                     //数据分隔符
            //int iGroupFlag = 0;                  //组数据标志

            /* 2、循环读取*/
            BLimitsRatio blr=null;
            for (strText = srt.ReadLine(); strText != null; strText = srt.ReadLine())
            {
                /* 2.1、判断是否读到数据。若读到，设置标志，进入下一轮循环开始读取；若没有读到，继续进入下一轮判断。*/
                /* 2.2、bRead=true，表示已经可以读数据了。读的时候要判断是否已经读完数据。*/
                if (!bRead)
                {
                    if (strText.StartsWith(strStartFlag))
                    {
                        bRead = true;
                    }
                    continue;
                }
                else if (strText.StartsWith(";"))//如果为注释则忽略
                    continue;
                else if (strText.CompareTo(strEndFlag) == 0)//如果读取数据结尾则返回
                    return;
                else
                {
                    #region 读取数据
                    string[] sCurs = strText.Trim().Split(szSplit);
                    if (sCurs.Length == 4)
                    {
                        List<int> elems = Tools.SelectCollection.StringToList(sCurs[0].Trim());
                        bool bNotCheck = false;
                        if (sCurs[1].Trim() != "NO")
                            bNotCheck = true;
                        double com = double.Parse(sCurs[2].Trim());
                        double ten = double.Parse(sCurs[3].Trim());
                        foreach (int ele in elems)
                        {
                            blr = new BLimitsRatio(ele, com, ten);
                            blr.BNotCheck = bNotCheck;
                            this._LimitsRatios.Add(ele,blr);//添加到数据模型中
                        }
                    }
                    else
                        continue;
                    #endregion
                }
            }
        }

        /// <summary>
        /// 读取MIDAS输出的梁单元内力结果入模型
        /// </summary>
        /// <param name="MidasFile">MIDAS输出的单元内力表，单位默认为kN,m</param>
        public void ReadElemForces(string MidasForceFile)
        {
            string line = null;//行文本
            string[] curdata = null;//当前数据表存储变量
            int curNum = 0;//当前单元号
            string curLC = null;//当前工况名
            double[] tempDouble = new double[6];

            int i = 0;

            FileStream stream = File.Open(MidasForceFile, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);
            line = reader.ReadLine();

            for (line = reader.ReadLine(); line != null; line = reader.ReadLine())
            {
                curdata = line.Split("\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);//字符串分割
                curNum = int.Parse(curdata[0], System.Globalization.NumberStyles.Number);//当前单元号
                curLC = curdata[1];//当前工况名称

                //取得内力数据
                for (int k = 0; k < 6; k++)
                {
                    tempDouble[k] = double.Parse(curdata[k + 3], System.Globalization.NumberStyles.Float);
                }
                //建立截面内力
                SecForce sec1 = new SecForce(tempDouble[0], tempDouble[3], tempDouble[1],
                    tempDouble[2], tempDouble[4], tempDouble[5]);

                #region 往模型数据结构中添加
                if (this.elemforce.ContainsKey(curNum))//如果已有当前单元
                {
                    if (this.elemforce[curNum].hasLC(curLC))//如果已有当前组合
                    {
                        SortedList<string, ElemForce> tempEF = this.elemforce[curNum].LCForces;
                        ;
                        if (curdata[2].StartsWith("I"))
                        {
                            tempEF[curLC].SetElemForce(sec1, 0);
                        }
                        else if (curdata[2].StartsWith("J"))
                        {
                            tempEF[curLC].SetElemForce(sec1, 8);
                        }
                        else if (curdata[2] == "1/4")
                        {
                            tempEF[curLC].SetElemForce(sec1, 2);
                        }
                        else if (curdata[2] == "2/4")
                        {
                            tempEF[curLC].SetElemForce(sec1, 4);
                        }
                        else if (curdata[2] == "3/4")
                        {
                            tempEF[curLC].SetElemForce(sec1, 6);
                        }

                        this.elemforce[curNum].LCForces = tempEF;//反回的到模型数据库中
                    }
                    else
                    {
                        ElemForce ef = new ElemForce();
                        if (curdata[2].StartsWith("I"))
                        {
                            ef.SetElemForce(sec1, 0);
                        }
                        else if (curdata[2].StartsWith("J"))
                        {
                            ef.SetElemForce(sec1, 8);
                        }
                        else if (curdata[2] == "1/4")
                        {
                            ef.SetElemForce(sec1, 2);
                        }
                        else if (curdata[2] == "2/4")
                        {
                            ef.SetElemForce(sec1, 4);
                        }
                        else if (curdata[2] == "3/4")
                        {
                            ef.SetElemForce(sec1, 6);
                        }
                        this.elemforce[curNum].add_LCForce(curLC, ef);
                    }
                }
                else
                {
                    ElemForce ef = new ElemForce();

                    if (curdata[2].StartsWith("I"))
                    {
                        ef.SetElemForce(sec1, 0);
                    }
                    else if (curdata[2].StartsWith("J"))
                    {
                        ef.SetElemForce(sec1, 8);
                    }
                    else if (curdata[2] == "1/4")
                    {
                        ef.SetElemForce(sec1, 2);
                    }
                    else if (curdata[2] == "2/4")
                    {
                        ef.SetElemForce(sec1, 4);
                    }
                    else if (curdata[2] == "3/4")
                    {
                        ef.SetElemForce(sec1, 6);
                    }

                    BElemForceTable eft = new BElemForceTable();
                    eft.add_LCForce(curLC, ef);
                    this.elemforce.Add(curNum, eft);
                }
                #endregion
                i++;
            }
            reader.Close();
        }

        /// <summary>
        /// 读取Midas输出的Truss单元内力信息
        /// </summary>
        /// <param name="MidasTrussForceOut">桁架单元内力表，单位默认为kN</param>
        public void ReadTrussForces(string MidasTrussForceOut)
        {
            string line = null;//行文本
            string[] curdata = null;//当前数据表存储变量
            int curNum = 0;//当前单元号
            string curLC = null;//当前工况名
            double[] tempDouble = new double[2];

            int i = 0;

            FileStream stream = File.Open(MidasTrussForceOut, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);
            line = reader.ReadLine();

            for (line = reader.ReadLine(); line != null; line = reader.ReadLine())
            {
                curdata = line.Split("\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);//字符串分割
                curNum = int.Parse(curdata[0], System.Globalization.NumberStyles.Number);//当前单元号
                curLC = curdata[1];//当前工况名称
                //取得内力数据
                for (int k = 0; k < 2; k++)
                {
                    tempDouble[k] = double.Parse(curdata[k + 2], System.Globalization.NumberStyles.Float);
                }

                //建立截面内力
                SecForce sec1 = new SecForce(tempDouble[0], 0, 0, 0, 0, 0);//I截面内力
                SecForce sec2 = new SecForce(tempDouble[1], 0, 0, 0, 0, 0);//J截面内力

                #region 往模型数据结构中添加
                if (this.elemforce.ContainsKey(curNum))//如果已有当前单元
                {
                    if (this.elemforce[curNum].hasLC(curLC))//如果已有当前组合
                    {
                        SortedList<string, ElemForce> tempEF = this.elemforce[curNum].LCForces;
                        tempEF[curLC].SetElemForce(sec1, sec2);
                        this.elemforce[curNum].LCForces = tempEF;//反回的到模型数据库中
                    }
                    else
                    {
                        ElemForce ef = new ElemForce();
                        ef.SetElemForce(sec1, sec2);
                        this.elemforce[curNum].add_LCForce(curLC, ef);
                    }
                }
                else
                {
                    ElemForce ef = new ElemForce();
                    ef.SetElemForce(sec1, sec2);
                    BElemForceTable eft = new BElemForceTable();
                    eft.add_LCForce(curLC, ef);
                    this.elemforce.Add(curNum, eft);
                }
                #endregion

                i++;
            }

            reader.Close();
        }
        #endregion

        #region model类输出接口方法
        /// <summary>
        /// 写出ANSYS命令流文件
        /// </summary>
        /// <param name="inp">写入文件路径</param>
        /// <param name="BeamType">梁单元类型：1表示beam44，2表示beam188，3表示beam189</param>
        public bool WriteToInp(string inp, int BeamType)
        {
            FileStream stream = File.Open(inp, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine("/COM,Midas2Ansys INP File Created at " + System.DateTime.Now);
            writer.WriteLine("/COM,*******http://www.lubanren.com********");

            #region 宏定义
            writer.WriteLine("\n!SPC截面宏定义...并执行");
            foreach (KeyValuePair<int, BSections> sec in this.sections)
            {
                if (sec.Value is SectionGeneral)
                {
                    SectionGeneral secg = sec.Value as SectionGeneral;
                    writer.WriteLine(secg.GetSectMac());//取得宏
                    writer.WriteLine("*use," + secg.Name + ".sec" + "\t!执行宏");
                }
            }
            #endregion

            writer.WriteLine("FINISH");
            writer.WriteLine("/CLEAR");
            writer.WriteLine("\n/PREP7");
            writer.WriteLine("\n!单元类型信息...");
            switch (BeamType)//根据选择输出单元类型定义命令
            {
                case 1:
                    writer.WriteLine("et,1,44");
                    writer.WriteLine("keyopt,1,10,1");
                    break;
                case 2:
                    writer.WriteLine("et,1,188");
                    writer.WriteLine("keyopt,1,1,1");
                    writer.WriteLine("keyopt,1,3,3");
                    //writer.WriteLine("keyopt,1,9,2");
                    break;
                case 3:
                    writer.WriteLine("et,1,189");
                    break;
                default:
                    //writer.WriteLine("et,1,188");
                    break;
            }
            //板单元类型声明
            writer.WriteLine("et,2,181");
            writer.WriteLine("keyopt,2,3,2");
            writer.WriteLine("keyopt,2,8,2");
            //桁架、只受拉、只受压单元类型声明
            writer.WriteLine("et,3,180");

            //实常数信息
            writer.WriteLine("\n!实常数信息定义...");
            foreach (KeyValuePair<int, BThickness> thi in this.thickness)//遍历板厚信息形成实常数
            {
                writer.WriteLine("r,{0},{1}", thi.Key.ToString(), thi.Value.THIK_IN.ToString());
            }
            #region LINK180单元实常数处理定义
            List<int> TRU_sec = new List<int>();//临时记录变链表
            List<int> TEN_sec = new List<int>();
            List<int> COM_sec = new List<int>();
            foreach (KeyValuePair<int, Element> elem in this.elements)// 遍历单元信息形成实常数（link180单元用）
            {
                if (elem.Value.TYPE == ElemType.TRUSS)
                {
                    int num = elem.Value.iPRO;
                    if (!TRU_sec.Contains(num))
                        TRU_sec.Add(num);
                }
                else if (elem.Value.TYPE == ElemType.TENSTR)
                {
                    int num = elem.Value.iPRO;
                    if (!TEN_sec.Contains(num))
                        TEN_sec.Add(num);
                }
                else if (elem.Value.TYPE == ElemType.COMPTR)
                {
                    int num = elem.Value.iPRO;
                    if (!COM_sec.Contains(num))
                        COM_sec.Add(num);
                }
            }

            foreach (int tru in TRU_sec)
            {
                writer.WriteLine("r,{0},{1},,0", tru + 100, this.sections[tru].Area.ToString("G3"));
            }
            foreach (int ten in TEN_sec)
            {
                writer.WriteLine("r,{0},{1},,1", ten + 200, this.sections[ten].Area.ToString("G3"));
            }
            foreach (int com in COM_sec)
            {
                writer.WriteLine("r,{0},{1},,-1", com + 300, this.sections[com].Area.ToString("G3"));
            }
            #endregion


            writer.WriteLine("\n!截面信息定义...");
            foreach (KeyValuePair<int, BSections> sec in this.sections)
            {
                writer.WriteLine(sec.Value.WriteData());
            }

            #region 材料信息输出
            writer.WriteLine("\n!材料信息定义...");
            foreach (KeyValuePair<int, BMaterial> mat in this.mats)
            {
                writer.WriteLine("mp,ex,{0},{1}", mat.Value.iMAT.ToString("G"), mat.Value.Elast.ToString("G"));
                writer.WriteLine("mp,prxy,{0},{1}", mat.Value.iMAT.ToString("G"), mat.Value.Poisn.ToString("G"));
                writer.WriteLine("mp,dens,{0},{1}", mat.Value.iMAT.ToString("G"), mat.Value.Den.ToString("G"));
                writer.WriteLine("mp,alpx,{0},{1}", mat.Value.iMAT.ToString("G"), mat.Value.Thermal.ToString("G"));
            }
            #endregion
            writer.WriteLine("\n!节点数据信息");
            //输出节点信息
            foreach (KeyValuePair<int, Bnodes> node in this.nodes)
            {
                writer.WriteLine("n," + node.Key.ToString("0") + "," + node.Value.X.ToString() + "," + node.Value.Y.ToString() + "," + node.Value.Z
                    .ToString());
            }

            //如果单元选择是Beam189则进行节点数的增加
            SortedList<int, Bnodes> beam189Mid = new SortedList<int, Bnodes>();//中间节点表
            if (BeamType == 3)
            {
                int maxnode = this.MaxNode;//模型最大节点
                writer.WriteLine("\n![Beam189单元中间节点信息]");
                int i = 1;
                foreach (KeyValuePair<int, Element> elem in this.elements)
                {
                    if (elem.Value.TYPE != ElemType.BEAM)
                        continue;
                    FrameElement bm = elem.Value as FrameElement;
                    Point3d pt1 = nodes[bm.I].Location;
                    Point3d pt2 = nodes[bm.J].Location;
                    Point3d ptM = Point3d.GetMidPoint(pt1, pt2);
                    Bnodes midNode = new Bnodes(i + maxnode, ptM.X, ptM.Y, ptM.Z);//中间节点编号
                    writer.WriteLine("n,{0},{1},{2},{3}", midNode.num, midNode.X, midNode.Y, midNode.Z);
                    //添加到中间节点表，<单元号，中间节点信息>
                    beam189Mid.Add(elem.Value.iEL, midNode);
                    i++;
                }
            }

            //输出单元信息
            writer.WriteLine("\n!单元数据信息");
            foreach (KeyValuePair<int, Element> elem in this.elements)
            {
                //按单元类型分类输出
                switch (elem.Value.TYPE)
                {
                    case ElemType.BEAM:
                        writer.WriteLine("type,1");
                        writer.WriteLine("mat,{0}", elem.Value.iMAT.ToString());
                        writer.WriteLine("secnum," + elem.Value.iPRO.ToString());
                        writer.WriteLine("real,");
                        if (BeamType != 3)//如果没有选择Beam189
                            writer.WriteLine("en," + elem.Value.NodeString());
                        else
                        {
                            int inodes = elem.Value.NodeCount;
                            string beam189out = "en," + elem.Key.ToString();
                            beam189out += "," + elem.Value.iNs[0] + "," + elem.Value.iNs[1]; ;
                            beam189out += "," + beam189Mid[elem.Key].num.ToString();
                            if (inodes == 3)
                                beam189out += "," + elem.Value.iNs[2];
                            writer.WriteLine(beam189out);
                        }
                        break;
                    case ElemType.TRUSS:
                        writer.WriteLine("type,3");
                        writer.WriteLine("mat,{0}", elem.Value.iMAT.ToString());
                        writer.WriteLine("secnum,");
                        writer.WriteLine("real,{0}", elem.Value.iPRO + 100);
                        writer.WriteLine("en,{0}", elem.Value.NodeString());
                        break;
                    case ElemType.PLATE:
                        //writer.WriteLine("!{0}号单元是平面单元", elem.Value.iEL.ToString());
                        writer.WriteLine("real,{0}", elem.Value.iPRO.ToString());//厚度号
                        writer.WriteLine("type,2");
                        writer.WriteLine("mat,{0}", elem.Value.iMAT.ToString());
                        writer.WriteLine("secnum");//将截面号置为初始值
                        writer.WriteLine("en," + elem.Value.NodeString());
                        break;
                    case ElemType.TENSTR:
                        //writer.WriteLine("!{0}号单元是只拉单元", elem.Value.iEL.ToString());
                        writer.WriteLine("type,3");
                        writer.WriteLine("mat,{0}", elem.Value.iMAT.ToString());
                        writer.WriteLine("secnum,");
                        writer.WriteLine("real,{0}", elem.Value.iPRO + 200);
                        writer.WriteLine("en,{0}", elem.Value.NodeString());
                        break;
                    case ElemType.COMPTR:
                        //writer.WriteLine("!{0}号单元是只压单元", elem.Value.iEL.ToString());
                        writer.WriteLine("type,3");
                        writer.WriteLine("mat,{0}", elem.Value.iMAT.ToString());
                        writer.WriteLine("secnum,");
                        writer.WriteLine("real,{0}", elem.Value.iPRO + 300);
                        writer.WriteLine("en,{0}", elem.Value.NodeString());
                        break;
                    default:
                        break;
                }

            }
            //进入后处理模块
            writer.WriteLine("\n/SOLU");
            writer.WriteLine("\n!约束条件");
            #region 一般固定边界输出
            writer.WriteLine("\n!一般固定边界条件");
            foreach (KeyValuePair<int, BConstraint> suport in this.constraint)
            {
                BConstraint nodesuport = suport.Value;
                //apdl命令格式如下：
                //D, NODE, Lab, VALUE, VALUE2, NEND, NINC, Lab2, Lab3, Lab4, Lab5, Lab6
                string NODE = nodesuport.Node.ToString(), 
                    Lab = "", NEND = "", NINC = "", Lab2 = "", Lab3 = "", Lab4 = "", Lab5 = "", Lab6 = "";

                if (nodesuport.UX == true)
                    Lab = "ux";
                if (nodesuport.UY == true)
                    Lab2 = "uy";
                if (nodesuport.UZ == true)
                    Lab3 = "uz";
                if (nodesuport.RX == true)
                    Lab4 = "rotx";
                if (nodesuport.RY == true)
                    Lab5 = "roty";
                if (nodesuport.RZ == true)
                    Lab6 = "rotz";

                writer.WriteLine("d," + NODE + "," + Lab + ",,," + NEND + "," + NINC + "," + Lab2 + "," + Lab3 + "," + Lab4 + "," + Lab5
                    + "," + Lab6);               
            }
            #endregion
            #region 刚性连接输出
            writer.WriteLine("\n!刚性连接（节点耦合）");
            foreach (KeyValuePair<int, BRigidLink> Rigid in this.RigidLinks)
            {
                List<int> RigiNodes = Rigid.Value.SNodesList;//从属节点号
                RigiNodes.Add(Rigid.Value.MNode);//添加主节点号

                writer.WriteLine("nsel,none");
                foreach (int nn in RigiNodes)
                {
                    writer.WriteLine("nsel,a,node,,{0}", nn.ToString());
                }
                writer.WriteLine("cm,cptemp,node");
                writer.WriteLine("allsel,all");
                if (Rigid.Value.bUx)
                {
                    writer.WriteLine("cp,next,ux,cptemp");
                }
                if (Rigid.Value.bUy)
                {
                    writer.WriteLine("cp,next,uy,cptemp");
                }
                if (Rigid.Value.bUz)
                {
                    writer.WriteLine("cp,next,uz,cptemp");
                }
                if (Rigid.Value.bRx)
                {
                    writer.WriteLine("cp,next,rotx,cptemp");
                }
                if (Rigid.Value.bRy)
                {
                    writer.WriteLine("cp,next,roty,cptemp");
                }
                if (Rigid.Value.bRz)
                {
                    writer.WriteLine("cp,next,rotz,cptemp");
                }
                writer.WriteLine("cmdele,cptemp");
            }

            #endregion

            #region 荷载输出
            writer.WriteLine("\n!施加模型荷载");
            foreach (BLoadCase lc in this.STLDCASE)
            {
                writer.WriteLine("\n!工况{0}", lc.LCName);
                writer.WriteLine("*create,LC_{0},lc", lc.LCName);
                #region 输出自重荷载
                foreach (KeyValuePair<string, BWeight> weightdata in this.selfweight)
                {
                    if (weightdata.Value.LC == lc.LCName)
                    {
                        //注意ANSYS中加速度方向反号
                        writer.WriteLine("acel,{0},{1},{2}",
                            (-weightdata.Value.ACELx).ToString(),
                            (-weightdata.Value.ACELy).ToString(),
                            (-weightdata.Value.ACELz).ToString());
                    }
                }
                #endregion
                #region 输出节点荷载
                foreach (KeyValuePair<int, BNLoad> nload in this.conloads)
                {
                    if (nload.Value.LC == lc.LCName)
                    {
                        if (nload.Value.FX != 0)
                        {
                            writer.WriteLine("f,{0},fx,{1}", nload.Value.iNode.ToString(),
                                nload.Value.FX.ToString());
                        }
                        if (nload.Value.FY != 0)
                        {
                            writer.WriteLine("f,{0},fy,{1}", nload.Value.iNode.ToString(),
                                nload.Value.FY.ToString());
                        }
                        if (nload.Value.FZ != 0)
                        {
                            writer.WriteLine("f,{0},fz,{1}", nload.Value.iNode.ToString(),
                                nload.Value.FZ.ToString());
                        }
                        if (nload.Value.MX != 0)
                        {
                            writer.WriteLine("f,{0},mx,{1}", nload.Value.iNode.ToString(),
                                nload.Value.MX.ToString());
                        }
                        if (nload.Value.MY != 0)
                        {
                            writer.WriteLine("f,{0},my,{1}", nload.Value.iNode.ToString(),
                                nload.Value.MY.ToString());
                        }
                        if (nload.Value.MZ != 0)
                        {
                            writer.WriteLine("f,{0},mz,{1}", nload.Value.iNode.ToString(),
                                nload.Value.MZ.ToString());
                        }
                    }
                }
                #endregion
                #region 输出梁单元荷载
                foreach (KeyValuePair<int, BBLoad> bload in this.beamloads)
                {
                    if (bload.Value.LC == lc.LCName)
                    {
                        //输出单元荷载信息
                        if (bload.Value.TYPE == BeamLoadType.UNIMOMENT ||
                            bload.Value.TYPE == BeamLoadType.CONMOMENT ||
                            bload.Value.getP(3) != 0)
                        {
                            writer.WriteLine("!单元({0})荷载为弯矩荷载,在ANSYS中需要单元细化...", bload.Key.ToString());
                        }
                        else if (bload.Value.TYPE == BeamLoadType.UNILOAD)
                        {
                            switch (bload.Value.Dir)
                            {
                                case DIR.GX:
                                    writer.WriteLine("!单元({0})的单元荷载施加坐标系为全局坐标系GX,未处理",
                                        bload.Key.ToString());
                                    break;
                                case DIR.GY:
                                    writer.WriteLine("!单元({0})的单元荷载施加坐标系为全局坐标系GY,未处理",
                                        bload.Key.ToString());
                                    break;
                                case DIR.GZ:
                                    writer.WriteLine("!单元({0})的单元荷载施加坐标系为全局坐标系GZ,暂按局部坐标处理",
                                        bload.Key.ToString());
                                    writer.WriteLine("sfbeam,{0},1,pres,{1},{2},,,,,1",
                                        bload.Key.ToString(),
                                        (-bload.Value.getP(1)).ToString(),
                                        (-bload.Value.getP(2)).ToString());
                                    break;
                                case DIR.LZ:
                                    writer.WriteLine("sfbeam,{0},1,pres,{1},{2},,,{3},{4},1",
                                        bload.Key.ToString(),
                                        (-bload.Value.getP(1)).ToString(),
                                        (-bload.Value.getP(2)).ToString(),
                                        bload.Value.getD(1).ToString(),
                                        bload.Value.getD(2).ToString());
                                    break;
                                case DIR.LY:
                                    writer.WriteLine("sfbeam,{0},2,pres,{1},{2},,,{3},{4},1",
                                        bload.Key.ToString(),
                                        (-bload.Value.getP(1)).ToString(),
                                        (-bload.Value.getP(2)).ToString(),
                                        bload.Value.getD(1).ToString(),
                                        bload.Value.getD(2).ToString());
                                    break;
                                case DIR.LX:
                                    writer.WriteLine("sfbeam,{0},3,pres,{1},{2},,,{3},{4},1",
                                        bload.Key.ToString(),
                                        bload.Value.getP(1).ToString(),
                                        bload.Value.getP(2).ToString(),
                                        bload.Value.getD(1).ToString(),
                                        bload.Value.getD(2).ToString());
                                    break;
                                default:
                                    break;
                            }

                        }
                        else if (bload.Value.TYPE == BeamLoadType.CONLOAD && bload.Value.getP(2) == 0)
                        {
                            switch (bload.Value.Dir)
                            {
                                case DIR.GX:
                                    writer.WriteLine("!单元({0})的单元荷载施加坐标系为全局坐标系GX,未处理",
                                        bload.Key.ToString());
                                    break;
                                case DIR.GY:
                                    writer.WriteLine("!单元({0})的单元荷载施加坐标系为全局坐标系GY,未处理",
                                        bload.Key.ToString());
                                    break;
                                case DIR.GZ:
                                    writer.WriteLine("!单元({0})的单元荷载施加坐标系为全局坐标系GZ,未处理",
                                        bload.Key.ToString());
                                    FrameElement fe = elements[bload.Key] as FrameElement;
                                    CoordinateSystem cs = fe.ECS;
                                    double angel = Vector3.zAxis.Angle(cs.AxisZ);
                                    break;
                                case DIR.LZ:
                                    writer.WriteLine("sfbeam,{0},1,pres,{1},,,,{2},-1,1",
                                        bload.Key.ToString(),
                                        (-bload.Value.getP(1)).ToString(),
                                        bload.Value.getD(1).ToString());
                                    break;
                                case DIR.LY:
                                    writer.WriteLine("sfbeam,{0},2,pres,{1},,,,{2},-1,1",
                                        bload.Key.ToString(),
                                        (-bload.Value.getP(1)).ToString(),
                                        bload.Value.getD(1).ToString());
                                    break;
                                case DIR.LX:
                                    writer.WriteLine("sfbeam,{0},3,pres,{1},,,,{2},-1,1",
                                        bload.Key.ToString(),
                                        bload.Value.getP(1).ToString(),
                                        bload.Value.getD(1).ToString());
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                #endregion
                #region 输出单元温度荷载
                foreach (KeyValuePair<int, BETLoad> Eload in this._EleTempLoads)
                {
                    if (Eload.Value.LC != lc.LCName)
                        continue;
                    writer.WriteLine("bfe,{0},temp,1,{1}",
                        Eload.Value.Elem_Num,
                        Eload.Value.Temp.ToString());
                }
                #endregion
                writer.WriteLine("*end");
            }
            #endregion
            writer.WriteLine("\nallsel,all");
            writer.WriteLine("eplot");
            writer.Close();
            stream.Close();
            return true;
        }

        /// <summary>
        /// 写出ansys宏文件
        /// 功能：将荷载转化为质量
        /// </summary>
        /// <param name="dir">宏文件目录</param>
        /// <returns>是否成功</returns>
        public bool WriteAnsysMarc_Load2Mass(string dir)
        {
            string FileName = Path.Combine(dir, "loadtomass.mac");
            using (FileStream stream = File.Open(FileName, FileMode.Create))
            {
                StreamWriter writer = new StreamWriter(stream);

                #region 宏文件内容
                writer.WriteLine("!宏文件功能：将荷载转化为质量");
                writer.WriteLine("/COM,Ansys Mac File Created at " + System.DateTime.Now);
                writer.WriteLine("/COM,*******http://www.lubanren.com********");

                writer.WriteLine("et,11,21");
                writer.WriteLine("keyopt,11,1,0");
                writer.WriteLine("keyopt,11,2,0");
                writer.WriteLine("keyopt,11,3,0");

                writer.WriteLine("!todo:通过实常数定义节点质量");
                writer.WriteLine("*get,enmax, elem,0, num, maxd");

                //节点荷载进行遍历
                BLoadTable blt = this.LoadTable;
                SortedList<int, BNLoad> NLoad_DL = blt.NLoadData["DL"] as SortedList<int, BNLoad>;
                SortedList<int, BNLoad> NLoad_LL = blt.NLoadData["LL"] as SortedList<int, BNLoad>;
                double DL_ra = 1.0;//工况荷载系数
                double LL_ra = 0.5;
                int i = 1;//单元数指示标志

                List<int> nodes = blt.NodeListForNLoad;//具有节点荷载的节点列表
                foreach (int nn in nodes)
                {

                    if (NLoad_DL.ContainsKey(nn) || NLoad_LL.ContainsKey(nn))
                    {
                        double mass = 0;
                        if (NLoad_DL.ContainsKey(nn))
                        {
                            mass += Math.Abs(NLoad_DL[nn].FZ) / 9.8 * DL_ra;
                        }
                        if (NLoad_LL.ContainsKey(nn))
                        {
                            mass += Math.Abs(NLoad_LL[nn].FZ) / 9.8 * LL_ra;
                        }
                        //实常数定义
                        writer.WriteLine("r,enmax+{0},{1},{1},{1},,,", i, mass.ToString("0.0"));

                        //单元定义
                        writer.WriteLine("real,enmax+{0}", i);
                        writer.WriteLine("type,11$mat$secnum");
                        writer.WriteLine("en,enmax+{0},{1}", i, nn);
                        i++;
                    }
                }

                //清理垃圾
                writer.WriteLine("enmax=");
                writer.WriteLine("real$type$mat$secnum");

                #endregion

                writer.Close();
                stream.Close();
            }

            return true;
        }

        /// <summary>
        /// 写出ansys宏文件
        /// 功能：将结构组转化为Components
        /// </summary>
        /// <param name="dir">宏文件目录</param>
        /// <returns>是否成功</returns>
        public bool WriteAnsysComponents(string dir)
        {
            string FileName = Path.Combine(dir, "group2cm.mac");
            using (FileStream stream = File.Open(FileName, FileMode.Create))
            {
                StreamWriter writer = new StreamWriter(stream);

                #region 宏文件内容
                writer.WriteLine("!宏文件功能：将结构组转化为Components");
                writer.WriteLine("!使用方法:*use,group2cm.mac");
                writer.WriteLine("/COM,Ansys Mac File Created at " + System.DateTime.Now);
                writer.WriteLine("/COM,*******http://www.lubanren.com********");

                //writer.WriteLine(System.Environment.NewLine + "!结构组信息");
                foreach (KeyValuePair<string, BSGroup> sg in this.StruGroups)
                {
                    List<int> sg_nodes = sg.Value.NodeList;
                    List<int> sg_elems = sg.Value.EleList;

                    if (sg_nodes.Count > 0)
                    {
                        writer.WriteLine("nsel,none");
                        foreach (int tt in sg_nodes)
                        {
                            writer.WriteLine("nsel,a,node,," + tt.ToString());
                        }
                        writer.WriteLine("cm,{0}_n,node", sg.Key);
                        writer.WriteLine("allsel,all");
                    }

                    if (sg_elems.Count > 0)
                    {
                        writer.WriteLine("esel,none");
                        foreach (int tt in sg_elems)
                        {
                            writer.WriteLine("esel,a,elem,," + tt.ToString());
                        }
                        writer.WriteLine("cm,{0}_e,elem", sg.Key);
                        writer.WriteLine("allsel,all");
                    }
                }
                #endregion
                writer.Close();
                stream.Close();
            }
            return true;
        }

        /// <summary>
        /// 导出模型为mgt文件
        /// </summary>
        /// <param name="mgtFile">文件绝对路径</param>
        /// <param name="TipOut">过程提示对话框</param>
        /// <returns>是否成功</returns>
        public bool WriteToMGT(string mgtFile,ref ToolStripStatusLabel TipOut)
        {
            FileStream stream = File.Open(mgtFile, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream,Encoding.GetEncoding("gb2312"));
            writer.WriteLine(";---------------------------------------------------------------------------");
            writer.WriteLine(";  midas Gen Text(MGT) File.");
            writer.WriteLine(";  EasyMidas Created at " + System.DateTime.Now);
            writer.WriteLine(";  http://www.lubanren.com");
            writer.WriteLine(";---------------------------------------------------------------------------");
            writer.WriteLine();
            writer.WriteLine("*VERSION");
            writer.WriteLine("   7.8.0");
            writer.WriteLine();
            writer.WriteLine("*UNIT    ; Unit System");
            writer.WriteLine("   {0},{1},{2},{3}",
                this.unit.Force,this.unit.Length,this.unit.Heat,this.unit.Temper);
            
            writer.WriteLine();
            writer.WriteLine("*NODE    ; Nodes");
            foreach(KeyValuePair <int,Bnodes>nn in nodes)
            {
                writer.WriteLine("   {0},{1},{2},{3}",
                    nn.Value.num,nn.Value.X,nn.Value.Y,nn.Value.Z);
            }
            TipOut.Text = "Ben:节点数据导出完成!";//信息提示

            writer.WriteLine();
            writer.WriteLine("*ELEMENT    ; Elements");
            foreach (KeyValuePair<int, Element> ee in elements)
            {
                if (ee.Value.TYPE == ElemType.BEAM)
                {
                    FrameElement fe = ee.Value as FrameElement;
                    
                    writer.WriteLine("   {0},{1},{2},{3},{4},{5},{6}",
                        fe.iEL,fe.TYPE,fe.iMAT,fe.iPRO,fe.I,fe.J,fe.beta);
                }
            }
            TipOut.Text = "Ben:单元数据导出完成!";//信息提示

            writer.WriteLine();
            writer.WriteLine("*SECTION    ; Section");
            foreach (KeyValuePair<int, BSections> sec in sections)
            {
                writer.WriteLine("   {0},{1},{2},CC, 0, 0, 0, 0, 0, 0, YES, SB , 2, 0.8, 0.4, 0, 0, 0, 0, 0, 0, 0, 0", 
                    sec.Value.Num, sec.Value.SecType, sec.Value.Name);
            }
            TipOut.Text = "Ben:截面数据导出完成!";//信息提示

            writer.WriteLine();
            writer.WriteLine("*GROUP    ; Group");
            foreach (KeyValuePair<string, BSGroup> bsg in _StruGroups)
            {
                BSGroup bs = bsg.Value;
                List<int> eles = bs.EleList;//单元表
                if (eles.Count < 2 && eles.Count > 0)
                {
                    writer.WriteLine(" " + bs.GroupName + ", ," + eles[0]);
                    continue;
                }
                else if (eles.Count >= 2)
                {
                    writer.WriteLine(" " + bs.GroupName + ", ," + eles[0] + "\\");
                    for (int i = 1; i < (eles.Count - 1); i++)
                    {
                        writer.WriteLine("                 {0}\\", eles[i]);
                    }
                    writer.WriteLine("                 " + eles[eles.Count - 1]);
                }
            }
            TipOut.Text = "Ben:分组信息导出完成!";//信息提示

            writer.WriteLine();
            writer.WriteLine("*STLDCASE    ; Static Load Cases");
            foreach (string slc in LoadTable.LCList)
            {
                writer.WriteLine("   {0},{1}, ",slc,LCType.USER);
            }

            foreach (string slc in LoadTable.LCList)
            {
                writer.WriteLine();
                writer.WriteLine("*USE-STLD,{0}",slc);
                //节点荷载
                if (LoadTable.NLoadData.ContainsKey(slc))
                {
                    writer.WriteLine();
                    writer.WriteLine("*CONLOAD    ; Nodal Loads");
                    SortedList<int, BNLoad> bnllist = LoadTable.NLoadData[slc] as SortedList<int, BNLoad>;
                    foreach (KeyValuePair<int, BNLoad> bnl in bnllist)
                    {
                        BNLoad bn = bnl.Value;
                        writer.WriteLine("   {0},{1},{2},{3},{4},{5},{6},{7}",
                            bn.iNode,bn.FX,bn.FY,bn.FZ,bn.MX,bn.MY,bn.MZ,bn.Group);
                    }
                }

                //梁单元荷载
                if (LoadTable.BLoadData.ContainsKey(slc))
                {
                    writer.WriteLine();
                    writer.WriteLine("*BEAMLOAD    ; Element Beam Loads");
                    SortedList<int, BBLoad> bbllist = LoadTable.BLoadData[slc] as SortedList<int,BBLoad>;
                    foreach (KeyValuePair<int, BBLoad> bbl in bbllist)
                    {
                        BBLoad bl=bbl.Value;
                        writer.WriteLine("   {0},{1},{2},{3},{4},{5},aDir[1], , , ,{6},{7},{8},{9},{10},{11},{12},{13},",
                            bl.ELEM_num,bl.CMD,bl.TYPE,bl.Dir,(bl.bPROJ? "YES":"NO"),(bl.bECCEN? "YES":"NO"),
                            bl.getD(1),bl.getP(1),bl.getD(2),bl.getP(2),bl.getD(3),bl.getP(3),bl.getD(4),bl.getP(4));
                    }
                }
                
            }
            TipOut.Text = "Ben:荷载数据导出完成!";//信息提示


            writer.WriteLine("*ENDDATA");

            writer.Close();
            stream.Close();
            return true;
        }
        /// <summary>
        /// 导出3d3s模型
        /// </summary>
        /// <param name="Tj3d3sFile">文件绝对路径</param>
        /// <param name="TipOut">过程提示信息</param>
        /// <returns>是否成功</returns>
        public bool WriteTo3D3STxt(string Tj3d3sFile,ref ToolStripStatusLabel TipOut)
        {
            FileStream stream = File.Open(Tj3d3sFile, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream,Encoding.GetEncoding("gb2312"));
            writer.WriteLine("!---------------------------------------------------------------------------");
            writer.WriteLine("!3D3S_v10.1_Text_File.");
            writer.WriteLine("!EasyMidas_Created_at_" +System.DateTime.Now.ToShortDateString()+"_" +System.DateTime.Now.ToShortTimeString());
            writer.WriteLine("!http://www.lubanren.com");
            writer.WriteLine("!---------------------------------------------------------------------------");
            writer.WriteLine();

            writer.WriteLine("STRU 3");
            writer.WriteLine("UNIT 0");
            writer.WriteLine();
            foreach(KeyValuePair <int,Bnodes>nn in nodes)
            {
                writer.WriteLine("N {0} {1} {2} {3}",
                    nn.Value.num,nn.Value.X,nn.Value.Y,nn.Value.Z);
            }

            writer.WriteLine();
            foreach (KeyValuePair<int, Element> ee in elements)
            {
                if (ee.Value.TYPE == ElemType.BEAM)
                {
                    FrameElement fe = ee.Value as FrameElement;
                    writer.WriteLine("E {0} {1} {2}",fe.iEL,fe.I,fe.J);
                }
            }
            TipOut.Text = "Ben:单元数据导出成功!";//信息提示

            writer.WriteLine();
            foreach (KeyValuePair<int, Element> ee in elements)
            {
                if (ee.Value.TYPE == ElemType.BEAM)
                {
                    FrameElement fe = ee.Value as FrameElement;
                    writer.WriteLine("DES {0} 17 {1}", fe.iEL, fe.iPRO);
                }
            }
            TipOut.Text = "Ben:单元截面分组导出成功!";//信息提示

            writer.WriteLine();
            foreach (KeyValuePair<int, BMaterial> bm in mats)
            {
                writer.WriteLine("MAT 1 0 0 0 0 0");
            }

            writer.WriteLine();
            foreach (KeyValuePair<int, Element> ee in elements)
            {
                if (ee.Value.TYPE == ElemType.BEAM)
                {
                    FrameElement fe = ee.Value as FrameElement;
                    writer.WriteLine("DEM {0} {1}", fe.iEL, fe.iMAT);
                }
            }
            TipOut.Text = "Ben:单元材料分组导出成功!";//信息提示

            writer.WriteLine();
            #region 荷载数据导出
            writer.WriteLine("!Load_Begin");

            int numForNodeLoadlib = 0;//节点荷载库序号：初始值
            int numForBeamLoadLib = 0;//单元荷载库序号:初始值
            foreach (string slc in LoadTable.LCList)
            {
                //writer.WriteLine();
                writer.WriteLine("!Current_Load:{0}", slc);
                string LCt = "2";//荷载工况类型，用于3d3s输入
                if (slc == "0"||slc=="1")
                    LCt = slc;
                //节点荷载
                if (LoadTable.NLoadData.ContainsKey(slc))
                {
                    writer.WriteLine();
                    SortedList bnllist = LoadTable.NLoadData[slc] as SortedList;
                    foreach (BNLoad bn in bnllist.Values)
                    {
                        writer.WriteLine("PL {0} {1} {2} {3} {4} {5} {6} {7}",LCt,slc,
                            bn.FX,bn.FY,bn.FZ,bn.MX,bn.MY,bn.MZ);
                        numForNodeLoadlib++;
                        writer.WriteLine("APL {0} {1}",bn.iNode,numForNodeLoadlib);
                    }
                }

                //梁单元荷载
                if (LoadTable.BLoadData.ContainsKey(slc))
                {

                    writer.WriteLine();
                    SortedList<int, BBLoad> bbllist = LoadTable.BLoadData[slc] as SortedList<int, BBLoad>;
                    foreach (KeyValuePair<int, BBLoad> bbl in bbllist)
                    {
                        BBLoad bl = bbl.Value;
                        if (bl.bIsOrdinaryUNILOAD)//均布
                        {
                            writer.WriteLine("EL {0} {1} 1 2 {2} {3} {4} {5}",LCt,slc,bl.getP(1),bl.getP(2),bl.getD(1),this.getFrameLength(bl.ELEM_num));
                            numForBeamLoadLib++;
                            writer.WriteLine("AEL {0} {1}",bl.ELEM_num,numForBeamLoadLib);
                        }
                        else if (bl.bIsTriangleUNILOAD)//三角形
                        {
                            writer.WriteLine("EL {0} {1} 5 2 {2} 0 {3} 0", LCt, slc, bl.getP(2), bl.getD(2)*this.getFrameLength(bl.ELEM_num) );
                            numForBeamLoadLib++;
                            writer.WriteLine("AEL {0} {1}", bl.ELEM_num, numForBeamLoadLib);
                        }
                        else if (bl.bIsTrapezoidalUNILOAD)//梯形
                        {
                            writer.WriteLine("EL {0} {1} 6 2 {2} 0 {3} {4}",
                                LCt, slc, bl.getP(2), bl.getD(2) * this.getFrameLength(bl.ELEM_num),
                                bl.getD(3)*this.getFrameLength(bl.ELEM_num));
                            numForBeamLoadLib++;
                            writer.WriteLine("AEL {0} {1}", bl.ELEM_num, numForBeamLoadLib);
                        }
                    }
                }

            }
#endregion
            TipOut.Text = "Ben:单元荷载导出成功!";//信息提示
            
            //分导信息
            writer.WriteLine();
            foreach (KeyValuePair<string, BSGroup> bsg in _StruGroups)
            {
                BSGroup sg = bsg.Value;//结构组
                foreach (int ele in sg.EleList)
                {
                    writer.WriteLine("LAYER {0} {1}",ele,sg.GroupName);
                }
            }
            TipOut.Text = "Ben:单元分层信息导出成功!";//信息提示

            //输出按截面分层信息，默认注释掉
            writer.WriteLine();
            writer.WriteLine("!截面分层信息，默认注释掉");
            foreach (KeyValuePair<int, Element> ee in elements)
            {
                if (ee.Value.TYPE == ElemType.BEAM)
                {
                    FrameElement fe = ee.Value as FrameElement;
                    writer.WriteLine("!LAYER {0} {1}", fe.iEL, this.sections[fe.iPRO].Name);
                }
            }
            TipOut.Text = "Ben:单元截面分层信息导出成功!";//信息提示

            writer.Close();
            stream.Close();

            return true;
        }

        /// <summary>
        /// 导出Sap2000文本文件
        /// </summary>
        /// <param name="S2KFile">文件绝对路径</param>
        /// <param name="TipOut">过程提示信息</param>
        /// <returns>是否成功</returns>
        public bool WriteToS2KTxt(string S2KFile, ref ToolStripStatusLabel TipOut)
        {
            FileStream stream = File.Open(S2KFile, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream, Encoding.GetEncoding("gb2312"));
            writer.WriteLine("!---------------------------------------------------------------------------");
            writer.WriteLine("!SAP2000_Text_File.");
            writer.WriteLine("!EasyMidas_Created_at_" + System.DateTime.Now.ToShortDateString() + "_" + System.DateTime.Now.ToShortTimeString());
            writer.WriteLine("!http://www.lubanren.com");
            writer.WriteLine("!---------------------------------------------------------------------------");
            writer.WriteLine();

            //程序控制表
            writer.WriteLine("TABLE:  \"{0}\"", "PROGRAM CONTROL");
            writer.WriteLine("   ProgramName=SAP2000   Version=14.1.0   ProgLevel=Advanced   LicenseOS=Yes   LicenseSC=Yes   LicenseBR=Yes   LicenseHT=No   CurrUnits=\"{0}, {1}, {2}\"   SteelCode=\"Chinese 2002\"   ConcCode=\"Chinese 2002\"   AlumCode=\"AA-ASD 2000\" _",
                UNIT.Force,UNIT.Length,UNIT.Temper);
            writer.WriteLine("        ColdCode=AISI-ASD96   BridgeCode=JTG-D62-2004   RegenHinge=Yes");
            writer.WriteLine();
            //自由度和坐标系表
            writer.WriteLine("TABLE:  \"{0}\"", "ACTIVE DEGREES OF FREEDOM");
            writer.WriteLine("   UX=Yes   UY=Yes   UZ=Yes   RX=Yes   RY=Yes   RZ=Yes");
            writer.WriteLine();
            writer.WriteLine("TABLE:  \"{0}\"", "COORDINATE SYSTEMS");
            writer.WriteLine("   Name=GLOBAL   Type=Cartesian   X=0   Y=0   Z=0   AboutZ=0   AboutY=0   AboutX=0");
            writer.WriteLine();

            //材料表
            writer.WriteLine("TABLE:  \"{0}\"", "MATERIAL PROPERTIES 01 - GENERAL");
            foreach (KeyValuePair<int, BMaterial> mat in this.mats)
            {
                writer.WriteLine("   Material={0}   Type=Concrete  SymType=Isotropic   TempDepend=No   Color=Blue", mat.Value.MNAME);
            }
            writer.WriteLine();

            //截面表
            writer.WriteLine("TABLE:  \"{0}\"", "FRAME SECTION PROPERTIES 01 - GENERAL");
            foreach (KeyValuePair<int, BSections> sec in sections)
            {
                string nn=sec.Value.Name;//截面名
                string[] temp=nn.Split('-','_','x');
                 double t2=double.Parse(temp[1]);
                    double t3=double .Parse(temp[2]);
                if (temp[0]=="WB")
                {
                   t2=t2/1000;
                    t3=t3/1000;
                }
                writer.WriteLine("   SectionName={0}   Material={3}   Shape=Rectangular   t3={1}   t2={2}   Area=0  TorsConst=0   I33=0   I22=0   AS2=0   AS3=0   S33=0   S22=0   Z33=0   Z22=0   R33=0",
                    sec.Value.Name, t3, t2,"C30");
            }
            writer.WriteLine();

            //节点坐标表
            writer.WriteLine("TABLE:  \"{0}\"", "JOINT COORDINATES");
            foreach (KeyValuePair<int, Bnodes> nn in nodes)
            {
                writer.WriteLine("   Joint={0}   CoordSys=GLOBAL   CoordType=Cartesian   XorR={1}   Y={2}   Z={3}   SpecialJt=No   GlobalX={1}   GlobalY={2}   GlobalZ={3}",
                    nn.Value.num,nn.Value.X,nn.Value.Y,nn.Value.Z);
            }
            writer.WriteLine();
            TipOut.Text = "Ben:节点坐标表导出完成!";//信息提示

            //梁单元表
            writer.WriteLine("TABLE:  \"{0}\"", "CONNECTIVITY - FRAME");
            foreach (KeyValuePair<int, Element> ee in elements)
            {
                if (ee.Value.TYPE == ElemType.BEAM)
                {
                    FrameElement fe = ee.Value as FrameElement;
                    double ll = this.getFrameLength(fe.iEL);//单元长
                    Point3d p1=nodes[fe.I].Location;
                    Point3d p2=nodes[fe.J].Location;
                    Point3d pMid=Point3d .GetMidPoint(p1,p2);
                    writer.WriteLine("   Frame={0}   JointI={1}   JointJ={2}   IsCurved=No   Length={3}   CentroidX={4}   CentroidY={5}   CentroidZ={6}",
                        fe.iEL, fe.I,fe.J,ll.ToString(),
                        pMid.X,pMid.Y,pMid.Z);
                }
            }
            writer.WriteLine();
            //梁单元截面指定表
            writer.WriteLine("TABLE:  \"{0}\"", "FRAME SECTION ASSIGNMENTS");
            foreach (KeyValuePair<int, Element> ee in elements)
            {
                if (ee.Value.TYPE == ElemType.BEAM)
                {
                    FrameElement fe = ee.Value as FrameElement;
                    string Strsec=sections[fe.iPRO].Name;
                    writer.WriteLine("   Frame={0}   SectionType=Rectangular   AutoSelect=N.A.   AnalSect={1}   DesignSect={1}   MatProp=Default",
                        fe.iEL,Strsec );
                }
            }
            writer.WriteLine();

            //墙单元表
            writer.WriteLine("TABLE:  \"{0}\"", "CONNECTIVITY - AREA");
            foreach (KeyValuePair<int, Element> ee in elements)
            {
                if (ee.Value.TYPE == ElemType.WALL)
                {
                    PlanarElement pe = ee.Value as PlanarElement;
                    List<int> Nns = pe.iNs;
                    writer.WriteLine("   Area={0}   NumJoints=4   Joint1={1}   Joint2={2}   Joint3={3}   Joint4={4}   Perimeter=0   AreaArea=0   CentroidX=0   CentroidY=0   CentroidZ=0",
                        pe.iEL, Nns[0], Nns[1], Nns[2],Nns[3]);
                }
            }
            writer.WriteLine();

            writer.WriteLine("END TABLE DATA");
            writer.Close();
            stream.Close();

            return true;
        }

        /// <summary>
        /// 导出OpenSees TCL命令文件
        /// </summary>
        /// <param name="tclFile">文件绝对路径</param>
        /// <param name="TipOut">过程提示信息</param>
        /// <returns>是否成功</returns>
        public bool WriteToOpenSees(string tclFile, ref ToolStripStatusLabel TipOut)
        {
            FileStream stream = File.Open(tclFile, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream, Encoding.GetEncoding("gb2312"));
            writer.WriteLine("# ---------------------------------------------------------------------------");
            writer.WriteLine("# OpenSees_TCL_File.");
            writer.WriteLine("# EasyMidas Created at:" + System.DateTime.Now.ToShortDateString() + "_" + System.DateTime.Now.ToShortTimeString());
            writer.WriteLine("# http://www.lubanren.com");
            writer.WriteLine("# ---------------------------------------------------------------------------");
            writer.WriteLine();

            //模型控制
            writer.WriteLine("wipe");
            writer.WriteLine("# Create ModelBuilder");
            writer.WriteLine("model basic -ndm 3 -ndf 6;");
            writer.WriteLine();                   

            //节点定义
            writer.WriteLine("# Define NODAL COORDINATES");
            foreach (KeyValuePair<int, Bnodes> nn in nodes)
            {
                writer.WriteLine("node {0} {1} {2} {3}",
                    nn.Value.num, nn.Value.X, nn.Value.Y, nn.Value.Z);
            }
            writer.WriteLine();
            TipOut.Text = "Ben:节点坐标表导出完成!";//信息提示

            //节点边界条件
            writer.WriteLine("# Define Constraint");
            foreach (KeyValuePair<int, BConstraint> suport in this.constraint)
            {
                BConstraint nodesuport = suport.Value;
                string NODE = nodesuport.Node.ToString(),
                    Lab = "0", Lab2 = "0", Lab3 = "0", Lab4 = "0", Lab5 = "0", Lab6 = "0";

                if (nodesuport.UX == true)
                    Lab = "1";
                if (nodesuport.UY == true)
                    Lab2 = "1";
                if (nodesuport.UZ == true)
                    Lab3 = "1";
                if (nodesuport.RX == true)
                    Lab4 = "1";
                if (nodesuport.RY == true)
                    Lab5 = "1";
                if (nodesuport.RZ == true)
                    Lab6 = "1";

                writer.WriteLine("fix {0} {1} {2} {3} {4} {5} {6}",
                    NODE, Lab ,Lab2, Lab3 , Lab4 , Lab5,Lab6);               
            }
            writer.WriteLine();

            //梁单元方向定义
            writer.WriteLine("# Transformation");
            //方向1：vecxz=(0,0,1)，用于不与z轴平行的单元
            writer.WriteLine("geomTransf Linear 1 0 0 1");
            //方向2：vecxz=(1,0,0),用于与z轴平行的单元
            writer.WriteLine("geomTransf Linear 2 1 0 0");
            writer.WriteLine();
            
            //梁单元定义
            writer.WriteLine("# Define Beam Elements");
            foreach (KeyValuePair<int, Element> elem in this.elements)
            {
                //按单元类型分类输出
                switch (elem.Value.TYPE)
                {
                    case ElemType.BEAM:
                        FrameElement fe=elem.Value as FrameElement;
                        double A = sections[fe.iPRO].Area;//面积
                        double E = mats[fe.iMAT].Elast;//弹性模量
                        double G = mats[fe.iMAT].G;//剪切模量
                        double J = sections[fe.iPRO].Ixx;//扭转贯性矩
                        double Iy = sections[fe.iPRO].Iyy;//惯性矩
                        double Iz = sections[fe.iPRO].Izz;
                        double massDens = mats[fe.iMAT].Den * A;//梁单元单位长度质量
                        //element elasticBeamColumn $eleTag $iNode $jNode $A $E $G $J $Iy $Iz $transfTag <-mass $massDens> <-cMass>
                        string transTag = "1";
                        Vector3 vecBeam=this.getFrameVec(fe.iEL);//梁单元方向向量
                        double vec_L = Math.Sqrt(Math.Pow(vecBeam.X,2)+ Math.Pow(vecBeam.Y,2));
                        if (vec_L < 0.001)//如果梁单元平行于全局z轴
                            transTag = "2";
                        writer.WriteLine("element elasticBeamColumn {0} {1} {2} {3} {4} {5} {6} {7} {8} {9} -mass {10}",
                            fe.iEL,fe.I,fe.J,A,E,G,J,Iy,Iz,transTag,massDens);
                        break;
                    case ElemType.TRUSS:
                        writer.WriteLine("# Truss Elem:{0}",elem.Value.iEL);
                        break;
                    case ElemType.PLATE:
                        writer.WriteLine("# Plate Elem:{0}", elem.Value.iEL);
                        break;
                    case ElemType.TENSTR:
                        writer.WriteLine("# Tenstr Elem:{0}", elem.Value.iEL);
                        break;
                    case ElemType.COMPTR:
                        writer.WriteLine("# Comptr Elem:{0}", elem.Value.iEL);
                        break;
                    default:
                        break;
                }
            }
            writer.WriteLine();
            //关闭文件
            writer.Close();
            stream.Close();
            return true;
        }
        /// <summary>
        /// 导出Sqlit数据库文件
        /// </summary>
        /// <param name="tclFile">文件绝对路径</param>
        /// <param name="TipOut">过程提示信息</param>
        /// <returns>是否成功</returns>
        public bool WriteToSqliteDb(string dbFile, ref ToolStripStatusLabel Tipout)
        {
            SQLiteConnection.CreateFile(dbFile);//创建数据库文件

            List<string> cmds=new List<string> ();
            cmds.Add("CREATE TABLE Bnodes(Num integer, X real, Y real, Z real)");
            cmds.Add("CREATE TABLE Belements(Num integer, Type text, iMat integer,iPro integer,iN1 integer, iN2 integer, Angle real)");
            this.ExecuteSQL(dbFile, cmds);//创建表
            Tipout.Text = "Ben:数据表创建完成!";

            cmds.Clear();
            foreach (KeyValuePair<int, Bnodes> nn in nodes)
            {
                string cmdInsert = string.Format("INSERT INTO Bnodes (Num,X,Y,Z) VALUES({0},{1},{2},{3})",
                    nn.Key,nn.Value.X,nn.Value.Y,nn.Value.Z);
                cmds.Add(cmdInsert);
            }
            this.ExecuteSQL(dbFile, cmds);//创建表
            Tipout.Text = "Ben:节点数据写出完成!";

            cmds.Clear();
            foreach (KeyValuePair<int, Element> elem in this.elements)
            {
                //按单元类型分类输出
                switch (elem.Value.TYPE)
                {
                    case ElemType.BEAM:
                        FrameElement fe = elem.Value as FrameElement;
                        string cmdInsert = string.Format("INSERT INTO Belements (Num,Type,iMat,iPro,iN1,iN2,Angle) VALUES({0},'{1}',{2},{3},{4},{5},{6})",
                    fe.iEL,fe.TYPE.ToString(), fe.iMAT, fe.iPRO, fe.iNs[0], fe.iNs[1], fe.beta);
                        cmds.Add(cmdInsert);
                        break;
                    case ElemType.TRUSS:
                        //桁架单元写出
                        break;
                    case ElemType.PLATE:
                        //桁架单元写出
                        break;
                    case ElemType.TENSTR:
                        //桁架单元写出
                        break;
                    case ElemType.COMPTR:
                        //桁架单元写出
                        break;
                    default:
                        break;
                }
            }
            this.ExecuteSQL(dbFile, cmds);//创建表
            Tipout.Text = "Ben:单元数据写出完成!";

            //todo:1.桁架单元、板单元数据写出
            //todo:2.材料数据写出
            //todo:3.截面数据写出
            //todo:4.分组信息写出
            //todo:5.单位信息写出
            //todo:6.节点荷载信息写出
            //todo:7.单元荷载信息写出
            return true;
        }

        /// <summary>
        /// 执行sql非查询语句,仅针对sqlite数据库；
        /// </summary>
        /// <param name="sql">命令集，可以传递多条命令</param>
        /// <param name="dbFile">数据库文件路径</param>
        private void ExecuteSQL(string dbFile,List<string> sqls)
        {
            SQLiteConnection conn = new SQLiteConnection ();
            SQLiteConnectionStringBuilder ssb = new SQLiteConnectionStringBuilder();
            ssb.DataSource = dbFile;
            conn.ConnectionString = ssb.ConnectionString;
            using (conn)
            {
                SQLiteCommand comm = new SQLiteCommand(conn);
                conn.Open();//打开数据库             
                DbTransaction trans = conn.BeginTransaction();//创建事务
                try
                {
                    //创建数据表
                    foreach (string cmd in sqls)
                    {
                        comm.CommandText = cmd;
                        comm.ExecuteNonQuery();
                    }                                      
                    trans.Commit();//提交事务
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }

                conn.Close();//关闭数据库
            }
        }
        #endregion
    }
}
