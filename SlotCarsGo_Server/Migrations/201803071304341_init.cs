namespace SlotCarsGo_Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        TrackRecord = c.Time(nullable: false, precision: 7),
                        ImageName = c.String(),
                        ApplicationUserId = c.String(maxLength: 128),
                        TrackId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Tracks", t => t.TrackId)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.TrackId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ImageName = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Tracks",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ApplicationUserId = c.String(),
                        Name = c.String(nullable: false),
                        Length = c.Single(nullable: false),
                        Secret = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DriverResults",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Position = c.Int(nullable: false),
                        Laps = c.Int(nullable: false),
                        Finished = c.Boolean(nullable: false),
                        Fuel = c.Single(nullable: false),
                        TotalTime = c.Time(nullable: false, precision: 7),
                        TimeOffPace = c.Time(nullable: false, precision: 7),
                        BestLapTime = c.Time(nullable: false, precision: 7),
                        ControllerNumber = c.Int(nullable: false),
                        ApplicationUserId = c.String(maxLength: 128),
                        SessionId = c.String(nullable: false, maxLength: 128),
                        CarId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Cars", t => t.CarId)
                .ForeignKey("dbo.RaceSessions", t => t.SessionId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.SessionId)
                .Index(t => t.CarId);
            
            CreateTable(
                "dbo.RaceSessions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        TrackId = c.String(maxLength: 128),
                        RaceTypeId = c.String(),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        FuelEnabled = c.Boolean(nullable: false),
                        NumberOfDrivers = c.Int(nullable: false),
                        RaceLimitValue = c.Int(nullable: false),
                        RaceLength = c.Time(nullable: false, precision: 7),
                        LapsNotDuration = c.Boolean(nullable: false),
                        CrashPenalty = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tracks", t => t.TrackId)
                .Index(t => t.TrackId);
            
            CreateTable(
                "dbo.Drivers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        TrackId = c.String(maxLength: 128),
                        ControllerId = c.Int(nullable: false),
                        ApplicationUserId = c.String(maxLength: 128),
                        CarId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Cars", t => t.CarId)
                .ForeignKey("dbo.Tracks", t => t.TrackId)
                .Index(t => t.TrackId)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.CarId);
            
            CreateTable(
                "dbo.LapTimes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        LapNumber = c.Int(nullable: false),
                        Time = c.Time(nullable: false, precision: 7),
                        DriverId = c.String(maxLength: 128),
                        RaceSessionId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Drivers", t => t.DriverId)
                .ForeignKey("dbo.RaceSessions", t => t.RaceSessionId)
                .Index(t => t.DriverId)
                .Index(t => t.RaceSessionId);
            
            CreateTable(
                "dbo.RaceTypes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        Rules = c.String(nullable: false),
                        Symbol = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.LapTimes", "RaceSessionId", "dbo.RaceSessions");
            DropForeignKey("dbo.LapTimes", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.Drivers", "TrackId", "dbo.Tracks");
            DropForeignKey("dbo.Drivers", "CarId", "dbo.Cars");
            DropForeignKey("dbo.Drivers", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.DriverResults", "SessionId", "dbo.RaceSessions");
            DropForeignKey("dbo.RaceSessions", "TrackId", "dbo.Tracks");
            DropForeignKey("dbo.DriverResults", "CarId", "dbo.Cars");
            DropForeignKey("dbo.DriverResults", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Cars", "TrackId", "dbo.Tracks");
            DropForeignKey("dbo.Cars", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.LapTimes", new[] { "RaceSessionId" });
            DropIndex("dbo.LapTimes", new[] { "DriverId" });
            DropIndex("dbo.Drivers", new[] { "CarId" });
            DropIndex("dbo.Drivers", new[] { "ApplicationUserId" });
            DropIndex("dbo.Drivers", new[] { "TrackId" });
            DropIndex("dbo.RaceSessions", new[] { "TrackId" });
            DropIndex("dbo.DriverResults", new[] { "CarId" });
            DropIndex("dbo.DriverResults", new[] { "SessionId" });
            DropIndex("dbo.DriverResults", new[] { "ApplicationUserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Cars", new[] { "TrackId" });
            DropIndex("dbo.Cars", new[] { "ApplicationUserId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RaceTypes");
            DropTable("dbo.LapTimes");
            DropTable("dbo.Drivers");
            DropTable("dbo.RaceSessions");
            DropTable("dbo.DriverResults");
            DropTable("dbo.Tracks");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Cars");
        }
    }
}
