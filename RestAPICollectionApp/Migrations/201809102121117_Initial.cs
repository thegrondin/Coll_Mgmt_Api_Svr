namespace RestAPICollectionApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CollectionModels",
                c => new
                    {
                        CollectionModelId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.CollectionModelId);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ItemId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        CollectionModelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ItemId)
                .ForeignKey("dbo.CollectionModels", t => t.CollectionModelId, cascadeDelete: true)
                .Index(t => t.CollectionModelId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "CollectionModelId", "dbo.CollectionModels");
            DropIndex("dbo.Items", new[] { "CollectionModelId" });
            DropTable("dbo.Items");
            DropTable("dbo.CollectionModels");
        }
    }
}
