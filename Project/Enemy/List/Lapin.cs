using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using CircleLib;
using System.Drawing;

namespace MachShooting
{
    /// <summary>
    /// ウサギ
    /// ラパン(フランス)
    /// </summary>
    public class Lapin : Enemy
    {
        public Lapin(My my) : base("ラパン", 0, 1500, my, Program.lapin)
        {

        }
        protected override List<AttackObject> Process_Enemy()
        {
            if (!this.SyncTransfer.Need)
            {
                this.SyncTransfer.Add(new ChargeEffect(this, 180, (int)this.R * 3, Color.Red));
                this.SyncTransfer.Add(new SetProperty(()=>this.Power=40));
                for (int i = 0; i < 10; i++)
                {
                    this.SyncTransfer.Add(new ULMTarget(this, this.My, 20, 10,i%2==0?-0.2:0.2));
                }
                this.SyncTransfer.Add(new SetProperty(() => this.Power = 0));
            }
            return null;
        }

        protected override void DrawGameObjectAfter()
        {
        }
    }
}
