using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PatoghBackend.Data.Migrations.Main
{
    public partial class V00 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Patogh");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Patogh",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(maxLength: 12, nullable: true),
                    FirstName = table.Column<string>(maxLength: 20, nullable: true),
                    LastName = table.Column<string>(maxLength: 30, nullable: true),
                    Email = table.Column<string>(maxLength: 30, nullable: true),
                    LoginTokenValue = table.Column<string>(maxLength: 10, nullable: true),
                    LoginTokenExpirationTime = table.Column<DateTime>(nullable: false),
                    SessionToken = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dorehamies",
                schema: "Patogh",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    CreatorId = table.Column<long>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    Address = table.Column<string>(maxLength: 200, nullable: true),
                    Size = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    IsPhysical = table.Column<bool>(nullable: false),
                    LocationId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dorehamies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dorehamies_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalSchema: "Patogh",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FavUserDorehamies",
                schema: "Patogh",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(nullable: false),
                    DorehamiId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavUserDorehamies", x => x.Id);
                    table.UniqueConstraint("AK_FavUserDorehamies_DorehamiId_UserId", x => new { x.DorehamiId, x.UserId });
                    table.ForeignKey(
                        name: "FK_FavUserDorehamies_Dorehamies_DorehamiId",
                        column: x => x.DorehamiId,
                        principalSchema: "Patogh",
                        principalTable: "Dorehamies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavUserDorehamies_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Patogh",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GeoLocations",
                schema: "Patogh",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DorehamiId = table.Column<long>(nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(13,10)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(13,10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeoLocations_Dorehamies_DorehamiId",
                        column: x => x.DorehamiId,
                        principalSchema: "Patogh",
                        principalTable: "Dorehamies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JoinUserDorehamies",
                schema: "Patogh",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(nullable: false),
                    DorehamiId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JoinUserDorehamies", x => x.Id);
                    table.UniqueConstraint("AK_JoinUserDorehamies_DorehamiId_UserId", x => new { x.DorehamiId, x.UserId });
                    table.ForeignKey(
                        name: "FK_JoinUserDorehamies_Dorehamies_DorehamiId",
                        column: x => x.DorehamiId,
                        principalSchema: "Patogh",
                        principalTable: "Dorehamies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JoinUserDorehamies_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Patogh",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dorehamies_CreatorId",
                schema: "Patogh",
                table: "Dorehamies",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_FavUserDorehamies_UserId",
                schema: "Patogh",
                table: "FavUserDorehamies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GeoLocations_DorehamiId",
                schema: "Patogh",
                table: "GeoLocations",
                column: "DorehamiId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JoinUserDorehamies_UserId",
                schema: "Patogh",
                table: "JoinUserDorehamies",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavUserDorehamies",
                schema: "Patogh");

            migrationBuilder.DropTable(
                name: "GeoLocations",
                schema: "Patogh");

            migrationBuilder.DropTable(
                name: "JoinUserDorehamies",
                schema: "Patogh");

            migrationBuilder.DropTable(
                name: "Dorehamies",
                schema: "Patogh");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Patogh");
        }
    }
}
