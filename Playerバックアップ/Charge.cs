﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using MachShooting.Graphic;

namespace MachShooting
{
    /// <summary>
    /// 溜め型
    /// </summary>
    public class Charge : Player
    {
        #region 定数
        #endregion
        #region フィールド
        /// <summary>
        /// 通常攻撃
        /// </summary>
        private int conventionalAttack;

        /// <summary>
        /// 特殊攻撃
        /// </summary>
        private int specialAttack;

        /// <summary>
        /// 攻撃系必殺技
        /// </summary>
        private int deathblow;

        /// <summary>
        /// カウンター
        /// </summary>
        private int counterAttack;
        #endregion
        #region プロパティ
        #endregion
        #region コンストラクタ
        /// <summary>
        /// 新しいインスタンスを作成します
        /// </summary>
        public Charge()
            : base("溜め型", Gauge.LARGE, Gauge.MEDIUM)
        {
        }
        #endregion
        #region メソッド
        protected override void DrawGameObjectAfter()
        {
            base.DrawGameObjectAfter();
            if (this.Action == PlayerAction.SPECIAL)
            {
                if (this.specialAttack < (this.Strengthen == 0 ? 60 : 30))//30～59|15～29
                {
                    DX.DrawCircle((int)this.X, (int)this.Y, 5, DXColor.Instance.Red, DX.FALSE);
                }
                else if (this.specialAttack < (this.Strengthen == 0 ? 120 : 60))//60～119|30～59
                {
                    DX.DrawCircle((int)this.X, (int)this.Y, 10, DXColor.Instance.Red, DX.FALSE);
                }
                else if (this.specialAttack < (this.Strengthen == 0 ? 180 : 89))//120～179|60～90
                {
                    DX.DrawCircle((int)this.X, (int)this.Y, 15, DXColor.Instance.Red, DX.FALSE);
                }
                else//180～|90～
                {
                    DX.DrawCircle((int)this.X, (int)this.Y, 20, DXColor.Instance.Red, DX.FALSE);
                }
            }
        }
        #endregion
        #region 実装メソッド
        /// <summary>
        /// 通常攻撃
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        protected override List<AttackObject> ConventionalAttack(byte[] key, bool start)
        {
            List<AttackObject> attack = null;
            if (start)
            {
                attack = new List<AttackObject>();
                this.conventionalAttack = 0;

                attack.Add(NewBullet(this.BulletDotC, 16, new Vec(0, -10), DXImage.Instance.BulletMedium, System.Drawing.Color.Red));
            }
            else
            {
                if (this.conventionalAttack >= 30)
                {
                    this.Action = PlayerAction.NONE;
                }
            }
            this.conventionalAttack++;
            return attack;
        }

        /// <summary>
        /// 特殊攻撃
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        protected override List<AttackObject> SpecialAttack(byte[] key, bool start)
        {
            List<AttackObject> attack = null;
            if (start)
            {
                this.specialAttack = 0;
            }
            else
            {
                if (key[Config.Instance.Key[KeyComfig.GAME_SPECIAL]] == DX.FALSE && this.specialAttack >= (this.Strengthen == 0 ? 30 : 15))
                {
                    attack = new List<AttackObject>();
                    if (this.specialAttack < (this.Strengthen == 0 ? 60 : 30))//30～59|15～29
                    {
                        attack.Add(NewBullet(this.BulletDotC, 30, new Vec(0, -10), DXImage.Instance.BulletSmall, System.Drawing.Color.Red));
                    }
                    else if (this.specialAttack < (this.Strengthen == 0 ? 120 : 60))//60～119|30～59
                    {
                        attack.Add(NewBullet(this.BulletDotC, 66, new Vec(0, -10), DXImage.Instance.BulletSmall, System.Drawing.Color.Red));
                    }
                    else if (this.specialAttack < (this.Strengthen == 0 ? 180 : 89))//120～179|60～90
                    {
                        attack.Add(NewBullet(this.BulletDotC, 144, new Vec(0, -10), DXImage.Instance.BulletMedium, System.Drawing.Color.Red));
                    }
                    else//180～|90～
                    {
                        attack.Add(NewBullet(this.BulletDotC, 234, new Vec(0, -10), DXImage.Instance.BulletBig, System.Drawing.Color.Red));
                    }
                    this.Action = PlayerAction.NONE;
                }
                this.specialAttack++;
            }
            return attack;
        }

        /// <summary>
        /// 攻撃系必殺技
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        protected override List<AttackObject> Deathblow(byte[] key, bool start)
        {
            List<AttackObject> attack = null;
            if (start)
            {
                this.deathblow = 0;
            }
            else
            {
                if (this.deathblow == (this.Strengthen == 0 ? 180 : 90))//撃つ
                {
                    attack = new List<AttackObject>();
                    attack.Add(NewBullet(this.BulletDotC, 468, new Vec(0, -10), DXImage.Instance.Bomb, System.Drawing.Color.White));
                    this.Action = PlayerAction.NONE;
                }
            }
            this.deathblow++;
            return attack;
        }

        /// <summary>
        /// 自己強化
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        protected override void Strengthen_(byte[] key, bool start)
        {
            if (start)
            {
                this.Strengthen = 450;
            }
            else
            {
                this.Strengthen--;
            }
        }

        /// <summary>
        /// カウンター攻撃
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        protected override List<AttackObject> CounterAttack(byte[] key, bool start)
        {
            List<AttackObject> attack = null;
            if (start)
            {
                attack = new List<AttackObject>();
                this.counterAttack = 0;
                attack.Add(NewBullet(this.BulletDotC, 60, new Vec(0, -10), DXImage.Instance.BulletBig, System.Drawing.Color.Red));
            }
            else
            {
                if (this.counterAttack >= 30)
                {
                    this.Action = PlayerAction.NONE;
                }
            }
            this.counterAttack++;
            return attack;
        }
        #endregion
        #region 未実装メソッド
        #endregion
    }
}