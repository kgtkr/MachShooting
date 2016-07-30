using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachShooting.Graphic;
using System.Drawing;
using DxLibDLL;
using NLua;

namespace MachShooting
{
    /// <summary>
    /// 弾
    /// </summary>
    public class Bullet : AttackObject
    {
        #region フィールド
        /// <summary>
        /// 1Fで動く座標
        /// </summary>
        private readonly Vec vec;

        private readonly Color color;
        #endregion
        #region プロパティ
        #endregion
        #region コンストラクタ
        /// <summary>
        /// インスタンスを作成します
        /// </summary>
        /// <param name="circle">円</param>
        /// <param name="power">攻撃力</param>
        /// <param name="speed">スピード</param>
        /// <param name="rad">ラジアン</param>
        /// <param name="image">画像</param>
        /// <param name="meta">メタ情報</param>
        public Bullet(LuaTable dot, int power, LuaTable vec, Image image, LuaTable color,LuaFunction call)
            : base(new Vec((double)dot["x"],(double)dot["y"]), power, image, 0, call)
        {
            this.color = Color.FromArgb((byte)color["r"], (byte)color["g"], (byte)color["b"]);
            //角度計算
            SE.Instance.Play(DXAudio.Instance.Shot);
            this.vec = new Vec((double)vec["x"],(double)vec["y"]);
            this.Rad = this.vec.Rad;
        }
        #endregion
        #region メソッド
        /// <summary>
        /// 処理を行います
        /// </summary>
        protected override List<AttackObject> ProcessAttackObject()
        {
            //移動
            this.X += this.vec.X;
            this.Y += this.vec.Y;
            //もし外にいるなら
            if (!this.In)
            {
                this.Life = false;
                this.Need = false;
            }
            return null;
        }

        protected override void DrawGameObjectAfter()
        {
            DX.SetDrawBright(this.color.R, this.color.G, this.color.B);
            base.DrawGameObjectAfter();
            DX.SetDrawBright(255, 255, 255);

        }
        #endregion
    }
}
