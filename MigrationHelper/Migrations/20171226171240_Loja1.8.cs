using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseGeral.Migrations
{
    public partial class Loja18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CEST",
                table: "Produtos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ICMS",
                table: "Produtos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImpostosSimples",
                table: "Produtos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProdutoEspecial",
                table: "Produtos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CEST",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "ICMS",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "ImpostosSimples",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "ProdutoEspecial",
                table: "Produtos");
        }
    }
}
