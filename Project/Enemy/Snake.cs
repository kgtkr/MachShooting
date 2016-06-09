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
    /// ヘビ
    /// スネーク(英語)
    /// </summary>
    public class Snake : Enemy
    {
        #region 定数
        #endregion
        #region フィールド
        #endregion
        #region プロパティ
        #endregion
        #region コンストラクタ
        public Snake(My my) : base("スネーク", 0, 1500, my, DXImage.Instance.snake)
        {

        }
        #endregion
        #region メソッド

        protected override List<AttackObject> Process_Enemy()
        {
            if (!this.SyncTransfer.Need)
            {
                this.SyncTransfer.Add(new ChargeEffect(this, 500, (int)this.R * 3, System.Drawing.Color.Red));
                this.SyncTransfer.Add(new SetProperty(() => this.Power = 30));
                this.SyncTransfer.Add(new ULMTarget(this, this.My, 10, 30));
                this.SyncTransfer.Add(new SetProperty(() => this.Power = 50));
                AsyncTransfer at = new AsyncTransfer();
                at.Add(new CircleTransfer(this, 0, 0, Math.PI * 2 / 60, 3, 300));
                SyncTransfer st = new SyncTransfer();
                for (int i = 0; i < 60; i++)
                {
                    st.Add(new Wait(4));
                    st.Add(new AttackObjectTransfer(() => new Bullet(this.Circle.Dot, 30, Vec.NewRadLength((this.My.Circle.Dot - this.Circle.Dot).Rad, 10), DXImage.Instance.bulletMedium, System.Drawing.Color.Red)));
                }
                at.Add(st);
                this.SyncTransfer.Add(at);
                this.SyncTransfer.Add(new ULMTarget(this, this.My, 30, 30));
                this.SyncTransfer.Add(new SetProperty(() => this.Power = 0));
            }
            return null;
        }

        protected override void DrawGameObjectAfter()
        {
        }
        #endregion
        #region 実装メソッド
        #endregion
        #region 未実装メソッド
        #endregion
    }
}
