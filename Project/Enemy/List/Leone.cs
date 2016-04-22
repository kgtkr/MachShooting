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
        public Leone(My my) : base("レオーネ", 0, 3000, my, Program.leone)
        {

        }

        protected override List<AttackObject> Process_Enemy()
        {

            if (!this.SyncTransfer.Need)
            {
                int action = DX.GetRand(4) + 1;
                switch (action)
                {
                    case 1:
                        this.SyncTransfer.Add(new ChargeEffect(this, 90, (int)this.R * 3, System.Drawing.Color.Yellow));
                        this.SyncTransfer.Add(new SetProperty(() => this.Power = 30));
                        this.SyncTransfer.Add(new ULMTarget(this, this.My, 10, 60));
                        this.SyncTransfer.Add(new SetProperty(() => this.Power = 0));
                        break;
                    case 2:
                        this.SyncTransfer.Add(new ChargeEffect(this, 120, (int)this.R * 3, System.Drawing.Color.Brown));
                        for (int i = 0; i < 4; i++)
                        {
                            this.SyncTransfer.Add(new AttackObjectTransfer(()=>new Bom(new Circle(this.My.Circle.Dot, 30), 40, 60, 10, 255, 0, 0)));
                            this.SyncTransfer.Add(new Wait(30));
                        }
                        break;
                    case 3:
                        this.SyncTransfer.Add(new ChargeEffect(this, 120, (int)this.R * 3, System.Drawing.Color.Orange));
                        for (int i = 0; i < 12; i++)
                        {
                            this.SyncTransfer.Add(new AttackObjectTransfer(() => new Bullet(this.Circle.Dot, 30, Vec.NewRadLength((this.My.Circle.Dot - this.Circle.Dot).Rad, 10), Program.bulletBig[0])));
                            this.SyncTransfer.Add(new Wait(10));
                        }
                        break;
                    case 4:
                        this.SyncTransfer.Add(new ChargeEffect(this, 120, (int)this.R * 3, System.Drawing.Color.Blue));
                        AsyncTransfer at = new AsyncTransfer();
                        for (int i = 0; i < 36; i++)
                        {
                            double rad = (i * 10.0).ToRad();
                            at.Add(new AttackObjectTransfer(() => new Bullet(this.Circle.Dot, 50, Vec.NewRadLength(rad, 10), Program.bulletBig[0], null)));
                        }
                        this.SyncTransfer.Add(at);
                        break;
                    case 5:
                        this.SyncTransfer.Add(new ChargeEffect(this, 180, (int)this.R * 5, System.Drawing.Color.Red));
                        this.SyncTransfer.Add(new AttackObjectTransfer(()=> new Bom(new Circle(this.Circle.Dot, 300), 80, 90, 20, 255, 0, 0)));
                        break;
                }
            }
            return null;
        }
    }
}
