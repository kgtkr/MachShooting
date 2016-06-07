﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using MachShooting.Graphic;

namespace MachShooting
{
    /// <summary>
    /// ネコ
    /// ガート(スペイン語)
    /// </summary>
    public class Gato : Enemy
    {
        public Gato(My my) : base("ガトー", 0, 1500, my, Program.gato)
        {
        }

        protected override List<AttackObject> Process_Enemy()
        {
            if (!this.SyncTransfer.Need)
            {
                this.SyncTransfer.Add(new ChargeEffect(this, 180, (int)this.R * 3, System.Drawing.Color.Red));
                this.SyncTransfer.Add(new SetProperty(() => this.Power = 30));
                this.SyncTransfer.Add(new Zigzag(this, this.My, 10 * Program.ROOT2, 10 * Program.ROOT2, 4, 6));
                this.SyncTransfer.Add(new SetProperty(() => this.Power = 0));
                this.SyncTransfer.Add(new ULM(this, Vec.NewRadLength(this.ToMapRad(Radian.UP), 10), 20));
                this.SyncTransfer.Add(new AttackObjectTransfer(() => new Bullet(this.Circle.Dot, 20, Vec.NewRadLength((this.My.Circle.Dot - this.Circle.Dot).Rad, 10), Program.bulletSmall[0])));
            }

            return null;
        }

        protected override void DrawGameObjectAfter()
        {
        }
    }
}