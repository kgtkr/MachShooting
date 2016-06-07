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
        /// 音楽
        /// </summary>
        private Dictionary<MP3,Music> music;

        private SE()
        {
            this.music = new Dictionary<MP3, Music>();

            var fileName= Enum.GetValues(typeof(MP3));

            foreach(MP3 mp3 in fileName)
            {
                this.music.Add(mp3, new Music(DX.LoadSoundMem("Data/Sound/SE/" + mp3 + ".mp3")));
            }
        }

        /// <summary>
        /// 効果音を流します。毎F呼び出してください
        /// </summary>
        public void Update()
        {
            foreach(MP3 mp3 in this.music.Keys.ToArray())
            {
                Music music = this.music[mp3];
                if (this.music[mp3].play)
                {
                    PlaySE(music.handle);
                    music.play = false;
                }
                this.music[mp3] = music;
            }
        }

        /// <summary>
        /// 再生予約します
        /// </summary>
        /// <param name="mp3">再生予約する音楽</param>
        public void Play(MP3 mp3)
        {
            Music music = this.music[mp3];
            music.play = true;
            this.music[mp3] = music;
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
    /// <summary>
    /// 音楽情報クラス
    /// </summary>
    public struct Music
    {
        /// <summary>
        /// ハンドル
        /// </summary>
        public int handle;

        /// <summary>
        /// 再生するかの予約
        /// </summary>
        public bool play;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="h">ハンドル</param>
        public Music(int h)
        {
            this.handle = h;
            this.play = false;
        }
    }
    /// <summary>
    /// MP3一覧
    /// </summary>
    public enum MP3
    {
       avoidance,
        bom,
        cancel,
        deathblow,
        hp1,
        hp2,
        OK,
        shot,
        shotHit,
        strengthen
    }
}
