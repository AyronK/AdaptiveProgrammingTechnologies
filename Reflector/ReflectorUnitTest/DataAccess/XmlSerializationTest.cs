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
        IAssemblyWriter writer;
        IAssemblyReader reader;
        readonly string xmlPath = "test.xml";
        AssemblyInfo assembly;

        public XmlSerializationTest()
        {
            assembly = new AssemblyInfo(typeof(XmlSerializationTest).Assembly);
            writer = new AssemblyXmlSerializer();
            reader = new AssemblyXmlDeserializer(xmlPath);
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
            writer.Write(assembly);
            AssemblyInfo processedAssembly = reader.Read(xmlPath);

            Assert.AreEqual(assembly.Name, processedAssembly.Name);
            foreach(var ns in processedAssembly.Namespaces)
            {
                Assert.IsNotNull(assembly.Namespaces.First(f=>f.Name == ns.Name));
            }
        }
    }
}
