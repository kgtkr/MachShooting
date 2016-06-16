using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachShooting.Graphic
{
    /// <summary>
    /// ベクトル
    /// </summary>
    public struct Vec
    {
        #region フィールド
        /// <summary>
        /// X方向の移動量
        /// </summary>
        private double x;

        /// <summary>
        /// Y方向の移動量
        /// </summary>
        private double y;
        #endregion
        #region プロパティ
        /// <summary>
        /// X方向の移動量
        /// </summary>
        public double X
        {
            get { return this.x; }
            set { this.x = value; }
        }

        /// <summary>
        /// Y方向の移動量
        /// </summary>
        public double Y
        {
            get { return this.y; }
            set { this.y = value; }
        }

        /// <summary>
        /// ベクトルの長さ
        /// </summary>
        public double Length
        {
            get { return Math.Sqrt(this * this); }
            set
            {
                double r = this.Rad;
                this.x = Math.Cos(r) * value;
                this.y = Math.Sin(r) * value;
            }
        }

        /// <summary>
        /// ベクトルの長さの2乗
        /// </summary>
        public double LengthSquare
        {
            get { return this * this; }
        }

        /// <summary>
        /// 長さ1のベクトル
        /// </summary>
        public Vec Unit
        {
            get
            {
                double len = this.Length;
                return len == 0 ? this : this / len;
            }
        }

        /// <summary>
        /// ラジアン
        /// </summary>
        public double Rad
        {
            get { return Math.Atan2(this.y, this.x); }
            set
            {
                double l = this.Length;
                this.x = Math.Cos(value) * l;
                this.y = Math.Sin(value) * l;
            }
        }
        #endregion
        #region コンストラクタ
        /// <summary>
        /// X方向の移動量、Y方向の移動量から新しいインスタンスを作成します
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vec(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        #endregion
        #region メソッド
        public override bool Equals(object obj)
        {
            Vec? vec = obj as Vec?;
            if (vec == null)
            {
                return false;
            }
            return this == vec;
        }

        public override int GetHashCode()
        {
            return this.x.GetHashCode() + this.y.GetHashCode();
        }
        #region その他new
        /// <summary>
        /// ラジアンと長さから新しいインスタンスを取得します
        /// </summary>
        /// <param name="rad"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static Vec NewRadLength(double rad, double length)
        {
            Vec vec = new Vec();

            vec.x = Math.Cos(rad) * length;
            vec.y = Math.Sin(rad) * length;

            return vec;
        }
        #endregion
        #region 演算子
        /// <summary>
        /// 同じならtrue
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator ==(Vec v1, Vec v2)
        {
            return v1.x == v2.x && v1.y == v2.y;
        }

        /// <summary>
        /// 違うならtrue
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator !=(Vec v1, Vec v2)
        {
            return v1.x != v2.x || v1.y != v2.y;
        }

        /// <summary>
        /// ベクトルを足し算します
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vec operator +(Vec v1, Vec v2)
        {
            return new Vec(v1.x + v2.x, v1.y + v2.y);
        }

        /// <summary>
        /// ベクトルを引き算します
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vec operator -(Vec v1, Vec v2)
        {
            return new Vec(v1.x - v2.x, v1.y - v2.y);
        }

        /// <summary>
        /// 掛け算
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Vec operator *(Vec v, double d)
        {
            return new Vec(v.x * d, v.y * d);
        }

        /// <summary>
        /// 割り算
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Vec operator /(Vec v, double d)
        {
            return new Vec(v.x / d, v.y / d);
        }

        /// <summary>
        /// 内積
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double operator *(Vec v1, Vec v2)
        {
            return v1.x * v2.x + v1.y * v2.y;
        }

        /// <summary>
        /// 外積
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double operator /(Vec v1, Vec v2)
        {
            return v1.x * v2.y - v1.y * v2.x;
        }
        #endregion
        #endregion
    }
}