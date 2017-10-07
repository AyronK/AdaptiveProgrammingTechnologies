using Reflector.Models;
using System.Runtime.Serialization;
using System.IO;

namespace Reflector.DataAccess.Xml
{
    public class AssemblyXmlSerializer : IAssemblyWriter
    {
        public void Write(AssemblyInfo assemblyInfo)
        {
            var serializer = new DataContractSerializer(assemblyInfo.GetType());
            using (FileStream stream = File.Create($"{assemblyInfo.Name}_Model.xml"))
            {
                serializer.WriteObject(stream, assemblyInfo);
            }
        }
    }
}
