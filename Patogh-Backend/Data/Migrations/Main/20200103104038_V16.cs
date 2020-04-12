using Microsoft.EntityFrameworkCore.Migrations;

namespace PatoghBackend.Data.Migrations.Main
{
    public partial class V16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagDorehamies_Dorehamies_DorehamiId",
                schema: "Patogh",
                table: "TagDorehamies");

            migrationBuilder.DropForeignKey(
                name: "FK_TagDorehamies_Tags_TagId",
                schema: "Patogh",
                table: "TagDorehamies");

            migrationBuilder.AddForeignKey(
                name: "FK_TagDorehamies_Dorehamies_DorehamiId",
                schema: "Patogh",
                table: "TagDorehamies",
                column: "DorehamiId",
                principalSchema: "Patogh",
                principalTable: "Dorehamies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TagDorehamies_Tags_TagId",
                schema: "Patogh",
                table: "TagDorehamies",
                column: "TagId",
                principalSchema: "Patogh",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagDorehamies_Dorehamies_DorehamiId",
                schema: "Patogh",
                table: "TagDorehamies");

            migrationBuilder.DropForeignKey(
                name: "FK_TagDorehamies_Tags_TagId",
                schema: "Patogh",
                table: "TagDorehamies");

            migrationBuilder.AddForeignKey(
                name: "FK_TagDorehamies_Dorehamies_DorehamiId",
                schema: "Patogh",
                table: "TagDorehamies",
                column: "DorehamiId",
                principalSchema: "Patogh",
                principalTable: "Dorehamies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TagDorehamies_Tags_TagId",
                schema: "Patogh",
                table: "TagDorehamies",
                column: "TagId",
                principalSchema: "Patogh",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
