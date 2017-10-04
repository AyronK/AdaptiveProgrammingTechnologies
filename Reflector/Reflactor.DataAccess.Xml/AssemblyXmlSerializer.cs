using System;
using Reflector.DataAccess;
using Reflector.Models;
using System.Runtime.Serialization;
using System.IO;

namespace Reflactor.DataAccess.Xml
{
    public class AssemblyXmlSerializer : IAssemblyWriter
    {
        public void Write(AssemblyInfo assemblyInfo)
        {
            var serializer = new DataContractSerializer(assemblyInfo.GetType());
            using (FileStream stream = File.Create("serialized.xml"))
            {
                serializer.WriteObject(stream, assemblyInfo);
            }
        }
    }
}
