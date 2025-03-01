using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    State = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_Categories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    State = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_Categories_T_Categories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "T_Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_T_Categories_T_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "T_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_Images",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Path = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    State = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_Images_T_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "T_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_Papers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Score = table.Column<double>(type: "float", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    State = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Papers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_Papers_T_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "T_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_Topics",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    Score = table.Column<double>(type: "float", nullable: false),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    State = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Topics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_Topics_T_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "T_Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_T_Topics_T_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "T_Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "T_Papers_Topics",
                columns: table => new
                {
                    PapersId = table.Column<long>(type: "bigint", nullable: false),
                    TopicsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Papers_Topics", x => new { x.PapersId, x.TopicsId });
                    table.ForeignKey(
                        name: "FK_T_Papers_Topics_T_Papers_PapersId",
                        column: x => x.PapersId,
                        principalTable: "T_Papers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_Papers_Topics_T_Topics_TopicsId",
                        column: x => x.TopicsId,
                        principalTable: "T_Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_Categories_ParentId",
                table: "T_Categories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Categories_UserId",
                table: "T_Categories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Images_UserId",
                table: "T_Images",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Papers_UserId",
                table: "T_Papers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Papers_Topics_TopicsId",
                table: "T_Papers_Topics",
                column: "TopicsId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Topics_CategoryId",
                table: "T_Topics",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Topics_UserId",
                table: "T_Topics",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_Images");

            migrationBuilder.DropTable(
                name: "T_Papers_Topics");

            migrationBuilder.DropTable(
                name: "T_Papers");

            migrationBuilder.DropTable(
                name: "T_Topics");

            migrationBuilder.DropTable(
                name: "T_Categories");

            migrationBuilder.DropTable(
                name: "T_Users");
        }
    }
}
