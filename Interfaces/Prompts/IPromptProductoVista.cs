using System;
using System.Collections.Generic;
using ProdLogApp.Models;

namespace ProdLogApp.Interfaces
{
    public interface IPromptProductoVista
    {
        event Action OnConfirmarSeleccion;
        event Action OnCancelar;

        void CargarProductos(List<Producto> productos);
        Producto ObtenerProductoSeleccionado();
        void MostrarMensaje(string mensaje);
        void Cerrar();
    }
}
