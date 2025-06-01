namespace Mapa_de_Clases.Entidades
{
    public class MiembroDeLaComunidad
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string CorreoElectronico { get; set; }
        public int Telefono { get; set; }
        public string Direccion { get; set; }
        public MiembroDeLaComunidad(string nombre, string apellido, string correoElectronico, int telefono, string direccion)
        {
            Nombre = nombre;
            Apellido = apellido;
            CorreoElectronico = correoElectronico;
            Telefono = telefono;
            Direccion = direccion;
        }
    }
}
