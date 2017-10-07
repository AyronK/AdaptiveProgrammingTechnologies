using Reflector.GUI.Models;
using Reflector.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reflector.GUI.ViewModels
{
    public class AssemblyTreeModel : ObservableCollection<TreeViewNode>
    {
        public AssemblyTreeModel(AssemblyInfo assemblyInfo)
        {
            foreach (NamespaceInfo namespaces in assemblyInfo.Namespaces)
            {
                TreeViewNode n = new TreeViewNode(namespaces);
                Add(n);
            }
        }
    }
}
