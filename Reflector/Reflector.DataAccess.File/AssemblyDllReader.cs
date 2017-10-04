using System;
using Reflector.Models;
using System.Xml.Serialization;

namespace Reflector.DataAccess.Dll
{
    public class AssemblyDllReader : IAssemblyReader
    {
        private string path;

        public AssemblyDllReader(string path)
        {
            this.path = path;
        }

        public AssemblyInfo Read()
        {
            if (!DllExists())
            {
                throw new DllNotFoundException("Indicated DLL does not exist");
            }
            System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFile(path);
            return new AssemblyInfo(assembly);
        }

        private bool DllExists()
        {
            return System.IO.File.Exists(path) && path.Contains(".dll");
        }
    }
}
