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
        public uint red
        {
            get;
            private set;
        }

        /// <summary>
        /// 緑
        /// </summary>
        public uint green
        {
            get;
            private set;
        }

        /// <summary>
        /// 青
        /// </summary>
        public uint blue
        {
            get;
            private set;
        }

        /// <summary>
        /// 白
        /// </summary>
        public uint white
        {
            get;
            private set;
        }

        /// <summary>
        /// 黒
        /// </summary>
        public uint black
        {
            get;
            private set;
        }

        /// <summary>
        /// 黄
        /// </summary>
        public uint yellow
        {
            get;
            private set;
        }

        /// <summary>
        /// シアン
        /// </summary>
        public uint cyan
        {
            get;
            private set;
        }

        /// <summary>
        /// マゼンタ
        /// </summary>
        public uint magenta
        {
            get;
            private set;
        }
        #endregion

        private DXColor()
        {
            this.red = DX.GetColor(255, 0, 0);
            this.green = DX.GetColor(0, 255, 0);
            this.blue = DX.GetColor(0, 0, 255);
            this.white = DX.GetColor(255, 255, 255);
            this.black = DX.GetColor(0, 0, 0);
            this.yellow = DX.GetColor(255, 255, 0);
            this.cyan = DX.GetColor(0, 255, 255);
            this.magenta = DX.GetColor(255, 0, 255);
        }
    }

    
}
