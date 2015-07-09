using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MidasGenModel.model;

namespace AutoLoadCombination
{
    /// <summary>
    /// 荷载工部有关各种系数类，用于存储分项系数、组合值系数、准永久值系数等
    /// </summary>
    class LCFactor
    {
        private DataTable _FTable;
        private double _Rg_DL;//不控制时恒载分项系数（不利）
        /// <summary>
        /// 不控制时恒载分项系数（不利）
        /// </summary>
        public double Rg_DL
        {
            get { return _Rg_DL; }
            set { _Rg_DL = value; }
        }

        private double _Rgn_DL;//不控制时恒载分项系数(有利)
        /// <summary>
        /// 不控制时恒载分项系数(有利)
        /// </summary>
        public double Rgn_DL
        {
            get { return _Rgn_DL; }
            set { _Rgn_DL = value; }
        }
        /// <summary>
        /// 默认构造函数，按规范生成默认的系数
        /// </summary>
        public LCFactor()
        {
            _Rg_DL = 1.2;
            _Rgn_DL = 1.0;

            _FTable = new DataTable();
            _FTable.Columns.Add("PartialF_ctr",System.Type.GetType("System.Double"));//控制时分项系数列
            _FTable.Columns.Add("PartialF", System.Type.GetType("System.Double"));//不控制时分项系数列（恒载项取1.2）
            _FTable.Columns.Add("CombinationF", System.Type.GetType("System.Double"));//组合值系数列
            _FTable.Columns.Add("Lamd_LL", System.Type.GetType("System.Double"));//活载使用年限调整系数

            DataRow dr = _FTable.NewRow();//恒载
            dr["PartialF_ctr"] = 1.35;
            dr["PartialF"] = 1.2;
            dr["CombinationF"] = 1.0;
            dr["Lamd_LL"] = 1.0;
            _FTable.Rows.Add(dr);

            dr = _FTable.NewRow();//活载
            dr["PartialF_ctr"] = 1.4;
            dr["PartialF"] = 1.0;
            dr["CombinationF"] = 0.7;
            dr["Lamd_LL"] = 1.0;
            _FTable.Rows.Add(dr);

            dr = _FTable.NewRow();//风载
            dr["PartialF_ctr"] = 1.4;
            dr["PartialF"] = 1.0;
            dr["CombinationF"] = 0.6;
            dr["Lamd_LL"] = 1.0;
            _FTable.Rows.Add(dr);

            dr = _FTable.NewRow();//温度
            dr["PartialF_ctr"] = 1.4;
            dr["PartialF"] = 1.0;
            dr["CombinationF"] = 0.6;
            dr["Lamd_LL"] = 1.0;
            _FTable.Rows.Add(dr);

        }

        #region 方法
        /// <summary>
        /// 查得控制时的分项系数
        /// </summary>
        /// <param name="type"></param>
        /// <returns>系数值</returns>
        public double getPartialF_ctr(LCType type)
        {
            int i = this.LCindex(type);//取得行索引
            return (double)_FTable.Rows[i]["PartialF_ctr"];
        }

        /// <summary>
        /// 查得活载使用年限调整系数
        /// </summary>
        /// <param name="type"></param>
        /// <returns>系数值</returns>
        public double getLamd_LL(LCType type)
        {
            int i = this.LCindex(type);//取得行索引
            return (double)_FTable.Rows[i]["Lamd_LL"];
        }
        /// <summary>
        /// 查得组合值系数
        /// </summary>
        /// <param name="type"></param>
        /// <returns>系数值</returns>
        public double getCombinationF(LCType type)
        {
            int i = this.LCindex(type);//取得行索引
            return (double)_FTable.Rows[i]["CombinationF"];
        }


        /// <summary>
        /// 由工况类型取得行数的对应关系
        /// </summary>
        /// <param name="type">工况类型</param>
        /// <returns>表中的行索引</returns>
        public int LCindex(LCType type)
        {
            int LCindex=0;
            switch (type)
            {
                case LCType.D:LCindex=0;break;
                case LCType.L:LCindex=1;break;
                case LCType.W: LCindex = 2; break;
                case LCType.T: LCindex = 3; break;
                default:LCindex=0;break;
            }
            return LCindex;
        }
        #endregion
    }
}
