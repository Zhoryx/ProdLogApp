namespace ProdLogApp.Models
{
    public class Puesto
    {
        public int PuestoId { get; set; }             
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;

        public string EstadoActivo => Activo ? "Sí" : "No";
    }
}
