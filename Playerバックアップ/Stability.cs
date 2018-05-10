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
    /// 通常型
    /// </summary>
    public class Stability : Player
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
        /// カウンター攻撃
        /// </summary>
        private int counterAttack;
        #endregion
        #region プロパティ
        #endregion
        #region コンストラクタ
        public Stability()
            : base("通常型", Gauge.SMALL, Gauge.MEDIUM)
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
                attack = new List<AttackObject>();
                this.conventionalAttack = 0;

                attack.Add(NewBullet(this.BulletDotC, this.Strengthen == 0 ? 12 : 14, new Vec(0, -10), DXImage.Instance.BulletSmall, System.Drawing.Color.Red));
            }
            else
            {
                if (this.conventionalAttack >= 10)
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
                attack = new List<AttackObject>();
                this.specialAttack = 0;

                attack.Add(NewBullet(this.BulletDotC, this.Strengthen == 0 ? 27 : 32, new Vec(0, -10), DXImage.Instance.BulletMedium, System.Drawing.Color.Red));
            }
            else
            {
                this.Speed = 1;
                if (this.specialAttack >= 20)
                {
                    this.Action = PlayerAction.NONE;
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
                if (this.deathblow < 60)//始まる前
                {
                    this.Speed = 1;
                }
                else if (this.deathblow == 60)//撃つ
                {
                    attack = new List<AttackObject>();
                    attack.Add(NewBullet(this.BulletDotC, this.Strengthen == 0 ? 145 : 174, new Vec(0, -10), DXImage.Instance.BulletBig, System.Drawing.Color.Red));
                    this.Action = PlayerAction.NONE;
                }
            }
            this.deathblow++;
            return attack;
        }

        /// <summary>
        /// 自己強化系必殺技
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
            }
            else
            {
                if (this.counterAttack % 2 == 0)
                {
                    attack = new List<AttackObject>();
                    attack.Add(NewBullet(this.BulletDotC, this.Strengthen == 0 ? 4 : 5, new Vec(0, -10), DXImage.Instance.BulletSmall, System.Drawing.Color.Red));
                }

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
