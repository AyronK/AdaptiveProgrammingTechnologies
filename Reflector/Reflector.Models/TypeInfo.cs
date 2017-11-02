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
        public TypeInfo(Type type, AssemblyInfo assembly)
        {
            Name = type.Name;
            LoadItself(type, assembly);
        }

        internal void LoadItself(Type type, AssemblyInfo assembly)
        {
            LoadFields(type, assembly);
            LoadMethods(type, assembly);
            LoadProperties(type, assembly);
            LoadAttributes(type, assembly);
            LoadNestedTypes(type, assembly);
        }

        private void LoadFields(Type type, AssemblyInfo assembly)
        {
            foreach(FieldInfo field in type.GetFields())
            {
                assembly.TryDefineTypeModel(field.FieldType);
                VarModel t = new VarModel() { Name = field.Name, Type = assembly.Classes[field.FieldType.Name] };
                Fields.Add(t);
            }
        }

        private void LoadMethods(Type type, AssemblyInfo assembly)
        {
            foreach (MethodInfo method in type.GetMethods())
            {
                MethodModel m = new MethodModel();
                m.LoadItself(method, assembly);
                Methods.Add(m);
            }
        }

        private void LoadProperties(Type type, AssemblyInfo assembly)
        {
            foreach (PropertyInfo property in type.GetProperties())
            {
                assembly.TryDefineTypeModel(property.PropertyType);
                VarModel p = new VarModel() { Name = property.Name, Type = assembly.Classes[property.PropertyType.Name] };
                // VarModel p = new VarModel() { Name = property.Name, BaseType = new TypeModel() { TypeName = property.PropertyType.Name } };
                Properties.Add(p);
            }
        }

        private void LoadNestedTypes(Type type, AssemblyInfo assembly)
        {
            foreach (System.Reflection.TypeInfo nestedType in type.GetNestedTypes())
            {
                assembly.TryDefineTypeModel(nestedType.GetType());
                VarModel n = new VarModel() { Name = nestedType.Name, Type = assembly.Classes[nestedType.Name] };
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

        private void LoadAttributes(Type type, AssemblyInfo assembly)
        {
            foreach (Attribute attribute in type.GetCustomAttributes())
            {
                assembly.TryDefineTypeModel(attribute.GetType());
                VarModel n = new VarModel() { Name = attribute.GetType().Name, Type = assembly.Classes[attribute.GetType().Name] };
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
