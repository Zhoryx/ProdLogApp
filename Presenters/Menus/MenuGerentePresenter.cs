using System;
using ProdLogApp.Interfaces;

namespace ProdLogApp.Presenters
{
    public sealed class MenuGerentePresenter
    {
        private readonly IMenuGerenteVista _vista;

        public MenuGerentePresenter(IMenuGerenteVista vista)
        {
            _vista = vista;

            _vista.OnAbrirGestionCategorias += () => _vista.MostrarMensaje("Abrir Gestión de Categorías.");
            _vista.OnAbrirGestionProductos += () => _vista.MostrarMensaje("Abrir Gestión de Productos.");
            _vista.OnAbrirGestionPuestos += () => _vista.MostrarMensaje("Abrir Gestión de Puestos.");
            _vista.OnAbrirGestionUsuarios += () => _vista.MostrarMensaje("Abrir Gestión de Usuarios.");
            _vista.OnAbrirPanelProduccion += () => _vista.MostrarMensaje("Abrir panel de Producción.");
            _vista.OnSalir += () => _vista.MostrarMensaje("Volver / Cerrar sesión.");
        }
    }
}
