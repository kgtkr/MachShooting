using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;

namespace MachShooting
{
    /// <summary>
    /// タイトル画面
    /// </summary>
    internal class Title : Screen
    {
        /*タイトル関係*/
        //最終的なx座標
        internal readonly int TITLE_X;

        //y座標
        internal const int TITLE_Y = 80;

        //スピード
        internal const int TITLE_SPEED = 10;

        //現在のx座標
        internal int titleNowX;

        //メッセージ
        internal const string TITLE_MESSAGE = "MachShooting";

        //メッセージのフォント
        internal readonly int TITLE_MESSAGE_FONT = Font.Instance.Font64;


        /*エンターキーを押して下さい関係*/
        //x座標
        internal readonly int ENTER_X;

        //y座標
        internal const int ENTER_Y = 300;

        //点滅切り替わり残りフレーム
        internal int enterFrame;

        //点滅間隔
        internal const int ENTER_INTERVAL = 30;

        //表示するか？(点滅)
        internal bool enterDisplay;

        //メッセージ
        internal const string ENTER_MESSAGE = "Zキーを押して下さい...";

        //メッセージのフォント
        internal readonly int ENTER_MESSAGE_FONT = Font.Instance.Font32;

        internal override void Draw()
        {
            if (this.Need)//必要なら
            {
                DX.DrawStringToHandle(this.titleNowX, TITLE_Y, TITLE_MESSAGE, DXColor.Instance.White, this.TITLE_MESSAGE_FONT);
                if (this.enterDisplay) DX.DrawStringToHandle(this.ENTER_X, ENTER_Y, ENTER_MESSAGE, DXColor.Instance.White, this.ENTER_MESSAGE_FONT);//表示するなら
            }
        }

        internal override void Process()
        {
            if (this.Need)//必要なら
            {
                //タイトル
                if (this.titleNowX < this.TITLE_X) this.titleNowX += TITLE_SPEED;//動かす
                if (this.titleNowX > this.TITLE_X) this.titleNowX = this.TITLE_X;//行き過ぎたなら

                //エンター
                if (this.enterFrame == 0)//残り時間0なら
                {
                    this.enterDisplay = !this.enterDisplay;//切り替え
                    this.enterFrame = Title.ENTER_INTERVAL;//残り時間をMAXにする
                }
                this.enterFrame--;//残り時間を減らす

                //キーが押されたか？
                if (Key.Instance.GetKeyUP(KeyComfig.MENU_OK))//Zが押されたなら
                {
                    SE.Instance.Play(DXAudio.Instance.OK);
                    this.Need = false;
                    this.Decision = true;
                }
            }
        }

        internal Title()
        {
            //タイトル
            {
                //描画する文字列の幅
                int w = DX.GetDrawStringWidthToHandle(TITLE_MESSAGE, Program.GetStringByte(TITLE_MESSAGE), this.TITLE_MESSAGE_FONT);
                this.TITLE_X = Program.WIDTH / 2 - w / 2;
                this.titleNowX = -w;
            }

            //エンターキー
            {
                //描画する文字列の幅
                int w = DX.GetDrawStringWidthToHandle(ENTER_MESSAGE, Program.GetStringByte(ENTER_MESSAGE), this.ENTER_MESSAGE_FONT);
                this.ENTER_X = Program.WIDTH / 2 - w / 2;
            }
        }
    }
}
