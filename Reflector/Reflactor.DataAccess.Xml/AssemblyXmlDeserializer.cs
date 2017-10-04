using Reflector.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reflector.Models;
using System.IO;
using System.Runtime.Serialization;

namespace Reflactor.DataAccess.Xml
{
    public class AssemblyXmlDeserializer : IAssemblyReader
    {
        private string path;

        public AssemblyXmlDeserializer(string path)
        {
            this.path = path;
        }

        public AssemblyInfo Read()
        {
            if (!XmlFileExists())
            {
                throw new DllNotFoundException("Indicated XML file does not exist");
            }
            var serializer = new DataContractSerializer(typeof(AssemblyInfo));
            AssemblyInfo assemblyInfo = null;
            using (FileStream stream = File.OpenRead(path))
            {
                assemblyInfo = (AssemblyInfo)serializer.ReadObject(stream);
            }
            return assemblyInfo;
        }

        private bool XmlFileExists()
        {
            return System.IO.File.Exists(path) && path.Contains(".xml");
        }
    }
}
