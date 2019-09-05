using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartHouseDataStore.Migrations
{
    public partial class DataSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DeviceTypes",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "", "Amplifier" },
                    { 2, "", "TV" },
                    { 3, "", "Music Player" },
                    { 4, "", "Lights" },
                    { 5, "", "Switch" },
                    { 6, "", "Telemetry Device" }
                });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "Id", "CrlAssembly", "CrlType", "Description", "DeviceTypeId", "Name" },
                values: new object[,]
                {
                    { 3, "", "", "", 1, "Pandora Player" },
                    { 4, "", "", "", 2, "Philips" },
                    { 1, "", "", "", 3, "Deezer Player" },
                    { 2, "", "", "", 3, "Pandora Player" },
                    { 5, "", "", "", 4, "Yeelight - living room" },
                    { 6, "", "", "", 4, "Yeelight - bedroom" },
                    { 7, "", "", "", 5, "Restart Router" },
                    { 8, "", "", "", 6, "Telemetry" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "DeviceTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "DeviceTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "DeviceTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "DeviceTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "DeviceTypes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "DeviceTypes",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
