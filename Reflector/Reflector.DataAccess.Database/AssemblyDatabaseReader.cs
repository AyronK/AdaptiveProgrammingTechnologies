using System;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using Reflector.Models;

namespace Reflector.DataAccess.Database
{
    [Export(typeof(IAssemblyReader))]
    [ExportMetadata("Name", nameof(AssemblyDatabaseReader))]
    public class AssemblyDatabaseReader : IAssemblyReader
    {

        public AssemblyMetadata Read(string name)
        {
            using (DataAccessContext _context = new DataAccessContext())
            {
                _context.AssemblyMetadatas
                    .Include(a => a.NamespaceMetadatas.Select(n => n.Classes.Select(c => c.Methods.Select(v => v.Attributes))))
                    .Include(a => a.NamespaceMetadatas.Select(n => n.Classes.Select(c => c.Methods.Select(v => v.Parameters))))
                    .Include(a => a.NamespaceMetadatas.Select(n => n.Classes.Select(c => c.Methods.Select(v => v.ReturnType).Select(t => t.Properties))))
                    .Include(a => a.NamespaceMetadatas.Select(n => n.Classes.Select(c => c.Methods.Select(v => v.ReturnType).Select(t => t.Fields))))
                    .Include(a => a.NamespaceMetadatas.Select(n => n.Classes.Select(c => c.Methods.Select(v => v.ReturnType).Select(t => t.Attributes))))
                    .Include(a => a.NamespaceMetadatas.Select(n => n.Classes.Select(c => c.Methods.Select(v => v.ReturnType).Select(t => t.Methods))))
                    .Include(a => a.NamespaceMetadatas.Select(n => n.Classes.Select(c => c.Methods.Select(v => v.ReturnType).Select(t => t.ImplementedInterfaces))))
                    .Include(a => a.NamespaceMetadatas.Select(n => n.Classes.Select(c => c.Methods.Select(v => v.ReturnType).Select(t => t.NestedTypes))))
                    .Include(a => a.NamespaceMetadatas.Select(n => n.Classes.Select(c => c.Methods.Select(v => v.ReturnType).Select(t => t.GenericArguments))))
                    .Include(a => a.NamespaceMetadatas.Select(n => n.Classes.Select(c => c.Methods.Select(v => v.ReturnType).Select(t => t.BaseType))))
                    .Include(a => a.NamespaceMetadatas.Select(n => n.Classes.Select(c => c.Properties.Select(v => v.Attributes))))
                    .Include(a => a.NamespaceMetadatas.Select(n => n.Classes.Select(c => c.Properties.Select(v => v.Type).Select(t=>t.Properties))))
                    .Include(a => a.NamespaceMetadatas.Select(n => n.Classes.Select(c => c.Properties.Select(v => v.Type).Select(t => t.Fields))))
                    .Include(a => a.NamespaceMetadatas.Select(n => n.Classes.Select(c => c.Properties.Select(v => v.Type).Select(t => t.Attributes))))
                    .Include(a => a.NamespaceMetadatas.Select(n => n.Classes.Select(c => c.Properties.Select(v => v.Type).Select(t => t.Methods))))
                    .Include(a => a.NamespaceMetadatas.Select(n => n.Classes.Select(c => c.Properties.Select(v => v.Type).Select(t => t.ImplementedInterfaces))))
                    .Include(a => a.NamespaceMetadatas.Select(n => n.Classes.Select(c => c.Properties.Select(v => v.Type).Select(t => t.NestedTypes))))
                    .Include(a => a.NamespaceMetadatas.Select(n => n.Classes.Select(c => c.Properties.Select(v => v.Type).Select(t => t.GenericArguments))))
                    .Include(a => a.NamespaceMetadatas.Select(n => n.Classes.Select(c => c.Properties.Select(v => v.Type).Select(t => t.BaseType))))
                    .Include(a => a.NamespaceMetadatas.Select(n => n.Classes.Select(c => c.Fields.Select(v => v.Attributes))))
                    .Include(a => a.NamespaceMetadatas.Select(n => n.Classes.Select(c => c.Fields.Select(v => v.Type).Select(t => t.Properties))))
                    .Include(a => a.NamespaceMetadatas.Select(n => n.Classes.Select(c => c.Fields.Select(v => v.Type).Select(t => t.Fields))))
                    .Include(a => a.NamespaceMetadatas.Select(n => n.Classes.Select(c => c.Fields.Select(v => v.Type).Select(t => t.Attributes))))
                    .Include(a => a.NamespaceMetadatas.Select(n => n.Classes.Select(c => c.Fields.Select(v => v.Type).Select(t => t.Methods))))
                    .Include(a => a.NamespaceMetadatas.Select(n => n.Classes.Select(c => c.Fields.Select(v => v.Type).Select(t => t.ImplementedInterfaces))))
                    .Include(a => a.NamespaceMetadatas.Select(n => n.Classes.Select(c => c.Fields.Select(v => v.Type).Select(t => t.NestedTypes))))
                    .Include(a => a.NamespaceMetadatas.Select(n => n.Classes.Select(c => c.Fields.Select(v => v.Type).Select(t => t.GenericArguments))))
                    .Include(a => a.NamespaceMetadatas.Select(n => n.Classes.Select(c => c.Fields.Select(v => v.Type).Select(t => t.BaseType))))
                    .Include(a => a.NamespaceMetadatas.Select(n => n.Classes.Select(c => c.ImplementedInterfaces)))
                    .Include(a => a.NamespaceMetadatas.Select(n => n.Classes.Select(c => c.GenericArguments)))
                    .Include(a => a.NamespaceMetadatas.Select(n => n.Classes.Select(c => c.Attributes)))
                    .Load();

                return _context.AssemblyMetadatas.Include(a => a.NamespaceMetadatas.Select(n => n.TypesAlreadyDefined)).FirstOrDefault(a => a.Name == name);
            }
        }
    }
}
