using Microsoft.EntityFrameworkCore;
using TraductorBasico.Models;
namespace TraductorBasico.Data
{
    public class TraductorBasicoDataContext : DbContext
    {
        public TraductorBasicoDataContext(DbContextOptions<TraductorBasicoDataContext> options)
            : base(options)
        {
        }
        public DbSet<Frase> Frases { get; set; } = null!;

       
    }
}