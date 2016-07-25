using MachShooting.Graphic;
using NLua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachShooting
{
    public class API
    {
        #region コンストラクタ
        public API(GameObject go)
        {
            this.go = go;
        }
        #endregion
        #region ゲームオブジェクト関係
        protected GameObject go;

        /// <summary>
        /// X座標
        /// </summary>
        public double X
        {
            get
            {
                return this.go.X;
            }

            set
            {
                this.go.X = value;
            }
        }

        /// <summary>
        /// Y座標
        /// </summary>
        public double Y
        {
            get
            {
                return this.go.Y;
            }

            set
            {
                this.go.Y = value;
            }
        }

        /// <summary>
        /// 角度
        /// </summary>
        public double Rad
        {
            get
            {
                return this.go.Rad;
            }

            set
            {
                this.go.Rad = value;
            }

        }

        /// <summary>
        /// 半径
        /// </summary>
        public double R
        {
            get { return this.go.R; }
        }
        #endregion
        #region リソース
        /// <summary>
        /// 画像一覧
        /// </summary>
        public DXImage Image
        {
            get { return DXImage.Instance; }
        }

        /// <summary>
        /// 効果音を再生します
        /// </summary>
        /// <param name="handle"></param>
        public void PlaySE(int handle)
        {
            SE.Instance.Play(handle);
        }

        /// <summary>
        /// オーディオハンドル
        /// </summary>
        public DXAudio Audio
        {
            get { return DXAudio.Instance; }
        }

        public DXColor Color
        {
            get { return DXColor.Instance; }
        }
        #endregion
        #region 数学
        /// <summary>
        /// ルート2
        /// </summary>
        public double Root2
        {
            get { return Program.ROOT2; }
        }

        /// <summary>
        /// このオブジェクトに対するラジアンを、マップに対するラジアンに変換します
        /// </summary>
        /// <param name="rad"></param>
        /// <returns></returns>
        public double ToMapRad(double rad)
        {
            return this.go.ToMapRad(rad);
        }

        /// <summary>
        /// マップに対するラジアンをこのオブジェクトに対するラジアンに変換します
        /// </summary>
        /// <param name="rad"></param>
        /// <returns></returns>
        public double ToObjectRad(double rad)
        {
            return this.go.ToObjectRad(rad);
        }
        #endregion
        #region 攻撃オブジェクト
        /// <summary>
        /// 攻撃オブジェクト
        /// </summary>
        protected List<AttackObject> attackObject;

        /// <summary>
        /// 攻撃オブジェクトを取得します
        /// 攻撃オブジェクトは削除されます
        /// </summary>
        /// <returns></returns>
        internal List<AttackObject> getAattackObject()
        {
            var ao = this.attackObject;
            this.attackObject = null;
            return ao;
        }

        /// <summary>
        /// 弾を発射します
        /// </summary>
        /// <param name="x">x座標</param>
        /// <param name="y">y座標</param>
        /// <param name="power">攻撃力</param>
        /// <param name="vx">Xベクトル</param>
        /// <param name="vy">Yベクトル</param>
        /// <param name="size">サイズフラグ。0:小、1:中、2:大、3:特大</param>
        /// <param name="r">赤</param>
        /// <param name="g">緑</param>
        /// <param name="b">青</param>
        /// <param name="call">弾がヒットした時のコールバック
        /// 
        public void ShotBullet(double x, double y,
            int power,
            double vx, double vy,
            int size,
            int r, int g, int b,
            LuaFunction call = null
            )
        {
            if (this.attackObject == null)
            {
                this.attackObject = new List<AttackObject>();
            }

            Image image;
            if (size == 0)
            {
                image = DXImage.Instance.BulletSmall;
            }
            else if (size == 1)
            {
                image = DXImage.Instance.BulletMedium;
            }
            else if (size == 2)
            {
                image = DXImage.Instance.BulletBig;
            }
            else
            {
                image = DXImage.Instance.Bomb;
            }

            Action<int> hitCall = null;
            if (call != null)
            {
                hitCall = damage =>
                {
                    call.Call(damage);
                };
            }

            this.attackObject.Add(new Bullet(new Vec(x, y), power, new Vec(vx, vy), image, System.Drawing.Color.FromArgb(0, r, g, b), hitCall));
        }
        #endregion
    }

    public class PlayerAPI:API
    {
        public PlayerAPI(Player player):base(player)
        {
        }
    }

    /// <summary>
    /// 敵のAPIです
    /// </summary>
    public class EnemyAPI:API
    {
        private Player player;

        public EnemyAPI(Enemy enemy):base(enemy)
        {
            this.player = enemy.Player;
        }

        

        /// <summary>
        /// 自機のX座標
        /// </summary>
        public double PlayerX
        {
            get
            {
                return this.player.X;
            }
        }

        /// <summary>
        /// 自機のY座標
        /// </summary>
        public double PlayerY
        {
            get
            {
                return this.player.Y;
            }
        }

        /// <summary>
        /// 自信を描画します
        /// </summary>
        public void Draw()
        {
            this.go.DrawGraph(this.go.Image);
        }

        /// <summary>
        /// 攻撃力
        /// </summary>
        public int Power
        {
            get { return this.go.Power; }
            set { this.go.Power = value; }
        }
    }
}
