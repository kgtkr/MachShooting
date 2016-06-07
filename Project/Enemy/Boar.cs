using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using MachShooting.Graphic;
using System.Drawing;

namespace MachShooting
{
    /// <summary>
    /// イノシシ
    /// ボア(英語)
    /// </summary>
    public class Boar : Enemy
    {
        public Boar(My my) : base("ボア", 0, 1500, my, Program.boar)
        {

        }

        protected override List<AttackObject> Process_Enemy()
        {
            if (!this.SyncTransfer.Need)
            {
                this.SyncTransfer.Add(new ChargeEffect(this, 300, (int)this.R * 3, Color.Red));
                this.SyncTransfer.Add(new SetProperty(() => this.Power = 20));
                this.SyncTransfer.Add(new ULMTarget(this, this.My, 10, 60));
                this.SyncTransfer.Add(new SetProperty(() => this.Power = 0));
            }
            return null;
        }
    }
}

