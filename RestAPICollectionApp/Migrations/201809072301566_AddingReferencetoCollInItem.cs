namespace RestAPICollectionApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingReferencetoCollInItem : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Items", "CollectionModel_CollectionModelId", "dbo.CollectionModels");
            DropIndex("dbo.Items", new[] { "CollectionModel_CollectionModelId" });
            RenameColumn(table: "dbo.Items", name: "CollectionModel_CollectionModelId", newName: "CollectionModelId");
            AlterColumn("dbo.Items", "CollectionModelId", c => c.Int(nullable: false));
            CreateIndex("dbo.Items", "CollectionModelId");
            AddForeignKey("dbo.Items", "CollectionModelId", "dbo.CollectionModels", "CollectionModelId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "CollectionModelId", "dbo.CollectionModels");
            DropIndex("dbo.Items", new[] { "CollectionModelId" });
            AlterColumn("dbo.Items", "CollectionModelId", c => c.Int());
            RenameColumn(table: "dbo.Items", name: "CollectionModelId", newName: "CollectionModel_CollectionModelId");
            CreateIndex("dbo.Items", "CollectionModel_CollectionModelId");
            AddForeignKey("dbo.Items", "CollectionModel_CollectionModelId", "dbo.CollectionModels", "CollectionModelId");
        }
    }
}
