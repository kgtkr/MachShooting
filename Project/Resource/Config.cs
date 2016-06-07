using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MachShooting
{
    public class Config
    {
        /// <summary>
        /// このクラスのインスタンスです
        /// </summary>
        private static Config instance;

        /// <summary>
        /// キーコンフィグ
        /// </summary>
        public IReadOnlyDictionary<KeyComfig, int> key
        {
            get;
            private set;
        }

        /// <summary>
        /// FPS
        /// 60/X
        /// </summary>
        public int fps
        {
            get;
            private set;
        }

        /// <summary>
        /// 低負荷モード
        /// </summary>
        public bool low
        {
            get;
            private set;
        }

        /// <summary>
        /// フルスクリーン
        /// </summary>
        public bool full
        {
            get;
            private set;
        }

        /// <summary>
        /// このクラスのインスタンス
        /// </summary>
        public static Config Instance
        {
            get
            {
                if (Config.instance == null)
                {
                    Config.instance = new Config();
                }

                return Config.instance;
            }
        }

        private Config()
        {
            {
                var keyMap = new Dictionary<KeyComfig, int>();
                var ini = ReadINI("keyconfig.ini");
                foreach (string key in ini.Keys)
                {
                    KeyComfig k = (KeyComfig)Enum.Parse(typeof(KeyComfig), key);
                    int v = (int)Enum.Parse(typeof(Key), ini[key]);
                    keyMap.Add(k, v);
                }
                this.key = new ReadOnlyDictionary<KeyComfig,int>(keyMap);
            }

            {
                var ini = ReadINI("config.ini");
                low = ini["LOW_MODE"] == "true";
                fps = int.Parse(ini["FPS"]);
                full = ini["FULL_SCREEN"] == "true";
            }
        }

        private Dictionary<string,string> ReadINI(string path)
        {
            Dictionary<string, string> ini = new Dictionary<string, string>();

            using (StreamReader sr = new StreamReader(
            path, Encoding.GetEncoding("UTF-8")))
            {
                while (sr.Peek() >= 0)
                {
                    // ファイルを 1 行ずつ読み込む
                    string stBuffer = sr.ReadLine();


                    Regex reg = new Regex("^(?<k>[^=#]+)=(?<v>[^=#]+)$", RegexOptions.IgnoreCase);
                    Match m = reg.Match(stBuffer);
                    if (m.Success == true)
                    {
                        ini.Add(m.Groups["k"].Value.Trim(), m.Groups["v"].Value.Trim());
                    }
                }
            }

            return ini;
        }
    }



    public enum KeyComfig
    {
        MENU_UP,
        MENU_DOWN,
        MENU_OK,
        MENU_BACK,

        GAME_UP,
        GAME_DOWN,
        GAME_RIGHT,
        GAME_LEFT,
        GAME_ATTACK,
        GAME_SPECIAL,
        GAME_AVOIDANCE,
        GAME_DEATHBLOW,
        GAME_STRENGTHEN,
        GAME_TARGET_CAMERA,
        GAME_CAMERA_RIGHT,
        GAME_CAMERA_LEFT,
        GAME_CAMERA_DOWN,
        GAME_CAMERA_ROTATION_RIGHT,
        GAME_CAMERA_ROTATION_LEFT,
        GAME_STOP
    }

    public enum Key
    {
        BACK = 14,
        TAB = 15,
        RETURN = 28,
        LSHIFT = 42,
        RSHIFT = 54,
        LCONTROL = 29,
        RCONTROL = 157,
        ESCAPE = 1,
        SPACE = 57,
        PGUP = 201,
        PGDN = 209,
        END = 207,
        HOME = 199,
        LEFT = 203,
        UP = 200,
        RIGHT = 205,
        DOWN = 208,
        INSERT = 210,
        DELETE = 211,
        MINUS = 12,
        YEN = 125,
        PREVTRACK = 144,
        PERIOD = 52,
        SLASH = 53,
        LALT = 56,
        RALT = 184,
        SCROLL = 70,
        SEMICOLON = 39,
        COLON = 146,
        LBRACKET = 26,
        RBRACKET = 27,
        AT = 145,
        BACKSLASH = 43,
        COMMA = 51,
        KANJI = 148,
        CONVERT = 121,
        NOCONVERT = 123,
        KANA = 112,
        APPS = 221,
        CAPSLOCK = 58,
        SYSRQ = 183,
        PAUSE = 197,
        LWIN = 219,
        RWIN = 220,
        NUMLOCK = 69,
        NUMPAD0 = 82,
        NUMPAD1 = 79,
        NUMPAD2 = 80,
        NUMPAD3 = 81,
        NUMPAD4 = 75,
        NUMPAD5 = 76,
        NUMPAD6 = 77,
        NUMPAD7 = 71,
        NUMPAD8 = 72,
        NUMPAD9 = 73,
        MULTIPLY = 55,
        ADD = 78,
        SUBTRACT = 74,
        DECIMAL = 83,
        DIVIDE = 181,
        NUMPADENTER = 156,
        F1 = 59,
        F2 = 60,
        F3 = 61,
        F4 = 62,
        F5 = 63,
        F6 = 64,
        F7 = 65,
        F8 = 66,
        F9 = 67,
        F10 = 68,
        F11 = 87,
        F12 = 88,
        A = 30,
        B = 48,
        C = 46,
        D = 32,
        E = 18,
        F = 33,
        G = 34,
        H = 35,
        I = 23,
        J = 36,
        K = 37,
        L = 38,
        M = 50,
        N = 49,
        O = 24,
        P = 25,
        Q = 16,
        R = 19,
        S = 31,
        T = 20,
        U = 22,
        V = 47,
        W = 17,
        X = 45,
        Y = 21,
        Z = 44,
        _0 = 11,
        _1 = 2,
        _2 = 3,
        _3 = 4,
        _4 = 5,
        _5 = 6,
        _6 = 7,
        _7 = 8,
        _8 = 9,
        _9 = 10
    }
}
