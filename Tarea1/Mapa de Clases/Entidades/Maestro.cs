namespace Mapa_de_Clases.Entidades
{
    public class Maestro : Docente
    {
        public string Especialidad { get; set; }
        public string Certificacion { get; set; }

        public Maestro(string nombre, string apellido, string correoElectronico, int telefono, string direccion, string cargo, string departamento, int fechaDeContratacion, int experiencia, string materia, int grado, string especialidad, string certificacion)
            : base(nombre, apellido, correoElectronico, telefono, direccion, cargo, departamento, fechaDeContratacion, experiencia, materia, grado)
        {
            Especialidad = especialidad;
            Certificacion = certificacion;
        }
       }
}
