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
    internal class Fps
    {
        private static Fps instance;

        internal static Fps Instance
        {
            get
            {
                if (Fps.instance == null)
                {
                    Fps.instance = new Fps();
                }

                return Fps.instance;
            }
        }

        private Fps()
        {
        }

        #region フィールド
        /// <summary>
        /// 何Fに一回カウントするか
        /// </summary>
        private const int WAIT=50;

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
        internal double FPS
        {
            get { return this.fps; }
        }
        #endregion
        #region メソッド
        /// <summary>
        /// フレームを1進めます
        /// </summary>
        internal void Update()
        {
            if (counter == 0) time[0] = DX.GetNowCount();//もし最後のカウントから50fたったなら一週目の時間取得
            if (counter == Fps.WAIT - 1)//時間ならfpsを取得
            {
                time[1] = DX.GetNowCount();//今回の時間
                fps = 1000.0 / ((time[1] - time[0]) / (double)Fps.WAIT);
                time[0] = time[1];//今回の時間を前回の時間に代入
                counter = 0;//カウント初期化
            }
            counter++;//カウントする
        }
        #endregion
    }
}
