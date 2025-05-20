using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdLogApp.Models
{
    class ParteModel

    {
        public int ParteId { get; set; }  // Clave primaria (autoincremental en SQL)
        public DateTime ParteFecha { get; set; }  // Fecha de la parte
        public int ProduccionId { get; set; }  // Relación con Producción
        public int UsuarioId { get; set; }  // Relación con Usuario
    }
}
