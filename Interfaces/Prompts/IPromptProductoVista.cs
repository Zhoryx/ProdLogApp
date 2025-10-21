using System;
using System.Collections.Generic;
using ProdLogApp.Models;

namespace ProdLogApp.Interfaces
{
    // Vista de selección de Producto (prompt o ventana modal).
    // Se usa cuando otra vista necesita elegir un Producto existente.
    public interface IPromptProductoVista
    {
        // Eventos que la vista dispara
        event Action OnConfirmarSeleccion; // Confirmar selección
        event Action OnCancelar;            // Cancelar

        // Métodos que el Presenter invoca sobre la vista
        void CargarProductos(List<Producto> productos);  // Cargar la lista de productos disponibles
        Producto ObtenerProductoSeleccionado();          // Devuelve el producto seleccionado
        void MostrarMensaje(string mensaje);             // Mensaje informativo o de error
        void Cerrar();                                   // Cerrar el diálogo
    }
}
