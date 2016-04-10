using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircleLib
{
    /// <summary>
    /// 点
    /// </summary>
    public struct Dot
    {
        #region フィールド
        /// <summary>
        /// X座標
        /// </summary>
        private double x;

        /// <summary>
        /// Y座標
        /// </summary>
        private double y;
        #endregion
        #region プロパティ
        /// <summary>
        /// X座標
        /// </summary>
        public double X
        {
            get { return this.x; }
            set { this.x = value; }
        }

        /// <summary>
        /// Y座標
        /// </summary>
        public double Y
        {
            get { return this.y; }
            set { this.y = value; }
        }

        /// <summary>
        /// この図形を囲む矩形
        /// </summary>
        public Rect Rect
        {
            get
            {
                return new Rect(this, this);
            }
        }
        #endregion
        #region コンストラクタ
        /// <summary>
        /// 新しいイントランスを作成します
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">y座標</param>
        public Dot(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        #endregion
        #region メソッド
        /// <summary>
        /// 移動を行います
        /// </summary>
        /// <param name="vec"></param>
        public void Move(Vec vec)
        {
            this.x += vec.X;
            this.y += vec.Y;
        }

        /// <summary>
        /// ベクトルをドットに変換します
        /// </summary>
        /// <param name="vec"></param>
        public static explicit operator Dot(Vec vec)
        {
            return new Dot(vec.X, vec.Y);
        }
        #endregion
    }
}
