using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reflector.DataAccess;
using Reflector.Models;
using Reflector.DataAccess.Xml;
using System.Linq;
using System.IO;

namespace ReflectorUnitTest.DataAccess
{
    [TestClass]
    public class XmlSerializationTest
    {
        string xmlPath = "test.xml";
        AssemblyXmlSerializer writer;
        AssemblyXmlDeserializer reader;
        AssemblyMetadata assembly;

        public XmlSerializationTest()
        {
            assembly = new AssemblyMetadata(typeof(XmlSerializationTest).Assembly);
            writer = new AssemblyXmlSerializer();
            reader = new AssemblyXmlDeserializer();
        }

        [TestCleanup()]
        public void Cleanup()
        {
            if (File.Exists(xmlPath))
            {
                File.Delete(xmlPath);
            }
        }

        [TestMethod]
        public void SerializeAndDeserializeTest()
        {
            writer.Write(assembly, xmlPath);
            AssemblyMetadata processedAssembly = reader.Read(xmlPath);

            Assert.AreEqual(assembly.Name, processedAssembly.Name);
            foreach(var ns in processedAssembly.NamespaceMetadatas)
            {
                Assert.IsNotNull(assembly.NamespaceMetadatas.First(f=>f.Name == ns.Name));
            }
        }
    }
}
