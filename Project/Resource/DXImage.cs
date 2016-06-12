using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using MachShooting.Graphic;
using System.Collections.ObjectModel;

namespace MachShooting
{
    public class DXImage
    {
        private static DXImage instance;

        public static DXImage Instance
        {
            get
            {
                if (DXImage.instance == null)
                {
                    DXImage.instance = new DXImage();
                }

                return DXImage.instance;
            }
        }

        #region 画像
        /// <summary>
        /// 背景画像
        /// </summary>
        public int back
        {
            get;
            private set;
        }

        /// <summary>
        /// 自機
        /// </summary>
        public Image my
        {
            get;
            private set;
        }

        /// <summary>
        /// 時計
        /// </summary>
        public int clock
        {
            get;
            private set;
        }
        #region 弾
        /// <summary>
        /// 超巨大弾
        /// </summary>
        public Image bomb
        {
            get;
            private set;
        }

        /// <summary>
        /// 小弾
        /// </summary>
        public Image bulletSmall
        {
            get;
            private set;
        }

        /// <summary>
        /// 中弾
        /// </summary>
        public Image bulletMedium
        {
            get;
            private set;
        }

        /// <summary>
        /// 大弾
        /// </summary>
        public Image bulletBig
        {
            get;
            private set;
        }
        #endregion
        #region エフェクト
        /// <summary>
        /// ヒット
        /// </summary>
        public int hit
        {
            get;
            private set;
        }

        /// <summary>
        /// チャージ
        /// </summary>
        public int charge
        {
            get;
            private set;
        }

        /// <summary>
        /// 特殊効果
        /// </summary>
        public int special
        {
            get;
            private set;
        }
        #endregion
        #endregion

        private DXImage()
        {
            /*=画像=*/
            this.back = DX.LoadGraph("Data/Image/back.png");

            this.my = new Image(DX.LoadGraph("Data/Image/My/1.png"), 15, new Vec(0, -1).Rad);

            this.clock = DX.LoadGraph("Data/Image/Clock.png");

            this.bomb = new Image(DX.LoadGraph("Data/Image/Bullet/bomb.png"), 140, Math.PI);

            this.bulletSmall=new Image(DX.LoadGraph("Data/Image/Bullet/small.png"), 4, new Vec(0, -1).Rad);
            this.bulletMedium=new Image(DX.LoadGraph("Data/Image/Bullet/medium.png"), 8, new Vec(0, -1).Rad);
            this.bulletBig=new Image(DX.LoadGraph("Data/Image/Bullet/big.png"), 14, new Vec(0, -1).Rad);


            //エフェクト
            this.hit = DX.LoadGraph("Data/Image/Effect/Hit.png");
            this.special = DX.LoadGraph("Data/Image/Effect/Special.png");
            this.charge = DX.LoadGraph("Data/Image/Effect/Charge.png");
        }
    }
}
