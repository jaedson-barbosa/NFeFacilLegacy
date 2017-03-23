using Microsoft.EntityFrameworkCore;

namespace NFeFacil
{
    public class DadosBaseContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=dadosBase.db");
        }
    }
}
