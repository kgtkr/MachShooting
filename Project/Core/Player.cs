/*
メモ:公開メンバ
-プロパティ
Action
Speed
DopingTime

-getterのみ
IsDoping
BulletDot
BulletDotR
BulletDotL

-メソッド
AddAO(AttackObject)
SetActionNone()
ResetSpeed()


*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using System.Drawing;
using NLua;

namespace MachShooting
{
    /// <summary>
    /// 自機の底クラス
    /// </summary>
    public class Player : GameObject, IDisposable
    {
        public void SetActionNone()
        {
            this.Action = PlayerAction.NONE;
        }

        public void ResetSpeed()
        {
            this.speed = Player.SPEED;
        }

        #region 定数
        /// <summary>
        /// デフォルトスピード
        /// </summary>
        private const double SPEED = 5;

        /// <summary>
        /// 半径
        /// </summary>
        private const int RADIUS = 15;

        /// <summary>
        /// 最大体力
        /// </summary>
        private const int MAX_HP = 100;
        #endregion
        #region フィールド
        /// <summary>
        /// 名前
        /// </summary>
        private readonly string name;

        private PlayerAction action自クラスからも絶対参照禁止プロパティ使え = PlayerAction.NONE;

        /// <summary>
        /// 現在の行動
        /// </summary>
        public PlayerAction Action {
            get
            {
                return this.action自クラスからも絶対参照禁止プロパティ使え;
            }
            set
            {
                //スピードを元に戻す
                this.speed = Player.SPEED;
                this.action自クラスからも絶対参照禁止プロパティ使え = value;
            }
        }

        /// <summary>
        /// 自己強化の残り時間
        /// </summary>
        public int DopingTime
        {
            get;
            set;
        }

        public bool IsDoping
        {
            get
            {
                return this.DopingTime != 0;
            }
        }

        /// <summary>
        /// まだ返していないアタックオブジェクト
        /// </summary>
        private List<AttackObject> ao = new List<AttackObject>();

        /// <summary>
        /// スピード
        /// </summary>
        private double speed = SPEED;

        /*ステータス*/
        /// <summary>
        /// 最大攻撃必殺技ゲージ
        /// </summary>
        private readonly int maxDG;

        /// <summary>
        /// 最大自己強化必殺技ゲージ
        /// </summary>
        private readonly int maxSG;

        //変数
        /// <summary>
        /// 体力
        /// </summary>
        private int hp = MAX_HP;

        /// <summary>
        /// 攻撃必殺技ゲージ
        /// </summary>
        private int deathblowGauge;

        /// <summary>
        /// 自己強化必殺技ゲージ
        /// </summary>
        private int strengthenGauge;


        /*行動*/
        /// <summary>
        /// 回避
        /// </summary>
        private AvoidanceData avoidance;

        /// <summary>
        /// ダッシュ
        /// </summary>
        private JustDashData justDash;

        /// <summary>
        /// 吹き飛ばし
        /// </summary>
        private CrisisData crisis;

        /// <summary>
        /// やられ判定があるか
        /// </summary>
        private bool hit;

        /// <summary>
        /// 生きているか
        /// </summary>
        private bool life;

        /// <summary>
        /// 自己強化時間の最大
        /// </summary>
        private int maxStrengthenTime;

        #region Lua
        /// <summary>
        /// luaオブジェクト
        /// </summary>
        private Lua lua;

        /// <summary>
        /// luaオブジェクト
        /// </summary>
        private LuaTable luaObject;

        /// <summary>
        /// 描画関数
        /// なくてもいい
        /// </summary>
        private LuaFunction drawFunc;

        /// <summary>
        /// アップデート
        /// </summary>
        private LuaFunction updateFunc;

        /// <summary>
        /// 終了関数
        /// </summary>
        private LuaFunction disposeFunc;

        /// <summary>
        /// 通常
        /// </summary>
        private LuaFunction normalFunc;

        /// <summary>
        /// 特殊
        /// </summary>
        private LuaFunction specialFunc;

        /// <summary>
        /// 必殺
        /// </summary>
        private LuaFunction killerFunc;

        /// <summary>
        /// 自己強化
        /// </summary>
        private LuaFunction dopingFunc;

        /// <summary>
        /// カウンター
        /// </summary>
        private LuaFunction counterFunc;
        #endregion
        #endregion
        #region プロパティ
        /// <summary>
        /// 名前
        /// </summary>
        internal string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// 現在のHP
        /// </summary>
        protected int Hp
        {
            get { return this.hp; }
            set { this.hp = value; }
        }

        /// <summary>
        /// スピード
        /// 毎フレームリセットされます
        /// </summary>
        public double Speed
        {
            get { return this.speed; }
            set { this.speed = value; }
        }

        public Vec BulletDot
        {
            get
            {
                Vec dot = this.Circle.Dot;
                dot += Vec.FromRadLength(this.Rad, this.R);
                return dot;
            }
        }

        public Vec BulletDotR
        {
            get
            {
                Vec dot = this.Circle.Dot;
                dot += Vec.FromRadLength(new Vec(1, -1).Rad + this.Rad - this.Image.rad, this.R * Program.ROOT2);
                return dot;
            }
        }

        public Vec BulletDotL
        {
            get
            {
                Vec dot = this.Circle.Dot;
                dot += Vec.FromRadLength(new Vec(-1, -1).Rad + this.Rad - this.Image.rad, this.R * Program.ROOT2);
                return dot;
            }
        }
        #endregion
        #region コンストラクタ
        internal Player(PlayerHeader h)
            : base(new Vec(Game.WINDOW_R, Game.WINDOW_R), 0, DXImage.Instance.Player, new Vec(0, -1).Rad)
        {
            this.name = h.name;
            this.maxDG = h.dg;
            this.maxSG = h.sg;

            this.lua = Script.Instance.lua;

            var initFunc = (LuaFunction)((LuaTable)lua[h.className])["new"];
            this.luaObject = (LuaTable)initFunc.Call(this)[0];

            this.updateFunc = (LuaFunction)this.luaObject["update"];

            this.drawFunc = (LuaFunction)this.luaObject["draw"];
            this.disposeFunc = (LuaFunction)this.luaObject["dispose"];

            this.normalFunc = (LuaFunction)this.luaObject["normal"];
            this.specialFunc = (LuaFunction)this.luaObject["special"];
            this.killerFunc = (LuaFunction)this.luaObject["killer"];
            this.dopingFunc = (LuaFunction)this.luaObject["doping"];
            this.counterFunc = (LuaFunction)this.luaObject["counter"];

            this.life = true;
        }
        #endregion
        #region メソッド

        /// <summary>
        /// 処理を行います
        /// </summary>
        /// <returns>アタックオブジェクトリスト。ないならnull</returns>
        internal List<AttackObject> Process(Enemy enemy)
        {
            Next();
            this.Draw = this.hit;

            //当たり判定
            this.hit = true;

            //ラジアン
            if (Key.Instance.GetKey(KeyComfig.GAME_CAMERA_ROTATION_LEFT))
            {
                this.Rad -= (1.0).ToRad();
            }
            if (Key.Instance.GetKey(KeyComfig.GAME_CAMERA_ROTATION_RIGHT))
            {
                this.Rad += (1.0).ToRad();
            }
            if (Key.Instance.GetKeyDown(KeyComfig.GAME_TARGET_CAMERA))
            {
                this.Rad = new Vec(enemy.X - this.X, enemy.Y - this.Y).Rad;
            }
            if (Key.Instance.GetKeyDown(KeyComfig.GAME_CAMERA_LEFT))
            {
                this.Rad = new Vec(-1, 0).Rad + this.Rad - this.Image.rad;
            }
            if (Key.Instance.GetKeyDown(KeyComfig.GAME_CAMERA_RIGHT))
            {
                this.Rad = new Vec(1, 0).Rad + this.Rad - this.Image.rad;
            }
            if (Key.Instance.GetKeyDown(KeyComfig.GAME_CAMERA_DOWN))
            {
                this.Rad = new Vec(0, 1).Rad + this.Rad - this.Image.rad;
            }

            /*自己強化を行う*/
            if (this.DopingTime == 0)
            {
                if (Key.Instance.GetKey(KeyComfig.GAME_STRENGTHEN))
                {
                    if (this.strengthenGauge == (int)this.maxSG)
                    {
                        SE.Instance.Play(DXAudio.Instance.Strengthen);
                        this.strengthenGauge = 0;
                        Strengthen_(true);
                        this.maxStrengthenTime = this.DopingTime;
                    }
                }
            }
            if (this.DopingTime != 0)
            {
                Strengthen_(false);
            }

            //何もしてないならキー取得
            if (this.Action == PlayerAction.NONE)
            {
                if (Key.Instance.GetKey(Config.Instance.Key[KeyComfig.GAME_ATTACK]))
                {
                    this.Action = PlayerAction.NORMAL;
                    ConventionalAttack(true);
                }
                else if (Key.Instance.GetKey(Config.Instance.Key[KeyComfig.GAME_SPECIAL]))
                {
                    this.Action = PlayerAction.SPECIAL;
                    SpecialAttack(true);
                }
                else if (Key.Instance.GetKey(Config.Instance.Key[KeyComfig.GAME_AVOIDANCE]))
                {
                    this.Action = PlayerAction.AVOIDANCE;
                    Avoidance(true);
                }
                else if (Key.Instance.GetKey(Config.Instance.Key[KeyComfig.GAME_DEATHBLOW]))
                {
                    if (this.deathblowGauge == (int)this.maxDG)
                    {
                        SE.Instance.Play(DXAudio.Instance.Deathblow);
                        this.deathblowGauge = 0;
                        this.Action = PlayerAction.KILLER;
                        Deathblow(true);
                    }
                }
            }

            //アクションごとに処理分岐
            switch (this.Action)
            {
                case PlayerAction.NORMAL:
                    ConventionalAttack(false);
                    break;
                case PlayerAction.AVOIDANCE:
                    Avoidance(false);
                    break;
                case PlayerAction.COUNTER:
                    CounterAttack(false);
                    break;
                case PlayerAction.DASH:
                    Dash(false);
                    break;
                case PlayerAction.KILLER:
                    Deathblow(false);
                    break;
                case PlayerAction.SPECIAL:
                    SpecialAttack(false);
                    break;
                case PlayerAction.CRISIS:
                    Crisis(false);
                    break;
            }

            //移動処理
            Move();

            Process_Player();

            Input();

            var ao = this.ao;
            this.ao = null;
            return ao;
        }

        /// <summary>
        /// 処理の最後に呼びだされます。必要に応じてオーバーライドして下さい
        /// </summary>
        /// <param name="key">キー1</param>
        /// <param name="key2">キー2</param>
        /// <returns>アタックオブジェクトリスト。ないならnull</returns>
        protected void Process_Player()
        {
            this.updateFunc.Call();
        }

        /// <summary>
        /// 移動処理
        /// </summary>
        /// <param name="key">キー</param>
        private void Move()
        {
            Direction d = Player.getDirection();
            double rad = 0;
            switch (d)
            {
                case Direction.UP_LEFT:
                    rad = Radian.LEFT_UP;
                    break;
                case Direction.UP_RIGHT:
                    rad = Radian.RIGHT_UP;
                    break;
                case Direction.UP:
                    rad = Radian.UP;
                    break;
                case Direction.DOWN_LEFT:
                    rad = Radian.LEFT_DOWN;
                    break;
                case Direction.DOWN_RIGHT:
                    rad = Radian.RIGHT_DOWN;
                    break;
                case Direction.DOWN:
                    rad = Radian.DOWN;
                    break;
                case Direction.LEFT:
                    rad = Radian.LEFT;
                    break;
                case Direction.RIGHT:
                    rad = Radian.RIGHT;
                    break;
            }

            if (d != Direction.NOTE)
            {
                Vec v = Vec.FromRadLength(this.ToMapRad(rad), this.speed);
                this.X += v.X;
                this.Y += v.Y;
            }
        }

        /// <summary>
        /// 方向を取得します
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static Direction getDirection()
        {
            //上下左右の情報取得
            int x = 2;//1:左、3:右
            int y = 2;//1:上、3:下
            if (Key.Instance.GetKey(KeyComfig.GAME_LEFT) ^ Key.Instance.GetKey(KeyComfig.GAME_RIGHT))//左右両方が一遍に押されていない
            {
                if (Key.Instance.GetKey(KeyComfig.GAME_LEFT)) x = 1;//左
                if (Key.Instance.GetKey(KeyComfig.GAME_RIGHT)) x = 3;//右
            }
            if (Key.Instance.GetKey(KeyComfig.GAME_UP) ^ Key.Instance.GetKey(KeyComfig.GAME_DOWN))//上下両方が一遍に押されていない
            {
                if (Key.Instance.GetKey(KeyComfig.GAME_UP)) y = 1;//上
                if (Key.Instance.GetKey(KeyComfig.GAME_DOWN)) y = 3;//下
            }


            if (y == 1)//上
            {
                if (x == 1)//左上
                {
                    return Direction.UP_LEFT;
                }
                else if (x == 3)//右上
                {
                    return Direction.UP_RIGHT;
                }
                else//上
                {
                    return Direction.UP;
                }
            }
            else if (y == 3)//下
            {
                if (x == 1)//左下
                {
                    return Direction.DOWN_LEFT;
                }
                else if (x == 3)//右下
                {
                    return Direction.DOWN_RIGHT;
                }
                else//下
                {
                    return Direction.DOWN;
                }
            }
            else//上下は押されていない
            {
                if (x == 1)//左
                {
                    return Direction.LEFT;
                }
                else if (x == 3)//右
                {
                    return Direction.RIGHT;
                }
                else//なし
                {
                    return Direction.NOTE;
                }
            }
        }

        /// <summary>
        /// 回避を行います
        /// </summary>
        /// <param name="key">キー</param>
        private void Avoidance(bool start)
        {
            if (start)//初めて
            {
                this.avoidance = new AvoidanceData();
                SE.Instance.Play(DXAudio.Instance.Avoidance);
            }
            else
            {
                if (this.avoidance.count < AvoidanceData.INVINCIBLE_TIME)//無敵時間
                {
                    this.hit = false;
                }
                else if (this.avoidance.count == AvoidanceData.TIME)//終わり
                {
                    this.Action = PlayerAction.NONE;
                }
            }
            this.avoidance.count++;
        }

        /// <summary>
        /// 攻撃があたった時の処理を行います
        /// </summary>
        /// <param name="power"></param>
        /// <returns></returns>
        internal override int Suffer(int power)
        {
            if (power == 0) return power;
            if (this.Action == PlayerAction.AVOIDANCE && this.avoidance.count <= AvoidanceData.JUST_TIME)//ジャスト回避中
            {
                this.Action = PlayerAction.DASH;
                Dash(true);
            }
            else if (this.hit)//判定がある
            {
                this.Action = PlayerAction.CRISIS;
                Crisis(true);
                hp -= power;
            }
            if (this.hp <= 0)//死ぬ
            {
                this.hp = 0;
                this.life = false;
                this.Need = false;
            }
            return this.hit ? power : 0;
        }

        /// <summary>
        /// ダッシュを行います
        /// </summary>
        /// <param name="key"></param>
        /// <returns>アタックオブジェクトリスト。ないならnull</returns>
        private void Dash(bool start)
        {
            if (start)
            {
                this.justDash = new JustDashData();
                this.hp += 10;
                SE.Instance.Play(DXAudio.Instance.HP1);
            }
            else
            {
                if (Key.Instance.GetKey(KeyComfig.GAME_ATTACK))//カウンター
                {
                    this.Action = PlayerAction.COUNTER;
                    CounterAttack(true);
                }
                else if (this.justDash.count < JustDashData.TIME)//終わってない
                {
                    this.hit = false;
                    this.speed = JustDashData.SPEED;
                    if (this.hp > Player.MAX_HP) this.hp = Player.MAX_HP;
                }
                else//終わり
                {
                    this.hp += 20;
                    if (this.hp > Player.MAX_HP) this.hp = Player.MAX_HP;
                    this.Action = PlayerAction.NONE;
                    SE.Instance.Play(DXAudio.Instance.HP2);
                }
            }
            this.justDash.count++;
        }

        /// <summary>
        /// コックピットを描画します
        /// <param name="time">経過時間</param>
        /// </summary>
        internal void DrawCockpit(int time)
        {
            {//3ゲージ
                //最大の長さ
                const int MAX_GAUGE = Program.WIDTH / 2 - 20;
                //X座標
                const int GAUGE_X = 10;
                //高さ
                const int GAUGE_H = 5;
                //余白
                const int SPACE = 2;
                //一番上のY座標
                const int GAUGE_Y = Program.HEIGHT - SPACE * 3 - GAUGE_H * 3;


                //各ゲージの長さ
                int hp = (int)((double)this.hp / (double)Player.MAX_HP * MAX_GAUGE);
                int deathblow = (int)((double)this.deathblowGauge / (double)this.maxDG * MAX_GAUGE);
                int strengthen = (int)((double)this.strengthenGauge / (double)this.maxSG * MAX_GAUGE);
                int strengthen2 = this.maxStrengthenTime == 0 ? 0 : (int)((double)this.DopingTime / (double)this.maxStrengthenTime * MAX_GAUGE);

                for (int i = 0; i < 3; i++)
                {
                    int y = GAUGE_Y + GAUGE_H * i + SPACE * i;
                    DX.DrawBox(GAUGE_X, y, GAUGE_X + MAX_GAUGE, y + GAUGE_H, DXColor.Instance.White, DX.TRUE);

                    //中身
                    const int H = GAUGE_H - 2;
                    uint color = 0;
                    int w = 0;
                    switch (i)
                    {
                        case 0:
                            color = DXColor.Instance.Green;
                            w = hp;
                            break;
                        case 1:
                            color = (this.deathblowGauge == (int)this.maxDG && this.Count % 30 < 15) ? DXColor.Instance.White : DXColor.Instance.Red;
                            w = deathblow;
                            break;
                        case 2:
                            if (this.DopingTime == 0)
                            {
                                color = (this.strengthenGauge == (int)this.maxSG && this.Count % 30 < 15) ? DXColor.Instance.White : DXColor.Instance.Yellow;
                                w = strengthen;
                            }
                            else
                            {
                                color = DXColor.Instance.Blue;
                                w = strengthen2;
                            }
                            break;
                    }
                    DX.DrawBox(GAUGE_X, y + 1, GAUGE_X + w, y + 1 + H, color, DX.TRUE);
                }
            }
            {//時間
                DX.DrawCircle(45, 45, 35, DXColor.Instance.White);
                DX.DrawCircle(45, 45, 32, DXColor.Instance.Blue);
                double percent = (double)time / (double)Game.TIME_LIMIT_F * 100.0;
                DX.DrawCircleGauge(45, 45, percent, DXImage.Instance.Clock);
            }
        }

        /// <summary>
        /// 吹き飛ばされ処理を行います
        /// </summary>
        /// <param name="key"></param>
        protected void Crisis(bool start)
        {
            if (start)//初めて
            {
                this.crisis = new CrisisData();
                SE.Instance.Play(DXAudio.Instance.ShotHit);
                this.hit = false;
            }
            else
            {
                if (this.crisis.count < CrisisData.TIME)//終わってない
                {
                    this.hit = false;
                }
                else
                {
                    this.Action = PlayerAction.NONE;
                }
            }

            this.crisis.count++;
        }

        protected override void DrawGameObjectAfter()
        {
            if (!this.hit)
            {
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA, 100);
                this.DrawGraph(this.Image);
            }

            if (this.DopingTime != 0)
            {
                DX.SetDrawBright(255, 255, 0);
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_ADD, 255);
                int size;
                DX.GetGraphSize(DXImage.Instance.Special, out size, out size);
                double ext = (double)(this.R * 4) / size;
                DX.DrawRotaGraph((int)this.X, (int)this.Y, ext, Math.PI / 180 * (360 * 5 - (this.Count % 360) * 5), DXImage.Instance.Special, DX.TRUE);
                DX.SetDrawBright(255, 255, 255);
            }

            if (this.Action == PlayerAction.KILLER)
            {
                DX.SetDrawBright(255, 0, 0);
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_ADD, 255);
                int size;
                DX.GetGraphSize(DXImage.Instance.Special, out size, out size);
                double ext = (double)(this.R * 8) / size;
                DX.DrawRotaGraph((int)this.X, (int)this.Y, ext, Math.PI / 180 * (360 * 10 - (this.Count % 360) * 10), DXImage.Instance.Special, DX.TRUE);
                DX.SetDrawBright(255, 255, 255);
            }
            if (this.Action == PlayerAction.DASH)
            {
                DX.SetDrawBright(0, 0, 255);
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_ADD, 255);
                int size;
                DX.GetGraphSize(DXImage.Instance.Special, out size, out size);
                double ext = (double)(this.R * 4) / size;
                DX.DrawRotaGraph((int)this.X, (int)this.Y, ext, Math.PI / 180 * (360 * 10 - (this.Count % 360) * 10), DXImage.Instance.Special, DX.TRUE);
                DX.SetDrawBright(255, 255, 255);
            }
            DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 0);

            this.drawFunc?.Call(this.luaObject);
        }
        #endregion
        #region 実装メソッド
        #endregion
        #region 未実装メソッド
        /// <summary>
        /// 通常攻撃
        /// </summary>
        /// <returns>アタックオブジェクトリスト。ないならnull</returns>
        private void ConventionalAttack(bool start)
        {
            this.normalFunc.Call(this.luaObject, start);
        }

        /// <summary>
        /// 特殊攻撃
        /// </summary>
        /// <returns>アタックオブジェクトリスト。ないならnull</returns>
        private void SpecialAttack(bool start)
        {
            this.specialFunc.Call(this.luaObject, start);
        }

        /// <summary>
        /// 必殺技(攻撃)
        /// </summary>
        /// <returns>アタックオブジェクトリスト。ないならnull</returns>
        private void Deathblow(bool start)
        {
            this.killerFunc.Call(this.luaObject, start);
        }

        /// <summary>
        /// 必殺技(自己強化)
        /// </summary>
        private void Strengthen_(bool start)
        {
            this.dopingFunc.Call(this.luaObject, start);
        }

        /// <summary>
        /// カウンター攻撃
        /// </summary>
        /// <returns>アタックオブジェクトリスト。ないならnull</returns>
        private void CounterAttack(bool start)
        {
            this.counterFunc.Call(this.luaObject, start);
        }
        #endregion
        public void Dispose()
        {
            this.disposeFunc.Call(this.luaObject);
        }

        ~Player()
        {
            this.Dispose();
        }

        public void AddAO(AttackObject ao)
        {
            //ヒット時のコールバック
            bool isDeathblow = this.Action == PlayerAction.KILLER;
            bool isStrengthen = this.DopingTime != 0;
            var call = ao.call;
            ao.call=damage =>
            {
                if (!isDeathblow)
                {
                    this.deathblowGauge += damage;
                    if (this.deathblowGauge > (int)this.maxDG) this.deathblowGauge = (int)this.maxDG;

                }

                if (!isStrengthen)
                {
                    this.strengthenGauge += damage;
                    if (this.strengthenGauge > (int)this.maxSG) this.strengthenGauge = (int)this.maxSG;
                }

                call(damage);

            };

            //追加
            if (this.ao == null)
            {
                this.ao = new List<AttackObject>();
            }
            this.ao.Add(ao);
        }
    }
    #region その他
    #region 列挙
    #region MyAction
    /// <summary>
    /// 行動
    /// </summary>
    public enum PlayerAction
    {
        /// <summary>
        /// なし
        /// </summary>
        NONE,
        /// <summary>
        /// 通常攻撃
        /// </summary>
        NORMAL,
        /// <summary>
        /// 特殊攻撃
        /// </summary>
        SPECIAL,
        /// <summary>
        /// 回避
        /// </summary>
        AVOIDANCE,
        /// <summary>
        /// 攻撃系必殺技
        /// </summary>
        KILLER,
        /// <summary>
        /// ジャスト回避ダッシュ
        /// </summary>
        DASH,
        /// <summary>
        /// ジャスト回避カウンター
        /// </summary>
        COUNTER,
        /// <summary>
        /// 攻撃にあたった
        /// </summary>
        CRISIS
    }
    #endregion
    #region Direction
    /// <summary>
    /// 方向
    /// </summary>
    internal enum Direction
    {
        /// <summary>
        /// 動かない
        /// </summary>
        NOTE,
        /// <summary>
        /// 上
        /// </summary>
        UP,
        /// <summary>
        /// 右上
        /// </summary>
        UP_RIGHT,
        /// <summary>
        /// 右
        /// </summary>
        RIGHT,
        /// <summary>
        /// 右下
        /// </summary>
        DOWN_RIGHT,
        /// <summary>
        /// 下
        /// </summary>
        DOWN,
        /// <summary>
        /// 左下
        /// </summary>
        DOWN_LEFT,
        /// <summary>
        /// 左
        /// </summary>
        LEFT,
        /// <summary>
        /// 左上
        /// </summary>
        UP_LEFT
    }
    #endregion
    #endregion
    #region 構造体
    #region AvoidanceData
    /// <summary>
    /// 回避
    /// </summary>
    internal struct AvoidanceData
    {
        /// <summary>
        /// 時間
        /// </summary>
        internal const int TIME = 50;

        /// <summary>
        /// 無敵時間
        /// </summary>
        internal const int INVINCIBLE_TIME = 30;

        /// <summary>
        /// ジャスト判定の時間
        /// </summary>
        internal const int JUST_TIME = 9;

        /// <summary>
        /// カウント
        /// </summary>
        internal int count;
    }
    #endregion
    #region JustDashData
    /// <summary>
    /// ダッシュ
    /// </summary>
    internal struct JustDashData
    {
        /// <summary>
        /// 時間
        /// </summary>
        internal const int TIME = 120;

        /// <summary>
        /// スピード
        /// </summary>
        internal const int SPEED = 10;

        /// <summary>
        /// カウント
        /// </summary>
        internal int count;
    }
    #endregion
    #region CrisisData
    /// <summary>
    /// 吹っ飛び
    /// </summary>
    internal struct CrisisData
    {
        /// <summary>
        /// 時間
        /// </summary>
        internal const int TIME = 120;

        /// <summary>
        /// カウント
        /// </summary>
        internal int count;
    }
    #endregion
    #endregion
    #endregion


}