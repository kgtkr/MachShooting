using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachShooting
{
    public class SetPower:ITransfer
    {
        /// <summary>
        /// 対象のオブジェクト
        /// </summary>
        private readonly GameObject go;

        /// <summary>
        /// 攻撃力
        /// </summary>
        private readonly int power;

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

        public SetPower(GameObject go,int power)
        {
            this.go = go;
            this.power = power;
        }

        public List<AttackObject> Process()
        {
            if (this.need)
            {
                this.go.Power = this.power;
                this.need = false;
            }

            return null;
        }

        public void Draw()
        {
        }
    }
}
