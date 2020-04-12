using Microsoft.EntityFrameworkCore.Migrations;

namespace PatoghBackend.Data.Migrations.Main
{
    public partial class V12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagDorehamies_Tags_TagId",
                schema: "Patogh",
                table: "TagDorehamies");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_TagDorehamies_DorehamiId_TagId",
                schema: "Patogh",
                table: "TagDorehamies");

            migrationBuilder.AlterColumn<long>(
                name: "TagId",
                schema: "Patogh",
                table: "TagDorehamies",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "DorehamiId",
                schema: "Patogh",
                table: "TagDorehamies",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_TagDorehamies_DorehamiId_TagId",
                schema: "Patogh",
                table: "TagDorehamies",
                columns: new[] { "DorehamiId", "TagId" },
                unique: true,
                filter: "[DorehamiId] IS NOT NULL AND [TagId] IS NOT NULL");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagDorehamies_Tags_TagId",
                schema: "Patogh",
                table: "TagDorehamies");

            migrationBuilder.DropIndex(
                name: "IX_TagDorehamies_DorehamiId_TagId",
                schema: "Patogh",
                table: "TagDorehamies");

            migrationBuilder.AlterColumn<long>(
                name: "TagId",
                schema: "Patogh",
                table: "TagDorehamies",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "DorehamiId",
                schema: "Patogh",
                table: "TagDorehamies",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_TagDorehamies_DorehamiId_TagId",
                schema: "Patogh",
                table: "TagDorehamies",
                columns: new[] { "DorehamiId", "TagId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TagDorehamies_Tags_TagId",
                schema: "Patogh",
                table: "TagDorehamies",
                column: "TagId",
                principalSchema: "Patogh",
                principalTable: "Tags",
                principalColumn: "Id");
        }
    }
}
