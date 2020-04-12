using Microsoft.EntityFrameworkCore.Migrations;

namespace PatoghBackend.Data.Migrations.Main
{
    public partial class V10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagDorehamies_Dorehamies_DorehamiId",
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagDorehamies_Dorehamies_DorehamiId",
                schema: "Patogh",
                table: "TagDorehamies");

            migrationBuilder.AddForeignKey(
                name: "FK_TagDorehamies_Dorehamies_DorehamiId",
                schema: "Patogh",
                table: "TagDorehamies",
                column: "DorehamiId",
                principalSchema: "Patogh",
                principalTable: "Dorehamies",
                principalColumn: "Id");
        }
    }
}
