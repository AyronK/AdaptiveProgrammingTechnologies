using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace Reflector.Models
{
    [DataContract(IsReference = true)]
    public class MethodMetadata : IReflectionElement
    {
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public List<string> Modifiers { get { return _modifiers; } private set { _modifiers = value; } }
        [DataMember]
        public List<VarMetadata> Parameters { get { return _parameters; } private set { _parameters = value; } }
        //[DataMember]
        //public TypeMetadata ReturnType { get { return _returnType; } set { _returnType = value; } }
        [DataMember]
        public List<TypeMetadata> Attributes { get { return _attributes; } private set { _attributes = value; } }

        internal void LoadItself(MethodInfo method, NamespaceMetadata _namespace)
        {
            Name = method.Name;
            LoadModifiers(method);
            LoadAttributes(method, _namespace);
            
            //ReturnType = _namespace.TryDefineTypeModel(method.ReturnType);

            foreach (ParameterInfo parameter in method.GetParameters())
            {
                AddParameter(_namespace, parameter);
            }
        }

        #region Privates
        private List<VarMetadata> _parameters = new List<VarMetadata>();
        private List<string> _modifiers = new List<string>();
        private TypeMetadata _returnType;
        private List<TypeMetadata> _attributes = new List<TypeMetadata>();

        private void LoadModifiers(MethodInfo method)
        {
            //if (method.IsAbstract) Modifiers.Add("abstract");
            //if (method.IsFinal) Modifiers.Add("final");
            //if (method.IsPrivate) Modifiers.Add("private");
            //if (method.IsPublic) Modifiers.Add("public");
            //if (method.IsStatic) Modifiers.Add("static");
            //if (method.IsVirtual) Modifiers.Add("virtual");
        }

        private void AddParameter(NamespaceMetadata _namespace, ParameterInfo parameter)
        {
            string typeName = parameter.ParameterType.Name;
            VarMetadata p = new VarMetadata()
            {
                Name = parameter.Name,
                Type = _namespace.TryDefineTypeModel(parameter.ParameterType)
            };
            p.LoadAttributes(parameter.GetCustomAttributes(), _namespace);

            Parameters.Add(p);
        }

        private void LoadAttributes(MethodInfo method, NamespaceMetadata _namespace)
        {
            foreach (Attribute attribute in method.GetCustomAttributes())
            {
                Attributes.Add(_namespace.TryDefineTypeModel(attribute.GetType()));
            }
        }
        #endregion
    }
}
