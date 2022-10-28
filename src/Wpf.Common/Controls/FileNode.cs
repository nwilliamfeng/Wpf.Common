using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.Common
    [Serializable]
    public class FileNode:Node
    {
        public override NodeType NodeType => NodeType.File;

        public override Node Copy(string newName = null)
        {
            return new FileNode
            {
                Name = string.IsNullOrEmpty(newName) ? this.Name : newName,
            };

        }

        public FileNode()
        {

        }

        public FileNode(FileInfo file)
        {
            this.Name = file.Name;
        }
    }

}
