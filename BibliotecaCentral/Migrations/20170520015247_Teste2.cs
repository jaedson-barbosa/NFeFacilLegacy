using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BibliotecaCentral.Migrations
{
    public partial class Teste2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Bairro = table.Column<string>(nullable: true),
                    CEP = table.Column<string>(nullable: true),
                    CNPJ = table.Column<string>(nullable: true),
                    CPF = table.Column<string>(nullable: true),
                    CPais = table.Column<int>(nullable: false),
                    CodigoMunicipio = table.Column<int>(nullable: false),
                    Complemento = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    ISUF = table.Column<string>(nullable: true),
                    IdEstrangeiro = table.Column<string>(nullable: true),
                    IndicadorIE = table.Column<int>(nullable: false),
                    InscricaoEstadual = table.Column<string>(nullable: true),
                    Logradouro = table.Column<string>(nullable: true),
                    Nome = table.Column<string>(nullable: true),
                    NomeMunicipio = table.Column<string>(nullable: true),
                    Numero = table.Column<string>(nullable: true),
                    SiglaUF = table.Column<string>(nullable: true),
                    Telefone = table.Column<string>(nullable: true),
                    UltimaData = table.Column<DateTime>(nullable: false),
                    XPais = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Emitentes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Bairro = table.Column<string>(nullable: true),
                    CEP = table.Column<string>(nullable: true),
                    CNAE = table.Column<string>(nullable: true),
                    CNPJ = table.Column<string>(nullable: true),
                    CPais = table.Column<int>(nullable: false),
                    CodigoMunicipio = table.Column<int>(nullable: false),
                    Complemento = table.Column<string>(nullable: true),
                    IEST = table.Column<string>(nullable: true),
                    IM = table.Column<string>(nullable: true),
                    InscricaoEstadual = table.Column<string>(nullable: true),
                    Logradouro = table.Column<string>(nullable: true),
                    Nome = table.Column<string>(nullable: true),
                    NomeFantasia = table.Column<string>(nullable: true),
                    NomeMunicipio = table.Column<string>(nullable: true),
                    Numero = table.Column<string>(nullable: true),
                    RegimeTributario = table.Column<int>(nullable: false),
                    SiglaUF = table.Column<string>(nullable: true),
                    Telefone = table.Column<string>(nullable: true),
                    UltimaData = table.Column<DateTime>(nullable: false),
                    XPais = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emitentes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotasFiscais",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CNPJEmitente = table.Column<string>(nullable: false),
                    DataEmissao = table.Column<string>(nullable: false),
                    NomeCliente = table.Column<string>(nullable: false),
                    NomeEmitente = table.Column<string>(nullable: false),
                    NumeroNota = table.Column<long>(nullable: false),
                    SerieNota = table.Column<ushort>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    UltimaData = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotasFiscais", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CFOP = table.Column<string>(nullable: true),
                    CodigoBarras = table.Column<string>(nullable: true),
                    CodigoBarrasTributo = table.Column<string>(nullable: true),
                    CodigoProduto = table.Column<string>(nullable: true),
                    Descricao = table.Column<string>(nullable: true),
                    EXTIPI = table.Column<string>(nullable: true),
                    NCM = table.Column<string>(nullable: true),
                    UltimaData = table.Column<DateTime>(nullable: false),
                    UnidadeComercializacao = table.Column<string>(nullable: true),
                    UnidadeTributacao = table.Column<string>(nullable: true),
                    ValorUnitario = table.Column<double>(nullable: false),
                    ValorUnitarioTributo = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Motoristas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CNPJ = table.Column<string>(nullable: true),
                    CPF = table.Column<string>(nullable: true),
                    Documento = table.Column<string>(nullable: true),
                    InscricaoEstadual = table.Column<string>(nullable: true),
                    Nome = table.Column<string>(nullable: true),
                    UF = table.Column<string>(nullable: true),
                    UltimaData = table.Column<DateTime>(nullable: false),
                    XEnder = table.Column<string>(nullable: true),
                    XMun = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motoristas", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Emitentes");

            migrationBuilder.DropTable(
                name: "NotasFiscais");

            migrationBuilder.DropTable(
                name: "ResultadosCliente");

            migrationBuilder.DropTable(
                name: "ResultadosServidor");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "Motoristas");
        }
    }
}
