using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachShooting
{
    public class AttackObjectTransfer : ITransfer
    {
        /// <summary>
        /// ゲームオブジェクト
        /// </summary>
        private readonly GameObject go;

        /// <summary>
        /// 攻撃オブジェクト
        /// </summary>
        private AttackObject ao;

        public bool Need
        {
            get
            {
                return this.ao != null;
            }
        }

        public AttackObjectTransfer(GameObject go, AttackObject ao)
        {
            this.go = go;
            this.ao = ao;
        }

        public void Draw()
        {
        }

        public List<AttackObject> Process()
        {
            if (this.Need)
            {
                this.ao.X = this.go.X;
                this.ao.Y = this.go.Y;
                this.ao.DotF = this.go.Circle.Dot;

                AttackObject ao = this.ao;
                this.ao = null;
                return new List<AttackObject> { ao };
            }
            else
            {
                return null;
            }
        }
    }
}
