using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using CircleLib;

namespace MachShooting
{
    /// <summary>
    /// パンダ
    /// ネガリャー(ネパール語)
    /// </summary>
    public class Nigalya:Enemy
    {
        #region 定数
        #endregion
        #region フィールド
        #endregion
        #region プロパティ
        #endregion
        #region コンストラクタ
        public Nigalya(My my):base("ネガリャー", 0, 2000, my, Program.nigalya)
        {

        }
        #endregion
        #region メソッド

        protected override List<AttackObject> Process_Enemy()
        {
            if (!this.SyncTransfer.Need)
            {
                this.SyncTransfer.Add(new ChargeEffect(this, 300, (int)this.R * 3, System.Drawing.Color.Green));
                this.SyncTransfer.Add(new SetProperty(() => this.Power = 40));
                this.SyncTransfer.Add(new ULMTarget(this, this.My, 10, 30));
                this.SyncTransfer.Add(new SetProperty(() => this.Power = 0));
                AsyncTransfer at = new AsyncTransfer();
                for (int i = 0; i < 36; i++)
                {
                    at.Add(new AttackObjectTransfer(()=> new Bullet(this.Circle.Dot, 50, Vec.NewRadLength((i * 10.0).ToRad(), 10), Program.bulletBig[0], null)));
                }
                this.SyncTransfer.Add(at);
            }
            return null;
        }

        protected override void DrawGameObjectAfter()
        {
            base.DrawGameObjectAfter();
        }
        #endregion
        #region 実装メソッド
        #endregion
        #region 未実装メソッド
        #endregion
    }
}
