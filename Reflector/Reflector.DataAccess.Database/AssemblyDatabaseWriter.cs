using System;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using Reflector.Models;

namespace Reflector.DataAccess.Database
{
    [Export(typeof(IAssemblyWriter))]
    [ExportMetadata("Name", nameof(AssemblyDatabaseWriter))]
    public class AssemblyDatabaseWriter : IAssemblyWriter
    {
        private DataAccessContext _context;

        public AssemblyDatabaseWriter()
        {
            _context = new DataAccessContext();
        }

        public void Write(AssemblyMetadata assemblyInfo)
        {
            _context.AssemblyMetadatas.Add(assemblyInfo);
            _context.SaveChanges();
        }
    }
}
