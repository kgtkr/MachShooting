using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLua;


namespace MachShooting
{
    public class Script
    {
        private static HeaderTree<EnemyHeader> enemyH;

        public static HeaderTree<EnemyHeader> EnemyH
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

        private static Script instance;

        public static Script Instance
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

        public Lua lua {
            get;
            private set;
        }

        private Script()
        {
            this.lua = new Lua();
            this.lua.LoadCLRPackage();
            this.lua.DoString("import (\"DxLibDotNet\",\"DxLibDLL\");");
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
        }
    }

    public class HeaderTree<TData>
    {
        /// <summary>
        /// ヘッダー
        /// </summary>
        public IReadOnlyList<TData> Header
        {
            get;
            private set;
        }

        /// <summary>
        /// ツリー
        /// </summary>
        public IReadOnlyList<HeaderTree<TData>> Tree
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public HeaderTree(string dir, string name,Func<string,TData> func)
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
    public class EnemyHeader
    {
        public string name
        {
            get;
            private set;
        }
        public int hp
        {
            get;
            private set;
        }
        public string image
        {
            get;
            private set;
        }
        public double r
        {
            get;
            private set;
        }
        public string className
        {
            get;
            private set;
        }

        public EnemyHeader(string path)
        {
            string src=null;
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
            this.name = h["NAME"];
            this.hp = int.Parse(h["HP"]);
            this.image = h["IMAGE"];
            this.r = double.Parse(h["R"]);
            this.className = h["CLASS"];

            Script.Instance.lua.DoFile(path);
        }
    }
}