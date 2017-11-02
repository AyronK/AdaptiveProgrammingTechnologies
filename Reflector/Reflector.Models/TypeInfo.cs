//using MVVMProject.Model.Reflection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace Reflector.Models
{
    [DataContract(IsReference = true)]
    public class TypeInfo : ReflectionElement
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
        public List<VarModel> NestedTypes { get { return _nestedTypes; } private set { _nestedTypes = value; } }
        [DataMember]
        public List<VarModel> ImplementedInterfaces { get { return _implementedInterfaces; } private set { _implementedInterfaces = value; } }
        public List<VarModel> Attributes { get { return _attributes; } private set { _attributes = value; } }

        public TypeInfo()
        {
        }
        public TypeInfo(Type type, NamespaceInfo _namespace)
        {
            Name = type.Name;
            LoadItself(type, _namespace);
        }

        internal void LoadItself(Type type, NamespaceInfo _namespace)
        {
            LoadFields(type, _namespace);
            LoadMethods(type, _namespace);
            LoadProperties(type, _namespace);
            LoadAttributes(type, _namespace);
            LoadNestedTypes(type, _namespace);
        }

        private void LoadFields(Type type, NamespaceInfo _namespace)
        {
            foreach (FieldInfo field in type.GetFields())
            {
                _namespace.TryDefineTypeModel(field.FieldType);
                VarModel t = new VarModel() { Name = field.Name, Type = _namespace.TypesAlreadyDefined.Find(f => f.Name == field.FieldType.Name)};
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
                _namespace.TryDefineTypeModel(property.PropertyType);
                VarModel p = new VarModel() { Name = property.Name, Type = _namespace.TypesAlreadyDefined.Find(f => f.Name == property.PropertyType.Name) };
                // VarModel p = new VarModel() { Name = property.Name, BaseType = new TypeModel() { TypeName = property.PropertyType.Name } };
                Properties.Add(p);
            }
        }

        private void LoadNestedTypes(Type type, NamespaceInfo _namespace)
        {
            foreach (System.Reflection.TypeInfo nestedType in type.GetNestedTypes())
            {
                _namespace.TryDefineTypeModel(nestedType.GetType());
                VarModel n = new VarModel() { Name = nestedType.Name, Type = _namespace.TypesAlreadyDefined.Find(f => f.Name == nestedType.Name) };
                NestedTypes.Add(n);
            }
        }

        /* private void LoadImplementedInterfaces(Type type, AssemblyInfo assembly)
         {
             foreach (var implementedInterface in type.i
                 property in type.GetProperties())
             {
                 assembly.TryDefineTypeModel(property.PropertyType);
                 VarModel p = new VarModel() { Name = property.Name, BaseType = assembly.Classes[property.PropertyType.Name] };
                 // VarModel p = new VarModel() { Name = property.Name, BaseType = new TypeModel() { TypeName = property.PropertyType.Name } };
                 Properties.Add(p);
             }
         } */

        private void LoadAttributes(Type type, NamespaceInfo _namespace)
        {
            foreach (Attribute attribute in type.GetCustomAttributes())
            {
                _namespace.TryDefineTypeModel(attribute.GetType());
                VarModel n = new VarModel() { Name = attribute.GetType().Name, Type = _namespace.TypesAlreadyDefined.Find(f => f.Name == attribute.GetType().Name) };
                Attributes.Add(n);
            }
        }

        #region Privates
        private List<MethodModel> _methods = new List<MethodModel>();
        private List<VarModel> _fields = new List<VarModel>();
        private List<VarModel> _properties = new List<VarModel>();
        private List<VarModel> _nestedTypes = new List<VarModel>();
        private List<VarModel> _implementedInterfaces = new List<VarModel>();
        private List<VarModel> _attributes = new List<VarModel>();
        #endregion

    }
}
