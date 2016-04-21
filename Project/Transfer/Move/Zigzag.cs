using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircleLib;

namespace MachShooting
{
    /// <summary>
    /// ジグザグ突進
    /// </summary>
    public class Zigzag : ITransfer
    {
        /// <summary>
        /// ゲームオブジェクト
        /// </summary>
        private readonly GameObject go;

        /// <summary>
        /// ターゲット
        /// </summary>
        private readonly GameObject target;

        /// <summary>
        /// 幅
        /// </summary>
        private readonly double w;

        /// <summary>
        /// 高さ
        /// </summary>
        private readonly double h;

        /// <summary>
        /// 1ジグザグのF数
        /// </summary>
        private readonly int number_1;

        /// <summary>
        /// 回数
        /// </summary>
        private readonly int number;

        /// <summary>
        /// ベクトル1 
        /// </summary>
        private Vec v1;

        /// <summary>
        /// ベクトル2 
        /// </summary>
        private Vec v2;

        /// <summary>
        /// 1回のカウント
        /// </summary>
        private int count_1;

        /// <summary>
        /// 全体のカウント
        /// </summary>
        private int count;


        public bool Need
        {
            get
            {
                return this.count <= this.number;
            }
        }

        public Zigzag(GameObject go,GameObject target,double w,double h,int f,int number)
        {
            this.go = go;
            this.target = target;
            this.w = w;
            this.h = h;
            this.number_1 = f;
            this.number = number;
        }

        public void Draw()
        {
        }

        public List<AttackObject> Process()
        {
            if (this.Need)
            {
                int f = this.number_1;

                if (this.count == 0)
                {
                    if (this.count_1 == 0)
                    {
                        //ベクトル計算
                        Vec v1 = new Vec(this.w, this.h);
                        Vec v2 = new Vec(-this.w, this.h);
                        this.go.Rad = new Vec(this.target.X - this.go.X, this.target.Y - this.go.Y).Rad;

                        this.v1 = Vec.NewRadLength(this.go.ToMapRad(v1.Rad), v1.Length);
                        this.v2 = Vec.NewRadLength(this.go.ToMapRad(v2.Rad), v2.Length);
                    }
                    //最初なら移動量は半分
                    f %= 2;
                }else if (this.count == this.number)
                {
                    //最後なら移動量は半分
                    f %= 2;
                }

                Vec v = this.count % 2 == 0 ? this.v1 : this.v2;
                this.go.X += v.X;
                this.go.Y += v.Y;

                this.count_1++;
                if (this.count_1 >= f)
                {
                    this.count_1 = 0;
                    this.count++;
                }
            }
            return null;
        }
    }
}
