namespace BookTickets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingLogNameColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "LogName", c => c.String(nullable: false, maxLength: 450));
            CreateIndex("dbo.People", "LogName", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.People", new[] { "LogName" });
            DropColumn("dbo.People", "LogName");
        }
    }
}
