using Microsoft.EntityFrameworkCore.Migrations;

namespace Banco.Migrations
{
    public partial class RegistroCancelamentoAdicionado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cancelamentos",
                columns: table => new
                {
                    ChaveNFe = table.Column<string>(nullable: false),
                    DataHoraEvento = table.Column<string>(nullable: true),
                    TipoAmbiente = table.Column<int>(nullable: false),
                    XML = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cancelamentos", x => x.ChaveNFe);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cancelamentos");
        }
    }
}
