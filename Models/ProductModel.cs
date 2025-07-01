namespace ProdLogApp.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int CategoriaId { get; set; }
        public string CategoriaNombre { get; set; }
        public bool Activo { get; set; }
        public string EstadoActivo => Activo ? "No" : "Sí";
    }
}
