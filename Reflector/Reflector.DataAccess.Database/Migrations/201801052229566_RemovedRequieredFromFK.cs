namespace Reflector.DataAccess.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedRequieredFromFK : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.VarMetadatas", new[] { "FieldParent_Id" });
            DropIndex("dbo.VarMetadatas", new[] { "PropertyParent_Id" });
            AlterColumn("dbo.VarMetadatas", "FieldParent_Id", c => c.Int());
            AlterColumn("dbo.VarMetadatas", "PropertyParent_Id", c => c.Int());
            CreateIndex("dbo.VarMetadatas", "FieldParent_Id");
            CreateIndex("dbo.VarMetadatas", "PropertyParent_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.VarMetadatas", new[] { "PropertyParent_Id" });
            DropIndex("dbo.VarMetadatas", new[] { "FieldParent_Id" });
            AlterColumn("dbo.VarMetadatas", "PropertyParent_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.VarMetadatas", "FieldParent_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.VarMetadatas", "PropertyParent_Id");
            CreateIndex("dbo.VarMetadatas", "FieldParent_Id");
        }
    }
}
