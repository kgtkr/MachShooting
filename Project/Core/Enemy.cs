/*
メモ:公開メンバ
-プロパティ
X
Y
Rad
R
Power

-getterのみ
PlayerX
PlayerY

-メソッド
DrawEnemy()
AddAO(AttackObject)
Move(Vec)
MoveX(double)
MoveY(double)
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using NLua;

namespace MachShooting
{
    /// <summary>
    /// 敵の親クラス
    /// </summary>
    public class Enemy : GameObject, IDisposable
    {
        #region GameObjectに追加で公開するプロパティ
        public new double X
        {
            get
            {
                return base.X;
            }

            set
            {
                base.X = value;
            }
        }

        public new double Y
        {
            get
            {
                return base.Y;
            }

            set
            {
                base.Y = value;
            }
        }

        public new double Rad
        {
            get
            {
                return base.Rad;
            }

            set
            {
                base.Rad = value;
            }
        }

        public new double R
        {
            get
            {
                return base.R;
            }

            set
            {
                base.R = value;
            }
        }

        public new int Power
        {
            get { return base.Power; }
            set { base.Power = value; }
        }
        #endregion

        /// <summary>
        /// まだ返していないアタックオブジェクト
        /// </summary>
        private List<AttackObject> ao;
        #region フィールド
        /// <summary>
        /// 名前
        /// </summary>
        internal string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 最大HP
        /// </summary>
        internal int MaxHp
        {
            get;
            private set;
        }

        /// <summary>
        /// HP
        /// </summary>
        internal int HP
        {
            get;
            private set;
        }

        /// <summary>
        /// 自機
        /// </summary>
        private Player player;

        /// <summary>
        /// やられ判定があるか
        /// </summary>
        internal bool IsHit
        {
            get;
            private set;
        }

        /// <summary>
        /// luaオブジェクト
        /// </summary>
        private Lua lua;

        /// <summary>
        /// luaオブジェクト
        /// </summary>
        private LuaTable luaObject;

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

        public double PlayerX
        {
            get { return this.player.X; }
        }

        public double PlayerY
        {
            get { return this.player.Y; }
        }
        #endregion
        #region プロパティ
        #endregion
        #region インスタンス作成
        /// <summary>
        /// 新しいインスタンスを作成します
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="circle">円</param>
        /// <param name="power">本体の攻撃力</param>
        /// <param name="maxHP">最大HP</param>
        /// <param name="player">自機</param>
        /// <param name="image">画像</param>
        internal Enemy(EnemyHeader h, Player player)
            : base(new Vec(Game.WINDOW_R, Game.WINDOW_R / 2), 0, new Image(DX.LoadGraph(h.image), h.r, new Vec(0, 1).Rad), new Vec(0, 1).Rad)
        {
            this.Name = h.name;
            this.HP = h.hp;
            this.MaxHp = h.hp;
            this.player = player;
            this.IsHit = true;
            this.Draw = false;

            this.lua = Script.Instance.lua;

            var initFunc = (LuaFunction)((LuaTable)this.lua[h.className])["new"];
            this.luaObject = (LuaTable)initFunc.Call(this)[0];

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
        internal List<AttackObject> Process()
        {
            Next();
            if (this.HP != 0)//生きているなら
            {
                this.updateFunc.Call(this.luaObject);

                Input();

                var ao = this.ao;
                this.ao = null;
                return ao;
            }
            else//死んでいるなら
            {
                return null;
            }
        }

        protected override void DrawGameObjectBefore()
        {
            this.drawFunc.Call(this.luaObject);
        }

        /// <summary>
        /// 攻撃を受けた時の処理を行います
        /// </summary>
        /// <param name="power">攻撃力</param>
        /// <returns>受けたダメージ</returns>
        internal override int Suffer(int power)
        {
            if (this.HP != 0 && this.IsHit)//生きているかつやられ判定がある
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
        internal void DrawHP()
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
            this.disposeFunc.Call();
        }

        ~Enemy()
        {
            this.Dispose();
        }

        public void AddAO(AttackObject ao)
        {
            if (this.ao == null)
            {
                this.ao = new List<AttackObject>();
            }
            this.ao.Add(ao);
        }

        public void DrawEnemy()
        {
            this.DrawGraph(this.Image);
        }

        public new void Move(Vec v)
        {
            base.Move(v);
        }

        public new void MoveX(double x)
        {
            base.MoveX(x);
        }

        public new void MoveY(double y)
        {
            base.MoveY(y);
        }

    }


    
}