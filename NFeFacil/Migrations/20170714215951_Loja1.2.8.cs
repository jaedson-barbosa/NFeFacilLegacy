using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NFeFacil.Migrations
{
    public partial class Loja128 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResultadosCliente");

            migrationBuilder.DropTable(
                name: "ResultadosServidor");

            migrationBuilder.AddColumn<DateTime>(
                name: "UltimaData",
                table: "Vendedores",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "Veiculo",
                table: "Motoristas",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Estoque",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LocalizacaoGenerica = table.Column<string>(nullable: true),
                    Locação = table.Column<string>(nullable: true),
                    Prateleira = table.Column<string>(nullable: true),
                    Segmento = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estoque", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vendas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Cliente = table.Column<Guid>(nullable: false),
                    DataHoraVenda = table.Column<DateTime>(nullable: false),
                    DescontoTotal = table.Column<double>(nullable: false),
                    Emitente = table.Column<Guid>(nullable: false),
                    Motorista = table.Column<Guid>(nullable: false),
                    NotaFiscalRelacionada = table.Column<string>(nullable: true),
                    Observações = table.Column<string>(nullable: true),
                    UltimaData = table.Column<DateTime>(nullable: false),
                    Vendedor = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Veiculos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Descricao = table.Column<string>(nullable: true),
                    Placa = table.Column<string>(nullable: true),
                    RNTC = table.Column<string>(nullable: true),
                    UF = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Veiculos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AlteracaoEstoque",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Alteração = table.Column<double>(nullable: false),
                    EstoqueId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlteracaoEstoque", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlteracaoEstoque_Estoque_EstoqueId",
                        column: x => x.EstoqueId,
                        principalTable: "Estoque",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProdutoSimplesVenda",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Desconto = table.Column<double>(nullable: false),
                    DespesasExtras = table.Column<double>(nullable: false),
                    Frete = table.Column<double>(nullable: false),
                    IdBase = table.Column<Guid>(nullable: false),
                    Quantidade = table.Column<double>(nullable: false),
                    RegistroVendaId = table.Column<Guid>(nullable: true),
                    Seguro = table.Column<double>(nullable: false),
                    TotalLíquido = table.Column<double>(nullable: false),
                    ValorUnitario = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutoSimplesVenda", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProdutoSimplesVenda_Vendas_RegistroVendaId",
                        column: x => x.RegistroVendaId,
                        principalTable: "Vendas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlteracaoEstoque_EstoqueId",
                table: "AlteracaoEstoque",
                column: "EstoqueId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoSimplesVenda_RegistroVendaId",
                table: "ProdutoSimplesVenda",
                column: "RegistroVendaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlteracaoEstoque");

            migrationBuilder.DropTable(
                name: "ProdutoSimplesVenda");

            migrationBuilder.DropTable(
                name: "Veiculos");

            migrationBuilder.DropTable(
                name: "Estoque");

            migrationBuilder.DropTable(
                name: "Vendas");

            migrationBuilder.DropColumn(
                name: "UltimaData",
                table: "Vendedores");

            migrationBuilder.DropColumn(
                name: "Veiculo",
                table: "Motoristas");

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
