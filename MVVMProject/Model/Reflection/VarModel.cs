using MVVMProject.Model.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MVVMProject.Model
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
