using System.ComponentModel.DataAnnotations;

namespace TraductorBasico.Dtos
{
    public class FraseDto
    {
        public int Id { get; set; }
        public string Español { get; set; } = null!;
        public string Ingles { get; set; } = null!;
        public string Pronunciacion { get; set; } = null!;
        public string Categoria { get; set; } = null!;
    }
}