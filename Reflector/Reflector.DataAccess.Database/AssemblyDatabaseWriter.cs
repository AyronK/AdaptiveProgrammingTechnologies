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
            //_context.TypeMetadatas.AddRange(assemblyInfo.NamespaceMetadatas.SelectMany(n => n.TypesAlreadyDefined));
            _context.AssemblyMetadatas.Add(assemblyInfo);
            //_context.NamespaceMetadatas.AddRange(assemblyInfo.NamespaceMetadatas);
            //var types = assemblyInfo.NamespaceMetadatas.SelectMany(n => n.Classes);
            //_context.TypeMetadatas.AddRange(types);
            //_context.TypeMetadatas.AddRange(types.SelectMany(t=>t.Attributes));
            //_context.TypeMetadatas.AddRange(types.SelectMany(t => t.ImplementedInterfaces));
            //_context.TypeMetadatas.AddRange(types.SelectMany(t => t.GenericArguments));
            //_context.MethodMetadatas.AddRange(types.SelectMany(t => t.Methods));
            //_context.VarMetadatas.AddRange(types.SelectMany(t => t.Fields));
            //_context.VarMetadatas.AddRange(types.SelectMany(t => t.Properties));
            _context.SaveChanges();

            var result = _context.AssemblyMetadatas.
                Include(a => a.NamespaceMetadatas.
                    Select(n => n.Classes.
                    Select(c => c.Methods))).
                Include(a => a.NamespaceMetadatas.
                    Select(n => n.Classes.
                    Select(c => c.Properties.
                    Select(v => v.Attributes)))).
                Include(a => a.NamespaceMetadatas.
                    Select(n => n.Classes.
                    Select(c => c.Fields.
                    Select(v => v.Attributes)))).
                Include(a => a.NamespaceMetadatas.
                    Select(n => n.Classes.
                    Select(c => c.ImplementedInterfaces))).
                Include(a => a.NamespaceMetadatas.
                    Select(n => n.Classes.
                    Select(c => c.GenericArguments))).
                Include(a => a.NamespaceMetadatas.
                    Select(n => n.Classes.
                    Select(c => c.Attributes))).
                FirstOrDefault(a => a.Id == 1011);
        }
    }
}
