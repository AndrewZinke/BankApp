namespace BankApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerController2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BusinessAccounts", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.CheckingAccounts", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CheckingAccounts", "Active");
            DropColumn("dbo.BusinessAccounts", "Active");
        }
    }
}
