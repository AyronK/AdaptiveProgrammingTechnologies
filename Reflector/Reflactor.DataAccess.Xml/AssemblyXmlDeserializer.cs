using System;
using Reflector.Models;
using System.IO;
using System.Runtime.Serialization;

namespace Reflector.DataAccess.Xml
{
    public class AssemblyXmlDeserializer : IAssemblyReader
    {
        public AssemblyMetadata Read(string name)
        {
            if (!XmlFileExists(name))
            {
                throw new FileNotFoundException("Indicated XML file does not exist");
            }
            var serializer = new DataContractSerializer(typeof(AssemblyMetadata));
            AssemblyMetadata assemblyInfo = null;
            using (FileStream stream = File.OpenRead(name))
            {
                assemblyInfo = (AssemblyMetadata)serializer.ReadObject(stream);
            }
            return assemblyInfo;
        }

        private bool XmlFileExists(string name)
        {
            return File.Exists(name) && name.Contains(".xml");
        }
    }
}
