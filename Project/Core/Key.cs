using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;

namespace MachShooting
{
    public class Key
    {
        /// <summary>
        /// このクラスのインスタンスです
        /// </summary>
        private static Key instance;

        /// <summary>
        /// このクラスのインスタンス
        /// </summary>
        public static Key Instance
        {
            get
            {
                if (Key.instance == null)
                {
                    Key.instance = new Key();
                }

                return Key.instance;
            }
        }

        private Key()
        {
            this.key = this.keyOld = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                key[i] = DX.FALSE;
            }
        }

        private byte[] key;

        private byte[] keyOld;

        public void Update()
        {
            this.keyOld = this.key;
            this.key = new byte[256];
            DX.GetHitKeyStateAll(out this.key[0]);
        }

        public bool GetKey(int code)
        {
            return this.key[code] == DX.TRUE;
        }

        public bool GetKeyDown(int code)
        {
            return this.key[code] == DX.TRUE && this.keyOld[code] == DX.FALSE;
        }

        public bool GetKeyUP(int code)
        {
            return this.key[code] == DX.FALSE && this.keyOld[code] == DX.TRUE;
        }

        public bool GetKey(KeyComfig c)
        {
            return this.GetKey(Config.Instance.Key[c]);
        }

        public bool GetKeyDown(KeyComfig c)
        {
            return this.GetKeyDown(Config.Instance.Key[c]);
        }

        public bool GetKeyUP(KeyComfig c)
        {
            return this.GetKeyUP(Config.Instance.Key[c]);
        }
    }
}
