using System;
using Reflector.DataAccess;
using Reflector.Models;
using System.Runtime.Serialization;
using System.IO;

namespace Reflactor.DataAccess.Xml
{
    public class AssemblyXmlSerializer : IAssemblyWriter
    {
        private string path;
        public AssemblyXmlSerializer()
        {
            path = "serializedAssembly.xml";
        }

        public AssemblyXmlSerializer(string path)
        {
            this.path = path;
        }

        public void Write(AssemblyInfo assemblyInfo)
        {
            var serializer = new DataContractSerializer(assemblyInfo.GetType());
            using (FileStream stream = File.Create(path))
            {
                serializer.WriteObject(stream, assemblyInfo);
            }
        }
    }
}
