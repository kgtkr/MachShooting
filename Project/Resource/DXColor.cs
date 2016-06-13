using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;

namespace MachShooting
{
    public class DXColor
    {
        private static DXColor instance;

        public static DXColor Instance
        {
            get
            {
                if (DXColor.instance == null)
                {
                    DXColor.instance = new DXColor();
                }

                return DXColor.instance;
            }
        }

        #region 色一覧
        /// <summary>
        /// 赤
        /// </summary>
        public uint Red
        {
            get;
            private set;
        }

        /// <summary>
        /// 緑
        /// </summary>
        public uint Green
        {
            get;
            private set;
        }

        /// <summary>
        /// 青
        /// </summary>
        public uint Blue
        {
            get;
            private set;
        }

        /// <summary>
        /// 白
        /// </summary>
        public uint White
        {
            get;
            private set;
        }

        /// <summary>
        /// 黒
        /// </summary>
        public uint Black
        {
            get;
            private set;
        }

        /// <summary>
        /// 黄
        /// </summary>
        public uint Yellow
        {
            get;
            private set;
        }

        /// <summary>
        /// シアン
        /// </summary>
        public uint Cyan
        {
            get;
            private set;
        }

        /// <summary>
        /// マゼンタ
        /// </summary>
        public uint Magenta
        {
            get;
            private set;
        }
        #endregion

        private DXColor()
        {
            this.Red = DX.GetColor(255, 0, 0);
            this.Green = DX.GetColor(0, 255, 0);
            this.Blue = DX.GetColor(0, 0, 255);
            this.White = DX.GetColor(255, 255, 255);
            this.Black = DX.GetColor(0, 0, 0);
            this.Yellow = DX.GetColor(255, 255, 0);
            this.Cyan = DX.GetColor(0, 255, 255);
            this.Magenta = DX.GetColor(255, 0, 255);
        }
    }

    
}
