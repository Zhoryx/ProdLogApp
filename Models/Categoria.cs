namespace ProdLogApp.Models
{
    public class Categoria
    {
        public int CategoriaId { get; set; }           
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
        public string EstadoActivo => Activo ? "Sí" : "No";
    }
}
