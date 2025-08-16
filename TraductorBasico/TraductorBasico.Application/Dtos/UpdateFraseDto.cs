using System.ComponentModel.DataAnnotations;

namespace TraductorBasico.Dtos
{
    public class UpdateFraseDto
    {
        [Required(ErrorMessage = "El campo en Español es requerido")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "El campo en Español debe tener entre 1 y 500 caracteres")]
        public string Español { get; set; } = null!;

        [Required(ErrorMessage = "El campo en Inglés es requerido")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "El campo en Inglés debe tener entre 1 y 500 caracteres")]
        public string Ingles { get; set; } = null!;

        [Required(ErrorMessage = "La pronunciación es requerida")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "La pronunciación debe tener entre 1 y 500 caracteres")]
        public string Pronunciacion { get; set; } = null!;

        [Required(ErrorMessage = "La categoría es requerida")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "La categoría debe tener entre 1 y 100 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "La categoría solo puede contener letras y espacios")]
        public string Categoria { get; set; } = null!;
    }
}
