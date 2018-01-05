namespace Reflector.DataAccess.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitializeModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssemblyMetadatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AssemblyMetadatas");
        }
    }
}
