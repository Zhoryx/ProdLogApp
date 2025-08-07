using ProdLogApp.Models;
using System;
using System.Collections.Generic;
using System.Windows;

namespace ProdLogApp.Interfaces
{
    public interface IProductManagementView
    {
        //  Eventos de gestión de productos
        event Action OnModifyProduct;
        event Action OnReturn;
        event Action OnAddProduct;

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
