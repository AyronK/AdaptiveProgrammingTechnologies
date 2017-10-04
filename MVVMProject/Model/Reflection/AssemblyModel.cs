using MVVMProject.Model.Reflection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;

namespace MVVMProject.Model
{
    [Serializable]
    public class AssemblyModel : IExpandable
    {
        #region Constructors       
        private AssemblyModel() { }  // Only for serialization purpose

        public AssemblyModel(Assembly assembly)
        {
            _assembly = assembly;
            Name = _assembly.GetName().Name;
            LoadNamespaces();
        }

        public AssemblyModel(string path) : this(Assembly.LoadFile(path)) { }
        #endregion
        public string Name { get; set; }

        public List<NamespaceModel> Namespaces { get { return _namespaces; } }        

        private void LoadNamespaces()
        {
            List<string> namespacesNames = GetNamespacesNames();

            foreach (string namespaceName in namespacesNames)
            {
                AddNamespace(namespaceName);
            }
        }

        private void AddNamespace(string namespaceName)
        {
            NamespaceModel namespaceModel = new NamespaceModel(namespaceName);
            namespaceModel.LoadClasses(_assembly, this);
            Namespaces.Add(namespaceModel);
        }

        private List<string> GetNamespacesNames()
        {
            List<string> names = new List<string>();
            foreach (Type type in _assembly.GetTypes())
                if (!names.Contains(type.Namespace))
                    names.Add(type.Namespace);
            return names;
        }

        #region Internal
        internal Dictionary<string, TypeModel> Classes = new Dictionary<string, TypeModel>();
        internal void TryDefineTypeModel(Type type)
        {
            if (!Classes.ContainsKey(type.Name))
            {
                TypeModel classModel = new TypeModel() { TypeName = type.Name };
                Classes.Add(type.Name, classModel);
                classModel.LoadItself(type, this);
            }
        }
        #endregion

        #region IExpandable implementation
        public IEnumerable<IExpandable> Expand()
        {
            return Namespaces;
        }
        #endregion

        #region Privates
        private List<NamespaceModel> _namespaces = new List<NamespaceModel>();
        private Dictionary<string, TypeModel> _classes = new Dictionary<string, TypeModel>();
        [XmlIgnore]
        private Assembly _assembly;
        #endregion

        #region Object override
        public override string ToString()
        {
            return Name;
        } 
        #endregion

    }
}
