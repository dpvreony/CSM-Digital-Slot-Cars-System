namespace SlotCarsGo_Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTrackOwnerEmail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tracks", "OwnerEmail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tracks", "OwnerEmail");
        }
    }
}
