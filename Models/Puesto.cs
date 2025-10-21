namespace ProdLogApp.Models
{
    // Entidad de dominio para Puesto de trabajo.
    public class Puesto
    {
        public int PuestoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;

        // Texto pensado para UI (sí/no). 
        
        public string EstadoActivo => Activo ? "Sí" : "No";
    }
}
