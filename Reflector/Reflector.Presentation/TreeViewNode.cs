using Reflector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reflector.Presentation
{
    public class TreeViewNode
    {
        public string Name { get; private set; }
        private IExpandable Expandable { get; set; }
        public List<TreeViewNode> Sublevel { get; private set; }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                if (_wasBuilt)
                    return;
                Sublevel.Clear();
                buildMyself();
                _wasBuilt = true;
            }
        }

        private bool _wasBuilt;
        private bool _isExpanded;

        private void buildMyself()
        {
            if (Expandable.Expand() != null)
                foreach (IExpandable expandable in Expandable.Expand())
                {
                    AddNode(new TreeViewNode(expandable));
                }
        }

        public TreeViewNode(IExpandable expandable)
        {
            Expandable = expandable;
            Name = Expandable.ToString();
            Sublevel = new List<TreeViewNode>();
            this._wasBuilt = false;
        }

        public void AddNode(TreeViewNode _treeNode)
        {
            Sublevel.Add(_treeNode);
        }
    }
}
