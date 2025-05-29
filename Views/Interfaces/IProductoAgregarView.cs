using ProdLogApp.Models;
using System.Collections.Generic;

namespace ProdLogApp.Views
{
    public interface IProductoAgregarView
    {
        void MostrarCategorias(List<Categoria> categorias);
        Producto ObtenerDatosProducto();
        void MostrarMensaje(string mensaje);
        void CerrarVista();
    }
}
