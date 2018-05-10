/*
メモ:公開されているクラス
DX
Key
KeyComfig
DXColor
DXImage(弾・エフェクトのみ)
SE
Image
Radian
Vec
Circle
Util
Bullet(生成のみ)
PlayerAction

以下のクラスは複雑なため各クラスのコメントに詳細あり
GameObject
Player
Enemy
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using System.IO;
using System.Diagnostics;
using NLua;

namespace MachShooting
{
    internal static class Program
    {
        /// <summary>
        /// カウント
        /// </summary>
        private static int count;

        #region 定数
        /// <summary>
        /// バージョン
        /// </summary>
        internal const string VER = "0.0.0";

        /// <summary>
        /// 画面の幅
        /// </summary>
        internal const int WIDTH = 640;
        /// <summary>
        /// 画面の高さ
        /// </summary>
        internal const int HEIGHT = 480;
        /// <summary>
        /// ルート2
        /// </summary>
        internal const double ROOT2 = 1.41421356;
        #endregion
        #region 各画面
        /// <summary>
        /// タイトル
        /// </summary>
        private static Title title;

        /// <summary>
        /// ミッション選択画面
        /// </summary>
        private static TreeMenu<EnemyHeader> missionMenu;

        /// <summary>
        /// 装備選択画面
        /// </summary>
        private static TreeMenu<PlayerHeader> equipmentMenu;

        /// <summary>
        /// ゲーム画面
        /// </summary>
        private static Game game;
        #endregion
        private const string ZIKI_MSG = "自機を選択して下さい。";
        /*選択情報*/
        /// <summary>
        /// 選択しているクエスト
        /// </summary>
        private static EnemyHeader missionData;

        /// <summary>
        /// 選択している装備
        /// </summary>
        private static PlayerHeader equipment;
        /// <summary>
        /// メインメソッド
        /// </summary>
        internal static void Main()
        {
            /*前処理*/
            DX.SetOutApplicationLogValidFlag(DX.FALSE);//ログを出力しない
            DX.SetMainWindowText("MachShooting");//タイトル設定
            DX.ChangeWindowMode(Config.Instance.Full ? DX.FALSE : DX.TRUE);//ウィンドウモード

            /*DXライブラリの読み込み*/
            if (DX.DxLib_Init() == -1)
            {
                DX.DxLib_End();
                Environment.Exit(0);
            }

            /*ライブラリの設定*/
            DX.SetDrawScreen(DX.DX_SCREEN_BACK);//裏画面に描画する設定
            DX.SetDrawMode(DX.DX_DRAWMODE_BILINEAR);

            title = new Title();
            while (true)
            {
                int startTime = DX.GetNowCount();//フレームの始まった時間を取得
                Fps.Instance.Update();
                string fpsString = "FPS:" + Fps.Instance.FPS.ToString("#0.0");//FPS取得
                uint fpsColor = Fps.Instance.FPS > 55 ? DXColor.Instance.White : DXColor.Instance.Red;//FPSが55以下なら赤で描画

                if (count % Config.Instance.FrameSkip == 0)
                {
                    if (DX.ClearDrawScreen() != 0)//画面消去。×が押されたら終了
                    {
                        DX.DxLib_End();
                        Environment.Exit(0);
                    }
                }

                SE.Instance.Update();

                Key.Instance.Update();

                Update();
                if (count % Config.Instance.FrameSkip == 0) DX.DrawStringToHandle(0, 0, fpsString, fpsColor, Font.Instance.Font16);


                if (count % Config.Instance.FrameSkip == 0) DX.ScreenFlip();//裏画面を表画面に表示
                if (DX.ProcessMessage() != 0)
                {
                    DX.DxLib_End();
                    Environment.Exit(0);
                }

                while (DX.GetNowCount() - startTime < 17) ;   //1周の処理が17ミリ秒になるまで待つ
            }
        }

        /// <summary>
        /// タイトル画面、ゲーム画面等を場合に応じて呼び出します
        /// </summary>
        private static void Update()
        {

            if (Program.title != null)//タイトル画面なら
            {
                Program.title.Process();
                if (count % Config.Instance.FrameSkip == 0) Program.title.Draw();
                if (!Program.title.Need)
                {
                    Program.title = null;
                    Program.equipmentMenu = new TreeMenu<PlayerHeader>(Program.ZIKI_MSG,Script.PlayerH);
                }
            }
            else if (Program.equipmentMenu != null)//装備選択画面なら
            {
                Program.equipmentMenu.Process();
                if (count % Config.Instance.FrameSkip == 0) Program.equipmentMenu.Draw();
                if (!Program.equipmentMenu.Need)
                {
                    Program.equipment = Program.equipmentMenu.Header;
                    Program.equipmentMenu = null;
                    Program.missionMenu = new TreeMenu<EnemyHeader>("敵を選択して下さい。",Script.EnemyH);
                }
            }
            else if (Program.missionMenu != null)//敵選択画面なら
            {
                Program.missionMenu.Process();
                if (count % Config.Instance.FrameSkip == 0) Program.missionMenu.Draw();
                if (!Program.missionMenu.Need)
                {
                    if (Program.missionMenu.Decision)
                    {
                        Program.missionData = Program.missionMenu.Header;//ミッションデータ取得
                        Program.missionMenu = null;
                        Program.game = new Game(Program.missionData, Program.equipment);
                    }
                    else
                    {
                        Program.missionMenu = null;
                        Program.equipmentMenu = new TreeMenu<PlayerHeader>(Program.ZIKI_MSG, Script.PlayerH);
                    }
                }
            }
            else//ゲーム画面なら
            {
                Program.game.Process();
                if (count % Config.Instance.FrameSkip == 0) Program.game.Draw();
                if (!Program.game.Need)
                {
                    DX.SetBackgroundColor(0, 0, 0);
                    Program.game = null;
                    Program.equipmentMenu = new TreeMenu<PlayerHeader>(Program.ZIKI_MSG, Script.PlayerH);
                }
            }
            count++;
        }

        /// <summary>
        /// 文字列を半角文字1byte、全角文字2byteとしてバイト数を取得します
        /// </summary>
        /// <param name="str">文字列</param>
        /// <returns>バイト数</returns>
        internal static int GetStringByte(this string str)
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
        internal static void AddList<T>(this List<T> list1, List<T> list2)
        {
            if (list2 != null)
            {
                list1.AddRange(list2);
            }
        }

        internal static int StringWidth(string str,int font)
        {
            return DX.GetDrawStringWidthToHandle(str, Program.GetStringByte(str), font);
        }
    }
}
