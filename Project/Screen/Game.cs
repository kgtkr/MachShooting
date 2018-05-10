using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using System.Diagnostics;

namespace MachShooting
{
    /// <summary>
    /// ゲーム画面
    /// </summary>
    internal class Game : Screen,IDisposable
    {
        #region 定数
        /// <summary>
        /// タイムリミット(F単位)
        /// 15分
        /// </summary>
        internal const int TIME_LIMIT_F = 15 * 60 * 60;

        /// <summary>
        /// 半径
        /// </summary>
        internal const int WINDOW_R = 1000;
        #endregion
        #region フィールド
        /// <summary>
        /// 自機
        /// </summary>
        private readonly Player player;

        /// <summary>
        /// ボス
        /// </summary>
        private readonly Enemy boss;

        /// <summary>
        /// 経過時間
        /// </summary>
        private int timeF;

        /// <summary>
        /// 状態。0:戦闘、1:一時停止、2:リザルト
        /// </summary>
        private int battle;

        /// <summary>
        /// 一時停止のindex(0:再開、1:メニューへ)
        /// </summary>
        private int stopIndex;

        /// <summary>
        /// 自機の攻撃オブジェクト
        /// </summary>
        private readonly List<AttackObject> playerAttack;

        /// <summary>
        /// 敵の攻撃オブジェクト
        /// </summary>
        private readonly List<AttackObject> enemyAttack;

        /// <summary>
        /// 描画可能グラフィック
        /// </summary>
        private readonly int screen;
        #endregion
        #region コンストラクタ
        /// <summary>
        /// 新しいインスタンスを作成します
        /// </summary>
        /// <param name="enemy">敵</param>
        /// <param name="equipment">装備</param>
        internal Game(EnemyHeader enemy, PlayerHeader equipment)
        {
            this.player = new Player(equipment);

            this.boss = new Enemy(enemy, player);

            this.battle = 0;
            this.playerAttack = new List<AttackObject>();
            this.enemyAttack = new List<AttackObject>();
            this.screen = DX.MakeScreen(Game.WINDOW_R * 2, Game.WINDOW_R * 2);
        }
        #endregion
        #region メソッド
        /// <summary>
        /// 描画を行います
        /// </summary>
        internal override void Draw()
        {
            DX.DrawBox(0, 0, Program.WIDTH, Program.HEIGHT, DXColor.Instance.Black, DX.TRUE);
            DX.SetDrawScreen(this.screen);
            DX.DrawBox(0, 0, Game.WINDOW_R * 2, Game.WINDOW_R * 2, DXColor.Instance.Black, DX.TRUE);

            Back.Draw();

            this.boss.DrawObject();

            this.player.DrawObject();

            foreach (AttackObject a in this.enemyAttack)
            {
                a.DrawObject();
            }
            foreach (AttackObject a in this.playerAttack)
            {
                a.DrawObject();
            }

            DX.SetDrawScreen(DX.DX_SCREEN_BACK);
            DX.DrawRotaGraph2(Program.WIDTH / 2, Program.HEIGHT / 2 + Program.HEIGHT / 4, (int)this.player.X, (int)this.player.Y, 1, Math.PI * 2 - (this.player.Rad - this.player.Image.rad), this.screen, DX.FALSE);


            DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA, 150);
            this.boss.DrawHP();
            this.player.DrawCockpit(this.timeF);
            DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 0);

            if (battle == 1)
            {
                //一番外側の枠
                const int SPACE1 = 100;
                //内側の枠
                const int SPACE2 = SPACE1 + 15;
                //幅
                const int WIDTH = Program.WIDTH - SPACE2 * 2;
                //高さ
                const int HEIGHT = Program.HEIGHT - SPACE2 * 2;
                //文字列の右側
                const int X = 15;

                //枠
                DX.DrawBox(SPACE1, SPACE1, Program.WIDTH - SPACE1, Program.HEIGHT - SPACE1, DXColor.Instance.Blue, DX.TRUE);
                DX.DrawBox(SPACE2, SPACE2, Program.WIDTH - SPACE2, Program.HEIGHT - SPACE2, DXColor.Instance.Black, DX.TRUE);

                {
                    uint color = this.stopIndex == 0 ? DXColor.Instance.Red : DXColor.Instance.White;
                    int w = DX.GetDrawStringWidthToHandle("再開", Program.GetStringByte("再開"), Font.Instance.Font64);
                    DX.DrawStringToHandle(WIDTH / 2 - w / 2 + SPACE2, SPACE2 + 30, "再開", color, Font.Instance.Font64);
                }

                {
                    uint color = this.stopIndex == 1 ? DXColor.Instance.Red : DXColor.Instance.White;
                    int w = DX.GetDrawStringWidthToHandle("メニューへ", Program.GetStringByte("メニューへ"), Font.Instance.Font64);
                    DX.DrawStringToHandle(WIDTH / 2 - w / 2 + SPACE2, SPACE2 + 15*3+64, "メニューへ", color, Font.Instance.Font64);
                }
            }
            else if (this.battle == 2)
            {
                //一番外側の枠
                const int SPACE1 = 100;
                //内側の枠
                const int SPACE2 = SPACE1 + 15;
                //幅
                const int WIDTH = Program.WIDTH - SPACE2 * 2;
                //高さ
                const int HEIGHT = Program.HEIGHT - SPACE2 * 2;
                //文字列の右側
                const int X = 15;

                //枠
                DX.DrawBox(SPACE1, SPACE1, Program.WIDTH - SPACE1, Program.HEIGHT - SPACE1, DXColor.Instance.Blue, DX.TRUE);
                DX.DrawBox(SPACE2, SPACE2, Program.WIDTH - SPACE2, Program.HEIGHT - SPACE2, DXColor.Instance.Black, DX.TRUE);

                //敵名
                int w = DX.GetDrawStringWidthToHandle(this.boss.Name, Program.GetStringByte(this.boss.Name), Font.Instance.Font64);
                DX.DrawStringToHandle(WIDTH / 2 - w / 2 + SPACE2, SPACE2 + 15, this.boss.Name, DXColor.Instance.White, Font.Instance.Font64);
                //自機名
                DX.DrawStringToHandle(X + SPACE2, SPACE2 + 64 + 10, this.player.Name, DXColor.Instance.White, Font.Instance.Font32);
                //クリア
                if (!this.boss.Need)
                {
                    //時間
                    int m = this.timeF / 6 / 10 / 60;
                    int s = (this.timeF - m * 6 * 10 * 60) / 6 / 10;
                    int s2 = (this.timeF - m * 6 * 10 * 60 - s * 6 * 10) / 6;
                    DX.DrawStringToHandle(X + SPACE2, SPACE2 + 64 + 10 + 32 + 5, "クリアタイム:" + m + "." + s + "." + s2, DXColor.Instance.White, Font.Instance.Font32);
                }
                //自機が死んだ
                else if (!this.player.Need)
                {
                    DX.DrawStringToHandle(X + SPACE2, SPACE2 + 64 + 10 + 32 + 5, "自機が破壊された…", DXColor.Instance.White, Font.Instance.Font32);
                }
                //タイムアップ
                else
                {
                    DX.DrawStringToHandle(X + SPACE2, SPACE2 + 64 + 10 + 32 + 5, "時間切れです…", DXColor.Instance.White, Font.Instance.Font32);
                }
            }
        }

        /// <summary>
        /// 処理を行います
        /// </summary>
        /// <param name="key"></param>
        /// <param name="key2"></param>
        internal override void Process()
        {
            if (this.battle==0)
            {
                this.timeF++;

                this.playerAttack.AddList(this.player.Process(this.boss));

                /*ボス*/
                this.enemyAttack.AddList(this.boss.Process());
                if (GameObject.Hit(this.player, this.boss))//当たっている
                {
                    this.boss.Attack(player.Suffer(this.boss.Power));
                }
                foreach (AttackObject a in this.playerAttack.ToArray())
                {
                    this.playerAttack.AddList(a.Process());
                    if (GameObject.Hit(this.boss, a))//当たっている
                    {
                        if (this.boss.Need)
                        {
                            a.Attack(this.boss.Suffer(a.Power));
                        }
                    }

                    if (!a.Need)
                    {
                        this.playerAttack.Remove(a);
                    }
                }
                foreach (AttackObject a in this.enemyAttack.ToArray())
                {
                    this.enemyAttack.AddList(a.Process());
                    if (GameObject.Hit(this.player, a))
                    {
                        a.Attack(this.player.Suffer(a.Power));
                    }
                    if (!a.Need)
                    {
                        this.enemyAttack.Remove(a);
                    }
                }

                if(Key.Instance.GetKeyDown(KeyComfig.GAME_STOP))
                {
                    this.battle = 1;
                }

                if (!this.boss.Need) this.battle = 2;
                if (this.timeF == Game.TIME_LIMIT_F) this.battle = 2;
                if (!player.Need)//自機が死んだ
                {
                    this.battle = 2;
                    this.Need = true;
                }
            }
            else if (battle == 1)//一時停止
            {
                if(Key.Instance.GetKeyDown(KeyComfig.MENU_UP) ^
                    Key.Instance.GetKeyDown(KeyComfig.MENU_DOWN))
                {
                    this.stopIndex++;
                    this.stopIndex %= 2;
                }

                if (Key.Instance.GetKeyDown(KeyComfig.MENU_OK))
                {
                    if (this.stopIndex == 0)
                    {
                        this.battle = 0;
                    }
                    else
                    {
                        this.Need = false;
                    }
                }
                else if (Key.Instance.GetKeyDown(KeyComfig.MENU_BACK))
                {
                    this.stopIndex = 0;
                    this.battle = 0;
                }
            }
            else//リザルト
            {
                if (Key.Instance.GetKeyDown(KeyComfig.MENU_OK))
                {
                    this.Need = false;
                }
            }
        }
        #endregion
        public void Dispose()
        {
            DX.DeleteGraph(this.screen);
        }

        ~Game()
        {
            this.Dispose();
        }
    }
}
