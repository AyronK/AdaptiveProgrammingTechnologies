//using MVVMProject.Model.Reflection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace Reflector.Models
{
    [DataContract(IsReference = true)]
    public class TypeInfo : IReflectionElement
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public List<MethodModel> Methods { get { return _methods; } private set { _methods = value; } }
        [DataMember]
        public List<VarModel> Fields { get { return _fields; } private set { _fields = value; } }
        [DataMember]
        public List<VarModel> Properties { get { return _properties; } private set { _properties = value; } }
        [DataMember]
        public List<TypeInfo> NestedTypes { get { return _nestedTypes; } private set { _nestedTypes = value; } }
        [DataMember]
        public List<TypeInfo> ImplementedInterfaces { get { return _implementedInterfaces; } private set { _implementedInterfaces = value; } }
        [DataMember]
        public List<TypeInfo> Attributes { get { return _attributes; } private set { _attributes = value; } }
        [DataMember]
        public TypeInfo BaseType;
        [DataMember]
        public List<TypeInfo> GenericArguments { get { return _genericArguments; } private set { _genericArguments = value; } }

        public TypeInfo()
        {
        }
        public TypeInfo(Type type, NamespaceInfo _namespace)
        {
            LoadItself(type, _namespace);
            Name = type.Name;
        }

        internal void LoadItself(Type type, NamespaceInfo _namespace)
        {
            LoadFields(type, _namespace);
            LoadMethods(type, _namespace);
            LoadProperties(type, _namespace);
            LoadAttributes(type, _namespace);
            LoadNestedTypes(type, _namespace);
            LoadImplementedInterfaces(type, _namespace);
            LoadBaseType(type, _namespace);            
        }

        private void LoadGenericArguments(Type type, NamespaceInfo _namespace)
        {
            foreach (Type genericArgument in type.GetGenericArguments())
            {                
                GenericArguments.Add(_namespace.TryDefineTypeModel(genericArgument));
            }
        }

        private void LoadBaseType(Type type, NamespaceInfo _namespace)
        {
            var baseType = type.BaseType;
            if (baseType != null && baseType != typeof(Object))
            {
                BaseType = _namespace.TryDefineTypeModel(baseType);
            }
        }

        private void LoadFields(Type type, NamespaceInfo _namespace)
        {
            foreach (FieldInfo field in type.GetFields())
            {               
                VarModel t = new VarModel() { Name = field.Name, Type = _namespace.TryDefineTypeModel(field.FieldType)};
                t.LoadAttributes(field.GetCustomAttributes(), _namespace);
                Fields.Add(t);
            }
        }

        private void LoadMethods(Type type, NamespaceInfo _namespace)
        {
            foreach (MethodInfo method in type.GetMethods())
            {
                MethodModel m = new MethodModel();
                m.LoadItself(method, _namespace);
                Methods.Add(m);
            }
        }

        private void LoadProperties(Type type, NamespaceInfo _namespace)
        {
            foreach (PropertyInfo property in type.GetProperties())
            {                
                VarModel p = new VarModel() { Name = property.Name, Type = _namespace.TryDefineTypeModel(property.PropertyType)};
                p.LoadAttributes(property.GetCustomAttributes(), _namespace);
                Properties.Add(p);
            }
        }

        private void LoadNestedTypes(Type type, NamespaceInfo _namespace)
        {
            foreach (System.Reflection.TypeInfo nestedType in type.GetNestedTypes())
            {               
                NestedTypes.Add(_namespace.TryDefineTypeModel(nestedType.GetType()));
            }
        }

        private void LoadImplementedInterfaces(Type type, NamespaceInfo _namespace)
        {
            foreach (System.Reflection.TypeInfo imlementedInterface in type.GetInterfaces())
            {                
                ImplementedInterfaces.Add(_namespace.TryDefineTypeModel(imlementedInterface));
            }
        }

        private void LoadAttributes(Type type, NamespaceInfo _namespace)
        {
            foreach (Attribute attribute in type.GetCustomAttributes())
            {
                Attributes.Add(_namespace.TryDefineTypeModel(attribute.GetType()));
            }
        }

        #region Privates
        private List<MethodModel> _methods = new List<MethodModel>();
        private List<VarModel> _fields = new List<VarModel>();
        private List<VarModel> _properties = new List<VarModel>();
        private List<TypeInfo> _nestedTypes = new List<TypeInfo>();
        private List<TypeInfo> _implementedInterfaces = new List<TypeInfo>();
        private List<TypeInfo> _attributes = new List<TypeInfo>();
        private List<TypeInfo> _genericArguments = new List<TypeInfo>();
        #endregion

    }
}
