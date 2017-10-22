using Reflector.Models;
using Reflector.Presentation;
using System.Collections.ObjectModel;

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
