using System;

namespace MidasGenModel.DelaunayTriangulator
{
    /// <summary>A class defining a triangle and some methods for the Delaunay algorithm.</summary>
    public class Triangle
    {
        /// <summary>The first vertex of the triangle.</summary>
        public Point Vertex1;
        /// <summary>The second vertex of the triangle.</summary>
        public Point Vertex2;
        /// <summary>The third vertex of the triangle.</summary>
        public Point Vertex3;
        
        #region Constructor

        /// <summary>Constructs a triangle from three points.</summary>
        /// <param name="vertex1">The first vertex of the triangle.</param>
        /// <param name="vertex2">The second vertex of the triangle.</param>
        /// <param name="vertex3">The third vertex of the triangle.</param>
        public Triangle(Point vertex1, Point vertex2, Point vertex3)
        {
            this.Vertex1 = vertex1;
            this.Vertex2 = vertex2;
            this.Vertex3 = vertex3;
        }

        #endregion

        #region Methods

        /// <summary>Tests if a point lies in the circumcircle of the triangle.</summary>
        /// <param name="point">A <see cref="Point"/>.</param>
        /// <returns>For a counterclockwise order of the vertices of the triangle, this test is 
        /// <list type ="bullet">
        /// <item>positive if <paramref name="point"/> lies inside the circumcircle.</item>
        /// <item>zero if <paramref name="point"/> lies on the circumference of the circumcircle.</item>
        /// <item>negative if <paramref name="point"/> lies outside the circumcircle.</item></list></returns>
        /// <remarks>The vertices of the triangle must be arranged in counterclockwise order or the result
        /// of this test will be reversed. This test ignores the z-coordinate of the vertices.</remarks>
        public double ContainsInCircumcircle(Point point)
        {
            double ax = this.Vertex1.X - point.X;
            double ay = this.Vertex1.Y - point.Y;
            double bx = this.Vertex2.X - point.X;
            double by = this.Vertex2.Y - point.Y;
            double cx = this.Vertex3.X - point.X;
            double cy = this.Vertex3.Y - point.Y;

            double det_ab = ax * by - bx * ay;
            double det_bc = bx * cy - cx * by;
            double det_ca = cx * ay - ax * cy;

            double a_squared = ax * ax + ay * ay;
            double b_squared = bx * bx + by * by;
            double c_squared = cx * cx + cy * cy;

            return a_squared * det_bc + b_squared * det_ca + c_squared * det_ab;
        }

        /// <summary>Tests if two triangles share at least one vertex.</summary>
        /// <param name="triangle">A <see cref="Triangle"/>.</param>
        /// <returns>Returns true if two triangles share at least one vertex, false otherwise.</returns>
        public bool SharesVertexWith(Triangle triangle)
        {
            if ( this.Vertex1.X == triangle.Vertex1.X && this.Vertex1.Y == triangle.Vertex1.Y ) return true;
            if ( this.Vertex1.X == triangle.Vertex2.X && this.Vertex1.Y == triangle.Vertex2.Y ) return true;
            if ( this.Vertex1.X == triangle.Vertex3.X && this.Vertex1.Y == triangle.Vertex3.Y ) return true;

            if ( this.Vertex2.X == triangle.Vertex1.X && this.Vertex2.Y == triangle.Vertex1.Y ) return true;
            if ( this.Vertex2.X == triangle.Vertex2.X && this.Vertex2.Y == triangle.Vertex2.Y ) return true;
            if ( this.Vertex2.X == triangle.Vertex3.X && this.Vertex2.Y == triangle.Vertex3.Y ) return true;

            if ( this.Vertex3.X == triangle.Vertex1.X && this.Vertex3.Y == triangle.Vertex1.Y ) return true;
            if ( this.Vertex3.X == triangle.Vertex2.X && this.Vertex3.Y == triangle.Vertex2.Y ) return true;
            if ( this.Vertex3.X == triangle.Vertex3.X && this.Vertex3.Y == triangle.Vertex3.Y ) return true;

            return false;
        }

        /// <summary>
        /// 求得三角形最大内角(度)
        /// </summary>
        /// <returns>最大内角，单位：度</returns>
        public double getMaxAngle()
        {
            double res = 0;
            double a, b, c;
            a = this.getEdgeLength_a();
            b = this.getEdgeLength_b();
            c = this.getEdgeLength_c();
            

            double Aa = Math.Acos((Math.Pow(b, 2) + Math.Pow(c, 2) - Math.Pow(a, 2)) /( 2 * b * c))*180/Math.PI;
            double Ab = Math.Acos((Math.Pow(c, 2) + Math.Pow(a, 2) - Math.Pow(b, 2)) / (2 * c * a)) * 180 / Math.PI;
            double Ac = Math.Acos((Math.Pow(a, 2) + Math.Pow(b, 2) - Math.Pow(c, 2)) /( 2 * a * b)) * 180 / Math.PI;

            res = Math.Max(Math.Max(Aa, Ab), Ac);
            return res;
        }
        /// <summary>
        /// 求得边长a
        /// </summary>
        /// <returns>边长a</returns>
        public double getEdgeLength_a()
        {
            double res;
            res = Math.Sqrt(Math.Pow(Vertex1.X - Vertex2.X, 2) +
                Math.Pow(Vertex1.Y - Vertex2.Y, 2) +
                Math.Pow(Vertex1.Z - Vertex2.Z, 2));
            return res;
        }
        /// <summary>
        /// 求得边长b
        /// </summary>
        /// <returns>边长b</returns>
        public double getEdgeLength_b()
        {
            double res;
            res = Math.Sqrt(Math.Pow(Vertex2.X - Vertex3.X, 2) +
                Math.Pow(Vertex2.Y - Vertex3.Y, 2) +
                Math.Pow(Vertex2.Z - Vertex3.Z, 2));
            return res;
        }
        /// <summary>
        /// 求得边长c
        /// </summary>
        /// <returns>边长c</returns>
        public double getEdgeLength_c()
        {
            double res;
            res = Math.Sqrt(Math.Pow(Vertex1.X - Vertex3.X, 2) +
                Math.Pow(Vertex1.Y - Vertex3.Y, 2) +
                Math.Pow(Vertex1.Z - Vertex3.Z, 2));
            return res;
        }

        #endregion
    }
}
