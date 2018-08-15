namespace BankApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedTransfer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transfers", "CustomerId", c => c.Int(nullable: false));
            CreateIndex("dbo.Transfers", "CustomerId");
            AddForeignKey("dbo.Transfers", "CustomerId", "dbo.Customers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transfers", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Transfers", new[] { "CustomerId" });
            DropColumn("dbo.Transfers", "CustomerId");
        }
    }
}
