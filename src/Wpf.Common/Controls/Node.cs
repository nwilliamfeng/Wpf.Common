using System;
using System.Collections.Generic;
using System.Linq;
 
namespace Wpf.Common
{     
    public abstract class Node : ViewModelBase,IComparable<Node>
    {     
        private object _tag;
        public object Tag
        {
            get => _tag;
            set => this.Set(ref _tag, value);
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (value && this.Parent != null && this.Parent.IsSelected) return; //如果父节点已选中则取消
                this.Set(ref _isSelected, value);
            }
        }

        private bool _isVisible = true;
        public bool IsVisible
        {
            get => _isVisible;
            set => this.Set(ref _isVisible, value);
        }
 
        private bool _isExpand;
        public bool IsExpanded
        {
            get => _isExpand;
            set => this.Set(ref _isExpand, value);
        }
 
        public abstract NodeType NodeType { get; }
       
        public int Level
        {
            get
            {
                if (this.Parent == null) return 1;
                var parent = this.Parent;
                int level = 1;
                while (true)
                {
                    if (parent == null)
                        break;
                    level += 1;
                    parent = parent.Parent;
                }
                return level;
            }
        }

        private FolderNode _parent;
        public FolderNode Parent
        {
            get => _parent;
            internal set
            {
                if (object.ReferenceEquals(this._parent, value)) return;
                if (value == null)
                    throw new ArgumentNullException("parent is null");
                this._parent = value; 
            }
        }

        public string Dir => this.NodeType == NodeType.Folder ? this.FullPath : System.IO.Path.GetDirectoryName(this.FullPath);

        public string FullPath
        {
            get
            {
                var parent = this.Parent;
                if (parent == null)
                    return this.Name;
                return parent.FullPath + "\\" + this.Name;
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => this.Set(ref _name, value);
        }

        public override bool Equals(object obj)
        {
            if (obj is Node other)
                return string.Equals( this.Name, other.Name,StringComparison.InvariantCultureIgnoreCase);
            return false;
        }

        public override string ToString()=> this.FullPath;

        public override int GetHashCode()=> this.Name == null ? base.GetHashCode() : this.Name.GetHashCode();

        public void Expand()
        {
            this.IsExpanded = true;
            var parent = this.Parent;
            while (parent != null)
            {
                (parent as Node).IsExpanded = true;
                parent = parent.Parent;
            }             
        }

        public Node Root
        {
            get
            {
                if (this.Parent == null) return this;
                Node parent = this.Parent;
                while (true)
                {
                    if (parent.Parent == null)
                        break;
                    parent = parent.Parent;
                }
                return parent;
            }
        }

        public abstract Node Copy(string newName = null);

        public int CompareTo(Node other)
        {
            if (this.NodeType != other.NodeType)
                return this.NodeType.CompareTo(other.NodeType);
            return this.Name.CompareTo(other.Name);
        }
    }

}
