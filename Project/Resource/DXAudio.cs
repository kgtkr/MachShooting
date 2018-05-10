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
    internal class DXAudio
    {
        /// <summary>
        /// このクラスのインスタンスです
        /// </summary>
        private static DXAudio instance;

        /// <summary>
        /// このクラスのインスタンス
        /// </summary>
        internal static DXAudio Instance
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
        internal int Avoidance
        {
            get;
            private set;
        }

        internal int Bom
        {
            get;
            private set;
        }

        internal int Cancel
        {
            get;
            private set;
        }

        internal int Deathblow
        {
            get;
            private set;
        }

        internal int HP1
        {
            get;
            private set;
        }

        internal int HP2
        {
            get;
            private set;
        }

        internal int OK
        {
            get;
            private set;
        }

        internal int Shot
        {
            get;
            private set;
        }

        internal int ShotHit
        {
            get;
            private set;
        }

        internal int Strengthen
        {
            get;
            private set;
        }
        #endregion

        private DXAudio()
        {
            this.Avoidance = DX.LoadSoundMem("Data/Sound/SE/avoidance.mp3");
            this.Bom = DX.LoadSoundMem("Data/Sound/SE/bom.mp3");
            this.Cancel = DX.LoadSoundMem("Data/Sound/SE/cancel.mp3");
            this.Deathblow = DX.LoadSoundMem("Data/Sound/SE/deathblow.mp3");
            this.HP1 = DX.LoadSoundMem("Data/Sound/SE/hp1.mp3");
            this.HP2 = DX.LoadSoundMem("Data/Sound/SE/hp2.mp3");
            this.OK = DX.LoadSoundMem("Data/Sound/SE/OK.mp3");
            this.Shot = DX.LoadSoundMem("Data/Sound/SE/shot.mp3");
            this.ShotHit = DX.LoadSoundMem("Data/Sound/SE/shotHit.mp3");
            this.Strengthen = DX.LoadSoundMem("Data/Sound/SE/strengthen.mp3");
        }
    }

}