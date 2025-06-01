namespace Mapa_de_Clases.Entidades
{
    public class Administrativo : Empleado
    {
        public string Area { get; set; }
        public string Turno { get; set; }

        public Administrativo(string nombre, string apellido, string correoElectronico, int telefono, string direccion, string cargo, string departamento, int fechaDeContratacion,int experiencia, string area, string turno)
            : base(nombre, apellido, correoElectronico, telefono, direccion, cargo, departamento, fechaDeContratacion, experiencia)
        {
            Area = area;
            Turno = turno;
        }
       }
}
