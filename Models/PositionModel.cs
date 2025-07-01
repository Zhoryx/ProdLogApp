using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdLogApp.Models
{
    public class Position
    {
        public int PuestoId { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public string EstadoActivo => Activo ? "No" : "Sí";
    }

}
