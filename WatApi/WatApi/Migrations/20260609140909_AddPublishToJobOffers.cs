using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WatApi.Migrations
{
    /// <inheritdoc />
    public partial class AddPublishToJobOffers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Feedback",
                table: "JobOffers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "JobOffers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "JobOffers",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Feedback",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "JobOffers");
        }
    }
}
