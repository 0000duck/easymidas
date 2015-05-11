using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using MidasGenModel.model;
using MidasGenModel.Tools;
using SerializerProgress;
using Xceed.DockingWindows;
using Xceed.DockingWindows.TabbedMdi;
using Xceed.Grid;
using EasyMidas.Post;
using MidasGenModel.Design;

namespace EasyMidas
{
    public partial class MainForm : Form
    {
        #region 数据成员
        private DockLayoutManager _LayoutManager=null;//布局管理器
        private TabbedMdiManager _mdiManager = null;//标签面管理器
        private string _modelKey;//模型窗口关键字
        //private static ModelForm ModelForm;//模型主视图窗口
        private BackgroundWorker _BackWorker;//后台单独进程
        private string _tempFileName;//模型文件存储路径
        #endregion

        #region 属性
        /// <summary>
        /// 取得布局管理器
        /// </summary>
        internal DockLayoutManager DockLayoutManager
        {
            get { return _LayoutManager; }
        }
        /// <summary>
        /// 信息提示窗口
        /// </summary>
        internal  MessageTools MessageTool
        {
            get { return _LayoutManager.ToolWindows["MessageTool"] as MessageTools; }
        }
        /// <summary>
        /// 工具窗口
        /// </summary>
        internal ToolPanel ToolPanel
        {
            get { return _LayoutManager.ToolWindows["MainPanel"] as ToolPanel; }
        }
        //工具窗口集
        internal ToolWindowCollection ToolWindows
        {
            get { return _LayoutManager.ToolWindows; }
        }
        /// <summary>
        /// 当前模型显示主窗口
        /// </summary>
        internal ModelForm1 ModelForm
        {
            get
            {
                return _LayoutManager.ToolWindows["Model"] as ModelForm1;
            }
        }
        /// <summary>
        /// 当前选择单元集
        /// </summary>
        internal List<int> SelectElems
        {
            get
            {
                return SelectCollection.StringToList(this.cb_selectEle.Text);
            }
        }
        #endregion

        #region 构造函数
        public MainForm()
        {
            InitializeComponent();

            _BackWorker = new BackgroundWorker();//创建后台进程
            _BackWorker.WorkerReportsProgress = true;//可以报告进程更新
            _BackWorker.WorkerSupportsCancellation = false;//进程不支持取消

            string CurDir = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
            string CurModelPath = CurDir + "\\models";
            _tempFileName = Path.Combine(CurModelPath, "model.emgb");//取得模型文件默认存储路径
            _modelKey = "Model";

            stusProgressBar.Visible = false;//先不显示进度条

            //初始化工具窗口
            this.InitFormLayout();//初始化布局
            this.ConfigureTabbedMdiManager();//初始化多标签页
        }
        #endregion

        #region 菜单事件相应
        private void 读取MgtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string CurDir = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
            string CurModelPath = CurDir + "\\models";
            string ModelFile = Path.Combine(CurModelPath, "model.emgb");
            if (Directory.Exists(CurModelPath) == false)//如果没有模型文件目录
            {
                Directory.CreateDirectory(CurModelPath);//创建目录
            }

            if (!this.HasToolWindow("Model"))//如果没有模型窗口
            {
                MessageBox.Show("请先新建模型","提示",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return;
            }
            else
            {
                if (File.Exists(ModelFile) == false)
                {
                    ReReadModel(ModelFile);//读取模型文件
                }
                else
                {
                    DialogResult res = MessageBox.Show("导入MGT将冲掉现有的模型信息，是否确定？", "提示",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                    if (res == DialogResult.Yes)
                    {
                        ReReadModel(ModelFile);
                    }
                }
            }

            
        }
   
        /// <summary>
        /// 重新读取mgt模型数据文件
        /// </summary>
        /// <param name="MoldeFile">模型文件存储路径</param>
        private void ReReadModel(string ModelFile)
        {
            OpenFileDialog OPD = new OpenFileDialog();
            OPD.Title = "选择Midas数据文件路径";
            OPD.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);//获取我的文档
            OPD.Filter = "mgt 文件(*.mgt)|*.mgt|All files (*.*)|*.*";

            if (OPD.ShowDialog() == DialogResult.OK)
            {
                ModelForm.CurModel = new MidasGenModel.model.Bmodel();
                ModelForm.CurModel.ReadFromMgt(OPD.FileName);//读取mgt文件
                //MidasGenModel.Application.WriteModelBinary(ModelForm.CurModel, ModelFile);//写出二进制文件
                MessageLabel.Text = "读取模型成功！";
                ModelForm.Refresh();
                this.Refresh();
            }
        }

        private void 读取内力ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "选择Midas梁单元内力输出文件";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            ofd.Filter = "nl 文件(*.nl)|*.nl|All files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ModelForm.CurModel.ReadElemForces(ofd.FileName);//读取内力
                int num = ModelForm.CurModel.elemforce.Count;//单位内力数
                MessageBox.Show("读入单位内力成功！单位内力数据数：" + num.ToString());
                this.Refresh();
            } 
        }

        //读取桁架单元内力
        private void MenuItem_TrussForceIn_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "选择Midas桁架单元内力输出文件";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            ofd.Filter = "nl 文件(*.nl)|*.nl|All files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ModelForm.CurModel.ReadTrussForces(ofd.FileName);
                int num = ModelForm.CurModel.elemforce.Count;//单位内力数
                MessageBox.Show("读入单位内力成功！单位内力数据数：" + num.ToString());
                this.Refresh();
            } 
        }
        //读取3d3s模型信息菜单
        //添加：2011.08.22
        private void Read3d3SMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.HasToolWindow(_modelKey))
            {
                MessageBox.Show("请先新建模型");
                return;
            }
            Import3D3SExcelFiles Im3d3s = new Import3D3SExcelFiles();
            
            Im3d3s.Owner = this;
            Im3d3s.Show();
        }

        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.HasToolWindow("Model"))
            {
                ModelForm1 mf = new ModelForm1();
                mf.Text = "新模型";
                _LayoutManager.ToolWindows.Add(mf);
                this.Refresh();//刷新界面
            }
            else
            {
                DialogResult res = MessageBox.Show("现有模型数据将被初始化，是否确认对现有模型进行初始化？", "注意",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2);
                if (res == DialogResult.Yes)
                {
                    _LayoutManager.ToolWindows.Remove(_LayoutManager.ToolWindows["Model"]);
                    ModelForm1 mf = new ModelForm1();
                    mf.Text = "新模型";
                    _LayoutManager.ToolWindows.Add(mf);
                    this.Refresh();//刷新界面
                }
            }          
        }

        private void 钢结构验算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Length == 0)
            {
                MessageBox.Show("请先新建模型");
                return;
            }

            CheckElemForm cef = new CheckElemForm();
            cef.Owner = this;
            cef.Show();
            //if (cb_selectEle.Text == null || cb_selectEle.Text == "")
            //{
            //    this.MessageTool.tb_out.AppendText("\r\n  未选择单元");
            //    //this.MessageTool.tb_out.ScrollToCaret();
            //}
            //else
            //{
            //    int num = SelectCollection.StringToList(this.cb_selectEle.Text).Count;
            //    this.MessageTool.tb_out.AppendText("\r\n  选择单元数为："+num.ToString());
            //}
        }

        private void 保存模型ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string CurDir = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
            string CurModelPath = CurDir + "\\models";
            string ModelFile = Path.Combine(CurModelPath, "model.emgb");
            if (Directory.Exists(CurModelPath) == false)//如果没有模型文件目录
            {
                Directory.CreateDirectory(CurModelPath);//创建目录
            }

            if (!this.HasToolWindow("Model"))
            {
                MessageBox.Show("请先新建模型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            else
            {
                MidasGenModel.Application.WriteModelBinary(this.ModelForm.CurModel, ModelFile);
                ModelForm.Text = ModelFile;
            }
        }

        /// <summary>
        /// 将数据文件另存为db文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 另存为dbToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.HasToolWindow("Model"))
            {
                MessageBox.Show("请先新建模型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            SaveFileDialog SFD = new SaveFileDialog();
            SFD.Title = "选择文件路径";
            SFD.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);//获取我的文档
            SFD.Filter = "emb 文件(*.emb)|*.emb|All files (*.*)|*.*";

            if (SFD.ShowDialog() == DialogResult.OK)
            {
                TextBox tb = this.MessageTool.Tb_out;//输出信息窗口
                this.ModelForm.CurModel.WriteToSqliteDb(SFD.FileName, ref this.MessageLabel,ref tb);
            }
        }

        private void 重读缓存模型ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ;
            string ModelFile = _tempFileName;
            if (ModelForm == null || ModelForm.IsDisposed)
            {
                MessageBox.Show("请先新建模型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            else if (File.Exists(ModelFile) == false)
            {
                MessageBox.Show("缓存文件不存在!请先保存模型...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                //ModelForm.CurModel=MidasGenModel.Application.ReadModelBinary(ModelFile);
                _BackWorker.ProgressChanged += delegate(object sender1, ProgressChangedEventArgs e1)
                {
                    stusProgressBar.Value = e1.ProgressPercentage;
                };
                _BackWorker.DoWork += new DoWorkEventHandler(Deserialize);
                _BackWorker.RunWorkerCompleted += delegate
                {
                    MessageLabel.Text = "读取缓存模型完成!";
                    ModelForm.Text = _tempFileName;
                    stusProgressBar.Visible = false;
                };
                stusProgressBar.Visible = true;//显示状态进度栏
                _BackWorker.RunWorkerAsync();//开始后台读取操作
            }
        }

        private void 存储验算结果ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string CurDir = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
            string CurModelPath = CurDir + "\\models";
            string ModelFile = Path.Combine(CurModelPath, "model.ga");
            if (Directory.Exists(CurModelPath) == false)//如果没有模型文件目录
            {
                Directory.CreateDirectory(CurModelPath);//创建目录
            }

            if (ModelForm == null || ModelForm.IsDisposed)
            {
                MessageBox.Show("请先新建模型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            else
            {
                MidasGenModel.Application.WriteCheckBinary(ModelForm.CheckTable, ModelFile);
                MessageLabel.Text = "存储验算结果成功！";
            }
        }

        private void 重读取验算结果ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string CurDir = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
            string CurModelPath = CurDir + "\\models";
            string ModelFile = Path.Combine(CurModelPath, "model.ga");
            if (Directory.Exists(CurModelPath) == false)//如果没有模型文件目录
            {
                Directory.CreateDirectory(CurModelPath);//创建目录
            }

            if (ModelForm == null || ModelForm.IsDisposed)
            {
                MessageBox.Show("请先新建模型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            else if (File.Exists(ModelFile) == false)
            {
                MessageBox.Show("缓存文件不存在!请先保存验算结果...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                ModelForm.CheckTable = MidasGenModel.Application.ReadCheckBinary(ModelFile);
                MessageLabel.Text = "重读验算结果成功!";
            }
        }

        private void midas选择集转换ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckSteelBeam csb = new CheckSteelBeam();
            csb.ShowDialog();
            csb.Owner = this;
        }

        private void 截面特性ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (!this.HasToolWindow(_modelKey))
            {
                MessageBox.Show("请先新建模型");
                return;
            }

            SectionForm sf = new SectionForm();
            sf.Owner = this;

            sf.InitSectList();//初始化截面表
            sf.ShowDialog();
        }

        private void 荷载组合编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.HasToolWindow(_modelKey))
            {
                MessageBox.Show("请先新建模型");
                return;
            }
            LoadComForm lcf = new LoadComForm();
            lcf.Owner = this;
            lcf.InitLoadComForm();//初始化
            lcf.ShowDialog();
        }

        /// <summary>
        /// 导出ansys文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_toAnsys_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "请输入ansys文件存储位置";
            sfd.Filter = "inp 文件(*.inp)|*.inp|All files (*.*)|*.*";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (ModelForm.CurModel.WriteToInp(sfd.FileName, 2))
                {
                    MessageTool.Tb_out.AppendText(Environment.NewLine + "  [Inp文件]Ansys主模型输出成功！");
                }
                else
                {
                    MessageTool.Tb_out.AppendText(Environment.NewLine + "$$$Ansys文件输出错误$$$");
                }

                //荷载转化为质量宏
                string InpPath = Path.GetDirectoryName(sfd.FileName);
                if (ModelForm.CurModel.WriteAnsysMarc_Load2Mass(InpPath))
                {
                    MessageTool.Tb_out.AppendText(Environment.NewLine +
                        "  [宏文件]荷载转化为质量输出成功！");
                }
            }
        }

        /// <summary>
        /// 导出midas文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_toMidas_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "请输入mgt文件存储位置";
            sfd.Filter = "mgt 文件(*.mgt)|*.mgt|All files (*.*)|*.*";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (ModelForm.CurModel.WriteToMGT(sfd.FileName,ref this.MessageLabel))
                {
                    MessageTool.Tb_out.AppendText(Environment.NewLine + "  [mgt文件]Midas/gen模型输出成功！");
                }
                else
                {
                    MessageTool.Tb_out.AppendText(Environment.NewLine + "$$$Midas/gen文件输出错误$$$");
                }
            }
        }
        /// <summary>
        /// 导出3d3s文本格式文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_to3D3S_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "请输入3d3s文本存储位置";
            sfd.Filter = "3d3s 文件(*.3d3s)|*.3d3s|All files (*.*)|*.*";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (ModelForm.CurModel.WriteTo3D3STxt(sfd.FileName, ref this.MessageLabel))
                {
                    MessageTool.Tb_out.AppendText(Environment.NewLine + "  [3D3S文件]3D3S v10.1模型输出成功！");
                }
                else
                {
                    MessageTool.Tb_out.AppendText(Environment.NewLine + "$$$3D3S模型文件输出错误$$$");
                }
            }
        }
        /// <summary>
        /// 导出Sap2000文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menu_to_Sap2000File_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "请输入s2k文本存储位置";
            sfd.Filter = "Sap2000 文件(*.s2k)|*.s2k|All files (*.*)|*.*";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (ModelForm.CurModel.WriteToS2KTxt(sfd.FileName, ref this.MessageLabel))
                {
                    MessageTool.Tb_out.AppendText(Environment.NewLine + "  [Sap2000文件]Sap2000模型输出成功！");
                }
                else
                {
                    MessageTool.Tb_out.AppendText(Environment.NewLine + "$$$Sap2000模型文件输出错误$$$");
                }
            }
        }
        /// <summary>
        /// 导出OpenSeesTCL文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_to_openSeeStcl_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "请输入OpenSees TCL文件存储位置";
            sfd.Filter = "OpenSees Tcl 文件(*.tcl)|*.tcl|All files (*.*)|*.*";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (ModelForm.CurModel.WriteToOpenSees(sfd.FileName, ref this.MessageLabel))
                {
                    MessageTool.Tb_out.AppendText(Environment.NewLine + "  [OpenSees Tcl文件]OpenSees模型输出成功！");
                }
                else
                {
                    MessageTool.Tb_out.AppendText(Environment.NewLine + "$$$OpenSees模型文件输出错误$$$");
                }
            }
        }
       
        private void 结构组处理工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.HasToolWindow(_modelKey))
            {
                MessageBox.Show("请先新建模型");
                return;
            }

            StruGroupTools SGTForm = new StruGroupTools();
            SGTForm.Owner = this;
            SGTForm.InitForm();//初始化窗体控件
            SGTForm.ShowInTaskbar = false;//不显示在务栏
            SGTForm.ShowDialog();//显示模态对话框
        }

        private void 空间点Delaunay三角化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //三角形分网功能
            if (!this.HasToolWindow(_modelKey))
            {
                MessageBox.Show("请先新建模型");
                return;
            }

            DelaunayTrans DTForm = new DelaunayTrans();
            DTForm.Owner = this;
            DTForm.InitForm();//初始化参数
            DTForm.ShowInTaskbar = false;//不显示在务栏
            DTForm.ShowDialog();//显示模态对话框
        }

        private void 节点内力查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.HasToolWindow(_modelKey))
            {
                MessageBox.Show("请先新建模型");
                return;
            }

            NodeForceForm NFForm = new NodeForceForm();
            NFForm.Owner = this;
            NFForm.ShowInTaskbar = false;//不显示在任务栏
            NFForm.Show();//显示无模态对话框
        }

        private void 查询加载点临时ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.HasToolWindow(_modelKey))
            {
                MessageBox.Show("请先新建模型");
                return;
            }

            ModelForm1 cf = this.ModelForm;
            BLoadTable blt = cf.CurModel.LoadTable;
            SortedList<int, BNLoad> NLD = blt.NLoadData["DL"] as SortedList<int, BNLoad>;

            MessageTool.Tb_out.AppendText(Environment.NewLine + "DL工况下节点荷载号如下：");
            foreach (int n in NLD.Keys)
            {
                MessageTool.Tb_out.AppendText(" "+n.ToString());
            }
        }

        private void 显示单元设计参数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<int> eles = SelectCollection.StringToList(this.cb_selectEle.Text);
            MidasGenModel.model.Bmodel cm;
            //如果单元选择数为0，作出提示
            if (eles.Count == 0)
            {
                MessageTool.Tb_out.AppendText(Environment.NewLine + "*Error*:未选择单元!");
                return;
            }
            if (!this.HasToolWindow(_modelKey))
            {
                MessageBox.Show("请先新建模型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            else
                cm = ModelForm.CurModel;               
            foreach( int ele in eles)
            {
                FrameElement cele = cm.elements[ele] as FrameElement;
                string outs=string.Format("++++++单元号：{0}++++++",ele);
                MessageTool.Tb_out.AppendText(Environment.NewLine + outs);
                MessageTool.Tb_out.AppendText(Environment.NewLine + cele.DPs.ToString());
            }
        }

        private void aboutClicked(object sender, EventArgs e)
        {
            //int num = _LayoutManager.ToolWindows.Count;
            //EasyMidas.ToolPanel tp = _LayoutManager.ToolWindows["MainPanel"] as ToolPanel;
            //ToolPanel.switchTW();
        }

        /// <summary>
        /// 查询验算结果命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tSMI_QuaryRes_Click(object sender, EventArgs e)
        {
            QueryResults QR = new QueryResults();
            QR.Owner = this;
            QR.InitPanels();//初始化
            QR.Show();
        }

        private void tSMI_CheckPara_Click(object sender, EventArgs e)
        {

            SetCheckingForm scf = new SetCheckingForm(this);
            scf.ShowDialog();
        }
        #endregion
        #region 其它事件
        private void statusStrip_Paint(object sender, PaintEventArgs e)
        {
            if (ModelForm == null || ModelForm.IsDisposed)
                return;
            else
            {
                this.ModelInfoLabel.Text = " 节点:" + ModelForm.CurModel.nodes.Count.ToString() +
                     "单元:" + ModelForm.CurModel.elements.Count.ToString() + "--有内力的单元数:" +
                     ModelForm.CurModel.elemforce.Count.ToString();
                this.UnitLabel.Text = " 单位:" + ModelForm.CurModel.unit.Force + "," +
                    ModelForm.CurModel.unit.Length + "," + ModelForm.CurModel.unit.Temper;
            }
        }
        //单元选择框鼠标停留时处理事件
        private void cb_selectEle_MouseHover(object sender, EventArgs e)
        {
            int num = SelectCollection.StringToList(this.cb_selectEle.Text).Count;
            this.cb_selectEle.ToolTipText = "选择单元数:" + num.ToString();
        }
        #endregion

        #region 方法
        //初始化布局
        private void InitFormLayout()
        {
            this.IsMdiContainer = true;
            _LayoutManager = new DockLayoutManager(this, null);

            //挂起工具窗口停靠管理器事务，以加快速度管理
            _LayoutManager.SuspendLayout();
            _LayoutManager.ToolWindows.Add(new MessageTools());
            _LayoutManager.ToolWindows["MessageTool"].Text = "信息窗口";
            _LayoutManager.ToolWindows["MessageTool"].DockTo(DockTargetHost.DockHost, DockPosition.Bottom);
            _LayoutManager.ToolWindows.Add(new ToolPanel());
            _LayoutManager.ToolWindows["MainPanel"].Text = "工具面板";
            _LayoutManager.ToolWindows["MainPanel"].DockTo(DockTargetHost.DockHost, DockPosition.Left);

            //ToolWindow  tt=new MessageTools();
            //tt.State=ToolWindowState.Mdi;
            //tt.Key = "md1";
            //ToolWindow  tt1=new MessageTools();
            //tt1.State=ToolWindowState.Mdi;
            //tt1.Key = "md2";
            //_LayoutManager.ToolWindows.Add(tt);
            //_LayoutManager.ToolWindows.Add(tt1);

            //执行正常的工具窗口停靠管理
            _LayoutManager.ResumeLayout();
            //_LayoutManager.LoadLayout(System.Environment.CurrentDirectory + @"\\DefaultLayout.xml");
        }
        /// <summary>
        /// 设置多标签管理器
        /// </summary>
        private void ConfigureTabbedMdiManager()
        {
            // Create a new instance of the TabbedMdiManager class, which will handle all ToolWindows
            // that have an Mdi state.
            _mdiManager = new TabbedMdiManager(this);

            _mdiManager.Style = RenderStyle.VS2005;
            _mdiManager.AllowClose = true;//不允许关闭
            _mdiManager.AllowModifications = false;//不允许修改
            // Subscribe to the GroupAdded event to reset the appropriate value
            // in the Orientation menu.
            //_mdiManager.GroupAdded += new GroupAddedEventHandler(m_mdiManager_GroupAdded); 
        }

        /// <summary>
        /// 反序列化文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Deserialize(object sender, DoWorkEventArgs e)
        {
            using (FileStream fs = File.OpenRead(_tempFileName))
            {
                Bmodel result = Utilities.Deserialize<Bmodel>(fs,
                    delegate(object sender2, ProgressChangedEventArgs e2)
                    {
                        _BackWorker.ReportProgress(e2.ProgressPercentage);
                    });
                ModelForm.CurModel = result;//存储到当前模型中
            }
        }

        /// <summary>
        /// 指示是否具有当前key的工具窗口
        /// </summary>
        /// <param name="key">工具窗口key</param>
        /// <returns>是否有</returns>
        public bool HasToolWindow(string key)
        {
            bool res = false;
            if (_LayoutManager.ToolWindows[key] != null)
                res = true;
            return res;
        }
        #endregion 

        #region 截面验算用菜单
        private void TSMI_SetSECDesignPara_Click(object sender, EventArgs e)
        {
            SetSecDesignPara ssdp = new SetSecDesignPara();
            ssdp.Owner = this;
            ssdp.InitPanels();//初始化
            ssdp.Show();
        }
        private void 计算长度系数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolPanel.switchUC(0);
        }
        private void 编辑构件类型ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolPanel.switchUC(1);
        }
        private void 编辑等效弯矩系数BetamBetatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolPanel.switchUC(2);
        }
        private void 地震作用放大系数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolPanel.switchUC(3);
        }
        #endregion

        private void 测试用命令ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region 测试荷载组合功能
            List<int> eles = SelectCollection.StringToList(this.cb_selectEle.Text);
            MidasGenModel.model.Bmodel cm;
            //如果单元选择数为0，作出提示
            if (eles.Count == 0)
            {
                MessageTool.Tb_out.AppendText(Environment.NewLine + "*Error*:未选择单元!");
                return;
            }
            if (!this.HasToolWindow(_modelKey))
            {
                MessageBox.Show("请先新建模型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            else
                cm = ModelForm.CurModel;
            string com = "SRSS3";//组合名
            BLoadComb myCom = cm.LoadCombTable.getLoadComb(LCKind.STEEL, com);
            foreach (int ele in eles)
            {
                FrameElement cele = cm.elements[ele] as FrameElement;
                string outs = string.Format("++++++单元号：{0}  组合:{1}++++++", ele,com);
                MessageTool.Tb_out.AppendText(Environment.NewLine);
                MessageTool.Tb_out.AppendText(outs);
                ElemForce EFcom = cm.CalElemForceComb(myCom, ele);
                MessageTool.Tb_out.AppendText(Environment.NewLine+"[i]--" + EFcom.Force_i.ToString());
                MessageTool.Tb_out.AppendText(Environment.NewLine + "[4/8]--" + EFcom.Force_48.ToString());
                MessageTool.Tb_out.AppendText(Environment.NewLine + "[j]--" + EFcom.Force_j.ToString());
            }
            #endregion
        }

        private void TSMI_WriteEleRes_Click(object sender, EventArgs e)
        {
            Bmodel mm = this.ModelForm.CurModel;
            CheckRes CR = this.ModelForm.CheckTable;

            string CurDir = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
            string myFile = Path.Combine(CurDir, "CodeCheck_bySec.txt");
            //输出验算结果
            CodeCheck.WriteAllCheckRes(ref mm, ref CR, myFile);
            MessageTool.Tb_out.AppendText(Environment.NewLine+"验算文件写出到："+myFile);
            //用记事本打开
            System.Diagnostics.Process.Start("notepad.exe", myFile);
        }

        private void TSMI_WriteEleRes_byEle_Click(object sender, EventArgs e)
        {
            Bmodel mm = this.ModelForm.CurModel;
            CheckRes CR = this.ModelForm.CheckTable;

            string CurDir = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
            string myFile = Path.Combine(CurDir, "CodeCheck_byEle.txt");
            //输出验算结果
            CodeCheck.WriteAllCheckRes2(ref mm, ref CR, myFile);
            MessageTool.Tb_out.AppendText(Environment.NewLine + "验算文件写出到：" + myFile);
            //用记事本打开
            System.Diagnostics.Process.Start("notepad.exe", myFile);
        }

        #region 设计菜单组
        private void 显示单元设计内力ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bmodel curMM = this.ModelForm.CurModel;//模型数据
            BCheckModel curCM = this.ModelForm.CheckModel;//设计数据
            List<int> eles = SelectCollection.StringToList(this.cb_selectEle.Text);//选择单元
            if (eles.Count == 0)
            {
                MessageTool.Tb_out.AppendText(Environment.NewLine + "*Error*:未选择单元!");
                return;
            }
            TableForm tf = new TableForm();
            tf.Text = "单元设计内力表";
            tf.Key = "DesignForceTable";
            tf.State = ToolWindowState.Mdi;
            tf.writeDesignForce(ref curMM,ref curCM,eles);//输出数据
            if (!this.HasToolWindow("DesignForceTable"))
            {
                _LayoutManager.ToolWindows.Add(tf);               
            }
            else
            {
                ToolWindow tw = _LayoutManager.ToolWindows["DesignForceTable"];
                _LayoutManager.ToolWindows.Remove(tw);
                _LayoutManager.ToolWindows.Add(tf); 
            }           
        }
        
        #endregion

        private void 节点荷载批量放大ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModifyNodeF mnf = new ModifyNodeF();
            mnf.Owner = this;
            mnf.InitForm();
            mnf.ShowInTaskbar = false;
            mnf.ShowDialog();
            
        }

        //显示关闭坐标轴
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            bool bt=ModelForm.hasAxis;
            ModelForm.hasAxis = !bt;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ModelForm.hasElem = true;
            ModelForm.Eye_distance = 50;
        }


    }
}
