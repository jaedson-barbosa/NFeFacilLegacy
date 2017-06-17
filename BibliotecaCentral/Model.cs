using Microsoft.EntityFrameworkCore;
using BibliotecaCentral.ItensBD;

namespace BibliotecaCentral
{
    public class AplicativoContext : DbContext
    {
        public DbSet<ClienteDI> Clientes { get; set; }
        public DbSet<EmitenteDI> Emitentes { get; set; }
        public DbSet<MotoristaDI> Motoristas { get; set; }
        public DbSet<ProdutoDI> Produtos { get; set; }
        public DbSet<NFeDI> NotasFiscais { get; set; }
        public DbSet<ResultadoSincronizacaoCliente> ResultadosCliente { get; set; }
        public DbSet<ResultadoSincronizacaoServidor> ResultadosServidor { get; set; }
        public DbSet<RegistroCancelamento> Cancelamentos { get; set; }

        public AplicativoContext()
        {
            ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=informacoes.db");
        }
    }
}
