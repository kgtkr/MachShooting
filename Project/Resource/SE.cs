//*新しい効果音を追加するには？
//MP3に新しい音楽名を追加

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using System.Collections.ObjectModel;

namespace MachShooting
{
    /// <summary>
    /// 効果音管理クラス
    /// </summary>
    public class SE
    {
        /// <summary>
        /// このクラスのインスタンスです
        /// </summary>
        private static SE instance;

        /// <summary>
        /// このクラスのインスタンス
        /// </summary>
        public static SE Instance
        {
            get
            {
                if (SE.instance == null)
                {
                    SE.instance = new SE();
                }

                return SE.instance;
            }
        }

        /// <summary>
        /// 再生リスト
        /// </summary>
        private HashSet<int> music;

        private SE()
        {
            this.music = new HashSet<int>();
        }

        /// <summary>
        /// 効果音を流します。毎F呼び出してください
        /// </summary>
        internal void Update()
        {
            foreach (int handle in this.music.ToArray())
            {
                PlaySE(handle);
                this.music.Remove(handle);
            }
        }

        /// <summary>
        /// 再生予約します
        /// </summary>
        /// <param name="mp3">再生予約する音楽</param>
        public void Play(int handle)
        {
            this.music.Add(handle);
        }

        /// <summary>
        /// 効果音を流します
        /// </summary>
        /// <param name="se">ハンドル</param>
        private void PlaySE(int se)
        {
            //コピー
            int h = DX.DuplicateSoundMem(se);

            //再生が終わったら削除
            DX.SetPlayFinishDeleteSoundMem(DX.TRUE, h);

            //再生
            DX.PlaySoundMem(h, DX.DX_PLAYTYPE_BACK);

        }
    }
}
