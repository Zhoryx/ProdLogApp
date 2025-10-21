using System;
using System.Collections.Generic;
using ProdLogApp.Models;

namespace ProdLogApp.Interfaces
{
    /// Interfaz que representa la vista de gestión de productos.
    /// Se utiliza en conjunto con el Presenter correspondiente.
    public interface IGestProductosVista
    {
        event Action OnAgregar;          // Alta de producto
        event Action OnModificar;        // Edición de producto
        event Action OnAlternarEstado;   // Activa/Desactiva producto
        event Action OnEliminar;         // Elimina (lógica o física)
        event Action OnVolverMenu;       // Regresa al menú

        void MostrarProductos(List<Producto> productos);        // Carga los productos en la vista
        Producto ObtenerProductoSeleccionado();                  // Devuelve el producto actual

        void AbrirVentanaAgregarProducto();                      // Ventana de alta
        void AbrirVentanaModificarProducto(Producto producto);   // Ventana de edición

        void MostrarMensaje(string mensaje);                     // Feedback al usuario
        void NavegarAMenu();                                    // Regresar al menú
    }
}
