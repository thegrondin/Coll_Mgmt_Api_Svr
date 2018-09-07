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
                        CollectionModel_CollectionModelId = c.Int(),
                    })
                .PrimaryKey(t => t.ItemId)
                .ForeignKey("dbo.CollectionModels", t => t.CollectionModel_CollectionModelId)
                .Index(t => t.CollectionModel_CollectionModelId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "CollectionModel_CollectionModelId", "dbo.CollectionModels");
            DropIndex("dbo.Items", new[] { "CollectionModel_CollectionModelId" });
            DropTable("dbo.Items");
            DropTable("dbo.CollectionModels");
        }
    }
}
