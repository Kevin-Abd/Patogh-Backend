using Microsoft.EntityFrameworkCore.Migrations;

namespace PatoghBackend.Data.Migrations.Main
{
    public partial class V07 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                schema: "Patogh",
                table: "Dorehamies",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                schema: "Patogh",
                table: "Dorehamies");
        }
    }
}
