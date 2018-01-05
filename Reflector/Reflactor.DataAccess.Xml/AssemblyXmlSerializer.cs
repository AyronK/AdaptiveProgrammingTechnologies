using Reflector.Models;
using System.Runtime.Serialization;
using System.IO;
using System.ComponentModel.Composition;

namespace Reflector.DataAccess.Xml
{
    [Export(typeof(IAssemblyWriter))]
    [ExportMetadata("Name", nameof(AssemblyXmlSerializer))]
    public class AssemblyXmlSerializer : IAssemblyWriter
    {
        public void Write(AssemblyMetadata assemblyInfo)
        {
            Write(assemblyInfo, $"{Directory.GetCurrentDirectory()}/{assemblyInfo.Name}_Model.xml");
        }

        public void Write(AssemblyMetadata assemblyInfo, string path)
        {
            var serializer = new DataContractSerializer(assemblyInfo.GetType());
            using (FileStream stream = File.Create(path))
            {
                serializer.WriteObject(stream, assemblyInfo);
            }
        }
    }
}
