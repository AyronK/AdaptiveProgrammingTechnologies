namespace Reflector.DataAccess.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedReturnTypeToMethod : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MethodMetadatas", "ReturnType_Id", c => c.Int());
            CreateIndex("dbo.MethodMetadatas", "ReturnType_Id");
            AddForeignKey("dbo.MethodMetadatas", "ReturnType_Id", "dbo.TypeMetadatas", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MethodMetadatas", "ReturnType_Id", "dbo.TypeMetadatas");
            DropIndex("dbo.MethodMetadatas", new[] { "ReturnType_Id" });
            DropColumn("dbo.MethodMetadatas", "ReturnType_Id");
        }
    }
}
