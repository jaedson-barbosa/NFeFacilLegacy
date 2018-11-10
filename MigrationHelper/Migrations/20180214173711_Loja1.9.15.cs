using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseGeral.Migrations
{
    public partial class Loja1915 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CSC",
                table: "Emitentes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdToken",
                table: "Emitentes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CSC",
                table: "Emitentes");

            migrationBuilder.DropColumn(
                name: "IdToken",
                table: "Emitentes");
        }
    }
}
