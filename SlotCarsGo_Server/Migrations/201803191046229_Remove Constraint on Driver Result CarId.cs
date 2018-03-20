namespace SlotCarsGo_Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveConstraintonDriverResultCarId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DriverResults", "CarId", "dbo.Cars");
            DropIndex("dbo.DriverResults", new[] { "CarId" });
            AddColumn("dbo.Cars", "Selectable", c => c.Boolean(nullable: false));
            AlterColumn("dbo.DriverResults", "CarId", c => c.String(maxLength: 128));
            CreateIndex("dbo.DriverResults", "CarId");
            AddForeignKey("dbo.DriverResults", "CarId", "dbo.Cars", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DriverResults", "CarId", "dbo.Cars");
            DropIndex("dbo.DriverResults", new[] { "CarId" });
            AlterColumn("dbo.DriverResults", "CarId", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Cars", "Selectable");
            CreateIndex("dbo.DriverResults", "CarId");
            AddForeignKey("dbo.DriverResults", "CarId", "dbo.Cars", "Id", cascadeDelete: true);
        }
    }
}
