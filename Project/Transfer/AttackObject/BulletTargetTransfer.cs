using CircleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachShooting
{
    public class BulletTargetTransfer : ITransfer
    {
        /// <summary>
        /// 発射するゲームオブジェクト
        /// </summary>
        private readonly GameObject go;

        /// <summary>
        /// ターゲット
        /// </summary>
        private readonly GameObject target;

        /// <summary>
        /// 攻撃力
        /// </summary>
        private readonly int power;

        /// <summary>
        /// スピード
        /// </summary>
        private readonly int speed;

        /// <summary>
        /// 画像
        /// </summary>
        private readonly Image image;

        /// <summary>
        /// メタデータ
        /// </summary>
        private readonly int[] meta;

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

        public BulletTargetTransfer(GameObject go,GameObject target, int power, int speed, Image image, int[] meta = null)
        {
            this.go = go;
            this.target = target;
            this.power = power;
            this.speed = speed;
            this.image = image;
            this.meta = meta;
        }

        public void Draw()
        {
        }

        public List<AttackObject> Process()
        {
            if (this.need)
            {
                this.need = false;
                return new List<AttackObject> { new Bullet(this.go.Circle.Dot,
                    this.power,
                    Vec.NewRadLength(new Vec(this.target.X-this.go.X,this.target.Y-this.go.Y).Rad,this.speed),
                    this.image,
                    this.meta) };
            }
            else
            {
                return null;
            }
        }
    }
}
