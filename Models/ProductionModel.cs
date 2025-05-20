using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdLogApp.Models
{
    public class Production
    {
        public int ProductionId { get; set; }  // Clave primaria
        public TimeSpan HInicio { get; set; }  // Hora de inicio
        public TimeSpan HFin { get; set; }  // Hora de fin
        public int Cantidad { get; set; }  // Cantidad producida
        public int ProductoId { get; set; }  // Relación con Producto
        public int PuestoId { get; set; }  // Relación con Puesto

        Production todayProduction = new Production();

    }
}
