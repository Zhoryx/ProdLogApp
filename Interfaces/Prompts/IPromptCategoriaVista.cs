using System;
using System.Collections.Generic;
using ProdLogApp.Models;

namespace ProdLogApp.Interfaces
{
    // Vista de selección de Categoría (prompt o ventana modal).
    // Permite elegir una Categoría desde la lista mostrada.
    public interface IPromptCategoriaVista
    {
        // Eventos que la vista dispara
        event Action OnConfirmarSeleccion; // Confirmar selección
        event Action OnCancelar;            // Cancelar

        // Métodos que el Presenter invoca sobre la vista
        void CargarCategorias(List<Categoria> categorias);   // Carga la lista de categorías
        Categoria ObtenerCategoriaSeleccionada();            // Devuelve la categoría seleccionada
        void MostrarMensaje(string mensaje);                 // Mensaje informativo o de error
        void Cerrar();                                       // Cierra la ventana/modal
    }
}
