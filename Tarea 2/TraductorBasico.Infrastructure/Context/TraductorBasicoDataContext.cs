using Microsoft.EntityFrameworkCore;
using TraductorBasico.Domain.Entities;

namespace TraductorBasico.Infrastructure.Context
{
    public class TraductorBasicoDataContext : DbContext
    {
        public TraductorBasicoDataContext(DbContextOptions<TraductorBasicoDataContext> options)
            : base(options) { }

        public DbSet<Frase> Frases { get; set; } = null!;
        public DbSet<ImagenDiccionario> ImagenesDiccionario { get; set; } = null!;
        public DbSet<FraseImagen> FraseImagenes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de la relación muchos a muchos
            modelBuilder.Entity<FraseImagen>()
                .HasKey(fi => new { fi.FraseId, fi.ImagenId });

            modelBuilder.Entity<FraseImagen>()
                .HasOne(fi => fi.Frase)
                .WithMany(f => f.FraseImagenes)
                .HasForeignKey(fi => fi.FraseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FraseImagen>()
                .HasOne(fi => fi.Imagen)
                .WithMany(i => i.FraseImagenes)
                .HasForeignKey(fi => fi.ImagenId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuraciones adicionales
            modelBuilder.Entity<ImagenDiccionario>()
                .HasIndex(i => i.Categoria);

            modelBuilder.Entity<ImagenDiccionario>()
                .Property(i => i.FechaCreacion)
                .HasDefaultValueSql("GETDATE()");

            base.OnModelCreating(modelBuilder);
        }
    }
}