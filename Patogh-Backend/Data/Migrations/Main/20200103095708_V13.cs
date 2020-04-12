using Microsoft.EntityFrameworkCore.Migrations;

namespace PatoghBackend.Data.Migrations.Main
{
    public partial class V13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavUserDorehamies_Users_UserId",
                schema: "Patogh",
                table: "FavUserDorehamies");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_FavUserDorehamies_DorehamiId_UserId",
                schema: "Patogh",
                table: "FavUserDorehamies");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                schema: "Patogh",
                table: "FavUserDorehamies",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "DorehamiId",
                schema: "Patogh",
                table: "FavUserDorehamies",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_FavUserDorehamies_DorehamiId_UserId",
                schema: "Patogh",
                table: "FavUserDorehamies",
                columns: new[] { "DorehamiId", "UserId" },
                unique: true,
                filter: "[DorehamiId] IS NOT NULL AND [UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_FavUserDorehamies_Users_UserId",
                schema: "Patogh",
                table: "FavUserDorehamies",
                column: "UserId",
                principalSchema: "Patogh",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavUserDorehamies_Users_UserId",
                schema: "Patogh",
                table: "FavUserDorehamies");

            migrationBuilder.DropIndex(
                name: "IX_FavUserDorehamies_DorehamiId_UserId",
                schema: "Patogh",
                table: "FavUserDorehamies");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                schema: "Patogh",
                table: "FavUserDorehamies",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "DorehamiId",
                schema: "Patogh",
                table: "FavUserDorehamies",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_FavUserDorehamies_DorehamiId_UserId",
                schema: "Patogh",
                table: "FavUserDorehamies",
                columns: new[] { "DorehamiId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_FavUserDorehamies_Users_UserId",
                schema: "Patogh",
                table: "FavUserDorehamies",
                column: "UserId",
                principalSchema: "Patogh",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
