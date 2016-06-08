using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using DxLibDLL;

namespace MachShooting
{
    public class ChargeEffect : ITransfer
    {
        /// <summary>
        /// 対象のオブジェクト
        /// </summary>
        private readonly GameObject go;

        /// <summary>
        /// f数
        /// </summary>
        private readonly int f;

        /// <summary>
        /// 最大の半径
        /// </summary>
        private readonly int maxR;

        /// <summary>
        /// 色
        /// </summary>
        private readonly Color color;

        /// <summary>
        /// カウント
        /// </summary>
        private int count;

        public bool Need
        {
            get
            {
                return this.count < this.f;
            }
        }

        public ChargeEffect(GameObject go, int f, int maxR,Color color)
        {
            this.go = go;
            this.f = f;
            this.maxR = maxR;
            this.color = color;
        }

        public void Draw()
        {
            if (this.Need)
            {
                DX.SetDrawBright(this.color.R, this.color.G, this.color.B);
                double ex = 1 - ((double)this.count / this.f);
                DX.DrawRotaGraph((int)this.go.X, (int)this.go.Y, ex, this.go.Rad, DXImage.Instance.charge, DX.TRUE);
                DX.SetDrawBright(255, 255, 255);
            }
        }

        public List<AttackObject> Process()
        {
            if (this.Need)
            {
                this.count++;
            }

            return null;
        }
    }
}
