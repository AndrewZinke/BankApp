namespace BankApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CheckingAccountInterest : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CheckingAccounts", "Interest");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CheckingAccounts", "Interest", c => c.Double(nullable: false));
        }
    }
}
