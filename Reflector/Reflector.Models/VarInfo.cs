using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Reflector.Models
{
    [DataContract(IsReference = true)]
    public class VarModel : IReflectionElement
    {

        [DataMember]
        public List<TypeInfo> Attributes { get { return _attributes; } private set { _attributes = value; } }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public TypeInfo Type { get; set; }

        internal void LoadAttributes(IEnumerable<Attribute> attributes, NamespaceInfo _namespace)
        {
            foreach (Attribute attribute in attributes)
            {
                _namespace.TryDefineTypeModel(attribute.GetType());
                Attributes.Add(_namespace.TypesAlreadyDefined.Find(f => f.Name == attribute.GetType().Name));
            }
        }

        private List<TypeInfo> _attributes = new List<TypeInfo>();

    }
}
