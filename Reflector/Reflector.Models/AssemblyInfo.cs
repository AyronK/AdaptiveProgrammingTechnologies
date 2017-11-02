using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Reflector.Models
{
    [DataContract(IsReference = true)]
    public class AssemblyInfo : ReflectionElement
    {
        #region Constructors       
        public AssemblyInfo(System.Reflection.Assembly assembly)
        {
            Name = assembly.ManifestModule.Name;
            Namespaces = from Type _type in assembly.GetTypes()
                         where _type.IsPublic || _type.IsNestedPublic || _type.IsNestedFamily || _type.IsNestedFamANDAssem
                         group _type by _type.Namespace != null ? _type.Namespace : string.Empty into _group
                         orderby _group.Key
                         select new NamespaceInfo(_group.Key, _group, this);
        }
        #endregion
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public IEnumerable<NamespaceInfo> Namespaces { get; private set; }

        #region Internal
        [DataMember]
        internal Dictionary<string, TypeInfo> Classes = new Dictionary<string, TypeInfo>();
        internal void TryDefineTypeModel(Type type)
        {
            if (!Classes.ContainsKey(type.Name))
            {
                TypeInfo classModel = new TypeInfo() { TypeName = type.Name };
                Classes.Add(type.Name, classModel);
                classModel.LoadItself(type, this);
            }
        }
        #endregion

        #region Privates
        private Dictionary<string, TypeInfo> _classes = new Dictionary<string, TypeInfo>();
        #endregion
    }
}
