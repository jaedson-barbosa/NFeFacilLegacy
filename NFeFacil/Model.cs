using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
