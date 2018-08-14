namespace BankApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BusinessAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        Balance = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BusinessTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        DateOfTransaction = c.DateTime(nullable: false),
                        Amount = c.Double(nullable: false),
                        BusinessAccount_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BusinessAccounts", t => t.BusinessAccount_Id)
                .Index(t => t.BusinessAccount_Id);
            
            CreateTable(
                "dbo.CheckingAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        Balance = c.Double(nullable: false),
                        Interest = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.CheckingTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        DateOfTransaction = c.DateTime(nullable: false),
                        Amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CheckingAccounts", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.BusinessTransactions", "BusinessAccount_Id", "dbo.BusinessAccounts");
            DropForeignKey("dbo.BusinessAccounts", "CustomerId", "dbo.Customers");
            DropIndex("dbo.CheckingAccounts", new[] { "CustomerId" });
            DropIndex("dbo.BusinessTransactions", new[] { "BusinessAccount_Id" });
            DropIndex("dbo.BusinessAccounts", new[] { "CustomerId" });
            DropTable("dbo.CheckingTransactions");
            DropTable("dbo.CheckingAccounts");
            DropTable("dbo.BusinessTransactions");
            DropTable("dbo.Customers");
            DropTable("dbo.BusinessAccounts");
        }
    }
}
