using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sula.Core.Migrations
{
    public partial class ModfiyLimits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Limit",
                table: "Limit");

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "Limit",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Limit",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "AlertTime",
                table: "Limit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConfirmationCode",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Limit",
                table: "Limit",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Limit",
                table: "Limit");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Limit");

            migrationBuilder.DropColumn(
                name: "AlertTime",
                table: "Limit");

            migrationBuilder.DropColumn(
                name: "ConfirmationCode",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "Limit",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Limit",
                table: "Limit",
                columns: new[] { "Operator", "DataType", "Value" });
        }
    }
}
