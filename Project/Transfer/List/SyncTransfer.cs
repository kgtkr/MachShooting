using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachShooting
{
    /// <summary>
    /// 同期的に複数の委譲処理を行うクラス
    /// </summary>
    public class SyncTransfer:TransferList
    {
        /// <summary>
        /// 新しいイントランスを作成します
        /// </summary>
        /// <param name="transferList"></param>
        public SyncTransfer(List<ITransfer> transferList=null):base(transferList)
        {
        }

        public override void Draw()
        {
            bool loop = true;

            while (loop)
            {
                if (this.Need)
                {
                    if (this[0].Need)
                    {
                        this[0].Draw();
                        loop = false;
                    }
                    else//最初からNeed=falseの操作ならループする
                    {
                        loop = true;
                    }

                    if (!this[0].Need)//必要ないなら削除
                    {
                        this.Remove(this[0]);
                    }
                }
                else
                {
                    loop = false;
                }
            }
        }

        public override List<AttackObject> Process()
        {
            
            bool loop = true;
            List<AttackObject> list = null;


            while (loop)
            {
                if (this.Need)
                {
                    if (this[0].Need)
                    {
                        list = this[0].Process();
                        loop = false;
                    }
                    else//最初からNeed=falseの操作ならループする
                    {
                        loop = true;
                    }

                    if (!this[0].Need)//必要ないなら削除
                    {
                        this.Remove(this[0]);
                    }
                }
                else
                {
                    loop = false;
                }
            }

            return list;
        }
    }
}
