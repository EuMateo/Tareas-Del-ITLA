using System.ComponentModel.DataAnnotations;

namespace TraductorBasico.Dtos
{
    public class ImagenDiccionarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string RutaImagen { get; set; } = null!;
        public string Categoria { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public int CantidadFrases { get; set; }
        public List<FraseDto> FrasesRelacionadas { get; set; } = new List<FraseDto>();
    }

    public class CreateImagenDiccionarioDto
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "El nombre debe tener entre 1 y 100 caracteres")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "La ruta de la imagen es requerida")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "La ruta debe tener entre 1 y 500 caracteres")]
        public string RutaImagen { get; set; } = null!;

        [Required(ErrorMessage = "La categoría es requerida")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "La categoría debe tener entre 1 y 100 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "La categoría solo puede contener letras y espacios")]
        public string Categoria { get; set; } = null!;

        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        public List<int> FrasesIds { get; set; } = new List<int>();
    }

    public class DiccionarioResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public ImagenDiccionarioDto? Data { get; set; }
        public IEnumerable<ImagenDiccionarioDto>? DataList { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public static DiccionarioResponseDto SuccessResponse(ImagenDiccionarioDto data, string message = "Operación exitosa")
        {
            return new DiccionarioResponseDto
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static DiccionarioResponseDto SuccessResponse(IEnumerable<ImagenDiccionarioDto> dataList, string message = "Operación exitosa")
        {
            return new DiccionarioResponseDto
            {
                Success = true,
                Message = message,
                DataList = dataList
            };
        }

        public static DiccionarioResponseDto ErrorResponse(string errorMessage)
        {
            return new DiccionarioResponseDto
            {
                Success = false,
                Message = errorMessage,
                Errors = new List<string> { errorMessage }
            };
        }

        public static DiccionarioResponseDto ErrorResponse(List<string> errors)
        {
            return new DiccionarioResponseDto
            {
                Success = false,
                Message = "Se encontraron errores",
                Errors = errors
            };
        }
    }
}