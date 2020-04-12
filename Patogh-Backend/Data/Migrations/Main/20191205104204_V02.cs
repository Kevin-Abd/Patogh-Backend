using Microsoft.EntityFrameworkCore.Migrations;

namespace PatoghBackend.Data.Migrations.Main
{
    public partial class V02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProfilePictureId",
                schema: "Patogh",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProfilePictureId1",
                schema: "Patogh",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProfilePictureId1",
                schema: "Patogh",
                table: "Users",
                column: "ProfilePictureId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Images_ProfilePictureId1",
                schema: "Patogh",
                table: "Users",
                column: "ProfilePictureId1",
                principalSchema: "Patogh",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Images_ProfilePictureId1",
                schema: "Patogh",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ProfilePictureId1",
                schema: "Patogh",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProfilePictureId",
                schema: "Patogh",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProfilePictureId1",
                schema: "Patogh",
                table: "Users");
        }
    }
}
