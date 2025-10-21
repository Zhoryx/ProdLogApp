namespace ProdLogApp.Models
{
    public class Producto
    {
        public int Id { get; set; }                  
        public string Nombre { get; set; } = string.Empty;
        public int CategoriaId { get; set; }
        public bool Activo { get; set; } = true;
        public string? CategoriaNombre { get; set; }
        public string EstadoActivo => Activo ? "Sí" : "No";
    }
}
