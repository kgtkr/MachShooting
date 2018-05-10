using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachShooting
{
    /// <summary>
    /// 画像ハンドルと半径を持つ
    /// </summary>
    public struct Image
    {
        /// <summary>
        /// 画像
        /// </summary>
        public int image { get; private set; }

        /// <summary>
        /// 半径
        /// </summary>
        public double r { get; private set; }

        /// <summary>
        /// 画像の向いている方向
        /// </summary>
        public double rad { get; private set; }

        public Image(int image,double r,double rad)
        {
            this.image = image;
            this.r = r;
            this.rad = rad;
        }
    }
}
