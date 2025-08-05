using TraductorBasico.Dtos;

namespace TraductorBasico.Contract
{
    public interface IDiccionarioService
    {
        Task<DiccionarioResponseDto> GetAllImagenesAsync();
        Task<DiccionarioResponseDto> GetImagenByIdAsync(int id);
        Task<DiccionarioResponseDto> GetImagenesByCategoriaAsync(string categoria);
        Task<DiccionarioResponseDto> GetImagenWithFrasesAsync(int id);
        Task<DiccionarioResponseDto> CreateImagenAsync(CreateImagenDiccionarioDto createImagenDto);
        Task<DiccionarioResponseDto> DeleteImagenAsync(int id);
        Task<DiccionarioResponseDto> AsignarFrasesAImagenAsync(int imagenId, List<int> frasesIds);
        Task<DiccionarioResponseDto> GetCategoriasDisponiblesAsync();
    }
}