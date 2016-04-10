using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircleLib;

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
        #endregion
        #region プロパティ
        #endregion
        #region コンストラクタ
        /// <summary>
        /// イントランスを作成します
        /// </summary>
        /// <param name="circle">円</param>
        /// <param name="power">攻撃力</param>
        /// <param name="speed">スピード</param>
        /// <param name="rad">ラジアン</param>
        /// <param name="image">画像</param>
        /// <param name="meta">メタ情報</param>
        public Bullet(Dot dot, int power, Vec vec, Image image,int[] meta)
            : base(dot,power,image,vec.Rad,meta)
        {
            //角度計算
            SE.Play(MP3.shot);
            this.vec = vec;
            this.Rad = vec.Rad;
        }
        #endregion
        #region メソッド
        /// <summary>
        /// 処理を行います
        /// </summary>
        /// <param name="key">現在のキー情報</param>
        /// <param name="key2">前Fで押されていないキー情報</param>
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
        #endregion
    }
}
