using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "T_Images");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "T_Images");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "T_Images");

            migrationBuilder.AddColumn<string>(
                name: "Origin",
                table: "T_Images",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Origin",
                table: "T_Images");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "T_Images",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "T_Images",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "T_Images",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");
        }
    }
}
