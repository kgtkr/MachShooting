﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachShooting
{
    /// <summary>
    /// 線
    /// </summary>
    internal struct Line
    {
        #region フィールド
        /// <summary>
        /// 点1
        /// </summary>
        private Vec dot1;

        /// <summary>
        /// 点2
        /// </summary>
        private Vec dot2;
        #endregion
        #region プロパティ
        /// <summary>
        /// 点1
        /// </summary>
        internal Vec Dot1
        {
            get { return this.dot1; }
            set { this.dot1 = value; }
        }

        /// <summary>
        /// 点2
        /// </summary>
        internal Vec Dot2
        {
            get { return this.dot2; }
            set { this.dot2 = value; }
        }

        /// <summary>
        /// 点1のX座標
        /// </summary>
        internal double X1
        {
            get { return this.dot1.X; }
            set { this.dot1.X = value; }
        }

        /// <summary>
        /// 点1のY座標
        /// </summary>
        internal double Y1
        {
            get { return this.dot1.Y; }
            set { this.dot1.Y = value; }
        }

        /// <summary>
        /// 点2のX座標
        /// </summary>
        internal double X2
        {
            get { return this.dot2.X; }
            set { this.dot2.X = value; }
        }

        /// <summary>
        /// 点2のY座標
        /// </summary>
        internal double Y2
        {
            get { return this.dot2.Y; }
            set { this.dot2.Y = value; }
        }

        /// <summary>
        /// この図形を囲む矩形
        /// </summary>
        internal Rect Rect
        {
            get
            {
                return new Rect(this.dot1, this.dot2);
            }
        }

        /// <summary>
        /// 点1→点2のベクトル
        /// </summary>
        internal Vec Vec
        {
            get
            {
                return new Vec(this.dot2.X - this.dot1.X, this.dot2.Y - this.dot1.Y);
            }
        }

        /// <summary>
        /// 点
        /// </summary>
        /// <param name="e">0ならドット1、それ以外ならドット2</param>
        /// <returns></returns>
        internal Vec this[int e]
        {
            get
            {
                return (e == 0 ? this.dot1 : this.dot2);
            }
        }
        #endregion
        #region コンストラクタ
        /// <summary>
        /// </summary>
        /// <param name="dot1">点1</param>
        /// <param name="dot2">点2</param>
        internal Line(Vec dot1, Vec dot2)
        {
            this.dot1 = dot1;
            this.dot2 = dot2;
        }
        #endregion
    }
}
