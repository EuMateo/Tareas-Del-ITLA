using TraductorBasico.Dtos;

namespace TraductorBasico.Contract
{
    public interface IFraseService
    {
        Task<FraseResponseDto> GetAllFrasesAsync();
        Task<FraseResponseDto> GetFraseByIdAsync(int id);
        Task<FraseResponseDto> CreateFraseAsync(CreateFraseDto createFraseDto);
        Task<FraseResponseDto> UpdateFraseAsync(int id, UpdateFraseDto updateFraseDto);
        Task<FraseResponseDto> DeleteFraseAsync(int id);
        Task<FraseResponseDto> GetFrasesByCategoriaAsync(string categoria);
    }
}