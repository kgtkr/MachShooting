using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachShooting.Graphic;

namespace MachShooting
{
    /// <summary>
    /// 等速直線運動
    /// Uniform linear motion
    /// </summary>
    public class ULM : ITransfer
    {
        /// <summary>
        /// 対象のゲームオブジェクト
        /// </summary>
        private readonly GameObject go;

        /// <summary>
        /// 1Fの移動量
        /// </summary>
        private readonly Vec vec;

        /// <summary>
        /// 移動するF数
        /// </summary>
        private readonly int f;

        /// <summary>
        /// 現在のカウント
        /// </summary>
        private int count;

        public bool Need
        {
            get
            {
                return this.count < this.f;
            }
        }

        public ULM(GameObject go,Vec vec,int f)
        {
            this.go = go;
            this.vec = vec;
            this.f = f;
        }

        public void Draw()
        {
        }

        public List<AttackObject> Process()
        {
            if (this.Need)
            {
                this.go.X += this.vec.X;
                this.go.Y += this.vec.Y;

                this.count++;
            }
            return null;
        }
    }
}
