using System;
using ProdLogApp.Interfaces;

namespace ProdLogApp.Presenters
{
    // Presenter pasivo del menú de Gerente.
    // La navegación se ejecuta en el .xaml.cs; aquí solo se dejan hooks opcionales.
    public sealed class MenuGerentePresenter
    {
        private readonly IMenuGerenteVista _vista;

        public MenuGerentePresenter(IMenuGerenteVista vista)
        {
            _vista = vista ?? throw new ArgumentNullException(nameof(vista));

            // Hooks opcionales para auditoría/telemetría/seguridad.
            _vista.OnAbrirGestionCategorias += () => { /* hook: categorías */ };
            _vista.OnAbrirGestionProductos += () => { /* hook: productos  */ };
            _vista.OnAbrirGestionPuestos += () => { /* hook: puestos    */ };
            _vista.OnAbrirGestionUsuarios += () => { /* hook: usuarios   */ };
            _vista.OnAbrirPanelProduccion += () => { /* hook: producción */ };
            _vista.OnSalir += () => { /* hook: logout     */ };
        }
    }
}
