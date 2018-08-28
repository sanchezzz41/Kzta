namespace Databas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSnToProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "SerialNumber", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "SerialNumber");
        }
    }
}
