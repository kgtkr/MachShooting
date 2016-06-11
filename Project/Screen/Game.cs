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
    public class Game : Screen
    {
        #region 定数
        /// <summary>
        /// タイムリミット(F単位)
        /// 15分
        /// </summary>
        public const int TIME_LIMIT_F = 15 * 60 * 60;

        /// <summary>
        /// 半径
        /// </summary>
        public const int WINDOW_R = 1000;
        #endregion
        #region フィールド
        /// <summary>
        /// 自機
        /// </summary>
        private readonly My my;

        /// <summary>
        /// 背景
        /// </summary>
        private readonly Back back;

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
        private readonly List<AttackObject> myAttack;

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
        /// 新しいイントランスを作成します
        /// </summary>
        /// <param name="enemy">敵</param>
        /// <param name="equipment">装備</param>
        public Game(EnemyHeader enemy, Equipment equipment)
        {
            switch (equipment)
            {
                case Equipment.STABILITY:
                    this.my = new Stability();
                    break;
                case Equipment.SUPER_CHARGE:
                    this.my = new SuperCharge();
                    break;
                case Equipment.CHARGE:
                    this.my = new Charge();
                    break;
                case Equipment.OVERALL:
                    this.my = new Overall();
                    break;
                case Equipment.DOUBLE:
                    this.my = new Double();
                    break;
            }


            this.back = new Back();

            this.boss = new Enemy(enemy, my);

            this.battle = 0;
            this.myAttack = new List<AttackObject>();
            this.enemyAttack = new List<AttackObject>();
            this.screen = DX.MakeScreen(Game.WINDOW_R * 2, Game.WINDOW_R * 2);
        }
        #endregion
        #region メソッド
        /// <summary>
        /// 描画を行います
        /// </summary>
        public override void Draw()
        {
            DX.DrawBox(0, 0, Program.WIDTH, Program.HEIGHT, DXColor.Instance.black, DX.TRUE);
            DX.SetDrawScreen(this.screen);
            DX.DrawBox(0, 0, Game.WINDOW_R * 2, Game.WINDOW_R * 2, DXColor.Instance.black, DX.TRUE);

            this.back.Draw();

            this.boss.DrawObject();

            this.my.DrawObject();

            foreach (AttackObject a in this.enemyAttack)
            {
                a.DrawObject();
            }
            foreach (AttackObject a in this.myAttack)
            {
                a.DrawObject();
            }

            DX.SetDrawScreen(DX.DX_SCREEN_BACK);
            DX.DrawRotaGraph2(Program.WIDTH / 2, Program.HEIGHT / 2 + Program.HEIGHT / 4, (int)this.my.X, (int)this.my.Y, 1, Math.PI * 2 - (this.my.Rad - this.my.Image.rad), this.screen, DX.FALSE);


            DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA, 150);
            this.boss.DrawHP();
            this.my.DrawCockpit(this.timeF);
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
                DX.DrawBox(SPACE1, SPACE1, Program.WIDTH - SPACE1, Program.HEIGHT - SPACE1, DXColor.Instance.blue, DX.TRUE);
                DX.DrawBox(SPACE2, SPACE2, Program.WIDTH - SPACE2, Program.HEIGHT - SPACE2, DXColor.Instance.black, DX.TRUE);

                {
                    uint color = this.stopIndex == 0 ? DXColor.Instance.red : DXColor.Instance.white;
                    int w = DX.GetDrawStringWidthToHandle("再開", Program.GetStringByte("再開"), Font.Instance.font64);
                    DX.DrawStringToHandle(WIDTH / 2 - w / 2 + SPACE2, SPACE2 + 30, "再開", color, Font.Instance.font64);
                }

                {
                    uint color = this.stopIndex == 1 ? DXColor.Instance.red : DXColor.Instance.white;
                    int w = DX.GetDrawStringWidthToHandle("メニューへ", Program.GetStringByte("メニューへ"), Font.Instance.font64);
                    DX.DrawStringToHandle(WIDTH / 2 - w / 2 + SPACE2, SPACE2 + 15*3+64, "メニューへ", color, Font.Instance.font64);
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
                DX.DrawBox(SPACE1, SPACE1, Program.WIDTH - SPACE1, Program.HEIGHT - SPACE1, DXColor.Instance.blue, DX.TRUE);
                DX.DrawBox(SPACE2, SPACE2, Program.WIDTH - SPACE2, Program.HEIGHT - SPACE2, DXColor.Instance.black, DX.TRUE);

                //敵名
                int w = DX.GetDrawStringWidthToHandle(this.boss.Name, Program.GetStringByte(this.boss.Name), Font.Instance.font64);
                DX.DrawStringToHandle(WIDTH / 2 - w / 2 + SPACE2, SPACE2 + 15, this.boss.Name, DXColor.Instance.white, Font.Instance.font64);
                //自機名
                DX.DrawStringToHandle(X + SPACE2, SPACE2 + 64 + 10, this.my.Name, DXColor.Instance.white, Font.Instance.font32);
                //クリア
                if (!this.boss.Need)
                {
                    //時間
                    int m = this.timeF / 6 / 10 / 60;
                    int s = (this.timeF - m * 6 * 10 * 60) / 6 / 10;
                    int s2 = (this.timeF - m * 6 * 10 * 60 - s * 6 * 10) / 6;
                    DX.DrawStringToHandle(X + SPACE2, SPACE2 + 64 + 10 + 32 + 5, "クリアタイム:" + m + "." + s + "." + s2, DXColor.Instance.white, Font.Instance.font32);
                }
                //自機が死んだ
                else if (!this.my.Need)
                {
                    DX.DrawStringToHandle(X + SPACE2, SPACE2 + 64 + 10 + 32 + 5, "自機が破壊された…", DXColor.Instance.white, Font.Instance.font32);
                }
                //タイムアップ
                else
                {
                    DX.DrawStringToHandle(X + SPACE2, SPACE2 + 64 + 10 + 32 + 5, "時間切れです…", DXColor.Instance.white, Font.Instance.font32);
                }
            }
        }

        /// <summary>
        /// 処理を行います
        /// </summary>
        /// <param name="key"></param>
        /// <param name="key2"></param>
        public override void Process(byte[] key, byte[] key2)
        {
            if (this.battle==0)
            {
                this.timeF++;

                this.back.Process();
                this.myAttack.AddList(this.my.Process(key, key2,this.boss));

                /*ボス*/
                this.enemyAttack.AddList(this.boss.Process());
                if (GameObject.Hit(this.my, this.boss))//当たっている
                {
                    this.boss.Attack(my.Suffer(this.boss.Power));
                }
                foreach (AttackObject a in this.myAttack.ToArray())
                {
                    this.myAttack.AddList(a.Process());
                    if (GameObject.Hit(this.boss, a))//当たっている
                    {
                        if (this.boss.Need)
                        {
                            a.Attack(this.boss.Suffer(a.Power));
                        }
                    }

                    if (!a.Need)
                    {
                        this.my.DisuseAttackObject(a);
                        this.myAttack.Remove(a);
                    }
                }
                foreach (AttackObject a in this.enemyAttack.ToArray())
                {
                    this.enemyAttack.AddList(a.Process());
                    if (GameObject.Hit(this.my, a))
                    {
                        a.Attack(this.my.Suffer(a.Power));
                    }
                    if (!a.Need)
                    {
                        this.enemyAttack.Remove(a);
                    }
                }

                if(key2[Config.Instance.key[KeyComfig.GAME_STOP]] == DX.TRUE)
                {
                    this.battle = 1;
                }

                if (!this.boss.Need) this.battle = 2;
                if (this.timeF == Game.TIME_LIMIT_F) this.battle = 2;
                if (!my.Need)//myが死んだ
                {
                    this.battle = 2;
                    this.Need = true;
                }
            }
            else if (battle == 1)//一時停止
            {
                if(key2[Config.Instance.key[KeyComfig.MENU_UP]]==DX.TRUE^
                    key2[Config.Instance.key[KeyComfig.MENU_DOWN]] == DX.TRUE)
                {
                    this.stopIndex++;
                    this.stopIndex %= 2;
                }

                if (key2[Config.Instance.key[KeyComfig.MENU_OK]] == DX.TRUE)
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
                else if (key2[Config.Instance.key[KeyComfig.MENU_BACK]] == DX.TRUE)
                {
                    this.stopIndex = 0;
                    this.battle = 0;
                }
            }
            else//リザルト
            {
                if (key2[Config.Instance.key[KeyComfig.MENU_OK]] == DX.TRUE)
                {
                    this.Need = false;
                }
            }
        }
        #endregion
    }
}
