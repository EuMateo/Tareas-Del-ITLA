using System.ComponentModel.DataAnnotations;
using TraductorBasico.Contract;
using TraductorBasico.Dtos;
using TraductorBasico.Domain.Entities;

namespace TraductorBasico.Application.Services
{
    public class FraseService : IFraseService
    {
        private readonly IFraseRepository _fraseRepository;

        public FraseService(IFraseRepository fraseRepository)
        {
            _fraseRepository = fraseRepository;
        }

        public async Task<FraseResponseDto> GetAllFrasesAsync()
        {
            try
            {
                var frases = await _fraseRepository.GetAllAsync();
                var frasesDto = frases.Select(MapToDto).ToList();

                return FraseResponseDto.SuccessResponse(frasesDto, $"Se encontraron {frasesDto.Count} frases");
            }
            catch (Exception ex)
            {
                return FraseResponseDto.ErrorResponse($"Error al obtener las frases: {ex.Message}");
            }
        }

        public async Task<FraseResponseDto> GetFraseByIdAsync(int id)
        {
            if (id <= 0)
                return FraseResponseDto.ErrorResponse("El ID debe ser mayor que cero");

            try
            {
                var frase = await _fraseRepository.GetByIdAsync(id);
                if (frase == null)
                    return FraseResponseDto.ErrorResponse($"No se encontró la frase con ID {id}");

                var dto = MapToDto(frase);
                return FraseResponseDto.SuccessResponse(dto, "Frase encontrada exitosamente");
            }
            catch (Exception ex)
            {
                return FraseResponseDto.ErrorResponse($"Error al obtener la frase: {ex.Message}");
            }
        }

        public async Task<FraseResponseDto> CreateFraseAsync(CreateFraseDto createFraseDto)
        {
            var validationResult = ValidateDto(createFraseDto);
            if (!validationResult.IsValid)
                return FraseResponseDto.ErrorResponse(validationResult.Errors);

            var customErrors = ValidateCustomBusinessRules(createFraseDto);
            if (customErrors.Any())
                return FraseResponseDto.ErrorResponse(customErrors);

            try
            {
                var existingFrase = await _fraseRepository.FindByEspañolOrInglesAsync(createFraseDto.Español, createFraseDto.Ingles);
                if (existingFrase != null)
                    return FraseResponseDto.ErrorResponse("Ya existe una frase similar en el sistema");

                var frase = new Frase
                {
                    Español = createFraseDto.Español.Trim(),
                    Ingles = createFraseDto.Ingles.Trim(),
                    Pronunciacion = createFraseDto.Pronunciacion.Trim(),
                    Categoria = createFraseDto.Categoria.Trim()
                };

                await _fraseRepository.AddAsync(frase);

                var dto = MapToDto(frase);
                return FraseResponseDto.SuccessResponse(dto, "Frase creada exitosamente");
            }
            catch (Exception ex)
            {
                return FraseResponseDto.ErrorResponse($"Error al crear la frase: {ex.Message}");
            }
        }

        public async Task<FraseResponseDto> UpdateFraseAsync(int id, UpdateFraseDto updateFraseDto)
        {
            if (id <= 0)
                return FraseResponseDto.ErrorResponse("El ID debe ser mayor que cero");

            var validationResult = ValidateDto(updateFraseDto);
            if (!validationResult.IsValid)
                return FraseResponseDto.ErrorResponse(validationResult.Errors);

            var customErrors = ValidateCustomBusinessRules(updateFraseDto);
            if (customErrors.Any())
                return FraseResponseDto.ErrorResponse(customErrors);

            try
            {
                var frase = await _fraseRepository.GetByIdAsync(id);
                if (frase == null)
                    return FraseResponseDto.ErrorResponse($"No se encontró la frase con ID {id}");

                var existingFrase = await _fraseRepository.FindByEspañolOrInglesAsync(updateFraseDto.Español, updateFraseDto.Ingles, id);
                if (existingFrase != null)
                    return FraseResponseDto.ErrorResponse("Ya existe otra frase similar en el sistema");

                frase.Español = updateFraseDto.Español.Trim();
                frase.Ingles = updateFraseDto.Ingles.Trim();
                frase.Pronunciacion = updateFraseDto.Pronunciacion.Trim();
                frase.Categoria = updateFraseDto.Categoria.Trim();

                await _fraseRepository.UpdateAsync(frase);

                var dto = MapToDto(frase);
                return FraseResponseDto.SuccessResponse(dto, "Frase actualizada exitosamente");
            }
            catch (Exception ex)
            {
                return FraseResponseDto.ErrorResponse($"Error al actualizar la frase: {ex.Message}");
            }
        }

        public async Task<FraseResponseDto> DeleteFraseAsync(int id)
        {
            if (id <= 0)
                return FraseResponseDto.ErrorResponse("El ID debe ser mayor que cero");

            try
            {
                var frase = await _fraseRepository.GetByIdAsync(id);
                if (frase == null)
                    return FraseResponseDto.ErrorResponse($"No se encontró la frase con ID {id}");

                await _fraseRepository.DeleteAsync(frase);

                return FraseResponseDto.SuccessResponse("Frase eliminada exitosamente");
            }
            catch (Exception ex)
            {
                return FraseResponseDto.ErrorResponse($"Error al eliminar la frase: {ex.Message}");
            }
        }

        public async Task<FraseResponseDto> GetFrasesByCategoriaAsync(string categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria))
                return FraseResponseDto.ErrorResponse("La categoría es requerida");

            if (!System.Text.RegularExpressions.Regex.IsMatch(categoria.Trim(), @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
                return FraseResponseDto.ErrorResponse("La categoría solo puede contener letras y espacios");

            try
            {
                var frases = await _fraseRepository.GetByCategoriaAsync(categoria);
                var frasesDto = frases.Select(MapToDto).ToList();

                return FraseResponseDto.SuccessResponse(frasesDto,
                    $"Se encontraron {frasesDto.Count} frases en la categoría '{categoria.Trim()}'");
            }
            catch (Exception ex)
            {
                return FraseResponseDto.ErrorResponse($"Error al obtener las frases por categoría: {ex.Message}");
            }
        }

        #region Métodos Privados

        private static FraseDto MapToDto(Frase frase)
        {
            return new FraseDto
            {
                Id = frase.Id,
                Español = frase.Español,
                Ingles = frase.Ingles,
                Pronunciacion = frase.Pronunciacion,
                Categoria = frase.Categoria
            };
        }

        private static ValidationResult ValidateDto<T>(T dto) where T : class
        {
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            return new ValidationResult
            {
                IsValid = isValid,
                Errors = validationResults.Select(vr => vr.ErrorMessage ?? "Error de validación").ToList()
            };
        }

        private static List<string> ValidateCustomBusinessRules<T>(T dto)
        {
            var errors = new List<string>();

            if (dto is CreateFraseDto createDto)
            {
                if (string.IsNullOrWhiteSpace(createDto.Español))
                    errors.Add("El campo en Español no puede estar vacío o contener solo espacios");

                if (string.IsNullOrWhiteSpace(createDto.Ingles))
                    errors.Add("El campo en Inglés no puede estar vacío o contener solo espacios");

                if (string.IsNullOrWhiteSpace(createDto.Pronunciacion))
                    errors.Add("La pronunciación no puede estar vacía o contener solo espacios");

                if (string.IsNullOrWhiteSpace(createDto.Categoria))
                    errors.Add("La categoría no puede estar vacía o contener solo espacios");

                if (!string.IsNullOrWhiteSpace(createDto.Español) &&
                    System.Text.RegularExpressions.Regex.IsMatch(createDto.Español, @"[<>""'&]"))
                    errors.Add("El campo en Español no puede contener caracteres especiales como <, >, \", ', &");

                if (!string.IsNullOrWhiteSpace(createDto.Ingles) &&
                    System.Text.RegularExpressions.Regex.IsMatch(createDto.Ingles, @"[<>""'&]"))
                    errors.Add("El campo en Inglés no puede contener caracteres especiales como <, >, \", ', &");
            }

            if (dto is UpdateFraseDto updateDto)
            {
                if (string.IsNullOrWhiteSpace(updateDto.Español))
                    errors.Add("El campo en Español no puede estar vacío o contener solo espacios");

                if (string.IsNullOrWhiteSpace(updateDto.Ingles))
                    errors.Add("El campo en Inglés no puede estar vacío o contener solo espacios");

                if (string.IsNullOrWhiteSpace(updateDto.Pronunciacion))
                    errors.Add("La pronunciación no puede estar vacía o contener solo espacios");

                if (string.IsNullOrWhiteSpace(updateDto.Categoria))
                    errors.Add("La categoría no puede estar vacía o contener solo espacios");

                if (!string.IsNullOrWhiteSpace(updateDto.Español) &&
                    System.Text.RegularExpressions.Regex.IsMatch(updateDto.Español, @"[<>""'&]"))
                    errors.Add("El campo en Español no puede contener caracteres especiales como <, >, \", ', &");

                if (!string.IsNullOrWhiteSpace(updateDto.Ingles) &&
                    System.Text.RegularExpressions.Regex.IsMatch(updateDto.Ingles, @"[<>""'&]"))
                    errors.Add("El campo en Inglés no puede contener caracteres especiales como <, >, \", ', &");
            }

            return errors;
        }

        #endregion

        private class ValidationResult
        {
            public bool IsValid { get; set; }
            public List<string> Errors { get; set; } = new List<string>();
        }
    }
}
