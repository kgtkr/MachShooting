using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using System.IO;
using System.Diagnostics;
using MachShooting.Graphic;
using NLua;

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

        /*選択情報*/
        /// <summary>
        /// 選択しているクエスト
        /// </summary>
        private static EnemyHeader missionData;

        /// <summary>
        /// 選択している装備
        /// </summary>
        private static Equipment equipment;
        /// <summary>
        /// メインメソッド
        /// </summary>
        public static void Main()
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
            //前フレームのキーを作成
            byte[] lastKey = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                lastKey[i] = DX.FALSE;
            }
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
        private static void Update(byte[] key, byte[] key2)
        {

            if (Program.title != null)//タイトル画面なら
            {
                Program.title.Process(key, key2);
                if (count % Config.Instance.FrameSkip == 0) Program.title.Draw();
                if (!Program.title.Need)
                {
                    Program.title = null;
                    Program.equipmentMenu = new EquipmentMenu();
                }
            }
            else if (Program.equipmentMenu != null)//装備選択画面なら
            {
                Program.equipmentMenu.Process(key, key2);
                if (count % Config.Instance.FrameSkip == 0) Program.equipmentMenu.Draw();
                if (!Program.equipmentMenu.Need)
                {
                    Program.equipment = Program.equipmentMenu.Equipment;
                    Program.equipmentMenu = null;
                    Program.missionMenu = new MissionMenu(EnemyHeaderTree.Instance);
                }
            }
            else if (Program.missionMenu != null)//敵選択画面なら
            {
                Program.missionMenu.Process(key, key2);
                if (count % Config.Instance.FrameSkip == 0) Program.missionMenu.Draw();
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
                if (count % Config.Instance.FrameSkip == 0) Program.game.Draw();
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

        public static int StringWidth(string str,int font)
        {
            return DX.GetDrawStringWidthToHandle(str, Program.GetStringByte(str), font);
        }
    }
}
