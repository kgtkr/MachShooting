using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;

namespace MachShooting
{
    internal class Font
    {
        private static Font instance;

        internal static Font Instance
        {
            get
            {
                if (Font.instance == null)
                {
                    Font.instance = new Font();
                }

                return Font.instance;
            }
        }

        #region フォント
        /// <summary>
        /// 8px
        /// </summary>
        internal int Font8
        {
            get;
            private set;
        }

        /// <summary>
        /// 16px
        /// </summary>
        internal int Font16
        {
            get;
            private set;
        }

        /// <summary>
        /// 32px
        /// </summary>
        internal int Font32
        {
            get;
            private set;
        }

        /// <summary>
        /// 64px
        /// </summary>
        internal int Font64
        {
            get;
            private set;
        }
        #endregion

        private Font()
        {
            this.Font8 = DX.LoadFontDataToHandle("Data/Font/8.dft");
            this.Font16 = DX.LoadFontDataToHandle("Data/Font/16.dft");
            this.Font32 = DX.LoadFontDataToHandle("Data/Font/32.dft");
            this.Font64 = DX.LoadFontDataToHandle("Data/Font/64.dft");
        }
    }
}
