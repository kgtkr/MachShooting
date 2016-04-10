using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircleLib
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
        private Dot dot;

        /// <summary>
        /// 半径
        /// </summary>
        private double r;
        #endregion
        #region プロパティ
        /// <summary>
        /// 中心座標
        /// </summary>
        public Dot Dot
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
        public Rect Rect
        {
            get
            {
                return new Rect(new Dot(this.dot.X - this.r, this.dot.Y - this.r), new Dot(this.dot.X + this.r, this.dot.Y + this.r));
            }
        }
        #endregion
        #region コンストラクタ
        /// <summary>
        ///
        /// </summary>
        /// <param name="dot">中心座標</param>
        /// <param name="r">半径</param>
        public Circle(Dot dot, double r)
        {
            this.dot = dot;
            this.r = r;
        }

        public Circle(double x,double y, double r)
        {
            this.dot = new Dot(x, y);
            this.r = r;
        }
        #endregion
        #region メソッド
        /// <summary>
        /// 移動を行います
        /// </summary>
        /// <param name="vec"></param>
        public void Move(Vec vec)
        {
            this.dot.Move(vec);
        }
        #endregion
    }
}
