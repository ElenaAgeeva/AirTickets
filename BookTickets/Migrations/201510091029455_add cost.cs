namespace BookTickets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcost : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Routes", "Cost", c => c.Int(nullable: false));
            DropColumn("dbo.Tickets", "Cost");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tickets", "Cost", c => c.Int(nullable: false));
            DropColumn("dbo.Routes", "Cost");
        }
    }
}
