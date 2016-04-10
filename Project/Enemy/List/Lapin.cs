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
    /// ウサギ
    /// ラパン(フランス)
    /// </summary>
    public class Lapin : Enemy
    {
        private const int SPEED = 10;

        /// <summary>
        /// 現在の行動
        /// 0:停止、1:動く
        /// </summary>
        private int flag;

        /// <summary>
        /// 現在の行動が始まってからのカウント
        /// </summary>
        private int count;

        /// <summary>
        /// 動くX
        /// </summary>
        private double fX;

        /// <summary>
        /// 動くY
        /// </summary>
        private double fY;

        public Lapin(My my) : base("ラパン", 0, 1500, my, Program.lapin)
        {

        }
        protected override List<AttackObject> Process_Enemy()
        {
            switch (this.flag)
            {
                case 0:
                    if (this.count == 300)
                    {
                        this.Power = 30;
                        this.flag = 1;
                        this.count = 0;
                        this.Rad = new Vec(My.X - this.X, My.Y - this.Y).Rad;
                    }
                    this.count++;
                    
                    break;
                case 1:
                    if (count == 60)
                    {
                        this.flag = 0;
                        this.Power = 0;
                        this.count = 0;
                    }
                    else
                    {
                        if (this.count % 10 == 0)
                        {
                            int dx = (int)(this.My.X / 2 - this.X / 2);
                            int dy = (int)(this.My.Y / 2 - this.Y / 2);
                            double rad = Math.Atan2(dy, dx)+(this.count%20==0?0.5:-0.5);
                            rad = rad * (180 / Math.PI);

                            //角度計算
                            this.fX = 20 * Math.Cos(rad / 180 * Math.PI);//進むx取得
                            this.fY = 20 * Math.Sin(rad / 180 * Math.PI);//進むy取得
                        }
                        this.X += this.fX;
                        this.Y += this.fY;
                    }
                    this.count++;
                    break;
            }

            this.count++;
            return null;
        }

        protected override void DrawGameObjectAfter()
        {
            base.DrawGameObjectAfter();
            if (this.flag == 0)
            {
                DX.DrawCircle((int)this.X, (int)this.Y, (300 - this.count) / 3, Program.blue, DX.FALSE);
            }
            
        }
    }
}
