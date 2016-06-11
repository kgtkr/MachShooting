using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;

namespace MachShooting
{
    public class EnemyHeaderTree
    {
        private static EnemyHeaderTree instance;

        public static EnemyHeaderTree Instance
        {
            get
            {
                if (EnemyHeaderTree.instance == null)
                {
                    EnemyHeaderTree.instance = new EnemyHeaderTree("enemy/","root");
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

        private EnemyHeaderTree(string dir,string name)
        {
            var header = new List<EnemyHeader>();
            var tree = new List<EnemyHeaderTree>();

            var files = Directory.GetFiles(dir);
            foreach(string file in files)
            {
                header.Add(new EnemyHeader(file));
            }

            var dirs = Directory.GetDirectories(dir);
            foreach (string dir2 in dirs)
            {
                tree.Add(new EnemyHeaderTree(dir2,Path.GetFileName(Path.GetDirectoryName(dir))));
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
        public string className{
            get;
            private set;
        }

        public EnemyHeader(string path)
        {
            var h=Config.ReadINI(path);
            this.script = h["SCRIPT"];
            this.name = h["NAME"];
            this.hp = int.Parse(h["HP"]);
            this.image = h["IMAGE"];
            this.r = double.Parse(h["R"]);
            this.className = h["CLASS"];
        }
    }
}
