using Microsoft.EntityFrameworkCore.Migrations;

namespace BibliotecaCentral.Migrations
{
    public partial class CancelamentoAdicionado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Exportada",
                table: "NotasFiscais",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Impressa",
                table: "NotasFiscais",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql("UPDATE NotasFiscais SET Impressa = true, Status = 4 WHERE Status = 5");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE NotasFiscais SET Status = 5 WHERE Impressa = true");

            migrationBuilder.DropColumn(
                name: "Exportada",
                table: "NotasFiscais");

            migrationBuilder.DropColumn(
                name: "Impressa",
                table: "NotasFiscais");
        }
    }
}
