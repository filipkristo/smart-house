namespace SmartHouseWebLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TelemetryRoomTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        ImageUrl = c.String(maxLength: 255),
                        HouseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Houses", t => t.HouseId, cascadeDelete: true)
                .Index(t => t.HouseId);
            
            CreateTable(
                "dbo.TelemetryDatas",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Temperature = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Humidity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GasValue = c.Int(nullable: false),
                        CreatedUtc = c.DateTime(nullable: false),
                        RoomId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rooms", t => t.RoomId, cascadeDelete: true)
                .Index(t => t.RoomId);
            
            AddColumn("dbo.UserLocations", "DeviceInfo", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TelemetryDatas", "RoomId", "dbo.Rooms");
            DropForeignKey("dbo.Rooms", "HouseId", "dbo.Houses");
            DropIndex("dbo.TelemetryDatas", new[] { "RoomId" });
            DropIndex("dbo.Rooms", new[] { "HouseId" });
            DropColumn("dbo.UserLocations", "DeviceInfo");
            DropTable("dbo.TelemetryDatas");
            DropTable("dbo.Rooms");
        }
    }
}
