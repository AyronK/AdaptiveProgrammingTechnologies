    using Reflector.Models;

namespace Reflector.DataAccess.Database
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class DataAccessContext : DbContext
    {
        // Your context has been configured to use a 'DataAccessContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Reflector.DataAccess.Database.DataAccessContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'DataAccessContext' 
        // connection string in the application configuration file.
        public DataAccessContext()
            : base("name=Reflector")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<AssemblyMetadata> AssemblyMetadatas { get; set; }
        public DbSet<NamespaceMetadata> NamespaceMetadatas { get; set; }
        public DbSet<TypeMetadata> TypeMetadatas { get; set; }
        public DbSet<VarMetadata> VarMetadatas { get; set; }
        public DbSet<MethodMetadata> MethodMetadatas { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AssemblyMetadata>()
                .HasMany(m => m.NamespaceMetadatas)
                .WithOptional(t => t.AssemblyMetadata)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NamespaceMetadata>()
                .HasMany(m => m.Classes)
                .WithOptional(t => t.NamespaceMetadata)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VarMetadata>()
                .HasRequired(m => m.Type)
                .WithMany(t => t.Vars)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VarMetadata>()
                .HasMany(m => m.Attributes)
                .WithMany(t => t.VarAttributes);

            modelBuilder.Entity<TypeMetadata>()
                .HasMany(m => m.Fields)
                .WithOptional(t => t.FieldParent)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TypeMetadata>()
                .HasMany(m => m.Properties)
                .WithOptional(t => t.PropertyParent)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TypeMetadata>()
               .HasMany(m => m.Attributes)
               .WithMany(t => t.AttributesParent)
               .Map(m => m.ToTable("TypeMetadataAttributes"));

            modelBuilder.Entity<TypeMetadata>()
               .HasMany(m => m.GenericArguments)
               .WithMany(t => t.GenericArgumentsParent)
               .Map(m => m.ToTable("TypeMetadataGenericArguments"));

            modelBuilder.Entity<TypeMetadata>()
               .HasMany(m => m.ImplementedInterfaces)
               .WithMany(t => t.ImplementedInterfacesParent)
               .Map(m => m.ToTable("TypeMetadataImplementedInterfaces"));

            modelBuilder.Entity<TypeMetadata>()
                .HasMany(m => m.NestedTypes)
                .WithMany(t => t.NestedTypesParent)
                .Map(m => m.ToTable("TypeMetadataNestedTypes"));

            modelBuilder.Entity<MethodMetadata>()
               .HasMany(m => m.Parameters)
               .WithOptional(v => v.ParameterParent);

            modelBuilder.Entity<MethodMetadata>()
               .HasMany(m => m.Attributes)
               .WithMany(v => v.MethodsParents)
               .Map(m => m.ToTable("TypeMetadataMethods"));

            modelBuilder.Entity<TypeMetadata>()
                .HasMany(m => m.Methods)
                .WithOptional(t => t.TypeMethodParent);

            modelBuilder.Entity<NamespaceMetadata>()
                .HasMany(m => m.TypesAlreadyDefined);

        }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}