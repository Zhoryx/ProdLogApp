using ProdLogApp.Models;
using System;
using System.Collections.Generic;
using System.Windows;

namespace ProdLogApp.Views
{
    public interface IProductManagementView
    {
        //  Eventos de gestión de productos
        event Action OnModifyProduct;
        event Action OnReturn;

        //  Métodos para abrir vistas
        void Addview();
        void Modifyview(Producto producto);
        void NavigateToMenu();

        //  Métodos para actualizar la UI
        void MostrarProductos(List<Producto> productos);
        void MostrarMensaje(string mensaje);
        Producto ObtenerProductoSeleccionado();
      
    }
}
