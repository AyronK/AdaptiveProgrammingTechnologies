﻿//using MVVMProject.Model.Reflection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Reflector.Models
{
    [DataContract(IsReference = true)]
    public class MethodModel : IReflectionElement
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public List<string> Modifiers { get { return _modifiers; } private set { _modifiers = value; } }
        [DataMember]
        public List<VarModel> Parameters { get { return _parameters; } private set { _parameters = value; } }
        [DataMember]
        public TypeInfo ReturnType { get { return _returnType; } set { _returnType = value; } }
        [DataMember]
        public List<TypeInfo> Attributes { get { return _attributes; } private set { _attributes = value; } }

        internal void LoadItself(MethodInfo method, NamespaceInfo _namespace)
        {
            Name = method.Name;
            LoadModifiers(method);
            LoadAttributes(method, _namespace);

            //ReturnType = new TypeModel() { TypeName = method.ReturnType.Name }; 
            _namespace.TryDefineTypeModel(method.ReturnType);
            ReturnType = _namespace.TypesAlreadyDefined.Find(f => f.Name == method.ReturnType.Name);

            foreach (ParameterInfo parameter in method.GetParameters())
            {
                AddParameter(_namespace, parameter);
            }
        }

        private void AddParameter(NamespaceInfo _namespace, ParameterInfo parameter)
        {
            string typeName = parameter.ParameterType.Name;

            /*if (!assembly.Classes.ContainsKey(typeName))
             {
                 TypeModel classModel = new TypeModel() { TypeName = typeName };
                 assembly.Classes.Add(typeName, classModel);
             }
             VarModel p = new VarModel() { Name = parameter.Name, BaseType = assembly.Classes[typeName] };*/

            _namespace.TryDefineTypeModel(parameter.ParameterType);
            VarModel p = new VarModel() { Name = parameter.Name, Type = _namespace.TypesAlreadyDefined.Find(f => f.Name == typeName) };
            p.LoadAttributes(parameter.GetCustomAttributes(), _namespace);

            Parameters.Add(p);
        }

        private void LoadAttributes(MethodInfo method, NamespaceInfo _namespace)
        {
            foreach (Attribute attribute in method.GetCustomAttributes())
            {
                _namespace.TryDefineTypeModel(attribute.GetType());
                Attributes.Add(_namespace.TypesAlreadyDefined.Find(f => f.Name == attribute.GetType().Name));
            }
        }


        #region Privates
        private List<VarModel> _parameters = new List<VarModel>();
        private List<string> _modifiers = new List<string>();
        private TypeInfo _returnType;
        private List<TypeInfo> _attributes = new List<TypeInfo>();
        private void LoadModifiers(MethodInfo method)
        {
            if (method.IsAbstract) Modifiers.Add("abstract");
            if (method.IsFinal) Modifiers.Add("final");
            if (method.IsPrivate) Modifiers.Add("private");
            if (method.IsPublic) Modifiers.Add("public");
            if (method.IsStatic) Modifiers.Add("static");
            if (method.IsVirtual) Modifiers.Add("virtual");
        }
        #endregion
    }
}
