namespace SmartHouseWebLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class HeatIndex : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TelemetryDatas", "HeatIndex", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }

        public override void Down()
        {
            DropColumn("dbo.TelemetryDatas", "HeatIndex");
        }
    }
}
