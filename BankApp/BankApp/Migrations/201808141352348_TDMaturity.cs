namespace BankApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TDMaturity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TermDeposits", "HasMatured", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TermDeposits", "HasMatured");
        }
    }
}
