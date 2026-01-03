using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlFarabiApi.Migrations
{
    /// <inheritdoc />
    public partial class add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectUser_Subjects_SubjectId",
                table: "SubjectUser");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectUser_Users_UserId",
                table: "SubjectUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubjectUser",
                table: "SubjectUser");

            migrationBuilder.RenameTable(
                name: "SubjectUser",
                newName: "SubjectUsers");

            migrationBuilder.RenameIndex(
                name: "IX_SubjectUser_UserId",
                table: "SubjectUsers",
                newName: "IX_SubjectUsers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_SubjectUser_SubjectId",
                table: "SubjectUsers",
                newName: "IX_SubjectUsers_SubjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubjectUsers",
                table: "SubjectUsers",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectUsers_Subjects_SubjectId",
                table: "SubjectUsers",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectUsers_Users_UserId",
                table: "SubjectUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectUsers_Subjects_SubjectId",
                table: "SubjectUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectUsers_Users_UserId",
                table: "SubjectUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubjectUsers",
                table: "SubjectUsers");

            migrationBuilder.RenameTable(
                name: "SubjectUsers",
                newName: "SubjectUser");

            migrationBuilder.RenameIndex(
                name: "IX_SubjectUsers_UserId",
                table: "SubjectUser",
                newName: "IX_SubjectUser_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_SubjectUsers_SubjectId",
                table: "SubjectUser",
                newName: "IX_SubjectUser_SubjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubjectUser",
                table: "SubjectUser",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectUser_Subjects_SubjectId",
                table: "SubjectUser",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectUser_Users_UserId",
                table: "SubjectUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
