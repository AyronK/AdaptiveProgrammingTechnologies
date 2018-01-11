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
        public void Write(AssemblyMetadata assemblyInfo)
        {
            using (DataAccessContext _context = new DataAccessContext())
            {
                _context.AssemblyMetadatas.Add(assemblyInfo);
                _context.SaveChanges();
            }
        }
    }
}
