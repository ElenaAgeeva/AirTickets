namespace BookTickets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConditionTypeFromStringToEnum : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tickets", "Condition", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tickets", "Condition", c => c.String());
        }
    }
}
