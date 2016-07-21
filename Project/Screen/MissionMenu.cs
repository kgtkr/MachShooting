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
        /// 一覧
        /// </summary>
        private HeaderTree<EnemyHeader> tree;

        /// <summary>
        /// 現在のインデックス
        /// </summary>
        private int index = 0;

        /// <summary>
        /// OK1段階目
        /// </summary>
        private bool ok1;

        /// <summary>
        /// カテゴリが選択されているならメニュー
        /// ないならnull
        /// </summary>
        private MissionMenu menu;
        #endregion
        #region プロパティ
        /// <summary>
        /// 選択された敵(Decision=trueの場合のみ)
        /// </summary>
        public EnemyHeader MissionData
        {
            get;
            private set;
        }
        #endregion
        #region メソッド
        public override void Draw()
        {
            if (this.Need)//必要なら
            {
                if (this.menu == null)
                {
                    //メッセージ
                    {
                        //定数
                        const string MESSAGE = "敵を選択して下さい";
                        const int Y = 30;

                        //変数
                        int font = Font.Instance.Font32;
                        int x = Program.WIDTH / 2 - DX.GetDrawStringWidthToHandle(MESSAGE, Program.GetStringByte(MESSAGE), font) / 2;

                        //描画
                        DX.DrawStringToHandle(x, Y, MESSAGE, DXColor.Instance.White, font);
                    }
                    //敵一覧
                    {
                        //定数
                        const int Y = 100;
                        const int SPACE = 20;

                        //変数
                        int font = Font.Instance.Font16;
                        int y = Y;

                        //描画
                        for (int i = 0; i < this.tree.Header.Count; i++)
                        {
                            uint color = i == this.index ? DXColor.Instance.Red : DXColor.Instance.White;
                            string name = this.tree.Header[i].name;
                            int x = Program.WIDTH / 2 - DX.GetDrawStringWidthToHandle(name, Program.GetStringByte(name), font) / 2;
                            DX.DrawStringToHandle(x, y, name, color, font);
                            y += SPACE;
                        }

                        for (int i = 0; i < this.tree.Tree.Count; i++)
                        {
                            uint color = i == this.index - this.tree.Header.Count ? DXColor.Instance.Red : DXColor.Instance.White;
                            string name = "+" + this.tree.Tree[i].Name;
                            int x = Program.WIDTH / 2 - DX.GetDrawStringWidthToHandle(name, Program.GetStringByte(name), font) / 2;
                            DX.DrawStringToHandle(x, y, name, color, font);
                            y += SPACE;
                        }
                    }
                }
                else
                {
                    this.menu.Draw();
                }
            }
        }

        public override void Process(byte[] key, byte[] key2)
        {
            if (this.Need)//必要なら
            {
                if (this.menu == null)
                {
                    //数取得
                    int mn = this.tree.Header.Count + this.tree.Tree.Count;


                    /*押されているかつ、最初か最後でないなら*/

                    if (key2[Config.Instance.Key[KeyComfig.MENU_DOWN]] == DX.TRUE)
                    {
                        if (this.index + 1 != mn)
                        {
                            this.index++;
                        }
                    }
                    else if (key2[Config.Instance.Key[KeyComfig.MENU_UP]] == DX.TRUE)
                    {
                        if (this.index != 0)
                        {
                            this.index--;
                        }
                    }

                    if (key2[Config.Instance.Key[KeyComfig.MENU_OK]] == DX.TRUE)//Zが押されたなら
                    {
                        this.ok1 = true;
                    }

                    if (this.ok1 && key[Config.Instance.Key[KeyComfig.MENU_OK]] == DX.FALSE && this.tree.Header.Count + this.tree.Tree.Count != 0)
                    {
                        //ミッションなら
                        if (this.index < this.tree.Header.Count)
                        {
                            this.Need = false;
                            this.Decision = true;
                            this.MissionData = this.tree.Header[this.index];
                        }
                        else//カテゴリなら
                        {
                            this.menu = new MissionMenu(this.tree.Tree[this.index-this.tree.Header.Count]);
                        }

                        SE.Instance.Play(DXAudio.Instance.OK);

                        this.ok1 = false;
                    }

                    else if (key2[Config.Instance.Key[KeyComfig.MENU_BACK]] == DX.TRUE)//戻る
                    {
                        this.Need = false;
                        this.Decision = false;
                        SE.Instance.Play(DXAudio.Instance.Cancel);
                    }
                }
                else
                {
                    this.menu.Process(key, key2);
                    if (!this.menu.Need)
                    {
                        if (this.menu.Decision)
                        {
                            this.Need = false;
                            this.Decision = true;
                            this.MissionData = this.menu.MissionData;
                            this.menu = null;
                        }
                        else
                        {
                            this.menu = null;
                        }
                    }
                }
            }
        }
        #endregion
        #region コンストラクタ
        /// <summary>
        /// 新しいメニューを作成します
        /// </summary>
        public MissionMenu(HeaderTree<EnemyHeader> tree)
        {
            this.tree = tree;
        }
        #endregion
        
        #region 実装メソッド
        #endregion
        #region 未実装メソッド
        #endregion
    }
}
