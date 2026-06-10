using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WatApi.Migrations
{
    /// <inheritdoc />
    public partial class UserToOffersOneToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_JobOffers_UserId",
                table: "JobOffers");

            migrationBuilder.CreateIndex(
                name: "IX_JobOffers_UserId",
                table: "JobOffers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_JobOffers_UserId",
                table: "JobOffers");

            migrationBuilder.CreateIndex(
                name: "IX_JobOffers_UserId",
                table: "JobOffers",
                column: "UserId",
                unique: true);
        }
    }
}
