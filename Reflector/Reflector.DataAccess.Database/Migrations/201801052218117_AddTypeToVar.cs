namespace Reflector.DataAccess.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTypeToVar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VarMetadatas", "Type_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.VarMetadatas", "Type_Id");
            AddForeignKey("dbo.VarMetadatas", "Type_Id", "dbo.TypeMetadatas", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VarMetadatas", "Type_Id", "dbo.TypeMetadatas");
            DropIndex("dbo.VarMetadatas", new[] { "Type_Id" });
            DropColumn("dbo.VarMetadatas", "Type_Id");
        }
    }
}
