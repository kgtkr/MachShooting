using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachShooting
{
    public abstract class TransferList:List<ITransfer>,ITransfer
    {
        /// <summary>
        /// 必要か？
        /// </summary>
        public bool Need
        {
            get
            {
                return this.ITransferListNeed();
            }
        }

        public TransferList(List<ITransfer> list=null)
        {
            if (list != null)
            {
                foreach (ITransfer t in list)
                {
                    this.Add(t);
                }
            }
        }

        public abstract List<AttackObject> Process();

        public abstract void Draw();
    }
}
