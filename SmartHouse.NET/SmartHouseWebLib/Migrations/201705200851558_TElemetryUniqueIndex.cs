namespace SmartHouseWebLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class TElemetryUniqueIndex : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.TelemetryDatas", "CreatedUtc", unique: true, name: "IDX_Date");
        }

        public override void Down()
        {
            DropIndex("dbo.TelemetryDatas", "IDX_Date");
        }
    }
}
