namespace RMS.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changed_and_updated_Unprocessed_to_ZkTimeClockingRecord : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UnprocessedZkTimeData", newName: "ZkTimeClockingRecord");
            AddColumn("dbo.ZkTimeClockingRecord", "TshiftId", c => c.Long());
            AddColumn("dbo.ZkTimeClockingRecord", "IsMatched", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ZkTimeClockingRecord", "IsMatched");
            DropColumn("dbo.ZkTimeClockingRecord", "TshiftId");
            RenameTable(name: "dbo.ZkTimeClockingRecord", newName: "UnprocessedZkTimeData");
        }
    }
}
