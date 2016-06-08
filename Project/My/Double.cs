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
    /// 双連射型
    /// </summary>
    public class Double : My
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
        /// <summary>
        /// 新しいイントランスを作成します
        /// </summary>
        public Double() : base("双連射型", Gauge.MEDIUM, Gauge.SUPER_SMALL)
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

                attack.Add(NewBullet(this.BulletDotR, this.Strengthen == 0 ? 2 : 3, new Vec(0, -10), DXImage.Instance.bulletSmall[0], this.AttackObjectMeta));
                attack.Add(NewBullet(this.BulletDotL, this.Strengthen == 0 ? 2 : 3, new Vec(0, -10), DXImage.Instance.bulletSmall[0], this.AttackObjectMeta));
            }
            else
            {
                if (this.conventionalAttack >= 4)
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
            }
            else
            {
                if (this.counterAttack % 4 == 0)
                {
                    attack = new List<AttackObject>();
                    attack.Add(NewBullet(this.BulletDotR, this.Strengthen == 0 ? 4 : 5, new Vec(0, -10), DXImage.Instance.bulletSmall[0], this.AttackObjectMeta));
                    attack.Add(NewBullet(this.BulletDotL, this.Strengthen == 0 ? 4 : 5, new Vec(0, -10), DXImage.Instance.bulletSmall[0], this.AttackObjectMeta));
                }
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
                this.Speed = 1;
                if (this.deathblow % 2 == 0)
                {
                    attack = new List<AttackObject>();
                    attack.Add(NewBullet(this.BulletDotR, this.Strengthen == 0 ? 2 : 3, new Vec(0, -10), DXImage.Instance.bulletSmall[0], this.AttackObjectMeta));
                    attack.Add(NewBullet(this.BulletDotL, this.Strengthen == 0 ? 2 : 3, new Vec(0, -10), DXImage.Instance.bulletSmall[0], this.AttackObjectMeta));
                }
                if (this.deathblow >= 120)
                {
                    this.Action = MyAction.NONE;
                }
            }
            this.deathblow++;
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
                attack.Add(NewBullet(this.BulletDotR, this.Strengthen == 0 ? 3 : 4, new Vec(0, -10), DXImage.Instance.bulletSmall[0], this.AttackObjectMeta));
                attack.Add(NewBullet(this.BulletDotL, this.Strengthen == 0 ? 3 : 4, new Vec(0, -10), DXImage.Instance.bulletSmall[0], this.AttackObjectMeta));
            }
            else
            {
                this.Speed = 1;
                if (this.specialAttack >= 4)
                {
                    this.Action = MyAction.NONE;
                }
            }
            this.specialAttack++;
            return attack;
        }

        protected override void Strengthen_(byte[] key, bool start)
        {
            if (start)
            {
                this.Strengthen = 450;
            }
            else
            {
                if (this.Strengthen % 20 == 0 && this.Hp != 0)
                {
                    this.Hp--;
                    if (this.Hp == 0)
                    {
                        this.Hp = 1;
                        this.Strengthen = 0;
                        return;
                    }
                }
                this.Strengthen--;
            }
        }
        #endregion
        #region 未実装メソッド
        #endregion





    }
}
