using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BibliotecaCentral.Migrations
{
    public partial class Loja128 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResultadosCliente");

            migrationBuilder.DropTable(
                name: "ResultadosServidor");

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
                    ProdutoId = table.Column<Guid>(nullable: true),
                    Segmento = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estoque", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Estoque_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vendas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ClienteId = table.Column<Guid>(nullable: true),
                    DataHoraVenda = table.Column<DateTime>(nullable: false),
                    DescontoTotal = table.Column<double>(nullable: false),
                    EmitenteId = table.Column<Guid>(nullable: true),
                    MotoristaId = table.Column<Guid>(nullable: true),
                    NotaFiscalRelacionadaId = table.Column<string>(nullable: true),
                    Observações = table.Column<string>(nullable: true),
                    VendedorId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vendas_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vendas_Emitentes_EmitenteId",
                        column: x => x.EmitenteId,
                        principalTable: "Emitentes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vendas_Motoristas_MotoristaId",
                        column: x => x.MotoristaId,
                        principalTable: "Motoristas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vendas_NotasFiscais_NotaFiscalRelacionadaId",
                        column: x => x.NotaFiscalRelacionadaId,
                        principalTable: "NotasFiscais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vendas_Vendedores_VendedorId",
                        column: x => x.VendedorId,
                        principalTable: "Vendedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    Id = table.Column<DateTime>(nullable: false),
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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Desconto = table.Column<double>(nullable: false),
                    DespesasExtras = table.Column<double>(nullable: false),
                    Frete = table.Column<double>(nullable: false),
                    Nome = table.Column<string>(nullable: true),
                    ProdutoBaseId = table.Column<Guid>(nullable: true),
                    Quantidade = table.Column<double>(nullable: false),
                    RegistroVendaId = table.Column<Guid>(nullable: true),
                    Seguro = table.Column<double>(nullable: false),
                    TotalLíquido = table.Column<double>(nullable: false),
                    Unidade = table.Column<string>(nullable: true),
                    ValorUnitario = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutoSimplesVenda", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProdutoSimplesVenda_Produtos_ProdutoBaseId",
                        column: x => x.ProdutoBaseId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_Estoque_ProdutoId",
                table: "Estoque",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoSimplesVenda_ProdutoBaseId",
                table: "ProdutoSimplesVenda",
                column: "ProdutoBaseId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoSimplesVenda_RegistroVendaId",
                table: "ProdutoSimplesVenda",
                column: "RegistroVendaId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_ClienteId",
                table: "Vendas",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_EmitenteId",
                table: "Vendas",
                column: "EmitenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_MotoristaId",
                table: "Vendas",
                column: "MotoristaId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_NotaFiscalRelacionadaId",
                table: "Vendas",
                column: "NotaFiscalRelacionadaId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_VendedorId",
                table: "Vendas",
                column: "VendedorId");
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
