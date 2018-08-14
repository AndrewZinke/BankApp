namespace BankApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedCheckingTransaction : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.CheckingTransactions", "AccountId");
            AddForeignKey("dbo.CheckingTransactions", "AccountId", "dbo.CheckingAccounts", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CheckingTransactions", "AccountId", "dbo.CheckingAccounts");
            DropIndex("dbo.CheckingTransactions", new[] { "AccountId" });
        }
    }
}
