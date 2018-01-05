namespace Reflector.DataAccess.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBaseType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TypeMetadatas", "BaseType_Id", c => c.Int());
            CreateIndex("dbo.TypeMetadatas", "BaseType_Id");
            AddForeignKey("dbo.TypeMetadatas", "BaseType_Id", "dbo.TypeMetadatas", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TypeMetadatas", "BaseType_Id", "dbo.TypeMetadatas");
            DropIndex("dbo.TypeMetadatas", new[] { "BaseType_Id" });
            DropColumn("dbo.TypeMetadatas", "BaseType_Id");
        }
    }
}
