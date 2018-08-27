namespace Databas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFiles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "Path", c => c.String(nullable: false, maxLength: 200));
            DropColumn("dbo.Files", "Extension");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Files", "Extension", c => c.String(maxLength: 100));
            DropColumn("dbo.Files", "Path");
        }
    }
}
