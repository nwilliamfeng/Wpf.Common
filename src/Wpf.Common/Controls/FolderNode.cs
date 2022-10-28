using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.IO;
using System.Windows.Data;

namespace Wpf.Common
{
    [Serializable]
    public class FolderNode:Node
    {
        public FolderNode()
        {
            this.Items = CollectionViewSource.GetDefaultView(this._items);
            (this.Items as ICollectionViewLiveShaping).IsLiveSorting = true;
            this.Items.SortDescriptions.Add(new SortDescription(nameof(Node.Name),ListSortDirection.Ascending));
        }

        public FolderNode(DirectoryInfo directory):this()
        {
            this.Name = directory.Name;
            foreach (var file in directory.GetFiles())
                this.AddNode(new FileNode( file ));
            foreach (var dir in directory.GetDirectories())
                this.AddNode(new FolderNode(dir));
        }

        public void Clear()
        {
            this._items.Clear();
        }

        public override NodeType NodeType => NodeType.Folder;

        public override Node Copy(string newName = null)
        {
            var folder = new FolderNode
            {
                Name = string.IsNullOrEmpty(newName) ? this.Name : newName,

            };
            foreach (var item in this._items)
                folder._items.Add(item.Copy());
            return folder;
        }

        public void AddNode(Node node)
        {
            if (_items.Any(x => x.Equals(node))) return;
            this._items.Add(node);
            node.Parent = this;
            this.Items.Refresh();
        }

        public void RemoveNode(Node node)
        {
            this._items.Remove(node);
            this.Items.Refresh();
        }

        public Node FindItem(string fullName)
        {
            var item = this._items.FirstOrDefault(x => string.Equals(x.FullPath, fullName, StringComparison.InvariantCultureIgnoreCase));
            if (item != null)
                return item;
            foreach (FolderNode folder in this._items.Where(x => x.NodeType == NodeType.Folder).ToList())
            {
                var rst = folder.FindItem(fullName);
                if (rst != null)
                    return rst;
            }

            return null;
        }

        public Node FindItem(Func<Node, bool> func)
        {
            var item = this._items.FirstOrDefault(x => func(x));
            if (item != null)
                return item;
            foreach (FolderNode folder in this._items.Where(x => x.NodeType == NodeType.Folder).ToList())
            {
                var rst = folder.FindItem(x => func(x));
                if (rst != null)
                    return rst;
            }
            return null;
        }

        public IEnumerable<Node> GetAllNodes()
        {
            yield return this;
            foreach (Node item in this._items)
            {
                if(item.NodeType == NodeType.File)
                    yield return item;
                else
                foreach (var sitem in (item as FolderNode).GetAllNodes())
                    yield return sitem;
            }
        }
 
        private ObservableCollection<Node> _items = new ObservableCollection<Node>();

        public ICollectionView Items { get; private set; }
    }

}
