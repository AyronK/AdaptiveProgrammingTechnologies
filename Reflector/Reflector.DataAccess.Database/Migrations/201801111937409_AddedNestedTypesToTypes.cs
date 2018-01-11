namespace Reflector.DataAccess.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNestedTypesToTypes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TypeMetadataNestedTypes",
                c => new
                    {
                        TypeMetadata_Id = c.Int(nullable: false),
                        TypeMetadata_Id1 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TypeMetadata_Id, t.TypeMetadata_Id1 })
                .ForeignKey("dbo.TypeMetadatas", t => t.TypeMetadata_Id)
                .ForeignKey("dbo.TypeMetadatas", t => t.TypeMetadata_Id1)
                .Index(t => t.TypeMetadata_Id)
                .Index(t => t.TypeMetadata_Id1);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TypeMetadataNestedTypes", "TypeMetadata_Id1", "dbo.TypeMetadatas");
            DropForeignKey("dbo.TypeMetadataNestedTypes", "TypeMetadata_Id", "dbo.TypeMetadatas");
            DropIndex("dbo.TypeMetadataNestedTypes", new[] { "TypeMetadata_Id1" });
            DropIndex("dbo.TypeMetadataNestedTypes", new[] { "TypeMetadata_Id" });
            DropTable("dbo.TypeMetadataNestedTypes");
        }
    }
}
