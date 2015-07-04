using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MidasGenModel.Design;
using MidasGenModel.Geometry3d;


namespace MidasGenModel.model
{
    #region Model Info(模型特性类)

    /// <summary>
    /// 模型单位信息
    /// </summary>
    [Serializable]
    public class BUNIT
    {
        private string _Force;
        private string _Length;
        private string _Heat;
        private string _Temper;

        /// <summary>
        /// 力的单位：N、KN等
        /// </summary>
        public string Force
        {
            set { _Force = value; }
            get { return _Force; }
        }

        /// <summary>
        /// 长度单位：m、mm等
        /// </summary>
        public string Length
        {
            set { _Length = value; }
            get { return _Length; }
        }

        /// <summary>
        /// 热量单位：kJ等
        /// </summary>
        public string Heat
        {
            set { _Heat = value; }
            get { return _Heat; }
        }

        /// <summary>
        /// 温度单位：C等
        /// </summary>
        public string Temper
        {
            set { _Temper = value; }
            get { return _Temper; }
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BUNIT()
        {
            _Force = "N";
            _Length = "M";
            _Heat = "KJ";
            _Temper = "C";
        }
    }
    #endregion
    #region Load Class(荷载类)

    /// <summary>
    /// 荷载工况类型
    /// </summary>
    public enum LCType
    {
        /// <summary>
        /// 恒荷
        /// </summary>
        D,
        /// <summary>
        /// 活荷
        /// </summary>
        L,
        /// <summary>
        /// 风荷
        /// </summary>
        W,
        /// <summary>
        /// 地震荷载
        /// </summary>
        E,
        /// <summary>
        /// 屋面活荷
        /// </summary>
        LR,
        /// <summary>
        /// 雪荷
        /// </summary>
        S,
        /// <summary>
        /// 温度荷载
        /// </summary>
        T,
        /// <summary>
        /// 预应力荷载
        /// </summary>
        PS,
        /// <summary>
        /// 用户定义
        /// </summary>
        USER
    }

    /// <summary>
    /// 荷载组合的种类
    /// </summary>
    public enum LCKind
    {
        /// <summary>
        /// General组合
        /// </summary>
        GEN,
        /// <summary>
        /// 钢结构设计用组合
        /// </summary>
        STEEL,
        /// <summary>
        /// 混凝土设计用组合
        /// </summary>
        CONC,
        /// <summary>
        /// SRC设计用组合
        /// </summary>
        SRC,
        /// <summary>
        /// 基础设计用组合
        /// </summary>
        FDN
    }

    /// <summary>
    /// 单位荷载条件种类
    /// </summary>
    public enum ANAL
    {
        /// <summary>
        /// Static 静力
        /// </summary>
        ST,
        /// <summary>
        /// Response Spectrum 反应谱
        /// </summary>
        RS,
        /// <summary>
        /// 偶然偏心的反应谱结果
        /// </summary>
        ES,
        /// <summary>
        /// Time History 时程
        /// </summary>
        TH,
        /// <summary>
        /// Moving 移动
        /// </summary>
        MV,
        /// <summary>
        /// Settlement 沉降
        /// </summary>
        SM,
        /// <summary>
        /// 组合
        /// </summary>
        CB,
        /// <summary>
        /// 钢结构组合
        /// </summary>
        CBS
    }
    /// <summary>
    ///加载方向 
    /// </summary>
    public enum DIR
    {
        /// <summary>
        /// 整体坐标X向
        /// </summary>
        GX,
        /// <summary>
        /// 整体坐标Y向
        /// </summary>
        GY,
        /// <summary>
        /// 整体坐标Z向
        /// </summary>
        GZ,
        /// <summary>
        /// 单元局部坐标X向
        /// </summary>
        LX,
        /// <summary>
        /// 单元局部坐标Y向
        /// </summary>
        LY,
        /// <summary>
        /// 单元局部坐标Z向
        /// </summary>
        LZ
    }
    /// <summary>
    /// 荷载工况类
    /// </summary>
    [Serializable]
    public class BLoadCase
    {
        /// <summary>
        /// 荷载工况名称
        /// </summary>
        public string LCName;
        /// <summary>
        /// 荷载工况类型
        /// </summary>
        public LCType LCType;
        private ANAL _ANALType;//荷载计算条件种类
        /// <summary>
        /// 荷载计算条件种类：静力，反应谱等
        /// </summary>
        public ANAL ANALType
        {
            get { return _ANALType; }
            set { _ANALType = value; }
        }
        /// <summary>
        /// 按名称实例化荷载工况（默认类型为User;计算条件类型为ST）
        /// </summary>
        /// <param name="Name">工况名</param>
        public BLoadCase(string Name)
        {
            LCName = Name;
            LCType = LCType.USER;
            _ANALType = ANAL.ST;
        }
    }

    /// <summary>
    /// 荷载工况组和系数对
    /// </summary>
    [Serializable]
    public class BLCFactGroup:ICloneable
    {
        private ANAL _ANAL;
        /// <summary>
        /// 单位荷载条件的种类
        /// </summary>
        public ANAL ANAL
        {
            get { return _ANAL; }
            set { _ANAL = value; }
        }
        private string _LCNAME;
        /// <summary>
        /// 工况名称
        /// </summary>
        public string LCNAME
        {
            get { return _LCNAME; }
            set { _LCNAME = value; }
        }
        private double _FACT;
        /// <summary>
        /// 单位荷载条件的荷载系数
        /// </summary>
        public double FACT
        {
            get { return _FACT; }
            set { _FACT = value; }
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BLCFactGroup()
        {
            _ANAL = ANAL.ST;
            _LCNAME = null;
            _FACT = 0;
        }
        /// <summary>
        /// 由荷载工况和系数生成
        /// </summary>
        /// <param name="lc">荷载工况</param>
        /// <param name="f">系数</param>
        public BLCFactGroup(BLoadCase lc,double f)
        {
            _ANAL = lc.ANALType;
            _LCNAME = lc.LCName;
            _FACT = f;
        }
        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    /// <summary>
    /// 荷载组合类
    /// </summary>
    [Serializable]
    public class BLoadComb:System.Object,ICloneable
    {
        #region 数据与属性
        protected string _NAME;//荷载组合条件的名称
        protected LCKind _KIND;//荷载组合的种类
        protected bool _bACTIVE;//是否激活
        private bool _bES;//不清楚的参数：一般多为NO
        protected int _iTYPE;//指定荷载组合方式：0为线性，1为包络,2为ABS，3为SRSS-平方开根号
        protected string _DESC;//简单说明
        protected List<BLCFactGroup> _LoadCombData;//荷载组合数据,一般为mgt文件第二行后数据

        /// <summary>
        /// 荷载组合条件的名称
        /// </summary>
        public string NAME
        {
            get { return _NAME; }
            set { _NAME = value; }
        }
        /// <summary>
        /// 荷载组合的种类
        /// </summary>
        public LCKind KIND
        {
            get { return _KIND; }
            set { _KIND = value; }
        }
        /// <summary>
        /// 荷载组合描述
        /// </summary>
        public string DESC
        {
            get { return _DESC; }
            set {_DESC=value;}
        }
        /// <summary>
        /// 当前组合是否激活
        /// </summary>
        public bool bACTIVE
        {
            get { return _bACTIVE; }
            set { _bACTIVE = value; }
        }
        /// <summary>
        /// 当前组合是否为地震作用组合
        /// </summary>
        public bool bES
        {
            get { return _bES; }
            set { _bES = value; }
        }
        /// <summary>
        /// 荷载组合方式：
        /// 0为线性，1为包络,2为ABS，3为SRSS-平方开根号
        /// </summary>
        public int iTYPE
        {
            get { return _iTYPE; }
        }

        /// <summary>
        /// 取得工况组的数量
        /// </summary>
        public int Num_LCGroup
        {
            get { return _LoadCombData.Count; }
        }

        /// <summary>
        /// 荷载工况系数对数据
        /// </summary>
        public List<BLCFactGroup> LoadCombData
        {
            get { return _LoadCombData; }
        }
        #endregion
        /// <summary>
        /// 构造函数
        /// </summary>
        public BLoadComb()
        {
            _bACTIVE = true;
            _KIND = LCKind.GEN;
            _bES = false;
            _iTYPE = 0;
            _LoadCombData = new List<BLCFactGroup>();
        }
        #region 方法函数
        /// <summary>
        /// 设置组合基本信息
        /// </summary>
        /// <param name="Name">组合条件名称</param>
        /// <param name="Kind">荷载组合种类</param>
        /// <param name="bActive">是否激活</param>
        /// <param name="bEs">不清楚的参数：一般多为NO</param>
        /// <param name="iType">指定荷载组合方式：0为线性，1为+SRSS,2为-SRSS</param>
        /// <param name="Desc">简单说明</param>
        public void SetData1(string Name, LCKind Kind, bool bActive, bool bEs,
            int iType, string Desc)
        {
            _NAME = Name;
            _KIND = Kind;
            _bACTIVE = bActive;
            _bES = bEs;
            _iTYPE = iType;
            _DESC = Desc;
        }

        /// <summary>
        /// 添加荷载工况组和系数对入当前组合
        /// </summary>
        /// <param name="lcfg">荷载工况组和系数对</param>
        public void AddLCFactGroup(BLCFactGroup lcfg)
        {
            _LoadCombData.Add(lcfg);
        }

        /// <summary>
        /// 荷载组合初始化：移除所有组合数据
        /// </summary>
        public void Clear()
        {
            _NAME="";
            _KIND=LCKind.GEN;
            _bACTIVE=true;
            _bES=false;
            _iTYPE=0;
            _DESC="";
            _LoadCombData.Clear();//移除所有元素
        }

        /// <summary>
        /// 判断当前组合是否含有某个工况
        /// </summary>
        /// <param name="LCName">工况名</param>
        /// <returns>含有为true</returns>
        public bool hasLC(string LCName)
        {
            foreach (BLCFactGroup bfg in _LoadCombData)
            {
                if (bfg.LCNAME == LCName)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 判断当前组合是否含有集合中（任何一个）的工况
        /// </summary>
        /// <param name="LCs">集合名</param>
        /// <returns>是否含有任一个</returns>
        public bool hasLC(List<string> LCs)
        {
            foreach (BLCFactGroup bfg in _LoadCombData)
            {
                if (LCs.Contains(bfg.LCNAME))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断当年组合是否含有某种类型工况，如地震反应谱
        /// </summary>
        /// <param name="type">工况类型</param>
        /// <returns>是或否</returns>
        public bool hasLC_ANAL(ANAL type)
        {
            foreach (BLCFactGroup bfg in _LoadCombData)
            {
                if (bfg.ANAL==type)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 组合整体放大
        /// </summary>
        /// <param name="factor">放大因子</param>
        public void  magnifyComb(double factor)
        {
            foreach (BLCFactGroup bfg in _LoadCombData)
            {
                bfg.FACT = bfg.FACT * factor;
            }
        }
        /// <summary>
        /// 对单个工况进行放大
        /// </summary>
        /// <param name="LcNames">工况名子集合</param>
        /// <param name="factor">放大因子</param>
        public void magnifyLc(List<string> LcNames, double factor)
        {
            foreach (BLCFactGroup bfg in _LoadCombData)
            {
                if (LcNames.Contains(bfg.LCNAME))
                {
                    bfg.FACT = bfg.FACT * factor;
                }            
            }
        }
        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            BLoadComb res = new BLoadComb();
            res.SetData1(_NAME, _KIND, _bACTIVE, _bES, _iTYPE, _DESC);
            foreach (BLCFactGroup bfg in _LoadCombData)
            {
                BLCFactGroup newBfg = bfg.Clone() as BLCFactGroup;
                res.AddLCFactGroup(newBfg);
            }
            return res;
        }
        #endregion
    }

    /// <summary>
    /// 荷载基本组合
    /// </summary>
    [Serializable]
    public class BLoadCombG : BLoadComb
    {
        private LCType _CtrLC;
        /// <summary>
        /// 控制组合类型
        /// </summary>
        public LCType CtrLC
        {
            get { return _CtrLC; }
            set { _CtrLC = value; }
        }
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="ctrlc">控制工况</param>
        public BLoadCombG(LCType ctrlc):base()
        {
            _CtrLC = ctrlc;//控制工况
        }
    }

    /// <summary>
    /// 荷载组合表
    /// </summary>
    [Serializable]
    public class BLoadCombTable
    {
        #region 数据成员
        private List<string> _ComGen;//一般组合表
        private List<string> _ComSteel;//钢结构验算用表
        private List<string> _ComCon;//混凝土验算用表
        private Hashtable _LoadCombData_G;//一般组合数据哈希表
        private Hashtable _LoadCombData_S;//钢结构组合哈希表
        private Hashtable _LoadCombData_C;//混凝土组合哈希表
        #endregion
        #region 属性
        /// <summary>
        /// 一般组合表
        /// </summary>
        public List<string> ComGen
        {
            get { return _ComGen; }
        }
        /// <summary>
        /// 钢结构验算用表
        /// </summary>
        public List<string> ComSteel
        {
            get { return _ComSteel; }
        }
        /// <summary>
        /// 混凝土验算用表
        /// </summary>
        public List<string> ComCon
        {
            get { return _ComCon; }
        }
        /// <summary>
        /// 一般组合数据哈希表
        /// </summary>
        public Hashtable LoadCombData_G
        {
            get { return _LoadCombData_G; }
        }
        /// <summary>
        /// 钢结构组合数据哈希表
        /// </summary>
        public Hashtable LoadCombData_S
        {
            get { return _LoadCombData_S; }
        }
        /// <summary>
        /// 混凝土组合数据哈希表
        /// </summary>
        public Hashtable LoadCombData_C
        {
            get { return _LoadCombData_C; }
        }
        #endregion
        #region 构造函数
        public BLoadCombTable()
        {
            _ComGen = new List<string>();
            _ComSteel = new List<string>();
            _ComCon = new List<string>();
            _LoadCombData_G = new Hashtable();
            _LoadCombData_S = new Hashtable();
            _LoadCombData_C = new Hashtable();
        }
        #endregion
        #region 方法
        /// <summary>
        /// 添加荷载组合数据入表
        /// 如果包含组合则更新
        /// </summary>
        /// <param name="com"></param>
        public void  Add(BLoadComb com)
        {
            if (this.ContainsKey(com.KIND, com.NAME))//如果组合表中含有组合名，则删除
            {
                this.Remove(com.NAME, com.KIND);
            }
            //记录原始组合顺序
            switch (com.KIND)
            {
                case LCKind.GEN:
                    _ComGen.Add(com.NAME);
                    _LoadCombData_G.Add(com.NAME, com);
                    break; 
                case LCKind.STEEL: 
                    _ComSteel.Add(com.NAME);
                    _LoadCombData_S.Add(com.NAME, com);
                    break;
                case LCKind.CONC:
                    _ComCon.Add(com.NAME);
                    _LoadCombData_C.Add(com.NAME, com);
                    break;
                default: break;
            }
        }
        /// <summary>
        /// 添加荷载组合数据入表
        /// 强制添加，组合名将自动修改
        /// </summary>
        /// <param name="com"></param>
        public void AddEnforce(BLoadComb com)
        {
            int Count = this.getCount(com.KIND);
            com.NAME = string.Format("LC{0}", Count + 1);
            //记录原始组合顺序
            switch (com.KIND)
            {
                case LCKind.GEN:
                    _ComGen.Add(com.NAME);
                    _LoadCombData_G.Add(com.NAME, com);
                    break;
                case LCKind.STEEL:
                    _ComSteel.Add(com.NAME);
                    _LoadCombData_S.Add(com.NAME, com);
                    break;
                case LCKind.CONC:
                    _ComCon.Add(com.NAME);
                    _LoadCombData_C.Add(com.NAME, com);
                    break;
                default: break;
            }
        }
        /// <summary>
        /// 移除指定荷载组合
        /// </summary>
        /// <param name="comName"></param>
        public void Remove(string comName,LCKind kind)
        {
            //如果不包含此组合
            if (!this.ContainsKey(kind,comName))
                return;
            else
            {
                switch (kind)
                {
                    case LCKind.GEN:
                        _ComGen.Remove(comName);
                        _LoadCombData_G.Remove(comName);
                        break;
                    case LCKind.STEEL:
                        _ComSteel.Remove(comName);
                        _LoadCombData_S.Remove(comName);
                        break;
                    case LCKind.CONC:
                        _ComCon.Remove(comName);
                        _LoadCombData_C.Remove(comName);
                        break;
                    default: break;
                }
            }
        }

        /// <summary>
        /// 设置组合激活状态
        /// </summary>
        /// <param name="kind">给合类型</param>
        /// <param name="Name">组合名</param>
        /// <param name="isActive">是否激活</param>
        public void setActive(LCKind kind, string Name,bool isActive)
        {
            //如果不包含此组合
            if (!this.ContainsKey(kind, Name))
                return;
            else
            {
                switch (kind)
                {
                    case LCKind.GEN:
                        (_LoadCombData_G[Name] as BLoadComb).bACTIVE=isActive;
                        break;
                    case LCKind.STEEL:
                        (_LoadCombData_S[Name] as BLoadComb).bACTIVE = isActive;
                        break;
                    case LCKind.CONC:
                        (_LoadCombData_C[Name] as BLoadComb).bACTIVE = isActive;
                        break;
                    default: break;
                }
            }
        }
        /// <summary>
        /// 查找组合表是否含有某个组合
        /// </summary>
        /// <param name="Key">组合关键字</param>
        /// <returns>是或否</returns>
        public bool ContainsKey(LCKind kind,string Key)
        {
            switch (kind)
            {
                case LCKind.GEN:return _LoadCombData_G.ContainsKey(Key);
                case LCKind.STEEL:
                    return _LoadCombData_S.ContainsKey(Key);
                case LCKind.CONC:
                    return _LoadCombData_C.ContainsKey(Key);
                default:
                    return false;
            }
        }
        /// <summary>
        /// 按组合名取得组合数据
        /// </summary>
        /// <param name="kind">组合类型</param>
        /// <param name="Name">组合名</param>
        /// <returns>荷载组合对象</returns>
        public BLoadComb getLoadComb(LCKind kind, string Name)
        {
            switch (kind)
            {
                case LCKind.GEN: 
                    return _LoadCombData_G[Name] as BLoadComb;
                case LCKind.STEEL:
                    return _LoadCombData_S[Name] as BLoadComb;
                case LCKind.CONC:
                    return _LoadCombData_C[Name] as BLoadComb;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 取得当前表中组合数量
        /// </summary>
        /// <param name="kind">组合类型</param>
        /// <returns>数</returns>
        public int getCount(LCKind kind)
        {
            switch (kind)
            {
                case LCKind.GEN:
                    return _LoadCombData_G.Count;
                case LCKind.STEEL:
                    return _LoadCombData_S.Count;
                case LCKind.CONC:
                    return _LoadCombData_C.Count;
                default:
                    return 0;
            }
        }
        #endregion
    }
    /// <summary>
    /// 荷载类
    /// </summary>
    [Serializable]
    public abstract class Load
    {
        protected string group;//组名
        protected string lc;//工况
        /// <summary>
        /// 荷载组名
        /// </summary>
        public abstract string Group
        {
            get;
            set;
        }
        /// <summary>
        /// 荷载工况
        /// </summary>
        public abstract string LC
        {
            get;
            set;
        }

    }

    /// <summary>
    /// 节点荷载类
    /// </summary>
    [Serializable]
    public class BNLoad : Load
    {
        private int node;//节点号
        private double fx, fy, fz, mx, my, mz;
        #region 属性
        /// <summary>
        /// 节点号
        /// </summary>
        public int iNode
        {
            get { return node; }
        }
        /// <summary>
        /// 沿x方向的力
        /// </summary>
        public double FX
        {
            get { return fx; }
            set { fx = value; }
        }
        /// <summary>
        /// 沿y方向的力
        /// </summary>
        public double FY
        {
            get { return fy; }
            set { fy = value; }
        }
        /// <summary>
        /// 沿z方向的力
        /// </summary>
        public double FZ
        {
            get { return fz; }
            set { fz = value; }
        }
        /// <summary>
        /// x向弯矩
        /// </summary>
        public double MX
        {
            get { return mx; }
            set { mx = value; }
        }
        /// <summary>
        /// y向弯矩
        /// </summary>
        public double MY
        {
            get { return my; }
            set { my = value; }
        }
        /// <summary>
        /// z向弯矩
        /// </summary>
        public double MZ
        {
            get { return mz; }
            set { mz = value; }
        }

        /// <summary>
        /// 重载抽象方法：Group
        /// </summary>
        public override string Group
        {
            get
            {
                return group;
                //throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                group = value;
                //throw new Exception("The method or operation is not implemented.");
            }
        }

        public override string LC
        {
            get
            {
                return lc;
                //throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                lc = value;
                //throw new Exception("The method or operation is not implemented.");
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数：初始荷载值全为0
        /// </summary>
        /// <param name="n">节点号</param>
        public BNLoad(int n)
        {
            node = n;
            fx = 0;
            fy = 0;
            fz = 0;
            mx = 0;
            my = 0;
            mz = 0;
            group = null;
            lc = null;
        }
        #endregion

        #region 方法
        /// <summary>
        /// 对节点荷载进行比例放大
        /// </summary>
        /// <param name="factor">系数</param>
        public void Magnify(double factor)
        {
            fx = fx * factor;
            fy = fy * factor;
            fz = fz * factor;
            mx = mx * factor;
            my = my * factor;
            mz = mz * factor;
        }
        /// <summary>
        /// 与另一个节点荷载相加
        /// </summary>
        /// <param name="NL">另一个节点荷载</param>
        public void plus (BNLoad NL)
        {
            if (this.LC == NL.LC&&this.iNode==NL.iNode)
            { 
                fx = fx + NL.fx;
                fy = fy + NL.fy;
                fz = fz + NL.fz;
                mx = mx + NL.mx;
                my = my + NL.my;
                mz = mz + NL.mz;
            }
        }
        /// <summary>
        /// 指定节点荷载值
        /// </summary>
        /// <param name="fx">Fx</param>
        /// <param name="fy">Fy</param>
        /// <param name="fz">Fz</param>
        /// <param name="mx">Mx</param>
        /// <param name="my">My</param>
        /// <param name="mz">Mz</param>
        public void SetLoadValue(double nfx, double nfy, double nfz, double nmx, double nmy, double nmz)
        {
            fx = nfx; fy = nfy; fz = nfz;
            mx = nmx; my = nmy; mz = nmz;
        }
        #endregion
    }

    /// <summary>
    /// 自重荷载类
    /// </summary>
    [Serializable]
    public class BWeight : Load
    {
        private double gx, gy, gz;
        /// <summary>
        /// 重力加速度常数g=9.805
        /// </summary>
        private const double g = 9.805;//重力加速度值
        /// <summary>
        /// 重力加速度x方向因子
        /// </summary>
        public double Gx
        {
            get
            {
                return gx;
            }
            set
            {
                gx = value;
            }
        }

        /// <summary>
        /// 重力加速度y方向因子
        /// </summary>
        public double Gy
        {
            get
            {
                return gy;
            }
            set
            {
                gy = value;
            }
        }

        /// <summary>
        /// 重力加速度z方向因子
        /// </summary>
        public double Gz
        {
            get
            {
                return gz;
            }
            set
            {
                gz = value;
            }
        }


        /// <summary>
        /// 重力加速度x方向值
        /// </summary>
        public double ACELx
        {
            get
            {
                return gx * g;
            }
            set
            {
                gx = value / g;
            }
        }

        /// <summary>
        /// 重力加速度y方向值
        /// </summary>
        public double ACELy
        {
            get
            {
                return gy * g;
            }
            set
            {
                gy = value / g;
            }
        }

        /// <summary>
        /// 重力加速度z方向值
        /// </summary>
        public double ACELz
        {
            get
            {
                return gz * g;
            }
            set
            {
                gz = value / g;
            }
        }
        /// <summary>
        /// 重载抽象方法：Group
        /// </summary>
        public override string Group
        {
            get
            {
                return group;
            }
            set
            {
                group = value;
            }
        }

        /// <summary>
        /// 重载抽象方法：LC
        /// </summary>
        public override string LC
        {
            get
            {
                return lc;
            }
            set
            {
                lc = value;
            }
        }
    }

    /// <summary>
    /// 梁单元荷载类型
    /// </summary>
    public enum BeamLoadType
    {
        /// <summary>
        /// Uniform Loads 均布荷载
        /// </summary>
        UNILOAD,
        /// <summary>
        /// Concentrated Forces 集中力
        /// </summary>
        CONLOAD,
        /// <summary>
        /// Concentrated Moments 集中弯矩
        /// </summary>
        CONMOMENT,
        /// <summary>
        /// Uniform Moments/Torsions 均布弯矩或扭矩
        /// </summary>
        UNIMOMENT
    }

    /// <summary>
    /// 梁单元荷载类
    /// </summary>
    [Serializable]
    public class BBLoad : Load
    {
        private int elem_num;//单元号
        private string cmd;
        private BeamLoadType _type;
        private DIR dir;//加载方向
        private bool bproj;//是否投影

        private bool beccen;//是否偏心
        private DIR eccdir;//偏心方向
        private double i_end, j_end;//i端和j端的偏心荷载值
        private bool bj_end;//是否具有j端偏心荷载

        private double d1, p1, d2, p2, d3, p3, d4, p4;//荷载数据组
        #region 属性
        /// <summary>
        /// 单元编号
        /// </summary>
        public int ELEM_num
        {
            get { return elem_num; }
            set { elem_num = value; }
        }

        /// <summary>
        /// BEAM代表梁单元荷载，TYPITAL代表标准梁单元荷载
        /// </summary>
        public string CMD
        {
            get { return cmd; }
            set { cmd = value; }
        }

        /// <summary>
        /// 梁单元荷载类型
        /// </summary>
        public BeamLoadType TYPE
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// 加载方向
        /// </summary>
        public DIR Dir
        {
            get { return dir; }
            set { dir = value; }
        }

        /// <summary>
        /// 荷载是否投影
        /// </summary>
        public bool bPROJ
        {
            get { return bproj; }
            set { bproj = value; }
        }

        /// <summary>
        /// 荷载是否偏心
        /// </summary>
        public bool bECCEN
        {
            get { return beccen; }
            set { beccen = value; }
        }

        /// <summary>
        /// 偏心荷载方向
        /// </summary>
        public DIR EccDir
        {
            get { return eccdir; }
            set { eccdir = value; }
        }

        /// <summary>
        /// 荷载组名
        /// </summary>
        public override string Group
        {
            get
            {
                return group;
            }
            set
            {
                group = value;
            }
        }

        /// <summary>
        /// 荷载工况名
        /// </summary>
        public override string LC
        {
            get
            {
                return lc;
            }
            set
            {
                lc = value;
            }
        }

        /// <summary>
        /// 是否为普通均布荷载：只有两个位置点，且线荷幅值相同
        /// </summary>
        public bool bIsOrdinaryUNILOAD
        {
            get
            {
                if (_type != BeamLoadType.UNILOAD)
                    return false;
                else if (d3 != 0 || d4 != 0 || p3 != 0 || p4 != 0)
                    return false;
                else if (p1 != p2)
                    return false;
                else
                    return true;
            }
        }
        /// <summary>
        /// 是否为三角形均布荷载:有三个位置点
        /// </summary>
        public bool bIsTriangleUNILOAD
        {
            get
            {
                if (_type != BeamLoadType.UNILOAD)
                    return false;
                else if (d4 != 0 || p4 != 0)
                    return false;
                else if (p1 != 0||p3!=0||p2==0)
                    return false;
                else
                    return true;
            }
        }
        /// <summary>
        /// 是否为梯形均布荷载:四个位置点，中间两个幅值相等
        /// </summary>
        public bool bIsTrapezoidalUNILOAD
        {
            get
            {
                if (_type != BeamLoadType.UNILOAD)
                    return false;
                else if (p1 != 0 || p4 != 0||d4==0)
                    return false;
                else if (p2!=p3)
                    return false;
                else
                    return true;
            }
        }

        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public BBLoad()
        {
            elem_num = 0;
            beccen = false;//不偏心
            bproj = false;//不投影
            cmd = "BEAM";
            _type = BeamLoadType.UNILOAD;
            beccen = false;
            dir = DIR.GX;
            bj_end = false;
        }
        #endregion

        #region 方法
        /// <summary>
        /// 提取偏心单元荷载信息
        /// </summary>
        /// <param name="dataline">mgt文件梁单元荷载数据行</param>
        public void readEccenDataMgt(string dataline)
        {
            string[] temp = dataline.Split(new char[] { ',' },
                        StringSplitOptions.RemoveEmptyEntries);
            string bEccen = temp[5].Trim();
            string Ecc_Dir = temp[6].Trim();

            if (bEccen == "YES")
            {
                bool bb = false;
                if (temp[9].Trim() == "YES")
                    bb = true;

                beccen = true;
                setEccenDir(Ecc_Dir);//设置偏心方向
                i_end = double.Parse(temp[7].Trim());
                j_end = double.Parse(temp[8].Trim());
                bj_end = bb;
            }
            else if (bEccen == "NO")
            {
                beccen = false;
                eccdir = DIR.GX;
                i_end = 0;
                j_end = 0;
                bj_end = false;
            }
        }
        /// <summary>
        /// 更新偏心荷载数据
        /// </summary>
        /// <param name="bEccen">是否偏心:"YES"/"NO"</param>
        /// <param name="Ecc_Dir">偏心方向</param>
        /// <param name="iData">i端偏心距</param>
        /// <param name="jData">j端偏心距</param>
        /// <param name="bJ_End">i端和j端偏心是否相同:"YES"/"NO"</param>
        public void updataEccenData(string bEccen, string Ecc_Dir, double iData,
            double jData, string bJ_End)
        {
            bool bb = false;
            if (bJ_End == "YES")
                bb = true;

            switch (bEccen)
            {
                case "NO":
                    beccen = false;
                    eccdir = DIR.GX;
                    i_end = 0;
                    j_end = 0;
                    bj_end = false;
                    return;
                case "YES":
                    beccen = true;
                    setEccenDir(Ecc_Dir);//设置偏心方向
                    i_end = iData;
                    j_end = jData;
                    bj_end = bb;
                    return;
                default:
                    beccen = false;
                    eccdir = DIR.GX;
                    i_end = 0;
                    j_end = 0;
                    bj_end = false;
                    return;
            }
        }

        /// <summary>
        /// 设置梁单元荷载信息数据
        /// </summary>
        /// <param name="dd1">位置1</param>
        /// <param name="pp1">荷载1</param>
        /// <param name="dd2">位置2</param>
        /// <param name="pp2">荷载2</param>
        /// <param name="dd3">位置3</param>
        /// <param name="pp3">荷载3</param>
        /// <param name="dd4">位置4</param>
        /// <param name="pp4">荷载4</param>
        public void setLoadData(double dd1, double pp1, double dd2, double pp2,
            double dd3, double pp3, double dd4, double pp4)
        {
            d1 = dd1;
            p1 = pp1;
            d2 = dd2;
            p2 = pp2;
            d3 = dd3;
            p3 = pp3;
            d4 = dd4;
            p4 = pp4;
        }

        /// <summary>
        /// 设置单元荷载方向
        /// </summary>
        /// <param name="direction">荷载方向字符串：GX/GY/GZ/LX/LY/LZ</param>
        public void setLoadDir(string direction)
        {
            direction = direction.ToUpper();
            switch (direction)
            {
                case "GX":
                    dir = DIR.GX;
                    return;
                case "GY":
                    dir = DIR.GY;
                    return;
                case "GZ":
                    dir = DIR.GZ;
                    return;
                case "LX":
                    dir = DIR.LX;
                    return;
                case "LY":
                    dir = DIR.LY;
                    return;
                case "LZ":
                    dir = DIR.LZ;
                    return;
                default:
                    dir = DIR.GX;
                    return;
            }
        }

        /// <summary>
        /// 设置偏心方向
        /// </summary>
        /// <param name="direction">偏心方向字符串：GX/GY/GZ/LX/LY/LZ</param>
        public void setEccenDir(string direction)
        {
            direction = direction.ToUpper();
            switch (direction)
            {
                case "GX":
                    eccdir = DIR.GX;
                    return;
                case "GY":
                    eccdir = DIR.GY;
                    return;
                case "GZ":
                    eccdir = DIR.GZ;
                    return;
                case "LX":
                    eccdir = DIR.LX;
                    return;
                case "LY":
                    eccdir = DIR.LY;
                    return;
                case "LZ":
                    eccdir = DIR.LZ;
                    return;
                default:
                    eccdir = DIR.GX;
                    return;
            }
        }

        /// <summary>
        /// 获取梁单元荷载数据的位置值
        /// </summary>
        /// <param name="i">位置编号：1,2,3,4</param>
        /// <returns>位置值：0~1之间的数值</returns>
        public double getD(int i)
        {
            double res = 0;
            switch (i)
            {
                case 1:
                    res = d1;
                    break;
                case 2:
                    res = d2;
                    break;
                case 3:
                    res = d3;
                    break;
                case 4:
                    res = d4;
                    break;
                default:
                    res = 0;
                    break;
            }
            return res;
        }

        /// <summary>
        /// 获取梁单元荷载数据的荷载值
        /// </summary>
        /// <param name="i">荷载值编号</param>
        /// <returns>荷载值</returns>
        public double getP(int i)
        {
            double res = 0;
            switch (i)
            {
                case 1:
                    res = p1;
                    break;
                case 2:
                    res = p2;
                    break;
                case 3:
                    res = p3;
                    break;
                case 4:
                    res = p4;
                    break;
                default:
                    res = 0;
                    break;
            }
            return res;
        }
        #endregion
    }

    /// <summary>
    /// 单元温度荷载类
    /// </summary>
    [Serializable]
    public class BETLoad : Load
    {
        private int _elem_num;//单元号
        private double _Temp;//单元温度

        #region 属性
        /// <summary>
        /// 单元号
        /// </summary>
        public int Elem_Num
        {
            get { return _elem_num;}
            set { _elem_num = value; }
        }

        /// <summary>
        /// 温度荷载
        /// </summary>
        public double Temp
        {
            get { return _Temp; }
            set { _Temp = value; }
        }
        /// <summary>
        /// 荷载分组
        /// </summary>
        public override string Group
        {
            get
            {
                return group;
            }
            set
            {
                group = value;
            }
        }
        /// <summary>
        /// 荷载工况
        /// </summary>
        public override string LC
        {
            get
            {
                return lc;
            }
            set
            {
                lc = value;
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public BETLoad()
        {
            _elem_num = 0;
            _Temp = 0;
        }
        #endregion
    }

    /// <summary>
    /// 荷载表类
    /// </summary>
    [Serializable]
    public class BLoadTable
    {
        #region 数据成员
        private Hashtable _NLoadData;//节点荷载数据
        private Hashtable _BeamLoadData;//单元荷载数据
        #endregion

        #region 属性
        /// <summary>
        /// 节点荷载数据表
        /// </summary>
        public Hashtable NLoadData
        {
            get { return _NLoadData; }
        }
        /// <summary>
        /// 单元荷载数据表
        /// </summary>
        public Hashtable BLoadData
        {
            get { return _BeamLoadData; }
        }
        /// <summary>
        /// 具有节点荷载的节点号列表
        /// </summary>
        public List<int> NodeListForNLoad
        {
            get
            {
                List<int> Res = new List<int>();
                foreach (DictionaryEntry de in _NLoadData)
                {
                    SortedList<int, BNLoad> ND = de.Value as SortedList<int, BNLoad>;
                    foreach (KeyValuePair<int, BNLoad> kvp in ND)
                    {
                        if (Res.Contains(kvp.Key)==false)
                            Res.Add(kvp.Key);
                    }
                }

                return Res;
            }
        }
        /// <summary>
        /// 有荷载数据的工况列表
        /// </summary>
        public List<string> LCList
        {
            get
            {
                List<string> Res = new List<string>();
                foreach (DictionaryEntry de in _NLoadData)
                {
                    string CurLC = de.Key.ToString();//工况名
                    if (Res.Contains(CurLC))
                        continue;
                    else
                        Res.Add(CurLC);
                }
                foreach (DictionaryEntry de in _BeamLoadData)
                {
                    string CurLC = de.Key.ToString();//工况名
                    if (Res.Contains(CurLC))
                        continue;
                    else
                        Res.Add(CurLC);
                }
                return Res;
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数初始化
        /// </summary>
        public BLoadTable()
        {
            _NLoadData = new Hashtable();
            _BeamLoadData = new Hashtable();
        }
        #endregion
        #region 方法
        /// <summary>
        /// 按旧数据更新荷载表中的节点荷载
        /// </summary>
        /// <param name="LCs">工况列表</param>
        /// <param name="NLoadData">旧版节点荷载数据</param>
        public void  UpdateNodeLoadList(List<BLoadCase> LCs,SortedList<int,BNLoad> NLoadData)
        {
            foreach (BLoadCase lc in LCs)
            {
                //当前组合的节点荷载表
                SortedList<int, BNLoad> CurNLoadData = new SortedList<int,BNLoad> ();

                foreach (KeyValuePair<int, BNLoad> NLoad in NLoadData)
                {
                    if (NLoad.Value.LC == lc.LCName)
                    {
                        CurNLoadData.Add(NLoad.Key, NLoad.Value);
                    }
                }
                //添加当前组合节点表入总表
                if (CurNLoadData.Count>0)
                    _NLoadData.Add(lc.LCName, CurNLoadData);                
            }
        }
        /// <summary>
        /// 按旧数据更新荷载表中的单元荷载
        /// </summary>
        /// <param name="LCs">工况列表</param>
        /// <param name="ELoadData">旧版单元荷载数据</param>
        public void UpdateElemLoadList(List<BLoadCase> LCs,SortedList<int,BBLoad> ELoadData)
        {
            foreach (BLoadCase lc in LCs)
            {
                //当前组合的单元荷载表
                //[2011.03.25]将CurELoadData修改为可重复键值
                SortedList<int, BBLoad> CurELoadData = new SortedList<int, BBLoad>(new RepeatedKeySort());

                foreach (KeyValuePair<int, BBLoad> ELoad in ELoadData)
                {
                    if (ELoad.Value.LC == lc.LCName)
                    {
                        CurELoadData.Add(ELoad.Key, ELoad.Value);
                    }
                }
                //添加当前组合节点表入总表
                if (CurELoadData.Count > 0)
                    _BeamLoadData.Add(lc.LCName, CurELoadData);
            }
        }

        /// <summary>
        /// 按比例系数修改节点荷载
        /// </summary>
        /// <param name="node">节点号</param>
        /// <param name="LC_Name">工况名</param>
        /// <param name="factor">比例系数</param>
        public void ModifyNodeLoad(int node, string LC_Name, double factor)
        {
            //如果没有此工况请返回
            if (_NLoadData.ContainsKey(LC_Name) == false)
                return;
            SortedList NLoadData = _NLoadData[LC_Name] as SortedList;
            if (NLoadData.ContainsKey(node) == false)
                return;
            else
            {
                BNLoad NL = NLoadData[node] as BNLoad;
                NL.Magnify(factor);
            }
        }
        /// <summary>
        /// 添加梁单元荷载入库
        /// 2011.08.23
        /// </summary>
        /// <param name="beamload">梁单元荷载</param>
        public void AddElemLoad(BBLoad beamload)
        {
            //如果包含荷载工况
            if (this._BeamLoadData.ContainsKey(beamload.LC))
            {
                SortedList<int, BBLoad> loadTable = this._BeamLoadData[beamload.LC]
                    as SortedList<int, BBLoad>;
                loadTable.Add(beamload.ELEM_num, beamload);
                
            }
            else
            {
                SortedList<int, BBLoad> loadTable = new SortedList<int, BBLoad>(new RepeatedKeySort());
                loadTable.Add(beamload.ELEM_num, beamload);
                _BeamLoadData.Add(beamload.LC, loadTable);//添加新的组合表
            }
        }
        /// <summary>
        /// 添加节点荷载入库
        /// </summary>
        /// <param name="nodeload">节点荷载</param>
        public void AddNodeLoad(BNLoad nodeload)
        {
            //如果包含荷载工况
            if (this._NLoadData.ContainsKey(nodeload.LC))
            {
                SortedList loadTable = this._NLoadData[nodeload.LC]
                    as SortedList;
                //如果节点号有重复
                if (loadTable.ContainsKey(nodeload.iNode))
                {
                    BNLoad nld = loadTable[nodeload.iNode] as BNLoad;
                    nld.plus(nodeload);//荷载叠加
                    loadTable[nodeload.iNode] = nld;
                }
                else
                    loadTable.Add(nodeload.iNode, nodeload);

            }
            else
            {
                SortedList loadTable = new SortedList();
                loadTable.Add(nodeload.iNode, nodeload);
                this._NLoadData.Add(nodeload.LC, loadTable);//添加新的组合表
            }
        }
        /// <summary>
        /// 查询荷载工况名称对应的工况表索引
        /// </summary>
        /// <param name="LcName">工况名</param>
        /// <returns>索引号，从0开始，以LCList属性返回的列表顺序为准</returns>
        public int IndexOf(string LcName)
        {
            List<string> lcl = this.LCList;
            if (lcl.Contains(LcName))
            {
                return lcl.IndexOf(LcName);
            }
            else
                return -1;
        }
        #endregion
    }
    #endregion

    #region Geometry Model Class(几何模型类)
    /// <summary>
    /// 定义存储文件信息的节点类：Bnodes
    /// </summary>
    [Serializable]
    public class Bnodes : Object
    {
        /// <summary>
        /// 节点编号
        /// </summary>
        public int num;
        /// <summary>
        /// 节点X坐标
        /// </summary>
        public double X;
        /// <summary>
        /// 节点Y坐标
        /// </summary>
        public double Y;
        /// <summary>
        /// 节点Z坐标
        /// </summary>
        public double Z;

        /// <summary>
        /// 节点坐标位置
        /// </summary>
        public Point3d Location
        {
            get
            {
                Point3d res = new Point3d(X, Y, Z);
                return res;
            }
        }
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="n">节点编号</param>
        public Bnodes(int n)
        {
            num = n;
            X = 0;
            Y = 0;
            Z = 0;
        }
        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="n">节点编号</param>
        /// <param name="nx">节点X坐标</param>
        /// <param name="ny">节点Y坐标</param>
        /// <param name="nz">节点Z坐标</param>
        public Bnodes(int n, double nx, double ny, double nz)
        {
            num = n;
            X = nx;
            Y = ny;
            Z = nz;
        }

        #region 方法函数
        //重载ToString函数
        new public string ToString()
        {
            return ("(" + num.ToString() + "," + X.ToString() + "," + Y.ToString() + "," + Z.ToString() + ")");
        }

        /// <summary>
        /// 求两节点间的矩离
        /// </summary>
        /// <param name="nodeNext">下一个节点</param>
        /// <returns>返回距离值</returns>
        public double DistanceTo(Bnodes nodeNext)
        {
            double res = Math.Sqrt(Math.Pow((nodeNext.X - this.X), 2) +
                Math.Pow((nodeNext.Y - this.Y), 2) +
                Math.Pow((nodeNext.Z - this.Z), 2));
            return res;
        }

        /// <summary>
        /// 求到另一节点的方向向量
        /// </summary>
        /// <param name="nodeto">到的节点</param>
        /// <returns>单位方向向量</returns>
        public Vector3 VectorTo(Bnodes nodeto)
        {
            Vector3 v1 = new Vector3(this.X, this.Y, this.Z);
            Vector3 v2 = new Vector3(nodeto.X, nodeto.Y, nodeto.Z);
            Vector3 Res = v2 - v1;//矢量相减即得方向向量
            Res.Normalize();//归一化
            return Res;
        }
        #endregion
        
    }

    /// <summary>
    /// 单元基类
    /// </summary>
    [Serializable]
    public abstract class Element : Object
    {
        private int _iEL, _iMAT, _iPRO;
        private ElemType _TYPE;
        private CoordinateSystem _ECS;//单元坐标系
        /// <summary>
        /// 单元节点号数组
        /// </summary>
        public List<int> iNs;
        #region 属性
        /// <summary>
        /// 单元编号
        /// </summary>
        public int iEL
        {
            get { return _iEL; }
            set { _iEL = value; }
        }
        /// <summary>
        /// 单元类型
        /// </summary>
        public ElemType TYPE
        {
            get { return _TYPE; }
            set { _TYPE = value; }
        }
        /// <summary>
        /// 单元材料号
        /// </summary>
        public int iMAT
        {
            get { return _iMAT; }
            set { _iMAT = value; }
        }
        /// <summary>
        /// 单元特性值号，即截面号
        /// </summary>
        public int iPRO
        {
            get { return _iPRO; }
            set { _iPRO = value; }
        }
        /// <summary>
        /// 单元局部坐标系
        /// </summary>
        public CoordinateSystem ECS
        {
            get { return _ECS; }
            set { _ECS = value; }
        }

        /// <summary>
        /// 节点数
        /// </summary>
        public int NodeCount
        {
            get { return iNs.Count; }
        }
        #endregion
        
        /// <summary>
        ///构造函数 
        /// </summary>
        public Element()
        {
            iEL = 0;
            TYPE = ElemType.NOTYPE;
            iMAT = 0;
            iPRO = 0;
            iNs = new List<int>();
            _ECS = new CoordinateSystem();
        }
        /// <summary>
        /// 构造函数重载
        /// </summary>
        /// <param name="num">单元编号</param>
        /// <param name="type">单元类型</param>
        /// <param name="mat">单元材料号</param>
        /// <param name="pro">单元特性值号，或截面号</param>
        /// <param name="iNodes">节点号数组</param>
        public Element(int num, ElemType type, int mat, int pro, params int[] iNodes)
        {
            iEL = num;
            TYPE = type;
            iMAT = mat;
            iPRO = pro;
            if (iNodes.Length < 2)
                return;
            else
            {
                iNs = new List<int>(iNodes.Length);
                foreach (int node in iNodes)
                {
                    iNs.Add(node);
                }
            }
            _ECS = new CoordinateSystem();
        }
        //方法

        public string NodeString()
        {
            string temp = iEL.ToString();
            foreach (object num in iNs)
            {
                temp += ",";
                temp += num.ToString();
            }
            return temp;
        }
    }

    /// <summary>
    /// 梁单元类
    /// </summary>
    [Serializable]
    public class FrameElement : Element
    {
        private double Angle;
        private int _iSUB;
        private double _EXVAL;
        private DesignParameters _DPs;
        /// <summary>
        /// 其它参数3
        /// </summary>
        public int iOPT;

        #region 属性
        /// <summary>
        /// 钢结构梁单元的设计参数
        /// </summary>
        public DesignParameters DPs
        {
            get { return _DPs; }
            set { _DPs = value; }
        }

        /// <summary>
        /// 梁单元方向角（beta角）
        /// </summary>
        public double beta
        {
            get
            {
                return Angle;
            }
            set
            {
                Angle = value;
            }
        }

        /// <summary>
        /// 子类型（BEAM和TRUSS无关）
        /// </summary>
        public int iSUB
        {
            set { _iSUB = value; }
            get { return _iSUB; }
        }
        /// <summary>
        /// 单元另行输入的数据（BEAM和TRUSS无关）
        /// </summary>
        public double EXVAL
        {
            set { _EXVAL = value; }
            get { return _EXVAL; }
        }

        /// <summary>
        /// 单元节点号i
        /// </summary>
        public int I
        {
            get 
            {
                return iNs[0]; 
            }
        }

        /// <summary>
        /// 单元节点号j
        /// </summary>
        public int J
        {
            get
            {
                return iNs[1];
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 不带参数的构造函数,beta=0;type="BEAM"
        /// </summary>
        public FrameElement()
            : base()
        {
            this.TYPE = ElemType.BEAM;
            this.beta = 0;
            this._DPs = new DesignParameters();
        }
        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="num">单元号</param>
        /// <param name="type">单元类型</param>
        /// <param name="mat">材料号</param>
        /// <param name="pro">截面特性号</param>
        /// <param name="iNodes">节点号数组</param>
        public FrameElement(int num, ElemType type, int mat, int pro, params int[] iNodes)
            : base(num, type, mat, pro, iNodes)
        {
            //调用基类的构造函数
            this._DPs = new DesignParameters();
        }
        #endregion

        #region 单元方法
        #endregion
    }

    /// <summary>
    /// 平面单元类
    /// </summary>
    [Serializable]
    public class PlanarElement : Element
    {
        private int _iSUB;//面单元子项号
        private int _iWID;//墙号

        /// <summary>
        /// 厚度截面号1
        /// </summary>
        public int iSUB
        {
            get { return _iSUB; }
            set { _iSUB = value; }
        }

        /// <summary>
        /// 墙号
        /// </summary>
        public int iWID
        {
            get { return _iWID; }
            set { _iWID = value; }
        }

        /// <summary>
        /// 不带参数的构造函数
        /// </summary>
        public PlanarElement()
            : base()
        {
            this.TYPE = ElemType.PLATE;
        }

        /// <summary>
        /// 调用基类的构造函数
        /// </summary>
        /// <param name="num">单元号</param>
        /// <param name="type">单元类型</param>
        /// <param name="mat">材料号</param>
        /// <param name="pro">截面号，厚度号</param>
        /// <param name="iNodes">节点号数组</param>
        public PlanarElement(int num, ElemType type, int mat, int pro, params int[] iNodes)
            : base(num, type, mat, pro, iNodes)
        {
            //调用基类的对应构造函数
        }
    }

    /// <summary>
    /// 存储截面特性的基类
    /// </summary>
    [Serializable]
    public abstract class BSections
    {
        /// <summary>
        /// 截面号
        /// </summary>
        private int iSEC;
        /// <summary>
        /// 截面类型：枚举
        /// </summary>
        public SecType TYPE;
        /// <summary>
        /// 截面名称
        /// </summary>
        private string SNAME;
        /// <summary>
        /// 截面偏移数据
        /// </summary>
        public ArrayList OFFSET;
        /// <summary>
        /// 是否考虑剪切变形
        /// </summary>
        public bool bsd;
        /// <summary>
        /// 截面形状：B表示箱形
        /// </summary>
        public SecShape SSHAPE;
        /// <summary>
        /// 截面数据信息
        /// </summary>
        public ArrayList SEC_Data;


        #region 存储截面特性值
        protected double _Area;//面积
        protected double _ASy;//单元坐标系y轴方向的有效剪切面积
        protected double _ASz;//单元坐标系z轴方向的有效剪切面积
        protected double _Ixx;//截面扭转贯性矩
        /// <summary>
        /// 截面扭转贯性矩
        /// </summary>
        public double Ixx
        {
            get { return _Ixx; }
            set { _Ixx = value; }
        }
        protected double _Iyy;//单元绕y轴的截面贯性矩
        /// <summary>
        /// 单元绕y轴的截面贯性矩
        /// </summary>
        public  double Iyy
        {
            get { return _Iyy; }
            set { _Iyy = value; }
        }
        protected double _Izz;//单元绕z轴的截面贯性矩
        /// <summary>
        /// 单元绕z轴的截面贯性矩
        /// </summary>
        public double Izz
        {
            get { return _Izz; }
            set { _Izz = value; }
        }

        private double _Iw;
        /// <summary>
        /// 毛截面扇性惯性矩
        /// </summary>
        public double Iw
        {
            get { return _Iw; }
            set { _Iw = value; }
        }

        protected double _CyP;//自中和轴到单元坐标系(+)y方向最外端的距离
        /// <summary>
        /// 自中和轴到单元坐标系(+)y方向最外端的距离
        /// </summary>
        public double CyP
        {
            get { return _CyP; }
            set { _CyP = value; }
        }
        protected double _CyM;//自中和轴到单元坐标系(-)y方向最外端的距离
        /// <summary>
        /// 自中和轴到单元坐标系(-)y方向最外端的距离
        /// </summary>
        public double CyM
        {
            get { return _CyM; }
            set { _CyM = value; }
        }
        protected double _CzP;//自中和轴到单元坐标系(+)z方向最外端的距离
        /// <summary>
        /// 自中和轴到单元坐标系(+)z方向最外端的距离
        /// </summary>
        public double CzP
        {
            get { return _CzP; }
            set { _CzP = value; }
        }
        protected double _CzM;//自中和轴到单元坐标系(-)z方向最外端的距离
        /// <summary>
        /// 自中和轴到单元坐标系(-)z方向最外端的距离
        /// </summary>
        public double CzM
        {
            get { return _CzM; }
            set { _CzM = value; }
        }
        protected double _QyB;//作用于单元坐标系y轴方向的剪切系数
        protected double _QzB;//作用于单元坐标系z轴方向的剪切系数
        protected double _PERI_OUT;//截面外轮廓周长
        protected double _PERI_IN;//截面内轮廓周长
        private double _Cy;//截面形心y坐标
        /// <summary>
        /// 截面形心y坐标
        /// </summary>
        public double Cy
        {
            get { return _Cy; }
            set { _Cy = value; }
        }
        private double _Cz;//截面形心z坐标
        /// <summary>
        /// 截面形心z坐标
        /// </summary>
        public  double Cz
        {
            get { return _Cz; }
            set { _Cz = value; }
        }
        private double _Sy;//截面剪心y坐标
        /// <summary>
        /// 截面剪心y坐标
        /// </summary>
        public double Sy
        {
            get { return _Sy; }
            set { _Sy = value; }
        }
        private double _Sz;//截面剪心z坐标
        /// <summary>
        /// 截面剪心z坐标
        /// </summary>
        public double Sz
        {
            get { return _Sz; }
            set { _Sz = value; }
        }

        protected double _y1;//四个角点坐标
        //四个角点坐标
        public double Y1
        {
            get { return _y1; }
            set { _y1 = value; }
        }
        protected double _z1;//四个角点坐标
        //四个角点坐标
        public  double Z1
        {
            get { return _z1; }
            set { _z1 = value; }
        }
        protected double _y2;//四个角点坐标
        //四个角点坐标
        public  double Y2
        {
            get { return _y2; }
            set { _y2 = value; }
        }
        protected double _z2;//四个角点坐标
        //四个角点坐标
        public  double Z2
        {
            get { return _z2; }
            set { _z2 = value; }
        }
        protected double _y3;//四个角点坐标
        //四个角点坐标
        public  double Y3
        {
            get { return _y3; }
            set { _y3 = value; }
        }
        protected double _z3;//四个角点坐标
        //四个角点坐标
        public  double Z3
        {
            get { return _z3; }
            set { _z3 = value; }
        }
        protected double _y4;//四个角点坐标
        //四个角点坐标
        public  double Y4
        {
            get { return _y4; }
            set { _y4 = value; }
        }
        protected double _z4;//四个角点坐标
        //四个角点坐标
        public  double Z4
        {
            get { return _z4; }
            set { _z4 = value; }
        }
        #endregion
        /// <summary>
        /// 截面名称属性
        /// </summary>
        public string Name
        {
            get { return SNAME; }
            set { SNAME = value; }
        }
        /// <summary>
        /// 截面编号
        /// </summary>
        public int Num
        {
            get { return iSEC; }
            set { iSEC = value; }
        }
        /// <summary>
        /// 面积特性
        /// </summary>
        public double Area
        {
            get { return _Area; }
            set { _Area = value; }
        }
        /// <summary>
        /// 截面类型
        /// </summary>
        public SecType SecType
        {
            get { return TYPE; }
        }

        /// <summary>
        /// 截面验算点集合（目前内有4个点集）
        /// </summary>
        public Point2dCollection CheckPointCollection
        {
            get
            {
                Point2dCollection ptc = new Point2dCollection();
                ptc.addPt(new Point2d(_y1, _z1));
                ptc.addPt(new Point2d(_y2, _z2));
                ptc.addPt(new Point2d(_y3, _z3));
                ptc.addPt(new Point2d(_y4, _z4));
                return ptc;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BSections()
        {
            iSEC = 1;
            TYPE = SecType.DBUSER;
            SNAME = "No Name";

            OFFSET = new ArrayList(7);
            OFFSET.Add("CC");
            OFFSET.Add(0);
            OFFSET.Add(0);
            OFFSET.Add(0);
            OFFSET.Add(0);
            OFFSET.Add(0);
            OFFSET.Add(0);

            bsd = true;
            SSHAPE = SecShape.P;

            SEC_Data = new ArrayList();
            SEC_Data.Add(1);
            SEC_Data.Add("GB-YB");
            SEC_Data.Add("P 180x10");
        }
        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        public BSections(int id,string Name)
        {
            iSEC = id;
            TYPE = SecType.DBUSER;
            SNAME = Name;

            OFFSET = new ArrayList(7);
            OFFSET.Add("CC");
            OFFSET.Add(0);
            OFFSET.Add(0);
            OFFSET.Add(0);
            OFFSET.Add(0);
            OFFSET.Add(0);
            OFFSET.Add(0);

            bsd = true;
            SSHAPE = SecShape.P;

            SEC_Data = new ArrayList();
            SEC_Data.Add(1);
            SEC_Data.Add("GB-YB");
            SEC_Data.Add("P 180x10");
        }
        /// <summary>
        /// 按一定格式输出截面数据信息
        /// </summary>
        /// <returns>ansys命令流字符串</returns>
        public abstract string WriteData();

        /// <summary>
        /// 重新按数据计算截面特性：面积、贯性矩等
        /// </summary>
        public abstract void  CalculateSecProp();

        /// <summary>
        /// 设置截面常用特性值1
        /// </summary>
        /// <param name="area">面积</param>
        /// <param name="asy">y向有效剪切面积</param>
        /// <param name="asz">z向有效剪切面积</param>
        /// <param name="ixx">扭转贯性矩</param>
        /// <param name="iyy">绕y轴的贯性矩</param>
        /// <param name="izz">绕z轴的贯性矩</param>
        public void setSecProp1(double area, double asy, double asz, double ixx, double iyy, double izz)
        {
            _Area = area; _ASy = asy; _ASz = asz;
            _Ixx = ixx; _Iyy = iyy; _Izz = izz;
        }

        /// <summary>
        /// 设置截面常用特性值2
        /// </summary>
        /// <param name="cyp">自中和轴到单元坐标系(+)y方向最外端的距离</param>
        /// <param name="cym">自中和轴到单元坐标系(-)y方向最外端的距离</param>
        /// <param name="czp">自中和轴到单元坐标系(+)z方向最外端的距离</param>
        /// <param name="czm">自中和轴到单元坐标系(-)z方向最外端的距离</param>
        /// <param name="qyb">作用于单元坐标系y轴方向的剪切系数</param>
        /// <param name="qzb">作用于单元坐标系z轴方向的剪切系数</param>
        /// <param name="p_out">截面外轮廓周长</param>
        /// <param name="p_in">截面内轮廓周长</param>
        /// <param name="cy">截面形心y坐标</param>
        /// <param name="cz">截面形心y坐标</param>
        public void setSecProp2(double cyp, double cym, double czp, double czm, double qyb, double qzb,
            double p_out, double p_in, double cy, double cz)
        {
            _CyP = cyp; _CyM = cym; _CzP = czp; _CzM = czm; _QyB = qyb; _QzB = qzb;
            _PERI_OUT = p_out; _PERI_IN = p_in;
        }

        /// <summary>
        /// 设置截面常用特性值3
        /// </summary>
        /// <param name="y1">左上点y坐标</param>
        /// <param name="z1">左上点z坐标</param>
        /// <param name="y2">右上点y坐标</param>
        /// <param name="z2">右上点z坐标</param>
        /// <param name="y3">左下点y坐标</param>
        /// <param name="z3">左下点z坐标</param>
        /// <param name="y4">右下点y坐标</param>
        /// <param name="z4">右下点z坐标</param>
        public void setSecProp3(double y1, double z1, double y2, double z2, double y3, double z3, double y4, double z4)
        {
            _y1 = y1; _z1 = z1; _y2 = y2; _z2 = z2;
            _y3 = y3; _z3 = z3; _y4 = y4; _z4 = z4;
        }
        /// <summary>
        /// 取得截面的指定验算点
        /// </summary>
        /// <param name="iPt">验算点号，目前只有1~4，共4个点数据</param>
        /// <param name="Y">验算点Y坐标</param>
        /// <param name="Z">验算点Z坐标</param>
        public void getCheckPoint(int iPt, out double Y, out double Z)
        {
            switch (iPt)
            {
                case 1: Y = _y1; Z = _z1; break;
                case 2: Y = _y2; Z = _z2; break;
                case 3: Y = _y3; Z = _z3; break;
                case 4: Y = _y4; Z = _z4; break;
                default: Y = _y1; Z = _z1; break;
            }
        }
    }

    /// <summary>
    /// 常用截面信息类
    /// </summary>
    [Serializable]
    public class SectionDBuser : BSections
    {
        public SectionDBuser():base()
        {
            this.TYPE = SecType.DBUSER;
            //更新截面特性
            CalculateSecProp();
        }

        /// <summary>
        /// 重载输出ansys 命令流函数
        /// </summary>
        /// <returns>ansys命令流</returns>
        public override string WriteData()
        {
            //throw new NotImplementedException();
            string res = null;
            if (this.SSHAPE == SecShape.B && (int)this.SEC_Data[0] == 2)//箱形截面
            {
                res += "sectype," + this.Num.ToString() + ",beam,hrec," + this.Name;
                res += "\nsecdata," + SEC_Data[2].ToString() + "," + SEC_Data[1].ToString() + "," + SEC_Data[3].ToString() + "," + SEC_Data[3].ToString()
                + "," + SEC_Data[6].ToString() + "," + SEC_Data[4].ToString();
            }
            else if (this.SSHAPE== SecShape.H && (int)this.SEC_Data[0] == 2)//H型截面
            {
                res += "sectype," + this.Num.ToString() + ",beam,i," + this.Name;
                res += "\nsecdata," + SEC_Data[2].ToString() + "," + SEC_Data[2].ToString() + "," + SEC_Data[1].ToString() + "," + SEC_Data[4].ToString()
                    + "," + SEC_Data[4].ToString() + "," + SEC_Data[3].ToString();
            }
            else if (this.SSHAPE == SecShape.P && (int)this.SEC_Data[0] == 2)//圆管截面
            {
                double ri = (double)SEC_Data[1] / 2 - (double)SEC_Data[2];
                double ro = (double)SEC_Data[1] / 2;
                res += "sectype," + this.Num.ToString() + ",beam,ctube," + this.Name;
                res += "\nsecdata," + ri.ToString() + "," + ro.ToString();
            }
            else if (this.SSHAPE==SecShape .SB&& (int)this.SEC_Data[0] == 2)//矩形截面
            {
                res += "sectype," + this.Num.ToString() + ",beam,rect," + this.Name;
                res += "\nsecdata," + SEC_Data[2].ToString() + "," + SEC_Data[1].ToString();
            }
            else if (this.SSHAPE== SecShape .T&& (int)this.SEC_Data[0] == 2)//T型截面
            {
                res += "sectype," + this.Num.ToString() + ",beam,t," + this.Name;
                res += "\nsecdata," + SEC_Data[1].ToString() + "," + SEC_Data[2].ToString() + "," + SEC_Data[3].ToString() + "," +
                    SEC_Data[4].ToString();
            }
            else if (this.SSHAPE==SecShape.SR && (int)this.SEC_Data[0] == 2)//圆形截面
            {
                res += "sectype," + this.Num.ToString() + ",beam,csolid," + this.Name;
                res += "\nsecdata," + ((double)SEC_Data[1] / 2).ToString();
            }
            else if (this.SSHAPE==SecShape.C&& (int)this.SEC_Data[0]==2)//槽钢截面
            {
                res += "sectype," + this.Num.ToString() + ",beam,chan," + this.Name;
                res += "\nsecdata," + SEC_Data[5].ToString() + "," + SEC_Data[2].ToString() + "," + SEC_Data[1].ToString() + "," +
                    SEC_Data[6].ToString() + "," + SEC_Data[4].ToString() + "," + SEC_Data[3].ToString();
            }
            else
            {
                res += "!此截面形状信息未处理：" + this.Num.ToString();
            }
            return res;
        }

        /// <summary>
        /// 重载计算截面特性
        /// </summary>
        public override void CalculateSecProp()
        {
            //throw new NotImplementedException();
            if (this.SSHAPE == SecShape.B && (int)this.SEC_Data[0] == 2)//箱形截面
            {
                double D_h = Convert.ToDouble(SEC_Data[1]);
                double D_b = Convert.ToDouble(SEC_Data[2]);
                double D_tw = Convert.ToDouble(SEC_Data[3]);
                double D_tf = Convert.ToDouble(SEC_Data[4]);
                this._Area = D_h*D_b-(D_h-2*D_tf)*(D_b-2*D_tw);//面积
                this._ASy = 2 * D_b * D_tf;//有效剪切面积
                this._ASz = 2 * D_h * D_tw;//有效剪切面积
                this._Ixx = 2 * Math.Pow((D_b-D_tw) * (D_h-D_tf), 2) / ((D_b-D_tw) / D_tf + (D_h-D_tf) / D_tw);//抗扭刚度
                this._Iyy = D_b * Math.Pow(D_h, 3) / 12 - (D_b - 2 * D_tw) * Math.Pow(D_h - 2 * D_tf, 3) / 12;//抗弯刚度
                this._Izz = D_h * Math.Pow(D_b, 3) / 12 - (D_h - 2 * D_tf) * Math.Pow(D_b - 2 * D_tw, 3) / 12;//抗弯刚度
                this.Cy = D_b / 2;
                this.Cz = D_h / 2;
                this._y1 = -D_b / 2;
                this._z1 = D_h / 2;
                this._y2 = D_b / 2;
                this._z2 = D_h / 2;
                this._y3 = D_b / 2;
                this._z3 = -D_h / 2;
                this._y4 = -D_b / 2;
                this._z4 = -D_h / 2;
                //todo:剪切系数和内外表面积未计算
            }
            else if (this.SSHAPE == SecShape.H && (int)this.SEC_Data[0] == 2)//H型截面
            {
                double h = Convert.ToDouble(SEC_Data[1]);
                double b = Convert.ToDouble(SEC_Data[2]);
                double tw = Convert.ToDouble(SEC_Data[3]);
                double tf = Convert.ToDouble(SEC_Data[4]);
                this._Area = h*b-(h-2*tf)*(b-tw);//面积
                this._ASy = 5*(2*b*tf)/6;//有效剪切面积
                this._ASz = h*tw;//有效剪切面积
                this._Ixx = (h*Math.Pow(tw,3)+2*b*Math.Pow(tf,3))/3;//抗扭刚度
                this._Iyy = b * Math.Pow(h, 3) / 12 - (b - tw) * Math.Pow(h - 2 * tf, 3) / 12;//抗弯刚度
                this._Izz = 2*tf * Math.Pow(b, 3) / 12 +(h-2*tf)*Math.Pow(tw,3)/12;//抗弯刚度
                this.Cy = b / 2;
                this.Cz = h / 2;
                this._y1 = -b / 2;
                this._z1 = h / 2;
                this._y2 = b / 2;
                this._z2 = h / 2;
                this._y3 = b / 2;
                this._z3 = -h / 2;
                this._y4 = -b / 2;
                this._z4 = -h / 2;
                //todo:剪切系数和内外表面积未计算
            }
            else if (this.SSHAPE == SecShape.P && (int)this.SEC_Data[0] == 2)//圆管截面
            {
                double tw=Convert.ToDouble(SEC_Data[2]);//壁厚
                double ri = (double)SEC_Data[1] / 2 - tw;
                double ro = (double)SEC_Data[1] / 2;
                this._Area = Math.PI * Math.Pow(ro, 2) - Math.PI * Math.Pow(ri, 2);
                this._ASy = Math.PI * (ri + tw / 2) * tw;//有效剪切面积
                this._ASz = Math.PI * (ri + tw / 2) * tw; ;//有效剪切面积
                this._Ixx = Math.PI*(Math.Pow(ro,4)-Math.Pow(ri,4))/2;//抗扭刚度
                this._Iyy = Math.PI * Math.Pow(2 * ro, 4) / 64 - Math.PI * Math.Pow(2 * ri, 4) / 64;//抗弯刚度
                this._Izz = Math.PI * Math.Pow(2 * ro, 4) / 64 - Math.PI * Math.Pow(2 * ri, 4) / 64;//抗弯刚度
                this.Cy = ro;
                this.Cz = ro;
                this._y1 = 0;
                this._z1 =ro;
                this._y2 = ro;
                this._z2 = 0;
                this._y3 =0;
                this._z3 = -ro;
                this._y4 = -ro;
                this._z4 = 0;
            }
            else if (this.SSHAPE == SecShape.SB && (int)this.SEC_Data[0] == 2)//矩形截面
            {
                double D_h = Convert.ToDouble(SEC_Data[1]);
                double D_b = Convert.ToDouble(SEC_Data[2]);
                this._Area = D_h * D_b;//面积
                this._ASy = 5 * D_b * D_h / 6;//有效剪切面积
                this._ASz = 5 * D_b * D_h / 6;//有效剪切面积
                this._Ixx =0;//抗扭刚度
                this._Iyy = D_b * Math.Pow(D_h, 3) / 12 ;//抗弯刚度
                this._Izz = D_h * Math.Pow(D_b, 3) / 12 ;//抗弯刚度
                this.Cy = D_b / 2;
                this.Cz = D_h / 2;
                this._y1 = -D_b / 2;
                this._z1 = D_h / 2;
                this._y2 = D_b / 2;
                this._z2 = D_h / 2;
                this._y3 = D_b / 2;
                this._z3 = -D_h / 2;
                this._y4 = -D_b / 2;
                this._z4 = -D_h / 2;
                //todo:面积计算实现
            }
            else if (this.SSHAPE == SecShape.T && (int)this.SEC_Data[0] == 2)//T型截面
            {
                this._Area = 0;
                //todo:面积计算实现
            }
            else if (this.SSHAPE == SecShape.SR && (int)this.SEC_Data[0] == 2)//圆形截面
            {
                double rr = (double)SEC_Data[1] / 2;
                this._Area = Math.PI * Math.Pow(rr, 2);
            }
            else if (this.SSHAPE == SecShape.C && (int)this.SEC_Data[0] == 2)//槽钢截面
            {
                this._Area = 0;
                //todo:面积计算实现
            }
            else
            {
                this._Area=0;
            }
        }
    }

    /// <summary>
    /// 渐变截面信息类
    /// </summary>
    [Serializable]
    public class SectionTapered : BSections
    {
        /// <summary>
        /// 考虑单元坐标系y轴截面弯矩的方法
        /// </summary>
        public iVAR iyVAR;
        /// <summary>
        /// 考虑单元坐标系z轴截面弯矩的方法
        /// </summary>
        public iVAR izVAR;

        /// <summary>
        /// 子截面输入类型
        /// </summary>
        public STYPE SubTYPE;
        
        /// <summary>
        /// 不带参数的构造函数
        /// </summary>
        public SectionTapered()
            : base()
        {
            this.TYPE = SecType.TAPERED;
            iyVAR = iVAR.Linear;
            izVAR = iVAR.Linear;
            SubTYPE = STYPE.USER;

            //更新截面特性
            CalculateSecProp();
        }

        /// <summary>
        /// 重载输出函数
        /// </summary>
        /// <returns>ansys截面定义命令</returns>
        public override string WriteData()
        {
            //throw new NotImplementedException();
            string res = "!此截面为TAPERED截面输入，信息未处理:" + this.Num.ToString();
            return res;
        }
        
        /// <summary>
        /// 重载计算截面特性
        /// </summary>
        public override void CalculateSecProp()
        {
            //throw new NotImplementedException();
            this._Area = 0;
            //todo:面积计算实现
        }
    }

    /// <summary>
    /// 自定义SPC截面信息类
    /// </summary>
    [Serializable]
    public class SectionGeneral : BSections
    {
        private Point2dCollection _OPOLY;
        /// <summary>
        /// 外轮廓点集
        /// </summary>
        public Point2dCollection OPOLY
        {
            get { return _OPOLY; }
        }
        private List<Point2dCollection> _IPOLYs;
        /// <summary>
        /// 内轮廓点集
        /// </summary>
        public List<Point2dCollection> IPOLYs
        {
            get { return _IPOLYs; }
        }
        private bool _bBU;
        private bool _bEQ;


        /// <summary>
        /// 未知参数
        /// </summary>
        public bool bBU
        {
            set { _bBU = value; }
            get { return _bBU; }
        }

        /// <summary>
        /// 未知参数
        /// </summary>
        public bool bEQ
        {
            set { _bEQ = value; }
            get { return _bEQ; }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public SectionGeneral()
            : base()
        {
            _OPOLY = new Point2dCollection();
            _IPOLYs = new List<Point2dCollection>();
            _bBU = true;
            _bEQ = true;
        }
        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="ise">截面号</param>
        /// <param name="Na">截面名</param>
        public SectionGeneral(int ise,string Na):base(ise,Na)
        {
            _OPOLY = new Point2dCollection();
            _IPOLYs = new List<Point2dCollection>();
            _bBU = true;
            _bEQ = true;
        }

        /// <summary>
        /// 输出Ansys截面信息
        /// </summary>
        /// <returns>APDL命令：截面类型 SectionGeneral</returns>
        public override string WriteData()
        {
            string res = null;
            res = "!此截面为SPC自定义截面";
            res += "\nsectype," + this.Num.ToString() + ",beam,mesh," + this.Name;
            res += "\nsecread,"+this.Name+",sect,,mesh";
            return res;
        }
        /// <summary>
        /// 生成自定义SPC截面的ansys宏文件
        /// </summary>
        /// <returns>宏命令流</returns>
        public string GetSectMac()
        {
            string res = null;
            res = "!"+this.Name+"截面为SPC自定义截面，用宏文件进行生成";
            res += "\n*create," + this.Name + ",sec";
            res += "\nfinish\n/clear\n/prep7";//清理模型
            res += "\n*get,kpmax,kp,0,num,maxd";//最大关键点号
            int i = 0;//编号
            //创建平面点
            foreach (Point2d pt in _OPOLY)
            {
                i++;
                res += "\nk," + "kpmax+" + i.ToString() + "," + pt.X.ToString() + "," + pt.Y.ToString();
            }
            //连接平面点为线
            for (int j = 0; j < _OPOLY.Length - 1; j++)
            {
                res += "\nl," + "kpmax+" + (j + 1).ToString() + "," + "kpmax+" + (j + 2).ToString();
            }
            res += "\nl," + "kpmax+" + i.ToString() + ",kpmax+1";//封闭曲线

            if (_IPOLYs.Count > 0)
            {
                foreach (Point2dCollection ptc in _IPOLYs)
                {
                    res += "\n!内轮廓";
                    res += "\n*get,kpmax,kp,0,num,maxd";//最大关键点号
                    i = 0;//归零
                    //创建平面点
                    foreach (Point2d pt in ptc)
                    {
                        i++;
                        res += "\nk," + "kpmax+" + i.ToString() + "," + pt.X.ToString() + "," + pt.Y.ToString();
                    }
                    //连接平面点为线
                    for (int j = 0; j < ptc.Length - 1; j++)
                    {
                        res += "\nl," + "kpmax+" + (j + 1).ToString() + "," + "kpmax+" + (j + 2).ToString();
                    }
                    res += "\nl," + "kpmax+" + i.ToString() + ",kpmax+1";//封闭曲线
                }
            }
            res += "\nlsel,all";
            res += "\nal,all\t!形成面";
            res += "\net,100,82";
            res += "\naatt,,,100";
            //设置网格划分尺寸
            //double ele = _OPOLY[0].DistantTo(_OPOLY[1])/2;//前两点距离的一半
            double ele = _CyM / 5;
            res += "\naesize,all,"+ele.ToString();
            res += "\namesh,all";
            res += "\nsecwrite," + this.Name + ",sect,,100" + "\t!输出截面文件";
            res += "\nkpmax=";
            res += "\n*end";
            return res;
        }
        /// <summary>
        /// 计算截面特性： SectionGeneral类截面什么也不做
        /// </summary>
        public override void CalculateSecProp()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 给外轮廓线增加控制点
        /// </summary>
        /// <param name="pt">点对</param>
        public void addtoOPOLY(Point2d pt)
        {
            _OPOLY.addPt(pt);
        }
        /// <summary>
        /// 给内轮廓线增加控制点
        /// </summary>
        /// <param name="index">内轮廓线索引号</param>
        /// <param name="pt">要增加的点对</param>
        public void addtoIPOLY(int index, Point2d pt)
        {
            if (_IPOLYs.Count > index)
            {
                _IPOLYs[index].addPt(pt);
            }
            else
            {
                Point2dCollection ptnew=new Point2dCollection ();
                ptnew.addPt(pt);
                _IPOLYs.Add(ptnew);
            }
        }
    }

    /// <summary>
    /// SRC截面信息类(未完成)
    /// </summary>
    [Serializable]
    public class SectionSRC : BSections
    {
        public SectionSRC()
            : base()
        {
            this.TYPE = SecType.SRC;            
        }
        /// <summary>
        /// 计算截面特性： SectionSRC类截面什么也不做
        /// </summary>
        public override void CalculateSecProp()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 输出Ansys截面信息
        /// </summary>
        /// <returns>APDL命令：截面类型 SectionSRC</returns>
        public override string WriteData()
        {
            string res = null;
            res = "!此截面为SRC混合截面";
            return res;
        }
    }

    /// <summary>
    /// 存储板单元厚度信息的类
    /// </summary>
    [Serializable]
    public class BThickness
    {
        private int _iTHK;
        private string _TYPE;
        private bool _bSAME;
        private double _THIK_IN, _THIK_OUT;

        /// <summary>
        /// 厚度截面号
        /// </summary>
        public int iTHK
        {
            set { _iTHK = value; }
            get { return _iTHK; }
        }

        /// <summary>
        /// 厚度截面类型
        /// </summary>
        public string TYPE
        {
            set { _TYPE = value; }
            get { return _TYPE; }
        }

        /// <summary>
        /// 是否平面内外同一数据
        /// </summary>
        public bool bSAME
        {
            set { _bSAME = value; }
            get { return _bSAME; }
        }

        /// <summary>
        /// 板单元面内厚度
        /// </summary>
        public double THIK_IN
        {
            set { _THIK_IN = value; }
            get { return _THIK_IN; }
        }

        /// <summary>
        /// 板单元面外厚度
        /// </summary>
        public double THIK_OUT
        {
            set { _THIK_OUT = value; }
            get { return _THIK_OUT; }
        }
    }

    /// <summary>
    /// 截面特性种类，枚举
    /// </summary>
    public enum SecType
    {
        /// <summary>
        /// 在DB中输入的，或者其它定型的截面
        /// </summary>
        DBUSER,
        /// <summary>
        /// 直接输入截面特性数据
        /// </summary>
        VALUE,
        /// <summary>
        /// SRC构件截面
        /// </summary>
        SRC,
        /// <summary>
        /// 组合截面
        /// </summary>
        COMBINED,
        /// <summary>
        /// 渐变截面
        /// </summary>
        TAPERED
    }

    /// <summary>
    /// 截面形状，枚举
    /// </summary>
    public enum SecShape
    {
        /// <summary>
        /// Angle 角钢
        /// </summary>
        L,
        /// <summary>
        /// Channel 槽钢 
        /// </summary>
        C,
        /// <summary>
        /// H型钢
        /// </summary>
        H,
        /// <summary>
        /// T型钢
        /// </summary>
        T,
        /// <summary>
        /// Box 箱形
        /// </summary>
        B,
        /// <summary>
        /// Pipe 钢管
        /// </summary>
        P,
        /// <summary>
        /// Solid Rectangle 实矩形
        /// </summary>
        SB,
        /// <summary>
        /// Solid Round 实圆形
        /// </summary>
        SR,
        /// <summary>
        /// Cold Formed Channel 冷弯槽钢
        /// </summary>
        CC,
        /// <summary>
        /// 自定义截面
        /// </summary>
        GEN
    }

    /// <summary>
    /// 考虑渐变截面贯性矩的方法（适用于TAPERED截面类型）
    /// </summary>
    public enum iVAR
    {
        /// <summary>
        /// 直线形
        /// </summary>
        Linear=1,
        /// <summary>
        /// 抛物线形
        /// </summary>
        Parabolic=2,
        /// <summary>
        /// 三次曲线形
        /// </summary>
        Cubic=3
    }

    /// <summary>
    /// 子截面形状数据输入类型（适用于TAPERED截面类型）
    /// </summary>
    public enum STYPE
    {
        /// <summary>
        /// 各国标准截面
        /// </summary>
        DB,
        /// <summary>
        /// 用户输入定型截面尺寸
        /// </summary>
        USER,
        /// <summary>
        /// 使用VALUE输入截面
        /// </summary>
        VALUE
    }

    /// <summary>
    /// 单元类型，枚举
    /// </summary>
    public enum ElemType
    {
        /// <summary>
        /// 桁架单元
        /// </summary>
        TRUSS,
        /// <summary>
        /// 梁单元
        /// </summary>
        BEAM,
        /// <summary>
        /// 只受拉单元
        /// </summary>
        TENSTR,
        /// <summary>
        /// 只受压单元
        /// </summary>
        COMPTR,
        /// <summary>
        /// 平面板单元
        /// </summary>
        PLATE,
        /// <summary>
        /// 平面应力单元
        /// </summary>
        PLSTRS,
        /// <summary>
        /// 平面应变单元
        /// </summary>
        PLSTRN,
        /// <summary>
        /// 轴对称单元
        /// </summary>
        AXISYM,
        /// <summary>
        /// 实体单元
        /// </summary>
        SOLID,
        /// <summary>
        /// 墙单元
        /// </summary>
        WALL,
        /// <summary>
        /// 未知单元
        /// </summary>
        NOTYPE
    }
    #endregion

    #region Constraint (边界约束类)
    /// <summary>
    /// 边界条件类
    /// </summary>
    [Serializable]
    public class BConstraint : Object
    {
        /// <summary>
        /// 节点号
        /// </summary>
        private int _node;
        private bool cUX;
        private bool cUY;
        private bool cUZ;
        private bool cRX;
        private bool cRY;
        private bool cRZ;
        //属性字段
        /// <summary>
        /// 节点号
        /// </summary>
        public int Node
        {
            get { return _node; }
            set { _node = value; }
        }
        /// <summary>
        /// 是否约束UX
        /// </summary>
        public bool UX
        {
            get { return cUX; }
            set { cUX = value; }
        }
        /// <summary>
        /// 是否约束UY
        /// </summary>
        public bool UY
        {
            get { return cUY; }
            set { cUY = value; }
        }
        /// <summary>
        /// 是否约束UZ
        /// </summary>
        public bool UZ
        {
            get { return cUZ; }
            set { cUZ = value; }
        }
        /// <summary>
        /// 是否约束RX
        /// </summary>
        public bool RX
        {
            get { return cRX; }
            set { cRX = value; }
        }
        /// <summary>
        /// 是否约束RY
        /// </summary>
        public bool RY
        {
            get { return cRY; }
            set { cRY = value; }
        }
        /// <summary>
        /// 是否约束RZ
        /// </summary>
        public bool RZ
        {
            get { return cRZ; }
            set { cRZ = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BConstraint()
        {
            _node = 1;
            UX = false;
            UY = false;
            UZ = false;
            RX = false;
            RY = false;
            RZ = false;
        }
        /// <summary>
        /// 复制另一个BConstraint的约束信息
        /// </summary>
        /// <param name="bc">包含约束信息的对象</param>
        public void copySupports(BConstraint bc)
        {
            cUX = bc.cUX;
            cUY = bc.cUY;
            cUZ = bc.cUZ;
            cRX = bc.cRX;
            cRY = bc.cRY;
            cRZ = bc.cRZ;
        }
    }

    /// <summary>
    /// 刚性连接类
    /// </summary>
    [Serializable]
    public class BRigidLink : Object
    {
        #region 成员
        private int _MNode;
        private bool _bUx,_bUy,_bUz,_bRx,_bRy,_bRz;
        private List<int> _SNodesList;
        private string _Group;//边界组名
        #endregion

        #region 属性
        /// <summary>
        /// 主节点号
        /// </summary>
        public int MNode
        {
            set { _MNode = value; }
            get { return _MNode; }
        }
        /// <summary>
        /// 是否约束刚性连接自由度Ux
        /// </summary>
        public bool bUx
        {
            get { return _bUx; }
        }
        /// <summary>
        /// 是否约束刚性连接自由度Uy
        /// </summary>
        public bool bUy
        {
            get { return _bUy; }
        }
        /// <summary>
        /// 是否约束刚性连接自由度Uz
        /// </summary>
        public bool bUz
        {
            get { return _bUz; }
        }
        /// <summary>
        /// 是否约束刚性连接自由度Rx
        /// </summary>
        public bool bRx
        {
            get { return _bRx; }
        }
        /// <summary>
        /// 是否约束刚性连接自由度Ry
        /// </summary>
        public bool bRy
        {
            get { return _bRy; }
        }
        /// <summary>
        /// 是否约束刚性连接自由度Rz
        /// </summary>
        public bool bRz
        {
            get { return _bRz; }
        }
        /// <summary>
        /// 刚性连接从属节点列表
        /// </summary>
        public List<int> SNodesList
        {
            get { return _SNodesList; }
        }
        /// <summary>
        /// 边界条件组名
        /// </summary>
        public string Group
        {
            set { _Group = value; }
            get { return _Group; }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 无参数化构造函数
        /// </summary>
        public BRigidLink()
        {
            _MNode = new int ();
            _bUx = false; _bUy = false; _bUz = false;
            _bRx = false; _bRy = false; _bRz = false;
            _SNodesList = new List<int>();
            _Group = null;
        }
        #endregion
        #region 方法
        /// <summary>
        /// 添加从属节点
        /// </summary>
        /// <param name="NewList">新的节点列表</param>
        public void AddSNodesList(List<int> NewList)
        {
            foreach (int NewNode in NewList)
            {
                if (_SNodesList.Contains(NewNode))
                    continue;
                else
                {
                    _SNodesList.Add(NewNode);
                }
            }

            _SNodesList.Sort();//排序
        }

        /// <summary>
        /// 按100100的格式设置约束自由度
        /// </summary>
        /// <param name="FlagString">格式字符串，只有六个字符"100100"</param>
        public void SetUxyzRxyz(string FlagString)
        {
            //如果字符数不为6则直接反回
            if (FlagString.Length != 6)
                return;

            if (FlagString[0] == '1')
                _bUx = true;
            else
                _bUx = false;
            if (FlagString[1] == '1')
                _bUy = true;
            else
                _bUy = false;
            if (FlagString[2] == '1')
                _bUz = true;
            else
                _bUz = false;
            if (FlagString[3] == '1')
                _bRx = true;
            else
                _bRx = false;
            if (FlagString[4] == '1')
                _bRy = true;
            else
                _bRy = false;
            if (FlagString[5] == '1')
                _bRz = true;
            else
                _bRz = false;
      
        }
        #endregion

    }

    /// <summary>
    /// 材料特性值类
    /// </summary>
    [Serializable]
    public class BMaterial:Object 
    {
        private int _iMAT;//材料编号
        private MatType _TYPE;//材料类型
        private string _MNAME;//材料名称
        private double _Elast;//弹性模量
        private double _Poisn;//泊松比
        private double _Thermal;//线膨胀系数
        private double _Den;//单位体积重量
        private double _Fy;//材料屈服强度

        /// <summary>
        /// 原始mgt数据信息
        /// </summary>
        public ArrayList MGT_Data;

        /// <summary>
        /// 材料号
        /// </summary>
        public int iMAT
        {
            get { return _iMAT; }
        }
        /// <summary>
        /// 材料类型
        /// </summary>
        public MatType TYPE
        {
            get { return _TYPE; }
        }
        /// <summary>
        /// 材料名称
        /// </summary>
        public string MNAME
        {
            get { return _MNAME; }
        }

        /// <summary>
        /// 弹性模量
        /// </summary>
        public double Elast
        {
            get { return _Elast; }
        }
        /// <summary>
        /// 泊松比
        /// </summary>
        public double Poisn
        {
            get { return _Poisn; }
        }
        /// <summary>
        /// 剪切模量，按各向均质材料由弹模和泊松比计算
        /// </summary>
        public double G
        {
            get { return _Elast / (2 * (1 + _Poisn)); }
        }
        /// <summary>
        /// 线膨胀系数
        /// </summary>
        public double Thermal
        {
            get { return _Thermal; }
        }
        /// <summary>
        /// 单位体积重量
        /// </summary>
        public double Den
        {
            get { return _Den; }
        }

        /// <summary>
        /// 材料屈服强度
        /// </summary>
        public double Fy
        {
            get { return _Fy; }
        }
        /// <summary>
        /// 构造函数:默认是钢的数据
        /// </summary>
        public BMaterial()
        {
            _iMAT = 1;
            _TYPE = MatType.USER;
            _MNAME = "dafault";
            _Elast = 2.06e11;
            _Poisn = 0.3;
            _Thermal = 1.2e-5;
            _Den = 7850;
            _Fy = 235e6;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="num">材料号</param>
        /// <param name="type">材料类型</param>
        /// <param name="name">材料名称</param>
        public BMaterial(int num, MatType type, string name)
        {
            _iMAT = num;
            _TYPE = type;
            _MNAME = name;
            _Elast = 0;
            _Poisn = 0;
            _Thermal = 0;
            _Den = 0;
            _Fy = 0;
            MGT_Data = new ArrayList();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="num">材料号</param>
        /// <param name="type">材料类型</param>
        /// <param name="name">材料名称</param>
        /// <param name="E">弹模（Pa）</param>
        /// <param name="Poi">泊松比</param>
        /// <param name="Ther">线膨胀系数</param>
        /// <param name="den">质量密度（kg/m3）</param>
        public  BMaterial(int num,MatType type,string name,double E,double Poi,double Ther,double den)
        {

            _iMAT = num;
            _TYPE = type;
            _MNAME = name;
            _Elast = E;
            _Poisn = Poi;
            _Thermal = Ther;
            _Den = den;
            MGT_Data = new ArrayList();
        }

        //方法
        /// <summary>
        /// 设置弹模等基本数据
        /// </summary>
        /// <param name="E">弹模（Pa）</param>
        /// <param name="Poi">泊松比</param>
        /// <param name="Ther">线膨胀系数</param>
        /// <param name="den">质量密度（kg/m3）</param>
        public void setProp(double E, double Poi, double Ther, double den)
        {
            _Elast = E;
            _Poisn = Poi;
            _Thermal = Ther;
            _Den = den;
        }

        /// <summary>
        /// 存储MGT文件原始记录信息
        /// </summary>
        /// <param name="data">mgt文件数据行</param>
        public void addMGTdata(string data)
        {
            string[] temp = data.Split(',');

            foreach (string dt in temp)
            {
                MGT_Data.Add(dt.Trim());
            }
        }
        /// <summary>
        /// 标准化材料特性值
        /// </summary>
        public void NormalizeProp()
        {
            string s1=this.MGT_Data[MGT_Data.Count-3] as string;
            string s2=this.MGT_Data[MGT_Data.Count-1] as string;
            if (this._TYPE == MatType.STEEL &&( s1 == "GB03(S)"))
            {
                if (s2 == "Q345")
                {
                    _Fy = 345e6;
                }
                else if (s2 == "Q235")
                {
                    _Fy = 235e6;
                }
                else if (s2 == "Q390")
                {
                    _Fy = 390e6;
                }
                else if (s2 == "Q420")
                {
                    _Fy = 420e6;
                }
            }
        }
    }

    /// <summary>
    /// 材料类型，枚举
    /// </summary>
    public enum MatType
    {
        /// <summary>
        /// 钢
        /// </summary>
        STEEL,
        /// <summary>
        /// 混凝土
        /// </summary>
        CONC,
        /// <summary>
        /// 钢与混凝土
        /// </summary>
        SRC,
        /// <summary>
        /// 用户自定义
        /// </summary>
        USER
    }
    #endregion

    #region 单元内力类
    /// <summary>
    /// 单元截面内力类
    /// </summary>
    [Serializable]
    public class SecForce
    {
        private double _N, _T, _Vy, _Vz, _My, _Mz;
        /// <summary>
        /// 轴向力(拉为正，压为负)
        /// </summary>
        public double N
        {
            get { return _N; }
        }
        /// <summary>
        /// 扭矩
        /// </summary>
        public double T
        {
            get { return _T; }
        }
        /// <summary>
        /// 沿单元y轴的剪力
        /// </summary>
        public double Vy
        {
            get { return _Vy; }
        }
        /// <summary>
        /// 沿单元z轴的剪力
        /// </summary>
        public double Vz
        {
            get { return _Vz; }
        }
        /// <summary>
        /// 绕单元y轴的弯矩
        /// </summary>
        public double My
        {
            get { return _My; }
        }
        /// <summary>
        /// 绕单元z轴的弯矩
        /// </summary>
        public double Mz
        {
            get { return _Mz; }
        }

        /// <summary>
        /// 构造函数1
        /// </summary>
        public SecForce()
        {
            this.SetAllForces(0, 0, 0, 0, 0, 0);
        }
        /// <summary>
        /// 构造函数2
        /// </summary>
        /// <param name="N">轴力/kN/m</param>
        /// <param name="T">扭矩/kN/m</param>
        /// <param name="Vy">剪力/kN/m</param>
        /// <param name="Vz">剪力/kN/m</param>
        /// <param name="My">弯矩/kN/m</param>
        /// <param name="Mz">弯矩/kN/m</param>
        public SecForce(double N, double T, double Vy, double Vz, double My, double Mz)
        {
            this.SetAllForces(N, T, Vy, Vz, My, Mz);
        }
        /// <summary>
        /// 指定截面内力
        /// </summary>
        /// <param name="N">轴力/kN/m</param>
        /// <param name="T">扭矩/kN/m</param>
        /// <param name="Vy">剪力/kN/m</param>
        /// <param name="Vz">剪力/kN/m</param>
        /// <param name="My">弯矩/kN/m</param>
        /// <param name="Mz">弯矩/kN/m</param>
        public void SetAllForces(double N,double T,double Vy,double Vz,double My,double Mz)
        {
            _N = N; _T = T;
            _Vy = Vy; _Vz = Vz;
            _My = My; _Mz = Mz;
        }

        /// <summary>
        /// 截面内力相加重载方法
        /// </summary>
        /// <param name="sf1">截面内力1</param>
        /// <param name="sf2">截面内力2</param>
        /// <returns>相加后的截面内力</returns>
        public static SecForce operator +(SecForce sf1, SecForce sf2)
        {
            SecForce res = new SecForce();
            res.SetAllForces(sf1.N + sf2.N, sf1.T + sf2.T, sf1.Vy + sf2.Vy,
                sf1.Vz + sf2.Vz, sf1.My + sf2.My, sf1.Mz + sf2.Mz);
            return res;
        }

        /// <summary>
        /// 截面内力自乘系数
        /// </summary>
        /// <param name="fact">因子</param>
        /// <returns>截面内力</returns>
        public  SecForce Mutiplyby(double fact)
        {
            SecForce res = new SecForce(N * fact, T * fact, Vy * fact,
                Vz * fact, My * fact, Mz * fact);
            return res;
        }

        /// <summary>
        /// 截面内力进行指数运算
        /// </summary>
        /// <param name="mi">幂指数</param>
        /// <returns>新的截面内力</returns>
        public SecForce POW(double mi)
        {
            SecForce Res = new SecForce(Math.Pow(_N, mi), Math.Pow(_T, mi),
                Math.Pow(_Vy, mi), Math.Pow(_Vz, mi), Math.Pow(_My, mi),
                Math.Pow(_Mz, mi));
            return Res;
        }
        /// <summary>
        /// 内力数据输出
        /// </summary>
        /// <returns>内力输出字符</returns>
        public override string ToString()
        {
            string res = string.Format("N:{0}, T:{1}, Vy:{2}, Vz:{3}, My:{4}, Mz:{5}",
                _N.ToString("0.0"),_T.ToString("0.0"),
                _Vy.ToString("0.0"),_Vz.ToString("0.0"),
                _My.ToString("0.0"),_Mz.ToString("0.0"));
            return res;
        }
    }
    /// <summary>
    /// 存储单元内力的类
    /// </summary>
    [Serializable]
    public class ElemForce
    {
        private SecForce _Force_i;
        private SecForce _Force_18;
        private SecForce _Force_28;
        private SecForce _Force_38;
        private SecForce _Force_48;
        private SecForce _Force_58;
        private SecForce _Force_68;
        private SecForce _Force_78;
        private SecForce _Force_j;
        #region 类属性
        /// <summary>
        /// 单元i处截面内力
        /// </summary>
        public SecForce Force_i
        {
            get { return _Force_i; }
        }
        /// <summary>
        /// 单元1/8处截面内力
        /// </summary>
        public SecForce Forcce_18
        {
            get { return _Force_18; }
        }
        /// <summary>
        /// 单元2/8处截面内力
        /// </summary>
        public SecForce Force_28
        {
            get { return _Force_28; }
        }
        /// <summary>
        /// 单元3/8处截面内力
        /// </summary>
        public SecForce Force_38
        {
            get { return _Force_38; }
        }
        /// <summary>
        /// 单元中点截面处的内力
        /// </summary>
        public SecForce Force_48
        {
            get { return _Force_48; }
        }
        /// <summary>
        /// 单元5/8处截面内力
        /// </summary>
        public SecForce Force_58
        {
            get { return _Force_58; }
        }
        /// <summary>
        /// 单元6/8处截面内力
        /// </summary>
        public SecForce Force_68
        {
            get { return _Force_68; }
        }
        /// <summary>
        /// 单元7/8处的截面内力
        /// </summary>
        public SecForce Force_78
        {
            get { return _Force_78; }
        }
        /// <summary>
        /// 单元j端截面内力
        /// </summary>
        public SecForce Force_j
        {
            get { return _Force_j; }
        }
        #endregion
        #region 类方法

        /// <summary>
        /// 构造函数
        /// </summary>
        public ElemForce()
        {
            SecForce sf = new SecForce();
            for (int i = 0; i < 9; i++)
            {
                this[i] = sf;
            }
        }
        /// <summary>
        /// 输入单元内力
        /// </summary>
        /// <param name="Fi">单元i端截面内力</param>
        /// <param name="Fj">单元j端截面内力</param>
        public void SetElemForce(SecForce Fi, SecForce Fj)
        {
            _Force_i = Fi;
            _Force_j = Fj;
        }
        /// <summary>
        /// 输入单元内力（三个截面）
        /// </summary>
        /// <param name="Fi">单元i端截面内力</param>
        /// <param name="F48">单元中截面内力</param>
        /// <param name="Fj">单元j端截面内力</param>
        public void SetElemForce(SecForce Fi, SecForce F48, SecForce Fj)
        {
            _Force_i = Fi; _Force_j = Fj;
            _Force_48 = F48;
        }
        /// <summary>
        /// 输入单元内力，每次一个截面
        /// </summary>
        /// <param name="F">要输入的截面内力</param>
        /// <param name="num">截面号：0代表i端截面，8代表j端截面</param>
        public void SetElemForce(SecForce F, int num)
        {
            switch (num)
            {
                case 0: _Force_i = F; break;
                case 1: _Force_18 = F; break;
                case 2: _Force_28 = F; break;
                case 3: _Force_38 = F; break;
                case 4: _Force_48 = F; break;
                case 5: _Force_58 = F; break;
                case 6: _Force_68 = F; break;
                case 7: _Force_78 = F; break;
                case 8: _Force_j = F; break;
                default: break;
            }
        }


        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="index">索引号</param>
        /// <returns>返回截面内力</returns>
        public SecForce this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:return _Force_i;
                    case 1:return _Force_18;
                    case 2:return _Force_28; 
                    case 3:return _Force_38;
                    case 4:return _Force_48; 
                    case 5:return _Force_58; 
                    case 6:return _Force_68; 
                    case 7:return _Force_78; 
                    case 8:return _Force_j;
                    default: return new SecForce();
                }
            }
            set
            {
                switch (index)
                {
                    case 0: _Force_i = value; break;
                    case 1: _Force_18 = value; break;
                    case 2: _Force_28 = value; break;
                    case 3: _Force_38 = value; break;
                    case 4: _Force_48 = value; break;
                    case 5: _Force_58 = value; break;
                    case 6: _Force_68 = value; break;
                    case 7: _Force_78 = value; break;
                    case 8: _Force_j =value; break;
                    default: break;
                }
            }
        }

        /// <summary>
        /// 重载单元内力相加运算符
        /// </summary>
        /// <param name="ef1">单元内力1</param>
        /// <param name="ef2">单元内力2</param>
        /// <returns>相加后的单元内力</returns>
        public static ElemForce operator +(ElemForce ef1, ElemForce ef2)
        {
            ElemForce res = new ElemForce();
            for (int i = 0; i < 9; i++)
            {
                res[i] = ef1[i] + ef2[i];
            }
            return res;
        }

        /// <summary>
        /// 单元内力自乘系数
        /// </summary>
        /// <param name="fact">系数</param>
        /// <returns>单元内力</returns>
        public ElemForce Mutiplyby(double fact)
        {
            ElemForce res = new ElemForce();
            for (int i = 0; i < 9; i++)
            {
                res[i]=this[i].Mutiplyby(fact);
            }
            return res;
        }
        /// <summary>
        /// 单元内力进行指数运算
        /// </summary>
        /// <param name="mi">幂指数</param>
        /// <returns>新的单元内力</returns>
        public ElemForce Pow(double mi)
        {
            ElemForce res = new ElemForce();
            for (int i = 0; i < 9; i++)
            {
                res[i] = this[i].POW(mi);
            }
            return res;
        }
        #endregion
    }

    /// <summary>
    /// 单元内力表
    /// </summary>
    [Serializable]
    public class BElemForceTable:Object
    {
        private int _elem;
        private SortedList<string,ElemForce> _LCForces;

        /// <summary>
        /// 单元号
        /// </summary>
        public int elem
        {
            get { return _elem; }
            set { _elem = value; }
        }

        /// <summary>
        /// 工况内力链表
        /// </summary>
        public SortedList<string, ElemForce> LCForces
        {
            get { return _LCForces; }
            set { _LCForces = value; }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public BElemForceTable()
        {
            _elem = 0;
            _LCForces = new SortedList<string, ElemForce>();
        }

        /// <summary>
        /// 给单元表添加工况内力
        /// </summary>
        /// <param name="lc">工况类型</param>
        /// <param name="force">工况内力</param>
        public void add_LCForce(string lc, ElemForce force)
        {
            _LCForces.Add(lc, force);
        }
        /// <summary>
        /// 判断是否含有相应的组合
        /// </summary>
        /// <param name="lc"></param>
        /// <returns></returns>
        public bool hasLC(string lc)
        {
            return _LCForces.ContainsKey(lc);
        }
    }
    #endregion

    #region 设计参数表类
    /// <summary>
    /// 单元自由长度表类
    /// </summary>
    [Serializable]
    public class BUnsupportedLen : Object
    {
        #region 数据与属性
        private int _iEle;
        /// <summary>
        /// 单元号
        /// </summary>
        public int IEle
        {
            get { return _iEle; }
            set { _iEle = value; }
        }
        private double _Ly, _Lz;
        /// <summary>
        /// 沿y轴自由长度
        /// </summary>
        public double Ly
        {
            get { return _Ly; }
            set { _Ly = value; }
        }
        /// <summary>
        /// 沿z轴的自由长度
        /// </summary>
        public double Lz
        {
            get { return _Lz; }
            set { _Lz = value; }
        }
        private bool _isCheckLb;
        /// <summary>
        /// 指示是否考虑受压翼缘支撑间距
        /// </summary>
        public bool IsCheckLb
        {
            get { return _isCheckLb; }
            set { _isCheckLb = value; }
        }
        private double _lb;
        /// <summary>
        /// 受压翼缘支撑点间距
        /// </summary>
        public double Lb
        {
            get { return _lb; }
            set { _lb = value; }
        }
        #endregion
        #region 构造函数
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="num">单元号</param>
        public BUnsupportedLen(int num)
        {
            _iEle = num;
            _Ly = 0;
            _Lz = 0;
            _isCheckLb = false;
            _lb = 0;
        }
        /// <summary>
        /// 指定单元自由长度
        /// </summary>
        /// <param name="num">单元号</param>
        /// <param name="lyy">沿y轴自由长度</param>
        /// <param name="lzz">沿z轴自由长度</param>
        public BUnsupportedLen(int num,double lyy,double lzz)
        {
            _iEle = num;
            _Ly = lyy;
            _Lz = lzz;
            _isCheckLb = false;
            _lb = 0;
        }
        #endregion
    }
    /// <summary>
    /// 计算长度系数表类
    /// </summary>
    [Serializable]
    public class Bk_Factor : Object
    {
        #region 数据与属性
        private int _iEle;
        /// <summary>
        /// 单元号
        /// </summary>
        public int iEle
        {
            get { return _iEle; }
            set { _iEle = value; }
        }
        private double _Ky, _Kz;
        /// <summary>
        /// 沿y轴的计算长度系数
        /// </summary>
        public double Ky
        {
            get { return _Ky; }
            set { _Ky = value; }
        }
        /// <summary>
        /// 沿z轴的计算长度系数
        /// </summary>
        public double Kz
        {
            get { return _Kz; }
            set { _Kz = value; }
        }
        #endregion
        #region 构造函数
        /// <summary>
        /// 指定单元号的构造函数，计算长度系数均取1
        /// </summary>
        /// <param name="num">单元号</param>
        public Bk_Factor(int num)
        {
            _iEle = num;
            _Ky = 1;
            _Kz = 1;
        }
        /// <summary>
        /// 指定单元计算长度系数
        /// </summary>
        /// <param name="num">单元号</param>
        /// <param name="ky">y向系数</param>
        /// <param name="kz">z向系数</param>
        public Bk_Factor(int num, double ky, double kz)
        {
            _iEle = num;
            _Ky = ky;
            _Kz = kz;
        }
        #endregion
    }
    /// <summary>
    /// 极限长细比表类
    /// </summary>
    [Serializable]
    public class BLimitsRatio : Object
    {
        #region 数据与属性
        private int _iEle;
        /// <summary>
        /// 单元号
        /// </summary>
        public int iEle
        {
            get { return _iEle; }
            set { _iEle = value; }
        }
        private bool _bNotCheck;
        /// <summary>
        /// 指示是否不验算
        /// </summary>
        public bool BNotCheck
        {
            get { return _bNotCheck; }
            set { _bNotCheck = value; }
        }
        private double _Comp, _Tens;
        /// <summary>
        /// 受压构件极限长细比
        /// </summary>
        public double Comp
        {
            get { return _Comp; }
            set { _Comp = value; }
        }
        /// <summary>
        /// 受拉构件极限长细比
        /// </summary>
        public double Tens
        {
            get { return _Tens; }
            set { _Tens = value; }
        }
        #endregion
        #region 构造函数
        /// <summary>
        /// 指定单元极限长细比，默认压200，拉300
        /// </summary>
        /// <param name="num">单元号</param>
        public BLimitsRatio(int num)
        {
            _iEle = num;
            _bNotCheck = false;
            _Comp = 200; _Tens = 300;
        }
        /// <summary>
        /// 指定单元极限长细比
        /// </summary>
        /// <param name="num">单元号</param>
        /// <param name="com">受压长细比</param>
        /// <param name="ten">受拉长细比</param>
        public BLimitsRatio(int num, double com, double ten)
        {
            _iEle = num;
            _bNotCheck = false;
            _Comp = com; _Tens = ten;
        }
        #endregion
    }
    #endregion

    #region 其它数据结构
    /// <summary>
    /// 结构组数据类[2010.8.10]
    /// </summary>
    [Serializable]
    public class BSGroup
    {
        #region 成员
        private string _GroupName;//组名称
        private List<int> _NodeList;//节点列表
        private List<int> _EleList;//单元列表
        #endregion

        #region 属性
        /// <summary>
        /// 组名称
        /// </summary>
        public string GroupName
        {
            get { return _GroupName; }
            set { _GroupName = value; }
        }
        /// <summary>
        /// 节点列表
        /// </summary>
        public List<int> NodeList
        {
            get { return _NodeList; }
        }
        /// <summary>
        /// 单元列表
        /// </summary>
        public List<int> EleList
        {
            get { return _EleList; }
        }
        /// <summary>
        /// 节点列表(用豆号格式化)
        /// </summary>
        public string FormatNodeList
        {
            get {
                string res = null;
                if (_NodeList.Count == 0)
                    return null;
                foreach (int nn in _NodeList)
                {
                    if (res!=null)
                        res += ",";
                    res = res + nn.ToString();    
                }
                return res;
            }
        }
        /// <summary>
        /// 单元列表（(用豆号格式化)）
        /// </summary>
        public string FormatEleList
        {
            get
            {
                string res = null;
                if (_EleList.Count==0)
                    return null;
                foreach (int nn in _EleList)
                {
                    if (res != null)
                        res += ",";
                    res = res + nn.ToString();  
                }
                return res;
            }
        }
        #endregion
        #region 构造函数
        /// <summary>
        /// 无参数初始化
        /// </summary>
        public BSGroup()
        {
            _GroupName = null;
            _NodeList = new List<int>();
            _EleList = new List<int>();
        }
        /// <summary>
        /// 指定组名初始化
        /// </summary>
        /// <param name="Name">组名称</param>
        public BSGroup(string Name)
        {
            _GroupName = Name;
            _NodeList = new List<int>();
            _EleList = new List<int>();
        }
        #endregion
        #region 方法
        /// <summary>
        /// 添加节点列表入组
        /// </summary>
        /// <param name="NewList">节点列表</param>
        public void AddNodeList(List<int> NewList)
        {
            foreach (int NewNode in NewList)
            {
                if (_NodeList.Contains(NewNode))
                    continue;
                else
                {
                    _NodeList.Add(NewNode);
                }
            }

            _NodeList.Sort();//排序
        }

        /// <summary>
        /// 添加单元列表入组
        /// </summary>
        /// <param name="NewList">单元列表</param>
        public void AddElemList(List<int> NewList)
        {
            foreach (int NewElem in NewList)
            {
                if (_EleList.Contains(NewElem))
                    continue;
                else
                {
                    _EleList.Add(NewElem);
                }
            }

            _EleList.Sort();//排序
        }
        #endregion
    }
    #endregion

    #region 常用基本数据类型
    /// <summary>
    /// 实现hash表重复键成员的添加
    /// </summary>
    [Serializable]
    public class RepeatedKeySort : IComparer<int>
    {
        #region IComparer 成员
        public int Compare(int x, int y)
        {
            //return -1;//直接返回不进行排序
            //以下代码可实现自动排序
            int iResult = x - y;
            if (iResult == 0) iResult = -1;
            return iResult;
        }
        #endregion
    }
    /// <summary>
    /// 实现SortedList不自动排序功能
    /// </summary>
    [Serializable]
    public class NoAutoSort : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            int iResult = string.Compare(x,y);
            if (iResult != 0)
                iResult = -1;
            return iResult;
            //排序
            // int iResult = (int)x - (int)y;
            // if(iResult == 0) iResult = -1;
            // return iResult;
        }
    }
    #endregion 
}
