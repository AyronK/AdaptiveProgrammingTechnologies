using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Reflector.Models
{
    [DataContract(IsReference = true)]
    public class VarMetadata : IReflectionElement
    {
        [DataMember]
        public List<TypeMetadata> Attributes { get { return _attributes; } private set { _attributes = value; } }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public TypeMetadata Type { get; set; }

        internal void LoadAttributes(IEnumerable<Attribute> attributes, NamespaceMetadata _namespace)
        {
            foreach (Attribute attribute in attributes)
            {
                Attributes.Add(_namespace.TryDefineTypeModel(attribute.GetType()));
            }
        }

        private List<TypeMetadata> _attributes = new List<TypeMetadata>();
    }
}
