using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace Reflector.Models
{
    [DataContract(IsReference = true)]
    public class NamespaceInfo : IReflectionElement
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
                _classes.Add(TryDefineTypeModel(type));
            }
        }

        [DataMember]
        internal List<TypeInfo> TypesAlreadyDefined = new List<TypeInfo>();

        internal TypeInfo TryDefineTypeModel(Type type)
        {
            string name;
            if (type.IsGenericType)
            {
                StringBuilder output = new StringBuilder();
                
                output.Append(type.Name);
                output.Remove(output.Length - 2, 2);
                if (type.GetGenericArguments().Length > 0)
                {
                    output.Append("<");
                    foreach (var genericArgument in type.GetGenericArguments())
                        output.Append($"{genericArgument.Name}, ");
                    output.Remove(output.Length - 2, 2);
                    output.Append(">");
                }
                name = output.ToString();
            }
            else
            {
                name = type.Name;
            }
            if (TypesAlreadyDefined.Find(t => t.Name == name) == null)
            {
                TypeInfo classModel = new TypeInfo() { Name = name };
                TypesAlreadyDefined.Add(classModel);
                classModel.LoadItself(type, this);
                return classModel;
            }            
            return TypesAlreadyDefined.First(t => t.Name == name);
        }

        #region Privates
        private List<TypeInfo> _classes = new List<TypeInfo>();
        #endregion
    }
}
