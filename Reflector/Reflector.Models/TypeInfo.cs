//using MVVMProject.Model.Reflection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace Reflector.Models
{
    [DataContract(IsReference = true)]
    public class TypeInfo : IExpandable
    {
        [DataMember]
        public string TypeName { get; set; }
        [DataMember]
        public List<MethodModel> Methods { get { return _methods; } private set { _methods = value; } }
        [DataMember]
        public List<VarModel> Fields { get { return _fields; } private set { _fields = value; } }
        [DataMember]
        public List<VarModel> Properties { get { return _properties; } private set { _properties = value; } }

        public TypeInfo()
        {
        }
        public TypeInfo(Type type, AssemblyInfo assembly)
        {
            TypeName = type.Name;
            LoadItself(type, assembly);
        }

        internal void LoadItself(Type type, AssemblyInfo assembly)
        {
            LoadFields(type, assembly);
            LoadMethods(type, assembly);
            LoadProperties(type, assembly);
        }

        private void LoadFields(Type type, AssemblyInfo assembly)
        {
            foreach(FieldInfo field in type.GetFields())
            {
                assembly.TryDefineTypeModel(field.FieldType);
                VarModel t = new VarModel() { Name = field.Name, BaseType = assembly.Classes[field.FieldType.Name] };
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
                VarModel p = new VarModel() { Name = property.Name, BaseType = assembly.Classes[property.PropertyType.Name] };
                // VarModel p = new VarModel() { Name = property.Name, BaseType = new TypeModel() { TypeName = property.PropertyType.Name } };
                Properties.Add(p);
            }
        }

        #region Object override
        public override string ToString()
        {
            return TypeName;
        }
        #endregion

        #region IExpandable implementation
        public IEnumerable<IExpandable> Expand()
        {
            List<IExpandable> children = new List<IExpandable>();
            children.AddRange(Fields);
            children.AddRange(Properties);
            children.AddRange(Methods);
            return children;
        }
        #endregion

        #region Privates
        private List<MethodModel> _methods = new List<MethodModel>();
        private List<VarModel> _fields = new List<VarModel>();       
        private List<VarModel> _properties = new List<VarModel>();
        #endregion

    }
}
