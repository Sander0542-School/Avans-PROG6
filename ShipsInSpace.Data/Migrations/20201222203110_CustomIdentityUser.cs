using Microsoft.EntityFrameworkCore.Migrations;

namespace ShipsInSpace.Data.Migrations
{
    public partial class CustomIdentityUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PilotLicense",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SecretKey",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PilotLicense",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SecretKey",
                table: "AspNetUsers");
        }
    }
}
