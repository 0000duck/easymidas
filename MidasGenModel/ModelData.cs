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
    #region Model Info(ģ��������)

    /// <summary>
    /// ģ�͵�λ��Ϣ
    /// </summary>
    [Serializable]
    public class BUNIT
    {
        private string _Force;
        private string _Length;
        private string _Heat;
        private string _Temper;

        /// <summary>
        /// ���ĵ�λ��N��KN��
        /// </summary>
        public string Force
        {
            set { _Force = value; }
            get { return _Force; }
        }

        /// <summary>
        /// ���ȵ�λ��m��mm��
        /// </summary>
        public string Length
        {
            set { _Length = value; }
            get { return _Length; }
        }

        /// <summary>
        /// ������λ��kJ��
        /// </summary>
        public string Heat
        {
            set { _Heat = value; }
            get { return _Heat; }
        }

        /// <summary>
        /// �¶ȵ�λ��C��
        /// </summary>
        public string Temper
        {
            set { _Temper = value; }
            get { return _Temper; }
        }

        /// <summary>
        /// Ĭ�Ϲ��캯��
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
    #region Load Class(������)

    /// <summary>
    /// ���ع�������
    /// </summary>
    public enum LCType
    {
        /// <summary>
        /// ���
        /// </summary>
        D,
        /// <summary>
        /// ���
        /// </summary>
        L,
        /// <summary>
        /// ���
        /// </summary>
        W,
        /// <summary>
        /// �������
        /// </summary>
        E,
        /// <summary>
        /// ������
        /// </summary>
        LR,
        /// <summary>
        /// ѩ��
        /// </summary>
        S,
        /// <summary>
        /// �¶Ⱥ���
        /// </summary>
        T,
        /// <summary>
        /// ԤӦ������
        /// </summary>
        PS,
        /// <summary>
        /// �û�����
        /// </summary>
        USER
    }

    /// <summary>
    /// ������ϵ�����
    /// </summary>
    public enum LCKind
    {
        /// <summary>
        /// General���
        /// </summary>
        GEN,
        /// <summary>
        /// �ֽṹ��������
        /// </summary>
        STEEL,
        /// <summary>
        /// ��������������
        /// </summary>
        CONC,
        /// <summary>
        /// SRC��������
        /// </summary>
        SRC,
        /// <summary>
        /// ������������
        /// </summary>
        FDN
    }

    /// <summary>
    /// ��λ������������
    /// </summary>
    public enum ANAL
    {
        /// <summary>
        /// Static ����
        /// </summary>
        ST,
        /// <summary>
        /// Response Spectrum ��Ӧ��
        /// </summary>
        RS,
        /// <summary>
        /// żȻƫ�ĵķ�Ӧ�׽��
        /// </summary>
        ES,
        /// <summary>
        /// Time History ʱ��
        /// </summary>
        TH,
        /// <summary>
        /// Moving �ƶ�
        /// </summary>
        MV,
        /// <summary>
        /// Settlement ����
        /// </summary>
        SM,
        /// <summary>
        /// ���
        /// </summary>
        CB,
        /// <summary>
        /// �ֽṹ���
        /// </summary>
        CBS
    }
    /// <summary>
    ///���ط��� 
    /// </summary>
    public enum DIR
    {
        /// <summary>
        /// ��������X��
        /// </summary>
        GX,
        /// <summary>
        /// ��������Y��
        /// </summary>
        GY,
        /// <summary>
        /// ��������Z��
        /// </summary>
        GZ,
        /// <summary>
        /// ��Ԫ�ֲ�����X��
        /// </summary>
        LX,
        /// <summary>
        /// ��Ԫ�ֲ�����Y��
        /// </summary>
        LY,
        /// <summary>
        /// ��Ԫ�ֲ�����Z��
        /// </summary>
        LZ
    }
    /// <summary>
    /// ���ع�����
    /// </summary>
    [Serializable]
    public class BLoadCase
    {
        /// <summary>
        /// ���ع�������
        /// </summary>
        public string LCName;
        /// <summary>
        /// ���ع�������
        /// </summary>
        public LCType LCType;
        private ANAL _ANALType;//���ؼ�����������
        /// <summary>
        /// ���ؼ����������ࣺ��������Ӧ�׵�
        /// </summary>
        public ANAL ANALType
        {
            get { return _ANALType; }
            set { _ANALType = value; }
        }
        /// <summary>
        /// ������ʵ�������ع�����Ĭ������ΪUser;������������ΪST��
        /// </summary>
        /// <param name="Name">������</param>
        public BLoadCase(string Name)
        {
            LCName = Name;
            LCType = LCType.USER;
            _ANALType = ANAL.ST;
        }
    }

    /// <summary>
    /// ���ع������ϵ����
    /// </summary>
    [Serializable]
    public class BLCFactGroup:ICloneable
    {
        private ANAL _ANAL;
        /// <summary>
        /// ��λ��������������
        /// </summary>
        public ANAL ANAL
        {
            get { return _ANAL; }
            set { _ANAL = value; }
        }
        private string _LCNAME;
        /// <summary>
        /// ��������
        /// </summary>
        public string LCNAME
        {
            get { return _LCNAME; }
            set { _LCNAME = value; }
        }
        private double _FACT;
        /// <summary>
        /// ��λ���������ĺ���ϵ��
        /// </summary>
        public double FACT
        {
            get { return _FACT; }
            set { _FACT = value; }
        }

        /// <summary>
        /// Ĭ�Ϲ��캯��
        /// </summary>
        public BLCFactGroup()
        {
            _ANAL = ANAL.ST;
            _LCNAME = null;
            _FACT = 0;
        }
        /// <summary>
        /// �ɺ��ع�����ϵ������
        /// </summary>
        /// <param name="lc">���ع���</param>
        /// <param name="f">ϵ��</param>
        public BLCFactGroup(BLoadCase lc,double f)
        {
            _ANAL = lc.ANALType;
            _LCNAME = lc.LCName;
            _FACT = f;
        }
        /// <summary>
        /// ���
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    /// <summary>
    /// ���������
    /// </summary>
    [Serializable]
    public class BLoadComb:System.Object,ICloneable
    {
        #region ����������
        protected string _NAME;//�����������������
        protected LCKind _KIND;//������ϵ�����
        protected bool _bACTIVE;//�Ƿ񼤻�
        private bool _bES;//������Ĳ�����һ���ΪNO
        protected int _iTYPE;//ָ��������Ϸ�ʽ��0Ϊ���ԣ�1Ϊ����,2ΪABS��3ΪSRSS-ƽ��������
        protected string _DESC;//��˵��
        protected List<BLCFactGroup> _LoadCombData;//�����������,һ��Ϊmgt�ļ��ڶ��к�����

        /// <summary>
        /// �����������������
        /// </summary>
        public string NAME
        {
            get { return _NAME; }
            set { _NAME = value; }
        }
        /// <summary>
        /// ������ϵ�����
        /// </summary>
        public LCKind KIND
        {
            get { return _KIND; }
            set { _KIND = value; }
        }
        /// <summary>
        /// �����������
        /// </summary>
        public string DESC
        {
            get { return _DESC; }
            set {_DESC=value;}
        }
        /// <summary>
        /// ��ǰ����Ƿ񼤻�
        /// </summary>
        public bool bACTIVE
        {
            get { return _bACTIVE; }
            set { _bACTIVE = value; }
        }
        /// <summary>
        /// ��ǰ����Ƿ�Ϊ�����������
        /// </summary>
        public bool bES
        {
            get { return _bES; }
            set { _bES = value; }
        }
        /// <summary>
        /// ������Ϸ�ʽ��
        /// 0Ϊ���ԣ�1Ϊ����,2ΪABS��3ΪSRSS-ƽ��������
        /// </summary>
        public int iTYPE
        {
            get { return _iTYPE; }
        }

        /// <summary>
        /// ȡ�ù����������
        /// </summary>
        public int Num_LCGroup
        {
            get { return _LoadCombData.Count; }
        }

        /// <summary>
        /// ���ع���ϵ��������
        /// </summary>
        public List<BLCFactGroup> LoadCombData
        {
            get { return _LoadCombData; }
        }
        #endregion
        /// <summary>
        /// ���캯��
        /// </summary>
        public BLoadComb()
        {
            _bACTIVE = true;
            _KIND = LCKind.GEN;
            _bES = false;
            _iTYPE = 0;
            _LoadCombData = new List<BLCFactGroup>();
        }
        #region ��������
        /// <summary>
        /// ������ϻ�����Ϣ
        /// </summary>
        /// <param name="Name">�����������</param>
        /// <param name="Kind">�����������</param>
        /// <param name="bActive">�Ƿ񼤻�</param>
        /// <param name="bEs">������Ĳ�����һ���ΪNO</param>
        /// <param name="iType">ָ��������Ϸ�ʽ��0Ϊ���ԣ�1Ϊ+SRSS,2Ϊ-SRSS</param>
        /// <param name="Desc">��˵��</param>
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
        /// ��Ӻ��ع������ϵ�����뵱ǰ���
        /// </summary>
        /// <param name="lcfg">���ع������ϵ����</param>
        public void AddLCFactGroup(BLCFactGroup lcfg)
        {
            _LoadCombData.Add(lcfg);
        }

        /// <summary>
        /// ������ϳ�ʼ�����Ƴ������������
        /// </summary>
        public void Clear()
        {
            _NAME="";
            _KIND=LCKind.GEN;
            _bACTIVE=true;
            _bES=false;
            _iTYPE=0;
            _DESC="";
            _LoadCombData.Clear();//�Ƴ�����Ԫ��
        }

        /// <summary>
        /// �жϵ�ǰ����Ƿ���ĳ������
        /// </summary>
        /// <param name="LCName">������</param>
        /// <returns>����Ϊtrue</returns>
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
        /// �жϵ�ǰ����Ƿ��м����У��κ�һ�����Ĺ���
        /// </summary>
        /// <param name="LCs">������</param>
        /// <returns>�Ƿ�����һ��</returns>
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
        /// �жϵ�������Ƿ���ĳ�����͹����������Ӧ��
        /// </summary>
        /// <param name="type">��������</param>
        /// <returns>�ǻ��</returns>
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
        /// �������Ŵ�
        /// </summary>
        /// <param name="factor">�Ŵ�����</param>
        public void  magnifyComb(double factor)
        {
            foreach (BLCFactGroup bfg in _LoadCombData)
            {
                bfg.FACT = bfg.FACT * factor;
            }
        }
        /// <summary>
        /// �Ե����������зŴ�
        /// </summary>
        /// <param name="LcNames">�������Ӽ���</param>
        /// <param name="factor">�Ŵ�����</param>
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
        /// ���
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
    /// ���ػ������
    /// </summary>
    [Serializable]
    public class BLoadCombG : BLoadComb
    {
        private LCType _CtrLC;
        /// <summary>
        /// �����������
        /// </summary>
        public LCType CtrLC
        {
            get { return _CtrLC; }
            set { _CtrLC = value; }
        }
        /// <summary>
        /// Ĭ�Ϲ��캯��
        /// </summary>
        /// <param name="ctrlc">���ƹ���</param>
        public BLoadCombG(LCType ctrlc):base()
        {
            _CtrLC = ctrlc;//���ƹ���
        }
    }

    /// <summary>
    /// ������ϱ�
    /// </summary>
    [Serializable]
    public class BLoadCombTable
    {
        #region ���ݳ�Ա
        private List<string> _ComGen;//һ����ϱ�
        private List<string> _ComSteel;//�ֽṹ�����ñ�
        private List<string> _ComCon;//�����������ñ�
        private Hashtable _LoadCombData_G;//һ��������ݹ�ϣ��
        private Hashtable _LoadCombData_S;//�ֽṹ��Ϲ�ϣ��
        private Hashtable _LoadCombData_C;//��������Ϲ�ϣ��
        #endregion
        #region ����
        /// <summary>
        /// һ����ϱ�
        /// </summary>
        public List<string> ComGen
        {
            get { return _ComGen; }
        }
        /// <summary>
        /// �ֽṹ�����ñ�
        /// </summary>
        public List<string> ComSteel
        {
            get { return _ComSteel; }
        }
        /// <summary>
        /// �����������ñ�
        /// </summary>
        public List<string> ComCon
        {
            get { return _ComCon; }
        }
        /// <summary>
        /// һ��������ݹ�ϣ��
        /// </summary>
        public Hashtable LoadCombData_G
        {
            get { return _LoadCombData_G; }
        }
        /// <summary>
        /// �ֽṹ������ݹ�ϣ��
        /// </summary>
        public Hashtable LoadCombData_S
        {
            get { return _LoadCombData_S; }
        }
        /// <summary>
        /// ������������ݹ�ϣ��
        /// </summary>
        public Hashtable LoadCombData_C
        {
            get { return _LoadCombData_C; }
        }
        #endregion
        #region ���캯��
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
        #region ����
        /// <summary>
        /// ��Ӻ�������������
        /// ���������������
        /// </summary>
        /// <param name="com"></param>
        public void  Add(BLoadComb com)
        {
            if (this.ContainsKey(com.KIND, com.NAME))//�����ϱ��к������������ɾ��
            {
                this.Remove(com.NAME, com.KIND);
            }
            //��¼ԭʼ���˳��
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
        /// ��Ӻ�������������
        /// ǿ����ӣ���������Զ��޸�
        /// </summary>
        /// <param name="com"></param>
        public void AddEnforce(BLoadComb com)
        {
            int Count = this.getCount(com.KIND);
            com.NAME = string.Format("LC{0}", Count + 1);
            //��¼ԭʼ���˳��
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
        /// �Ƴ�ָ���������
        /// </summary>
        /// <param name="comName"></param>
        public void Remove(string comName,LCKind kind)
        {
            //��������������
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
        /// ������ϼ���״̬
        /// </summary>
        /// <param name="kind">��������</param>
        /// <param name="Name">�����</param>
        /// <param name="isActive">�Ƿ񼤻�</param>
        public void setActive(LCKind kind, string Name,bool isActive)
        {
            //��������������
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
        /// ������ϱ��Ƿ���ĳ�����
        /// </summary>
        /// <param name="Key">��Ϲؼ���</param>
        /// <returns>�ǻ��</returns>
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
        /// �������ȡ���������
        /// </summary>
        /// <param name="kind">�������</param>
        /// <param name="Name">�����</param>
        /// <returns>������϶���</returns>
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
        /// ȡ�õ�ǰ�����������
        /// </summary>
        /// <param name="kind">�������</param>
        /// <returns>��</returns>
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
    /// ������
    /// </summary>
    [Serializable]
    public abstract class Load
    {
        protected string group;//����
        protected string lc;//����
        /// <summary>
        /// ��������
        /// </summary>
        public abstract string Group
        {
            get;
            set;
        }
        /// <summary>
        /// ���ع���
        /// </summary>
        public abstract string LC
        {
            get;
            set;
        }

    }

    /// <summary>
    /// �ڵ������
    /// </summary>
    [Serializable]
    public class BNLoad : Load
    {
        private int node;//�ڵ��
        private double fx, fy, fz, mx, my, mz;
        #region ����
        /// <summary>
        /// �ڵ��
        /// </summary>
        public int iNode
        {
            get { return node; }
        }
        /// <summary>
        /// ��x�������
        /// </summary>
        public double FX
        {
            get { return fx; }
            set { fx = value; }
        }
        /// <summary>
        /// ��y�������
        /// </summary>
        public double FY
        {
            get { return fy; }
            set { fy = value; }
        }
        /// <summary>
        /// ��z�������
        /// </summary>
        public double FZ
        {
            get { return fz; }
            set { fz = value; }
        }
        /// <summary>
        /// x�����
        /// </summary>
        public double MX
        {
            get { return mx; }
            set { mx = value; }
        }
        /// <summary>
        /// y�����
        /// </summary>
        public double MY
        {
            get { return my; }
            set { my = value; }
        }
        /// <summary>
        /// z�����
        /// </summary>
        public double MZ
        {
            get { return mz; }
            set { mz = value; }
        }

        /// <summary>
        /// ���س��󷽷���Group
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

        #region ���캯��
        /// <summary>
        /// ���캯������ʼ����ֵȫΪ0
        /// </summary>
        /// <param name="n">�ڵ��</param>
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

        #region ����
        /// <summary>
        /// �Խڵ���ؽ��б����Ŵ�
        /// </summary>
        /// <param name="factor">ϵ��</param>
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
        /// ����һ���ڵ�������
        /// </summary>
        /// <param name="NL">��һ���ڵ����</param>
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
        /// ָ���ڵ����ֵ
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
    /// ���غ�����
    /// </summary>
    [Serializable]
    public class BWeight : Load
    {
        private double gx, gy, gz;
        /// <summary>
        /// �������ٶȳ���g=9.805
        /// </summary>
        private const double g = 9.805;//�������ٶ�ֵ
        /// <summary>
        /// �������ٶ�x��������
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
        /// �������ٶ�y��������
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
        /// �������ٶ�z��������
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
        /// �������ٶ�x����ֵ
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
        /// �������ٶ�y����ֵ
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
        /// �������ٶ�z����ֵ
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
        /// ���س��󷽷���Group
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
        /// ���س��󷽷���LC
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
    /// ����Ԫ��������
    /// </summary>
    public enum BeamLoadType
    {
        /// <summary>
        /// Uniform Loads ��������
        /// </summary>
        UNILOAD,
        /// <summary>
        /// Concentrated Forces ������
        /// </summary>
        CONLOAD,
        /// <summary>
        /// Concentrated Moments �������
        /// </summary>
        CONMOMENT,
        /// <summary>
        /// Uniform Moments/Torsions ������ػ�Ť��
        /// </summary>
        UNIMOMENT
    }

    /// <summary>
    /// ����Ԫ������
    /// </summary>
    [Serializable]
    public class BBLoad : Load
    {
        private int elem_num;//��Ԫ��
        private string cmd;
        private BeamLoadType _type;
        private DIR dir;//���ط���
        private bool bproj;//�Ƿ�ͶӰ

        private bool beccen;//�Ƿ�ƫ��
        private DIR eccdir;//ƫ�ķ���
        private double i_end, j_end;//i�˺�j�˵�ƫ�ĺ���ֵ
        private bool bj_end;//�Ƿ����j��ƫ�ĺ���

        private double d1, p1, d2, p2, d3, p3, d4, p4;//����������
        #region ����
        /// <summary>
        /// ��Ԫ���
        /// </summary>
        public int ELEM_num
        {
            get { return elem_num; }
            set { elem_num = value; }
        }

        /// <summary>
        /// BEAM��������Ԫ���أ�TYPITAL�����׼����Ԫ����
        /// </summary>
        public string CMD
        {
            get { return cmd; }
            set { cmd = value; }
        }

        /// <summary>
        /// ����Ԫ��������
        /// </summary>
        public BeamLoadType TYPE
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// ���ط���
        /// </summary>
        public DIR Dir
        {
            get { return dir; }
            set { dir = value; }
        }

        /// <summary>
        /// �����Ƿ�ͶӰ
        /// </summary>
        public bool bPROJ
        {
            get { return bproj; }
            set { bproj = value; }
        }

        /// <summary>
        /// �����Ƿ�ƫ��
        /// </summary>
        public bool bECCEN
        {
            get { return beccen; }
            set { beccen = value; }
        }

        /// <summary>
        /// ƫ�ĺ��ط���
        /// </summary>
        public DIR EccDir
        {
            get { return eccdir; }
            set { eccdir = value; }
        }

        /// <summary>
        /// ��������
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
        /// ���ع�����
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
        /// �Ƿ�Ϊ��ͨ�������أ�ֻ������λ�õ㣬���ߺɷ�ֵ��ͬ
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
        /// �Ƿ�Ϊ�����ξ�������:������λ�õ�
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
        /// �Ƿ�Ϊ���ξ�������:�ĸ�λ�õ㣬�м�������ֵ���
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

        #region ���캯��
        /// <summary>
        /// ���캯��
        /// </summary>
        public BBLoad()
        {
            elem_num = 0;
            beccen = false;//��ƫ��
            bproj = false;//��ͶӰ
            cmd = "BEAM";
            _type = BeamLoadType.UNILOAD;
            beccen = false;
            dir = DIR.GX;
            bj_end = false;
        }
        #endregion

        #region ����
        /// <summary>
        /// ��ȡƫ�ĵ�Ԫ������Ϣ
        /// </summary>
        /// <param name="dataline">mgt�ļ�����Ԫ����������</param>
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
                setEccenDir(Ecc_Dir);//����ƫ�ķ���
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
        /// ����ƫ�ĺ�������
        /// </summary>
        /// <param name="bEccen">�Ƿ�ƫ��:"YES"/"NO"</param>
        /// <param name="Ecc_Dir">ƫ�ķ���</param>
        /// <param name="iData">i��ƫ�ľ�</param>
        /// <param name="jData">j��ƫ�ľ�</param>
        /// <param name="bJ_End">i�˺�j��ƫ���Ƿ���ͬ:"YES"/"NO"</param>
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
                    setEccenDir(Ecc_Dir);//����ƫ�ķ���
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
        /// ��������Ԫ������Ϣ����
        /// </summary>
        /// <param name="dd1">λ��1</param>
        /// <param name="pp1">����1</param>
        /// <param name="dd2">λ��2</param>
        /// <param name="pp2">����2</param>
        /// <param name="dd3">λ��3</param>
        /// <param name="pp3">����3</param>
        /// <param name="dd4">λ��4</param>
        /// <param name="pp4">����4</param>
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
        /// ���õ�Ԫ���ط���
        /// </summary>
        /// <param name="direction">���ط����ַ�����GX/GY/GZ/LX/LY/LZ</param>
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
        /// ����ƫ�ķ���
        /// </summary>
        /// <param name="direction">ƫ�ķ����ַ�����GX/GY/GZ/LX/LY/LZ</param>
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
        /// ��ȡ����Ԫ�������ݵ�λ��ֵ
        /// </summary>
        /// <param name="i">λ�ñ�ţ�1,2,3,4</param>
        /// <returns>λ��ֵ��0~1֮�����ֵ</returns>
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
        /// ��ȡ����Ԫ�������ݵĺ���ֵ
        /// </summary>
        /// <param name="i">����ֵ���</param>
        /// <returns>����ֵ</returns>
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
    /// ��Ԫ�¶Ⱥ�����
    /// </summary>
    [Serializable]
    public class BETLoad : Load
    {
        private int _elem_num;//��Ԫ��
        private double _Temp;//��Ԫ�¶�

        #region ����
        /// <summary>
        /// ��Ԫ��
        /// </summary>
        public int Elem_Num
        {
            get { return _elem_num;}
            set { _elem_num = value; }
        }

        /// <summary>
        /// �¶Ⱥ���
        /// </summary>
        public double Temp
        {
            get { return _Temp; }
            set { _Temp = value; }
        }
        /// <summary>
        /// ���ط���
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
        /// ���ع���
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

        #region ���캯��
        /// <summary>
        /// ���캯��
        /// </summary>
        public BETLoad()
        {
            _elem_num = 0;
            _Temp = 0;
        }
        #endregion
    }

    /// <summary>
    /// ���ر���
    /// </summary>
    [Serializable]
    public class BLoadTable
    {
        #region ���ݳ�Ա
        private Hashtable _NLoadData;//�ڵ��������
        private Hashtable _BeamLoadData;//��Ԫ��������
        #endregion

        #region ����
        /// <summary>
        /// �ڵ�������ݱ�
        /// </summary>
        public Hashtable NLoadData
        {
            get { return _NLoadData; }
        }
        /// <summary>
        /// ��Ԫ�������ݱ�
        /// </summary>
        public Hashtable BLoadData
        {
            get { return _BeamLoadData; }
        }
        /// <summary>
        /// ���нڵ���صĽڵ���б�
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
        /// �к������ݵĹ����б�
        /// </summary>
        public List<string> LCList
        {
            get
            {
                List<string> Res = new List<string>();
                foreach (DictionaryEntry de in _NLoadData)
                {
                    string CurLC = de.Key.ToString();//������
                    if (Res.Contains(CurLC))
                        continue;
                    else
                        Res.Add(CurLC);
                }
                foreach (DictionaryEntry de in _BeamLoadData)
                {
                    string CurLC = de.Key.ToString();//������
                    if (Res.Contains(CurLC))
                        continue;
                    else
                        Res.Add(CurLC);
                }
                return Res;
            }
        }
        #endregion

        #region ���캯��
        /// <summary>
        /// ���캯����ʼ��
        /// </summary>
        public BLoadTable()
        {
            _NLoadData = new Hashtable();
            _BeamLoadData = new Hashtable();
        }
        #endregion
        #region ����
        /// <summary>
        /// �������ݸ��º��ر��еĽڵ����
        /// </summary>
        /// <param name="LCs">�����б�</param>
        /// <param name="NLoadData">�ɰ�ڵ��������</param>
        public void  UpdateNodeLoadList(List<BLoadCase> LCs,SortedList<int,BNLoad> NLoadData)
        {
            foreach (BLoadCase lc in LCs)
            {
                //��ǰ��ϵĽڵ���ر�
                SortedList<int, BNLoad> CurNLoadData = new SortedList<int,BNLoad> ();

                foreach (KeyValuePair<int, BNLoad> NLoad in NLoadData)
                {
                    if (NLoad.Value.LC == lc.LCName)
                    {
                        CurNLoadData.Add(NLoad.Key, NLoad.Value);
                    }
                }
                //��ӵ�ǰ��Ͻڵ�����ܱ�
                if (CurNLoadData.Count>0)
                    _NLoadData.Add(lc.LCName, CurNLoadData);                
            }
        }
        /// <summary>
        /// �������ݸ��º��ر��еĵ�Ԫ����
        /// </summary>
        /// <param name="LCs">�����б�</param>
        /// <param name="ELoadData">�ɰ浥Ԫ��������</param>
        public void UpdateElemLoadList(List<BLoadCase> LCs,SortedList<int,BBLoad> ELoadData)
        {
            foreach (BLoadCase lc in LCs)
            {
                //��ǰ��ϵĵ�Ԫ���ر�
                //[2011.03.25]��CurELoadData�޸�Ϊ���ظ���ֵ
                SortedList<int, BBLoad> CurELoadData = new SortedList<int, BBLoad>(new RepeatedKeySort());

                foreach (KeyValuePair<int, BBLoad> ELoad in ELoadData)
                {
                    if (ELoad.Value.LC == lc.LCName)
                    {
                        CurELoadData.Add(ELoad.Key, ELoad.Value);
                    }
                }
                //��ӵ�ǰ��Ͻڵ�����ܱ�
                if (CurELoadData.Count > 0)
                    _BeamLoadData.Add(lc.LCName, CurELoadData);
            }
        }

        /// <summary>
        /// ������ϵ���޸Ľڵ����
        /// </summary>
        /// <param name="node">�ڵ��</param>
        /// <param name="LC_Name">������</param>
        /// <param name="factor">����ϵ��</param>
        public void ModifyNodeLoad(int node, string LC_Name, double factor)
        {
            //���û�д˹����뷵��
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
        /// �������Ԫ�������
        /// 2011.08.23
        /// </summary>
        /// <param name="beamload">����Ԫ����</param>
        public void AddElemLoad(BBLoad beamload)
        {
            //����������ع���
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
                _BeamLoadData.Add(beamload.LC, loadTable);//����µ���ϱ�
            }
        }
        /// <summary>
        /// ��ӽڵ�������
        /// </summary>
        /// <param name="nodeload">�ڵ����</param>
        public void AddNodeLoad(BNLoad nodeload)
        {
            //����������ع���
            if (this._NLoadData.ContainsKey(nodeload.LC))
            {
                SortedList loadTable = this._NLoadData[nodeload.LC]
                    as SortedList;
                //����ڵ�����ظ�
                if (loadTable.ContainsKey(nodeload.iNode))
                {
                    BNLoad nld = loadTable[nodeload.iNode] as BNLoad;
                    nld.plus(nodeload);//���ص���
                    loadTable[nodeload.iNode] = nld;
                }
                else
                    loadTable.Add(nodeload.iNode, nodeload);

            }
            else
            {
                SortedList loadTable = new SortedList();
                loadTable.Add(nodeload.iNode, nodeload);
                this._NLoadData.Add(nodeload.LC, loadTable);//����µ���ϱ�
            }
        }
        /// <summary>
        /// ��ѯ���ع������ƶ�Ӧ�Ĺ���������
        /// </summary>
        /// <param name="LcName">������</param>
        /// <returns>�����ţ���0��ʼ����LCList���Է��ص��б�˳��Ϊ׼</returns>
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

    #region Geometry Model Class(����ģ����)
    /// <summary>
    /// ����洢�ļ���Ϣ�Ľڵ��ࣺBnodes
    /// </summary>
    [Serializable]
    public class Bnodes : Object
    {
        /// <summary>
        /// �ڵ���
        /// </summary>
        public int num;
        /// <summary>
        /// �ڵ�X����
        /// </summary>
        public double X;
        /// <summary>
        /// �ڵ�Y����
        /// </summary>
        public double Y;
        /// <summary>
        /// �ڵ�Z����
        /// </summary>
        public double Z;

        /// <summary>
        /// �ڵ�����λ��
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
        /// Ĭ�Ϲ��캯��
        /// </summary>
        /// <param name="n">�ڵ���</param>
        public Bnodes(int n)
        {
            num = n;
            X = 0;
            Y = 0;
            Z = 0;
        }
        /// <summary>
        /// �������Ĺ��캯��
        /// </summary>
        /// <param name="n">�ڵ���</param>
        /// <param name="nx">�ڵ�X����</param>
        /// <param name="ny">�ڵ�Y����</param>
        /// <param name="nz">�ڵ�Z����</param>
        public Bnodes(int n, double nx, double ny, double nz)
        {
            num = n;
            X = nx;
            Y = ny;
            Z = nz;
        }

        #region ��������
        //����ToString����
        new public string ToString()
        {
            return ("(" + num.ToString() + "," + X.ToString() + "," + Y.ToString() + "," + Z.ToString() + ")");
        }

        /// <summary>
        /// �����ڵ��ľ���
        /// </summary>
        /// <param name="nodeNext">��һ���ڵ�</param>
        /// <returns>���ؾ���ֵ</returns>
        public double DistanceTo(Bnodes nodeNext)
        {
            double res = Math.Sqrt(Math.Pow((nodeNext.X - this.X), 2) +
                Math.Pow((nodeNext.Y - this.Y), 2) +
                Math.Pow((nodeNext.Z - this.Z), 2));
            return res;
        }

        /// <summary>
        /// ����һ�ڵ�ķ�������
        /// </summary>
        /// <param name="nodeto">���Ľڵ�</param>
        /// <returns>��λ��������</returns>
        public Vector3 VectorTo(Bnodes nodeto)
        {
            Vector3 v1 = new Vector3(this.X, this.Y, this.Z);
            Vector3 v2 = new Vector3(nodeto.X, nodeto.Y, nodeto.Z);
            Vector3 Res = v2 - v1;//ʸ��������÷�������
            Res.Normalize();//��һ��
            return Res;
        }
        #endregion
        
    }

    /// <summary>
    /// ��Ԫ����
    /// </summary>
    [Serializable]
    public abstract class Element : Object
    {
        private int _iEL, _iMAT, _iPRO;
        private ElemType _TYPE;
        private CoordinateSystem _ECS;//��Ԫ����ϵ
        /// <summary>
        /// ��Ԫ�ڵ������
        /// </summary>
        public List<int> iNs;
        #region ����
        /// <summary>
        /// ��Ԫ���
        /// </summary>
        public int iEL
        {
            get { return _iEL; }
            set { _iEL = value; }
        }
        /// <summary>
        /// ��Ԫ����
        /// </summary>
        public ElemType TYPE
        {
            get { return _TYPE; }
            set { _TYPE = value; }
        }
        /// <summary>
        /// ��Ԫ���Ϻ�
        /// </summary>
        public int iMAT
        {
            get { return _iMAT; }
            set { _iMAT = value; }
        }
        /// <summary>
        /// ��Ԫ����ֵ�ţ��������
        /// </summary>
        public int iPRO
        {
            get { return _iPRO; }
            set { _iPRO = value; }
        }
        /// <summary>
        /// ��Ԫ�ֲ�����ϵ
        /// </summary>
        public CoordinateSystem ECS
        {
            get { return _ECS; }
            set { _ECS = value; }
        }

        /// <summary>
        /// �ڵ���
        /// </summary>
        public int NodeCount
        {
            get { return iNs.Count; }
        }
        #endregion
        
        /// <summary>
        ///���캯�� 
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
        /// ���캯������
        /// </summary>
        /// <param name="num">��Ԫ���</param>
        /// <param name="type">��Ԫ����</param>
        /// <param name="mat">��Ԫ���Ϻ�</param>
        /// <param name="pro">��Ԫ����ֵ�ţ�������</param>
        /// <param name="iNodes">�ڵ������</param>
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
        //����

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
    /// ����Ԫ��
    /// </summary>
    [Serializable]
    public class FrameElement : Element
    {
        private double Angle;
        private int _iSUB;
        private double _EXVAL;
        private DesignParameters _DPs;
        /// <summary>
        /// ��������3
        /// </summary>
        public int iOPT;

        #region ����
        /// <summary>
        /// �ֽṹ����Ԫ����Ʋ���
        /// </summary>
        public DesignParameters DPs
        {
            get { return _DPs; }
            set { _DPs = value; }
        }

        /// <summary>
        /// ����Ԫ����ǣ�beta�ǣ�
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
        /// �����ͣ�BEAM��TRUSS�޹أ�
        /// </summary>
        public int iSUB
        {
            set { _iSUB = value; }
            get { return _iSUB; }
        }
        /// <summary>
        /// ��Ԫ������������ݣ�BEAM��TRUSS�޹أ�
        /// </summary>
        public double EXVAL
        {
            set { _EXVAL = value; }
            get { return _EXVAL; }
        }

        /// <summary>
        /// ��Ԫ�ڵ��i
        /// </summary>
        public int I
        {
            get 
            {
                return iNs[0]; 
            }
        }

        /// <summary>
        /// ��Ԫ�ڵ��j
        /// </summary>
        public int J
        {
            get
            {
                return iNs[1];
            }
        }
        #endregion

        #region ���캯��
        /// <summary>
        /// ���������Ĺ��캯��,beta=0;type="BEAM"
        /// </summary>
        public FrameElement()
            : base()
        {
            this.TYPE = ElemType.BEAM;
            this.beta = 0;
            this._DPs = new DesignParameters();
        }
        /// <summary>
        /// �������Ĺ��캯��
        /// </summary>
        /// <param name="num">��Ԫ��</param>
        /// <param name="type">��Ԫ����</param>
        /// <param name="mat">���Ϻ�</param>
        /// <param name="pro">�������Ժ�</param>
        /// <param name="iNodes">�ڵ������</param>
        public FrameElement(int num, ElemType type, int mat, int pro, params int[] iNodes)
            : base(num, type, mat, pro, iNodes)
        {
            //���û���Ĺ��캯��
            this._DPs = new DesignParameters();
        }
        #endregion

        #region ��Ԫ����
        #endregion
    }

    /// <summary>
    /// ƽ�浥Ԫ��
    /// </summary>
    [Serializable]
    public class PlanarElement : Element
    {
        private int _iSUB;//�浥Ԫ�����
        private int _iWID;//ǽ��

        /// <summary>
        /// ��Ƚ����1
        /// </summary>
        public int iSUB
        {
            get { return _iSUB; }
            set { _iSUB = value; }
        }

        /// <summary>
        /// ǽ��
        /// </summary>
        public int iWID
        {
            get { return _iWID; }
            set { _iWID = value; }
        }

        /// <summary>
        /// ���������Ĺ��캯��
        /// </summary>
        public PlanarElement()
            : base()
        {
            this.TYPE = ElemType.PLATE;
        }

        /// <summary>
        /// ���û���Ĺ��캯��
        /// </summary>
        /// <param name="num">��Ԫ��</param>
        /// <param name="type">��Ԫ����</param>
        /// <param name="mat">���Ϻ�</param>
        /// <param name="pro">����ţ���Ⱥ�</param>
        /// <param name="iNodes">�ڵ������</param>
        public PlanarElement(int num, ElemType type, int mat, int pro, params int[] iNodes)
            : base(num, type, mat, pro, iNodes)
        {
            //���û���Ķ�Ӧ���캯��
        }
    }

    /// <summary>
    /// �洢�������ԵĻ���
    /// </summary>
    [Serializable]
    public abstract class BSections
    {
        /// <summary>
        /// �����
        /// </summary>
        private int iSEC;
        /// <summary>
        /// �������ͣ�ö��
        /// </summary>
        public SecType TYPE;
        /// <summary>
        /// ��������
        /// </summary>
        private string SNAME;
        /// <summary>
        /// ����ƫ������
        /// </summary>
        public ArrayList OFFSET;
        /// <summary>
        /// �Ƿ��Ǽ��б���
        /// </summary>
        public bool bsd;
        /// <summary>
        /// ������״��B��ʾ����
        /// </summary>
        public SecShape SSHAPE;
        /// <summary>
        /// ����������Ϣ
        /// </summary>
        public ArrayList SEC_Data;


        #region �洢��������ֵ
        protected double _Area;//���
        protected double _ASy;//��Ԫ����ϵy�᷽�����Ч�������
        protected double _ASz;//��Ԫ����ϵz�᷽�����Ч�������
        protected double _Ixx;//����Ťת���Ծ�
        /// <summary>
        /// ����Ťת���Ծ�
        /// </summary>
        public double Ixx
        {
            get { return _Ixx; }
            set { _Ixx = value; }
        }
        protected double _Iyy;//��Ԫ��y��Ľ�����Ծ�
        /// <summary>
        /// ��Ԫ��y��Ľ�����Ծ�
        /// </summary>
        public  double Iyy
        {
            get { return _Iyy; }
            set { _Iyy = value; }
        }
        protected double _Izz;//��Ԫ��z��Ľ�����Ծ�
        /// <summary>
        /// ��Ԫ��z��Ľ�����Ծ�
        /// </summary>
        public double Izz
        {
            get { return _Izz; }
            set { _Izz = value; }
        }

        private double _Iw;
        /// <summary>
        /// ë�������Թ��Ծ�
        /// </summary>
        public double Iw
        {
            get { return _Iw; }
            set { _Iw = value; }
        }

        protected double _CyP;//���к��ᵽ��Ԫ����ϵ(+)y��������˵ľ���
        /// <summary>
        /// ���к��ᵽ��Ԫ����ϵ(+)y��������˵ľ���
        /// </summary>
        public double CyP
        {
            get { return _CyP; }
            set { _CyP = value; }
        }
        protected double _CyM;//���к��ᵽ��Ԫ����ϵ(-)y��������˵ľ���
        /// <summary>
        /// ���к��ᵽ��Ԫ����ϵ(-)y��������˵ľ���
        /// </summary>
        public double CyM
        {
            get { return _CyM; }
            set { _CyM = value; }
        }
        protected double _CzP;//���к��ᵽ��Ԫ����ϵ(+)z��������˵ľ���
        /// <summary>
        /// ���к��ᵽ��Ԫ����ϵ(+)z��������˵ľ���
        /// </summary>
        public double CzP
        {
            get { return _CzP; }
            set { _CzP = value; }
        }
        protected double _CzM;//���к��ᵽ��Ԫ����ϵ(-)z��������˵ľ���
        /// <summary>
        /// ���к��ᵽ��Ԫ����ϵ(-)z��������˵ľ���
        /// </summary>
        public double CzM
        {
            get { return _CzM; }
            set { _CzM = value; }
        }
        protected double _QyB;//�����ڵ�Ԫ����ϵy�᷽��ļ���ϵ��
        protected double _QzB;//�����ڵ�Ԫ����ϵz�᷽��ļ���ϵ��
        protected double _PERI_OUT;//�����������ܳ�
        protected double _PERI_IN;//�����������ܳ�
        private double _Cy;//��������y����
        /// <summary>
        /// ��������y����
        /// </summary>
        public double Cy
        {
            get { return _Cy; }
            set { _Cy = value; }
        }
        private double _Cz;//��������z����
        /// <summary>
        /// ��������z����
        /// </summary>
        public  double Cz
        {
            get { return _Cz; }
            set { _Cz = value; }
        }
        private double _Sy;//�������y����
        /// <summary>
        /// �������y����
        /// </summary>
        public double Sy
        {
            get { return _Sy; }
            set { _Sy = value; }
        }
        private double _Sz;//�������z����
        /// <summary>
        /// �������z����
        /// </summary>
        public double Sz
        {
            get { return _Sz; }
            set { _Sz = value; }
        }

        protected double _y1;//�ĸ��ǵ�����
        //�ĸ��ǵ�����
        public double Y1
        {
            get { return _y1; }
            set { _y1 = value; }
        }
        protected double _z1;//�ĸ��ǵ�����
        //�ĸ��ǵ�����
        public  double Z1
        {
            get { return _z1; }
            set { _z1 = value; }
        }
        protected double _y2;//�ĸ��ǵ�����
        //�ĸ��ǵ�����
        public  double Y2
        {
            get { return _y2; }
            set { _y2 = value; }
        }
        protected double _z2;//�ĸ��ǵ�����
        //�ĸ��ǵ�����
        public  double Z2
        {
            get { return _z2; }
            set { _z2 = value; }
        }
        protected double _y3;//�ĸ��ǵ�����
        //�ĸ��ǵ�����
        public  double Y3
        {
            get { return _y3; }
            set { _y3 = value; }
        }
        protected double _z3;//�ĸ��ǵ�����
        //�ĸ��ǵ�����
        public  double Z3
        {
            get { return _z3; }
            set { _z3 = value; }
        }
        protected double _y4;//�ĸ��ǵ�����
        //�ĸ��ǵ�����
        public  double Y4
        {
            get { return _y4; }
            set { _y4 = value; }
        }
        protected double _z4;//�ĸ��ǵ�����
        //�ĸ��ǵ�����
        public  double Z4
        {
            get { return _z4; }
            set { _z4 = value; }
        }
        #endregion
        /// <summary>
        /// ������������
        /// </summary>
        public string Name
        {
            get { return SNAME; }
            set { SNAME = value; }
        }
        /// <summary>
        /// ������
        /// </summary>
        public int Num
        {
            get { return iSEC; }
            set { iSEC = value; }
        }
        /// <summary>
        /// �������
        /// </summary>
        public double Area
        {
            get { return _Area; }
            set { _Area = value; }
        }
        /// <summary>
        /// ��������
        /// </summary>
        public SecType SecType
        {
            get { return TYPE; }
        }

        /// <summary>
        /// ��������㼯�ϣ�Ŀǰ����4���㼯��
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
        /// ���캯��
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
        /// �������Ĺ��캯��
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
        /// ��һ����ʽ�������������Ϣ
        /// </summary>
        /// <returns>ansys�������ַ���</returns>
        public abstract string WriteData();

        /// <summary>
        /// ���°����ݼ���������ԣ���������Ծص�
        /// </summary>
        public abstract void  CalculateSecProp();

        /// <summary>
        /// ���ý��泣������ֵ1
        /// </summary>
        /// <param name="area">���</param>
        /// <param name="asy">y����Ч�������</param>
        /// <param name="asz">z����Ч�������</param>
        /// <param name="ixx">Ťת���Ծ�</param>
        /// <param name="iyy">��y��Ĺ��Ծ�</param>
        /// <param name="izz">��z��Ĺ��Ծ�</param>
        public void setSecProp1(double area, double asy, double asz, double ixx, double iyy, double izz)
        {
            _Area = area; _ASy = asy; _ASz = asz;
            _Ixx = ixx; _Iyy = iyy; _Izz = izz;
        }

        /// <summary>
        /// ���ý��泣������ֵ2
        /// </summary>
        /// <param name="cyp">���к��ᵽ��Ԫ����ϵ(+)y��������˵ľ���</param>
        /// <param name="cym">���к��ᵽ��Ԫ����ϵ(-)y��������˵ľ���</param>
        /// <param name="czp">���к��ᵽ��Ԫ����ϵ(+)z��������˵ľ���</param>
        /// <param name="czm">���к��ᵽ��Ԫ����ϵ(-)z��������˵ľ���</param>
        /// <param name="qyb">�����ڵ�Ԫ����ϵy�᷽��ļ���ϵ��</param>
        /// <param name="qzb">�����ڵ�Ԫ����ϵz�᷽��ļ���ϵ��</param>
        /// <param name="p_out">�����������ܳ�</param>
        /// <param name="p_in">�����������ܳ�</param>
        /// <param name="cy">��������y����</param>
        /// <param name="cz">��������y����</param>
        public void setSecProp2(double cyp, double cym, double czp, double czm, double qyb, double qzb,
            double p_out, double p_in, double cy, double cz)
        {
            _CyP = cyp; _CyM = cym; _CzP = czp; _CzM = czm; _QyB = qyb; _QzB = qzb;
            _PERI_OUT = p_out; _PERI_IN = p_in;
        }

        /// <summary>
        /// ���ý��泣������ֵ3
        /// </summary>
        /// <param name="y1">���ϵ�y����</param>
        /// <param name="z1">���ϵ�z����</param>
        /// <param name="y2">���ϵ�y����</param>
        /// <param name="z2">���ϵ�z����</param>
        /// <param name="y3">���µ�y����</param>
        /// <param name="z3">���µ�z����</param>
        /// <param name="y4">���µ�y����</param>
        /// <param name="z4">���µ�z����</param>
        public void setSecProp3(double y1, double z1, double y2, double z2, double y3, double z3, double y4, double z4)
        {
            _y1 = y1; _z1 = z1; _y2 = y2; _z2 = z2;
            _y3 = y3; _z3 = z3; _y4 = y4; _z4 = z4;
        }
        /// <summary>
        /// ȡ�ý����ָ�������
        /// </summary>
        /// <param name="iPt">�����ţ�Ŀǰֻ��1~4����4��������</param>
        /// <param name="Y">�����Y����</param>
        /// <param name="Z">�����Z����</param>
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
    /// ���ý�����Ϣ��
    /// </summary>
    [Serializable]
    public class SectionDBuser : BSections
    {
        public SectionDBuser():base()
        {
            this.TYPE = SecType.DBUSER;
            //���½�������
            CalculateSecProp();
        }

        /// <summary>
        /// �������ansys ����������
        /// </summary>
        /// <returns>ansys������</returns>
        public override string WriteData()
        {
            //throw new NotImplementedException();
            string res = null;
            if (this.SSHAPE == SecShape.B && (int)this.SEC_Data[0] == 2)//���ν���
            {
                res += "sectype," + this.Num.ToString() + ",beam,hrec," + this.Name;
                res += "\nsecdata," + SEC_Data[2].ToString() + "," + SEC_Data[1].ToString() + "," + SEC_Data[3].ToString() + "," + SEC_Data[3].ToString()
                + "," + SEC_Data[6].ToString() + "," + SEC_Data[4].ToString();
            }
            else if (this.SSHAPE== SecShape.H && (int)this.SEC_Data[0] == 2)//H�ͽ���
            {
                res += "sectype," + this.Num.ToString() + ",beam,i," + this.Name;
                res += "\nsecdata," + SEC_Data[2].ToString() + "," + SEC_Data[2].ToString() + "," + SEC_Data[1].ToString() + "," + SEC_Data[4].ToString()
                    + "," + SEC_Data[4].ToString() + "," + SEC_Data[3].ToString();
            }
            else if (this.SSHAPE == SecShape.P && (int)this.SEC_Data[0] == 2)//Բ�ܽ���
            {
                double ri = (double)SEC_Data[1] / 2 - (double)SEC_Data[2];
                double ro = (double)SEC_Data[1] / 2;
                res += "sectype," + this.Num.ToString() + ",beam,ctube," + this.Name;
                res += "\nsecdata," + ri.ToString() + "," + ro.ToString();
            }
            else if (this.SSHAPE==SecShape .SB&& (int)this.SEC_Data[0] == 2)//���ν���
            {
                res += "sectype," + this.Num.ToString() + ",beam,rect," + this.Name;
                res += "\nsecdata," + SEC_Data[2].ToString() + "," + SEC_Data[1].ToString();
            }
            else if (this.SSHAPE== SecShape .T&& (int)this.SEC_Data[0] == 2)//T�ͽ���
            {
                res += "sectype," + this.Num.ToString() + ",beam,t," + this.Name;
                res += "\nsecdata," + SEC_Data[1].ToString() + "," + SEC_Data[2].ToString() + "," + SEC_Data[3].ToString() + "," +
                    SEC_Data[4].ToString();
            }
            else if (this.SSHAPE==SecShape.SR && (int)this.SEC_Data[0] == 2)//Բ�ν���
            {
                res += "sectype," + this.Num.ToString() + ",beam,csolid," + this.Name;
                res += "\nsecdata," + ((double)SEC_Data[1] / 2).ToString();
            }
            else if (this.SSHAPE==SecShape.C&& (int)this.SEC_Data[0]==2)//�۸ֽ���
            {
                res += "sectype," + this.Num.ToString() + ",beam,chan," + this.Name;
                res += "\nsecdata," + SEC_Data[5].ToString() + "," + SEC_Data[2].ToString() + "," + SEC_Data[1].ToString() + "," +
                    SEC_Data[6].ToString() + "," + SEC_Data[4].ToString() + "," + SEC_Data[3].ToString();
            }
            else
            {
                res += "!�˽�����״��Ϣδ����" + this.Num.ToString();
            }
            return res;
        }

        /// <summary>
        /// ���ؼ����������
        /// </summary>
        public override void CalculateSecProp()
        {
            //throw new NotImplementedException();
            if (this.SSHAPE == SecShape.B && (int)this.SEC_Data[0] == 2)//���ν���
            {
                double D_h = Convert.ToDouble(SEC_Data[1]);
                double D_b = Convert.ToDouble(SEC_Data[2]);
                double D_tw = Convert.ToDouble(SEC_Data[3]);
                double D_tf = Convert.ToDouble(SEC_Data[4]);
                this._Area = D_h*D_b-(D_h-2*D_tf)*(D_b-2*D_tw);//���
                this._ASy = 2 * D_b * D_tf;//��Ч�������
                this._ASz = 2 * D_h * D_tw;//��Ч�������
                this._Ixx = 2 * Math.Pow((D_b-D_tw) * (D_h-D_tf), 2) / ((D_b-D_tw) / D_tf + (D_h-D_tf) / D_tw);//��Ť�ն�
                this._Iyy = D_b * Math.Pow(D_h, 3) / 12 - (D_b - 2 * D_tw) * Math.Pow(D_h - 2 * D_tf, 3) / 12;//����ն�
                this._Izz = D_h * Math.Pow(D_b, 3) / 12 - (D_h - 2 * D_tf) * Math.Pow(D_b - 2 * D_tw, 3) / 12;//����ն�
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
                //todo:����ϵ������������δ����
            }
            else if (this.SSHAPE == SecShape.H && (int)this.SEC_Data[0] == 2)//H�ͽ���
            {
                double h = Convert.ToDouble(SEC_Data[1]);
                double b = Convert.ToDouble(SEC_Data[2]);
                double tw = Convert.ToDouble(SEC_Data[3]);
                double tf = Convert.ToDouble(SEC_Data[4]);
                this._Area = h*b-(h-2*tf)*(b-tw);//���
                this._ASy = 5*(2*b*tf)/6;//��Ч�������
                this._ASz = h*tw;//��Ч�������
                this._Ixx = (h*Math.Pow(tw,3)+2*b*Math.Pow(tf,3))/3;//��Ť�ն�
                this._Iyy = b * Math.Pow(h, 3) / 12 - (b - tw) * Math.Pow(h - 2 * tf, 3) / 12;//����ն�
                this._Izz = 2*tf * Math.Pow(b, 3) / 12 +(h-2*tf)*Math.Pow(tw,3)/12;//����ն�
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
                //todo:����ϵ������������δ����
            }
            else if (this.SSHAPE == SecShape.P && (int)this.SEC_Data[0] == 2)//Բ�ܽ���
            {
                double tw=Convert.ToDouble(SEC_Data[2]);//�ں�
                double ri = (double)SEC_Data[1] / 2 - tw;
                double ro = (double)SEC_Data[1] / 2;
                this._Area = Math.PI * Math.Pow(ro, 2) - Math.PI * Math.Pow(ri, 2);
                this._ASy = Math.PI * (ri + tw / 2) * tw;//��Ч�������
                this._ASz = Math.PI * (ri + tw / 2) * tw; ;//��Ч�������
                this._Ixx = Math.PI*(Math.Pow(ro,4)-Math.Pow(ri,4))/2;//��Ť�ն�
                this._Iyy = Math.PI * Math.Pow(2 * ro, 4) / 64 - Math.PI * Math.Pow(2 * ri, 4) / 64;//����ն�
                this._Izz = Math.PI * Math.Pow(2 * ro, 4) / 64 - Math.PI * Math.Pow(2 * ri, 4) / 64;//����ն�
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
            else if (this.SSHAPE == SecShape.SB && (int)this.SEC_Data[0] == 2)//���ν���
            {
                double D_h = Convert.ToDouble(SEC_Data[1]);
                double D_b = Convert.ToDouble(SEC_Data[2]);
                this._Area = D_h * D_b;//���
                this._ASy = 5 * D_b * D_h / 6;//��Ч�������
                this._ASz = 5 * D_b * D_h / 6;//��Ч�������
                this._Ixx =0;//��Ť�ն�
                this._Iyy = D_b * Math.Pow(D_h, 3) / 12 ;//����ն�
                this._Izz = D_h * Math.Pow(D_b, 3) / 12 ;//����ն�
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
                //todo:�������ʵ��
            }
            else if (this.SSHAPE == SecShape.T && (int)this.SEC_Data[0] == 2)//T�ͽ���
            {
                this._Area = 0;
                //todo:�������ʵ��
            }
            else if (this.SSHAPE == SecShape.SR && (int)this.SEC_Data[0] == 2)//Բ�ν���
            {
                double rr = (double)SEC_Data[1] / 2;
                this._Area = Math.PI * Math.Pow(rr, 2);
            }
            else if (this.SSHAPE == SecShape.C && (int)this.SEC_Data[0] == 2)//�۸ֽ���
            {
                this._Area = 0;
                //todo:�������ʵ��
            }
            else
            {
                this._Area=0;
            }
        }
    }

    /// <summary>
    /// ���������Ϣ��
    /// </summary>
    [Serializable]
    public class SectionTapered : BSections
    {
        /// <summary>
        /// ���ǵ�Ԫ����ϵy�������صķ���
        /// </summary>
        public iVAR iyVAR;
        /// <summary>
        /// ���ǵ�Ԫ����ϵz�������صķ���
        /// </summary>
        public iVAR izVAR;

        /// <summary>
        /// �ӽ�����������
        /// </summary>
        public STYPE SubTYPE;
        
        /// <summary>
        /// ���������Ĺ��캯��
        /// </summary>
        public SectionTapered()
            : base()
        {
            this.TYPE = SecType.TAPERED;
            iyVAR = iVAR.Linear;
            izVAR = iVAR.Linear;
            SubTYPE = STYPE.USER;

            //���½�������
            CalculateSecProp();
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <returns>ansys���涨������</returns>
        public override string WriteData()
        {
            //throw new NotImplementedException();
            string res = "!�˽���ΪTAPERED�������룬��Ϣδ����:" + this.Num.ToString();
            return res;
        }
        
        /// <summary>
        /// ���ؼ����������
        /// </summary>
        public override void CalculateSecProp()
        {
            //throw new NotImplementedException();
            this._Area = 0;
            //todo:�������ʵ��
        }
    }

    /// <summary>
    /// �Զ���SPC������Ϣ��
    /// </summary>
    [Serializable]
    public class SectionGeneral : BSections
    {
        private Point2dCollection _OPOLY;
        /// <summary>
        /// �������㼯
        /// </summary>
        public Point2dCollection OPOLY
        {
            get { return _OPOLY; }
        }
        private List<Point2dCollection> _IPOLYs;
        /// <summary>
        /// �������㼯
        /// </summary>
        public List<Point2dCollection> IPOLYs
        {
            get { return _IPOLYs; }
        }
        private bool _bBU;
        private bool _bEQ;


        /// <summary>
        /// δ֪����
        /// </summary>
        public bool bBU
        {
            set { _bBU = value; }
            get { return _bBU; }
        }

        /// <summary>
        /// δ֪����
        /// </summary>
        public bool bEQ
        {
            set { _bEQ = value; }
            get { return _bEQ; }
        }
        /// <summary>
        /// ���캯��
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
        /// �������Ĺ��캯��
        /// </summary>
        /// <param name="ise">�����</param>
        /// <param name="Na">������</param>
        public SectionGeneral(int ise,string Na):base(ise,Na)
        {
            _OPOLY = new Point2dCollection();
            _IPOLYs = new List<Point2dCollection>();
            _bBU = true;
            _bEQ = true;
        }

        /// <summary>
        /// ���Ansys������Ϣ
        /// </summary>
        /// <returns>APDL����������� SectionGeneral</returns>
        public override string WriteData()
        {
            string res = null;
            res = "!�˽���ΪSPC�Զ������";
            res += "\nsectype," + this.Num.ToString() + ",beam,mesh," + this.Name;
            res += "\nsecread,"+this.Name+",sect,,mesh";
            return res;
        }
        /// <summary>
        /// �����Զ���SPC�����ansys���ļ�
        /// </summary>
        /// <returns>��������</returns>
        public string GetSectMac()
        {
            string res = null;
            res = "!"+this.Name+"����ΪSPC�Զ�����棬�ú��ļ���������";
            res += "\n*create," + this.Name + ",sec";
            res += "\nfinish\n/clear\n/prep7";//����ģ��
            res += "\n*get,kpmax,kp,0,num,maxd";//���ؼ����
            int i = 0;//���
            //����ƽ���
            foreach (Point2d pt in _OPOLY)
            {
                i++;
                res += "\nk," + "kpmax+" + i.ToString() + "," + pt.X.ToString() + "," + pt.Y.ToString();
            }
            //����ƽ���Ϊ��
            for (int j = 0; j < _OPOLY.Length - 1; j++)
            {
                res += "\nl," + "kpmax+" + (j + 1).ToString() + "," + "kpmax+" + (j + 2).ToString();
            }
            res += "\nl," + "kpmax+" + i.ToString() + ",kpmax+1";//�������

            if (_IPOLYs.Count > 0)
            {
                foreach (Point2dCollection ptc in _IPOLYs)
                {
                    res += "\n!������";
                    res += "\n*get,kpmax,kp,0,num,maxd";//���ؼ����
                    i = 0;//����
                    //����ƽ���
                    foreach (Point2d pt in ptc)
                    {
                        i++;
                        res += "\nk," + "kpmax+" + i.ToString() + "," + pt.X.ToString() + "," + pt.Y.ToString();
                    }
                    //����ƽ���Ϊ��
                    for (int j = 0; j < ptc.Length - 1; j++)
                    {
                        res += "\nl," + "kpmax+" + (j + 1).ToString() + "," + "kpmax+" + (j + 2).ToString();
                    }
                    res += "\nl," + "kpmax+" + i.ToString() + ",kpmax+1";//�������
                }
            }
            res += "\nlsel,all";
            res += "\nal,all\t!�γ���";
            res += "\net,100,82";
            res += "\naatt,,,100";
            //�������񻮷ֳߴ�
            //double ele = _OPOLY[0].DistantTo(_OPOLY[1])/2;//ǰ��������һ��
            double ele = _CyM / 5;
            res += "\naesize,all,"+ele.ToString();
            res += "\namesh,all";
            res += "\nsecwrite," + this.Name + ",sect,,100" + "\t!��������ļ�";
            res += "\nkpmax=";
            res += "\n*end";
            return res;
        }
        /// <summary>
        /// ����������ԣ� SectionGeneral�����ʲôҲ����
        /// </summary>
        public override void CalculateSecProp()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// �������������ӿ��Ƶ�
        /// </summary>
        /// <param name="pt">���</param>
        public void addtoOPOLY(Point2d pt)
        {
            _OPOLY.addPt(pt);
        }
        /// <summary>
        /// �������������ӿ��Ƶ�
        /// </summary>
        /// <param name="index">��������������</param>
        /// <param name="pt">Ҫ���ӵĵ��</param>
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
    /// SRC������Ϣ��(δ���)
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
        /// ����������ԣ� SectionSRC�����ʲôҲ����
        /// </summary>
        public override void CalculateSecProp()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// ���Ansys������Ϣ
        /// </summary>
        /// <returns>APDL����������� SectionSRC</returns>
        public override string WriteData()
        {
            string res = null;
            res = "!�˽���ΪSRC��Ͻ���";
            return res;
        }
    }

    /// <summary>
    /// �洢�嵥Ԫ�����Ϣ����
    /// </summary>
    [Serializable]
    public class BThickness
    {
        private int _iTHK;
        private string _TYPE;
        private bool _bSAME;
        private double _THIK_IN, _THIK_OUT;

        /// <summary>
        /// ��Ƚ����
        /// </summary>
        public int iTHK
        {
            set { _iTHK = value; }
            get { return _iTHK; }
        }

        /// <summary>
        /// ��Ƚ�������
        /// </summary>
        public string TYPE
        {
            set { _TYPE = value; }
            get { return _TYPE; }
        }

        /// <summary>
        /// �Ƿ�ƽ������ͬһ����
        /// </summary>
        public bool bSAME
        {
            set { _bSAME = value; }
            get { return _bSAME; }
        }

        /// <summary>
        /// �嵥Ԫ���ں��
        /// </summary>
        public double THIK_IN
        {
            set { _THIK_IN = value; }
            get { return _THIK_IN; }
        }

        /// <summary>
        /// �嵥Ԫ������
        /// </summary>
        public double THIK_OUT
        {
            set { _THIK_OUT = value; }
            get { return _THIK_OUT; }
        }
    }

    /// <summary>
    /// �����������࣬ö��
    /// </summary>
    public enum SecType
    {
        /// <summary>
        /// ��DB������ģ������������͵Ľ���
        /// </summary>
        DBUSER,
        /// <summary>
        /// ֱ�����������������
        /// </summary>
        VALUE,
        /// <summary>
        /// SRC��������
        /// </summary>
        SRC,
        /// <summary>
        /// ��Ͻ���
        /// </summary>
        COMBINED,
        /// <summary>
        /// �������
        /// </summary>
        TAPERED
    }

    /// <summary>
    /// ������״��ö��
    /// </summary>
    public enum SecShape
    {
        /// <summary>
        /// Angle �Ǹ�
        /// </summary>
        L,
        /// <summary>
        /// Channel �۸� 
        /// </summary>
        C,
        /// <summary>
        /// H�͸�
        /// </summary>
        H,
        /// <summary>
        /// T�͸�
        /// </summary>
        T,
        /// <summary>
        /// Box ����
        /// </summary>
        B,
        /// <summary>
        /// Pipe �ֹ�
        /// </summary>
        P,
        /// <summary>
        /// Solid Rectangle ʵ����
        /// </summary>
        SB,
        /// <summary>
        /// Solid Round ʵԲ��
        /// </summary>
        SR,
        /// <summary>
        /// Cold Formed Channel ����۸�
        /// </summary>
        CC,
        /// <summary>
        /// �Զ������
        /// </summary>
        GEN
    }

    /// <summary>
    /// ���ǽ��������Ծصķ�����������TAPERED�������ͣ�
    /// </summary>
    public enum iVAR
    {
        /// <summary>
        /// ֱ����
        /// </summary>
        Linear=1,
        /// <summary>
        /// ��������
        /// </summary>
        Parabolic=2,
        /// <summary>
        /// ����������
        /// </summary>
        Cubic=3
    }

    /// <summary>
    /// �ӽ�����״�����������ͣ�������TAPERED�������ͣ�
    /// </summary>
    public enum STYPE
    {
        /// <summary>
        /// ������׼����
        /// </summary>
        DB,
        /// <summary>
        /// �û����붨�ͽ���ߴ�
        /// </summary>
        USER,
        /// <summary>
        /// ʹ��VALUE�������
        /// </summary>
        VALUE
    }

    /// <summary>
    /// ��Ԫ���ͣ�ö��
    /// </summary>
    public enum ElemType
    {
        /// <summary>
        /// ��ܵ�Ԫ
        /// </summary>
        TRUSS,
        /// <summary>
        /// ����Ԫ
        /// </summary>
        BEAM,
        /// <summary>
        /// ֻ������Ԫ
        /// </summary>
        TENSTR,
        /// <summary>
        /// ֻ��ѹ��Ԫ
        /// </summary>
        COMPTR,
        /// <summary>
        /// ƽ��嵥Ԫ
        /// </summary>
        PLATE,
        /// <summary>
        /// ƽ��Ӧ����Ԫ
        /// </summary>
        PLSTRS,
        /// <summary>
        /// ƽ��Ӧ�䵥Ԫ
        /// </summary>
        PLSTRN,
        /// <summary>
        /// ��ԳƵ�Ԫ
        /// </summary>
        AXISYM,
        /// <summary>
        /// ʵ�嵥Ԫ
        /// </summary>
        SOLID,
        /// <summary>
        /// ǽ��Ԫ
        /// </summary>
        WALL,
        /// <summary>
        /// δ֪��Ԫ
        /// </summary>
        NOTYPE
    }
    #endregion

    #region Constraint (�߽�Լ����)
    /// <summary>
    /// �߽�������
    /// </summary>
    [Serializable]
    public class BConstraint : Object
    {
        /// <summary>
        /// �ڵ��
        /// </summary>
        private int _node;
        private bool cUX;
        private bool cUY;
        private bool cUZ;
        private bool cRX;
        private bool cRY;
        private bool cRZ;
        //�����ֶ�
        /// <summary>
        /// �ڵ��
        /// </summary>
        public int Node
        {
            get { return _node; }
            set { _node = value; }
        }
        /// <summary>
        /// �Ƿ�Լ��UX
        /// </summary>
        public bool UX
        {
            get { return cUX; }
            set { cUX = value; }
        }
        /// <summary>
        /// �Ƿ�Լ��UY
        /// </summary>
        public bool UY
        {
            get { return cUY; }
            set { cUY = value; }
        }
        /// <summary>
        /// �Ƿ�Լ��UZ
        /// </summary>
        public bool UZ
        {
            get { return cUZ; }
            set { cUZ = value; }
        }
        /// <summary>
        /// �Ƿ�Լ��RX
        /// </summary>
        public bool RX
        {
            get { return cRX; }
            set { cRX = value; }
        }
        /// <summary>
        /// �Ƿ�Լ��RY
        /// </summary>
        public bool RY
        {
            get { return cRY; }
            set { cRY = value; }
        }
        /// <summary>
        /// �Ƿ�Լ��RZ
        /// </summary>
        public bool RZ
        {
            get { return cRZ; }
            set { cRZ = value; }
        }

        /// <summary>
        /// ���캯��
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
        /// ������һ��BConstraint��Լ����Ϣ
        /// </summary>
        /// <param name="bc">����Լ����Ϣ�Ķ���</param>
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
    /// ����������
    /// </summary>
    [Serializable]
    public class BRigidLink : Object
    {
        #region ��Ա
        private int _MNode;
        private bool _bUx,_bUy,_bUz,_bRx,_bRy,_bRz;
        private List<int> _SNodesList;
        private string _Group;//�߽�����
        #endregion

        #region ����
        /// <summary>
        /// ���ڵ��
        /// </summary>
        public int MNode
        {
            set { _MNode = value; }
            get { return _MNode; }
        }
        /// <summary>
        /// �Ƿ�Լ�������������ɶ�Ux
        /// </summary>
        public bool bUx
        {
            get { return _bUx; }
        }
        /// <summary>
        /// �Ƿ�Լ�������������ɶ�Uy
        /// </summary>
        public bool bUy
        {
            get { return _bUy; }
        }
        /// <summary>
        /// �Ƿ�Լ�������������ɶ�Uz
        /// </summary>
        public bool bUz
        {
            get { return _bUz; }
        }
        /// <summary>
        /// �Ƿ�Լ�������������ɶ�Rx
        /// </summary>
        public bool bRx
        {
            get { return _bRx; }
        }
        /// <summary>
        /// �Ƿ�Լ�������������ɶ�Ry
        /// </summary>
        public bool bRy
        {
            get { return _bRy; }
        }
        /// <summary>
        /// �Ƿ�Լ�������������ɶ�Rz
        /// </summary>
        public bool bRz
        {
            get { return _bRz; }
        }
        /// <summary>
        /// �������Ӵ����ڵ��б�
        /// </summary>
        public List<int> SNodesList
        {
            get { return _SNodesList; }
        }
        /// <summary>
        /// �߽���������
        /// </summary>
        public string Group
        {
            set { _Group = value; }
            get { return _Group; }
        }
        #endregion

        #region ���캯��
        /// <summary>
        /// �޲��������캯��
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
        #region ����
        /// <summary>
        /// ��Ӵ����ڵ�
        /// </summary>
        /// <param name="NewList">�µĽڵ��б�</param>
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

            _SNodesList.Sort();//����
        }

        /// <summary>
        /// ��100100�ĸ�ʽ����Լ�����ɶ�
        /// </summary>
        /// <param name="FlagString">��ʽ�ַ�����ֻ�������ַ�"100100"</param>
        public void SetUxyzRxyz(string FlagString)
        {
            //����ַ�����Ϊ6��ֱ�ӷ���
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
    /// ��������ֵ��
    /// </summary>
    [Serializable]
    public class BMaterial:Object 
    {
        private int _iMAT;//���ϱ��
        private MatType _TYPE;//��������
        private string _MNAME;//��������
        private double _Elast;//����ģ��
        private double _Poisn;//���ɱ�
        private double _Thermal;//������ϵ��
        private double _Den;//��λ�������
        private double _Fy;//��������ǿ��

        /// <summary>
        /// ԭʼmgt������Ϣ
        /// </summary>
        public ArrayList MGT_Data;

        /// <summary>
        /// ���Ϻ�
        /// </summary>
        public int iMAT
        {
            get { return _iMAT; }
        }
        /// <summary>
        /// ��������
        /// </summary>
        public MatType TYPE
        {
            get { return _TYPE; }
        }
        /// <summary>
        /// ��������
        /// </summary>
        public string MNAME
        {
            get { return _MNAME; }
        }

        /// <summary>
        /// ����ģ��
        /// </summary>
        public double Elast
        {
            get { return _Elast; }
        }
        /// <summary>
        /// ���ɱ�
        /// </summary>
        public double Poisn
        {
            get { return _Poisn; }
        }
        /// <summary>
        /// ����ģ������������ʲ����ɵ�ģ�Ͳ��ɱȼ���
        /// </summary>
        public double G
        {
            get { return _Elast / (2 * (1 + _Poisn)); }
        }
        /// <summary>
        /// ������ϵ��
        /// </summary>
        public double Thermal
        {
            get { return _Thermal; }
        }
        /// <summary>
        /// ��λ�������
        /// </summary>
        public double Den
        {
            get { return _Den; }
        }

        /// <summary>
        /// ��������ǿ��
        /// </summary>
        public double Fy
        {
            get { return _Fy; }
        }
        /// <summary>
        /// ���캯��:Ĭ���Ǹֵ�����
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
        /// ���캯��
        /// </summary>
        /// <param name="num">���Ϻ�</param>
        /// <param name="type">��������</param>
        /// <param name="name">��������</param>
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
        /// ���캯��
        /// </summary>
        /// <param name="num">���Ϻ�</param>
        /// <param name="type">��������</param>
        /// <param name="name">��������</param>
        /// <param name="E">��ģ��Pa��</param>
        /// <param name="Poi">���ɱ�</param>
        /// <param name="Ther">������ϵ��</param>
        /// <param name="den">�����ܶȣ�kg/m3��</param>
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

        //����
        /// <summary>
        /// ���õ�ģ�Ȼ�������
        /// </summary>
        /// <param name="E">��ģ��Pa��</param>
        /// <param name="Poi">���ɱ�</param>
        /// <param name="Ther">������ϵ��</param>
        /// <param name="den">�����ܶȣ�kg/m3��</param>
        public void setProp(double E, double Poi, double Ther, double den)
        {
            _Elast = E;
            _Poisn = Poi;
            _Thermal = Ther;
            _Den = den;
        }

        /// <summary>
        /// �洢MGT�ļ�ԭʼ��¼��Ϣ
        /// </summary>
        /// <param name="data">mgt�ļ�������</param>
        public void addMGTdata(string data)
        {
            string[] temp = data.Split(',');

            foreach (string dt in temp)
            {
                MGT_Data.Add(dt.Trim());
            }
        }
        /// <summary>
        /// ��׼����������ֵ
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
    /// �������ͣ�ö��
    /// </summary>
    public enum MatType
    {
        /// <summary>
        /// ��
        /// </summary>
        STEEL,
        /// <summary>
        /// ������
        /// </summary>
        CONC,
        /// <summary>
        /// ���������
        /// </summary>
        SRC,
        /// <summary>
        /// �û��Զ���
        /// </summary>
        USER
    }
    #endregion

    #region ��Ԫ������
    /// <summary>
    /// ��Ԫ����������
    /// </summary>
    [Serializable]
    public class SecForce
    {
        private double _N, _T, _Vy, _Vz, _My, _Mz;
        /// <summary>
        /// ������(��Ϊ����ѹΪ��)
        /// </summary>
        public double N
        {
            get { return _N; }
        }
        /// <summary>
        /// Ť��
        /// </summary>
        public double T
        {
            get { return _T; }
        }
        /// <summary>
        /// �ص�Ԫy��ļ���
        /// </summary>
        public double Vy
        {
            get { return _Vy; }
        }
        /// <summary>
        /// �ص�Ԫz��ļ���
        /// </summary>
        public double Vz
        {
            get { return _Vz; }
        }
        /// <summary>
        /// �Ƶ�Ԫy������
        /// </summary>
        public double My
        {
            get { return _My; }
        }
        /// <summary>
        /// �Ƶ�Ԫz������
        /// </summary>
        public double Mz
        {
            get { return _Mz; }
        }

        /// <summary>
        /// ���캯��1
        /// </summary>
        public SecForce()
        {
            this.SetAllForces(0, 0, 0, 0, 0, 0);
        }
        /// <summary>
        /// ���캯��2
        /// </summary>
        /// <param name="N">����/kN/m</param>
        /// <param name="T">Ť��/kN/m</param>
        /// <param name="Vy">����/kN/m</param>
        /// <param name="Vz">����/kN/m</param>
        /// <param name="My">���/kN/m</param>
        /// <param name="Mz">���/kN/m</param>
        public SecForce(double N, double T, double Vy, double Vz, double My, double Mz)
        {
            this.SetAllForces(N, T, Vy, Vz, My, Mz);
        }
        /// <summary>
        /// ָ����������
        /// </summary>
        /// <param name="N">����/kN/m</param>
        /// <param name="T">Ť��/kN/m</param>
        /// <param name="Vy">����/kN/m</param>
        /// <param name="Vz">����/kN/m</param>
        /// <param name="My">���/kN/m</param>
        /// <param name="Mz">���/kN/m</param>
        public void SetAllForces(double N,double T,double Vy,double Vz,double My,double Mz)
        {
            _N = N; _T = T;
            _Vy = Vy; _Vz = Vz;
            _My = My; _Mz = Mz;
        }

        /// <summary>
        /// ��������������ط���
        /// </summary>
        /// <param name="sf1">��������1</param>
        /// <param name="sf2">��������2</param>
        /// <returns>��Ӻ�Ľ�������</returns>
        public static SecForce operator +(SecForce sf1, SecForce sf2)
        {
            SecForce res = new SecForce();
            res.SetAllForces(sf1.N + sf2.N, sf1.T + sf2.T, sf1.Vy + sf2.Vy,
                sf1.Vz + sf2.Vz, sf1.My + sf2.My, sf1.Mz + sf2.Mz);
            return res;
        }

        /// <summary>
        /// ���������Գ�ϵ��
        /// </summary>
        /// <param name="fact">����</param>
        /// <returns>��������</returns>
        public  SecForce Mutiplyby(double fact)
        {
            SecForce res = new SecForce(N * fact, T * fact, Vy * fact,
                Vz * fact, My * fact, Mz * fact);
            return res;
        }

        /// <summary>
        /// ������������ָ������
        /// </summary>
        /// <param name="mi">��ָ��</param>
        /// <returns>�µĽ�������</returns>
        public SecForce POW(double mi)
        {
            SecForce Res = new SecForce(Math.Pow(_N, mi), Math.Pow(_T, mi),
                Math.Pow(_Vy, mi), Math.Pow(_Vz, mi), Math.Pow(_My, mi),
                Math.Pow(_Mz, mi));
            return Res;
        }
        /// <summary>
        /// �����������
        /// </summary>
        /// <returns>��������ַ�</returns>
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
    /// �洢��Ԫ��������
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
        #region ������
        /// <summary>
        /// ��Ԫi����������
        /// </summary>
        public SecForce Force_i
        {
            get { return _Force_i; }
        }
        /// <summary>
        /// ��Ԫ1/8����������
        /// </summary>
        public SecForce Forcce_18
        {
            get { return _Force_18; }
        }
        /// <summary>
        /// ��Ԫ2/8����������
        /// </summary>
        public SecForce Force_28
        {
            get { return _Force_28; }
        }
        /// <summary>
        /// ��Ԫ3/8����������
        /// </summary>
        public SecForce Force_38
        {
            get { return _Force_38; }
        }
        /// <summary>
        /// ��Ԫ�е���洦������
        /// </summary>
        public SecForce Force_48
        {
            get { return _Force_48; }
        }
        /// <summary>
        /// ��Ԫ5/8����������
        /// </summary>
        public SecForce Force_58
        {
            get { return _Force_58; }
        }
        /// <summary>
        /// ��Ԫ6/8����������
        /// </summary>
        public SecForce Force_68
        {
            get { return _Force_68; }
        }
        /// <summary>
        /// ��Ԫ7/8���Ľ�������
        /// </summary>
        public SecForce Force_78
        {
            get { return _Force_78; }
        }
        /// <summary>
        /// ��Ԫj�˽�������
        /// </summary>
        public SecForce Force_j
        {
            get { return _Force_j; }
        }
        #endregion
        #region �෽��

        /// <summary>
        /// ���캯��
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
        /// ���뵥Ԫ����
        /// </summary>
        /// <param name="Fi">��Ԫi�˽�������</param>
        /// <param name="Fj">��Ԫj�˽�������</param>
        public void SetElemForce(SecForce Fi, SecForce Fj)
        {
            _Force_i = Fi;
            _Force_j = Fj;
        }
        /// <summary>
        /// ���뵥Ԫ�������������棩
        /// </summary>
        /// <param name="Fi">��Ԫi�˽�������</param>
        /// <param name="F48">��Ԫ�н�������</param>
        /// <param name="Fj">��Ԫj�˽�������</param>
        public void SetElemForce(SecForce Fi, SecForce F48, SecForce Fj)
        {
            _Force_i = Fi; _Force_j = Fj;
            _Force_48 = F48;
        }
        /// <summary>
        /// ���뵥Ԫ������ÿ��һ������
        /// </summary>
        /// <param name="F">Ҫ����Ľ�������</param>
        /// <param name="num">����ţ�0����i�˽��棬8����j�˽���</param>
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
        /// ������
        /// </summary>
        /// <param name="index">������</param>
        /// <returns>���ؽ�������</returns>
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
        /// ���ص�Ԫ������������
        /// </summary>
        /// <param name="ef1">��Ԫ����1</param>
        /// <param name="ef2">��Ԫ����2</param>
        /// <returns>��Ӻ�ĵ�Ԫ����</returns>
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
        /// ��Ԫ�����Գ�ϵ��
        /// </summary>
        /// <param name="fact">ϵ��</param>
        /// <returns>��Ԫ����</returns>
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
        /// ��Ԫ��������ָ������
        /// </summary>
        /// <param name="mi">��ָ��</param>
        /// <returns>�µĵ�Ԫ����</returns>
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
    /// ��Ԫ������
    /// </summary>
    [Serializable]
    public class BElemForceTable:Object
    {
        private int _elem;
        private SortedList<string,ElemForce> _LCForces;

        /// <summary>
        /// ��Ԫ��
        /// </summary>
        public int elem
        {
            get { return _elem; }
            set { _elem = value; }
        }

        /// <summary>
        /// ������������
        /// </summary>
        public SortedList<string, ElemForce> LCForces
        {
            get { return _LCForces; }
            set { _LCForces = value; }
        }

        /// <summary>
        /// ��ʼ��
        /// </summary>
        public BElemForceTable()
        {
            _elem = 0;
            _LCForces = new SortedList<string, ElemForce>();
        }

        /// <summary>
        /// ����Ԫ����ӹ�������
        /// </summary>
        /// <param name="lc">��������</param>
        /// <param name="force">��������</param>
        public void add_LCForce(string lc, ElemForce force)
        {
            _LCForces.Add(lc, force);
        }
        /// <summary>
        /// �ж��Ƿ�����Ӧ�����
        /// </summary>
        /// <param name="lc"></param>
        /// <returns></returns>
        public bool hasLC(string lc)
        {
            return _LCForces.ContainsKey(lc);
        }
    }
    #endregion

    #region ��Ʋ�������
    /// <summary>
    /// ��Ԫ���ɳ��ȱ���
    /// </summary>
    [Serializable]
    public class BUnsupportedLen : Object
    {
        #region ����������
        private int _iEle;
        /// <summary>
        /// ��Ԫ��
        /// </summary>
        public int IEle
        {
            get { return _iEle; }
            set { _iEle = value; }
        }
        private double _Ly, _Lz;
        /// <summary>
        /// ��y�����ɳ���
        /// </summary>
        public double Ly
        {
            get { return _Ly; }
            set { _Ly = value; }
        }
        /// <summary>
        /// ��z������ɳ���
        /// </summary>
        public double Lz
        {
            get { return _Lz; }
            set { _Lz = value; }
        }
        private bool _isCheckLb;
        /// <summary>
        /// ָʾ�Ƿ�����ѹ��Ե֧�ż��
        /// </summary>
        public bool IsCheckLb
        {
            get { return _isCheckLb; }
            set { _isCheckLb = value; }
        }
        private double _lb;
        /// <summary>
        /// ��ѹ��Ե֧�ŵ���
        /// </summary>
        public double Lb
        {
            get { return _lb; }
            set { _lb = value; }
        }
        #endregion
        #region ���캯��
        /// <summary>
        /// Ĭ�Ϲ��캯��
        /// </summary>
        /// <param name="num">��Ԫ��</param>
        public BUnsupportedLen(int num)
        {
            _iEle = num;
            _Ly = 0;
            _Lz = 0;
            _isCheckLb = false;
            _lb = 0;
        }
        /// <summary>
        /// ָ����Ԫ���ɳ���
        /// </summary>
        /// <param name="num">��Ԫ��</param>
        /// <param name="lyy">��y�����ɳ���</param>
        /// <param name="lzz">��z�����ɳ���</param>
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
    /// ���㳤��ϵ������
    /// </summary>
    [Serializable]
    public class Bk_Factor : Object
    {
        #region ����������
        private int _iEle;
        /// <summary>
        /// ��Ԫ��
        /// </summary>
        public int iEle
        {
            get { return _iEle; }
            set { _iEle = value; }
        }
        private double _Ky, _Kz;
        /// <summary>
        /// ��y��ļ��㳤��ϵ��
        /// </summary>
        public double Ky
        {
            get { return _Ky; }
            set { _Ky = value; }
        }
        /// <summary>
        /// ��z��ļ��㳤��ϵ��
        /// </summary>
        public double Kz
        {
            get { return _Kz; }
            set { _Kz = value; }
        }
        #endregion
        #region ���캯��
        /// <summary>
        /// ָ����Ԫ�ŵĹ��캯�������㳤��ϵ����ȡ1
        /// </summary>
        /// <param name="num">��Ԫ��</param>
        public Bk_Factor(int num)
        {
            _iEle = num;
            _Ky = 1;
            _Kz = 1;
        }
        /// <summary>
        /// ָ����Ԫ���㳤��ϵ��
        /// </summary>
        /// <param name="num">��Ԫ��</param>
        /// <param name="ky">y��ϵ��</param>
        /// <param name="kz">z��ϵ��</param>
        public Bk_Factor(int num, double ky, double kz)
        {
            _iEle = num;
            _Ky = ky;
            _Kz = kz;
        }
        #endregion
    }
    /// <summary>
    /// ���޳�ϸ�ȱ���
    /// </summary>
    [Serializable]
    public class BLimitsRatio : Object
    {
        #region ����������
        private int _iEle;
        /// <summary>
        /// ��Ԫ��
        /// </summary>
        public int iEle
        {
            get { return _iEle; }
            set { _iEle = value; }
        }
        private bool _bNotCheck;
        /// <summary>
        /// ָʾ�Ƿ�����
        /// </summary>
        public bool BNotCheck
        {
            get { return _bNotCheck; }
            set { _bNotCheck = value; }
        }
        private double _Comp, _Tens;
        /// <summary>
        /// ��ѹ�������޳�ϸ��
        /// </summary>
        public double Comp
        {
            get { return _Comp; }
            set { _Comp = value; }
        }
        /// <summary>
        /// �����������޳�ϸ��
        /// </summary>
        public double Tens
        {
            get { return _Tens; }
            set { _Tens = value; }
        }
        #endregion
        #region ���캯��
        /// <summary>
        /// ָ����Ԫ���޳�ϸ�ȣ�Ĭ��ѹ200����300
        /// </summary>
        /// <param name="num">��Ԫ��</param>
        public BLimitsRatio(int num)
        {
            _iEle = num;
            _bNotCheck = false;
            _Comp = 200; _Tens = 300;
        }
        /// <summary>
        /// ָ����Ԫ���޳�ϸ��
        /// </summary>
        /// <param name="num">��Ԫ��</param>
        /// <param name="com">��ѹ��ϸ��</param>
        /// <param name="ten">������ϸ��</param>
        public BLimitsRatio(int num, double com, double ten)
        {
            _iEle = num;
            _bNotCheck = false;
            _Comp = com; _Tens = ten;
        }
        #endregion
    }
    #endregion

    #region �������ݽṹ
    /// <summary>
    /// �ṹ��������[2010.8.10]
    /// </summary>
    [Serializable]
    public class BSGroup
    {
        #region ��Ա
        private string _GroupName;//������
        private List<int> _NodeList;//�ڵ��б�
        private List<int> _EleList;//��Ԫ�б�
        #endregion

        #region ����
        /// <summary>
        /// ������
        /// </summary>
        public string GroupName
        {
            get { return _GroupName; }
            set { _GroupName = value; }
        }
        /// <summary>
        /// �ڵ��б�
        /// </summary>
        public List<int> NodeList
        {
            get { return _NodeList; }
        }
        /// <summary>
        /// ��Ԫ�б�
        /// </summary>
        public List<int> EleList
        {
            get { return _EleList; }
        }
        /// <summary>
        /// �ڵ��б�(�ö��Ÿ�ʽ��)
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
        /// ��Ԫ�б�(�ö��Ÿ�ʽ��)��
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
        #region ���캯��
        /// <summary>
        /// �޲�����ʼ��
        /// </summary>
        public BSGroup()
        {
            _GroupName = null;
            _NodeList = new List<int>();
            _EleList = new List<int>();
        }
        /// <summary>
        /// ָ��������ʼ��
        /// </summary>
        /// <param name="Name">������</param>
        public BSGroup(string Name)
        {
            _GroupName = Name;
            _NodeList = new List<int>();
            _EleList = new List<int>();
        }
        #endregion
        #region ����
        /// <summary>
        /// ��ӽڵ��б�����
        /// </summary>
        /// <param name="NewList">�ڵ��б�</param>
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

            _NodeList.Sort();//����
        }

        /// <summary>
        /// ��ӵ�Ԫ�б�����
        /// </summary>
        /// <param name="NewList">��Ԫ�б�</param>
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

            _EleList.Sort();//����
        }
        #endregion
    }
    #endregion

    #region ���û�����������
    /// <summary>
    /// ʵ��hash���ظ�����Ա�����
    /// </summary>
    [Serializable]
    public class RepeatedKeySort : IComparer<int>
    {
        #region IComparer ��Ա
        public int Compare(int x, int y)
        {
            //return -1;//ֱ�ӷ��ز���������
            //���´����ʵ���Զ�����
            int iResult = x - y;
            if (iResult == 0) iResult = -1;
            return iResult;
        }
        #endregion
    }
    /// <summary>
    /// ʵ��SortedList���Զ�������
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
            //����
            // int iResult = (int)x - (int)y;
            // if(iResult == 0) iResult = -1;
            // return iResult;
        }
    }
    #endregion 
}
