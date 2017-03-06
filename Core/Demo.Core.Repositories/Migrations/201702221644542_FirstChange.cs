namespace Demo.Core.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstChange : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProductCategory = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Orders", "OrderDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "OrderStatus", c => c.Int(nullable: false));
            CreateIndex("dbo.OrderLines", "ProductId");
            AddForeignKey("dbo.OrderLines", "ProductId", "dbo.Products", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderLines", "ProductId", "dbo.Products");
            DropIndex("dbo.OrderLines", new[] { "ProductId" });
            DropColumn("dbo.Orders", "OrderStatus");
            DropColumn("dbo.Orders", "OrderDate");
            DropTable("dbo.Products");
        }
    }
}
