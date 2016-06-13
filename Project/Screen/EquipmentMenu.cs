using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;

namespace MachShooting
{
    /// <summary>
    /// 装備選択画面
    /// </summary>
    public class EquipmentMenu : Screen
    {
        #region 定数
        #endregion
        #region フィールド
        private int index = 0;
        #endregion
        #region プロパティ
        /// <summary>
        /// 選択されている武器種
        /// </summary>
        public Equipment Equipment
        {
            get { return (Equipment)this.index; }
        }
        #endregion
        #region コンストラクタ
        #endregion
        #region メソッド
        public override void Draw()
        {
            if (this.Need)
            {
                //メッセージ
                {
                    //定数
                    const string MESSAGE = "自機を選択して下さい";
                    const int Y = 30;

                    //変数
                    int font = Font.Instance.font32;
                    int x = Program.WIDTH / 2 - DX.GetDrawStringWidthToHandle(MESSAGE, Program.GetStringByte(MESSAGE), font) / 2;

                    //描画
                    DX.DrawStringToHandle(x, Y, MESSAGE, DXColor.Instance.white, font);
                }
                //武器一覧
                {
                    //定数
                    const int Y = 100;
                    const int SPACE = 20;
                    const int NUMBER = 5;//武器の種類

                    //変数
                    int font = Font.Instance.font16;
                    int y = Y;

                    //描画
                    for (int i = 0; i < NUMBER; i++)
                    {
                        uint color = i == this.index ? DXColor.Instance.red : DXColor.Instance.white;
                        string name = ((Equipment)i).GetName();
                        int x = Program.WIDTH / 2 - DX.GetDrawStringWidthToHandle(name, Program.GetStringByte(name), font) / 2;
                        DX.DrawStringToHandle(x, y, name, color, font);
                        y += SPACE;
                    }
                }
            }
        }

        public override void Process(byte[] key, byte[] key2)
        {
            if (this.Need)
            {
                //押されているかつ要素の最後でないなら
                if (key2[Config.Instance.key[KeyComfig.MENU_DOWN]] == DX.TRUE)//下
                {
                    if (this.index + 1 != 5)
                    {
                        this.index++;
                    }
                }
                else if (key2[Config.Instance.key[KeyComfig.MENU_UP]] == DX.TRUE)//上
                {
                    if (this.index != 0)
                    {
                        this.index--;
                    }
                }

                //決定
                if (key2[Config.Instance.key[KeyComfig.MENU_OK]] == DX.TRUE)//決定
                {
                    this.Need = false;
                    this.Decision = true;
                    SE.Instance.Play(DXAudio.Instance.OK);
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
