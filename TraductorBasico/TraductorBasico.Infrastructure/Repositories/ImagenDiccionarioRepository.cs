using Microsoft.EntityFrameworkCore;
using TraductorBasico.Contract;
using TraductorBasico.Domain.Entities;
using TraductorBasico.Infrastructure.Context;

namespace TraductorBasico.Infrastructure.Repositories
{
    public class ImagenDiccionarioRepository : IImagenDiccionarioRepository
    {
        private readonly TraductorBasicoDataContext _context;

        public ImagenDiccionarioRepository(TraductorBasicoDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ImagenDiccionario>> GetAllAsync()
        {
            return await _context.ImagenesDiccionario
                .AsNoTracking()
                .OrderBy(i => i.Categoria)
                .ThenBy(i => i.Nombre)
                .ToListAsync();
        }

        public async Task<ImagenDiccionario?> GetByIdAsync(int id)
        {
            return await _context.ImagenesDiccionario.FindAsync(id);
        }

        public async Task<IEnumerable<ImagenDiccionario>> GetByCategoriaAsync(string categoria)
        {
            return await _context.ImagenesDiccionario
                .Where(i => i.Categoria.ToLower() == categoria.ToLower().Trim())
                .AsNoTracking()
                .OrderBy(i => i.Nombre)
                .ToListAsync();
        }

        public async Task<ImagenDiccionario?> GetByIdWithFrasesAsync(int id)
        {
            return await _context.ImagenesDiccionario
                .Include(i => i.FraseImagenes)
                    .ThenInclude(fi => fi.Frase)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<ImagenDiccionario>> GetAllWithFrasesAsync()
        {
            return await _context.ImagenesDiccionario
                .Include(i => i.FraseImagenes)
                    .ThenInclude(fi => fi.Frase)
                .AsNoTracking()
                .OrderBy(i => i.Categoria)
                .ThenBy(i => i.Nombre)
                .ToListAsync();
        }

        public async Task<ImagenDiccionario> AddAsync(ImagenDiccionario imagen)
        {
            _context.ImagenesDiccionario.Add(imagen);
            await _context.SaveChangesAsync();
            return imagen;
        }

        public async Task<bool> UpdateAsync(ImagenDiccionario imagen)
        {
            var exists = await _context.ImagenesDiccionario.AnyAsync(i => i.Id == imagen.Id);
            if (!exists)
                return false;

            _context.ImagenesDiccionario.Update(imagen);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(ImagenDiccionario imagen)
        {
            // Primero eliminar las relaciones
            var relaciones = await _context.FraseImagenes
                .Where(fi => fi.ImagenId == imagen.Id)
                .ToListAsync();

            _context.FraseImagenes.RemoveRange(relaciones);

            // Luego eliminar la imagen
            _context.ImagenesDiccionario.Remove(imagen);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AsignarFrasesAsync(int imagenId, List<int> frasesIds)
        {
            // Verificar que la imagen existe
            var imagenExists = await _context.ImagenesDiccionario.AnyAsync(i => i.Id == imagenId);
            if (!imagenExists)
                return false;

            // Eliminar relaciones existentes
            var relacionesExistentes = await _context.FraseImagenes
                .Where(fi => fi.ImagenId == imagenId)
                .ToListAsync();
            _context.FraseImagenes.RemoveRange(relacionesExistentes);

            // Crear nuevas relaciones
            var nuevasRelaciones = frasesIds
                .Where(fraseId => _context.Frases.Any(f => f.Id == fraseId))
                .Select(fraseId => new FraseImagen
                {
                    ImagenId = imagenId,
                    FraseId = fraseId
                }).ToList();

            _context.FraseImagenes.AddRange(nuevasRelaciones);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoverFrasesAsync(int imagenId, List<int> frasesIds)
        {
            var relaciones = await _context.FraseImagenes
                .Where(fi => fi.ImagenId == imagenId && frasesIds.Contains(fi.FraseId))
                .ToListAsync();

            _context.FraseImagenes.RemoveRange(relaciones);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}