namespace SlotCarsGo_Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTrackRecord : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TrackApplicationUsers", "Track_Id", "dbo.Tracks");
            DropForeignKey("dbo.TrackApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.TrackApplicationUsers", new[] { "Track_Id" });
            DropIndex("dbo.TrackApplicationUsers", new[] { "ApplicationUser_Id" });
            AddColumn("dbo.AspNetUsers", "Track_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Tracks", "TrackRecord", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.Tracks", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Tracks", "ApplicationUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUsers", "Track_Id");
            CreateIndex("dbo.Tracks", "ApplicationUserId");
            CreateIndex("dbo.Tracks", "ApplicationUser_Id");
            AddForeignKey("dbo.Tracks", "ApplicationUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "Track_Id", "dbo.Tracks", "Id");
            AddForeignKey("dbo.Tracks", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            DropTable("dbo.TrackApplicationUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TrackApplicationUsers",
                c => new
                    {
                        Track_Id = c.String(nullable: false, maxLength: 128),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Track_Id, t.ApplicationUser_Id });
            
            DropForeignKey("dbo.Tracks", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Track_Id", "dbo.Tracks");
            DropForeignKey("dbo.Tracks", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Tracks", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Tracks", new[] { "ApplicationUserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Track_Id" });
            AlterColumn("dbo.Tracks", "ApplicationUserId", c => c.String());
            DropColumn("dbo.Tracks", "ApplicationUser_Id");
            DropColumn("dbo.Tracks", "TrackRecord");
            DropColumn("dbo.AspNetUsers", "Track_Id");
            CreateIndex("dbo.TrackApplicationUsers", "ApplicationUser_Id");
            CreateIndex("dbo.TrackApplicationUsers", "Track_Id");
            AddForeignKey("dbo.TrackApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TrackApplicationUsers", "Track_Id", "dbo.Tracks", "Id", cascadeDelete: true);
        }
    }
}
