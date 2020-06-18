namespace BookstoreManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPricePropertyMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Orders", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Price");
            DropColumn("dbo.Books", "Price");
        }
    }
}
