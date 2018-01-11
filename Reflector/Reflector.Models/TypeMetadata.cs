using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using System.Runtime.Serialization;

namespace Reflector.Models
{
    [DataContract(IsReference = true)]
    public class TypeMetadata : IReflectionElement
    {
        #region db
        public TypeMetadata()
        {

        }
        public virtual List<VarMetadata> Vars { get; set; }
        public virtual List<VarMetadata> VarAttributes { get; set; }
        public virtual List<MethodMetadata> MethodsParents { get; set; }

        public virtual List<TypeMetadata> AttributesParent { get; set; }
        public virtual List<TypeMetadata> GenericArgumentsParent { get; set; }
        public virtual List<TypeMetadata> ImplementedInterfacesParent { get; set; }
        public virtual List<TypeMetadata> NestedTypesParent { get; set; }

        public int Id { get; set; } 
        #endregion

        public virtual NamespaceMetadata NamespaceMetadata { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public List<MethodMetadata> Methods { get { return _methods; } set { _methods = value; } }
        [DataMember]
        public List<VarMetadata> Fields { get { return _fields; } set { _fields = value; } }
        [DataMember]
        public List<VarMetadata> Properties { get { return _properties; } set { _properties = value; } }
        [DataMember]
        public List<TypeMetadata> NestedTypes { get { return _nestedTypes; } set { _nestedTypes = value; } }
        [DataMember]
        public List<TypeMetadata> ImplementedInterfaces { get { return _implementedInterfaces; } set { _implementedInterfaces = value; } }
        [DataMember]
        public List<TypeMetadata> Attributes { get { return _attributes; } set { _attributes = value; } }
        [DataMember]
        public TypeMetadata BaseType { get; set; }
        [DataMember]
        public List<TypeMetadata> GenericArguments { get { return _genericArguments; } set { _genericArguments = value; } }

        internal TypeMetadata(Type type, NamespaceMetadata _namespace)
        {
            LoadItself(type, _namespace);
            NamespaceMetadata = _namespace;
            Name = type.Name;
        }

        internal void LoadItself(Type type, NamespaceMetadata _namespace)
        {
            LoadFields(type, _namespace);
            LoadMethods(type, _namespace);
            LoadProperties(type, _namespace);
            LoadAttributes(type, _namespace);
            LoadNestedTypes(type, _namespace);
            LoadImplementedInterfaces(type, _namespace);
            LoadBaseType(type, _namespace);
            LoadGenericArguments(type, _namespace);
        }

        #region Privates
        private List<MethodMetadata> _methods = new List<MethodMetadata>();
        private List<VarMetadata> _fields = new List<VarMetadata>();
        private List<VarMetadata> _properties = new List<VarMetadata>();
        private List<TypeMetadata> _nestedTypes = new List<TypeMetadata>();
        private List<TypeMetadata> _implementedInterfaces = new List<TypeMetadata>();
        private List<TypeMetadata> _attributes = new List<TypeMetadata>();
        private List<TypeMetadata> _genericArguments = new List<TypeMetadata>();

        private void LoadGenericArguments(Type type, NamespaceMetadata _namespace)
        {
            foreach (Type genericArgument in type.GetGenericArguments())
            {
                GenericArguments.Add(_namespace.TryDefineTypeModel(genericArgument));
            }
        }

        private void LoadBaseType(Type type, NamespaceMetadata _namespace)
        {
            var baseType = type.BaseType;
            if (baseType != null && baseType != typeof(Object))
            {
                BaseType = _namespace.TryDefineTypeModel(baseType);
            }
        }

        private void LoadFields(Type type, NamespaceMetadata _namespace)
        {
            foreach (FieldInfo field in type.GetFields())
            {
                VarMetadata t = new VarMetadata() { Name = field.Name, Type = _namespace.TryDefineTypeModel(field.FieldType) };
                t.LoadAttributes(field.GetCustomAttributes(), _namespace);
                Fields.Add(t);
            }
        }

        private void LoadMethods(Type type, NamespaceMetadata _namespace)
        {
            foreach (MethodInfo method in type.GetMethods())
            {
                MethodMetadata m = new MethodMetadata();
                m.LoadItself(method, _namespace);
                Methods.Add(m);
            }
        }

        private void LoadProperties(Type type, NamespaceMetadata _namespace)
        {
            foreach (PropertyInfo property in type.GetProperties())
            {
                VarMetadata p = new VarMetadata() { Name = property.Name, Type = _namespace.TryDefineTypeModel(property.PropertyType) };
                p.LoadAttributes(property.GetCustomAttributes(), _namespace);
                Properties.Add(p);
            }
        }

        private void LoadNestedTypes(Type type, NamespaceMetadata _namespace)
        {
            foreach (TypeInfo nestedType in type.GetNestedTypes())
            {
                NestedTypes.Add(_namespace.TryDefineTypeModel(nestedType.GetType()));
            }
        }

        private void LoadImplementedInterfaces(Type type, NamespaceMetadata _namespace)
        {
            foreach (TypeInfo imlementedInterface in type.GetInterfaces())
            {
                ImplementedInterfaces.Add(_namespace.TryDefineTypeModel(imlementedInterface));
            }
        }

        private void LoadAttributes(Type type, NamespaceMetadata _namespace)
        {
            foreach (Attribute attribute in type.GetCustomAttributes())
            {
               Attributes.Add(_namespace.TryDefineTypeModel(attribute.GetType()));
            }
        }
        #endregion

    }
}
