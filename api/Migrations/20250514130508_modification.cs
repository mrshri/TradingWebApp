using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class modification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a975974-d8e3-4302-adab-cdf9bf2129ea");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "42ef24ec-ad9d-4fd1-928d-74edb812bdaf");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7568361b-4ac6-42a9-9750-6b56aa69bcaf", null, "User", "USER" },
                    { "e7069752-32bb-4876-99d4-ec026ad38bb0", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7568361b-4ac6-42a9-9750-6b56aa69bcaf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e7069752-32bb-4876-99d4-ec026ad38bb0");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1a975974-d8e3-4302-adab-cdf9bf2129ea", null, "Admin", "ADMIN" },
                    { "42ef24ec-ad9d-4fd1-928d-74edb812bdaf", null, "User", "USER" }
                });
        }
    }
}
