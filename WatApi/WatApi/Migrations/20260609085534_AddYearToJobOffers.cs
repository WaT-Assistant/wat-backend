using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WatApi.Migrations
{
    /// <inheritdoc />
    public partial class AddYearToJobOffers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "JobOffers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "JobOffers");
        }
    }
}
