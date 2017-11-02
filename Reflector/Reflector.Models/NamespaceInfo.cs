using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Reflector.Models
{
    [DataContract(IsReference = true)]
    public class NamespaceInfo : ReflectionElement
    {
        #region Constructors
        public NamespaceInfo(string name, IEnumerable<Type> types, AssemblyInfo assembly)
        {
            Name = name;
            Classes = from type in types orderby type.Name select new TypeInfo(type, this);
        }
        #endregion

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public IEnumerable<TypeInfo> Classes
        {
            get; private set;
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
                TryDefineTypeModel(type);
                _classes.Add(TypesAlreadyDefined.Find(t => t.Name == type.Name));
            }
        }

        [DataMember]
        internal List<TypeInfo> TypesAlreadyDefined = new List<TypeInfo>();

        internal void TryDefineTypeModel(Type type)
        {
            if (TypesAlreadyDefined.Find(t => t.Name == type.Name) == null)
            {
                TypeInfo classModel = new TypeInfo() { Name = type.Name };
                TypesAlreadyDefined.Add(classModel);
                classModel.LoadItself(type, this);
            }
        }

        #region Privates
        private List<TypeInfo> _classes = new List<TypeInfo>();
        #endregion
    }
}
