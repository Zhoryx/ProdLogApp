using System;

namespace ProdLogApp.Models
{
    public class Produccion
    {
        public int ProduccionId { get; set; }         
        public TimeSpan HoraInicio { get; set; }      
        public TimeSpan HoraFin { get; set; }         
        public int Cantidad { get; set; }             

        public int ProductoId { get; set; }
        public int PuestoId { get; set; }
        public int ParteId { get; set; }

       
        public string? ProductoNombre { get; set; }
        public string CategoriaNombre { get; set; }  

        public string? PuestoNombre { get; set; }

        
        public TimeSpan Duracion => HoraFin - HoraInicio;
        public bool EsValida => Cantidad > 0 && Duracion.TotalMinutes > 0;

// 👈
       
    }
}
