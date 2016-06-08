﻿using System;
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
        public IReadOnlyList<Image> bulletSmall
        {
            get;
            private set;
        }

        /// <summary>
        /// 中弾
        /// </summary>
        public IReadOnlyList<Image> bulletMedium
        {
            get;
            private set;
        }

        /// <summary>
        /// 大弾
        /// </summary>
        public IReadOnlyList<Image> bulletBig
        {
            get;
            private set;
        }
        #endregion
        #region 敵
        /// <summary>
        /// ボア
        /// </summary>
        public Image boar
        {
            get;
            private set;
        }

        /// <summary>
        /// ガトー
        /// </summary>
        public Image gato
        {
            get;
            private set;
        }

        /// <summary>
        /// ラパン
        /// </summary>
        public Image lapin
        {
            get;
            private set;
        }

        /// <summary>
        /// ネガリャー　
        /// </summary>
        public Image nigalya
        {
            get;
            private set;
        }

        /// <summary>
        /// スネーク
        /// </summary>
        public Image snake
        {
            get;
            private set;
        }

        /// <summary>
        /// レオーネ
        /// </summary>
        public Image leone
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

            var big = new List<Image>(10);
            var medium = new List<Image>(10);
            var small = new List<Image>(10);
            for (int i = 0; i < 10; i++) small.Add(new Image(DX.LoadGraph("Data/Image/Bullet/Small/" + (i + 1) + ".png"), 4, new Vec(0, -1).Rad));
            for (int i = 0; i < 10; i++) medium.Add(new Image(DX.LoadGraph("Data/Image/Bullet/Medium/" + (i + 1) + ".png"), 8, new Vec(0, -1).Rad));
            for (int i = 0; i < 10; i++) big.Add(new Image(DX.LoadGraph("Data/Image/Bullet/Big/" + (i + 1) + ".png"), 14, new Vec(0, -1).Rad));
            this.bulletSmall = small.AsReadOnly();
            this.bulletMedium = medium.AsReadOnly();
            this.bulletBig = big.AsReadOnly();


            /*=敵=*/
            this.boar = new Image(DX.LoadGraph("Data/Image/Enemy/boar.png"), 15, new Vec(0, 1).Rad);
            this.gato = new Image(DX.LoadGraph("Data/Image/Enemy/gato.png"), 15, new Vec(0, 1).Rad);
            this.lapin = new Image(DX.LoadGraph("Data/Image/Enemy/lapin.png"), 15, new Vec(0, 1).Rad);
            this.nigalya = new Image(DX.LoadGraph("Data/Image/Enemy/nigalya.png"), 20, new Vec(0, 1).Rad);
            this.snake = new Image(DX.LoadGraph("Data/Image/Enemy/snake.png"), 15, new Vec(0, 1).Rad);
            this.leone = new Image(DX.LoadGraph("Data/Image/Enemy/leone.png"), 50, new Vec(0, 1).Rad);


            //エフェクト
            this.hit = DX.LoadGraph("Data/Image/Effect/Hit.png");
            this.special = DX.LoadGraph("Data/Image/Effect/Special.png");
            this.charge = DX.LoadGraph("Data/Image/Effect/Charge.png");


        }
    }
}