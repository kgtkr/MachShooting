using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachShooting.Graphic;
using DxLibDLL;

namespace MachShooting
{
    public class Bom : AttackObject
    {
        /// <summary>
        /// 攻撃力
        /// </summary>
        private int p;

        /// <summary>
        /// 待ち時間
        /// </summary>
        private int wait;

        /// <summary>
        /// 長さ
        /// </summary>
        private int len;

        private int r;
        private int g;
        private int b;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c">円</param>
        /// <param name="power">パワー</param>
        /// <param name="wait">待ち時間</param>
        /// <param name="len">長さ</param>
        public Bom(Circle c,int power,int wait,int len,int r,int g,int b) : base(c.Dot,0,new Image(Program.charge,c.R,0),0,null)
        {
            this.p = power;
            this.wait = wait;
            this.len = len;
            this.Draw = false;
            this.r = r;
            this.g = g;
            this.b = b;
        }


        protected override List<AttackObject> ProcessAttackObject()
        {
            if (this.Count >= this.wait)
            {
                this.Power = this.p;
            }
            if (this.Count >= this.wait + this.len)
            {
                this.Life = false;
            }
            if (!this.In)
            {
                this.Life = false;
                this.Need = false;
            }

            return null;
        }

        protected override void DrawGameObjectAfter()
        {
            if (this.Life)
            {
                int r;
                if (this.wait >= this.Count)
                {
                    r = (int)(this.R * (1.0-((double)this.Count/ this.wait)));
                }
                else
                {
                    r = (int)this.R;
                }

                DX.SetDrawBright(this.r, this.g, this.b);
                DX.DrawExtendGraph((int)this.X - r,
                        (int)this.Y - r,
                        (int)this.X + r + 1,
                        (int)this.Y + r + 1,
                        this.Image.image,
                        DX.TRUE);
                DX.SetDrawBright(255, 255, 255);

            }
        }
    }
}
