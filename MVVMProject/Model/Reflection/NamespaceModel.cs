using MVVMProject.Model.Reflection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MVVMProject.Model
{
    [Serializable]
    public class NamespaceModel : IExpandable
    {
        #region Constructors
        public NamespaceModel(string name)
        {
            Name = name;
        }
                
        private NamespaceModel() : this("") { } // Only for serialization purpose
        #endregion

        public string Name { get; set; }

        public List<TypeModel> Classes
        {
            get { return _classes; }
        }

        internal void LoadClasses(Assembly assembly, AssemblyModel assemblyModel)
        {
            foreach (Type type in assembly.GetTypes())
            {
                AddClass(assemblyModel, type);
            }
        }

        private void AddClass(AssemblyModel assemblyModel, Type type)
        {
            if (type.Namespace == Name)
            {
                assemblyModel.TryDefineTypeModel(type);
                Classes.Add(assemblyModel.Classes[type.Name]);
            }
        }

        #region Object override
        public override string ToString()
        {
            return Name;
        } 
        #endregion

        #region IExpandable implementation
        public IEnumerable<IExpandable> Expand()
        {
            return Classes;
        }
        #endregion

        #region Privates
        private List<TypeModel> _classes = new List<TypeModel>(); 
        #endregion
    }
}
