using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachShooting
{
    public class Wait : ITransfer
    {
        /// <summary>
        /// 時間
        /// </summary>
        private readonly int wait;

        /// <summary>
        /// カウント
        /// </summary>
        private int count;

        public Wait(int wait)
        {
            this.wait = wait;
        }

        public bool Need
        {
            get
            {
                return this.count < this.wait;
            }
        }

        public void Draw()
        {
        }

        public List<AttackObject> Process()
        {
            if (this.Need)
            {
                this.count++;
            }

            return null;
        }
    }
}
