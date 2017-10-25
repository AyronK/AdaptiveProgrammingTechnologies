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
        public List<TreeViewNode> Sublevels { get; private set; }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                if (_wasBuilt)
                    return;
                Sublevels.Clear();
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
                    AddSublevel(new TreeViewNode(expandable));
                }
        }

        public TreeViewNode(IExpandable expandable)
        {
            Expandable = expandable;
            Name = Expandable.ToString();
            Sublevels = new List<TreeViewNode>();
            Sublevels.Add(null); // Adding null makes sublevel never empty, so view libraries can display it as expandable
            _wasBuilt = false;
        }

        private void AddSublevel(TreeViewNode _treeNode)
        {
            Sublevels.Add(_treeNode);
        }
    }
}
