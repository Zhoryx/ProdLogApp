using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace ProdLogApp.Models
    {
        public class ParteHeaderItem
        {
            public int ParteId { get; set; }
            public DateTime FechaParte { get; set; }
            public int UserId { get; set; }
            public string Operario { get; set; }

            public int CantProducciones { get; set; }
            public decimal TotalCantidad { get; set; }
        }
    }


