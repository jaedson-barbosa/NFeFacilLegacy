using Microsoft.EntityFrameworkCore;
using BibliotecaCentral.ItensBD;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;

namespace BibliotecaCentral
{
    public class AplicativoContext : DbContext
    {
        public DbSet<Destinatario> Clientes { get; set; }
        public DbSet<Emitente> Emitentes { get; set; }
        public DbSet<Motorista> Motoristas { get; set; }
        public DbSet<BaseProdutoOuServico> Produtos { get; set; }
        public DbSet<NFeDI> NotasFiscais { get; set; }
        public DbSet<ResultadoSincronizacaoCliente> ResultadosCliente { get; set; }
        public DbSet<ResultadoSincronizacaoServidor> ResultadosServidor { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=informacoes.db");
        }
    }
}
