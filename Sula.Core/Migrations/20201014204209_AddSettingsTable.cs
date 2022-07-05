using Microsoft.EntityFrameworkCore.Migrations;

namespace Sula.Core.Migrations
{
    public partial class AddSettingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TemperatureSetting",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "Limit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Temperature = table.Column<int>(nullable: false),
                    Language = table.Column<int>(nullable: false),
                    // UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                    // table.ForeignKey(
                    //     name: "FK_Settings_AspNetUsers_UserId",
                    //     column: x => x.UserId,
                    //     principalTable: "AspNetUsers",
                    //     principalColumn: "Id",
                    //     onDelete: ReferentialAction.Restrict);
                });

            // migrationBuilder.CreateIndex(
            //     name: "IX_Settings_UserId",
            //     table: "Settings",
            //     column: "UserId",
            //     unique: true,
            //     filter: "[UserId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "Limit");

            migrationBuilder.AddColumn<int>(
                name: "TemperatureSetting",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
