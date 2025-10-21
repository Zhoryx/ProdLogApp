using System;
using ProdLogApp.Interfaces;

namespace ProdLogApp.Presenters
{
    // Presenter pasivo del menú de Producción del Gerente.
    // La navegación y acciones visuales se gestionan en el .xaml.cs.
    // Aquí solo se mantienen los hooks por consistencia con el resto del patrón.
    public sealed class MenuProduccionGerentePresenter
    {
        private readonly IMenuProduccionGerenteVista _vista;

        public MenuProduccionGerentePresenter(IMenuProduccionGerenteVista vista)
        {
            _vista = vista ?? throw new ArgumentNullException(nameof(vista));

            // Hooks vacíos: permiten agregar lógica adicional sin afectar la vista actual.
            _vista.AlAbrirConsultaPorFecha += () => { /* hook: consulta por fecha */ };
            _vista.AlAbrirConsultaPorRango += () => { /* hook: consulta por rango */ };
            _vista.AlAbrirDashboard += () => { /* hook: dashboard producción */ };
            _vista.AlExportar += () => { /* hook: exportación */ };
            _vista.AlVolverMenuPrincipal += () => { /* hook: volver al menú principal */ };
        }
    }
}
