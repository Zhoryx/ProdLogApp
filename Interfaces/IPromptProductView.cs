using ProdLogApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdLogApp.Interfaces
{
    public interface IPromptProductView
    {
        
        void MostrarProductos(List<Producto> productos);
        void MostrarMensaje(string mensaje);

    }
}
