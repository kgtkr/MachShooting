//*新しい効果音を追加するには？
//SE.fileNameに拡張子を除いたファイル名を追加
//MP3に新しい音楽名を追加

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;

namespace MachShooting
{
    /// <summary>
    /// 効果音管理クラス
    /// </summary>
    public static class SE
    {
        /// <summary>
        /// ファイル名
        /// </summary>
        private static string[] fileName={ "avoidance",
        "bom",
        "cancel",
        "deathblow",
        "hp1",
        "hp2",
        "OK",
        "shot",
        "shotHit",
        "strengthen"};

        /// <summary>
        /// 音楽
        /// </summary>
        private　static Dictionary<MP3,Music> music;

        /// <summary>
        /// ロードします
        /// </summary>
        public static void Load()
        {
            SE.music = new Dictionary<MP3, Music>();

            for (int i = 0; i < SE.fileName.Length; i++)
            {
                SE.music.Add((MP3)i, new Music(DX.LoadSoundMem("Data/Sound/SE/"+((MP3)i).GetFileName() +".mp3")));
            }
        }

        /// <summary>
        /// 効果音を流します。毎F呼び出してください
        /// </summary>
        public static void Update()
        {
            foreach(MP3 mp3 in SE.music.Keys.ToArray())
            {
                Music music = SE.music[mp3];
                if (SE.music[mp3].play)
                {
                    PlaySE(music.handle);
                    music.play = false;
                    SE.music[mp3] = music;
                }
            }
        }

        /// <summary>
        /// 再生予約します
        /// </summary>
        /// <param name="mp3">再生予約する音楽</param>
        public static void Play(MP3 mp3)
        {
            Music music = SE.music[mp3];
            music.play = true;
            SE.music[mp3] = music;
        }

        /// <summary>
        /// ファイル名を取得します
        /// </summary>
        /// <param name="mp3"></param>
        /// <returns></returns>
        private static string GetFileName(this MP3 mp3)
        {
            return SE.fileName[(int)mp3];
        }

        /// <summary>
        /// 効果音を流します
        /// </summary>
        /// <param name="se">ハンドル</param>
        private static void PlaySE(int se)
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
