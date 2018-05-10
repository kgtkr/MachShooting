using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachShooting
{
    /// <summary>
    /// 円
    /// </summary>
    public struct Circle
    {
        #region フィールド
        /// <summary>
        /// 中心座標
        /// </summary>
        private Vec dot;

        /// <summary>
        /// 半径
        /// </summary>
        private double r;
        #endregion
        #region プロパティ
        /// <summary>
        /// 中心座標
        /// </summary>
        public Vec Dot
        {
            get { return this.dot; }
            set { this.dot = value; }
        }

        /// <summary>
        /// 半径
        /// </summary>
        public double R
        {
            get { return this.r; }
            set { this.r = value; }
        }

        /// <summary>
        /// X座標
        /// </summary>
        public double X
        {
            get { return this.dot.X; }
            set { this.dot.X = value; }
        }

        /// <summary>
        /// Y座標
        /// </summary>
        public double Y
        {
            get { return this.dot.Y; }
            set { this.dot.Y = value; }
        }
        /// <summary>
        /// この図形を囲む矩形
        /// </summary>
        internal Rect Rect
        {
            get
            {
                return new Rect(new Vec(this.dot.X - this.r, this.dot.Y - this.r), new Vec(this.dot.X + this.r, this.dot.Y + this.r));
            }
        }
        #endregion
        #region コンストラクタ
        /// <summary>
        ///
        /// </summary>
        /// <param name="dot">中心座標</param>
        /// <param name="r">半径</param>
        public Circle(Vec dot, double r)
        {
            this.dot = dot;
            this.r = r;
        }

        public Circle(double x, double y, double r)
        {
            this.dot = new Vec(x, y);
            this.r = r;
        }
        #endregion
    }
}
