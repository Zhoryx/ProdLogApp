namespace ProdLogApp.Models
{
    // Entidad de dominio para Producto.
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int CategoriaId { get; set; }
        public bool Activo { get; set; } = true;

        // Nombre de categoría para mostrar en listados (no siempre vendrá cargado).
      
        public string? CategoriaNombre { get; set; }

        // Texto pensado para UI.
        public string EstadoActivo => Activo ? "Sí" : "No";
    }
}
