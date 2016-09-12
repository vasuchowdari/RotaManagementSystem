namespace RMS.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Reason_and_Notes_fields_to_ShiftProfile_Entity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShiftProfile", "Reason", c => c.String());
            AddColumn("dbo.ShiftProfile", "Notes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShiftProfile", "Notes");
            DropColumn("dbo.ShiftProfile", "Reason");
        }
    }
}
