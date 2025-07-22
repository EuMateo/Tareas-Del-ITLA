using TraductorBasico.Domain.Entities;

namespace TraductorBasico.Contract
{
    public interface IFraseRepository
    {
        Task<IEnumerable<Frase>> GetAllAsync();
        Task<Frase?> GetByIdAsync(int id);
        Task<Frase?> FindByEspañolOrInglesAsync(string espanol, string ingles, int? excludeId = null);
        Task<IEnumerable<Frase>> GetByCategoriaAsync(string categoria);
        Task<Frase> AddAsync(Frase frase);
        Task<bool> UpdateAsync(Frase frase);
        Task<bool> DeleteAsync(Frase frase);
    }
}