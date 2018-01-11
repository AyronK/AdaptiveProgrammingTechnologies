using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Reflector.Models
{
    [DataContract(IsReference = true)]
    public class VarMetadata : IReflectionElement
    {
        #region db
        public virtual TypeMetadata PropertyParent { get; set; }
        public virtual TypeMetadata FieldParent { get; set; }
        public virtual MethodMetadata ParameterParent { get; set; }
        public int Id { get; set; } 
        #endregion

        [DataMember]
        public virtual List<TypeMetadata> Attributes { get { return _attributes; } set { _attributes = value; } }
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public virtual TypeMetadata Type { get; set; }

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
