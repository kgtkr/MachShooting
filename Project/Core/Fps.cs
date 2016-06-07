using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;

namespace MachShooting
{
    /// <summary>
    /// FPSを管理するクラス
    /// </summary>
    public class Fps
    {
        #region フィールド
        /// <summary>
        /// 何Fに一回カウントするか
        /// </summary>
        private int f;

        /// <summary>
        /// 0:前回の時間 1:今回の時間
        /// </summary>
        private int[] time = new int[2];

        /// <summary>
        /// カウント
        /// </summary>
        private int counter = 0;

        /// <summary>
        /// FPS
        /// </summary>
        private double fps = 0.0;
        #endregion
        #region プロパティ
        /// <summary>
        /// FPS
        /// </summary>
        public double FPS
        {
            get { return this.fps; }
        }
        #endregion
        #region メソッド
        /// <summary>
        /// フレームを1進めます
        /// </summary>
        public void Update()
        {
            if (counter == 0) time[0] = DX.GetNowCount();//もし最後のカウントから50fたったなら一週目の時間取得
            if (counter == f - 1)//50f目ならfpsを取得
            {
                time[1] = DX.GetNowCount();//今回の時間
                fps = 1000.0 / ((time[1] - time[0]) / (double)f);
                time[0] = time[1];//今回の時間を前回の時間に代入
                counter = 0;//カウント初期化
            }
            counter++;//カウントする
        }

        /// <summary>
        /// 新しいFPSカウンターを作ります
        /// </summary>
        /// <param name="f">何フレームに一度計測するか</param>
        public Fps(int f)
        {
            this.f = f;
        }

        /// <summary>
        /// 新しいFPSカウンターを作ります
        /// </summary>
        public Fps()
        {
            this.f = 50;
        }
        #endregion
    }
}
