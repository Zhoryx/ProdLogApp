using System;

namespace ProdLogApp.Models
{
    // Detalle de Producción asociado a un Parte (cabecera).
    // Contiene referencias a Producto y Puesto (por Id) y datos enriquecidos (nombres) para UI.
    public class Produccion
    {
        public int ProduccionId { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public int Cantidad { get; set; }

        public int ProductoId { get; set; }
        public int PuestoId { get; set; }
        public int ParteId { get; set; }

        // Campos “decorativos” para mostrar en grillas (pueden venir null si la query no los incluye).
        public string? ProductoNombre { get; set; }

       
        public string CategoriaNombre { get; set; }

        public string? PuestoNombre { get; set; }

        // Duración calculada (no valida horarios cruzados; si Fin < Inicio dará negativo).
        public TimeSpan Duracion => HoraFin - HoraInicio;

        // Validación básica: cantidad > 0 y duración positiva (requiere que HoraFin > HoraInicio).
        public bool EsValida => Cantidad > 0 && Duracion.TotalMinutes > 0;

    }
}
