namespace TraductorBasico.Domain.Entities
{
    public class ImagenDiccionario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string RutaImagen { get; set; } = null!;
        public string Categoria { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public ICollection<FraseImagen> FraseImagenes { get; set; } = new List<FraseImagen>();
    }

    public class FraseImagen
    {
        public int FraseId { get; set; }
        public int ImagenId { get; set; }
        public Frase Frase { get; set; } = null!;
        public ImagenDiccionario Imagen { get; set; } = null!;
    }
}