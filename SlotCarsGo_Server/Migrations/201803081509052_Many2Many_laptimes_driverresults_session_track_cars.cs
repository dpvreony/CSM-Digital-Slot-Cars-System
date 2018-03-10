namespace SlotCarsGo_Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Many2Many_laptimes_driverresults_session_track_cars : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LapTimes", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.LapTimes", "RaceSessionId", "dbo.RaceSessions");
            DropIndex("dbo.LapTimes", new[] { "DriverId" });
            DropIndex("dbo.LapTimes", new[] { "RaceSessionId" });
            AddColumn("dbo.LapTimes", "DriverResultId", c => c.String(maxLength: 128));
            CreateIndex("dbo.LapTimes", "DriverResultId");
            AddForeignKey("dbo.LapTimes", "DriverResultId", "dbo.DriverResults", "Id");
            DropColumn("dbo.LapTimes", "DriverId");
            DropColumn("dbo.LapTimes", "RaceSessionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LapTimes", "RaceSessionId", c => c.String(maxLength: 128));
            AddColumn("dbo.LapTimes", "DriverId", c => c.String(maxLength: 128));
            DropForeignKey("dbo.LapTimes", "DriverResultId", "dbo.DriverResults");
            DropIndex("dbo.LapTimes", new[] { "DriverResultId" });
            DropColumn("dbo.LapTimes", "DriverResultId");
            CreateIndex("dbo.LapTimes", "RaceSessionId");
            CreateIndex("dbo.LapTimes", "DriverId");
            AddForeignKey("dbo.LapTimes", "RaceSessionId", "dbo.RaceSessions", "Id");
            AddForeignKey("dbo.LapTimes", "DriverId", "dbo.Drivers", "Id");
        }
    }
}
