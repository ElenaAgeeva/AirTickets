namespace BookTickets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isdeleted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tickets", "IsDeleted");
        }
    }
}
