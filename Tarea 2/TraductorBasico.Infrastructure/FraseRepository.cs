using Microsoft.EntityFrameworkCore;
using TraductorBasico.Contract;
using TraductorBasico.Domain.Entities;
using TraductorBasico.Infrastructure.Context;

namespace TraductorBasico.Infrastructure.Repositories
{
    public class FraseRepository : IFraseRepository
    {
        private readonly TraductorBasicoDataContext _context;

        public FraseRepository(TraductorBasicoDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Frase>> GetAllAsync()
        {
            return await _context.Frases
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Frase?> GetByIdAsync(int id)
        {
            return await _context.Frases.FindAsync(id);
        }

        public async Task<Frase> AddAsync(Frase frase)
        {
            _context.Frases.Add(frase);
            await _context.SaveChangesAsync();
            return frase;
        }

        public async Task<bool> UpdateAsync(Frase frase)
        {
            var exists = await _context.Frases.AnyAsync(f => f.Id == frase.Id);
            if (!exists)
                return false;

            _context.Frases.Update(frase);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Frase frase)
        {
            _context.Frases.Remove(frase);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Frase?> FindByEspañolOrInglesAsync(string espanol, string ingles, int? excludeId = null)
        {
            return await _context.Frases
                .Where(f =>
                    (f.Español == espanol || f.Ingles == ingles) &&
                    (!excludeId.HasValue || f.Id != excludeId.Value))
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Frase>> GetByCategoriaAsync(string categoria)
        {
            return await _context.Frases
                .Where(f => f.Categoria.ToLower() == categoria.ToLower().Trim())
                .ToListAsync();
        }
    }
}
