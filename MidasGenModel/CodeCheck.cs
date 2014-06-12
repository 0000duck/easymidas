using System;
using System.Collections.Generic;
using System.Text;
using MidasGenModel.model;
using MidasGenModel.Geometry3d;
using System.IO;
using System.Collections;

namespace MidasGenModel.Design
{
    /// <summary>
    /// 截面设计基本类
    /// （主要为查表，计算设计参数的静态方法）
    /// </summary>
    public class CodeCheck
    {
        /// <summary>
        /// 计算验算点的压弯组合构件强度 GB 50017-2003 式(5.2.1) 
        /// </summary>
        /// <param name="NL">截面内力</param>
        /// <param name="Sec">截面</param>
        /// <param name="iPt">截面中的验算点号</param>
        /// <param name="DPs">截面验算参数</param>
        /// <returns>强度应力(拉为正，压为负)</returns>
        public static double CalPointStrength_YW(SecForce NL,BSections Sec,int iPt,DesignParameters DPs)
        {
            double y, z,Res;
            if (Sec is SectionDBuser)
            {
                Sec.getCheckPoint(iPt, out y, out z);//取得截面验算点坐标
            }
            else if (Sec is SectionGeneral)//如果为自定截面则验算所有外轮廓点
            {
                SectionGeneral GenSec = Sec as SectionGeneral;
                y = GenSec.OPOLY[iPt].X;
                z = GenSec.OPOLY[iPt].Y;
            }
            else
            {
                y = 0; z = 0;
            }
            
            //如果验算点为0,0,则直接返回0应力值
            if (y == 0 && z == 0)
            {
                return 0;
            }
            double Wy=Sec.Iyy/z;//抗弯截面模量
            double Wz=Sec.Izz/y;//抗弯截面模量
            double Wny = Wy * DPs.Ratio_Anet;//净截面模量
            double Wnz = Wz * DPs.Ratio_Anet;//净截面模量
            double My=-(NL.My);//弯矩，取内力的反号:参Midsa手册02 P16页梁单元内力输出方向
            double Mz=-(NL.Mz);

            Res = NL.N / (Sec.Area * DPs.Ratio_Anet) +
                My / (DPs.Gamma_y * Wny) + Mz / (DPs.Gamma_z * Wnz);
            return Res;
        }
        /// <summary>
        /// 计算截面验算点上的强度最大值（绝对值）
        /// </summary>
        /// <param name="NL">截面内力</param>
        /// <param name="Sec">截面对像</param>
        /// <param name="DPs">单元设计参数</param>
        /// <returns>截面强度应力值（绝对值）</returns>
        public static double CalSecMaxStrength_YW(SecForce NL, BSections Sec, DesignParameters DPs)
        {
            double Res = 0;
            List<double> slist = new List<double>();
            int count=4;
            if (Sec is SectionGeneral)
            {
                SectionGeneral secg = Sec as SectionGeneral;
                count = secg.OPOLY.Length;
            }
            else
            {
                count = 4;
            }
            //添加到结果表
            for (int i = 0; i < count; i++)
            {
                slist.Add(CodeCheck.CalPointStrength_YW(NL, Sec, i, DPs));
            }

            //求得集合中的最大值
            foreach (double ss in slist)
            {
                Res = Math.Max(Res, Math.Abs(ss));
            }
            return Res;
        }

        /// <summary>
        /// 计算四个最大外边角点压弯组合构件稳定性 GB 50017-2003 式(5.2.5-1) (5.2.5-2)
        /// 注：目前默认支持单位为：N,m
        /// </summary>
        /// <param name="NL">截面内力</param>
        /// <param name="Sec">截面参数</param>
        /// <param name="DPs">设计参数</param>
        /// <param name="E">材料的弹性模量</param>
        /// <returns>稳定验算应力值(均为正值)</returns>
        public static double CalStability_YW(SecForce NL, BSections Sec, DesignParameters DPs,
            double E)
        {
            double Res1, Res2,RES;//分别对应(5.2.5-1) (5.2.5-2)式结果
            double Wy, Wz;//抗弯截面模量
            if (NL.My >= 0)
            {
                Wy = Sec.Iyy / Sec.CzP;//+z侧受压
            }
            else
            {
                Wy = Sec.Iyy / Sec.CzM;//-z侧受压
            }
            if (NL.Mz >= 0)
            {
                Wz = Sec.Izz / Sec.CyP;//+y侧受压
            }
            else
            {
                Wz = Sec.Izz / Sec.CyM;//-y侧受压
            }
            double N = NL.N;//轴向力
            double My = Math.Abs(NL.My);//弯矩，取绝对值
            double Mz = Math.Abs(NL.Mz);

            //参数
            double N_ey = Math.Pow(Math.PI, 2) * E * Sec.Area / (1.1 * Math.Pow(DPs.Lemda_y, 2));
            double N_ez = Math.Pow(Math.PI, 2) * E * Sec.Area / (1.1 * Math.Pow(DPs.Lemda_z, 2));

            double Ref_Ny = 1;//与轴压力有关的参数
            double Ref_Nz = 1;
            if (N < 0)//如为轴压力对参数进行调整
            {
                Ref_Ny = 1 - 0.8 * Math.Abs(N) / N_ey;
                Ref_Nz = 1 - 0.8 * Math.Abs(N) / N_ez;
            }

            //进行功能计算，注意弯矩后两项应为-号
            Res1 = N / (DPs.Phi_y * Sec.Area) - DPs.Belta_my * My / (DPs.Gamma_y * Wy * Ref_Ny) -
                DPs.Yita * DPs.Belta_tz * Mz / (DPs.Phi_bz * Wz);
            Res2 = N / (DPs.Phi_z * Sec.Area) -DPs.Yita * DPs.Belta_ty * My / (DPs.Phi_by * Wy)
                - DPs.Belta_mz * Mz / (DPs.Gamma_z * Wz * Ref_Nz);

            RES = Math.Max(Math.Abs(Res1), Math.Abs(Res2));
            return RES;
        }

        /// <summary>
        /// 计算验算点的稳定性应力 GB 50017-2003 式(5.2.5-1) (5.2.5-2)
        /// </summary>
        /// <param name="NL">截面内力</param>
        /// <param name="Sec">截面</param>
        /// <param name="Pt">截面验算点，以截面形心为原点</param>
        /// <param name="DPs">截面验算参数</param>
        /// <param name="E">材料弹性模量</param>
        /// <returns>稳定应力(为正或0)</returns>
        public static double CalPointStability(SecForce NL, BSections Sec, Point2d Pt, DesignParameters DPs,double E)
        {
            double y, z, Res,Sta1,Sta2;
            y = Pt.X; z = Pt.Y;
            
            double N = NL.N;//轴向力
            double My = NL.My;//弯矩
            double Mz = NL.Mz;

            //参数
            double N_ey = Math.Pow(Math.PI, 2) * E * Sec.Area / (1.1 * Math.Pow(DPs.Lemda_y, 2));
            double N_ez = Math.Pow(Math.PI, 2) * E * Sec.Area / (1.1 * Math.Pow(DPs.Lemda_z, 2));

            double Ref_Ny = 1;//与轴压力有关的参数
            double Ref_Nz = 1;
            if (N < 0)//如为轴压力对参数进行调整
            {
                Ref_Ny = 1 - 0.8 * Math.Abs(N) / N_ey;
                Ref_Nz = 1 - 0.8 * Math.Abs(N) / N_ez;
            }

            //进行功能计算，注意弯矩后两项应为-号
            Sta1 = N / (DPs.Phi_y * Sec.Area) - DPs.Belta_my * My*z / (DPs.Gamma_y * Sec.Iyy * Ref_Ny) -
                DPs.Yita * DPs.Belta_tz * Mz*y / (DPs.Phi_bz * Sec.Izz);
            Sta2 = N / (DPs.Phi_z * Sec.Area) - DPs.Yita * DPs.Belta_ty * My*z / (DPs.Phi_by * Sec.Iyy)
                - DPs.Belta_mz * Mz *y/ (DPs.Gamma_z * Sec.Izz * Ref_Nz);

            //取控制值
            Res = Math.Min(Sta1, Sta2);
            //如果为拉应力则返回0，如果为压应力反回正的应力值
            if (Res > 0)
            {
                return 0;
            }
            else
            {
                return Math.Abs(Res);
            }
        }

        /// <summary>
        /// 计算截面所有验算点的控制稳定应力
        /// </summary>
        /// <param name="NL">截面内力</param>
        /// <param name="Sec">截面对像</param>
        /// <param name="DPs">单元设计参数</param>
        /// <param name="E">材料弹性模量</param>
        /// <returns>截面稳定应力值</returns>
        public static double CalSecMaxStability_YW(SecForce NL, BSections Sec, DesignParameters DPs,double E)
        {
            double Res = 0;
            List<double> slist = new List<double>();
            Point2dCollection pts;
            if (Sec is SectionGeneral)
            {
                SectionGeneral secg = Sec as SectionGeneral;
                pts = secg.OPOLY;
            }
            else
            {
                pts = Sec.CheckPointCollection;
            }
            //添加到结果表
            foreach (Point2d pt in pts)
            {
                slist.Add(CodeCheck.CalPointStability(NL, Sec, pt, DPs,E));
            }

            //求得集合中的最大值
            foreach (double ss in slist)
            {
                Res = Math.Max(Res, ss);
            }
            return Res;
        }

        /// <summary>
        /// 输出单个单元的验算结果表格
        /// </summary>
        /// <param name="mm">模型对像</param>
        /// <param name="FileOut">输出文件路径</param>
        /// <param name="cr">截面验算结果</param>
        /// <param name="iElem">单元号</param>
        public static void WriteElemCheckRes(ref Bmodel mm,ref CheckRes cr, string FileOut, int iElem)
        {
            FileStream stream = File.Open(FileOut, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine("单元号\t截面位置\t荷载组合\tN(kN)\tMy(kN*m)\tMz(kN*m)\t强度(MPa)\t稳定(MPa)\t应力比");

            List<string> coms = mm.LoadCombTable.ComSteel;
            foreach (string com in coms)
            {
                //如果组合未激活，则跳过
                BLoadComb curComb = mm.LoadCombTable.getLoadComb(LCKind.STEEL, com);
                if (curComb.bACTIVE == false)
                    continue;

                SingleEleCheckResData serd = cr.CheckResTable[iElem].GetCheckResByCom(com);
                writer.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}", iElem.ToString(), serd.Sec_contral, com,
                    (serd.N / 1e3).ToString("0.0"), (serd.My/1e3).ToString("0.0"),
                    (serd.Mz/1e3).ToString("0.0"), (serd.Strength/1e6).ToString("0.0"),
                    (serd.Stability/1e6).ToString("0.0"),serd.Ratio.ToString("0.00"));
                //writer.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}", iElem.ToString(), "1/2", com,
                //    EFcom.Force_48.N.ToString("0.0"), EFcom.Force_48.My.ToString("0.0"),
                //    EFcom.Force_48.Mz.ToString("0.0"), Strength_2.ToString("0.0"),
                //    Stability_2.ToString("0.0"), Ratio_2.ToString("0.00"));
                //writer.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}", iElem.ToString(), "J", com,
                //    EFcom.Force_j.N.ToString("0.0"), EFcom.Force_j.My.ToString("0.0"),
                //    EFcom.Force_j.Mz.ToString("0.0"), Strength_j.ToString("0.0"),
                //    Stability_j.ToString("0.0"), Ratio_j.ToString("0.00"));
            }
            //输出控制组合
            writer.WriteLine("\n\n******控制组合*******: {0}", cr.CheckResTable[iElem].GetControlData().ComName);
            writer.Close();
            stream.Close();
        }

        /// <summary>
        /// 按截面输出验算结构表格
        /// </summary>
        /// <param name="mm">模型对像</param>
        /// <param name="FileOut">输出文件路径</param>
        /// <param name="iSec">截面号</param>
        public static void WriteSecCheckRes(ref Bmodel mm, ref CheckRes cr,string FileOut, int iSec)
        {
            List<int> Elems = mm.getElemBySec(iSec);//单元号表
            FileStream stream = File.Open(FileOut, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine("截面名称\t单元号（控制位置）\t控制组合\tN(kN)\tMy(kN*m)\tMz(kN*m)\t强度(MPa)\t稳定(MPa)\t应力比");
            
            foreach (int ele in Elems)
            {
                SingleEleCheckResData SECR=cr.CheckResTable[ele].GetControlData();
                writer.WriteLine(mm.sections[iSec].Name+"\t"+ele.ToString()+"("+SECR.Sec_contral+")\t"+
                    SECR.ComName+"\t"+(SECR.N/1e3).ToString("0.0")+"\t"+(SECR.My/1e3).ToString("0.0")+
                    "\t"+(SECR.Mz/1e3).ToString("0.0")+"\t"+
                    (SECR.Strength/1e6).ToString("0.0")+"\t"+(SECR.Stability/1e6).ToString("0.0")+"\t"+
                    SECR.Ratio.ToString("0.00"));
            }

            writer.WriteLine("\n\n*********控制单元号**********: {0}",cr.GetControlElem(Elems).ToString());
            writer.Close();
            stream.Close();
        }

        /// <summary>
        /// 输出所有截面的验算结果
        /// 按截面进行归并
        /// </summary>
        /// <param name="mm">模型对像</param>
        /// <param name="cr">验算结果数据对像</param>
        /// <param name="FileOut">输出文件路径</param>
        public static void WriteAllCheckRes(ref Bmodel mm,ref CheckRes cr,string FileOut)
        {
            FileStream stream = File.Open(FileOut, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine("截面名称\t控制单元号（截面）\t控制组合\tN(kN)\tMy(kN*m)\tMz(kN*m)\t强度(MPa)\t稳定(MPa)\t应力比\t计算长度y(m)\t计算长度z(m)\t长细比y\t长细比z");
            
            foreach (BSections sec in mm.sections.Values)
            {
                List<int> curElems = cr.GetElemsBySec(ref mm,sec.Num);//当前截面信息
                if (curElems.Count==0)
                    continue;
                int num_control=cr.GetControlElem(curElems);//控制单元号

                FrameElement conEle=mm.elements[num_control] as FrameElement;//控制单元
                double Ly=0;double Lz=0;
                mm.getEleLyLz(num_control,out Ly,out Lz);//取得计算长度

                SingleEleCheckResData secrd = cr.CheckResTable[num_control].GetControlData();

                writer.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}",
                    sec.Name,num_control.ToString()+"("+secrd.Sec_contral+")",
                    secrd.ComName,(secrd.N/1e3).ToString("0.0"),
                    (secrd.My/1e3).ToString("0.0"),(secrd.Mz/1e3).ToString("0.0"),
                    (secrd.Strength/1e6).ToString("0.0"),(secrd.Stability/1e6).ToString("0.0"),
                    secrd.Ratio.ToString("0.00"),
                    Ly.ToString("0.0"),Lz.ToString("0.0"),//计算长度
                    conEle.DPs.Lemda_y.ToString("0"),conEle.DPs.Lemda_z.ToString("0")//长细比
                    );
            }

            writer.Close();
            stream.Close();
        }

        /// <summary>
        /// 输出所有截面的验算结果
        /// 每个单元输出一行
        /// 2013.12.21
        /// </summary>
        /// <param name="mm">模型对像</param>
        /// <param name="cr">验算结果数据</param>
        /// <param name="FileOut">输出文件路径</param>
        public static void WriteAllCheckRes2(ref Bmodel mm, ref CheckRes cr, string FileOut)
        {
            FileStream stream = File.Open(FileOut, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine("单元号\t截面号\t控制组合\tN(kN)\tMy(kN*m)\tMz(kN*m)\t强度(MPa)\t稳定(MPa)\t应力比\t计算长度y(m)\t计算长度z(m)\t长细比y\t长细比z");

            if (cr.CheckResTable.Count == 0)//如果无验算数据则提示输出
            {
                writer.WriteLine("*********无验算数据*************");
                writer.Close();
                stream.Close();
                return;
            }

            foreach (int iEle in cr.CheckResTable.Keys)
            {
                FrameElement conEle = mm.elements[iEle] as FrameElement;//控制单元
                double Ly = 0; double Lz = 0;
                mm.getEleLyLz(iEle, out Ly, out Lz);//取得计算长度

                SingleEleCheckResData secrd = cr.CheckResTable[iEle].GetControlData();

                writer.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}",
                    iEle, conEle.iPRO,
                    secrd.ComName, (secrd.N / 1e3).ToString("0.0"),
                    (secrd.My / 1e3).ToString("0.0"), (secrd.Mz / 1e3).ToString("0.0"),
                    (secrd.Strength / 1e6).ToString("0.0"), (secrd.Stability / 1e6).ToString("0.0"),
                    secrd.Ratio.ToString("0.00"),
                    Ly.ToString("0.0"), Lz.ToString("0.0"),//计算长度
                    conEle.DPs.Lemda_y.ToString("0"), conEle.DPs.Lemda_z.ToString("0")//长细比
                    );
            }

            writer.Close();
            stream.Close();
        }

        /// <summary>
        /// 输出所有截面验算参数设置
        /// </summary>
        /// <param name="mm"></param>
        /// <param name="cr"></param>
        /// <param name="FileOut"></param>
        public static void WriteCheckPara(ref Bmodel mm, ref CheckRes cr, string FileOut)
        {
            FileStream stream = File.Open(FileOut, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine("截面号\t截面名称\t平面内计算长度\t平面外计算长度\t材料设计强度\t净毛面积比\t塑性发展系数γx\t"+
                "塑性发展系数γy\t等效弯矩系数βmx\t等效弯矩系数βmy\t等效弯矩系数βtx\t等效弯矩系数βty\t"+
                "受弯稳定系数ψbx\t受弯稳定系数ψby\t平面内长细比\t平面外长细比"+
                "\t截面类别x\t截面类别y\t抗震承力调整系数γre1\t抗震承力调整系数γre2\t截面影响系数η");

            foreach (BSections sec in mm.sections.Values)
            {
                List<int> tempElem = mm.getElemBySec(sec.Num);
                if (tempElem.Count == 0)
                    continue;
                int eNum=tempElem[0];
                double eLeng=mm.getFrameLength(eNum);
                FrameElement fe = mm.elements[eNum] as FrameElement;
                DesignParameters DP = fe.DPs;
                double ly=DP.Lk_y*eLeng;
                double lz=DP.Lk_z*eLeng;
                writer.Write("{0}\t{1}\t{2}\t{3}",sec.Num.ToString(),sec.Name,ly.ToString("0.00"),lz.ToString("0.00"));
                writer.Write("\t{0}\t{1}\t{2}",DP.fy.ToString("0"),DP.Ratio_Anet.ToString("0.00"),DP.Gamma_y.ToString("0.00"));
                writer.Write("\t{0}\t{1}\t{2}",DP.Gamma_z.ToString("0.00"),DP.Belta_my.ToString("0.00"),
                    DP.Belta_mz.ToString("0.00"));
                writer.Write("\t{0}\t{1}",DP.Belta_ty.ToString("0.00"),DP.Belta_tz.ToString("0.00"));
                writer.Write("\t{0}\t{1}", DP.Phi_by.ToString("0.00"),DP.Phi_bz.ToString("0.00"));
                writer.Write("\t{0}\t{1}",DP.Lemda_y.ToString("0.00"),DP.Lemda_z.ToString("0.00") );
                writer.Write("\t{0}\t{1}\t{2}\t{3}",DP.SecCat_y.ToString(),DP.SecCat_z.ToString(),
                    DP.Gamma_re_1.ToString("0.00"),DP.Gamma_re_2.ToString("0.00"));
                writer.Write("\t{0}",DP.Yita.ToString("0.00"));
                writer.Write("\n");
            }

            writer.Close();
            stream.Close();
        }

        /// <summary>
        /// 读入截面验算的设置参数
        /// </summary>
        /// <param name="mm"></param>
        /// <param name="cr"></param>
        /// <param name="FileIn"></param>
        public static void ReadCheckPara(ref Bmodel mm, ref CheckRes cr, string FileIn)
        {
            string line = null;//行文本
            string[] curdata = null;//当前行数据变量
            int curNum = 0;//当前截面号

            FileStream stream = File.Open(FileIn, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);
            line = reader.ReadLine();

            for (line = reader.ReadLine(); line != null; line = reader.ReadLine())
            {
                curdata = line.Split("\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);//字符串分割
                curNum = Convert.ToInt32(curdata[0]);//当前截面号

                List<int> eles = mm.getElemBySec(curNum);//当前截面的单元集
                foreach (int ele in eles)
                {
                    if (!(mm.elements[ele] is FrameElement))
                        continue;
                    FrameElement fe = mm.elements[ele] as FrameElement;

                    double len_y = Convert.ToDouble(curdata[2]);
                    double len_z = Convert.ToDouble(curdata[3]);
                    double Net_r = Convert.ToDouble(curdata[5]);
                    double Gamma_y = Convert.ToDouble(curdata[6]);
                    double Gamma_z = Convert.ToDouble(curdata[7]);
                    double Betla_my = Convert.ToDouble(curdata[8]);
                    double Betal_mz = Convert.ToDouble(curdata[9]);
                    double Betal_ty = Convert.ToDouble(curdata[10]);
                    double Betal_tz = Convert.ToDouble(curdata[11]);
                    double Phi_by = Convert.ToDouble(curdata[12]);
                    double Phi_bz = Convert.ToDouble(curdata[13]);
                    double F = Convert.ToDouble(curdata[4]);//强度设计值
                    double Gamma_re1 = Convert.ToDouble(curdata[18]);//承载力调整系数
                    double Gamma_re2 = Convert.ToDouble(curdata[19]);
                    SecCategory cat1=(SecCategory)Enum.Parse(typeof(SecCategory),curdata[16]);
                    SecCategory cat2 = (SecCategory)Enum.Parse(typeof(SecCategory), curdata[17]);

                    double Yita = Convert.ToDouble(curdata[20]);//截面影响系数

                    fe.DPs.SecCat_y = cat1;
                    fe.DPs.SecCat_z=cat2;
                    fe.DPs.Ratio_Anet = Net_r;
                    fe.DPs.Gamma_y = Gamma_y;
                    fe.DPs.Gamma_z = Gamma_z;
                    fe.DPs.Belta_my = Betla_my;
                    fe.DPs.Belta_mz = Betal_mz;
                    fe.DPs.Belta_ty = Betal_ty;
                    fe.DPs.Belta_tz = Betal_tz;
                    fe.DPs.fy = F;//强度设计值
                    fe.DPs.Gamma_re_1 = Gamma_re1;
                    fe.DPs.Gamma_re_2 = Gamma_re2;
                    fe.DPs.Yita = Yita;

                    //更新长细比
                    CodeCheck.CalDesignPara_lemda(ref mm, ele, len_y, len_z);
                    //更新受压稳定系数
                    CodeCheck.CalDesignPara_phi(ref mm, ele, 1);
                    CodeCheck.CalDesignPara_phi(ref mm, ele, 2);
                }
            }

            reader.Close();
        }
        /// <summary>
        /// 根据用户指定参数刷新模型数据中的单元设计参数
        /// </summary>
        /// <param name="mm">模型数据</param>
        /// <param name="cm">用户指定的设计参数</param>
        /// <param name="iEle">单元号</param>
        public static void RefreshDesignPara(ref Bmodel mm, BCheckModel cm, int iEle)
        {
            FrameElement ele = mm.elements[iEle] as FrameElement;//单元
            BSecDesignPara mySDP = new BSecDesignPara(iEle);
            if (cm.getSecDesignPara(ele.iPRO, out mySDP))//截面相关设计参数
            {
                ele.DPs.Ratio_Anet = mySDP.RatioNet;
                ele.DPs.SecCat_y = mySDP.SecCat_y;
                ele.DPs.SecCat_z = mySDP.SecCat_z;
                ele.DPs.Yita = (mySDP.IsClosed) ? 0.7 : 1.0;
                ele.DPs.Gamma_y = mySDP.Gama_x;
                ele.DPs.Gamma_z = mySDP.Gama_y;
            }
            BEquivalentCoeff myEC=new BEquivalentCoeff (iEle);
            if (cm.getEquBendCoeff(iEle, out myEC))//等效弯矩系数
            {
                ele.DPs.Belta_my = myEC.Beta_my;
                ele.DPs.Belta_mz = myEC.Beta_mz;
                ele.DPs.Belta_ty = myEC.Beta_ty;
                ele.DPs.Belta_tz = myEC.Beta_tz;
            }
            BQuakeFactor myQF=new BQuakeFactor (iEle);//承载力调整系数
            if (cm.getQuakeFactor(iEle, out myQF))
            {
                ele.DPs.Gamma_re_1 = myQF.GamaRe_1;
                ele.DPs.Gamma_re_2 = myQF.GamaRe_2;
            }
        }

        /// <summary>
        /// 对某单元按指定的计算长度计算长细比
        /// </summary>
        /// <param name="mm">模型对像</param>
        /// <param name="iElem">单元号，必须为梁单元FrameElement</param>
        /// <param name="l_0y">平面内计算长度</param>
        /// <param name="l_0z">平面外计算长度</param>
        public static void CalDesignPara_lemda(ref Bmodel mm, int iElem, double l_0y, double l_0z)
        {
            FrameElement ele = mm.elements[iElem] as FrameElement;
            int iSec =ele.iPRO;//截面号
            BSections curSec= mm.sections[iSec];//当前截面
            double i_y = Math.Sqrt(mm.sections[iSec].Iyy / mm.sections[iSec].Area);//回转半径
            double i_z = Math.Sqrt(mm.sections[iSec].Izz / mm.sections[iSec].Area);

            ele.DPs.Lemda_y = l_0y / i_y;//计算长细比
            ele.DPs.Lemda_z = l_0z / i_z;//计算长细比

            //形心到剪心的矩离
            double e0 = Math.Sqrt(Math.Pow(curSec.Sy-curSec.Cy,2)+Math.Pow(curSec.Sz-curSec.Cz,2));
            double i02 = Math.Pow(e0, 2) + Math.Pow(i_y, 2) + Math.Pow(i_z, 2);
            double Lemda_z2 = 0;//扭转屈曲的换算长细比
            //如果截面上下不对称
            if (Math.Abs(curSec.CzM -curSec.CzP)>0.002)
            {
                //扭转屈曲的换算长细比
                Lemda_z2 = i02 * curSec.Area / (curSec.Ixx / 25.7 + curSec.Iw/Math.Pow(l_0z,2));
                double temp1 = Math.Pow(ele.DPs.Lemda_z, 2) + Lemda_z2;
                double temp2 = temp1 + Math.Sqrt(Math.Pow(temp1,2)-4*(1-Math.Pow(e0,2)/i02)*
                    Math.Pow(ele.DPs.Lemda_z,2)*Lemda_z2);
                ele.DPs.Lemda_yz = Math.Sqrt(temp2) / Math.Sqrt(2);
            }
            else if (Math.Abs(curSec.CyM - curSec.CyP) > 0.002)
            {
                //扭转屈曲的换算长细比
                Lemda_z2 = i02 * curSec.Area / (curSec.Ixx / 25.7 + curSec.Iw / Math.Pow(l_0y, 2));
                double temp1 = Math.Pow(ele.DPs.Lemda_y, 2) + Lemda_z2;
                double temp2 = temp1 + Math.Sqrt(Math.Pow(temp1, 2) - 4 * (1 - Math.Pow(e0, 2) / i02) *
                    Math.Pow(ele.DPs.Lemda_y, 2) * Lemda_z2);
                ele.DPs.Lemda_yz = Math.Sqrt(temp2) / Math.Sqrt(2);
            }

            double eleLeng = mm.getFrameLength(iElem);//单元长度
            ele.DPs.Lk_y = l_0y/eleLeng;//计算长度系数（单元长度的倍数）
            ele.DPs.Lk_z = l_0z/eleLeng;
        }
        /// <summary>
        /// 根据用户指定计算长度更新单元的长细比
        /// </summary>
        /// <param name="bmm">更新模型</param>
        /// <param name="iElem">单元号</param>
        public static void CalDesignPara_lemda(ref Bmodel bmm, int iElem)
        {
            double ly = 0;
            double lz = 0;
            bmm.getEleLyLz(iElem, out ly, out lz);//取得模型中对应单元的计算长度
            CalDesignPara_lemda(ref bmm, iElem, ly, lz);//按指定长度计算
        }

        /// <summary>
        /// 计算受压构件的稳定系数phi
        /// GB50017 附表C公式
        /// </summary>
        /// <param name="mm">模型对像</param>
        /// <param name="iElem">单元号</param>
        /// <param name="iYZ">指示计算当前截面哪个方向的稳定系数 1：phi_y;2:phi_z</param>
        public static void CalDesignPara_phi(ref Bmodel mm, int iElem,int iYZ)
        {
            FrameElement ele = mm.elements[iElem] as FrameElement;
            int iSec = ele.iPRO;//截面号
            double E = mm.mats[ele.iMAT].Elast;//弹性模量
            ///此处取名义屈服强度，实际的屈服强度还与板厚有关
            ///随着厚度的增加，屈服强度有所降低
            ///此部分因素目前程序未考虑---2013.12.17
            double Fy = mm.mats[ele.iMAT].Fy;//屈服强度

            SecCategory Cat = SecCategory.b;
            double lemda=0;
            double a1=0;
            double a2=0;
            double a3=0;
            double phi=1;//稳定系数
            if (iYZ==1)
            {
                //如果截面左右不对称
                if (Math.Abs(mm.sections[iSec].CyM - mm.sections[iSec].CyP) > 0.002)
                {
                    lemda = ele.DPs.Lemda_yz;
                }
                else
                {
                    lemda = ele.DPs.Lemda_y;
                }
                Cat = ele.DPs.SecCat_y;//取得当前单元的截面类别
            }
            else if (iYZ ==2)
            {
                //如果截面上下不对称
                if (Math.Abs(mm.sections[iSec].CzM - mm.sections[iSec].CzP) > 0.002)
                {
                    lemda = ele.DPs.Lemda_yz;
                }
                else
                {
                    lemda = ele.DPs.Lemda_z;
                }
                Cat = ele.DPs.SecCat_z;//取得当前单元的截面类别
            }

            double lemda_n = lemda * Math.Sqrt(Fy / E) / Math.PI;//正则长细比
            
            TableC_5(Cat,lemda_n,out a1,out a2,out a3);//查表C-5

            if (lemda_n <= 0.215)
            {
               phi=1-a1*Math.Pow(lemda_n,2);
            }
            else 
            {
                double temp1=a2+a3*lemda_n+Math.Pow(lemda_n,2);
                double temp2=Math.Pow(lemda_n,2);
                phi=(temp1-Math.Sqrt(Math.Pow(temp1,2)-4*temp2))/(2*temp2);
            }

            //存储
            if (iYZ == 1)
            {
                ele.DPs.Phi_y = phi;
            }
            else if (iYZ ==2)
            {
                ele.DPs.Phi_z = phi;
            }
        }

        /// <summary>
        /// 查附录表C-5中的三个系数
        /// </summary>
        /// <param name="Cat">截面类别</param>
        /// <param name="lemda_n">正则长细比</param>
        /// <param name="alph1">参数</param>
        /// <param name="alph2">参数</param>
        /// <param name="alph3">参数</param>
        public static void TableC_5(SecCategory Cat,double lemda_n,out double alph1,out double alph2,
            out double alph3)
        {
            switch (Cat)
            {
                case SecCategory.a:
                    alph1 = 0.41; alph2 = 0.986; alph3 = 0.152; break;
                case SecCategory .b:
                    alph1 = 0.65; alph2 = 0.965; alph3 = 0.3; break;
                case SecCategory .c:
                    if (lemda_n <= 1.05)
                    {
                        alph1 = 0.73; alph2 = 0.906; alph3 = 0.595; break;
                    }
                    else
                    {
                        alph1 = 0.73; alph2 = 1.216; alph3 = 0.302; break;
                    }
                case SecCategory .d:
                    if (lemda_n <= 1.05)
                    {
                        alph1 =1.35; alph2 = 0.868; alph3 = 0.915; break;
                    }
                    else
                    {
                        alph1 = 1.35; alph2 = 1.375; alph3 = 0.432; break;
                    }
                default:
                    alph1 = 0.65; alph2 = 0.965; alph3 = 0.3; break;
            }
        }
    }
    /// <summary>
    /// 钢结构主要设计参数类
    /// </summary>
    [Serializable]
    public class DesignParameters
    {
        private double _Ratio_Anet;//净毛面积比
        private double _Gamma_y;//截面塑性发展系数
        private double _Gamma_z;//截面塑性发展系数
        private double _Belta_my, _Belta_mz, _Belta_ty, _Belta_tz;//等效弯矩系数
        private double _Phi_by,_Phi_bz;//受弯构件整体稳定性系数
        private double _Phi_y, _Phi_z;//受压构件稳定系数
        private double _lemda_y, _lemda_z;//长细比
        private double _lemda_yz;//换算长细比 GB50017-2003 式(5.1.2-3)
        private double _lk_y, _lk_z;//计算长度系数
        private double _Yita;//截面影响系数GB50017-2003 P48 ：闭口截面取0.7,其它截面取1.0

        private double _fy;//抗拉，抗压强度设计值
        private double _Gamma_re_1,_Gamma_re_2;//承载力抗震调整系数:强度，稳定
        private SecCategory _SecCat_y;//截面类别x轴
        private SecCategory _SecCat_z;//截面类别z轴

        #region 属性
        /// <summary>
        /// 净毛面积比
        /// </summary>
        public double Ratio_Anet
        {
            get { return _Ratio_Anet; }
            set { _Ratio_Anet = value; }
        }
        /// <summary>
        /// 截面塑性发展系数
        /// </summary>
        public double Gamma_y
        {
            get { return _Gamma_y; }
            set { _Gamma_y = value; }
        }
        /// <summary>
        /// 截面塑性发展系数
        /// </summary>
        public double Gamma_z
        {
            get { return _Gamma_z; }
            set { _Gamma_z = value; }
        }

        /// <summary>
        /// 等效弯矩系数
        /// </summary>
        public double Belta_my
        {
            get { return _Belta_my; }
            set { _Belta_my = value; }
        }
        /// <summary>
        /// 等效弯矩系数
        /// </summary>
        public double Belta_mz
        {
            get { return _Belta_mz; }
            set { _Belta_mz = value; }
        }
        /// <summary>
        /// 等效弯矩系数
        /// </summary>
        public double Belta_ty
        {
            get { return _Belta_ty; }
            set { _Belta_ty = value; }
        }
        /// <summary>
        /// 等效弯矩系数
        /// </summary>
        public double Belta_tz
        {
            get { return _Belta_tz; }
            set { _Belta_tz = value; }
        }

        /// <summary>
        /// 受弯构件整体稳定性系数
        /// </summary>
        public double Phi_by
        {
            get { return _Phi_by; }
            set { _Phi_by = value; }
        }
        /// <summary>
        /// 受弯构件整体稳定性系数
        /// </summary>
        public double Phi_bz
        {
            get { return _Phi_bz; }
            set { _Phi_bz = value; }
        }
        /// <summary>
        /// 受压构件稳定系数
        /// </summary>
        public double Phi_y
        {
            get { return _Phi_y; }
            set { _Phi_y = value; }
        }
        /// <summary>
        /// 受压构件稳定系数
        /// </summary>
        public double Phi_z
        {
            get { return _Phi_z; }
            set { _Phi_z = value; }
        }
        /// <summary>
        /// 长细比
        /// </summary>
        public double Lemda_y
        {
            get { return _lemda_y; }
            set { _lemda_y = value; }
        }
        /// <summary>
        /// 长细比
        /// </summary>
        public double Lemda_z
        {
            get { return _lemda_z; }
            set { _lemda_z = value; }
        }
        /// <summary>
        /// 换算长细比 GB50017-2003 式(5.1.2-3)
        /// </summary>
        public double Lemda_yz
        {
            get { return _lemda_yz; }
            set { _lemda_yz = value; }
        }
        /// <summary>
        /// 计算长度系数
        /// </summary>
        public double Lk_y
        {
            get { return _lk_y; }
            set { _lk_y = value; }
        }
        /// <summary>
        /// 计算长度系数
        /// </summary>
        public double Lk_z
        {
            get { return _lk_z; }
            set { _lk_z = value; }
        }
        /// <summary>
        /// 钢材强度设计值（MPa）
        /// </summary>
        public double fy
        {
            get { return _fy; }
            set { _fy = value; }
        }

        /// <summary>
        /// 承载力抗震调整系数:强度
        /// </summary>
        public double Gamma_re_1
        {
            get { return _Gamma_re_1; }
            set { _Gamma_re_1 = value; }
        }
        /// <summary>
        /// 承载力抗震调整系数:稳定
        /// </summary>
        public double Gamma_re_2
        {
            get { return _Gamma_re_2; }
            set { _Gamma_re_2 = value; }
        }

        /// <summary>
        /// 截面影响系数GB50017-2003 P48 ：闭口截面取0.7,其它截面取1.0
        /// </summary>
        public double Yita
        {
            get { return _Yita; }
            set { _Yita = value; }
        }

        //截面类别（x轴）
        public SecCategory SecCat_y
        {
            get { return _SecCat_y; }
            set { _SecCat_y = value; }
        }
        //截面类别(y轴)
        public SecCategory SecCat_z
        {
            get { return _SecCat_z; }
            set { _SecCat_z = value; }
        }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public DesignParameters()
        {
            _Ratio_Anet = 0.85;
            _Gamma_y = 1.0;
            _Gamma_z = 1.0;
            _Belta_my = 1.0;
            _Belta_mz = 1.0;
            _Belta_ty = 1.0;
            _Belta_tz = 1.0;
            _Phi_by = 1;
            _Phi_bz = 1;
            _Phi_y = 1;
            _Phi_z = 1;
            _lemda_y = 50;
            _lemda_z = 50;
            _lemda_yz = 50;
            _lk_y = 1;
            _lk_z = 1;

            _fy = 310e6;
            _Yita = 1.0;
            _Gamma_re_1 = 0.75;
            _Gamma_re_2 = 0.85;
            _SecCat_y = SecCategory.b;
            _SecCat_z = SecCategory.c;
        }

        #region 方法

        /// <summary>
        /// 输出截面设置参数所有信息
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string Res = "";
            Res += "净毛面积比："+_Ratio_Anet.ToString();
            Res+="\n截面塑性发展系数(γy,γz)："+_Gamma_y.ToString()+","+_Gamma_z.ToString();
            Res += "\n等效弯矩系数(βmy,βmz,βty,βtz):" + _Belta_my.ToString()+","+_Belta_mz.ToString()+
                ","+_Belta_ty.ToString()+","+_Belta_tz.ToString();
            Res += "\n受弯构件整体稳定性系数(ψby,ψbz)：" + _Phi_by.ToString() + "," + _Phi_bz.ToString();
            Res += "\n受压构件稳定系数(ψy,ψz):" + _Phi_y.ToString("0.000") + "," + _Phi_z.ToString("0.000");
            Res += string.Format("\n计算长度系数(lk_y,lk_z):{0},{1}", _lk_y.ToString("0.0"), _lk_z.ToString("0.0"));
            Res+="\n长细比(λy,λz,λyz):"+_lemda_y.ToString("0.000")+","+_lemda_z.ToString("0.000")+","+_lemda_yz.ToString("0.000");
            Res += "\n截面影响系数:" + _Yita.ToString();
            Res += string.Format("\n承载力抗震调整系数:强度{0},稳定{1}", _Gamma_re_1.ToString("0.00"), _Gamma_re_2.ToString("0.00"));
            Res += string.Format("\n截面类别:沿x轴[{0}],沿y轴[{1}]",_SecCat_y.ToString(),_SecCat_z.ToString());
            Res += "\n抗拉-抗压强度设计值:" + _fy.ToString();

            return Res;
        }
        #endregion
    }

    /// <summary>
    /// 单元验算结果数据类
    /// </summary>
    [Serializable]
    public class EleCheckResData
    {
        private int _iElem;//单元号
        private Hashtable _SingleEleCheckRes;//按组合排列的单元验算表

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="num">单元号</param>
        public EleCheckResData(int num)
        {
            _iElem = num;
            _SingleEleCheckRes = new Hashtable();
        }

        /// <summary>
        /// 向啥希表中添加验算数据
        /// </summary>
        /// <param name="com">组合名称</param>
        /// <param name="ComData">组合验算结果</param>
        public void Add(string com, SingleEleCheckResData ComData)
        {
            _SingleEleCheckRes.Add(com, ComData);
        }

        /// <summary>
        /// 取得控制验算结果
        /// </summary>
        /// <returns></returns>
        public SingleEleCheckResData GetControlData()
        {
            SingleEleCheckResData Res = new SingleEleCheckResData();
            //对字典进行遍历
            foreach (DictionaryEntry DE in _SingleEleCheckRes)
            {
                Res = Res.TheMaxRatio(DE.Value as SingleEleCheckResData);
            }
            return Res;
        }

        /// <summary>
        /// 由组合名查得验算数据
        /// </summary>
        /// <param name="comb">组合名</param>
        /// <returns>验算数据</returns>
        public SingleEleCheckResData GetCheckResByCom(string comb)
        {
            return _SingleEleCheckRes[comb] as SingleEleCheckResData;
        }
    }

    /// <summary>
    /// 单个单元按组合验算结果数据类
    /// </summary>
    [Serializable]
    public class SingleEleCheckResData
    {
        private string _Sec_contral;
        private string _ComName;
        private double _N;
        private double _My;
        private double _Mz;
        private double _Strength;
        private double _Stability;
        private double _Ratio;

        /// <summary>
        /// 截面位置
        /// </summary>
        public string Sec_contral
        {
            get { return _Sec_contral; }
        }
        /// <summary>
        /// 组合名
        /// </summary>
        public string ComName
        {
            get { return _ComName; }
        }
        /// <summary>
        /// 轴力
        /// </summary>
        public double N
        {
            get { return _N; }
        }
        /// <summary>
        /// 弯矩
        /// </summary>
        public double My
        {
            get { return _My; }
        }
        /// <summary>
        /// 弯矩
        /// </summary>
        public double Mz
        {
            get { return _Mz; }
        }
        /// <summary>
        /// 强度验算应力
        /// </summary>
        public double Strength
        {
            get { return _Strength; }
        }
        /// <summary>
        /// 稳定验算应力
        /// </summary>
        public double Stability
        {
            get { return _Stability; }
        }
        /// <summary>
        /// 应力比
        /// </summary>
        public double Ratio
        {
            get { return _Ratio; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SingleEleCheckResData()
        {
            _Sec_contral = "1/2";
            _ComName = "None";
            _N = 0; _My = 0; _Mz = 0;
            _Strength = 0; _Stability = 0;
            _Ratio = 0;
        }
        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="sec">截面控制点标志</param>
        /// <param name="com">组合名</param>
        /// <param name="N">轴向力</param>
        /// <param name="My">弯矩</param>
        /// <param name="Mz">弯矩</param>
        /// <param name="Stre">强度应力</param>
        /// <param name="Stab">稳定应力</param>
        /// <param name="Ratio">最大应力比</param>
        public SingleEleCheckResData(string sec, string com, double N, double My, double Mz,
            double Stre, double Stab, double Ratio)
        {
            _Sec_contral = sec;
            _ComName = com;
            _N = N; _My = My; _Mz = Mz;
            _Strength = Stre; _Stability = Stab;
            _Ratio = Ratio;
        }
        /// <summary>
        /// 比较两个截面应力比，返回控制应力比
        /// </summary>
        /// <param name="SECRD">另一个截面验算结果</param>
        /// <returns>应力比最大的一个验算结果</returns>
        public SingleEleCheckResData TheMaxRatio(SingleEleCheckResData SECRD)
        {
            if (this.Ratio >= SECRD.Ratio)
            {
                return this;
            }
            else
            {
                return SECRD;
            }
        }
    }
    /// <summary>
    /// 验算结果类
    /// </summary>
    [Serializable]
    public class CheckRes:Object
    {
        //截面验算表
        private SortedList<int, EleCheckResData> _CheckResTable;

        /// <summary>
        /// 截面验算表
        /// </summary>
        public SortedList<int, EleCheckResData> CheckResTable
        {
            get { return _CheckResTable; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckRes()
        {
            _CheckResTable = new SortedList<int, EleCheckResData>();
        }
        #region 截面验算方法
        /// <summary>
        /// 验算单个单元
        /// </summary>
        /// <param name="mm">模型对像</param>
        /// <param name="iElem">单元号</param>
        public void CheckElemByNum(ref Bmodel mm,int iElem)
        {
            EleCheckResData EleData = new EleCheckResData (iElem);//单元组合验算表

            List<string> coms = mm.LoadCombTable.ComSteel;
            foreach (string com in coms)
            {
                double gamma_re = 1.0;//承载力抗震调整系数
                BLoadComb curComb = mm.LoadCombTable.getLoadComb(LCKind.STEEL, com);
                //如果未激活则不验算
                if (curComb.bACTIVE == false)
                    continue;
                 FrameElement ele =mm.elements[iElem] as FrameElement;

                //若为地震组合则取存储的承载力抗震调整系数
                if (curComb.hasLC_ANAL(ANAL.ES) ||
                    curComb.hasLC_ANAL(ANAL.RS))
                {
                    gamma_re = ele.DPs.Gamma_re_1;
                }
               
                //先进行单元内力组合
                ElemForce EFcom = mm.CalElemForceComb(curComb, iElem);
           
                //计算强度
                double Strength_i=CodeCheck.CalSecMaxStrength_YW(EFcom.Force_i,
                    mm.sections[ele.iPRO],
                    ele.DPs)*gamma_re;//i截面计算强度
                double Strength_2 = CodeCheck.CalSecMaxStrength_YW(EFcom.Force_48,
                    mm.sections[ele.iPRO],
                    ele.DPs)*gamma_re;
                double Strength_j = CodeCheck.CalSecMaxStrength_YW(EFcom.Force_j,
                    mm.sections[ele.iPRO],
                    ele.DPs)*gamma_re;
                //计算稳定性强度
                //double Stability_i = CodeCheck.CalStability_YW(EFcom.Force_i, mm.sections[ele.iPRO],
                //    ele.DPs, mm.mats[ele.iMAT].Elast)*gamma_re;
                //double Stability_2 =CodeCheck. CalStability_YW(EFcom.Force_48, mm.sections[ele.iPRO],
                //    ele.DPs, mm.mats[ele.iMAT].Elast)*gamma_re;
                //double Stability_j =CodeCheck. CalStability_YW(EFcom.Force_j, mm.sections[ele.iPRO],
                //    ele.DPs, mm.mats[ele.iMAT].Elast)*gamma_re;
                double Stability_i = CodeCheck.CalSecMaxStability_YW(EFcom.Force_i, mm.sections[ele.iPRO],
                    ele.DPs, mm.mats[ele.iMAT].Elast) * gamma_re;
                double Stability_2 = CodeCheck.CalSecMaxStability_YW(EFcom.Force_48, mm.sections[ele.iPRO],
                    ele.DPs, mm.mats[ele.iMAT].Elast) * gamma_re;
                double Stability_j = CodeCheck.CalSecMaxStability_YW(EFcom.Force_j, mm.sections[ele.iPRO],
                    ele.DPs, mm.mats[ele.iMAT].Elast) * gamma_re;

                double Ratio = Math.Max( Strength_i,Stability_i) / ele.DPs.fy;
                double Ratio_2 = Math.Max(Strength_2,Stability_2)/ ele.DPs.fy;
                double Ratio_j = Math.Max(Strength_j,Stability_j)/ ele.DPs.fy;

                //加入到数据库
                SingleEleCheckResData Secrd =
                    new SingleEleCheckResData("I",com,EFcom.Force_i.N,EFcom.Force_i.My,EFcom.Force_i.Mz,
                        Strength_i,Stability_i,Ratio);
                SingleEleCheckResData Secrd_2=
                    new SingleEleCheckResData("1/2", com, EFcom.Force_48.N, EFcom.Force_48.My, EFcom.Force_48.Mz,
                        Strength_2, Stability_2, Ratio_2);
                SingleEleCheckResData Secrd_j =
                    new SingleEleCheckResData("J", com, EFcom.Force_j.N, EFcom.Force_j.My, EFcom.Force_j.Mz,
                        Strength_j, Stability_j, Ratio_j);

                Secrd = Secrd.TheMaxRatio(Secrd_2);//取得控制内力
                Secrd = Secrd.TheMaxRatio(Secrd_j);

                EleData.Add(com, Secrd);//添加到单元验算结果数据中
            }

            //添加到数据表
            if (this._CheckResTable.ContainsKey(iElem))
            {
                this._CheckResTable.Remove(iElem);
                this._CheckResTable.Add(iElem, EleData);
            }
            else
                this._CheckResTable.Add(iElem, EleData);
        }

        /// <summary>
        /// 验算同一截面的所有单元
        /// </summary>
        /// <param name="mm">模型对像</param>
        /// <param name="iSec">截面号</param>
        public void CheckElemBySec(ref Bmodel mm, int iSec)
        {
            List<int> Elems = mm.getElemBySec(iSec);//单元号组
            foreach (int iEle in Elems)
            {
                CheckElemByNum(ref mm, iEle);
            }
        }

        /// <summary>
        /// 新函数：验算单个单元
        /// </summary>
        /// <param name="mm">模型对像</param>
        /// <param name="iElem">单元号</param>
        /// <param name="bcm">截面验算参数指定</param>
        public void CheckElemByNum_N(ref Bmodel mm, int iElem,ref BCheckModel bcm)
        {
            EleCheckResData EleData = new EleCheckResData(iElem);//单元组合验算表

            List<string> coms = mm.LoadCombTable.ComSteel;
            foreach (string com in coms)
            {
                BLoadComb curComb = mm.LoadCombTable.getLoadComb(LCKind.STEEL, com);
                //如果未激活则不验算
                if (curComb.bACTIVE == false)
                    continue;

                FrameElement ele = mm.elements[iElem] as FrameElement;
                //double gamma_re1 = ele.DPs.Gamma_re_1; ;//承载力抗震调整系数:强度
                //double gamma_re2 = ele.DPs.Gamma_re_2;//承载力抗震调整系数：稳定
                double gamma_re1 = 1; ;//承载力抗震调整系数:强度
                double gamma_re2 = 1;//承载力抗震调整系数：稳定
                //先进行单元内力组合
                ElemForce EFcom = mm.CalElemForceComb(curComb, iElem);

                //计算强度
                double Strength_i = CodeCheck.CalSecMaxStrength_YW(EFcom.Force_i,
                    mm.sections[ele.iPRO],
                    ele.DPs) * gamma_re1;//i截面计算强度
                double Strength_2 = CodeCheck.CalSecMaxStrength_YW(EFcom.Force_48,
                    mm.sections[ele.iPRO],
                    ele.DPs) * gamma_re1;
                double Strength_j = CodeCheck.CalSecMaxStrength_YW(EFcom.Force_j,
                    mm.sections[ele.iPRO],
                    ele.DPs) * gamma_re1;
                //计算稳定
                double Stability_i = CodeCheck.CalSecMaxStability_YW(EFcom.Force_i, mm.sections[ele.iPRO],
                    ele.DPs, mm.mats[ele.iMAT].Elast) * gamma_re2;
                double Stability_2 = CodeCheck.CalSecMaxStability_YW(EFcom.Force_48, mm.sections[ele.iPRO],
                    ele.DPs, mm.mats[ele.iMAT].Elast) * gamma_re2;
                double Stability_j = CodeCheck.CalSecMaxStability_YW(EFcom.Force_j, mm.sections[ele.iPRO],
                    ele.DPs, mm.mats[ele.iMAT].Elast) * gamma_re2;

                double Ratio = Math.Max(Strength_i, Stability_i) / ele.DPs.fy;
                double Ratio_2 = Math.Max(Strength_2, Stability_2) / ele.DPs.fy;
                double Ratio_j = Math.Max(Strength_j, Stability_j) / ele.DPs.fy;

                //加入到数据库
                SingleEleCheckResData Secrd =
                    new SingleEleCheckResData("I", com, EFcom.Force_i.N, EFcom.Force_i.My, EFcom.Force_i.Mz,
                        Strength_i, Stability_i, Ratio);
                SingleEleCheckResData Secrd_2 =
                    new SingleEleCheckResData("1/2", com, EFcom.Force_48.N, EFcom.Force_48.My, EFcom.Force_48.Mz,
                        Strength_2, Stability_2, Ratio_2);
                SingleEleCheckResData Secrd_j =
                    new SingleEleCheckResData("J", com, EFcom.Force_j.N, EFcom.Force_j.My, EFcom.Force_j.Mz,
                        Strength_j, Stability_j, Ratio_j);

                Secrd = Secrd.TheMaxRatio(Secrd_2);//取得控制内力
                Secrd = Secrd.TheMaxRatio(Secrd_j);

                EleData.Add(com, Secrd);//添加到单元验算结果数据中
            }

            //添加到数据表
            if (this._CheckResTable.ContainsKey(iElem))
            {
                this._CheckResTable.Remove(iElem);
                this._CheckResTable.Add(iElem, EleData);
            }
            else
                this._CheckResTable.Add(iElem, EleData);
        }
        #endregion

        #region 其它方法
        /// <summary>
        /// 取得单元中的控制单元
        /// </summary>
        /// <param name="iElems">单元号集合</param>
        /// <returns>起控制作用的单元号</returns>
        public int GetControlElem(List<int> iElems)
        {
            int Res = iElems[0];
            for (int i = 1; i < iElems.Count; i++)
            {
                SingleEleCheckResData temp=this.CheckResTable[Res].GetControlData();
                SingleEleCheckResData temp1=this.CheckResTable[iElems[i]].GetControlData();
                if (temp.Ratio < temp1.Ratio)
                {
                    Res = iElems[i];
                }
            }
            return Res;
        }

        /// <summary>
        /// 取得相应截面号的单元号集合
        /// </summary>
        /// <param name="mm">模型对像</param>
        /// <param name="iSec">截面号</param>
        /// <returns>单元号集合</returns>
        public List<int> GetElemsBySec(ref Bmodel mm,int iSec)
        {
            List<int> Res = new List<int>();
            foreach (int key in _CheckResTable.Keys)
            {
                if (mm.elements[key].iPRO == iSec)
                {
                    Res.Add(key);
                }
            }
            return Res;//返回
        }

        /// <summary>
        /// 根据应力比范围取得单元号集合
        /// </summary>
        /// <param name="R_min">最小应力比</param>
        /// <param name="R_max">最大应力比</param>
        /// <returns>单元号集合</returns>
        public List<int> GetElemsByRatio(double R_min, double R_max)
        {
            List<int> Res = new List<int>();
            foreach (int key in _CheckResTable.Keys)
            {
                SingleEleCheckResData secrd=_CheckResTable[key].GetControlData();
                if (secrd.Ratio > R_min && secrd.Ratio < R_max)
                {
                    Res.Add(key);
                }
            }
            return Res;
        }
        #endregion
    }
    /// <summary>
    /// 截面类别
    /// </summary>
    public enum SecCategory
    {
        a,b,c,d
    }
    /// <summary>
    /// 构件受力类型
    /// </summary>
    public enum CompForceType
    {
        /// <summary>
        /// 梁，受弯剪扭构件
        /// </summary>
        beam,
        /// <summary>
        /// 桁架，轴心受力构件
        /// </summary>
        truss,
        /// <summary>
        /// 柱，拉弯、压弯构件
        /// </summary>
        column
    }
    /// <summary>
    /// 设计参数类
    /// 包括所有截面验算可能用到的参数表
    /// </summary>
    public class BCheckModel
    {
        #region 成员与属性
        private SafeLevel _safelevel;//结构安全等级
        private SortedList<int, CompForceType> _ComponentType;//构件类型链表
        private SortedList<int, BSecDesignPara> _SecDP;//截面相关设计参数链表
        private SortedList<int, BEquivalentCoeff> _EquBendCoeff;//等效弯矩系数链表
        private SortedList<int, BQuakeFactor> _QuakeFactors;//抗震承载力调整系数
        private SortedList<int, BQuakeAdjustFactor> _QuakeAdjustFacors;//地震作用放大系数表
        /// <summary>
        /// 结构安全等级
        /// </summary>
        public SafeLevel Safelevel
        {
            get { return _safelevel; }
            set { _safelevel = value; }
        }
        /// <summary>
        /// 结构重要性系数，根据安全等级调整
        /// </summary>
        public double SafeCoefficient
        {
            get 
            {
                switch (_safelevel)
                {
                    case SafeLevel.One: return 1.1;
                    case SafeLevel.Two: return 1.0;
                    case SafeLevel.Three: return 0.9;
                }
                return 1.0;
            }
        }
        /// <summary>
        /// 构件受力类型链表
        /// </summary>
        public SortedList<int, CompForceType> ComponentType
        {
            get { return _ComponentType; }
        }
        /// <summary>
        /// 截面相关设计参数链表
        /// </summary>
        public SortedList<int, BSecDesignPara> SecDP
        {
            get { return _SecDP; }
        }
        /// <summary>
        /// 等效弯矩系数链表
        /// </summary>
        public SortedList<int, BEquivalentCoeff> EquBendCoeff
        {
            get { return _EquBendCoeff; }
        }
        /// <summary>
        /// 抗震承载力调整系数
        /// </summary>
        public SortedList<int, BQuakeFactor> QuakeFactors
        {
            get { return _QuakeFactors; }
        }
        /// <summary>
        /// 地震作用放大系数表
        /// </summary>
        public SortedList<int, BQuakeAdjustFactor> QuakeAdjustFacors
        {
            get { return _QuakeAdjustFacors; }
        }
        #endregion
        #region 构造函数
        public BCheckModel()
        {
            _safelevel = SafeLevel.Two;//默认安全等级二级
            _ComponentType = new SortedList<int, CompForceType>();
            _SecDP = new SortedList<int, BSecDesignPara>();
            _EquBendCoeff = new SortedList<int, BEquivalentCoeff>();
            _QuakeFactors = new SortedList<int, BQuakeFactor>();
            _QuakeAdjustFacors = new SortedList<int, BQuakeAdjustFactor>();
        }
        #endregion
        #region 方法
        /// <summary>
        /// 指定单元的构件类型
        /// </summary>
        /// <param name="num">单元号</param>
        /// <param name="type">构件类型</param>
        public void assignCompType(int num,CompForceType type)
        {
            if (_ComponentType.ContainsKey(num))
            {
                _ComponentType[num] = type;
            }
            else
            {
                _ComponentType.Add(num, type);
            }
        }
        /// <summary>
        /// 删除单元的构件类型指定
        /// </summary>
        /// <param name="num">单元号</param>
        public void deleCompType(int num)
        {
            _ComponentType.Remove(num);
        }
        /// <summary>
        /// 指定截面设计参数
        /// </summary>
        /// <param name="num">截面号</param>
        /// <param name="DP">截面相关设计参数</param>
        public void assignSecDP(int num, BSecDesignPara DP)
        {
            if (_SecDP.ContainsKey(num))
            {
                _SecDP[num] = DP;
            }
            else
            {
                _SecDP.Add(num, DP);
            }
        }
        /// <summary>
        /// 删除指定截面号的设计参数
        /// </summary>
        /// <param name="num">截面号</param>
        public void deleSecTP(int num)
        {
            _SecDP.Remove(num);
        }
        /// <summary>
        /// 指定单元等效弯矩系数
        /// </summary>
        /// <param name="num">单元号</param>
        /// <param name="EC">弯矩系数参数</param>
        public void assignEquCoeff(int num, BEquivalentCoeff EC)
        {
            if (_EquBendCoeff.ContainsKey(num))
            {
                _EquBendCoeff[num] = EC;
            }
            else
            {
                _EquBendCoeff.Add(num, EC);
            }
        }
        /// <summary>
        /// 删除指定单元号的等效弯矩系数数据
        /// </summary>
        /// <param name="num">单元号</param>
        public void deldEquCoeff(int num)
        {
            _EquBendCoeff.Remove(num);
        }
        /// <summary>
        /// 指定单元地震作用放大系数
        /// </summary>
        /// <param name="num">单元号</param>
        /// <param name="QAF">放大系数参数</param>
        public void assignQuakeAdjustFactor(int num, BQuakeAdjustFactor QAF)
        {
            if (_QuakeAdjustFacors.ContainsKey(num))
            {
                _QuakeAdjustFacors[num] = QAF;
            }
            else
            {
                _QuakeAdjustFacors.Add(num, QAF);
            }
        }
        /// <summary>
        /// 删除指定单元号的地震作用放大系数
        /// </summary>
        /// <param name="num">单元号</param>
        public void deleQuakeAdjustFactor(int num)
        {
            _QuakeAdjustFacors.Remove(num);
        }

        /// <summary>
        /// 取得截面设计参数定义
        /// </summary>
        /// <param name="iSec">截面号</param>
        /// <param name="SDP">设计参数</param>
        /// <returns>是否进行了指定</returns>
        public bool getSecDesignPara(int iSec,out BSecDesignPara SDP)
        {
            SDP = new BSecDesignPara(iSec);
            if (_SecDP.ContainsKey(iSec))
            {
                SDP = _SecDP[iSec];
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// 取得单元的等效弯矩系数定义
        /// </summary>
        /// <param name="iEle">单元号</param>
        /// <param name="EC">等效弯矩系数数据</param>
        /// <returns>是否进行了定义</returns>
        public bool getEquBendCoeff(int iEle, out BEquivalentCoeff EC)
        {
            EC = new BEquivalentCoeff(iEle);
            if (_EquBendCoeff.ContainsKey(iEle))
            {
                EC = _EquBendCoeff[iEle];
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// 取得单元的抗震承载力调整系数指定
        /// </summary>
        /// <param name="iEle">单元号</param>
        /// <param name="QF">抗震承载力调整系数</param>
        /// <returns>是否进行了指定</returns>
        public bool getQuakeFactor(int iEle, out BQuakeFactor QF)
        {
            QF = new BQuakeFactor(iEle);
            if (_QuakeFactors.ContainsKey(iEle))
            {
                QF = _QuakeFactors[iEle];
                return true;
            }
            else
                return false;
        }
        #endregion
    }

    /// <summary>
    /// 与截面相关的设计参数数据类
    /// </summary>
    public class BSecDesignPara
    {
        #region 成员与属性
        private int _iSec;
        /// <summary>
        /// 截面号
        /// </summary>
        public int iSec
        {
            get { return _iSec; }
            set { _iSec = value; }
        }
        private double _RatioNet;
        /// <summary>
        /// 净毛面积比
        /// </summary>
        public double RatioNet
        {
            get { return _RatioNet; }
            set { _RatioNet = value; }
        }

        private bool _isClosed;
        /// <summary>
        /// 是否闭口截面
        /// </summary>
        public bool IsClosed
        {
            get { return _isClosed; }
            set { _isClosed = value; }
        }
        private SecCategory _SecCat_y,_SecCat_z;
        /// <summary>
        /// 截面类别:沿x轴（强轴）
        /// </summary>
        public SecCategory SecCat_y
        {
            get { return _SecCat_y; }
            set { _SecCat_y = value; }
        }
        /// <summary>
        /// 截面类别：沿y轴（弱轴）
        /// </summary>
        public SecCategory SecCat_z
        {
            get { return _SecCat_z; }
            set { _SecCat_z = value; }
        }
        private double _Gama_x, _Gama_y;
        /// <summary>
        /// 截面塑性发展系数-x轴
        /// </summary>
        public double Gama_x
        {
            get { return _Gama_x; }
            set { _Gama_x = value; }
        }
        /// <summary>
        /// 截面塑性发展系数-y轴
        /// </summary>
        public double Gama_y
        {
            get { return _Gama_y; }
            set { _Gama_y = value; }
        }
        #endregion
        #region 构造函数
        /// <summary>
        /// 默认构造函数
        /// 净毛面积比：0.9；闭口截面；强轴b类截面、弱轴c类；塑性发展系数均到1.05
        /// </summary>
        /// <param name="num">截面号</param>
        public BSecDesignPara(int num)
        {
            _iSec = num;
            _isClosed = false;
            _RatioNet = 0.9;
            _SecCat_y = SecCategory.b;
            _SecCat_z = SecCategory.c;
            _Gama_x = 1.05;
            _Gama_y = 1.05;
        }
        #endregion
    }
    /// <summary>
    /// 等效弯矩系数数据类
    /// </summary>
    public class BEquivalentCoeff
    {
        #region 成员与属性
        private int _iEle;
        /// <summary>
        /// 单元号
        /// </summary>
        public int IEle
        {
            get { return _iEle; }
            set { _iEle = value; }
        }
        private double _Beta_my, _Beta_mz, _Beta_ty, _Beta_tz;
        /// <summary>
        /// Beta_my
        /// </summary>
        public double Beta_my
        {
            get { return _Beta_my; }
            set { _Beta_my = value; }
        }
        /// <summary>
        /// Beta_mz
        /// </summary>
        public double Beta_mz
        {
            get { return _Beta_mz; }
            set { _Beta_mz = value; }
        }
        /// <summary>
        /// Beta_ty
        /// </summary>
        public double Beta_ty
        {
            get { return _Beta_ty; }
            set { _Beta_ty = value; }
        }
        /// <summary>
        /// Beta_tz
        /// </summary>
        public double Beta_tz
        {
            get { return _Beta_tz; }
            set { _Beta_tz = value; }
        }
        #endregion
        /// <summary>
        /// 构造函数
        /// 默认所有变矩系数取1.0
        /// </summary>
        /// <param name="num">单元号</param>
        public BEquivalentCoeff(int num)
        {
            _iEle = num;
            _Beta_my = 1;
            _Beta_mz = 1;
            _Beta_ty = 1;
            _Beta_tz = 1;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="num">单元号</param>
        /// <param name="beta_my">Beta_my</param>
        /// <param name="beta_mz">Beta_mz</param>
        /// <param name="beta_ty">Beta_ty</param>
        /// <param name="beta_tz">Beta_tz</param>
        public BEquivalentCoeff(int num, double beta_my, double beta_mz, double beta_ty, double beta_tz)
        {
            _iEle = num;
            _Beta_my = beta_my;
            _Beta_mz = beta_mz;
            _Beta_ty = beta_ty;
            _Beta_tz = beta_tz;
        }
    }

    /// <summary>
    /// 承载力抗震调整系数
    /// 和单元对应
    /// </summary>
    public class BQuakeFactor
    {
        #region 成员与属性
        private int _iEle;
        /// <summary>
        /// 单元号
        /// </summary>
        public int IEle
        {
            get { return _iEle; }
            set { _iEle = value; }
        }
        private double _GamaRe_1, _GamaRe_2;
        /// <summary>
        /// [强度计算]抗震承载力调整系数
        /// </summary>
        public double GamaRe_1
        {
            get { return _GamaRe_1; }
            set { _GamaRe_1 = value; }
        }
        /// <summary>
        /// [稳定计算]抗震承载力调整系数
        /// </summary>
        public double GamaRe_2
        {
            get { return _GamaRe_2; }
            set { _GamaRe_2 = value; }
        }

        #endregion

        /// <summary>
        /// 构造函数
        /// 默认钢结构，强度取0.75，稳定取0.8
        /// </summary>
        /// <param name="num">单元号</param>
        public BQuakeFactor(int num)
        {
            _iEle = num;
            _GamaRe_1 = 0.75;
            _GamaRe_2 = 0.8;
        }
    }
    /// <summary>
    /// 地震作用放大系数
    /// 和单元对应
    /// </summary>
    public class BQuakeAdjustFactor
    {
        #region 成员与属性
        private int _iEle;
        /// <summary>
        /// 单元号
        /// </summary>
        public int IEle
        {
            get { return _iEle; }
            set { _iEle = value; }
        }
        private double _N_LC, _M_LC, _V_LC;
        /// <summary>
        /// 轴力调整放大系数：用于地震作用工况（组合前）
        /// </summary>
        public double N_LC
        {
            get { return _N_LC; }
            set { _N_LC = value; }
        }
        /// <summary>
        /// 弯矩放大系数：用于地震作用工况（组合前）
        /// </summary>
        public double M_LC
        {
            get { return _M_LC; }
            set { _M_LC = value; }
        }
        /// <summary>
        /// 剪力放大系数：用于地震作用工况（组合前）
        /// </summary>
        public double V_LC
        {
            get { return _V_LC; }
            set { _V_LC = value; }
        }
        private double _N_COM, _M_COM, _V_COM;
        /// <summary>
        /// 轴力放大系数：用于地震作用组合（组合后）
        /// </summary>
        public double N_COM
        {
            get { return _N_COM; }
            set { _N_COM = value; }
        }
        /// <summary>
        /// 弯矩放大系数：用于地震作用组合（组合后）
        /// </summary>
        public double M_COM
        {
            get { return _M_COM; }
            set { _M_COM = value; }
        }
        /// <summary>
        /// 剪力放大系数：用于地震作用组合（组合后）
        /// </summary>
        public double V_COM
        {
            get { return _V_COM; }
            set { _V_COM = value; }
        }
        #endregion
        #region 构造函数
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="ele">单元号</param>
        public BQuakeAdjustFactor(int ele)
        {
            _iEle = ele;
            _N_LC = 1.0; _M_LC = 1.0; _V_LC = 1.0;
            _N_COM = 1.0; _M_COM = 1.0; _V_COM = 1.0;
        }
        #endregion
    }
    /// <summary>
    /// 建筑结构安全等级：一级、二级、三级
    /// </summary>
    public enum SafeLevel
    {
        One,Two,Three
    }
}
