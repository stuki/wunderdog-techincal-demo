using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sula.Core.Migrations
{
    public partial class AddLimitsAndSensorData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.CreateTable(
                name: "Sensors",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    SensorType = table.Column<int>(nullable: false),
                    Latitude = table.Column<double>(nullable: true),
                    Longitude = table.Column<double>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sensors_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Limit",
                columns: table => new
                {
                    DataType = table.Column<int>(nullable: false),
                    Operator = table.Column<int>(nullable: false),
                    Value = table.Column<decimal>(nullable: false),
                    DeviceId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Limit", x => new { x.Operator, x.DataType, x.Value });
                    table.ForeignKey(
                        name: "FK_Limit_Sensors_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Sensors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SensorData",
                columns: table => new
                {
                    Time = table.Column<DateTimeOffset>(nullable: false),
                    DeviceId = table.Column<string>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Value = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorData", x => new { x.DeviceId, x.Type, x.Time });
                    table.ForeignKey(
                        name: "FK_SensorData_Sensors_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Sensors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Limit_DeviceId",
                table: "Limit",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_UserId",
                table: "Sensors",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Limit");

            migrationBuilder.DropTable(
                name: "SensorData");

            migrationBuilder.DropTable(
                name: "Sensors");

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DeviceType = table.Column<int>(type: "int", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_UserId",
                table: "Devices",
                column: "UserId");
        }
    }
}
