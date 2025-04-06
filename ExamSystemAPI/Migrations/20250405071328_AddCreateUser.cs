using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddCreateUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreateUserId",
                table: "T_Teams",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_T_Teams_CreateUserId",
                table: "T_Teams",
                column: "CreateUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Teams_AspNetUsers_CreateUserId",
                table: "T_Teams",
                column: "CreateUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_Teams_AspNetUsers_CreateUserId",
                table: "T_Teams");

            migrationBuilder.DropIndex(
                name: "IX_T_Teams_CreateUserId",
                table: "T_Teams");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "T_Teams");
        }
    }
}
