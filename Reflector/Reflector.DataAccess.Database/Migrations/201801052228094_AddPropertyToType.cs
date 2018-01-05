namespace Reflector.DataAccess.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPropertyToType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VarMetadatas", "FieldParent_Id", c => c.Int(nullable: false));
            AddColumn("dbo.VarMetadatas", "PropertyParent_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.VarMetadatas", "FieldParent_Id");
            CreateIndex("dbo.VarMetadatas", "PropertyParent_Id");
            AddForeignKey("dbo.VarMetadatas", "FieldParent_Id", "dbo.TypeMetadatas", "Id");
            AddForeignKey("dbo.VarMetadatas", "PropertyParent_Id", "dbo.TypeMetadatas", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VarMetadatas", "PropertyParent_Id", "dbo.TypeMetadatas");
            DropForeignKey("dbo.VarMetadatas", "FieldParent_Id", "dbo.TypeMetadatas");
            DropIndex("dbo.VarMetadatas", new[] { "PropertyParent_Id" });
            DropIndex("dbo.VarMetadatas", new[] { "FieldParent_Id" });
            DropColumn("dbo.VarMetadatas", "PropertyParent_Id");
            DropColumn("dbo.VarMetadatas", "FieldParent_Id");
        }
    }
}
