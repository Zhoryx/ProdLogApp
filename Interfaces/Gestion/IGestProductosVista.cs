using System;
using System.Collections.Generic;
using ProdLogApp.Models;

namespace ProdLogApp.Interfaces
{
    public interface IGestProductosVista
    {
        event Action OnAgregar;
        event Action OnModificar;
        event Action OnAlternarEstado;
        event Action OnEliminar;
        event Action OnVolverMenu;

        void MostrarProductos(List<Producto> productos);
        Producto ObtenerProductoSeleccionado();

        void AbrirVentanaAgregarProducto();
        void AbrirVentanaModificarProducto(Producto producto);

        void MostrarMensaje(string mensaje);

       
        void NavegarAMenu();
    }
}
