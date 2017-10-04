using System;
using System.Collections.Generic;

namespace Reflector.Models
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

        internal void LoadClasses(System.Reflection.Assembly assembly, AssemblyInfo assemblyModel)
        {
            foreach (Type type in assembly.GetTypes())
            {
                AddClass(assemblyModel, type);
            }
        }

        private void AddClass(AssemblyInfo assemblyModel, Type type)
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
