using System;

namespace ProdLogApp.Interfaces
{
    public interface IMenuGerenteVista
    {
        event Action OnAbrirGestionCategorias;
        event Action OnAbrirGestionProductos;
        event Action OnAbrirGestionPuestos;
        event Action OnAbrirGestionUsuarios;
        event Action OnAbrirPanelProduccion;
        event Action OnSalir;

        void MostrarMensaje(string mensaje);
    }
}
