using System;
using Reflector.Models;
using System.Xml.Serialization;
using System.ComponentModel.Composition;

namespace Reflector.DataAccess.Dll
{
    [Export(typeof(IAssemblyReader))]
    [ExportMetadata("Name", nameof(AssemblyDllReader))]
    public class AssemblyDllReader : IAssemblyReader
    {
        public AssemblyMetadata Read(string name)
        {
            if (!DllExists(name))
            {
                throw new DllNotFoundException("Indicated DLL does not exist");
            }
            System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFile(name);
            return new AssemblyMetadata(assembly);
        }

        private bool DllExists(string name)
        {
            return System.IO.File.Exists(name) && name.Contains(".dll");
        }
    }
}
