using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachShooting
{
    /// <summary>
    /// ラジアン
    /// </summary>
    public static class Radian
    {
        #region 8方位
        /// <summary>
        /// 上
        /// </summary>
        public const double UP = -Math.PI / 2;

        /// <summary>
        /// 下
        /// </summary>
        public const double DOWN = Math.PI / 2;

        /// <summary>
        /// 左
        /// </summary>
        public const double LEFT = Math.PI;

        /// <summary>
        /// 右
        /// </summary>
        public const double RIGHT = 0;

        /// <summary>
        /// 右上
        /// </summary>
        public const double RIGHT_UP = -Math.PI / 4;

        /// <summary>
        /// 右下
        /// </summary>
        public const double RIGHT_DOWN = Math.PI / 4;

        /// <summary>
        /// 左上
        /// </summary>
        public const double LEFT_UP = -Math.PI * 0.75;

        /// <summary>
        /// 左下
        /// </summary>
        public const double LEFT_DOWN = Math.PI * 0.75;
        #endregion
        #region 角度・ラジアン変換
        /// <summary>
        /// 角度をラジアンに変換します
        /// </summary>
        /// <param name="angle">角度</param>
        /// <returns>ラジアン</returns>
        public static double ToRad(this double angle)
        {
            return angle * Math.PI / 180;
        }

        /// <summary>
        /// ラジアンを角度に変換します
        /// </summary>
        /// <param name="rad">ラジアン</param>
        /// <returns>角度</returns>
        public static double ToAngle(this double rad)
        {
            return rad * 180 / Math.PI;
        }
        #endregion
        
    }
}