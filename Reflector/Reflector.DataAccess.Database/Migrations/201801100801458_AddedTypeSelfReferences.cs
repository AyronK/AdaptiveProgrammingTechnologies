namespace Reflector.DataAccess.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTypeSelfReferences : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TypeMetadataAttributes",
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
            
            CreateTable(
                "dbo.TypeMetadataGenericArguments",
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
            
            CreateTable(
                "dbo.TypeMetadataImplementedInterfaces",
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
            DropForeignKey("dbo.TypeMetadataImplementedInterfaces", "TypeMetadata_Id1", "dbo.TypeMetadatas");
            DropForeignKey("dbo.TypeMetadataImplementedInterfaces", "TypeMetadata_Id", "dbo.TypeMetadatas");
            DropForeignKey("dbo.TypeMetadataGenericArguments", "TypeMetadata_Id1", "dbo.TypeMetadatas");
            DropForeignKey("dbo.TypeMetadataGenericArguments", "TypeMetadata_Id", "dbo.TypeMetadatas");
            DropForeignKey("dbo.TypeMetadataAttributes", "TypeMetadata_Id1", "dbo.TypeMetadatas");
            DropForeignKey("dbo.TypeMetadataAttributes", "TypeMetadata_Id", "dbo.TypeMetadatas");
            DropIndex("dbo.TypeMetadataImplementedInterfaces", new[] { "TypeMetadata_Id1" });
            DropIndex("dbo.TypeMetadataImplementedInterfaces", new[] { "TypeMetadata_Id" });
            DropIndex("dbo.TypeMetadataGenericArguments", new[] { "TypeMetadata_Id1" });
            DropIndex("dbo.TypeMetadataGenericArguments", new[] { "TypeMetadata_Id" });
            DropIndex("dbo.TypeMetadataAttributes", new[] { "TypeMetadata_Id1" });
            DropIndex("dbo.TypeMetadataAttributes", new[] { "TypeMetadata_Id" });
            DropTable("dbo.TypeMetadataImplementedInterfaces");
            DropTable("dbo.TypeMetadataGenericArguments");
            DropTable("dbo.TypeMetadataAttributes");
        }
    }
}
