namespace SlotCarsGo_Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Tracks_Users_Link : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TrackApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.TrackApplicationUsers", "Track_Id", "dbo.Tracks");
            DropIndex("dbo.TrackApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.TrackApplicationUsers", new[] { "Track_Id" });
            DropTable("dbo.TrackApplicationUsers");
        }
    }
}
