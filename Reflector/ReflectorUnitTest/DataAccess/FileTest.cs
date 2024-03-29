﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reflector.DataAccess;
using Reflector.DataAccess.Dll;
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
            fileReader = new AssemblyDllReader();
            AssemblyMetadata assembly = fileReader.Read(path);            
        }
    }
}
