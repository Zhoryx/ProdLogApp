using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdLogApp.Models
{
    // Paginador genérico para listas (útil en grillas de gerente).
    public class Page<T>
    {
        public List<T> Items { get; set; } = new();
        public int PageNumber { get; set; } // Número de página actual (1..N)
        public int PageSize { get; set; }   // Tamaño de página solicitado
        public int Total { get; set; }      // Total de registros (sin paginar)

       
    }
}
