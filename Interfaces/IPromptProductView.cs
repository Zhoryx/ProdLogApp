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
        //event Action OnReturn;
        void MostrarProductos(List<Producto> productos);
        void MostrarCategorias(List<Categoria> categorias);

    }
}
