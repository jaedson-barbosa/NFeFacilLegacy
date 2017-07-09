using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BibliotecaCentral.Migrations
{
    public partial class RegistroVeiculoEVenda : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "IdVeiculo",
                table: "Motoristas",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
                name: "ProdutoSimplesVenda",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Desconto = table.Column<double>(nullable: false),
                    DespesasExtras = table.Column<double>(nullable: false),
                    Frete = table.Column<double>(nullable: false),
                    Nome = table.Column<string>(nullable: true),
                    ProdutoBase = table.Column<Guid>(nullable: false),
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
                        name: "FK_ProdutoSimplesVenda_Vendas_RegistroVendaId",
                        column: x => x.RegistroVendaId,
                        principalTable: "Vendas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                name: "IX_Vendas_VendedorId",
                table: "Vendas",
                column: "VendedorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProdutoSimplesVenda");

            migrationBuilder.DropTable(
                name: "Veiculos");

            migrationBuilder.DropTable(
                name: "Vendas");

            migrationBuilder.DropColumn(
                name: "IdVeiculo",
                table: "Motoristas");
        }
    }
}
