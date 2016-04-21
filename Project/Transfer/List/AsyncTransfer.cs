using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachShooting
{
    /// <summary>
    /// 非同期的に複数の委譲処理を行うクラス
    /// </summary>
    public class AsyncTransfer : TransferList
    {
        /// <summary>
        /// 新しいイントランスを作成します
        /// </summary>
        /// <param name="transferList"></param>
        public AsyncTransfer(List<ITransfer> transferList=null):base(transferList)
        { 
        }

        public override void Draw()
        {
            foreach(ITransfer t in this.ToArray())
            {
                if (t.Need)
                {
                    t.Draw();
                }
                
                if(!t.Need)
                {
                    this.Remove(t);
                }
            }
        }

        public override List<AttackObject> Process()
        {
            List<AttackObject> list = new List<AttackObject>();

            foreach (ITransfer t in this.ToArray())
            {
                if (t.Need)
                {
                    list.AddList(t.Process());
                }

                if (!t.Need)
                {
                    this.Remove(t);
                }
            }

            return list;
        }
    }
}
