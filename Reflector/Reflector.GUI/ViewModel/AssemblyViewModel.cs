using Reflector.GUI.Model;
using Reflector.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reflector.GUI.ViewModel
{
    class AssemblyViewModel : ObservableCollection<TreeViewNode>
    {
        public AssemblyViewModel(AssemblyInfo assemblyInfo)
        {
            foreach (NamespaceModel namespaces in assemblyInfo.Namespaces)
            {
                TreeViewNode n = new TreeViewNode(namespaces);
                Add(n);
            }
        }
    }
}
