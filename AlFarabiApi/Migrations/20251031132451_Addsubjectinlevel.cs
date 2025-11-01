using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlFarabiApi.Migrations
{
    /// <inheritdoc />
    public partial class Addsubjectinlevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subjects_LevelId",
                table: "Subjects");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_LevelId",
                table: "Subjects",
                column: "LevelId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subjects_LevelId",
                table: "Subjects");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_LevelId",
                table: "Subjects",
                column: "LevelId");
        }
    }
}
