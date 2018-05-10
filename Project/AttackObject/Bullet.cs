using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public Bullet(Vec dot, int power, Vec vec, Image image, Color color,LuaFunction call=null)
            : base(dot, power, image, 0, call)
        {
            this.color = color;
            //角度計算
            SE.Instance.Play(DXAudio.Instance.Shot);
            this.vec = vec;
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
            if (!this.IsIn)
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
