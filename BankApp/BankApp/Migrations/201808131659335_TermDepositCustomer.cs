namespace BankApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TermDepositCustomer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TermDeposits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateOpened = c.DateTime(nullable: false),
                        Amount = c.Double(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        InterestRate = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TermDeposits", "CustomerId", "dbo.Customers");
            DropIndex("dbo.TermDeposits", new[] { "CustomerId" });
            DropTable("dbo.TermDeposits");
        }
    }
}
