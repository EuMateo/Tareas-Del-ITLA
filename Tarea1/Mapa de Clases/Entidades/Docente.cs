namespace Mapa_de_Clases.Entidades
{
    public class Docente : Empleado
    {
        public string Materia { get; set; }
        public int Grado { get; set; }
     

        public Docente(string nombre, string apellido, string correoElectronico, int telefono, string direccion, string cargo, string departamento, int fechaDeContratacion, int experiencia, string materia, int grado)
            : base(nombre, apellido, correoElectronico, telefono, direccion, cargo, departamento, fechaDeContratacion, experiencia)
        {
            Materia = materia;
            Grado = grado;
        }
       }
}
