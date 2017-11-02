using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Reflector.Models
{
    [DataContract(IsReference = true)]
    public class AssemblyMetadata : IReflectionElement
    {
        #region Constructors       
        public AssemblyMetadata(System.Reflection.Assembly assembly)
        {
            Name = assembly.ManifestModule.Name;
            Namespaces = from Type _type in assembly.GetTypes()
                         where _type.IsPublic || _type.IsNestedPublic || _type.IsNestedFamily || _type.IsNestedFamANDAssem
                         group _type by _type.Namespace != null ? _type.Namespace : string.Empty into _group
                         orderby _group.Key
                         select new NamespaceMetadata(_group.Key, _group, this);
        }
        #endregion

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public IEnumerable<NamespaceMetadata> Namespaces { get; private set; }
    }
}
