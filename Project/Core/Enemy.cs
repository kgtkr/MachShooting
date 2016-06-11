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
    public class Enemy : GameObject
    {
        #region フィールド
        /// <summary>
        /// 名前
        /// </summary>
        private string name;

        /// <summary>
        /// 最大HP
        /// </summary>
        private int maxHp;

        /// <summary>
        /// HP
        /// </summary>
        private int hp;

        /// <summary>
        /// 自機
        /// </summary>
        private readonly My my;

        /// <summary>
        /// やられ判定があるか
        /// </summary>
        private bool hit;

        /// <summary>
        /// luaオブジェクト
        /// </summary>
        private Lua lua;

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
        /// <summary>
        /// 名前
        /// </summary>
        public string Name
        {
            get { return this.name; }
        }
        #endregion
        #region イントランス作成
        /// <summary>
        /// 新しいイントランスを作成します
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="circle">円</param>
        /// <param name="power">本体の攻撃力</param>
        /// <param name="maxHP">最大HP</param>
        /// <param name="my">自機</param>
        /// <param name="image">画像</param>
        public Enemy(EnemyHeader h,My my)
            : base(new Vec(Game.WINDOW_R, Game.WINDOW_R / 2), 0, new Image(DX.LoadGraph(h.image),h.r, new Vec(0, 1).Rad), new Vec(0, 1).Rad)
        {
            this.name = h.name;
            this.hp = h.hp;
            this.maxHp = h.hp;
            this.my = my;
            this.hit = true;
            this.Draw = false;

            this.lua = new Lua();
            this.lua.DoFile("script/" + h.script + ".lua");

            this.initFunc = lua.GetFunction("init");
            this.updateFunc = lua.GetFunction("update");
            this.drawFunc = lua.GetFunction("draw");
            this.disposeFunc = lua.GetFunction("dispose");

            this.initFunc.Call(new object[] { h, this.Image.image });
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
            if (this.hp != 0)//生きているなら
            {
                List<AttackObject> list = (List<AttackObject>)this.updateFunc.Call(new object[] { this.my.Dot })[0];

                Input();
                return list;
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
            if (this.hp != 0 && this.hit)//生きているかつやられ判定がある
            {
                this.hp -= power;
                if (this.hp <= 0)
                {
                    this.hp = 0;
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

            int w = (int)((double)this.hp / (double)this.maxHp * maxW);

            DX.DrawBox(x, y, x + maxW, y + h, DXColor.Instance.white, DX.TRUE);
            DX.DrawBox(x + 1, y, x + w + 1, y + h, DXColor.Instance.green, DX.TRUE);
        }
        #endregion
    }
}
