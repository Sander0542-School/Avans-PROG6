using Microsoft.EntityFrameworkCore.Migrations;

namespace ShipsInSpace.Data.Migrations
{
    public partial class RoleSeeder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ed6e1f94-991e-482e-9d88-308a0a3359dd", "9e44444f-5d62-4f3d-b898-f3e230d8a73d", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "83eabd0c-ac64-47c5-ac5b-fa8d842af737", "fbb12815-9a6d-42e7-9737-d202c0953f08", "Pirate", "PIRATE" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "83eabd0c-ac64-47c5-ac5b-fa8d842af737");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ed6e1f94-991e-482e-9d88-308a0a3359dd");
        }
    }
}
