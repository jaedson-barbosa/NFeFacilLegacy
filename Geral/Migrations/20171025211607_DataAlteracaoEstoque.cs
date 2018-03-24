using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseGeral.Migrations
{
    public partial class DataAlteracaoEstoque : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "MomentoRegistro",
                table: "AlteracaoEstoque",
                nullable: false,
                defaultValue: default(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MomentoRegistro",
                table: "AlteracaoEstoque");
        }
    }
}
