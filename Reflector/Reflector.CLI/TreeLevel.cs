using Reflector.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reflector.CLI
{
    public class TreeLevel
    {
        public string Name { get; private set; }
        private IExpandable Expandable { get; set; }
        public Dictionary<string, TreeLevel> Sublevel { get; private set; }

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
                    AddNode(new TreeLevel(expandable));                    
                }
        }

        public TreeLevel(IExpandable expandable)
        {
            Expandable = expandable;
            Name = Expandable.ToString();
            Sublevel = new Dictionary<string, TreeLevel>();
            this._wasBuilt = false;
        }

        public void AddNode(TreeLevel _treeNode)
        {
            Sublevel.Add($"{Sublevel.Count}",_treeNode);
        }
    }
}
