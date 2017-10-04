using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Reflector.Models
{
    [Serializable]
    public class VarModel : IExpandable
    {
        public string Name { get; set; }

        // Nie działa serializacja ale działa rekurencyjne drzewo
        [XmlIgnore]
        public TypeModel BaseType { get; set; }     

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
