using TraductorBasico.Domain.Entities;

namespace TraductorBasico.Contract
{
    public interface IImagenDiccionarioRepository
    {
        Task<IEnumerable<ImagenDiccionario>> GetAllAsync();
        Task<ImagenDiccionario?> GetByIdAsync(int id);
        Task<IEnumerable<ImagenDiccionario>> GetByCategoriaAsync(string categoria);
        Task<ImagenDiccionario?> GetByIdWithFrasesAsync(int id);
        Task<IEnumerable<ImagenDiccionario>> GetAllWithFrasesAsync();
        Task<ImagenDiccionario> AddAsync(ImagenDiccionario imagen);
        Task<bool> UpdateAsync(ImagenDiccionario imagen);
        Task<bool> DeleteAsync(ImagenDiccionario imagen);
        Task<bool> AsignarFrasesAsync(int imagenId, List<int> frasesIds);
        Task<bool> RemoverFrasesAsync(int imagenId, List<int> frasesIds);
    }
}