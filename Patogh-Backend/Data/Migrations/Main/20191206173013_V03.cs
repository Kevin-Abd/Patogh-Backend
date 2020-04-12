using Microsoft.EntityFrameworkCore.Migrations;

namespace PatoghBackend.Data.Migrations.Main
{
    public partial class V03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImageDorehamies",
                schema: "Patogh",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageId = table.Column<long>(nullable: false),
                    DorehamiId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageDorehamies", x => x.Id);
                    table.UniqueConstraint("AK_ImageDorehamies_DorehamiId_ImageId", x => new { x.DorehamiId, x.ImageId });
                    table.ForeignKey(
                        name: "FK_ImageDorehamies_Dorehamies_DorehamiId",
                        column: x => x.DorehamiId,
                        principalSchema: "Patogh",
                        principalTable: "Dorehamies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ImageDorehamies_Images_ImageId",
                        column: x => x.ImageId,
                        principalSchema: "Patogh",
                        principalTable: "Images",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageDorehamies_ImageId",
                schema: "Patogh",
                table: "ImageDorehamies",
                column: "ImageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageDorehamies",
                schema: "Patogh");
        }
    }
}
