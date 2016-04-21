using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircleLib;

namespace MachShooting
{
    public class SetRadTarget:ITransfer
    {
        /// <summary>
        /// 対象のオブジェクト
        /// </summary>
        private readonly GameObject go;

        /// <summary>
        /// ターゲット
        /// </summary>
        private readonly GameObject target;

        /// <summary>
        /// 必要か
        /// </summary>
        private bool need = true;

        public bool Need
        {
            get
            {
                return this.need;
            }
        }

        public SetRadTarget(GameObject go, GameObject target)
        {
            this.go = go;
            this.target = target;
        }

        public List<AttackObject> Process()
        {
            if (this.need)
            {
                this.go.Rad = new Vec(this.target.X-this.go.X,this.target.Y-this.go.Y).Rad;
                this.need = false;
            }

            return null;
        }

        public void Draw()
        {
        }
    }
}
