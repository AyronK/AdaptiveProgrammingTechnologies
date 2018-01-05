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
        }

        public DbSet<AssemblyMetadata> AssemblyMetadatas { get; set; }
        public DbSet<NamespaceMetadata> NamespaceMetadatas { get; set; }
        public DbSet<TypeMetadata> TypeMetadatas { get; set; }
        public DbSet<VarMetadata> VarMetadatas { get; set; }
        public DbSet<MethodMetadata> MethodMetadatas { get; set; }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }

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
            

        }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}