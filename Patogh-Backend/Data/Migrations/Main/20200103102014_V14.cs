using Microsoft.EntityFrameworkCore.Migrations;

namespace PatoghBackend.Data.Migrations.Main
{
    public partial class V14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JoinUserDorehamies_Users_UserId",
                schema: "Patogh",
                table: "JoinUserDorehamies");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_JoinUserDorehamies_DorehamiId_UserId",
                schema: "Patogh",
                table: "JoinUserDorehamies");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                schema: "Patogh",
                table: "JoinUserDorehamies",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "DorehamiId",
                schema: "Patogh",
                table: "JoinUserDorehamies",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_JoinUserDorehamies_DorehamiId_UserId",
                schema: "Patogh",
                table: "JoinUserDorehamies",
                columns: new[] { "DorehamiId", "UserId" },
                unique: true,
                filter: "[DorehamiId] IS NOT NULL AND [UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_JoinUserDorehamies_Users_UserId",
                schema: "Patogh",
                table: "JoinUserDorehamies",
                column: "UserId",
                principalSchema: "Patogh",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JoinUserDorehamies_Users_UserId",
                schema: "Patogh",
                table: "JoinUserDorehamies");

            migrationBuilder.DropIndex(
                name: "IX_JoinUserDorehamies_DorehamiId_UserId",
                schema: "Patogh",
                table: "JoinUserDorehamies");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                schema: "Patogh",
                table: "JoinUserDorehamies",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "DorehamiId",
                schema: "Patogh",
                table: "JoinUserDorehamies",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_JoinUserDorehamies_DorehamiId_UserId",
                schema: "Patogh",
                table: "JoinUserDorehamies",
                columns: new[] { "DorehamiId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_JoinUserDorehamies_Users_UserId",
                schema: "Patogh",
                table: "JoinUserDorehamies",
                column: "UserId",
                principalSchema: "Patogh",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
