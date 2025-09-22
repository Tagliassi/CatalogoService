using Microsoft.EntityFrameworkCore;
using CatalogoService.Models;

namespace CatalogoService.Data
{
    public class CatalogoDbContext : DbContext
    {
        public CatalogoDbContext(DbContextOptions<CatalogoDbContext> options) : base(options)
        {
        }

        // Mapeia o modelo 'Servico' para uma tabela chamada 'Servicos'.
        public DbSet<Servico> Servicos { get; set; }
    }
}