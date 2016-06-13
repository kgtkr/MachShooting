using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using MachShooting.Graphic;
using NLua;

namespace MachShooting
{
    /// <summary>
    /// 敵の親クラス
    /// </summary>
    public class Enemy : GameObject, IDisposable
    {
        #region フィールド
        /// <summary>
        /// 名前
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 最大HP
        /// </summary>
        public int MaxHp
        {
            get;
            private set;
        }

        /// <summary>
        /// HP
        /// </summary>
        public int HP
        {
            get;
            private set;
        }

        /// <summary>
        /// 自機
        /// </summary>
        public Player Player
        {
            get;
            private set;
        }

        /// <summary>
        /// やられ判定があるか
        /// </summary>
        public bool Hit
        {
            get;
            private set;
        }

        /// <summary>
        /// luaオブジェクト
        /// </summary>
        private Lua lua;

        /// <summary>
        /// API
        /// </summary>
        private EnemyAPI api;

        /// <summary>
        /// luaオブジェクト
        /// </summary>
        private LuaTable luaObject;

        /// <summary>
        /// 初期化関数
        /// </summary>
        private LuaFunction initFunc;

        /// <summary>
        /// 処理関数
        /// </summary>
        private LuaFunction updateFunc;

        /// <summary>
        /// 描画関数
        /// </summary>
        private LuaFunction drawFunc;

        /// <summary>
        /// 終了関数
        /// </summary>
        private LuaFunction disposeFunc;
        #endregion
        #region プロパティ
        #endregion
        #region イントランス作成
        /// <summary>
        /// 新しいイントランスを作成します
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="circle">円</param>
        /// <param name="power">本体の攻撃力</param>
        /// <param name="maxHP">最大HP</param>
        /// <param name="player">自機</param>
        /// <param name="image">画像</param>
        public Enemy(EnemyHeader h, Player player)
            : base(new Vec(Game.WINDOW_R, Game.WINDOW_R / 2), 0, new Image(DX.LoadGraph(h.image), h.r, new Vec(0, 1).Rad), new Vec(0, 1).Rad)
        {
            this.Name = h.name;
            this.HP = h.hp;
            this.MaxHp = h.hp;
            this.Player = player;
            this.Hit = true;
            this.Draw = false;

            this.api = new EnemyAPI(this);
            this.lua = new Lua();
            this.lua.LoadCLRPackage();
            this.lua.DoString("import (\"DxLibDotNet\",\"DxLibDLL\");");
            this.lua.DoFile(h.script);

            this.initFunc = lua.GetFunction(h.className);
            this.luaObject=(LuaTable) this.initFunc.Call(new object[] {this.api})[0];

            this.updateFunc = (LuaFunction)this.luaObject["update"];
            this.drawFunc = (LuaFunction)this.luaObject["draw"];
            this.disposeFunc = (LuaFunction)this.luaObject["dispose"];
        }
        #endregion
        #region メソッド
        /// <summary>
        /// 処理を行います
        /// </summary>
        /// <returns>アタックオブジェクトリスト。ないならnull</returns>
        public List<AttackObject> Process()
        {
            Next();
            if (this.HP != 0)//生きているなら
            {
                this.updateFunc.Call();

                Input();
                return this.api.getAattackObject();
            }
            else//死んでいるなら
            {
                return null;
            }
        }

        protected override void DrawGameObjectBefore()
        {
            this.drawFunc.Call();
        }

        /// <summary>
        /// 攻撃を受けた時の処理を行います
        /// </summary>
        /// <param name="power">攻撃力</param>
        /// <returns>受けたダメージ</returns>
        public override int Suffer(int power)
        {
            if (this.HP != 0 && this.Hit)//生きているかつやられ判定がある
            {
                this.HP -= power;
                if (this.HP <= 0)
                {
                    this.HP = 0;
                    this.Need = false;
                }
                return power;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// HPを描画します
        /// </summary>
        public void DrawHP()
        {
            const int x = 100;
            const int y = 10;
            const int maxW = Program.WIDTH - 20 - x;
            const int h = 5;

            int w = (int)((double)this.HP / (double)this.MaxHp * maxW);

            DX.DrawBox(x, y, x + maxW, y + h, DXColor.Instance.White, DX.TRUE);
            DX.DrawBox(x + 1, y, x + w + 1, y + h, DXColor.Instance.Green, DX.TRUE);
        }
        #endregion

        public void Dispose()
        {
            DX.DeleteGraph(this.Image.image);
            this.lua.Dispose();
        }

        ~Enemy()
        {
            this.Dispose();
        }
    }


    /// <summary>
    /// 敵のAPIです
    /// </summary>
    public class EnemyAPI
    {
        /// <summary>
        /// 対象の敵
        /// </summary>
        private Enemy enemy;

        /// <summary>
        /// 攻撃オブジェクト
        /// </summary>
        private List<AttackObject> attackObject;

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

        public EnemyAPI(Enemy enemy)
        {
            this.enemy = enemy;
        }

        /// <summary>
        /// 弾を発射します
        /// </summary>
        /// <param name="x">x座標</param>
        /// <param name="y">y座標</param>
        /// <param name="power">攻撃力</param>
        /// <param name="vx">Xベクトル</param>
        /// <param name="vy">Yベクトル</param>
        /// <param name="size">サイズフラグ。0:小、1:中、2:大</param>
        /// <param name="r">赤</param>
        /// <param name="g">緑</param>
        /// <param name="b">青</param>
        /// <param name="call">弾がヒットした時のコールバック
        /// 
        public void ShotBullet(double x,double y,
            int power,
            double vx,double vy,
            int size,
            int r,int g,int b,
            LuaFunction call
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
            }else if (size== 1)
            {
                image = DXImage.Instance.BulletMedium;
            }
            else
            {
                image = DXImage.Instance.BulletBig;
            }

            Action<int> hitCall = null;
            if (call != null)
            {
                hitCall = damage => {
                    call.Call(damage);
                };
            }

            this.attackObject.Add(new Bullet(new Vec(x,y),power,new Vec(vx,vy),image,System.Drawing.Color.FromArgb(0,r,g,b),hitCall));
        }
        
        /// <summary>
        /// X座標
        /// </summary>
        public double X
        {
            get
            {
                return this.enemy.X;
            }

            set
            {
                this.enemy.X = value;
            }
        }

        /// <summary>
        /// Y座標
        /// </summary>
        public double Y
        {
            get
            {
                return this.enemy.Y;
            }

            set
            {
                this.enemy.Y = value;
            }
        }

        /// <summary>
        /// 自機のX座標
        /// </summary>
        public double PlayerX
        {
            get
            {
                return this.enemy.Player.X;
            }
        }

        /// <summary>
        /// 自機のY座標
        /// </summary>
        public double PlayerY
        {
            get
            {
                return this.enemy.Player.Y;
            }
        }

        /// <summary>
        /// 敵の角度
        /// </summary>
        public double Rad
        {
            get
            {
                return this.enemy.Rad;
            }

            set
            {
                this.enemy.Rad=value;
            }
        }

        /// <summary>
        /// このオブジェクトに対するラジアンを、マップに対するラジアンに変換します
        /// </summary>
        /// <param name="rad"></param>
        /// <returns></returns>
        public double ToMapRad(double rad)
        {
            return this.enemy.ToMapRad(rad);
        }

        /// <summary>
        /// マップに対するラジアンをこのオブジェクトに対するラジアンに変換します
        /// </summary>
        /// <param name="rad"></param>
        /// <returns></returns>
        public double ToObjectRad(double rad)
        {
            return this.enemy.ToObjectRad(rad);
        }

        /// <summary>
        /// 自信を描画します
        /// </summary>
        public void Draw()
        {
            this.enemy.DrawGraph(this.enemy.Image);
        }

        /// <summary>
        /// 攻撃力
        /// </summary>
        public int Power
        {
            get { return this.enemy.Power; }
            set { this.enemy.Power = value; }
        }

        /// <summary>
        /// 半径
        /// </summary>
        public double R
        {
            get { return this.enemy.R; }
            set { this.enemy.R = value; }
        }

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
    }
}