using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachShooting.Graphic;

namespace MachShooting
{
    public class CircleTransfer : ITransfer
    {
        /// <summary>
        /// 対象のゲームオブジェクト
        /// </summary>
        private readonly GameObject go;

        /// <summary>
        /// 初期ラジアン
        /// </summary>
        private readonly double rad;

        /// <summary>
        /// ラジアン速度
        /// </summary>
        private readonly double speedRad;

        /// <summary>
        /// 何F続くか
        /// </summary>
        private readonly int f;

        /// <summary>
        /// カウント
        /// </summary>
        private int count;

        /// <summary>
        /// 中心座標
        /// </summary>
        private Vec dot;

        /// <summary>
        /// 半径
        /// </summary>
        private Func<int,int> r;

        public bool Need
        {
            get
            {
                return this.count < this.f;
            }
        }

        public CircleTransfer(GameObject go,Func<int,int> r,double rad,double speedRad,int f)
        {
            this.go = go;
            this.r = r;
            this.rad = rad;
            this.speedRad = speedRad;
            this.f = f;
        }

        public void Draw()
        {
        }

        public List<AttackObject> Process()
        {
            if (this.Need)
            {
                if (this.count == 0)
                {
                    this.dot = this.go.Circle.Dot;
                }

                double rad = this.rad + this.speedRad * this.count;
                double r = this.r(this.count);
                Vec vec = Vec.NewRadLength(rad, r);

                this.go.X = this.dot.X+vec.X;
                this.go.Y = this.dot.Y+vec.Y;

                this.count++;
            }

            return null;
        }
    }
}
