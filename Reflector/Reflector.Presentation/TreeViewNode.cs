using Reflector.Models;
using Reflector.Presentation.ViewModels;
using System.Collections.Generic;

namespace Reflector.Presentation
{
    public class TreeViewNode
    {
        public string Name { get; private set; }
        private ReflectionElement Expandable { get; set; }
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
            if (Expandable.GetChildren() != null)
                foreach (ReflectionElement expandable in Expandable.GetChildren())
                {
                    AddSublevel(new TreeViewNode(expandable));
                }
        }

        public TreeViewNode(ReflectionElement expandable)
        {
            Expandable = expandable;
            Name = Expandable.GetDescription();
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
