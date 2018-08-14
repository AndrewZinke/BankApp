namespace BankApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransactionTypes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BusinessTransactions", "Type", c => c.String());
            AddColumn("dbo.CheckingTransactions", "Type", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CheckingTransactions", "Type");
            DropColumn("dbo.BusinessTransactions", "Type");
        }
    }
}
