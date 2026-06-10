using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WatApi.Migrations
{
    /// <inheritdoc />
    public partial class AddImportantInfoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImportantInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    JobOfferId = table.Column<Guid>(type: "uuid", nullable: false),
                    SevisID = table.Column<string>(type: "text", nullable: true),
                    VisaAppointment = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Flight = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DS160 = table.Column<string>(type: "text", nullable: true),
                    DS2019 = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportantInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportantInfos_JobOffers_JobOfferId",
                        column: x => x.JobOfferId,
                        principalTable: "JobOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImportantInfos_JobOfferId",
                table: "ImportantInfos",
                column: "JobOfferId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImportantInfos");
        }
    }
}
