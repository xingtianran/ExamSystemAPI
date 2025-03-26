using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Column1",
                table: "T_Topics",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Column2",
                table: "T_Topics",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Column3",
                table: "T_Topics",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Column4",
                table: "T_Topics",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Column5",
                table: "T_Topics",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Column6",
                table: "T_Topics",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Column1",
                table: "T_Topics");

            migrationBuilder.DropColumn(
                name: "Column2",
                table: "T_Topics");

            migrationBuilder.DropColumn(
                name: "Column3",
                table: "T_Topics");

            migrationBuilder.DropColumn(
                name: "Column4",
                table: "T_Topics");

            migrationBuilder.DropColumn(
                name: "Column5",
                table: "T_Topics");

            migrationBuilder.DropColumn(
                name: "Column6",
                table: "T_Topics");
        }
    }
}
