using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WatApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDs19PropertyToWorkDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DS2019",
                table: "ImportantInfos");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndOfWork",
                table: "ImportantInfos",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartOfWork",
                table: "ImportantInfos",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndOfWork",
                table: "ImportantInfos");

            migrationBuilder.DropColumn(
                name: "StartOfWork",
                table: "ImportantInfos");

            migrationBuilder.AddColumn<string>(
                name: "DS2019",
                table: "ImportantInfos",
                type: "text",
                nullable: true);
        }
    }
}
