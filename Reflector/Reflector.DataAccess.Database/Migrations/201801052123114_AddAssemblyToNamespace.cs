namespace Reflector.DataAccess.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAssemblyToNamespace : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NamespaceMetadatas", "AssemblyMetadataId", c => c.Int(nullable: false));
            CreateIndex("dbo.NamespaceMetadatas", "AssemblyMetadataId");
            AddForeignKey("dbo.NamespaceMetadatas", "AssemblyMetadataId", "dbo.AssemblyMetadatas", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NamespaceMetadatas", "AssemblyMetadataId", "dbo.AssemblyMetadatas");
            DropIndex("dbo.NamespaceMetadatas", new[] { "AssemblyMetadataId" });
            DropColumn("dbo.NamespaceMetadatas", "AssemblyMetadataId");
        }
    }
}
