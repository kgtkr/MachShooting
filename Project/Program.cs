using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using System.IO;
using System.Diagnostics;
using MachShooting.Graphic;

namespace MachShooting
{
    public static class Program
    {
        /// <summary>
        /// カウント
        /// </summary>
        private static int count;

        #region 定数
        /// <summary>
        /// バージョン
        /// </summary>
        public const string VER = "0.0.0";

        /// <summary>
        /// 画面の幅
        /// </summary>
        public const int WIDTH = 640;
        /// <summary>
        /// 画面の高さ
        /// </summary>
        public const int HEIGHT = 480;
        /// <summary>
        /// ルート2
        /// </summary>
        public const double ROOT2 = 1.41421356;
        #endregion
        #region 変数
        /// <summary>
        /// FPS
        /// </summary>
        private static readonly Fps fps = new Fps(50);
        #region 各画面
        /// <summary>
        /// タイトル
        /// </summary>
        private static Title title;

        /// <summary>
        /// ミッション選択画面
        /// </summary>
        private static MissionMenu missionMenu;

        /// <summary>
        /// 装備選択画面
        /// </summary>
        private static EquipmentMenu equipmentMenu;

        /// <summary>
        /// ゲーム画面
        /// </summary>
        private static Game game;
        #endregion

        #region 画像
        /// <summary>
        /// 背景画像
        /// </summary>
        public static int back;

        /// <summary>
        /// 自機
        /// </summary>
        public static Image my;

        /// <summary>
        /// 時計
        /// </summary>
        public static int clock;
        #region 弾
        /// <summary>
        /// 超巨大弾
        /// </summary>
        public static Image bomb;

        /// <summary>
        /// 小弾
        /// </summary>
        public static readonly Image[] bulletSmall = new Image[10];

        /// <summary>
        /// 中弾
        /// </summary>
        public static readonly Image[] bulletMedium = new Image[10];

        /// <summary>
        /// 大弾
        /// </summary>
        public static readonly Image[] bulletBig = new Image[10];
        #endregion
        #region 敵
        /// <summary>
        /// ボア
        /// </summary>
        public static Image boar;

        /// <summary>
        /// ガトー
        /// </summary>
        public static Image gato;

        /// <summary>
        /// ラパン
        /// </summary>
        public static Image lapin;

        /// <summary>
        /// ネガリャー　
        /// </summary>
        public static Image nigalya;

        /// <summary>
        /// スネーク
        /// </summary>
        public static Image snake;

        /// <summary>
        /// レオーネ
        /// </summary>
        public static Image leone;
        #endregion
        #region エフェクト
        /// <summary>
        /// ヒット
        /// </summary>
        public static int hit;

        /// <summary>
        /// チャージ
        /// </summary>
        public static int charge;

        /// <summary>
        /// 特殊効果
        /// </summary>
        public static int special;
        #endregion
        #endregion
        #region フォント
        /// <summary>
        /// 8px
        /// </summary>
        public static int font8;

        /// <summary>
        /// 16px
        /// </summary>
        public static int font16;

        /// <summary>
        /// 32px
        /// </summary>
        public static int font32;

        /// <summary>
        /// 64px
        /// </summary>
        public static int font64;
        #endregion
        #region 色
        /// <summary>
        /// 赤
        /// </summary>
        public static uint red;

        /// <summary>
        /// 緑
        /// </summary>
        public static uint green;

        /// <summary>
        /// 青
        /// </summary>
        public static uint blue;

        /// <summary>
        /// 白
        /// </summary>
        public static uint white;

        /// <summary>
        /// 黒
        /// </summary>
        public static uint black;

        /// <summary>
        /// 黄
        /// </summary>
        public static uint yellow;

        /// <summary>
        /// シアン
        /// </summary>
        public static uint cyan;

        /// <summary>
        /// マゼンタ
        /// </summary>
        public static uint magenta;
        #endregion

        /*選択情報*/
        private static int missionData;
        private static Equipment equipment;

        #endregion
        /// <summary>
        /// メインメソッド
        /// </summary>
        public static void Main()
        {
            /*前処理*/
            DX.SetOutApplicationLogValidFlag(DX.FALSE);//ログを出力しない
            DX.SetMainWindowText("MachShooting");//タイトル設定
            DX.ChangeWindowMode(Config.Instance.full ? DX.FALSE : DX.TRUE);//ウィンドウモード

            /*DXライブラリの読み込み*/
            if (DX.DxLib_Init() == -1)
            {
                DX.DxLib_End();
                Environment.Exit(0);
            }

            //ファイルの読み込み
            LoadFile();

            /*ライブラリの設定*/
            DX.SetDrawScreen(DX.DX_SCREEN_BACK);//裏画面に描画する設定

            title = new Title();
            //前フレームのキーを作成
            byte[] lastKey = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                lastKey[i] = DX.FALSE;
            }
            while (true)
            {
                int startTime = DX.GetNowCount();//フレームの始まった時間を取得
                fps.Update();
                string fpsString = "FPS:" + fps.FPS.ToString("#0.0");//FPS取得
                uint fpsColor = fps.FPS > 55 ? Program.white : Program.red;//FPSが55以下なら赤で描画

                if (count % Config.Instance.fps == 0)
                {
                    if (DX.ClearDrawScreen() != 0)//画面消去。×が押されたら終了
                    {
                        DX.DxLib_End();
                        Environment.Exit(0);
                    }
                }

                SE.Instance.Update();

                //キー情報取得
                byte[] key = new byte[256];
                DX.GetHitKeyStateAll(out key[0]);
                //前フレームで押されていないかつキーが押されているか調べる
                byte[] key2 = new byte[256];
                for (int i = 0; i < 256; i++)
                {
                    key2[i] = (byte)(key[i] == DX.TRUE && lastKey[i] == DX.FALSE ? DX.TRUE : DX.FALSE);
                }
                lastKey = key;

                Update(key, key2);
                if (count % Config.Instance.fps == 0) DX.DrawStringToHandle(0, 0, fpsString, fpsColor, Program.font16);


                if (count % Config.Instance.fps == 0) DX.ScreenFlip();//裏画面を表画面に表示
                if (DX.ProcessMessage() != 0)
                {
                    DX.DxLib_End();
                    Environment.Exit(0);
                }

                while (DX.GetNowCount() - startTime < 17) ;   //1周の処理が17ミリ秒になるまで待つ
            }
        }

        /// <summary>
        /// 必要なファイル(画像、音楽、ミッションデータ等)を読み込みます
        /// </summary>
        private static void LoadFile()
        {
            /*=画像=*/
            Program.back = DX.LoadGraph("Data/Image/back.png");

            Program.my = new Image(DX.LoadGraph("Data/Image/My/1.png"), 15, new Vec(0, -1).Rad);

            Program.clock = DX.LoadGraph("Data/Image/Clock.png");

            Program.bomb = new Image(DX.LoadGraph("Data/Image/Bullet/bomb.png"), 140, Math.PI);
            for (int i = 0; i < 10; i++) Program.bulletSmall[i] = new Image(DX.LoadGraph("Data/Image/Bullet/Small/" + (i + 1) + ".png"), 4, new Vec(0, -1).Rad);
            for (int i = 0; i < 10; i++) Program.bulletMedium[i] = new Image(DX.LoadGraph("Data/Image/Bullet/Medium/" + (i + 1) + ".png"), 8, new Vec(0, -1).Rad);
            for (int i = 0; i < 10; i++) Program.bulletBig[i] = new Image(DX.LoadGraph("Data/Image/Bullet/Big/" + (i + 1) + ".png"), 14, new Vec(0, -1).Rad);

            /*=敵=*/
            Program.boar = new Image(DX.LoadGraph("Data/Image/Enemy/boar.png"), 15, new Vec(0, 1).Rad);
            Program.gato = new Image(DX.LoadGraph("Data/Image/Enemy/gato.png"), 15, new Vec(0, 1).Rad);
            Program.lapin = new Image(DX.LoadGraph("Data/Image/Enemy/lapin.png"), 15, new Vec(0, 1).Rad);
            Program.nigalya = new Image(DX.LoadGraph("Data/Image/Enemy/nigalya.png"), 20, new Vec(0, 1).Rad);
            Program.snake = new Image(DX.LoadGraph("Data/Image/Enemy/snake.png"), 15, new Vec(0, 1).Rad);
            Program.leone = new Image(DX.LoadGraph("Data/Image/Enemy/leone.png"), 50, new Vec(0, 1).Rad);


            //エフェクト
            Program.hit = DX.LoadGraph("Data/Image/Effect/Hit.png");
            Program.special = DX.LoadGraph("Data/Image/Effect/Special.png");
            Program.charge = DX.LoadGraph("Data/Image/Effect/Charge.png");

            /*=フォント=*/
            Program.font8 = DX.LoadFontDataToHandle("Data/Font/8.dft");
            Program.font16 = DX.LoadFontDataToHandle("Data/Font/16.dft");
            Program.font32 = DX.LoadFontDataToHandle("Data/Font/32.dft");
            Program.font64 = DX.LoadFontDataToHandle("Data/Font/64.dft");

            /*=色=*/
            Program.red = DX.GetColor(255, 0, 0);
            Program.green = DX.GetColor(0, 255, 0);
            Program.blue = DX.GetColor(0, 0, 255);
            Program.white = DX.GetColor(255, 255, 255);
            Program.black = DX.GetColor(0, 0, 0);
            Program.yellow = DX.GetColor(255, 255, 0);
            Program.cyan = DX.GetColor(0, 255, 255);
            Program.magenta = DX.GetColor(255, 0, 255);


        }

        /// <summary>
        /// タイトル画面、ゲーム画面等を場合に応じて呼び出します
        /// </summary>
        private static void Update(byte[] key, byte[] key2)
        {

            if (Program.title != null)//タイトル画面なら
            {
                Program.title.Process(key, key2);
                if (count % Config.Instance.fps == 0) Program.title.Draw();
                if (!Program.title.Need)
                {
                    Program.title = null;
                    Program.equipmentMenu = new EquipmentMenu();
                }
            }
            else if (Program.equipmentMenu != null)//装備選択画面なら
            {
                Program.equipmentMenu.Process(key, key2);
                if (count % Config.Instance.fps == 0) Program.equipmentMenu.Draw();
                if (!Program.equipmentMenu.Need)
                {
                    Program.equipment = Program.equipmentMenu.Equipment;
                    Program.equipmentMenu = null;
                    Program.missionMenu = new MissionMenu();
                }
            }
            else if (Program.missionMenu != null)//敵選択画面なら
            {
                Program.missionMenu.Process(key, key2);
                if (count % Config.Instance.fps == 0) Program.missionMenu.Draw();
                if (!Program.missionMenu.Need)
                {
                    if (Program.missionMenu.Decision)
                    {
                        Program.missionData = Program.missionMenu.MissionData;//ミッションデータ取得
                        Program.missionMenu = null;
                        Program.game = new Game(Program.missionData, Program.equipment);
                    }
                    else
                    {
                        Program.missionMenu = null;
                        Program.equipmentMenu = new EquipmentMenu();
                    }
                }
            }
            else//ゲーム画面なら
            {
                Program.game.Process(key, key2);
                if (count % Config.Instance.fps == 0) Program.game.Draw();
                if (!Program.game.Need)
                {
                    DX.SetBackgroundColor(0, 0, 0);
                    Program.game = null;
                    Program.equipmentMenu = new EquipmentMenu();
                }
            }
            count++;
        }

        /// <summary>
        /// 文字列を半角文字1byte、全角文字2byteとしてバイト数を取得します
        /// </summary>
        /// <param name="str">文字列</param>
        /// <returns>バイト数</returns>
        public static int GetStringByte(this string str)
        {
            return Encoding.GetEncoding("Shift_JIS").GetByteCount(str);
        }

        /// <summary>
        /// List1にList2を追加します
        /// List2がnullなら追加しません
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        public static void AddList<T>(this List<T> list1, List<T> list2)
        {
            if (list2 != null)
            {
                list1.AddRange(list2);
            }
        }

        /// <summary>
        /// リストが必要かを調べます
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool ITransferListNeed(this List<ITransfer> list)
        {
            if (list == null) return false;
            if (list.Count == 0) return false;

            foreach (ITransfer t in list)
            {
                if (t.Need)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
