namespace Reflector.DataAccess.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMethodsToType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MethodMetadatas", "TypeMetadata_Id", c => c.Int());
            CreateIndex("dbo.MethodMetadatas", "TypeMetadata_Id");
            AddForeignKey("dbo.MethodMetadatas", "TypeMetadata_Id", "dbo.TypeMetadatas", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MethodMetadatas", "TypeMetadata_Id", "dbo.TypeMetadatas");
            DropIndex("dbo.MethodMetadatas", new[] { "TypeMetadata_Id" });
            DropColumn("dbo.MethodMetadatas", "TypeMetadata_Id");
        }
    }
}
