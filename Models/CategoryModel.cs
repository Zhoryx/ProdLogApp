public class Categoria
{
    public int CategoryId { get; set; }
    public string Nombre { get; set; }
    public bool Activo { get; set; }

   
    public string EstadoActivo => Activo ? "No" : "Sí";
}


