namespace SlotCarsGo_Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBestLapTimeToCarAndTrack : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tracks", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Track_Id", "dbo.Tracks");
            DropForeignKey("dbo.Tracks", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Cars", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.BestLapTimes", "CarId", "dbo.Cars");
            DropForeignKey("dbo.Cars", "TrackId", "dbo.Tracks");
            DropIndex("dbo.Cars", new[] { "ApplicationUserId" });
            DropIndex("dbo.Cars", new[] { "TrackId" });
            DropIndex("dbo.AspNetUsers", new[] { "Track_Id" });
            DropIndex("dbo.Tracks", new[] { "ApplicationUserId" });
            DropIndex("dbo.Tracks", new[] { "ApplicationUser_Id" });
            CreateTable(
                "dbo.TrackApplicationUsers",
                c => new
                    {
                        Track_Id = c.String(nullable: false, maxLength: 128),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Track_Id, t.ApplicationUser_Id })
                .ForeignKey("dbo.Tracks", t => t.Track_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.Track_Id)
                .Index(t => t.ApplicationUser_Id);
            
            AddColumn("dbo.Cars", "BestLapTimeId", c => c.String(maxLength: 128));
            AddColumn("dbo.BestLapTimes", "Car_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Tracks", "BestLapTimeId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Cars", "TrackId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.BestLapTimes", "Car_Id");
            CreateIndex("dbo.Cars", "TrackId");
            CreateIndex("dbo.Cars", "BestLapTimeId");
            CreateIndex("dbo.Tracks", "BestLapTimeId");
            AddForeignKey("dbo.Cars", "BestLapTimeId", "dbo.BestLapTimes", "Id");
            AddForeignKey("dbo.Tracks", "BestLapTimeId", "dbo.BestLapTimes", "Id");
            AddForeignKey("dbo.BestLapTimes", "Car_Id", "dbo.Cars", "Id");
            AddForeignKey("dbo.Cars", "TrackId", "dbo.Tracks", "Id", cascadeDelete: true);
            DropColumn("dbo.Cars", "TrackRecord");
            DropColumn("dbo.Cars", "ApplicationUserId");
            DropColumn("dbo.AspNetUsers", "Track_Id");
            DropColumn("dbo.Tracks", "ApplicationUserId");
            DropColumn("dbo.Tracks", "TrackRecord");
            DropColumn("dbo.Tracks", "ApplicationUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tracks", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Tracks", "TrackRecord", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.Tracks", "ApplicationUserId", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "Track_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Cars", "ApplicationUserId", c => c.String(maxLength: 128));
            AddColumn("dbo.Cars", "TrackRecord", c => c.Time(nullable: false, precision: 7));
            DropForeignKey("dbo.Cars", "TrackId", "dbo.Tracks");
            DropForeignKey("dbo.BestLapTimes", "Car_Id", "dbo.Cars");
            DropForeignKey("dbo.Tracks", "BestLapTimeId", "dbo.BestLapTimes");
            DropForeignKey("dbo.TrackApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.TrackApplicationUsers", "Track_Id", "dbo.Tracks");
            DropForeignKey("dbo.Cars", "BestLapTimeId", "dbo.BestLapTimes");
            DropIndex("dbo.TrackApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.TrackApplicationUsers", new[] { "Track_Id" });
            DropIndex("dbo.Tracks", new[] { "BestLapTimeId" });
            DropIndex("dbo.Cars", new[] { "BestLapTimeId" });
            DropIndex("dbo.Cars", new[] { "TrackId" });
            DropIndex("dbo.BestLapTimes", new[] { "Car_Id" });
            AlterColumn("dbo.Cars", "TrackId", c => c.String(maxLength: 128));
            DropColumn("dbo.Tracks", "BestLapTimeId");
            DropColumn("dbo.BestLapTimes", "Car_Id");
            DropColumn("dbo.Cars", "BestLapTimeId");
            DropTable("dbo.TrackApplicationUsers");
            CreateIndex("dbo.Tracks", "ApplicationUser_Id");
            CreateIndex("dbo.Tracks", "ApplicationUserId");
            CreateIndex("dbo.AspNetUsers", "Track_Id");
            CreateIndex("dbo.Cars", "TrackId");
            CreateIndex("dbo.Cars", "ApplicationUserId");
            AddForeignKey("dbo.Cars", "TrackId", "dbo.Tracks", "Id");
            AddForeignKey("dbo.BestLapTimes", "CarId", "dbo.Cars", "Id");
            AddForeignKey("dbo.Cars", "ApplicationUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Tracks", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "Track_Id", "dbo.Tracks", "Id");
            AddForeignKey("dbo.Tracks", "ApplicationUserId", "dbo.AspNetUsers", "Id");
        }
    }
}
