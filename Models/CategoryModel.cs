public class Categoria
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public bool Activo { get; set; }

   
    public string EstadoActivo => Activo ? "No" : "Sí";
}


