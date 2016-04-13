using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircleLib;

namespace MachShooting
{
    /// <summary>
    /// オブジェクト操作の委譲用インターフェイス
    /// </summary>
    public interface ITransfer
    {
        /// <summary>
        /// 処理が終わってないならtrue
        /// </summary>
        bool Need
        {
            get;
        }

        /// <summary>
        /// 処理を行います
        /// </summary>
        /// <returns>攻撃オブジェクト</returns>
        List<AttackObject> Process();

        /// <summary>
        /// 描画を行います
        /// </summary>
        void Draw();
    }
}
