using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserAuthenticationService.Data.Migrations
{
    public partial class Changedimagebase64tourl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Base64",
                table: "UserImages");

            migrationBuilder.RenameColumn(
                name: "ContentType",
                table: "UserImages",
                newName: "ImageUrl");

            migrationBuilder.AddColumn<int>(
                name: "ImageSize",
                table: "UserImages",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageSize",
                table: "UserImages");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "UserImages",
                newName: "ContentType");

            migrationBuilder.AddColumn<string>(
                name: "Base64",
                table: "UserImages",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
