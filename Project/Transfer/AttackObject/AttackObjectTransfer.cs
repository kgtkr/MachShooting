﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachShooting
{
    public class AttackObjectTransfer : ITransfer
    {
        private readonly Func<AttackObject> ao;

        private bool need=true;

        public bool Need
        {
            get
            {
                return this.need;
            }
        }

        public AttackObjectTransfer(Func<AttackObject> ao)
        {
            this.ao = ao;
        }

        public void Draw()
        {
        }

        public List<AttackObject> Process()
        {
            if (this.Need)
            {
                this.need = false;
                return new List<AttackObject> { this.ao() };
            }
            else
            {
                return null;
            }
        }
    }
}
