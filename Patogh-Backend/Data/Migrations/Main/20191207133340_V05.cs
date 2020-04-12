using Microsoft.EntityFrameworkCore.Migrations;

namespace PatoghBackend.Data.Migrations.Main
{
    public partial class V05 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ThumbnailId",
                schema: "Patogh",
                table: "Dorehamies",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dorehamies_ThumbnailId",
                schema: "Patogh",
                table: "Dorehamies",
                column: "ThumbnailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dorehamies_Images_ThumbnailId",
                schema: "Patogh",
                table: "Dorehamies",
                column: "ThumbnailId",
                principalSchema: "Patogh",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dorehamies_Images_ThumbnailId",
                schema: "Patogh",
                table: "Dorehamies");

            migrationBuilder.DropIndex(
                name: "IX_Dorehamies_ThumbnailId",
                schema: "Patogh",
                table: "Dorehamies");

            migrationBuilder.DropColumn(
                name: "ThumbnailId",
                schema: "Patogh",
                table: "Dorehamies");
        }
    }
}
