namespace BankApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerController : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.BusinessTransactions", "CustomerId");
            CreateIndex("dbo.CheckingTransactions", "CustomerId");
            AddForeignKey("dbo.BusinessTransactions", "CustomerId", "dbo.Customers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CheckingTransactions", "CustomerId", "dbo.Customers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CheckingTransactions", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.BusinessTransactions", "CustomerId", "dbo.Customers");
            DropIndex("dbo.CheckingTransactions", new[] { "CustomerId" });
            DropIndex("dbo.BusinessTransactions", new[] { "CustomerId" });
        }
    }
}
