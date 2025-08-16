using System.ComponentModel.DataAnnotations;
using TraductorBasico.Contract;
using TraductorBasico.Domain.Entities;
using TraductorBasico.Dtos;

namespace TraductorBasico.Application.Services
{
    public class DiccionarioService : IDiccionarioService
    {
        private readonly IImagenDiccionarioRepository _imagenRepository;
        private readonly IFraseRepository _fraseRepository;

        public DiccionarioService(IImagenDiccionarioRepository imagenRepository, IFraseRepository fraseRepository)
        {
            _imagenRepository = imagenRepository;
            _fraseRepository = fraseRepository;
        }

        public async Task<DiccionarioResponseDto> GetAllImagenesAsync()
        {
            try
            {
                var imagenes = await _imagenRepository.GetAllWithFrasesAsync();
                var imagenesDto = imagenes.Select(MapToDto).ToList();

                return DiccionarioResponseDto.SuccessResponse(imagenesDto,
                    $"Se encontraron {imagenesDto.Count} imágenes en el diccionario");
            }
            catch (Exception ex)
            {
                return DiccionarioResponseDto.ErrorResponse($"Error al obtener las imágenes: {ex.Message}");
            }
        }

        public async Task<DiccionarioResponseDto> GetImagenByIdAsync(int id)
        {
            if (id <= 0)
                return DiccionarioResponseDto.ErrorResponse("El ID debe ser mayor que cero");

            try
            {
                var imagen = await _imagenRepository.GetByIdAsync(id);
                if (imagen == null)
                    return DiccionarioResponseDto.ErrorResponse($"No se encontró la imagen con ID {id}");

                var dto = MapToDto(imagen);
                return DiccionarioResponseDto.SuccessResponse(dto, "Imagen encontrada exitosamente");
            }
            catch (Exception ex)
            {
                return DiccionarioResponseDto.ErrorResponse($"Error al obtener la imagen: {ex.Message}");
            }
        }

        public async Task<DiccionarioResponseDto> GetImagenesByCategoriaAsync(string categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria))
                return DiccionarioResponseDto.ErrorResponse("La categoría es requerida");

            if (!System.Text.RegularExpressions.Regex.IsMatch(categoria.Trim(), @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
                return DiccionarioResponseDto.ErrorResponse("La categoría solo puede contener letras y espacios");

            try
            {
                var imagenes = await _imagenRepository.GetByCategoriaAsync(categoria);
                var imagenesDto = imagenes.Select(MapToDto).ToList();

                return DiccionarioResponseDto.SuccessResponse(imagenesDto,
                    $"Se encontraron {imagenesDto.Count} imágenes en la categoría '{categoria.Trim()}'");
            }
            catch (Exception ex)
            {
                return DiccionarioResponseDto.ErrorResponse($"Error al obtener las imágenes por categoría: {ex.Message}");
            }
        }

        public async Task<DiccionarioResponseDto> GetImagenWithFrasesAsync(int id)
        {
            if (id <= 0)
                return DiccionarioResponseDto.ErrorResponse("El ID debe ser mayor que cero");

            try
            {
                var imagen = await _imagenRepository.GetByIdWithFrasesAsync(id);
                if (imagen == null)
                    return DiccionarioResponseDto.ErrorResponse($"No se encontró la imagen con ID {id}");

                var dto = MapToDtoWithFrases(imagen);
                return DiccionarioResponseDto.SuccessResponse(dto, "Imagen con frases encontrada exitosamente");
            }
            catch (Exception ex)
            {
                return DiccionarioResponseDto.ErrorResponse($"Error al obtener la imagen con frases: {ex.Message}");
            }
        }

        public async Task<DiccionarioResponseDto> CreateImagenAsync(CreateImagenDiccionarioDto createImagenDto)
        {
            var validationResult = ValidateDto(createImagenDto);
            if (!validationResult.IsValid)
                return DiccionarioResponseDto.ErrorResponse(validationResult.Errors);

            try
            {
                var imagen = new ImagenDiccionario
                {
                    Nombre = createImagenDto.Nombre.Trim(),
                    RutaImagen = createImagenDto.RutaImagen.Trim(),
                    Categoria = createImagenDto.Categoria.Trim(),
                    Descripcion = createImagenDto.Descripcion.Trim(),
                    FechaCreacion = DateTime.Now
                };

                await _imagenRepository.AddAsync(imagen);

                // Asignar frases si se proporcionaron
                if (createImagenDto.FrasesIds.Any())
                {
                    await _imagenRepository.AsignarFrasesAsync(imagen.Id, createImagenDto.FrasesIds);
                }

                // Obtener la imagen completa con frases para el response
                var imagenCompleta = await _imagenRepository.GetByIdWithFrasesAsync(imagen.Id);
                var dto = MapToDtoWithFrases(imagenCompleta!);

                return DiccionarioResponseDto.SuccessResponse(dto, "Imagen creada exitosamente");
            }
            catch (Exception ex)
            {
                return DiccionarioResponseDto.ErrorResponse($"Error al crear la imagen: {ex.Message}");
            }
        }

        public async Task<DiccionarioResponseDto> DeleteImagenAsync(int id)
        {
            if (id <= 0)
                return DiccionarioResponseDto.ErrorResponse("El ID debe ser mayor que cero");

            try
            {
                var imagen = await _imagenRepository.GetByIdAsync(id);
                if (imagen == null)
                    return DiccionarioResponseDto.ErrorResponse($"No se encontró la imagen con ID {id}");

                await _imagenRepository.DeleteAsync(imagen);

                return DiccionarioResponseDto.SuccessResponse(new List<ImagenDiccionarioDto>(),
                    "Imagen eliminada exitosamente");
            }
            catch (Exception ex)
            {
                return DiccionarioResponseDto.ErrorResponse($"Error al eliminar la imagen: {ex.Message}");
            }
        }

        public async Task<DiccionarioResponseDto> AsignarFrasesAImagenAsync(int imagenId, List<int> frasesIds)
        {
            if (imagenId <= 0)
                return DiccionarioResponseDto.ErrorResponse("El ID de la imagen debe ser mayor que cero");

            if (!frasesIds.Any())
                return DiccionarioResponseDto.ErrorResponse("Debe proporcionar al menos un ID de frase");

            try
            {
                var success = await _imagenRepository.AsignarFrasesAsync(imagenId, frasesIds);
                if (!success)
                    return DiccionarioResponseDto.ErrorResponse("No se pudo asignar las frases a la imagen");

                // Obtener la imagen actualizada
                var imagen = await _imagenRepository.GetByIdWithFrasesAsync(imagenId);
                var dto = MapToDtoWithFrases(imagen!);

                return DiccionarioResponseDto.SuccessResponse(dto, "Frases asignadas exitosamente");
            }
            catch (Exception ex)
            {
                return DiccionarioResponseDto.ErrorResponse($"Error al asignar frases: {ex.Message}");
            }
        }

        public async Task<DiccionarioResponseDto> GetCategoriasDisponiblesAsync()
        {
            try
            {
                var imagenes = await _imagenRepository.GetAllAsync();
                var categorias = imagenes
                    .Select(i => i.Categoria)
                    .Distinct()
                    .OrderBy(c => c)
                    .Select(categoria => new ImagenDiccionarioDto
                    {
                        Categoria = categoria,
                        CantidadFrases = imagenes.Count(i => i.Categoria == categoria)
                    })
                    .ToList();

                return DiccionarioResponseDto.SuccessResponse(categorias,
                    $"Se encontraron {categorias.Count} categorías disponibles");
            }
            catch (Exception ex)
            {
                return DiccionarioResponseDto.ErrorResponse($"Error al obtener las categorías: {ex.Message}");
            }
        }

        #region Métodos Privados

        private static ImagenDiccionarioDto MapToDto(ImagenDiccionario imagen)
        {
            return new ImagenDiccionarioDto
            {
                Id = imagen.Id,
                Nombre = imagen.Nombre,
                RutaImagen = imagen.RutaImagen,
                Categoria = imagen.Categoria,
                Descripcion = imagen.Descripcion,
                CantidadFrases = imagen.FraseImagenes?.Count ?? 0
            };
        }

        private static ImagenDiccionarioDto MapToDtoWithFrases(ImagenDiccionario imagen)
        {
            var dto = MapToDto(imagen);
            dto.FrasesRelacionadas = imagen.FraseImagenes?
                .Select(fi => new FraseDto
                {
                    Id = fi.Frase.Id,
                    Español = fi.Frase.Español,
                    Ingles = fi.Frase.Ingles,
                    Pronunciacion = fi.Frase.Pronunciacion,
                    Categoria = fi.Frase.Categoria
                })
                .ToList() ?? new List<FraseDto>();

            return dto;
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

        #endregion

        private class ValidationResult
        {
            public bool IsValid { get; set; }
            public List<string> Errors { get; set; } = new List<string>();
        }
    }
}