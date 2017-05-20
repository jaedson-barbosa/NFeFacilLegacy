using Microsoft.EntityFrameworkCore;
using BibliotecaCentral.ItensBD;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;

namespace BibliotecaCentral
{
    public class AplicativoContext : DbContext
    {
        public DbSet<ClienteDI> Clientes { get; set; }
        public DbSet<EmitenteDI> Emitentes { get; set; }
        public DbSet<Motorista> Motoristas { get; set; }
        public DbSet<BaseProdutoOuServico> Produtos { get; set; }
        public DbSet<NFeDI> NotasFiscais { get; set; }
        public DbSet<ResultadoSincronizacaoCliente> ResultadosCliente { get; set; }
        public DbSet<ResultadoSincronizacaoServidor> ResultadosServidor { get; set; }

        public AplicativoContext()
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=informacoes.db");
        }
    }
}
