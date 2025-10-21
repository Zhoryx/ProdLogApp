using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdLogApp.Models
{
    // Item de cabecera para listados paginados (DTO/VM para grilla de Partes).
    // Similar a 'Parte', pero ya trae datos agregados (cantidades, conteo).
    public class ParteHeaderItem
    {
        public int ParteId { get; set; }
        public DateTime FechaParte { get; set; } // Mismo concepto que Parte.ParteFecha
        public int UserId { get; set; }          // Mismo que Parte.UsuarioId
        public string Operario { get; set; }

        public int CantProducciones { get; set; }  // Cantidad de ítems en el detalle
        public decimal TotalCantidad { get; set; } // Suma de Produccion_Cantidad
    }
}
