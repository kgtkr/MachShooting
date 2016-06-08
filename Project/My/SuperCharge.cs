using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using MachShooting.Graphic;

namespace MachShooting
{
    /// <summary>
    /// 超溜め型
    /// </summary>
    public class SuperCharge : My
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

        /// <summary>
        /// 溜めレベル
        /// </summary>
        private int lv;

        /// <summary>
        /// 追加入力開始からの時間
        /// </summary>
        private int add;
        #endregion
        #region プロパティ
        #endregion
        #region コンストラクタ
        /// <summary>
        /// 新しいイントランスを作成します
        /// </summary>
        public SuperCharge()
            : base("超溜め型", Gauge.SUPER_LARGE, Gauge.MEDIUM)
        {
        }
        #endregion
        #region メソッド
        /// <summary>
        /// 処理を行います
        /// </summary>
        /// <param name="key"></param>
        /// <param name="key2"></param>
        protected override List<AttackObject> Process_My(byte[] key, byte[] key2)
        {
            if (!(this.Action == MyAction.ATTACK || this.Action == MyAction.SPECIAL))
            {
                this.lv = 0;
            }
            return null;
        }


        protected override void DrawGameObjectAfter()
        {
            base.DrawGameObjectAfter();
            if (this.Action == MyAction.ATTACK)
            {
                if (this.conventionalAttack < (this.Strengthen == 0 ? 60 : 30))//30～59|15～29
                {
                    DX.DrawCircle((int)this.X, (int)this.Y, 5, DXColor.Instance.red, DX.FALSE);
                }
                else if (this.conventionalAttack < (this.Strengthen == 0 ? 120 : 60))//60～119|30～59
                {
                    DX.DrawCircle((int)this.X, (int)this.Y, 10, DXColor.Instance.red, DX.FALSE);
                }
                else if (this.conventionalAttack < (this.Strengthen == 0 ? 180 : 89))//120～179|60～90
                {
                    DX.DrawCircle((int)this.X, (int)this.Y, 15, DXColor.Instance.red, DX.FALSE);
                }
                else//180～|90～
                {
                    DX.DrawCircle((int)this.X, (int)this.Y, 20, DXColor.Instance.red, DX.FALSE);
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
            List < AttackObject > attack= null;
            if (start)
            {
                this.conventionalAttack = 0;
                this.lv = 0;
                this.add = 0;
            }
            else
            {
                if (this.lv == 0)
                {
                    attack = new List<AttackObject>();
                    this.Speed = 1;
                    if (key[Config.Instance.key[KeyComfig.GAME_ATTACK]] == DX.FALSE && this.conventionalAttack >= (this.Strengthen == 0 ? 30 : 15))
                    {
                        if (this.conventionalAttack < (this.Strengthen == 0 ? 60 : 30))//30～59|15～29
                        {
                            attack.Add(NewBullet(this.BulletDotC, 33, new Vec(0,-10), DXImage.Instance.bulletSmall, System.Drawing.Color.Red, this.AttackObjectMeta));
                            this.lv = 1;
                        }
                        else if (this.conventionalAttack < (this.Strengthen == 0 ? 120 : 60))//60～119|30～59
                        {
                            attack.Add(NewBullet(this.BulletDotC, 72, new Vec(0, -10), DXImage.Instance.bulletSmall, System.Drawing.Color.Red, this.AttackObjectMeta));
                            this.lv = 2;
                        }
                        else if (this.conventionalAttack < (this.Strengthen == 0 ? 180 : 89))//120～179|60～90
                        {
                            attack.Add(NewBullet(this.BulletDotC, 158, new Vec(0, -10), DXImage.Instance.bulletMedium, System.Drawing.Color.Red, this.AttackObjectMeta));
                            this.lv = 3;
                        }
                        else//180～|90～
                        {
                            attack.Add(NewBullet(this.BulletDotC, 257, new Vec(0, -10), DXImage.Instance.bulletBig, System.Drawing.Color.Red, this.AttackObjectMeta));
                            this.lv = 4;
                        }
                    }
                    this.conventionalAttack++;
                }
                else
                {
                    this.add++;
                    if (key[Config.Instance.key[KeyComfig.GAME_SPECIAL]] == DX.TRUE)//追加
                    {
                        attack = new List<AttackObject>();
                        attack.AddList(SpecialAttack(key, true));
                        this.Action = MyAction.SPECIAL;
                    }
                    if (this.add == 60)//時間切れ
                    {
                        this.Action = MyAction.NONE;
                    }
                }
            }
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
                attack = new List<AttackObject>();
                this.specialAttack = 0;

                if (this.lv == 1)
                {
                    attack.Add(NewBullet(this.BulletDotC, 30, new Vec(0, -10), DXImage.Instance.bulletSmall, System.Drawing.Color.Red, this.AttackObjectMeta));
                }
                else if (this.lv == 2)
                {
                    attack.Add(NewBullet(this.BulletDotC, 36, new Vec(0, -10), DXImage.Instance.bulletSmall, System.Drawing.Color.Red, this.AttackObjectMeta));
                }
                else if (this.lv == 3)
                {
                    attack.Add(NewBullet(this.BulletDotC, 42, new Vec(0, -10), DXImage.Instance.bulletMedium, System.Drawing.Color.Red, this.AttackObjectMeta));
                }
                else if (this.lv == 4)
                {
                    attack.Add(NewBullet(this.BulletDotC, 48, new Vec(0, -10), DXImage.Instance.bulletBig, System.Drawing.Color.Red, this.AttackObjectMeta));
                }
                else
                {
                    this.Action = MyAction.NONE;
                }
                this.lv = 0;
            }
            else
            {
                if (this.specialAttack >= 30)
                {
                    this.Action = MyAction.NONE;
                }
            }
            this.specialAttack++;
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
                if (this.deathblow < (this.Strengthen == 0 ? 300 : 150))//始まる前
                {
                    this.Speed = 1;
                }
                else if (this.deathblow == (this.Strengthen == 0 ? 300 : 150))//撃つ
                {
                    attack = new List<AttackObject>();
                    attack.Add(NewBullet(this.BulletDotC, 990, new Vec(0, -10), DXImage.Instance.bomb, System.Drawing.Color.White, this.AttackObjectMeta));
                    this.Action = MyAction.NONE;
                    return attack;
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
                attack.Add(NewBullet(this.BulletDotC, 60, new Vec(0, -10), DXImage.Instance.bulletBig, System.Drawing.Color.Red, this.AttackObjectMeta));
            }
            else
            {
                if (this.counterAttack >= 30)
                {
                    this.Action = MyAction.NONE;
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
