namespace Mapa_de_Clases.Entidades
{
    public class ExAlumno : MiembroDeLaComunidad
    {
        public int AñoDeGraduacion { get; set; }
        public string Carrera { get; set; }
        public string TrabajoActual { get; set; }

        public ExAlumno(string nombre, string apellido, string correoElectronico, int telefono, string direccion, int añoDeGraduacion, string carrera, string trabajoActual)
            : base(nombre, apellido, correoElectronico, telefono, direccion)
        {
            AñoDeGraduacion = añoDeGraduacion;
            Carrera = carrera;
            TrabajoActual = trabajoActual;
        }
    }
}
