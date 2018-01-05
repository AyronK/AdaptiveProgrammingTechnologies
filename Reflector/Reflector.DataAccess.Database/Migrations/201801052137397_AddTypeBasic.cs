namespace Reflector.DataAccess.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTypeBasic : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TypeMetadatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        NamespaceMetadata_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NamespaceMetadatas", t => t.NamespaceMetadata_Id)
                .Index(t => t.NamespaceMetadata_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TypeMetadatas", "NamespaceMetadata_Id", "dbo.NamespaceMetadatas");
            DropIndex("dbo.TypeMetadatas", new[] { "NamespaceMetadata_Id" });
            DropTable("dbo.TypeMetadatas");
        }
    }
}
