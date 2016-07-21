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

    public class EnemyHeaderTree
    {
        private static EnemyHeaderTree instance;

        public static EnemyHeaderTree Instance
        {
            get
            {
                if (EnemyHeaderTree.instance == null)
                {
                    EnemyHeaderTree.instance = new EnemyHeaderTree("script/enemy/", "root");
                }

                return EnemyHeaderTree.instance;
            }
        }

        /// <summary>
        /// ヘッダー
        /// </summary>
        public IReadOnlyList<EnemyHeader> Header
        {
            get;
            private set;
        }

        /// <summary>
        /// ツリー
        /// </summary>
        public IReadOnlyList<EnemyHeaderTree> Tree
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        private EnemyHeaderTree(string dir, string name)
        {
            this.Name = Path.GetFileName(dir);

            var header = new List<EnemyHeader>();
            var tree = new List<EnemyHeaderTree>();

            var files = Directory.GetFiles(dir);
            foreach (string file in files)
            {
                header.Add(new EnemyHeader(file));
            }

            var dirs = Directory.GetDirectories(dir);
            foreach (string dir2 in dirs)
            {
                tree.Add(new EnemyHeaderTree(dir2, Path.GetFileName(Path.GetDirectoryName(dir))));
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
        public string script
        {
            get;
            private set;
        }
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
            this.script = path;
            this.name = h["NAME"];
            this.hp = int.Parse(h["HP"]);
            this.image = h["IMAGE"];
            this.r = double.Parse(h["R"]);
            this.className = h["CLASS"];

            //Script.Instance.lua.DoString(src);
            Script.Instance.lua.DoFile(path);
        }
    }
}