using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVVMProject.Model;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Text;
using System;

namespace MVVMProject.UnitTest
{
    [TestClass]
    public class AssemblyModelTest
    {
        [TestMethod]
        public void LoadAssemblyTest()
        {            
            string libraryPath = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName, "Bookshop\\bin\\Debug\\Bookshop.dll");
            AssemblyModel assembly = new AssemblyModel(libraryPath);
            
            // Count number of classes
            int numberOfClasses = 0;
            foreach (NamespaceModel n in assembly.Namespaces)
                foreach (TypeModel t in n.Classes)
                    numberOfClasses++;

            Assert.AreEqual("Bookshop", assembly.Name);
            Assert.AreEqual(1, assembly.Namespaces.Count);
            Assert.AreEqual(3, numberOfClasses);            

            // Name
            Assert.AreEqual("Bookshop", assembly.Classes["Bookshop"].TypeName);
            Assert.AreEqual("Book", assembly.Classes["Book"].TypeName);
            Assert.AreEqual("Bookshelf", assembly.Classes["Bookshelf"].TypeName);

            // Fields
            Assert.AreEqual("String", assembly.Classes["Bookshop"].Fields[0].BaseType.TypeName);
            Assert.AreEqual("adress", assembly.Classes["Bookshop"].Fields[0].Name);
            
            // Methods
            Assert.AreEqual("public Void SellBook(Book book)", assembly.Classes["Bookshop"].Methods[4].ToString());
            Assert.AreEqual("public static Double MonthlySales(Double income, String currency)", assembly.Classes["Bookshop"].Methods[5].ToString());

            // Properties
            Assert.AreEqual(3, assembly.Classes["Book"].Properties.Count);
        }

        [Ignore]
        // Test nie odzwieciedla dobrze procesu serializacji/deserializacji
        // XMLSerializer nie pozwala na serializacje wyłącznie referencji przez co
        // nie współpracuje z rekurencją. 
        [TestMethod]
        public void SerializeTest()
        {           
            string libraryPath = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName, "Bookshop\\bin\\Debug\\Bookshop.dll");
            AssemblyModel assembly = new AssemblyModel(libraryPath);

            // Z wykorzystaniem XmlSerializer

            XmlSerializer serializer = new XmlSerializer(typeof(AssemblyModel));
            XmlWriter writer = new XmlTextWriter("serializedFile.xml", Encoding.Unicode);
            serializer.Serialize(writer, assembly);
            writer.Close();

            XmlReader reader = new XmlTextReader("serializedFile.xml");

            AssemblyModel deserializedSecond = (AssemblyModel)serializer.Deserialize(reader);

            // Check name
            Assert.AreEqual(assembly.Name, deserializedSecond.Name);
            Assert.AreEqual(assembly.Namespaces[0].Name, deserializedSecond.Namespaces[0].Name);

            // Check number of classes
            int numberOfClassesOriginal = 0;
            foreach (NamespaceModel n in assembly.Namespaces)
                foreach (TypeModel t in n.Classes)
                    numberOfClassesOriginal++;
           int numberOfClassesDeserialized = 0;
            foreach (NamespaceModel n in deserializedSecond.Namespaces)
                foreach (TypeModel t in n.Classes)
                    numberOfClassesDeserialized++;
            Assert.AreEqual(numberOfClassesOriginal, numberOfClassesDeserialized);

            // Check type of the first class
            Assert.AreEqual(assembly.Namespaces[0].Classes[0].TypeName, deserializedSecond.Namespaces[0].Classes[0].TypeName);

            // Check number of methods
            int numberOfMethodsOriginal = 0;
            foreach (NamespaceModel n in assembly.Namespaces)
                foreach (TypeModel t in n.Classes)
                    foreach (MethodModel m in t.Methods)
                        numberOfMethodsOriginal++;
            int numberOfMethodsDeserialized = 0;
            foreach (NamespaceModel n in deserializedSecond.Namespaces)
                foreach (TypeModel t in n.Classes)
                    foreach (MethodModel m in t.Methods)
                        numberOfMethodsDeserialized++;
            Assert.AreEqual(numberOfMethodsOriginal, numberOfMethodsDeserialized);
        }

        [TestMethod]
        public void RecursionTest()
        {
            AssemblyModel assembly = new AssemblyModel(typeof(RecursiveNamespace.A).Assembly);

            /*
             * RecursiveNamespace contains three classes: A B and C
             * A contains a reference to B
             * B contains a reference to C via method
             * C contains a reference to A, B via property and C itself via method
             */

            Assert.IsTrue(assembly.Classes.ContainsKey("A"));
            Assert.IsTrue(assembly.Classes.ContainsKey("B"));
            Assert.IsTrue(assembly.Classes.ContainsKey("C"));
        }

    }
}
