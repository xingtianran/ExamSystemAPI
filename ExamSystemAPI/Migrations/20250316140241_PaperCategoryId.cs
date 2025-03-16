using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class PaperCategoryId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CategoryId",
                table: "T_Papers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_T_Papers_CategoryId",
                table: "T_Papers",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Papers_T_Categories_CategoryId",
                table: "T_Papers",
                column: "CategoryId",
                principalTable: "T_Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_Papers_T_Categories_CategoryId",
                table: "T_Papers");

            migrationBuilder.DropIndex(
                name: "IX_T_Papers_CategoryId",
                table: "T_Papers");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "T_Papers");
        }
    }
}
