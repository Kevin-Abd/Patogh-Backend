using Microsoft.EntityFrameworkCore.Migrations;

namespace PatoghBackend.Data.Migrations.Main
{
    public partial class V08 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tags",
                schema: "Patogh",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TagDorehamies",
                schema: "Patogh",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagId = table.Column<long>(nullable: false),
                    DorehamiId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagDorehamies", x => x.Id);
                    table.UniqueConstraint("AK_TagDorehamies_DorehamiId_TagId", x => new { x.DorehamiId, x.TagId });
                    table.ForeignKey(
                        name: "FK_TagDorehamies_Dorehamies_DorehamiId",
                        column: x => x.DorehamiId,
                        principalSchema: "Patogh",
                        principalTable: "Dorehamies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TagDorehamies_Tags_TagId",
                        column: x => x.TagId,
                        principalSchema: "Patogh",
                        principalTable: "Tags",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TagDorehamies_TagId",
                schema: "Patogh",
                table: "TagDorehamies",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TagDorehamies",
                schema: "Patogh");

            migrationBuilder.DropTable(
                name: "Tags",
                schema: "Patogh");
        }
    }
}
