namespace Reflector.DataAccess.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMethodAttributesReturnTypeAndParameters : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.MethodMetadatas", name: "TypeMetadata_Id", newName: "ReturnType_Id");
            RenameIndex(table: "dbo.MethodMetadatas", name: "IX_TypeMetadata_Id", newName: "IX_ReturnType_Id");
            CreateTable(
                "dbo.TypeMetadataMethods",
                c => new
                    {
                        MethodMetadata_Id = c.Int(nullable: false),
                        TypeMetadata_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MethodMetadata_Id, t.TypeMetadata_Id })
                .ForeignKey("dbo.MethodMetadatas", t => t.MethodMetadata_Id, cascadeDelete: true)
                .ForeignKey("dbo.TypeMetadatas", t => t.TypeMetadata_Id, cascadeDelete: true)
                .Index(t => t.MethodMetadata_Id)
                .Index(t => t.TypeMetadata_Id);
            
            AddColumn("dbo.VarMetadatas", "ParameterParent_Id", c => c.Int());
            CreateIndex("dbo.VarMetadatas", "ParameterParent_Id");
            AddForeignKey("dbo.VarMetadatas", "ParameterParent_Id", "dbo.MethodMetadatas", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VarMetadatas", "ParameterParent_Id", "dbo.MethodMetadatas");
            DropForeignKey("dbo.TypeMetadataMethods", "TypeMetadata_Id", "dbo.TypeMetadatas");
            DropForeignKey("dbo.TypeMetadataMethods", "MethodMetadata_Id", "dbo.MethodMetadatas");
            DropIndex("dbo.TypeMetadataMethods", new[] { "TypeMetadata_Id" });
            DropIndex("dbo.TypeMetadataMethods", new[] { "MethodMetadata_Id" });
            DropIndex("dbo.VarMetadatas", new[] { "ParameterParent_Id" });
            DropColumn("dbo.VarMetadatas", "ParameterParent_Id");
            DropTable("dbo.TypeMetadataMethods");
            RenameIndex(table: "dbo.MethodMetadatas", name: "IX_ReturnType_Id", newName: "IX_TypeMetadata_Id");
            RenameColumn(table: "dbo.MethodMetadatas", name: "ReturnType_Id", newName: "TypeMetadata_Id");
        }
    }
}
