namespace Reflector.DataAccess.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedFKIDFromNamespace : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.NamespaceMetadatas", "AssemblyMetadataId", "dbo.AssemblyMetadatas");
            DropIndex("dbo.NamespaceMetadatas", new[] { "AssemblyMetadataId" });
            RenameColumn(table: "dbo.NamespaceMetadatas", name: "AssemblyMetadataId", newName: "AssemblyMetadata_Id");
            AlterColumn("dbo.NamespaceMetadatas", "AssemblyMetadata_Id", c => c.Int());
            CreateIndex("dbo.NamespaceMetadatas", "AssemblyMetadata_Id");
            AddForeignKey("dbo.NamespaceMetadatas", "AssemblyMetadata_Id", "dbo.AssemblyMetadatas", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NamespaceMetadatas", "AssemblyMetadata_Id", "dbo.AssemblyMetadatas");
            DropIndex("dbo.NamespaceMetadatas", new[] { "AssemblyMetadata_Id" });
            AlterColumn("dbo.NamespaceMetadatas", "AssemblyMetadata_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.NamespaceMetadatas", name: "AssemblyMetadata_Id", newName: "AssemblyMetadataId");
            CreateIndex("dbo.NamespaceMetadatas", "AssemblyMetadataId");
            AddForeignKey("dbo.NamespaceMetadatas", "AssemblyMetadataId", "dbo.AssemblyMetadatas", "Id", cascadeDelete: true);
        }
    }
}
