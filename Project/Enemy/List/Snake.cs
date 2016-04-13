using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using CircleLib;

namespace MachShooting
{
    /// <summary>
    /// ヘビ
    /// スネーク(英語)
    /// </summary>
    public class Snake:Enemy
    {
        #region 定数
        #endregion
        #region フィールド
        /// <summary>
        /// 0:停止、1:移動、2:回転
        /// </summary>
        private int flag;

        /// <summary>
        /// カウント
        /// </summary>
        private int count;

        /// <summary>
        /// ドット
        /// </summary>
        private Dot dot;

        /// <summary>
        /// 突進で動く量
        /// </summary>
        private Vec vec;

        /// <summary>
        /// 突進であと何回動くか？
        /// </summary>
        private int c;
        #endregion
        #region プロパティ
        #endregion
        #region コンストラクタ
        public Snake(My my):base("スネーク", 0, 1500, my, Program.snake)
        {

        }
        #endregion
        #region メソッド

        protected override List<AttackObject> Process_Enemy()
        {
            List<AttackObject> list = null;
            switch (this.flag)
            {
                case 0:
                    if (this.count == 500)
                    {
                        this.count = 0;
                        this.Power = 30;
                        this.flag = 1;
                        this.dot = this.My.Circle.Dot;
                        Vec v = new Vec(this.dot.X - this.X, this.dot.Y - this.Y);
                        this.c = (int)(v.Length / 20)+1;
                        this.vec = v.Unit*20;
                        this.Rad = v.Rad;
                    }
                    break;
                case 1:
                    if (this.c == 0)
                    {
                        this.count = 0;
                        this.flag = 2;
                        this.Power = 50;
                    }
                    else
                    {
                        c--;
                        this.X += this.vec.X;
                        this.Y += this.vec.Y;
                    }
                    break;
                case 2:
                    if (this.count == 350)
                    {
                        this.count = 0;
                        this.flag = 0;
                        this.Power = 0;
                    }
                    else
                    {
                        double rad = ((this.Count % 36)*10) * Math.PI / 180;
                        Vec v = Vec.NewRadLength(rad,this.count<50? this.count*6:300-(this.count-50));
                        this.X = this.dot.X + v.X;
                        this.Y = this.dot.Y + v.Y;
                        if (this.count % 30 == 0)
                        {
                            list = new List<AttackObject>();
                            Vec v2 = new Vec(this.My.X - this.X, this.My.Y - this.Y);
                            list.Add(new Bullet(this.Circle.Dot,30,Vec.NewRadLength(v2.Rad,10),Program.bulletSmall[0],null));
                        }
                    }
                    break;
            }
            this.count++;
            return list;
        }

        protected override void DrawGameObjectAfter()
        {
            base.DrawGameObjectAfter();
            if (this.flag == 0)
            {
                DX.DrawCircle((int)this.X, (int)this.Y, (500 - this.count) / 5, Program.blue, DX.FALSE);
            }
        }
        #endregion
        #region 実装メソッド
        #endregion
        #region 未実装メソッド
        #endregion
    }
}
