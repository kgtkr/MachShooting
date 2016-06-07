using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;

namespace MachShooting
{
    /// <summary>
    /// ミッション選択画面
    /// </summary>
    public class MissionMenu : Screen
    {
        #region 定数
        #endregion
        #region フィールド
        /// <summary>
        /// 現在選択されているインデックス
        /// </summary>
        private int missionIndex = 0;

        /// <summary>
        /// 敵一覧
        /// </summary>
        private static string[] missionData = { "ボア", "ガトー", "ラパン" , "ネガリャー" ,"スネーク","レオーネ"};

        /// <summary>
        /// OK1段階目
        /// </summary>
        private bool ok1;
        #endregion
        #region プロパティ
        /// <summary>
        /// 現在選択されている敵ID
        /// </summary>
        public int MissionData
        {
            get
            {
                return this.missionIndex;
            }
        }
        #endregion
        #region メソッド
        public override void Draw()
        {
            if (this.Need)//必要なら
            {
                //メッセージ
                {
                    //定数
                    const string MESSAGE = "敵を選択して下さい";
                    const int Y = 30;

                    //変数
                    int font = Program.font32;
                    int x = Program.WIDTH / 2 - DX.GetDrawStringWidthToHandle(MESSAGE, Program.GetStringByte(MESSAGE), font) / 2;

                    //描画
                    DX.DrawStringToHandle(x, Y, MESSAGE, Program.white, font);
                }
                //敵一覧
                {
                    //定数
                    const int Y = 100;
                    const int SPACE = 20;

                    //変数
                    int font = Program.font16;
                    int y = Y;

                    //描画
                    for (int i = 0; i < MissionMenu.missionData.Length; i++)
                    {
                        uint color = i == this.missionIndex ? Program.red : Program.white;
                        string name = MissionMenu.missionData[i];
                        int x = Program.WIDTH / 2 - DX.GetDrawStringWidthToHandle(name, Program.GetStringByte(name), font) / 2;
                        DX.DrawStringToHandle(x, y, name, color, font);
                        y += SPACE;
                    }
                }
            }
        }

        public override void Process(byte[] key, byte[] key2)
        {
            if (this.Need)//必要なら
            {
                //数取得
                int mn = MissionMenu.missionData.Length;//ミッションの数


                /*押されているかつ、最初か最後でないなら*/

                if (key2[Config.Instance.key[KeyComfig.MENU_DOWN]] == DX.TRUE)
                {
                    if (this.missionIndex + 1 != mn)
                    {
                        this.missionIndex++;
                    }
                }
                else if (key2[Config.Instance.key[KeyComfig.MENU_UP]] == DX.TRUE)
                {
                    if (this.missionIndex != 0)
                    {
                        this.missionIndex--;
                    }
                }

                if (key2[Config.Instance.key[KeyComfig.MENU_OK]] == DX.TRUE)//Zが押されたなら
                {
                    this.ok1 = true;
                }
                if(this.ok1&& key[Config.Instance.key[KeyComfig.MENU_OK]] == DX.FALSE)
                {
                    this.Need = false;
                    this.Decision = true;
                    SE.Instance.Play(MP3.OK);
                }
                else if (key2[Config.Instance.key[KeyComfig.MENU_BACK]] == DX.TRUE)//戻る
                {
                    this.Need = false;
                    this.Decision = false;
                    SE.Instance.Play(MP3.cancel);
                }
            }
        }
        #endregion
        #region コンストラクタ
        /// <summary>
        /// 新しいメニューを作成します
        /// </summary>
        public MissionMenu()
        {
        }
        #endregion
        
        #region 実装メソッド
        #endregion
        #region 未実装メソッド
        #endregion
    }
}
