using Microsoft.EntityFrameworkCore.Migrations;

namespace NFeFacil.Migrations
{
    public partial class Loja179 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Motoristas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telefone",
                table: "Motoristas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VeiculosSecundarios",
                table: "Motoristas",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Motoristas");

            migrationBuilder.DropColumn(
                name: "Telefone",
                table: "Motoristas");

            migrationBuilder.DropColumn(
                name: "VeiculosSecundarios",
                table: "Motoristas");
        }
    }
}
