using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddPaperTopicIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "T_Papers_Topics");

            migrationBuilder.AddColumn<long>(
                name: "TopicIndex",
                table: "T_Papers_Topics",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TopicSetIndex",
                table: "T_Papers_Topics",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TopicIndex",
                table: "T_Papers_Topics");

            migrationBuilder.DropColumn(
                name: "TopicSetIndex",
                table: "T_Papers_Topics");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTime",
                table: "T_Papers_Topics",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
