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
    /// イノシシ
    /// ボア(英語)
    /// </summary>
    public class Boar:Enemy
    {
        /// <summary>
        /// フラグ。0:待ち、1:移動
        /// </summary>
        private int flag = 0;

        /// <summary>
        /// 現在の行動が始まってからの時間
        /// </summary>
        private int count;

        public Boar(My my) : base("ボア", 0, 1500, my, Program.boar)
        {

        }

        protected override List<AttackObject> Process_Enemy()
        {
            if (this.flag == 0)
            {
                if (this.count == 300)
                {
                    this.Rad = new Vec(this.My.X - this.X, this.My.Y - this.Y).Rad;

                    this.Power = 20;

                    this.flag = 1;
                    this.count = 0;
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
                }
            }
            this.count++;
            return null;
        }

        protected override void DrawGameObjectAfter()
        {
            base.DrawGameObjectAfter();
            if (this.flag == 0)
            {
                DX.DrawCircle((int)this.X, (int)this.Y, (300 - this.count)/5, Program.blue,DX.FALSE);
            }
        }
    }
}
