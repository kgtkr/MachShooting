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
    public class DXAudio
    {
        /// <summary>
        /// このクラスのインスタンスです
        /// </summary>
        private static DXAudio instance;

        /// <summary>
        /// このクラスのインスタンス
        /// </summary>
        public static DXAudio Instance
        {
            get
            {
                if (DXAudio.instance == null)
                {
                    DXAudio.instance = new DXAudio();
                }

                return DXAudio.instance;
            }
        }

        #region SE
        public int avoidance
        {
            get;
            private set;
        }

        public int bom
        {
            get;
            private set;
        }

        public int cancel
        {
            get;
            private set;
        }

        public int deathblow
        {
            get;
            private set;
        }

        public int hp1
        {
            get;
            private set;
        }

        public int hp2
        {
            get;
            private set;
        }

        public int OK
        {
            get;
            private set;
        }

        public int shot
        {
            get;
            private set;
        }

        public int shotHit
        {
            get;
            private set;
        }

        public int strengthen
        {
            get;
            private set;
        }
        #endregion

        private DXAudio()
        {
            this.avoidance = DX.LoadSoundMem("Data/Sound/SE/avoidance.mp3");
            this.bom = DX.LoadSoundMem("Data/Sound/SE/bom.mp3");
            this.cancel = DX.LoadSoundMem("Data/Sound/SE/cancel.mp3");
            this.deathblow = DX.LoadSoundMem("Data/Sound/SE/deathblow.mp3");
            this.hp1 = DX.LoadSoundMem("Data/Sound/SE/hp1.mp3");
            this.hp2 = DX.LoadSoundMem("Data/Sound/SE/hp2.mp3");
            this.OK = DX.LoadSoundMem("Data/Sound/SE/OK.mp3");
            this.shot = DX.LoadSoundMem("Data/Sound/SE/shot.mp3");
            this.shotHit = DX.LoadSoundMem("Data/Sound/SE/shotHit.mp3");
            this.strengthen = DX.LoadSoundMem("Data/Sound/SE/strengthen.mp3");
        }
    }

}