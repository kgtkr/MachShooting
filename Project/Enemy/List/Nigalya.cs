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
        /// <summary>
        /// フラグ。0:待ち、1:移動
        /// </summary>
        private int flag = 0;

        /// <summary>
        /// 現在の行動が始まってからの時間
        /// </summary>
        private int count;
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
            List<AttackObject> list = null;

            if (this.flag == 0)
            {
                if (this.count == 300)
                {
                    this.Rad = new Vec(this.My.X - this.X, this.My.Y - this.Y).Rad;

                    this.Power = 40;

                    this.flag = 1;
                    this.count = 1;
                }
            }
            else if (this.flag == 1)
            {
                Vec v = Vec.NewRadLength(this.Rad, 10);
                this.X += v.X;
                this.Y += v.Y;

                if (this.count == 30)
                {
                    this.Power = 0;
                    this.flag = 0;
                    this.count = 0;

                    list = new List<AttackObject>();
                    for (int i = 0; i < 36; i++)
                    {
                        list.Add(new Bullet(this.Dot, 50,Vec.NewRadLength((i*10.0).ToRad(),10), Program.bulletBig[0], null));
                    }
                }
            }
            this.count++;
            return list;
        }

        protected override void DrawGameObjectAfter()
        {
            base.DrawGameObjectAfter();
            if (this.flag == 0)
            {
                DX.DrawCircle((int)this.X, (int)this.Y, (300 - this.count) / 5, Program.blue, DX.FALSE);
            }
        }
        #endregion
        #region 実装メソッド
        #endregion
        #region 未実装メソッド
        #endregion
    }
}
