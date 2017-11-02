using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace Reflector.Models
{
    [DataContract(IsReference = true)]
    public class NamespaceMetadata : IReflectionElement
    {
        #region Constructors
        public NamespaceMetadata(string name, IEnumerable<Type> types, AssemblyMetadata assembly)
        {
            Name = name;
            Classes = from type in types orderby type.Name select new TypeMetadata(type, this);
        }
        #endregion

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public IEnumerable<TypeMetadata> Classes
        {
            get; private set;
        }

        internal void LoadClasses(Assembly assembly, AssemblyMetadata assemblyModel)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.Namespace == Name)
                {
                    _classes.Add(TryDefineTypeModel(type));
                }
            }
        }

        #region Recursion Protection
        [DataMember]
        internal List<TypeMetadata> TypesAlreadyDefined = new List<TypeMetadata>();

        internal TypeMetadata TryDefineTypeModel(Type type)
        {
            string name;
            if (type.IsGenericType)
            {
                name = GetGenericName(type);
            }
            else
            {
                name = type.Name;
            }
            if (TypesAlreadyDefined.Find(t => t.Name == name) == null)
            {
                TypeMetadata classModel = new TypeMetadata() { Name = name };
                TypesAlreadyDefined.Add(classModel);
                classModel.LoadItself(type, this);
                return classModel;
            }
            return TypesAlreadyDefined.First(t => t.Name == name);
        }

        private static string GetGenericName(Type type)
        {
            string name;
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
            return name;
        }
        #endregion

        #region Privates
        private List<TypeMetadata> _classes = new List<TypeMetadata>();
        #endregion
    }
}
