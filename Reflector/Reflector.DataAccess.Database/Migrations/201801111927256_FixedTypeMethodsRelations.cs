namespace Reflector.DataAccess.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedTypeMethodsRelations : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.MethodMetadatas", name: "TypeMetadata_Id", newName: "TypeMethodParent_Id");
            RenameIndex(table: "dbo.MethodMetadatas", name: "IX_TypeMetadata_Id", newName: "IX_TypeMethodParent_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.MethodMetadatas", name: "IX_TypeMethodParent_Id", newName: "IX_TypeMetadata_Id");
            RenameColumn(table: "dbo.MethodMetadatas", name: "TypeMethodParent_Id", newName: "TypeMetadata_Id");
        }
    }
}
