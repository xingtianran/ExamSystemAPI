using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddPaperTopic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_Papers_Topics_T_Papers_PapersId",
                table: "T_Papers_Topics");

            migrationBuilder.DropForeignKey(
                name: "FK_T_Papers_Topics_T_Topics_TopicsId",
                table: "T_Papers_Topics");

            migrationBuilder.RenameColumn(
                name: "TopicsId",
                table: "T_Papers_Topics",
                newName: "TopicId");

            migrationBuilder.RenameColumn(
                name: "PapersId",
                table: "T_Papers_Topics",
                newName: "PaperId");

            migrationBuilder.RenameIndex(
                name: "IX_T_Papers_Topics_TopicsId",
                table: "T_Papers_Topics",
                newName: "IX_T_Papers_Topics_TopicId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "T_Papers_Topics",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Papers_Topics_T_Papers_PaperId",
                table: "T_Papers_Topics",
                column: "PaperId",
                principalTable: "T_Papers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_T_Papers_Topics_T_Topics_TopicId",
                table: "T_Papers_Topics",
                column: "TopicId",
                principalTable: "T_Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_Papers_Topics_T_Papers_PaperId",
                table: "T_Papers_Topics");

            migrationBuilder.DropForeignKey(
                name: "FK_T_Papers_Topics_T_Topics_TopicId",
                table: "T_Papers_Topics");

            migrationBuilder.RenameColumn(
                name: "TopicId",
                table: "T_Papers_Topics",
                newName: "TopicsId");

            migrationBuilder.RenameColumn(
                name: "PaperId",
                table: "T_Papers_Topics",
                newName: "PapersId");

            migrationBuilder.RenameIndex(
                name: "IX_T_Papers_Topics_TopicId",
                table: "T_Papers_Topics",
                newName: "IX_T_Papers_Topics_TopicsId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "T_Papers_Topics",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Papers_Topics_T_Papers_PapersId",
                table: "T_Papers_Topics",
                column: "PapersId",
                principalTable: "T_Papers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_T_Papers_Topics_T_Topics_TopicsId",
                table: "T_Papers_Topics",
                column: "TopicsId",
                principalTable: "T_Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
