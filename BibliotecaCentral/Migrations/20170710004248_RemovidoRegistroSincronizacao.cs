using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BibliotecaCentral.Migrations
{
    public partial class RemovidoRegistroSincronizacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResultadosCliente");

            migrationBuilder.DropTable(
                name: "ResultadosServidor");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResultadosCliente",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MomentoSincronizacao = table.Column<DateTime>(nullable: false),
                    NumeroDadosBaseTrafegados = table.Column<int>(nullable: false),
                    NumeroNotasTrafegadas = table.Column<int>(nullable: false),
                    SincronizacaoAutomatica = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadosCliente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResultadosServidor",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MomentoRequisicao = table.Column<DateTime>(nullable: false),
                    SucessoSolicitacao = table.Column<bool>(nullable: false),
                    TipoDadoSolicitado = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadosServidor", x => x.Id);
                });
        }
    }
}
