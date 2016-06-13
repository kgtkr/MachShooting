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
    /// 拡散型
    /// </summary>
    public class Overall : My
    {
        #region 定数
        #endregion
        #region フィールド
        /// <summary>
        /// 通常攻撃
        /// </summary>
        private int conventionalAttack;

        /// <summary>
        /// カウンター攻撃
        /// </summary>
        private int counterAttack;

        /// <summary>
        /// 特殊攻撃
        /// </summary>
        private int specialAttack;

        /// <summary>
        /// 必殺技
        /// </summary>
        private int deathblow;
        #endregion
        #region プロパティ
        #endregion
        #region コンストラクタ
        /// <summary>
        /// 新しいイントランスを作成します
        /// </summary>
        public Overall() : base("拡散型",Gauge.SUPER_LARGE, Gauge.MEDIUM)
        {

        }
        #endregion
        #region メソッド
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
                this.conventionalAttack = 0;
                attack = new List<AttackObject>();
                for (int i = 0; i < 11; i++)
                {
                    int d = this.Strengthen == 0 ? 2 : 3;
                    Image img = DXImage.Instance.bulletSmall;

                    switch (i)
                    {
                        case 5:
                            d *= 6;
                            img= DXImage.Instance.bulletBig;
                            break;
                        case 4:
                        case 6:
                            d *= 2;
                            img = DXImage.Instance.bulletMedium;
                            break;
                    }

                    attack.Add(NewBullet(this.BulletDotC, d,Vec.NewRadLength((225.0 + i * 9).ToRad(), 10), img, System.Drawing.Color.Red));
                }
            }
            else
            {
                if (this.conventionalAttack >= 30)
                {
                    this.Action = MyAction.NONE;
                }
            }
            this.conventionalAttack++;
            return attack;
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
                this.counterAttack = 0;
                attack = new List<AttackObject>();
                for (int i = 0; i < 11; i++)
                {
                    int d = this.Strengthen == 0 ? 2 : 3;
                    Image img = DXImage.Instance.bulletSmall;

                    switch (i)
                    {
                        case 5:
                            d *= 6;
                            img = DXImage.Instance.bulletBig;
                            break;
                        case 4:
                        case 6:
                            d *= 2;
                            img = DXImage.Instance.bulletMedium;
                            break;
                    }

                    attack.Add(NewBullet(this.BulletDotC, d,Vec.NewRadLength((225.0 + i * 9).ToRad(),10), img, System.Drawing.Color.Red));
                }
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

        /// <summary>
        /// 必殺技
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
                if (this.deathblow % 3 == 0)
                {
                    attack = new List<AttackObject>();
                    for (int i = 0; i < 36; i++)
                    {
                        attack.Add(NewBullet(this.BulletDotC, this.Strengthen == 0 ? 1 : 2,Vec.NewRadLength((i * 10.0 + this.deathblow % 360).ToRad(),15), DXImage.Instance.bulletSmall, System.Drawing.Color.Red));
                    }
                }
            }
            this.deathblow++;
            if (this.deathblow == 300)
            {
                this.Action = MyAction.NONE;
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
                this.specialAttack = 0;

                attack = new List<AttackObject>();
                for (int i = 0; i < 27; i++)
                {
                    attack.Add(NewBullet(this.BulletDotC, this.Strengthen == 0 ? 2 : 3, Vec.NewRadLength((i*15.0).ToRad(),10), DXImage.Instance.bulletSmall, System.Drawing.Color.Red));
                }
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
        /// 自己強化
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        protected override void Strengthen_(byte[] key, bool start)
        {
            if (start)
            {
                this.Strengthen = 900;
            }
            else
            {
                this.Strengthen--;
            }
        }
        #endregion
        #region 未実装メソッド
        #endregion
    }
}
