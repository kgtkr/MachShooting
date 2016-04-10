using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircleLib;
using DxLibDLL;

namespace MachShooting
{
    /// <summary>
    /// ライオン
    /// レオーネ(イタリア)
    /// </summary>
    public class Leone : Enemy
    {
        /// <summary>
        /// アクション
        /// 0:待機、1:突進、2:小爆発、3:連続弾、4:全方位弾、5:大爆発
        /// </summary>
        private int action;

        /// <summary>
        /// アクションが始まってからの時間
        /// </summary>
        private int count;

        /// <summary>
        /// 突進で動く量
        /// </summary>
        private Vec 突進_vec;

        /// <summary>
        /// 大爆発で動く量
        /// </summary>
        private Vec 大爆発_vec;

        /// <summary>
        /// あと何回動くか
        /// </summary>
        private int 大爆発_count;

        public Leone(My my) : base("レオーネ", 0, 3000, my, Program.leone)
        {

        }

        protected override List<AttackObject> Process_Enemy()
        {
            List<AttackObject> list = new List<AttackObject>();
            if (this.action == 0)
            {
                if (this.count > 180)
                {

                    this.action = DX.GetRand(4) + 1;
                    if (this.action == 1)
                    {
                        突進(true);
                    }
                    if (this.action == 5)
                    {
                        大爆発(true);
                    }
                    this.count = 0;
                }
            }
            switch (this.action)
            {
                case 1:
                    突進(false);
                    break;
                case 2:
                    list.AddList(小爆発());
                    break;
                case 3:
                    list.AddList(連続弾());
                    break;
                case 4:
                    list.AddList(全方位弾());
                    break;
                case 5:
                    list.AddList(大爆発(false));
                    break;
            }
            this.count++;
            return list;
        }

        private void 突進(bool fast)
        {
            if (fast)
            {
                this.突進_vec = new Vec(My.X - this.X, My.Y - this.Y);
                this.突進_vec.Length = 10;
                this.Power = 30;
                this.Rad = this.突進_vec.Rad;
            }
            else
            {
                this.X += this.突進_vec.X;
                this.Y += this.突進_vec.Y;

                if (this.count > 60)
                {
                    this.action = 0;
                    this.count = 0;
                    this.Power = 0;
                }
            }
        }

        private List<AttackObject> 小爆発()
        {
            List<AttackObject> list = null;

            if (this.count % 30 == 0)
            {
                list = new List<AttackObject>();
                list.Add(new Bom(new Circle(this.My.Dot, 30), 40, 60, 10, 255, 0, 0));
            }

            if (this.count > 120)
            {
                this.action = 0;
                this.count = 0;
            }

            return list;
        }

        private List<AttackObject> 連続弾()
        {
            List<AttackObject> list = null;

            if (this.count % 10 == 0)
            {
                list = new List<AttackObject>();
                Vec v= new Vec(My.X - this.X, My.Y - this.Y);
                v.Length = 10;
                list.Add(new Bullet(this.Dot,30,v,Program.bulletBig[0],null));
            }

            if (this.count > 120)
            {
                this.action = 0;
                this.count = 0;
            }

            return list;
        }

        private List<AttackObject> 全方位弾()
        {
            List<AttackObject> list = new List<AttackObject>();

            for (int i = 0; i < 36; i++)
            {
                list.Add(new Bullet(this.Dot, 50, Vec.NewRadLength((i * 10.0).ToRad(), 10), Program.bulletBig[0], null));
            }

            this.action = 0;
            this.count = 0;
            return list;
        }

        private List<AttackObject> 大爆発(bool fast)
        {
            if (fast)
            {
                this.大爆発_vec= new Vec(My.X - this.X, My.Y - this.Y);
                this.大爆発_count = (int)this.大爆発_vec.Length / 30+1;
                this.大爆発_vec.Length = 30;
            }
            else
            {
                if (this.大爆発_count != 0)
                {
                    this.X += this.大爆発_vec.X;
                    this.Y += this.大爆発_vec.Y;
                    this.大爆発_count--;
                }
                else
                {
                    List<AttackObject> list = new List<AttackObject>();
                    list.Add(new Bom(new Circle(this.Dot, 300), 80, 90, 20, 255, 0, 0));
                    this.action = 0;
                    this.count = 0;
                    return list;
                }
            }
            return null;
        }

        protected override void DrawGameObjectAfter()
        {
            if (this.action == 0)
            {
                DX.DrawCircle((int)this.X, (int)this.Y, (120 - this.count)*3, Program.red, DX.FALSE);
            }
            base.DrawGameObjectAfter();
        }
    }
}
