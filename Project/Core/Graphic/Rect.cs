using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachShooting
{
    internal struct Rect
    {
        #region フィールド
        /// <summary>
        /// X座標
        /// </summary>
        private double x;

        /// <summary>
        /// 座標
        /// </summary>
        private double y;

        /// <summary>
        /// 幅
        /// </summary>
        private double w;

        /// <summary>
        /// 高さ
        /// </summary>
        private double h;
        #endregion
        #region プロパティ
        /// <summary>
        /// X座標
        /// </summary>
        internal double X
        {
            get { return this.x; }
            set { this.x = value; }
        }

        /// <summary>
        /// Y座標
        /// </summary>
        internal double Y
        {
            get { return this.y; }
            set { this.y = value; }
        }

        /// <summary>
        /// 幅
        /// </summary>
        internal double W
        {
            get { return this.w; }
            set { this.w = value; }
        }

        /// <summary>
        /// 幅
        /// </summary>
        internal double H
        {
            get { return this.h; }
            set { this.h = value; }
        }

        /// <summary>
        /// 左
        /// </summary>
        internal double L
        {
            get { return this.x; }
            set { this.x = value; }
        }

        /// <summary>
        /// 右
        /// </summary>
        internal double T
        {
            get { return this.y; }
            set { this.y = value; }
        }

        /// <summary>
        /// 右
        /// </summary>
        internal double R
        {
            get { return this.x + this.w; }
            set { this.x = value - this.w; }
        }

        /// <summary>
        /// 下
        /// </summary>
        internal double B
        {
            get { return this.y + this.h; }
            set { this.y = value - this.h; }
        }

        /// <summary>
        /// 中心のX座標
        /// </summary>
        internal double CX
        {
            get { return this.x + this.w / 2; }
            set { this.x = value - this.w / 2; }
        }

        /// <summary>
        /// 中心のY座標
        /// </summary>
        internal double CY
        {
            get { return this.y + this.h / 2; }
            set { this.y = value - this.h / 2; }
        }
        #endregion
        #region コンストラクタ
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        internal Rect(double x, double y, double w, double h)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dot1">左上のX座標</param>
        /// <param name="dot2">右下のX座標</param>
        internal Rect(Vec dot1, Vec dot2)
        {
            double l = Math.Min(dot1.X, dot2.X);
            double r = Math.Max(dot1.X, dot2.X);
            double t = Math.Min(dot1.Y, dot2.Y);
            double b = Math.Max(dot1.Y, dot2.Y);

            this.x = l;
            this.y = t;
            this.w = r - l;
            this.h = b - t;
        }
        #endregion
        #region メソッド
        #region 演算子
        /// <summary>
        /// 2つの矩形がきっちり入る矩形を返します
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <returns></returns>
        public static Rect operator +(Rect r1, Rect r2)
        {
            double l = Math.Min(r1.L, r2.L);
            double r = Math.Max(r1.R, r2.R);
            double t = Math.Min(r1.T, r2.T);
            double b = Math.Max(r1.B, r2.B);

            return new Rect(l, t, r - l, b - t);
        }
        #endregion

        /// <summary>
        /// 移動を行います
        /// </summary>
        /// <param name="vec"></param>
        internal void Move(Vec vec)
        {
            this.x += vec.X;
            this.y += vec.Y;
        }
        #endregion
    }
}
