using System;

namespace ProdLogApp.Models
{
    // Cabecera de Parte (un parte por usuario por día).
    public class Parte
    {
        public int ParteId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime ParteFecha { get; set; }     // Suele manejarse como DATE (sin hora)
        public string? UsuarioNombre { get; set; }   // Decorativo para listados
    }
}
