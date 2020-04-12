using Microsoft.EntityFrameworkCore.Migrations;

namespace PatoghBackend.Data.Migrations.Main
{
    public partial class V06 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JoinUserDorehamies_Dorehamies_DorehamiId",
                schema: "Patogh",
                table: "JoinUserDorehamies");

            migrationBuilder.AddForeignKey(
                name: "FK_JoinUserDorehamies_Dorehamies_DorehamiId",
                schema: "Patogh",
                table: "JoinUserDorehamies",
                column: "DorehamiId",
                principalSchema: "Patogh",
                principalTable: "Dorehamies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JoinUserDorehamies_Dorehamies_DorehamiId",
                schema: "Patogh",
                table: "JoinUserDorehamies");

            migrationBuilder.AddForeignKey(
                name: "FK_JoinUserDorehamies_Dorehamies_DorehamiId",
                schema: "Patogh",
                table: "JoinUserDorehamies",
                column: "DorehamiId",
                principalSchema: "Patogh",
                principalTable: "Dorehamies",
                principalColumn: "Id");
        }
    }
}
