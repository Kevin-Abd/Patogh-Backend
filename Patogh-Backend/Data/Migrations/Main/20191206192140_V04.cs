using Microsoft.EntityFrameworkCore.Migrations;

namespace PatoghBackend.Data.Migrations.Main
{
    public partial class V04 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageDorehamies_Dorehamies_DorehamiId",
                schema: "Patogh",
                table: "ImageDorehamies");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageDorehamies_Dorehamies_DorehamiId",
                schema: "Patogh",
                table: "ImageDorehamies",
                column: "DorehamiId",
                principalSchema: "Patogh",
                principalTable: "Dorehamies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageDorehamies_Dorehamies_DorehamiId",
                schema: "Patogh",
                table: "ImageDorehamies");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageDorehamies_Dorehamies_DorehamiId",
                schema: "Patogh",
                table: "ImageDorehamies",
                column: "DorehamiId",
                principalSchema: "Patogh",
                principalTable: "Dorehamies",
                principalColumn: "Id");
        }
    }
}
