using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseGeral.Migrations
{
    public partial class Loja179 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Veiculos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UltimaData",
                table: "Veiculos",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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
                name: "Ativo",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "UltimaData",
                table: "Veiculos");

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
