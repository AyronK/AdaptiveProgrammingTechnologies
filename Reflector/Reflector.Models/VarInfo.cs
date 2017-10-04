using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Reflector.Models
{
    [DataContract(IsReference = true)]
    public class VarModel : IExpandable
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public TypeInfo BaseType { get; set; }     

        #region Object override
        public override string ToString()
        {
            return BaseType.ToString() + " " + Name;
        } 
        #endregion

        #region IExpandable implementation
        public IEnumerable<IExpandable> Expand()
        {
            return BaseType.Expand();
        } 
        #endregion
    }
}
