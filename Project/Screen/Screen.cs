//各画面
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;

namespace MachShooting
{
    /// <summary>
    /// 各画面のクラス
    /// </summary>
    public abstract class Screen
    {
        private bool need = true;
        private bool decision = false;

        /// <summary>
        /// 必要か？
        /// </summary>
        public bool Need
        {
            get { return this.need; }
            protected set { this.need = value; }
        }

        /// <summary>
        /// 決定になったか？
        /// </summary>
        public bool Decision
        {
            get { return this.decision; }
            protected set { this.decision = value; }
        }

        public abstract void Draw();

        public abstract void Process(byte[] key, byte[] key2);
    }
}
