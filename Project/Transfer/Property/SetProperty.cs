using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachShooting
{
    /// <summary>
    /// プロパティをセットします
    /// </summary>
    public class SetProperty : ITransfer
    {
        private readonly Action action;

        private bool need=true;

        public bool Need
        {
            get
            {
                return this.need;
            }
        }

        public SetProperty(Action action)
        {
            this.action = action;

        }

        public void Draw()
        {
        }

        public List<AttackObject> Process()
        {
            if (this.need)
            {
                this.action();
                this.need = false;
            }

            return null;
        }
    }
}
