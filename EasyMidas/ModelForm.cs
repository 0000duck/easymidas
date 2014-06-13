using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Xceed.DockingWindows;
using MidasGenModel.model;
using MidasGenModel.Design;

using OpenTK.Graphics.OpenGL;
using OpenTK;
using OpenTK.Graphics;

namespace EasyMidas
{
    public partial class ModelForm1 : ToolWindow
    {
        #region 公有数据
        public Bmodel CurModel;//模型数据
        public CheckRes CheckTable;//截面验算结果表
        public BCheckModel CheckModel;//截面验算参数数据
        public bool GLLoaded = false;//指示是否GL控件已被加载
        
        //绘图控制数据
        public bool hasAxis = true;//指示是否显示坐标轴
        static PointF angle = new PointF();//转角
        static PointF last = new PointF();//最后一次转角
        static Point offset = new Point();//鼠标偏移量
        #endregion

        public ModelForm1()
        {
            InitializeComponent();
            CurModel = new Bmodel();
            CheckTable = new CheckRes();
            CheckModel = new BCheckModel();
            //初始化控件
            this.State = ToolWindowState.Mdi;
            this.Key = "Model";
            
        }
        #region 窗口事件
        //重载窗口OnLoad事件
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(Color.Black);//背景
            GL.Enable(EnableCap.DepthTest);

            Application.Idle += Application_Idle; //绑定程序空闲事件

            button5.Text = GL.GetString(StringName.Version);//GL版本

            glControl1_Resize(this, EventArgs.Empty);   // Ensure the Viewport is set up correctly
        }
        //重载窗口OnCloseing事件
        protected  void OnClosing(CancelEventArgs e)
        {
            Application.Idle -= Application_Idle;

        }

        private void ChildForm_Paint(object sender, PaintEventArgs e)
        {
            label1.Text = CurModel.nodes.Count.ToString() +
                " | " + CurModel.elements.Count.ToString() + " [" +
                CurModel.elemforce.Count.ToString()+"]";
        }
        #endregion
        #region GL控件相关方法和事件处理函数
        private void Render()
        {           
            //初始化屏幕
            //1.清理屏幕           
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(0, 0, 0, 0);//背景为黑
            //2.设置矩阵模式：GL_Projection或者GL_Modelview
            GL.MatrixMode(MatrixMode.Modelview);//指定当前矩阵模型视图矩阵
            //3.加载当前矩阵模式
            GL.LoadIdentity();//将当前矩阵转为单位矩阵

            if (glControl1.Focused)
                GL.Color3(Color.Yellow);
            else
                GL.Color3(Color.SeaGreen);
            //视口设置
            //4.视口设置
            Matrix4 lookat = Matrix4.LookAt(-3, -4, 4, 0, 0, 0, 0, 0, 1);
            GL.LoadMatrix(ref lookat);//加载视口矩阵
            GL.Rotate(angle.X, 1, 0, 0);//旋转物体
            GL.Rotate(angle.Y, 0, 0, 1);
            //显示设置
            //5.显示设置

            //绘图
            if (hasAxis)
            {
                DrawAxis();//画坐标轴
            }           
            //绘图最后：swapBuffers()
            glControl1.SwapBuffers();//调换缓存
        }

        //画坐标轴函数
        private void DrawAxis()
        {
            GL.LineWidth(5);//线宽
            //GL.Begin(BeginMode.Lines);
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Red);           
            GL.Vertex3(0.0f,0.0f,0.0f);
            GL.Vertex3(1.0f, 0.0f, 0.0f);
            GL.Color3(Color.Green);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 1.0f, 0.0f);
            GL.Color3(Color.Blue);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, 1.0f);
            GL.End();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            base.OnLoad(e);//触发原来的onload
            GLLoaded = true;
            //重要命令，用于节省CPU使用率
            //GraphicsContext.CurrentContext.VSync = true;
            GraphicsContext.CurrentContext.SwapInterval = 1;

            //SetupViewport();//建立视口
            GL.Enable(EnableCap.DepthTest);//深度测试
            //触发更新控件大事件
            glControl1_Resize(glControl1,EventArgs.Empty);
        }
        /// <summary>
        /// 建立视口
        /// </summary>
        private void SetupViewport()
        {
            int w = glControl1.Width;
            int h = glControl1.Height;
            GL.MatrixMode(MatrixMode.Projection);//当前矩阵为投影矩阵
            GL.LoadIdentity();//单位化

            //设置二维视口用GL.Ortho
            // Bottom-left corner pixel has coordinate (0, 0)
            //GL.Ortho(-w / 2, w / 2, -h / 2, h / 2, -1, 1); 
            //GL.Viewport(0, 0, w, h); // Use all of the glControl painting area

            //设置三维视口用GL
            
            
        }
        //重绘事件
        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!GLLoaded)
                return;
            Render();//显示当前图形
        }

        private void glControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!GLLoaded)
                return;
            if (e.KeyCode == Keys.Space)
            {
                glControl1.Invalidate();//重新绘制图形
            }
        }
        //控件大小调整事件 
        //改变大小时触发，在程序启动时将执行一次
        private void glControl1_Resize(object sender, EventArgs e)
        {
            if (!GLLoaded)//如果没有加载控件，则返回
                return;

            if (glControl1.ClientSize.Height == 0)
                glControl1.ClientSize = new System.Drawing.Size(glControl1.ClientSize.Width, 1);

            GL.Viewport(0, 0, glControl1.ClientSize.Width, glControl1.ClientSize.Height);
            //设置3D绘图画布
            float aspect_ratio=Width/(float)Height;//求得控件宽高比
            //三维透视矩阵
            Matrix4 perpective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                aspect_ratio, 1, 64);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perpective);//加载透视矩阵
        }
        #endregion

        #region Application_Idle 事件

        void Application_Idle(object sender, EventArgs e)
        {
            while (glControl1.IsIdle)
            {
                Render();
            }
        }

        #endregion

        private void glControl1_MouseDown(object sender, MouseEventArgs e)
        {
            offset = e.Location;
            last.X = angle.X; last.Y = angle.Y;
        }

        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.Y - offset.Y > 0) angle.X = last.X + e.Y - offset.Y;
                else angle.X = last.X - offset.Y + e.Y;
                if (e.X - offset.X > 0) angle.Y = last.Y + e.X - offset.X;
                else angle.Y = last.Y - offset.X + e.X;
            }
            glControl1.Refresh();
        }
    }
}
