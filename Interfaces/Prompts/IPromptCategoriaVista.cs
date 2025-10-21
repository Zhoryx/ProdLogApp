using System;
using System.Collections.Generic;
using ProdLogApp.Models;

namespace ProdLogApp.Interfaces
{
    public interface IPromptCategoriaVista
    {
        event Action OnConfirmarSeleccion;
        event Action OnCancelar;

        void CargarCategorias(List<Categoria> categorias);
        Categoria ObtenerCategoriaSeleccionada();
        void MostrarMensaje(string mensaje);
        void Cerrar();
    }
}
