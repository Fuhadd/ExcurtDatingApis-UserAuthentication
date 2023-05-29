using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserAuthenticationService.Data.Migrations
{
    public partial class InlueLikealityfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Likeability",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Likeability",
                table: "AspNetUsers");
        }
    }
}
