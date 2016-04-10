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
    /// ゲームに出てくるオブジェクト
    /// </summary>
    public abstract class GameObject
    {
        #region フィールド
        /// <summary>
        /// 現在の円
        /// </summary>
        private Circle circle;

        /// <summary>
        /// 前Fの点
        /// </summary>
        private Dot dot;

        /// <summary>
        /// 攻撃力。攻撃判定がないなら0
        /// </summary>
        private int power;

        /// <summary>
        /// 必要か
        /// </summary>
        private bool need;

        /// <summary>
        /// 描画を行うか？
        /// </summary>
        private bool draw;

        /// <summary>
        /// 画像
        /// </summary>
        private Image image;

        /// <summary>
        /// Nextが呼ばれた回数
        /// </summary>
        private int count;

        /// <summary>
        /// 方向
        /// </summary>
        private double rad;
        #endregion
        #region プロパティ
        /// <summary>
        /// 円
        /// </summary>
        public Circle Circle
        {
            get { return this.circle; }
            protected set { this.circle = value; }
        }

        /// <summary>
        /// ラジアン
        /// </summary>
        public double Rad
        {
            get { return this.rad; }
            set { this.rad = value; }
        }

        /// <summary>
        /// 前Fの点
        /// </summary>
        public Dot Dot
        {
            get { return this.dot; }
        }

        /// <summary>
        /// 攻撃力。0なら当たり判定を持たない
        /// </summary>
        public int Power
        {
            get { return this.power; }
            protected set { this.power = value; }
        }

        /// <summary>
        /// 必要か？
        /// </summary>
        public bool Need
        {
            get { return this.need; }
            protected set { this.need = value; }
        }

        /// <summary>
        /// オブジェクトが画面内にいるか？
        /// </summary>
        public bool In
        {
            get { return new Vec(this.X-Game.WINDOW_R,this.Y-Game.WINDOW_R).LengthSquare <= (Game.WINDOW_R - this.R) * (Game.WINDOW_R - this.R); }
        }

        /// <summary>
        /// 描画を行うか？
        /// </summary>
        protected bool Draw
        {
            get { return this.draw; }
            set { this.draw = value; }
        }

        /// <summary>
        /// 画像
        /// </summary>
        public Image Image
        {
            get { return this.image; }
            protected set { this.image = value; }
        }

        /// <summary>
        /// Nextが呼ばれた回数
        /// </summary>
        public int Count
        {
            get { return this.count; }
        }

        #region 円の実装
        /// <summary>
        /// x座標
        /// </summary>
        public double X
        {
            get
            {
                return this.circle.X;
            }
            set
            {
                this.circle.X = value;
            }
        }

        /// <summary>
        /// y座標
        /// </summary>
        public double Y
        {
            get
            {
                return this.circle.Y;
            }
            set
            {
                this.circle.Y = value;
            }
        }

        /// <summary>
        /// 半径
        /// </summary>
        public double R
        {
            get
            {
                return this.circle.R;
            }
            set
            {
                this.circle.R = value;
            }
        }
        #endregion
        #endregion
        #region コンストラクタ

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dot">座標</param>
        /// <param name="power">攻撃力</param>
        /// <param name="image">画像</param>
        public GameObject(Dot dot,int power,Image image,double rad)
        {
            this.circle = new Circle(dot,image.r);
            this.dot = circle.Dot;
            this.power = power;
            this.draw = true;
            this.need = true;
            this.image = image;
            this.rad = rad;
        }
        #endregion
        #region メソッド
        /// <summary>
        /// 処理を行います
        /// Next→処理→当たり判定の順に呼び出して下さい
        /// </summary>
        public void Next()
        {
            this.dot = this.circle.Dot;
            this.count++;
        }

        /// <summary>
        /// オブジェクトを中に入れます
        /// </summary>
        protected void Input()
        {
            if (!this.In)
            {
                double rad = new Vec(this.X - Game.WINDOW_R, this.Y - Game.WINDOW_R).Rad;
                this.circle.Dot = (Dot)Vec.NewRadLength(rad, Game.WINDOW_R - this.R);
                this.X += Game.WINDOW_R;
                this.Y += Game.WINDOW_R;
            }
        }

        /// <summary>
        /// 当たり判定を行います
        /// </summary>
        /// <param name="o1">オブジェクト1</param>
        /// <param name="o2">オブジェクト2></param>
        /// <returns></returns>
        public static bool Hit(GameObject o1,GameObject o2)
        {
            return Graphic.Hit(new Capsule(new Line(o1.circle.Dot, o1.dot), o1.R),
                new Capsule(new Line(o2.circle.Dot, o2.dot), o2.R));
        }

        /// <summary>
        /// 攻撃された時に呼び出されるメソッド
        /// 必要に応じてオーバーライドして下さい
        /// </summary>
        /// <param name="power">パワー</param>
        /// <returns>受けたダメージ</returns>
        public virtual int Suffer(int power)
        {
            return 0;
        }

        /// <summary>
        /// 攻撃が当たった時に呼び出されるメソッド
        /// 必要に応じてオーバーライドして下さい
        /// </summary>
        /// <param name="damage">与えたダメージ</param>
        public virtual void Attack(int damage)
        {
        }

        /// <summary>
        /// 描画を行います
        /// </summary>
        public void DrawObject()
        {
            DrawGameObjectBefore();
            if (this.draw)
            {
                DrawGraph(this.image);
            }
            DrawGameObjectAfter();
        }

        /// <summary>
        /// 画像を描画する前に呼び出されるメソッド
        /// Doawプロパティの値に関係なく呼び出されます
        /// 追加で描画を行う場合は必要に応じてオーバーライドして下さい
        /// </summary>
        protected virtual void DrawGameObjectBefore()
        {

        }

        /// <summary>
        /// 画像を描画した後に呼び出されるメソッド
        /// Doawプロパティの値に関係なく呼び出されます
        /// 追加で描画を行う場合は必要に応じてオーバーライドして下さい
        /// </summary>
        protected virtual void DrawGameObjectAfter()
        {

        }

        /// <summary>
        /// 画像を矩形の中心に描画します
        /// </summary>
        /// <param name="grHandle"></param>
        /// <returns>失敗したら-1</returns>
        public int DrawGraph(Image grHandle)
        {
            return DX.DrawRotaGraph((int)this.X, (int)this.Y,1,this.rad-grHandle.rad,grHandle.image, DX.TRUE);
        }
        #endregion
        #region 抽象メソッド
        #endregion
    }
}
