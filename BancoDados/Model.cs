using Microsoft.EntityFrameworkCore;
using Banco.ItensBD;

namespace Banco
{
    public class AplicativoContext : DbContext
    {
        public DbSet<ClienteDI> Clientes { get; set; }
        public DbSet<EmitenteDI> Emitentes { get; set; }
        public DbSet<MotoristaDI> Motoristas { get; set; }
        public DbSet<Vendedor> Vendedores { get; set; }
        public DbSet<ProdutoDI> Produtos { get; set; }
        public DbSet<Estoque> Estoque { get; set; }
        public DbSet<VeiculoDI> Veiculos { get; set; }
        public DbSet<NFeDI> NotasFiscais { get; set; }
        public DbSet<RegistroVenda> Vendas { get; set; }
        public DbSet<RegistroCancelamento> Cancelamentos { get; set; }
        public DbSet<Imagem> Imagens { get; set; }

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
