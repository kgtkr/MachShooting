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
    /// 敵の親クラス
    /// </summary>
    public abstract class Enemy : GameObject
    {
        #region フィールド
        /// <summary>
        /// 名前
        /// </summary>
        private readonly string name;

        /// <summary>
        /// 最大HP
        /// </summary>
        private readonly int maxHp;

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
        #endregion
        #region プロパティ
        /// <summary>
        /// 名前
        /// </summary>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// 自機
        /// </summary>
        protected My My
        {
            get { return this.my; }
        }

        /// <summary>
        /// HP
        /// </summary>
        protected int Hp
        {
            get { return this.hp; }
        }

        /// <summary>
        /// 最大HP
        /// </summary>
        protected int MaxHp
        {
            get { return this.maxHp; }
        }

        /// <summary>
        /// やられ判定
        /// </summary>
        public bool Hit
        {
            get { return this.hit; }
            protected set { this.hit = value; }
        }

        #endregion
        #region コンストラクタ
        /// <summary>
        /// 新しいイントランスを作成します
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="circle">円</param>
        /// <param name="power">本体の攻撃力</param>
        /// <param name="maxHP">最大HP</param>
        /// <param name="my">自機</param>
        /// <param name="image">画像</param>
        public Enemy(string name, int power,  int maxHP,  My my,Image image)
            : base(new Dot(Game.WINDOW_R,Game.WINDOW_R/2), power, image, new Vec(0, 1).Rad)
        {
            this.name = name;
            this.hp = maxHP;
            this.maxHp = maxHP;
            this.my = my;
            this.hit = true;
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
            if (this.hp!=0)//生きているなら
            {
                List<AttackObject> list= Process_Enemy();
                Input();
                return list;
            }
            else//死んでいるなら
            {
                return null;
            }
        }

        /// <summary>
        /// 攻撃を受けた時の処理を行います
        /// </summary>
        /// <param name="power">攻撃力</param>
        /// <returns>受けたダメージ</returns>
        public override int Suffer(int power)
        {
            if (this.hp!=0 && this.Hit)//生きているかつやられ判定がある
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

            DX.DrawBox(x, y, x + maxW, y + h, Program.white, DX.TRUE);
            DX.DrawBox(x + 1, y, x + w + 1, y + h, Program.green, DX.TRUE);
        }
        #endregion
        #region 抽象メソッド
        /// <summary>
        /// 処理を行います
        /// </summary>
        /// <returns>攻撃オブジェクト。ないならnull</returns>
        protected abstract List<AttackObject> Process_Enemy();
        #endregion
    }
}
