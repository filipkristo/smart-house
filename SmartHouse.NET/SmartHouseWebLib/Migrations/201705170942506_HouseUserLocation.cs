namespace SmartHouseWebLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class HouseUserLocation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Houses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Address = c.String(maxLength: 255),
                        City = c.String(maxLength: 255),
                        ImageUrl = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.UserLocations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Status = c.Int(nullable: false),
                        UpdatedUtc = c.DateTime(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            AddColumn("dbo.AspNetUsers", "HouseId", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "HouseId");
            AddForeignKey("dbo.AspNetUsers", "HouseId", "dbo.Houses", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.UserLocations", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "HouseId", "dbo.Houses");
            DropIndex("dbo.UserLocations", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "HouseId" });
            DropColumn("dbo.AspNetUsers", "HouseId");
            DropTable("dbo.UserLocations");
            DropTable("dbo.Houses");
        }
    }
}
