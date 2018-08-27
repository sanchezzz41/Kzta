namespace Databas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Details",
                c => new
                    {
                        DetailtGuid = c.Guid(nullable: false),
                        PropertyTypeGuid = c.Guid(nullable: false),
                        ImageGuid = c.Guid(),
                        Name = c.String(nullable: false, maxLength: 100),
                        InventoryNumber = c.String(maxLength: 100),
                        Description = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.DetailtGuid)
                .ForeignKey("dbo.Files", t => t.ImageGuid)
                .ForeignKey("dbo.PropertyTypes", t => t.PropertyTypeGuid, cascadeDelete: true)
                .Index(t => t.PropertyTypeGuid)
                .Index(t => t.ImageGuid);
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        FileGuid = c.Guid(nullable: false),
                        Extension = c.String(maxLength: 100),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.FileGuid);
            
            CreateTable(
                "dbo.PropertyTypes",
                c => new
                    {
                        PropertyTypeGuid = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.PropertyTypeGuid);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Details", "PropertyTypeGuid", "dbo.PropertyTypes");
            DropForeignKey("dbo.Details", "ImageGuid", "dbo.Files");
            DropIndex("dbo.Details", new[] { "ImageGuid" });
            DropIndex("dbo.Details", new[] { "PropertyTypeGuid" });
            DropTable("dbo.PropertyTypes");
            DropTable("dbo.Files");
            DropTable("dbo.Details");
        }
    }
}
