namespace Mapa_de_Clases.Entidades
{
    public class Administrador : Docente
    {
        public string Nivel { get; set; }
        public string Categoria { get; set; }

        public Administrador(string nombre, string apellido, string correoElectronico, int telefono, string direccion, string cargo, string departamento, int fechaDeContratacion, int experiencia, string materia, int grado, string nivel, string categoria)
            : base(nombre, apellido, correoElectronico, telefono, direccion, cargo, departamento, fechaDeContratacion, experiencia, materia, grado)
        {
            Nivel = nivel;
            Categoria = categoria;
        }
       }
}
