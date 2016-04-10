using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircleLib
{
    /// <summary>
    /// カプセル
    /// </summary>
    public struct Capsule
    {
        #region フィールド
        /// <summary>
        /// 線分
        /// </summary>
        private Line line;

        /// <summary>
        /// 半径
        /// </summary>
        private double r;
        #endregion
        #region プロパティ
        /// <summary>
        /// 線分
        /// </summary>
        public Line Line
        {
            get { return this.line; }
            set { this.line = value; }
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
        /// 点1
        /// </summary>
        public Dot Dot1
        {
            get { return this.line.Dot1; }
            set { this.line.Dot1 = value; }
        }

        /// <summary>
        /// 点2
        /// </summary>
        public Dot Dot2
        {
            get { return this.line.Dot2; }
            set { this.line.Dot2 = value; }
        }

        /// <summary>
        /// 点1のX座標
        /// </summary>
        public double Dot1X
        {
            get { return this.Dot1.X; }
            set
            {
                Dot dot = this.Dot1;
                dot.X = value;
                this.Dot1 = dot;
            }
        }

        /// <summary>
        /// 点1のY座標
        /// </summary>
        public double Dot1Y
        {
            get { return this.Dot1.Y; }
            set
            {
                Dot dot = this.Dot1;
                dot.Y = value;
                this.Dot1 = dot;
            }
        }

        /// <summary>
        /// 点2のX座標
        /// </summary>
        public double Dot2X
        {
            get { return this.Dot2.X; }
            set
            {
                Dot dot = this.Dot2;
                dot.X = value;
                this.Dot2 = dot;
            }
        }

        /// <summary>
        /// 点2のY座標
        /// </summary>
        public double Dot2Y
        {
            get { return this.Dot2.Y; }
            set
            {
                Dot dot = this.Dot2;
                dot.Y = value;
                this.Dot2 = dot;
            }
        }


        /// <summary>
        /// この図形を囲む矩形
        /// </summary>
        public Rect Rect
        {
            get
            {
                double minX = Math.Min(this.Dot1X, this.Dot2X)-this.r;
                double minY = Math.Min(this.Dot1Y, this.Dot2Y)-this.r;
                double maxX = Math.Max(this.Dot1X, this.Dot2X)+this.r;
                double maxY = Math.Max(this.Dot1Y, this.Dot2Y)+this.r;

                return new Rect(new Dot(minX,minY), new Dot(maxX,maxY));
            }
        }
        #endregion
        #region コンストラクタ
        /// <summary>
        /// 
        /// </summary>
        /// <param name="line">線分</param>
        /// <param name="r">半径</param>
        public Capsule(Line line,double r)
        {
            this.line = line;
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
            this.line.Move(vec);
        }
        #endregion
    }
}
