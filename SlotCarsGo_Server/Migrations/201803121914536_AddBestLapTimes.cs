namespace SlotCarsGo_Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBestLapTimes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BestLapTimes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        LapTimeId = c.String(maxLength: 128),
                        ApplicationUserId = c.String(maxLength: 128),
                        CarId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Cars", t => t.CarId)
                .ForeignKey("dbo.LapTimes", t => t.LapTimeId)
                .Index(t => t.LapTimeId)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.CarId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BestLapTimes", "LapTimeId", "dbo.LapTimes");
            DropForeignKey("dbo.BestLapTimes", "CarId", "dbo.Cars");
            DropForeignKey("dbo.BestLapTimes", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.BestLapTimes", new[] { "CarId" });
            DropIndex("dbo.BestLapTimes", new[] { "ApplicationUserId" });
            DropIndex("dbo.BestLapTimes", new[] { "LapTimeId" });
            DropTable("dbo.BestLapTimes");
        }
    }
}
