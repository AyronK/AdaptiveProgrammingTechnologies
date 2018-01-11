namespace Reflector.DataAccess.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TypesDefined : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TypeMetadatas", "NamespaceMetadata_Id1", c => c.Int());
            CreateIndex("dbo.TypeMetadatas", "NamespaceMetadata_Id1");
            AddForeignKey("dbo.TypeMetadatas", "NamespaceMetadata_Id1", "dbo.NamespaceMetadatas", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TypeMetadatas", "NamespaceMetadata_Id1", "dbo.NamespaceMetadatas");
            DropIndex("dbo.TypeMetadatas", new[] { "NamespaceMetadata_Id1" });
            DropColumn("dbo.TypeMetadatas", "NamespaceMetadata_Id1");
        }
    }
}
