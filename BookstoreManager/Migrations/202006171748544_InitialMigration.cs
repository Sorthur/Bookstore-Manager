namespace BookstoreManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Author = c.String(),
                        Year = c.Int(nullable: false),
                        Edition = c.Int(nullable: false),
                        NumberOfPages = c.Int(nullable: false),
                        IsHardCover = c.Boolean(nullable: false),
                        Quantity = c.Int(nullable: false),
                        IsAvailable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumberOfOrderedBooks = c.Int(nullable: false),
                        OrderedBook_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Books", t => t.OrderedBook_Id)
                .Index(t => t.OrderedBook_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "OrderedBook_Id", "dbo.Books");
            DropIndex("dbo.Orders", new[] { "OrderedBook_Id" });
            DropTable("dbo.Orders");
            DropTable("dbo.Books");
        }
    }
}
