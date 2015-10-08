namespace BookTickets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Person : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.People", "Ticket_TicketID", "dbo.Tickets");
            DropIndex("dbo.People", new[] { "Ticket_TicketID" });
            AddColumn("dbo.Tickets", "Person_PersonID", c => c.Int());
            AddForeignKey("dbo.Tickets", "Person_PersonID", "dbo.People", "PersonID");
            CreateIndex("dbo.Tickets", "Person_PersonID");
            DropColumn("dbo.People", "Ticket_TicketID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.People", "Ticket_TicketID", c => c.Int());
            DropIndex("dbo.Tickets", new[] { "Person_PersonID" });
            DropForeignKey("dbo.Tickets", "Person_PersonID", "dbo.People");
            DropColumn("dbo.Tickets", "Person_PersonID");
            CreateIndex("dbo.People", "Ticket_TicketID");
            AddForeignKey("dbo.People", "Ticket_TicketID", "dbo.Tickets", "TicketID");
        }
    }
}
