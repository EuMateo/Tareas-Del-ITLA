using Microsoft.EntityFrameworkCore;
using TraductorBasico.Domain.Entities; // Cambiar este using según tu namespace real

namespace TraductorBasico.Infrastructure.Context
{
    public class TraductorBasicoDataContext : DbContext
    {
        public TraductorBasicoDataContext(DbContextOptions<TraductorBasicoDataContext> options)
            : base(options) { }

        public DbSet<Frase> Frases { get; set; } = null!;
    }
}