namespace ProdLogApp.Models
{
    // Entidad de dominio para Categoría de productos.
    public class Categoria
    {
        public int CategoriaId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;

        // Texto pensado para UI.
        public string EstadoActivo => Activo ? "Sí" : "No";
    }
}
