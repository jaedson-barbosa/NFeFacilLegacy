using Microsoft.EntityFrameworkCore.Migrations;

namespace NFeFacil.Migrations
{
    public partial class Loja188 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MotivoEdicao",
                table: "Vendas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImpostosPadrao",
                table: "Produtos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MotivoEdicao",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "ImpostosPadrao",
                table: "Produtos");
        }
    }
}
