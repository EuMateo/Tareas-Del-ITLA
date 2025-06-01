namespace Mapa_de_Clases.Entidades
{
    public class Estudiante : MiembroDeLaComunidad
    {
        public string Carrera { get; set; }
        public int Matricula { get; set; }

        public Estudiante(string nombre, string apellido, string correoElectronico, int telefono, string direccion, string carrera, int matricula)
            : base(nombre, apellido, correoElectronico, telefono, direccion)
        {
            Carrera = carrera;
            Matricula = matricula;
        }
    }
} 
