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
    public class Title : Screen
    {
        /*タイトル関係*/
        //最終的なx座標
        public readonly int TITLE_X;

        //y座標
        public const int TITLE_Y = 80;

        //スピード
        public const int TITLE_SPEED = 10;

        //現在のx座標
        public int titleNowX;

        //メッセージ
        public const string TITLE_MESSAGE = "MachShooting";

        //メッセージのフォント
        public readonly int TITLE_MESSAGE_FONT = Program.font64;


        /*エンターキーを押して下さい関係*/
        //x座標
        public readonly int ENTER_X;

        //y座標
        public const int ENTER_Y = 300;

        //点滅切り替わり残りフレーム
        public int enterFrame;

        //点滅間隔
        public const int ENTER_INTERVAL = 30;

        //表示するか？(点滅)
        public bool enterDisplay;

        //メッセージ
        public const string ENTER_MESSAGE = "Zキーを押して下さい...";

        //メッセージのフォント
        public readonly int ENTER_MESSAGE_FONT = Program.font32;

        public override void Draw()
        {
            if (this.Need)//必要なら
            {
                DX.DrawStringToHandle(this.titleNowX, TITLE_Y, TITLE_MESSAGE, Program.white, this.TITLE_MESSAGE_FONT);
                if (this.enterDisplay) DX.DrawStringToHandle(this.ENTER_X, ENTER_Y, ENTER_MESSAGE, Program.white, this.ENTER_MESSAGE_FONT);//表示するなら
            }
        }

        public override void Process(byte[] key, byte[] key2)
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
                if (key2[Config.key[KeyComfig.MENU_OK]] == DX.TRUE)//Zが押されたなら
                {
                    SE.Play(MP3.OK);
                    this.Need = false;
                    this.Decision = true;
                }
            }
        }

        public Title()
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
