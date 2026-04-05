using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoveJourney.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddJourneyReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "journey_reviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    JourneyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CoupleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rating = table.Column<short>(type: "smallint", nullable: false),
                    ReviewText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Highlights = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WouldRevisit = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_journey_reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_journey_reviews_couples_CoupleId",
                        column: x => x.CoupleId,
                        principalTable: "couples",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_journey_reviews_journeys_JourneyId",
                        column: x => x.JourneyId,
                        principalTable: "journeys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_journey_reviews_CoupleId",
                table: "journey_reviews",
                column: "CoupleId");

            migrationBuilder.CreateIndex(
                name: "IX_journey_reviews_JourneyId",
                table: "journey_reviews",
                column: "JourneyId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "journey_reviews");
        }
    }
}
