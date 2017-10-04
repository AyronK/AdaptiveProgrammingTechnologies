using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reflector.DataAccess;
using Reflector.DataAccess.File;
using Reflector.Models;

namespace ReflectorUnitTest.DataAccess
{
    [TestClass]
    public class FileTest
    {
        IAssemblyReader fileReader;

        [TestMethod]
        [ExpectedException(typeof(DllNotFoundException),
            "Not existing DLL was read.")]
        public void ReadDllFile()
        {
            string path = "";
            fileReader = new AssemblyDllReader(path);
            AssemblyInfo assembly = fileReader.Read();            
        }
    }
}
