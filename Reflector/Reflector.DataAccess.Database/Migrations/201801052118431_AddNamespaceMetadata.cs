namespace Reflector.DataAccess.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNamespaceMetadata : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NamespaceMetadatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.NamespaceMetadatas");
        }
    }
}
