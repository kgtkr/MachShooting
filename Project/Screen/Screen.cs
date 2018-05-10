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
    internal abstract class Screen
    {
        private bool need = true;
        private bool decision = false;

        /// <summary>
        /// 必要か？
        /// </summary>
        internal bool Need
        {
            get { return this.need; }
            set { this.need = value; }
        }

        /// <summary>
        /// 決定になったか？
        /// </summary>
        public bool Decision
        {
            get { return this.decision; }
            protected set { this.decision = value; }
        }

        internal abstract void Draw();

        internal abstract void Process();
    }
}
