using System;
using Reflector.Models;
using System.Xml.Serialization;

namespace Reflector.DataAccess.Dll
{
    public class AssemblyDllReader : IAssemblyReader
    {
        public AssemblyInfo Read(string name)
        {
            if (!DllExists(name))
            {
                throw new DllNotFoundException("Indicated DLL does not exist");
            }
            System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFile(name);
            return new AssemblyInfo(assembly);
        }

        private bool DllExists(string name)
        {
            return System.IO.File.Exists(name) && name.Contains(".dll");
        }
    }
}
