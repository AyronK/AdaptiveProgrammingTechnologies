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
        public string Name { get; set; }

        [DataMember]
        public TypeInfo Type { get; set; }     
        
    }
}
