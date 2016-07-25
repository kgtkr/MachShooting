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
            this.lua = Script.Instance.lua;

            this.initFunc = (LuaFunction)((LuaTable)lua[h.className])["new"];
            this.luaObject = (LuaTable)this.initFunc.Call(this.api)[0];

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
                this.updateFunc.Call(this.luaObject);

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
            this.drawFunc.Call(this.luaObject);
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
            this.disposeFunc.Call();
        }

        ~Enemy()
        {
            this.Dispose();
        }
    }


    
}