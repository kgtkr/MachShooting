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
    /// ネコ
    /// ガート(スペイン語)
    /// </summary>
    public class Gato : Enemy
    {
        /// <summary>
        /// フラグ。0:待機、1:右前に移動、2:左前に移動、3:上がる
        /// </summary>
        private int flag = 0;

        /// <summary>
        /// 現在の行動を始めてからの時間
        /// </summary>
        private int count;

        /// <summary>
        /// ターゲットにつくまでの残り移動回数
        /// </summary>
        private int target;

        public Gato(My my) : base("ガトー", 0, 1500, my, Program.gato)
        {
        }

        protected override List<AttackObject> Process_Enemy()
        {
            List<AttackObject> attack = null;
            switch (this.flag)
            {
                case 0:
                    if (this.count == 180)
                    {
                        this.count = 0;
                        this.flag = 1;
                        Vec v = new Vec(My.X - this.X, My.Y - this.Y);
                        this.target = (int)(v.Length*Program.ROOT2)/60+1;
                        this.Rad = v.Rad;

                    }
                    break;
                case 1:
                    {
                        Vec v = Vec.NewRadLength(this.ToMapRad(Radian.RIGHT_DOWN), 20);
                        this.X += v.X;
                        this.Y += v.Y;
                        
                        if (this.count == 3)
                        {
                            target--;
                            this.count = 0;
                            this.flag = 2;
                            this.Power = 30;
                            if (this.target == 0)
                            {
                                this.flag = 3;
                                this.count = 0;
                                this.Power = 0;
                            }
                        }
                    }
                    break;
                case 2:
                    {
                        Vec v = Vec.NewRadLength(this.ToMapRad(Radian.LEFT_DOWN), 20);
                        this.X += v.X;
                        this.Y += v.Y;

                        if (this.count == 3)
                        {
                            target--;
                            this.count = 0;
                            this.flag = 1;
                            this.Power = 30;
                            if (this.target == 0)
                            {
                                this.flag = 3;
                                this.count = 0;
                                this.Power = 0;
                            }
                        }
                    }
                    break;
                case 3:
                    {
                        Vec v = Vec.NewRadLength(this.ToMapRad(Radian.UP), 10);
                        this.X += v.X;
                        this.Y += v.Y;

                        if (count == 60)
                        {
                            this.flag = 0;
                            this.count = 0;
                            attack = new List<AttackObject>();
                            Vec v2 = new Vec(this.My.X - this.X, this.My.Y - this.Y);
                            attack.Add(new Bullet(new Dot(this.X, this.Y), 20, Vec.NewRadLength(v2.Rad, 10), Program.bulletMedium[0], null));
                        }
                    }
                    break;
            }

            this.count++;
            return attack;
        }

        protected override void DrawGameObjectAfter()
        {
            base.DrawGameObjectAfter();
            if (this.flag == 0)
            {
                DX.DrawCircle((int)this.X, (int)this.Y, (180 - this.count) / 3, Program.blue, DX.FALSE);
            }
        }
    }
}
