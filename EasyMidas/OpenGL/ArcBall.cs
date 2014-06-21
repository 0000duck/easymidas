using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace EasyMidas.OpenGL
{
    /// <summary>
    /// 轨迹球类:用于鼠标旋转
    /// </summary>
    [Serializable()]
    public class ArcBall
    {
        bool mouseDownFlag;//鼠标按下指示
        float _angle, _radiusRadius;//转角，长度，轨迹球半径
        //转动矩阵
        double[] _lastTransform = new Double[16] { 
            1, 0, 0, 0,
            0, 1, 0, 0, 
            0, 0, 1, 0, 
            0, 0, 0, 1 };
        //起始向量，终点向量，转轴向量
        Vector3 _startPosition, _endPosition, _normalVector = new Vector3(0, 1, 0);
        int _width;
        int _height;

        float _translateZ;
        float _translateY;
        float _translateX;

        private float _scale = 1.0f;//缩放因子，目前未使用
        private float _rRatio = 0.4f;//用于调整轨迹球半径大小，长宽最小值的倍数
        private float _angRatio = 1.0f;//用于调整转动的大小

        private Vector3 _vectorCenterEye;//物体指向眼的向量
        private Vector3 _vectorUp;//相机向上向量
        private Vector3 _vectorRight;//相机向右向量


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="eyex">眼位x</param>
        /// <param name="eyey">眼位Y</param>
        /// <param name="eyez">眼位</param>
        /// <param name="centerx">物位</param>
        /// <param name="centery">物位</param>
        /// <param name="centerz">物位</param>
        /// <param name="upx">上</param>
        /// <param name="upy">上</param>
        /// <param name="upz">上</param>
        public ArcBall(float eyex, float eyey, float eyez,
            float centerx, float centery, float centerz,
            float upx, float upy, float upz)
        {
            SetCamera(eyex, eyey, eyez, centerx, centery, centerz, upx, upy, upz);
        }
        /// <summary>
        /// 根据视口宽度设置轨迹球的半径
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void SetBounds(int width, int height)
        {
            this._width = width; this._height = height;
            float minH=width < height ? width : height;
            _radiusRadius = _rRatio * minH;//轨迹球半径取0.4倍的最小视口宽
        }

        public void MouseDown(int x, int y)
        {
            this._startPosition = GetArcBallPosition(x, y);

            mouseDownFlag = true;
        }
        /// <summary>
        /// 由屏幕x，y位置求得轨迹球曲面上的点
        /// </summary>
        /// <param name="x">屏幕位置点x</param>
        /// <param name="y">屏幕位置点y</param>
        /// <returns>当前点的单位向量</returns>
        private Vector3 GetArcBallPosition(int x, int y)
        {
            var rx = x - _width / 2;
            var ry = _height / 2 - y;

            var zz = _radiusRadius*_radiusRadius - rx * rx - ry * ry;
            var zz1 = Math.Pow(_radiusRadius, 2) / 2 - rx * rx - ry * ry;
            var zz2 = Math.Pow(_radiusRadius,2)/2 / 
                Math.Sqrt(Math.Pow(rx,2)+Math.Pow(ry,2));
            var rz = (zz1 > 0 ? Math.Sqrt(zz) : zz2);

            Vector3 result = new Vector3(
                (float)(rx * _vectorRight.X + ry * _vectorUp.X + rz * _vectorCenterEye.X),
                (float)(rx * _vectorRight.Y + ry * _vectorUp.Y + rz * _vectorCenterEye.Y),
                (float)(rx * _vectorRight.Z + ry * _vectorUp.Z + rz * _vectorCenterEye.Z)
                );
            result.Normalize();
            return result;
        }


        public void MouseMove(int x, int y)
        {
            if (mouseDownFlag)
            {
                this._endPosition = GetArcBallPosition(x, y);
                var cosAngle = Vector3.Dot(_startPosition,_endPosition) / (_startPosition.Length * _endPosition.Length);
                if (cosAngle > 1) { cosAngle = 1; }
                else if (cosAngle < -1) { cosAngle = -1; }
                var angle = _angRatio * (float)(Math.Acos(cosAngle) / Math.PI * 180);
                System.Threading.Interlocked.Exchange(ref _angle, angle);
                _normalVector =Vector3.Cross(_startPosition,_endPosition);
                _startPosition = _endPosition;
            }
        }

        public void MouseUp(int x, int y)
        {
            mouseDownFlag = false;
        }

        public void TransformMatrix()
        {           
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Rotate(2 * _angle, _normalVector.X, _normalVector.Y, _normalVector.Z);
            System.Threading.Interlocked.Exchange(ref _angle, 0);
            GL.MultMatrix(_lastTransform);
            GL.GetDouble(GetPName.ModelviewMatrix, _lastTransform);
            GL.PopMatrix();
            GL.Translate(_translateX, _translateY, _translateZ);
            GL.MultMatrix(_lastTransform);
            GL.Scale(Scale, Scale, Scale);
        }

        /// <summary>
        /// Default camera is at positive Z axis to look at negtive Z axis with up vector to positive Y axis.
        /// </summary>
        /// <param name="eyex"></param>
        /// <param name="eyey"></param>
        /// <param name="eyez"></param>
        /// <param name="centerx"></param>
        /// <param name="centery"></param>
        /// <param name="centerz"></param>
        /// <param name="upx"></param>
        /// <param name="upy"></param>
        /// <param name="upz"></param>
        public void SetCamera(float eyex, float eyey, float eyez,
            float centerx, float centery, float centerz,
            float upx, float upy, float upz)
        {
            _vectorCenterEye = new Vector3(eyex - centerx, eyey - centery, eyez - centerz);
            _vectorCenterEye.Normalize();
            _vectorUp = new Vector3(upx, upy, upz);
            _vectorRight = Vector3.Cross(_vectorUp,_vectorCenterEye);
            _vectorRight.Normalize();
            _vectorUp = Vector3.Cross(_vectorCenterEye,_vectorRight);
            _vectorUp.Normalize();
        }

        public void GoUp(float interval)
        {
            this._translateX += this._vectorUp.X * interval;
            this._translateY += this._vectorUp.Y * interval;
            this._translateZ += this._vectorUp.Z * interval;
        }
        public void GoDown(float interval)
        {
            this._translateX -= this._vectorUp.X * interval;
            this._translateY -= this._vectorUp.Y * interval;
            this._translateZ -= this._vectorUp.Z * interval;
        }
        public void GoLeft(float interval)
        {
            this._translateX -= this._vectorRight.X * interval;
            this._translateY -= this._vectorRight.Y * interval;
            this._translateZ -= this._vectorRight.Z * interval;
        }
        public void GoRight(float interval)
        {
            this._translateX += this._vectorRight.X * interval;
            this._translateY += this._vectorRight.Y * interval;
            this._translateZ += this._vectorRight.Z * interval;
        }

        
        public float Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }


        public void SetTranslate(double x, double y, double z)
        {
            this._translateX = (float)x;
            this._translateY = (float)y;
            this._translateZ = (float)z;
        }

        public void GoFront(int interval)
        {
            this._translateX -= this._vectorCenterEye.X * interval;
            this._translateY -= this._vectorCenterEye.Y * interval;
            this._translateZ -= this._vectorCenterEye.Z * interval;
        }
        public void GoBack(int interval)
        {
            this._translateX += this._vectorCenterEye.X * interval;
            this._translateY += this._vectorCenterEye.Y * interval;
            this._translateZ += this._vectorCenterEye.Z * interval;
        }
    }
}
