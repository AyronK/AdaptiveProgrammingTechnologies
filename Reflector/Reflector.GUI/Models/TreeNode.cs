using Reflector.Models;
using System.Collections.ObjectModel;

namespace Reflector.GUI.Models
{
    class TreeViewNode
    {
        public string Name {get; private set;}
        private IExpandable Expandable { get; set; }
        public ObservableCollection<TreeViewNode> Sublevel { get; private set; }

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
            if(Expandable.Expand() != null)
            foreach (IExpandable expandable in Expandable.Expand())
            {
                AddNode(new TreeViewNode(expandable));
            }
        }

        public TreeViewNode(IExpandable expandable)
        {
            Expandable = expandable;
            Name = Expandable.ToString();
            Sublevel = new ObservableCollection<TreeViewNode>();
            Sublevel.Add(null);
            this._wasBuilt = false;
        }
        
        public void AddNode(TreeViewNode _treeNode)
        {
            Sublevel.Add(_treeNode);
        }        
    }
}
