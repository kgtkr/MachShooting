using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircleLib;

namespace MachShooting
{
    public class ULMTarget : ITransfer
    {
        /// <summary>
        /// 対象のゲームオブジェクト
        /// </summary>
        private readonly GameObject go;

        /// <summary>
        /// ターゲット
        /// </summary>
        private readonly GameObject target;

        /// <summary>
        /// 速度
        /// </summary>
        private readonly double speed;

        /// <summary>
        /// 移動するF数
        /// </summary>
        private readonly int f;

        /// <summary>
        /// 現在のカウント
        /// </summary>
        private int count;

        /// <summary>
        /// ベクトル
        /// </summary>
        private Vec vec;

        public bool Need
        {
            get
            {
                return this.count < this.f;
            }
        }

        public void Draw()
        {
        }

        public ULMTarget(GameObject go,GameObject target,double speed,int f)
        {
            this.go = go;
            this.target = target;
            this.speed = speed;
            this.f = f;
        }

        public List<AttackObject> Process()
        {
            if (this.Need)
            {
                if (this.count == 0)
                {
                    this.vec = Vec.NewRadLength(new Vec(this.target.X-this.go.X,this.target.Y-this.go.Y).Rad, this.speed);
                }
                this.go.X += this.vec.X;
                this.go.Y += this.vec.Y;

                this.count++;
            }
            return null;
        }
    }
}
