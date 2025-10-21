using System;
using ProdLogApp.Interfaces;

namespace ProdLogApp.Presenters
{
    public sealed class MenuProduccionGerentePresenter
    {
        private readonly IMenuProduccionGerenteVista _vista;

        public MenuProduccionGerentePresenter(IMenuProduccionGerenteVista vista)
        {
            _vista = vista;

            _vista.AlAbrirConsultaPorFecha += () => _vista.MostrarMensaje("Ir a consulta por fecha.");
            _vista.AlAbrirConsultaPorRango += () => _vista.MostrarMensaje("Ir a consulta por rango.");
            _vista.AlAbrirDashboard += () => _vista.MostrarMensaje("Ir a dashboard de producción.");
            _vista.AlExportar += () => _vista.MostrarMensaje("Exportación desde menú.");
            _vista.AlVolverMenuPrincipal += () => _vista.MostrarMensaje("Volver al menú del gerente.");
        }
    }
}
