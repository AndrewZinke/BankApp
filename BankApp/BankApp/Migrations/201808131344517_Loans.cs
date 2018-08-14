namespace BankApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Loans : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Loans",
                c => new
                    {
                        LoanId = c.Int(nullable: false, identity: true),
                        Amount = c.Double(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.LoanId)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Loans", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Loans", new[] { "CustomerId" });
            DropTable("dbo.Loans");
        }
    }
}
