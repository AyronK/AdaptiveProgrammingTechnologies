using System;
using Reflector.Models;
using System.IO;
using System.Runtime.Serialization;

namespace Reflector.DataAccess.Xml
{
    public class AssemblyXmlDeserializer : IAssemblyReader
    {
        public AssemblyInfo Read(string name)
        {
            if (!XmlFileExists(name))
            {
                throw new FileNotFoundException("Indicated XML file does not exist");
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
            return File.Exists(name) && name.Contains(".xml");
        }
    }
}
