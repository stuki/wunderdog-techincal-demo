using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sula.Core.Migrations
{
    public partial class AddOrdersAndTextMessageLogging : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Settings_AspNetUsers_UserId",
                table: "Settings");

            migrationBuilder.DropIndex(
                name: "IX_Settings_UserId",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ConfirmationCode",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<bool>(
                name: "IsActivated",
                table: "Sensors",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "OpenIddictTokens",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "OpenIddictTokens",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RedemptionDate",
                table: "OpenIddictTokens",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "OpenIddictAuthorizations",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SettingsId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeCustomerId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeSubscriptionId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Line1 = table.Column<string>(nullable: true),
                    Line2 = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    StripeSessionId = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TextMessages",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    SentAt = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(nullable: true),
                    ErrorCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    OrderId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItem_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AddressId",
                table: "AspNetUsers",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SettingsId",
                table: "AspNetUsers",
                column: "SettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderId",
                table: "OrderItem",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Address_AddressId",
                table: "AspNetUsers",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Settings_SettingsId",
                table: "AspNetUsers",
                column: "SettingsId",
                principalTable: "Settings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Address_AddressId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Settings_SettingsId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "OrderItem");

            migrationBuilder.DropTable(
                name: "TextMessages");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AddressId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SettingsId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsActivated",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "RedemptionDate",
                table: "OpenIddictTokens");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SettingsId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StripeCustomerId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StripeSubscriptionId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Settings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ExpirationDate",
                table: "OpenIddictTokens",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreationDate",
                table: "OpenIddictTokens",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreationDate",
                table: "OpenIddictAuthorizations",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConfirmationCode",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Settings_UserId",
                table: "Settings",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Settings_AspNetUsers_UserId",
                table: "Settings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
