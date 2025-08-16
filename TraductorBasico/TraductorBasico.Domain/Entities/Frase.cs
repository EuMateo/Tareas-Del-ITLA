namespace TraductorBasico.Domain.Entities
{
    public class Frase
    {
        public int Id { get; set; }
        public string Español { get; set; } = null!;
        public string Ingles { get; set; } = null!;
        public string Pronunciacion { get; set; } = null!;
        public string Categoria { get; set; } = null!;
        public ICollection<FraseImagen> FraseImagenes { get; set; } = new List<FraseImagen>();
    }
}