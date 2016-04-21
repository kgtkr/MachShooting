using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachShooting
{
    public class SetRad:ITransfer
    {
        /// <summary>
        /// 対象のオブジェクト
        /// </summary>
        private readonly GameObject go;

        /// <summary>
        /// ラジアン
        /// </summary>
        private readonly double rad;

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

        public SetRad(GameObject go, double rad)
        {
            this.go = go;
            this.rad = rad;
        }

        public List<AttackObject> Process()
        {
            if (this.need)
            {
                this.go.Rad = this.rad;
                this.need = false;
            }

            return null;
        }

        public void Draw()
        {
        }
    }
}
