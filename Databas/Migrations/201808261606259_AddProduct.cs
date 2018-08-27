namespace Databas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProduct : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DetailInProducts",
                c => new
                    {
                        DetailGuid = c.Guid(nullable: false),
                        ProductGuid = c.Guid(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DetailGuid, t.ProductGuid })
                .ForeignKey("dbo.Details", t => t.DetailGuid, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductGuid, cascadeDelete: true)
                .Index(t => t.DetailGuid)
                .Index(t => t.ProductGuid);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductGuid = c.Guid(nullable: false),
                        FileGuid = c.Guid(),
                        Name = c.String(nullable: false, maxLength: 200),
                        Description = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.ProductGuid)
                .ForeignKey("dbo.Files", t => t.FileGuid)
                .Index(t => t.FileGuid);
            
            AddColumn("dbo.Details", "Material", c => c.String(maxLength: 100));
            AddColumn("dbo.Details", "Size", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DetailInProducts", "ProductGuid", "dbo.Products");
            DropForeignKey("dbo.Products", "FileGuid", "dbo.Files");
            DropForeignKey("dbo.DetailInProducts", "DetailGuid", "dbo.Details");
            DropIndex("dbo.Products", new[] { "FileGuid" });
            DropIndex("dbo.DetailInProducts", new[] { "ProductGuid" });
            DropIndex("dbo.DetailInProducts", new[] { "DetailGuid" });
            DropColumn("dbo.Details", "Size");
            DropColumn("dbo.Details", "Material");
            DropTable("dbo.Products");
            DropTable("dbo.DetailInProducts");
        }
    }
}
