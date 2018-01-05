namespace Reflector.DataAccess.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddManyToManyTypeVar : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VarMetadatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VarMetadataTypeMetadatas",
                c => new
                    {
                        VarMetadata_Id = c.Int(nullable: false),
                        TypeMetadata_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VarMetadata_Id, t.TypeMetadata_Id })
                .ForeignKey("dbo.VarMetadatas", t => t.VarMetadata_Id, cascadeDelete: true)
                .ForeignKey("dbo.TypeMetadatas", t => t.TypeMetadata_Id, cascadeDelete: true)
                .Index(t => t.VarMetadata_Id)
                .Index(t => t.TypeMetadata_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VarMetadataTypeMetadatas", "TypeMetadata_Id", "dbo.TypeMetadatas");
            DropForeignKey("dbo.VarMetadataTypeMetadatas", "VarMetadata_Id", "dbo.VarMetadatas");
            DropIndex("dbo.VarMetadataTypeMetadatas", new[] { "TypeMetadata_Id" });
            DropIndex("dbo.VarMetadataTypeMetadatas", new[] { "VarMetadata_Id" });
            DropTable("dbo.VarMetadataTypeMetadatas");
            DropTable("dbo.VarMetadatas");
        }
    }
}
