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
    internal class TreeMenu<THeader> : Screen
    {
        #region 定数
        #endregion
        #region フィールド
        /// <summary>
        /// 一覧
        /// </summary>
        private HeaderTree<THeader> tree;

        /// <summary>
        /// 現在のインデックス
        /// </summary>
        private int index = 0;

        /// <summary>
        /// カテゴリが選択されているならメニュー
        /// ないならnull
        /// </summary>
        private TreeMenu<THeader> menu;
        #endregion
        #region プロパティ
        /// <summary>
        /// 選択された敵(Decision=trueの場合のみ)
        /// </summary>
        internal THeader Header
        {
            get;
            private set;
        }

        private string msg;

        internal TreeMenu(string msg, HeaderTree<THeader> tree)
        {
            this.msg = msg;
            this.tree = tree;
        }
        #endregion
        #region メソッド
        internal override void Draw()
        {
            if (this.Need)//必要なら
            {
                if (this.menu == null)
                {
                    //メッセージ
                    {
                        const int Y = 30;

                        //変数
                        int font = Font.Instance.Font32;
                        int x = Program.WIDTH / 2 - DX.GetDrawStringWidthToHandle(this.msg, Program.GetStringByte(this.msg), font) / 2;

                        //描画
                        DX.DrawStringToHandle(x, Y, this.msg, DXColor.Instance.White, font);
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
                            string name = this.tree.Header[i].ToString();
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

        internal override void Process()
        {
            if (this.Need)//必要なら
            {
                if (this.menu == null)
                {
                    //数取得
                    int mn = this.tree.Header.Count + this.tree.Tree.Count;


                    /*押されているかつ、最初か最後でないなら*/

                    if (Key.Instance.GetKeyUP(KeyComfig.MENU_DOWN))
                    {
                        if (this.index + 1 != mn)
                        {
                            this.index++;
                        }
                    }
                    else if (Key.Instance.GetKeyUP(KeyComfig.MENU_UP))
                    {
                        if (this.index != 0)
                        {
                            this.index--;
                        }
                    }

                    if (Key.Instance.GetKeyUP(KeyComfig.MENU_OK) && this.tree.Header.Count + this.tree.Tree.Count != 0)
                    {
                        //ミッションなら
                        if (this.index < this.tree.Header.Count)
                        {
                            this.Need = false;
                            this.Decision = true;
                            this.Header = this.tree.Header[this.index];
                        }
                        else//カテゴリなら
                        {
                            this.menu = new TreeMenu<THeader>(this.msg,this.tree.Tree[this.index - this.tree.Header.Count]);
                        }

                        SE.Instance.Play(DXAudio.Instance.OK);
                    }

                    else if (Key.Instance.GetKeyUP(KeyComfig.MENU_BACK))//戻る
                    {
                        this.Need = false;
                        this.Decision = false;
                        SE.Instance.Play(DXAudio.Instance.Cancel);
                    }
                }
                else
                {
                    this.menu.Process();
                    if (!this.menu.Need)
                    {
                        if (this.menu.Decision)
                        {
                            this.Need = false;
                            this.Decision = true;
                            this.Header = this.menu.Header;
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

        #region 実装メソッド
        #endregion
        #region 未実装メソッド
        #endregion
    }
}
