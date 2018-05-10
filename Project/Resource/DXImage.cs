using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
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
        internal int Back
        {
            get;
            private set;
        }

        /// <summary>
        /// 自機
        /// </summary>
        internal Image Player
        {
            get;
            private set;
        }

        /// <summary>
        /// 時計
        /// </summary>
        internal int Clock
        {
            get;
            private set;
        }
        #region 弾
        /// <summary>
        /// 超巨大弾
        /// </summary>
        public Image Bomb
        {
            get;
            private set;
        }

        /// <summary>
        /// 小弾
        /// </summary>
        public Image BulletSmall
        {
            get;
            private set;
        }

        /// <summary>
        /// 中弾
        /// </summary>
        public Image BulletMedium
        {
            get;
            private set;
        }

        /// <summary>
        /// 大弾
        /// </summary>
        public Image BulletBig
        {
            get;
            private set;
        }
        #endregion
        #region エフェクト
        /// <summary>
        /// ヒット
        /// </summary>
        public int Hit
        {
            get;
            private set;
        }

        /// <summary>
        /// チャージ
        /// </summary>
        public int Charge
        {
            get;
            private set;
        }

        /// <summary>
        /// 特殊効果
        /// </summary>
        public int Special
        {
            get;
            private set;
        }
        #endregion
        #endregion

        private DXImage()
        {
            /*=画像=*/
            this.Back = DX.LoadGraph("Data/Image/back.png");

            this.Player = new Image(DX.LoadGraph("Data/Image/Player/center.png"), 15, new Vec(0, -1).Rad);

            this.Clock = DX.LoadGraph("Data/Image/Clock.png");

            this.Bomb = new Image(DX.LoadGraph("Data/Image/Bullet/bomb.png"), 140, Math.PI);

            this.BulletSmall=new Image(DX.LoadGraph("Data/Image/Bullet/small.png"), 4, new Vec(0, -1).Rad);
            this.BulletMedium=new Image(DX.LoadGraph("Data/Image/Bullet/medium.png"), 8, new Vec(0, -1).Rad);
            this.BulletBig=new Image(DX.LoadGraph("Data/Image/Bullet/big.png"), 14, new Vec(0, -1).Rad);


            //エフェクト
            this.Hit = DX.LoadGraph("Data/Image/Effect/Hit.png");
            this.Special = DX.LoadGraph("Data/Image/Effect/Special.png");
            this.Charge = DX.LoadGraph("Data/Image/Effect/Charge.png");
        }
    }
}
