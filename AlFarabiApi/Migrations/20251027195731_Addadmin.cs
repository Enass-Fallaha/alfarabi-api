using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlFarabiApi.Migrations
{
    /// <inheritdoc />
    public partial class Addadmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsLogIn", "Name", "Password", "Phone", "Role" },
                values: new object[] { 1, "Admin@enass.com", false, "Enass", "enass023", "0940053146", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
