/*
メモ:GameObjectの公開メンバ
-getter
X
Y
R
Rad
Count

-メソッド
ToMapRad
ToObjRad
*/
//16-08-04クラス整理
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;

namespace MachShooting
{
    /// <summary>
    /// ゲームに出てくるオブジェクトのベースクラス
    /// </summary>
    public abstract class GameObject
    {
        #region 図形
        /// <summary>
        /// このオブジェクトの座標と半径を表す円
        /// </summary>
        internal Circle Circle { get; private set; }

        /// <summary>
        /// 前Fの座標
        /// </summary>
        internal Vec DotBefore { get; private set; }

        /// <summary>
        /// 方向
        /// </summary>
        public double Rad
        {
            get;
            internal set;
        }

        /// <summary>
        /// x座標
        /// </summary>
        public double X
        {
            get
            {
                return this.Circle.X;
            }
            internal set
            {
                var c = this.Circle;
                c.X = value;
                this.Circle = c;
            }
        }

        /// <summary>
        /// y座標
        /// </summary>
        public double Y
        {
            get
            {
                return this.Circle.Y;
            }
            internal set
            {
                var c = this.Circle;
                c.Y = value;
                this.Circle = c;
            }
        }

        /// <summary>
        /// 半径
        /// </summary>
        public double R
        {
            get
            {
                return this.Circle.R;
            }
            internal set
            {
                var c = this.Circle;
                c.R = value;
                this.Circle = c;
            }
        }

        /// <summary>
        /// 座標
        /// </summary>
        internal Vec Dot
        {
            get { return this.Circle.Dot; }
            set
            {
                var c = this.Circle;
                c.Dot = value;
                this.Circle = c;
            }
        }
        #endregion
        #region フィールド
        /// <summary>
        /// 攻撃力。攻撃判定がないなら0
        /// </summary>
        internal int Power { get; set; }

        /// <summary>
        /// このオブジェクトは必要か
        /// </summary>
        internal bool Need { get; set; }

        /// <summary>
        /// 自動的な描画を行うか？
        /// </summary>
        internal bool Draw { get; set; }

        /// <summary>
        /// 画像
        /// </summary>
        internal Image Image { get; set; }

        /// <summary>
        /// Nextが呼ばれた回数
        /// </summary>
        internal int Count { get; private set; }
        #endregion
        #region プロパティ
        /// <summary>
        /// オブジェクトが画面内にいるか？
        /// </summary>
        internal bool IsIn
        {
            get { return new Vec(this.X - Game.WINDOW_R, this.Y - Game.WINDOW_R).LengthSquare <= (Game.WINDOW_R - this.R) * (Game.WINDOW_R - this.R); }
        }
        #endregion
        #region コンストラクタ
        /// <summary>
        /// 位置、攻撃力、画像、ラジアンを使って初期化します
        /// </summary>
        /// <param name="dot">座標</param>
        /// <param name="power">攻撃力</param>
        /// <param name="image">画像</param>
        internal GameObject(Vec dot, int power, Image image, double rad)
        {
            this.Circle = new Circle(dot, image.r);
            this.DotBefore = Circle.Dot;
            this.Power = power;
            this.Draw = true;
            this.Need = true;
            this.Image = image;
            this.Rad = rad;
        }
        #endregion
        #region メソッド
        /// <summary>
        /// 処理を行います
        /// Next→処理→当たり判定の順に呼び出して下さい
        /// </summary>
        internal void Next()
        {
            this.DotBefore = this.Circle.Dot;
            this.Count++;
        }

        /// <summary>
        /// オブジェクトを中に入れます
        /// </summary>
        protected void Input()
        {
            if (!this.IsIn)
            {
                double rad = new Vec(this.X - Game.WINDOW_R, this.Y - Game.WINDOW_R).Rad;
                this.Dot = Vec.FromRadLength(rad, Game.WINDOW_R - this.R);
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
        internal static bool Hit(GameObject o1, GameObject o2)
        {
            return GraphicProcess.Hit(new Capsule(new Line(o1.Circle.Dot, o1.DotBefore), o1.R),
                new Capsule(new Line(o2.Circle.Dot, o2.DotBefore), o2.R));
        }

        /// <summary>
        /// 描画を行います
        /// </summary>
        internal void DrawObject()
        {
            DrawGameObjectBefore();
            if (this.Draw)
            {
                DrawGraph(this.Image);
            }
            DrawGameObjectAfter();
        }

        /// <summary>
        /// 画像を矩形の中心に描画します
        /// </summary>
        /// <param name="grHandle"></param>
        /// <returns>失敗したら-1</returns>
        internal int DrawGraph(Image grHandle)
        {
            return DX.DrawRotaGraph((int)this.X, (int)this.Y, 1, this.Rad - grHandle.rad, grHandle.image, DX.TRUE);
        }
        #region 空の関数
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
        /// 攻撃された時に呼び出されるメソッド
        /// 必要に応じてオーバーライドして下さい
        /// </summary>
        /// <param name="power">パワー</param>
        /// <returns>受けたダメージ</returns>
        internal virtual int Suffer(int power)
        {
            return 0;
        }

        /// <summary>
        /// 攻撃が当たった時に呼び出されるメソッド
        /// 必要に応じてオーバーライドして下さい
        /// </summary>
        /// <param name="damage">与えたダメージ</param>
        internal virtual void Attack(int damage)
        {
        }
        #endregion
        #region 移動
        /// <summary>
        /// 移動を行います
        /// </summary>
        /// <param name="v">ベクトル</param>
        internal void Move(Vec v)
        {
            this.X += v.X;
            this.Y += v.Y;
        }

        /// <summary>
        /// X方向に移動します
        /// </summary>
        /// <param name="x">X方向の移動量</param>
        internal void MoveX(double x)
        {
            this.X += x;
        }

        /// <summary>
        /// Y方向に移動します
        /// </summary>
        /// <param name="y"></param>
        internal void MoveY(double y)
        {
            this.Y += y;
        }
        #endregion
        #region ラジアン変換
        /// <summary>
        /// 特定のオブジェクトに対するラジアンを、マップに対するラジアンに変換します
        /// </summary>
        /// <param name="go">オブジェクト</param>
        /// <param name="objRad">オブジェクトに対するラジアン</param>
        /// <returns>マップに対するラジアン</returns>
        public double ToMapRad(double objRad)
        {
            return objRad + this.Rad - this.Image.rad;
        }

        /// <summary>
        /// マップに対するラジアンを、特定のオブジェクトに対するラジアンに変換します
        /// </summary>
        /// <param name="go"></param>
        /// <param name="mapRad"></param>
        /// <returns></returns>
        public double ToObjRad(double mapRad)
        {
            return mapRad - this.Rad + this.Image.rad;
        }
        #endregion
        #endregion
    }
}
