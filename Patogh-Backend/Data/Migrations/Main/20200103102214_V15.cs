using Microsoft.EntityFrameworkCore.Migrations;

namespace PatoghBackend.Data.Migrations.Main
{
    public partial class V15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageDorehamies_Dorehamies_DorehamiId",
                schema: "Patogh",
                table: "ImageDorehamies");

            migrationBuilder.DropForeignKey(
                name: "FK_ImageDorehamies_Images_ImageId",
                schema: "Patogh",
                table: "ImageDorehamies");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ImageDorehamies_DorehamiId_ImageId",
                schema: "Patogh",
                table: "ImageDorehamies");

            migrationBuilder.AlterColumn<long>(
                name: "ImageId",
                schema: "Patogh",
                table: "ImageDorehamies",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "DorehamiId",
                schema: "Patogh",
                table: "ImageDorehamies",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_ImageDorehamies_DorehamiId_ImageId",
                schema: "Patogh",
                table: "ImageDorehamies",
                columns: new[] { "DorehamiId", "ImageId" },
                unique: true,
                filter: "[DorehamiId] IS NOT NULL AND [ImageId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageDorehamies_Dorehamies_DorehamiId",
                schema: "Patogh",
                table: "ImageDorehamies",
                column: "DorehamiId",
                principalSchema: "Patogh",
                principalTable: "Dorehamies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ImageDorehamies_Images_ImageId",
                schema: "Patogh",
                table: "ImageDorehamies",
                column: "ImageId",
                principalSchema: "Patogh",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageDorehamies_Dorehamies_DorehamiId",
                schema: "Patogh",
                table: "ImageDorehamies");

            migrationBuilder.DropForeignKey(
                name: "FK_ImageDorehamies_Images_ImageId",
                schema: "Patogh",
                table: "ImageDorehamies");

            migrationBuilder.DropIndex(
                name: "IX_ImageDorehamies_DorehamiId_ImageId",
                schema: "Patogh",
                table: "ImageDorehamies");

            migrationBuilder.AlterColumn<long>(
                name: "ImageId",
                schema: "Patogh",
                table: "ImageDorehamies",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "DorehamiId",
                schema: "Patogh",
                table: "ImageDorehamies",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ImageDorehamies_DorehamiId_ImageId",
                schema: "Patogh",
                table: "ImageDorehamies",
                columns: new[] { "DorehamiId", "ImageId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ImageDorehamies_Dorehamies_DorehamiId",
                schema: "Patogh",
                table: "ImageDorehamies",
                column: "DorehamiId",
                principalSchema: "Patogh",
                principalTable: "Dorehamies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ImageDorehamies_Images_ImageId",
                schema: "Patogh",
                table: "ImageDorehamies",
                column: "ImageId",
                principalSchema: "Patogh",
                principalTable: "Images",
                principalColumn: "Id");
        }
    }
}
