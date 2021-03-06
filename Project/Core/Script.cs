﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLua;


namespace MachShooting
{
    internal class Script
    {
        private static HeaderTree<EnemyHeader> enemyH;

        internal static HeaderTree<EnemyHeader> EnemyH
        {
            get
            {
                if (Script.enemyH == null)
                {
                    Script.enemyH = new HeaderTree<EnemyHeader>("script/enemy/", "root", path => new EnemyHeader(path));
                }

                return Script.enemyH;
            }
        }

        private static HeaderTree<PlayerHeader> playerH;

        internal static HeaderTree<PlayerHeader> PlayerH
        {
            get
            {
                if (Script.playerH == null)
                {
                    Script.playerH = new HeaderTree<PlayerHeader>("script/enemy/", "root", path => new PlayerHeader(path));
                }

                return Script.playerH;
            }
        }

        private static Script instance;

        internal static Script Instance
        {
            get
            {
                if (Script.instance == null)
                {
                    Script.instance = new Script();
                }

                return Script.instance;
            }
        }

        internal Lua lua {
            get;
            private set;
        }

        private Script()
        {
            this.lua = new Lua();
            this.lua.LoadCLRPackage();
            this.lua.DoString("import (\"DxLibDotNet\",\"DxLibDLL\");");
            this.lua.DoString("import (\"MachShooting\",\"MachShooting\");");
            this.lua.DoString("import (\"System.Drawing\",\"System.Drawing\");");
            
            //ライブラリ読み込み
            Action<string> loadLib = null;
            loadLib = dir =>
            {
                foreach (string f in Directory.GetFiles(dir))
                {
                    this.lua.DoFile(f);
                }

                foreach (string d in Directory.GetDirectories(dir))
                {
                    loadLib(d);
                }
            };
            loadLib("script/lib");
            loadLib("Data/script");
        }

        internal static Dictionary<string,string> ParseHeaderAndLoadScript(string path)
        {
            string src = null;
            using (StreamReader sr = new StreamReader(
            path, Encoding.GetEncoding("UTF-8")))
            {
                src = sr.ReadToEnd();
            }

            string data = "";
            var srcR = new StringReader(src);
            while (srcR.Peek() >= 0)
            {
                // ファイルを 1 行ずつ読み込む
                string stBuffer = srcR.ReadLine();

                if (stBuffer != "--[[")
                {
                    data += stBuffer + "\n";
                }

                if (stBuffer == "]]")
                {
                    break;
                }
            }
            var h = Config.ParseINI(data);

            Script.Instance.lua.DoFile(path);

            return h;
        }
    }

    internal class HeaderTree<TData>
    {
        /// <summary>
        /// ヘッダー
        /// </summary>
        internal IReadOnlyList<TData> Header
        {
            get;
            private set;
        }

        /// <summary>
        /// ツリー
        /// </summary>
        internal IReadOnlyList<HeaderTree<TData>> Tree
        {
            get;
            private set;
        }

        internal string Name
        {
            get;
            private set;
        }

        internal HeaderTree(string dir, string name,Func<string,TData> func)
        {
            this.Name = Path.GetFileName(dir);

            var header = new List<TData>();
            var tree = new List<HeaderTree<TData>>();

            var files = Directory.GetFiles(dir);
            foreach (string file in files)
            {
                header.Add(func(file));
            }

            var dirs = Directory.GetDirectories(dir);
            foreach (string dir2 in dirs)
            {
                tree.Add(new HeaderTree<TData>(dir2, Path.GetFileName(Path.GetDirectoryName(dir)),func));
            }

            this.Header = header.AsReadOnly();
            this.Tree = tree.AsReadOnly();
        }
    }

    /// <summary>
    /// 敵のヘッダー構造体
    /// </summary>
    internal class EnemyHeader
    {
        internal string name
        {
            get;
            private set;
        }
        internal int hp
        {
            get;
            private set;
        }
        internal string image
        {
            get;
            private set;
        }
        internal double r
        {
            get;
            private set;
        }
        internal string className
        {
            get;
            private set;
        }

        internal EnemyHeader(string path)
        {
            var h = Script.ParseHeaderAndLoadScript(path);
            this.name = h["NAME"];
            this.hp = int.Parse(h["HP"]);
            this.image = h["IMAGE"];
            this.r = double.Parse(h["R"]);
            this.className = h["CLASS"];
        }

        public override string ToString()
        {
            return this.name;
        }
    }

    /// <summary>
    /// 自機のヘッダー構造体
    /// </summary>
    internal class PlayerHeader
    {
        internal string name
        {
            get;
            private set;
        }
        /// <summary>
        /// 必殺技ゲージ
        /// </summary>
        internal int dg
        {
            get;
            private set;
        }
        /// <summary>
        /// 自己強化ゲージ
        /// </summary>
        internal int sg
        {
            get;
            private set;
        }
        internal string className
        {
            get;
            private set;
        }

        internal PlayerHeader(string path)
        {
            var h = Script.ParseHeaderAndLoadScript(path);
            this.name = h["NAME"];
            this.dg = int.Parse(h["KG"]);
            this.sg = int.Parse(h["DG"]);
            this.className = h["CLASS"];
        }

        public override string ToString()
        {
            return this.name;
        }
    }
}