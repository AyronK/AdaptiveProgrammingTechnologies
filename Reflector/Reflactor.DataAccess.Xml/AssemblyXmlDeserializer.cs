using Reflector.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reflector.Models;
using System.IO;
using System.Runtime.Serialization;

namespace Reflector.DataAccess.Xml
{
    public class AssemblyXmlDeserializer : IAssemblyReader
    {
        public AssemblyXmlDeserializer()
        {
        }

        public AssemblyInfo Read(string name)
        {
            if (!XmlFileExists(name))
            {
                throw new DllNotFoundException("Indicated XML file does not exist");
            }
            var serializer = new DataContractSerializer(typeof(AssemblyInfo));
            AssemblyInfo assemblyInfo = null;
            using (FileStream stream = File.OpenRead(name))
            {
                assemblyInfo = (AssemblyInfo)serializer.ReadObject(stream);
            }
            return assemblyInfo;
        }

        private bool XmlFileExists(string name)
        {
            return System.IO.File.Exists(name) && name.Contains(".xml");
        }
    }
}
