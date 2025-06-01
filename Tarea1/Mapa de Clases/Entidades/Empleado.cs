namespace Mapa_de_Clases.Entidades
{
    public class Empleado : MiembroDeLaComunidad
    {
        public string Cargo { get; set; }
        public string Departamento { get; set; }
        public int FechaDeContratacion { get; set; }
        public int Experiencia { get; set; } 
        public Empleado(string nombre, string apellido, string correoElectronico, int telefono, string direccion, string cargo, string departamento, int fechaDeContratacion, int experiencia)
            : base(nombre, apellido, correoElectronico, telefono, direccion)
        {
            Cargo = cargo;
            Departamento = departamento;
            FechaDeContratacion = fechaDeContratacion;
            Experiencia = experiencia;
        }
    }
}
