using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using MachShooting.Graphic;
using NLua;

namespace MachShooting
{
    /// <summary>
    /// 攻撃オブジェクトです
    /// </summary>
    public abstract class AttackObject : GameObject
    {
        #region フィールド
        /// <summary>
        /// 生きているか
        /// </summary>
        private bool life;

        /// <summary>
        /// 当たったときのコールバック
        /// </summary>
        private LuaFunction call;

        /// <summary>
        /// エフェクトが始まって何Fか？
        /// </summary>
        private int effect = 0;
        #endregion
        #region プロパティ
        /// <summary>
        /// 生きているか
        /// </summary>
        public bool Life
        {
            get { return this.life; }
            protected set { this.life = value; }
        }
        #endregion
        #region コンストラクタ
        /// <summary>
        /// 新しいインスタンスを作成します
        /// </summary>
        /// <param name="circle">円</param>
        /// <param name="power">攻撃力。0なら判定を持たない</param>
        /// <param name="image">画像</param>
        /// <param name="call">ヒット時のコールバック</param>
        protected AttackObject(Vec dot, int power, Image image, double rad, LuaFunction call) : base(dot, power, image, rad)
        {
            this.life = true;
            this.call = call;
            this.Draw = false;
        }
        #endregion
        #region メソッド
        /// <summary>
        /// 攻撃が当たったら当たり判定を消します
        /// </summary>
        /// <param name="damage">ダメージ</param>
        public override void Attack(int damage)
        {
            if (this.life && damage != 0)
            {
                SE.Instance.Play(DXAudio.Instance.ShotHit);
                this.Power = 0;
                this.life = false;
                if (this.call != null)
                {
                    call.Call(damage);
                }
            }
        }

        /// <summary>
        /// 処理を行います
        /// </summary>
        /// <returns>新たに生み出した攻撃オブジェクト。生み出していないならnull</returns>
        public List<AttackObject> Process()
        {
            Next();
            if (this.Life)
            {
                List<AttackObject> l = ProcessAttackObject();
                Input();
                return l;
            }
            else if (this.effect < 30)
            {
                this.Draw = false;
                this.effect++;
            }
            else
            {
                this.Need = false;
            }
            Input();
            return null;

        }

        protected override void DrawGameObjectAfter()
        {
            if (this.life)
            {
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA, 150);
                DrawGraph(this.Image);
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 0);
            }
            else
            {
                DX.SetDrawBright(255, 0, 0); DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA, 255 - this.effect * (255 / 30)); int size = this.effect * (int)this.R; DX.DrawExtendGraph((int)(this.X - size), (int)(this.Y - size), (int)(this.X + size), (int)(this.Y + size), DXImage.Instance.Hit, DX.TRUE); DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 0); DX.SetDrawBright(255, 255, 255);
            }
        }
        #endregion
        #region 実装メソッド
        #endregion
        #region 未実装メソッド
        /// <summary>
        /// 処理を行います
        /// </summary>
        /// <returns>新たに生み出した攻撃オブジェクト。生み出していないならnull</returns>
        protected abstract List<AttackObject> ProcessAttackObject();
        #endregion
    }
}
