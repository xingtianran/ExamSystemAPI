using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class PaperTeamState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "T_Papers_Teams",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "T_Papers_Teams");
        }
    }
}
