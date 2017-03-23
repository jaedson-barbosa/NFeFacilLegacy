using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NFeFacil.Migrations
{
    public partial class MigracaoCompleta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Motoristas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CNPJ = table.Column<string>(nullable: true),
                    CPF = table.Column<string>(nullable: true),
                    UF = table.Column<string>(nullable: true),
                    inscricaoEstadual = table.Column<string>(nullable: true),
                    nome = table.Column<string>(nullable: true),
                    xEnder = table.Column<string>(nullable: true),
                    xMun = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motoristas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotasFiscais",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DataEmissao = table.Column<string>(nullable: false),
                    IdentificacaoNota = table.Column<string>(nullable: false),
                    NomeCliente = table.Column<string>(nullable: false),
                    NomeEmitente = table.Column<string>(nullable: false),
                    NumeroNota = table.Column<string>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotasFiscais", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CFOP = table.Column<string>(nullable: true),
                    CodigoBarras = table.Column<string>(nullable: true),
                    CodigoBarrasTributo = table.Column<string>(nullable: true),
                    CodigoProduto = table.Column<string>(nullable: true),
                    Descricao = table.Column<string>(nullable: true),
                    EXTIPI = table.Column<string>(nullable: true),
                    NCM = table.Column<string>(nullable: true),
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
                name: "ResultadosCliente",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MomentoSincronizacao = table.Column<DateTime>(nullable: false),
                    NumeroDadosEnviados = table.Column<int>(nullable: false),
                    NumeroDadosRecebidos = table.Column<int>(nullable: false),
                    NumeroNotasEnviadas = table.Column<int>(nullable: false),
                    NumeroNotasRecebidas = table.Column<int>(nullable: false),
                    PodeSincronizarDadoBase = table.Column<bool>(nullable: false),
                    PodeSincronizarNota = table.Column<bool>(nullable: false),
                    SincronizacaoAutomatica = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadosCliente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResultadosServudir",
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
                    table.PrimaryKey("PK_ResultadosServudir", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnderecoCompleto",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Bairro = table.Column<string>(nullable: true),
                    CEP = table.Column<string>(nullable: true),
                    CPais = table.Column<int>(nullable: false),
                    CodigoMunicipio = table.Column<long>(nullable: false),
                    Complemento = table.Column<string>(nullable: true),
                    Logradouro = table.Column<string>(nullable: true),
                    NomeMunicipio = table.Column<string>(nullable: true),
                    Numero = table.Column<string>(nullable: true),
                    SiglaUF = table.Column<string>(nullable: true),
                    Telefone = table.Column<string>(nullable: true),
                    XPais = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnderecoCompleto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CNPJ = table.Column<string>(nullable: true),
                    CPF = table.Column<string>(nullable: true),
                    ISUF = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    endereçoId = table.Column<int>(nullable: true),
                    idEstrangeiro = table.Column<string>(nullable: true),
                    indicadorIE = table.Column<int>(nullable: false),
                    inscricaoEstadual = table.Column<string>(nullable: true),
                    nome = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clientes_EnderecoCompleto_endereçoId",
                        column: x => x.endereçoId,
                        principalTable: "EnderecoCompleto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Emitentes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CNAE = table.Column<string>(nullable: true),
                    CNPJ = table.Column<string>(nullable: true),
                    IEST = table.Column<string>(nullable: true),
                    IM = table.Column<string>(nullable: true),
                    endereçoId = table.Column<int>(nullable: true),
                    inscricaoEstadual = table.Column<string>(nullable: true),
                    nome = table.Column<string>(nullable: true),
                    nomeFantasia = table.Column<string>(nullable: true),
                    regimeTributario = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emitentes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Emitentes_EnderecoCompleto_endereçoId",
                        column: x => x.endereçoId,
                        principalTable: "EnderecoCompleto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_endereçoId",
                table: "Clientes",
                column: "endereçoId");

            migrationBuilder.CreateIndex(
                name: "IX_Emitentes_endereçoId",
                table: "Emitentes",
                column: "endereçoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Emitentes");

            migrationBuilder.DropTable(
                name: "Motoristas");

            migrationBuilder.DropTable(
                name: "NotasFiscais");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "ResultadosCliente");

            migrationBuilder.DropTable(
                name: "ResultadosServudir");

            migrationBuilder.DropTable(
                name: "EnderecoCompleto");
        }
    }
}
