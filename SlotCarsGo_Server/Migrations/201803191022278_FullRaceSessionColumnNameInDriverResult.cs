namespace SlotCarsGo_Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FullRaceSessionColumnNameInDriverResult : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DriverResults", "CarId", "dbo.Cars");
            DropIndex("dbo.DriverResults", new[] { "CarId" });
            RenameColumn(table: "dbo.DriverResults", name: "SessionId", newName: "RaceSessionId");
            RenameIndex(table: "dbo.DriverResults", name: "IX_SessionId", newName: "IX_RaceSessionId");
            AlterColumn("dbo.DriverResults", "CarId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.DriverResults", "CarId");
            AddForeignKey("dbo.DriverResults", "CarId", "dbo.Cars", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DriverResults", "CarId", "dbo.Cars");
            DropIndex("dbo.DriverResults", new[] { "CarId" });
            AlterColumn("dbo.DriverResults", "CarId", c => c.String(maxLength: 128));
            RenameIndex(table: "dbo.DriverResults", name: "IX_RaceSessionId", newName: "IX_SessionId");
            RenameColumn(table: "dbo.DriverResults", name: "RaceSessionId", newName: "SessionId");
            CreateIndex("dbo.DriverResults", "CarId");
            AddForeignKey("dbo.DriverResults", "CarId", "dbo.Cars", "Id");
        }
    }
}
