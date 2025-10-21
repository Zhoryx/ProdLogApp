using System;

namespace ProdLogApp.Models
{
    public class Parte
    {
        public int ParteId { get; set; }            
        public int UsuarioId { get; set; }
        public DateTime ParteFecha { get; set; }     
        public string? UsuarioNombre { get; set; }
    }
}
