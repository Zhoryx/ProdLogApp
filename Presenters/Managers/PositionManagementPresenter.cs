using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Services;
using System;

namespace ProdLogApp.Presenters
{
    public class PositionManagementPresenter
    {
        private readonly IPositionManagementView _view;
        private readonly IDatabaseService _databaseService;

        public PositionManagementPresenter(IPositionManagementView view, IDatabaseService databaseService)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));

            // Suscribirse a eventos de la vista
            _view.OnAddPosition += AgregarPuesto;
            _view.OnModifyPosition += ModificarPuesto;
            _view.OnReturn += () => _view.NavigateToMenu();
            _view.OnDeletePosition += CambiarEstadoPuesto;

        }

        public void AgregarPuesto()
        {
            _view.AbrirVentanaAgregar();
            CargarPuestos(); // Recargar después de agregar
        }

        public void ModificarPuesto()
        {
            var puesto = _view.ObtenerPuestoSeleccionado();
            if (puesto == null)
            {
                _view.MostrarMensaje("Seleccione un puesto para modificar.");
                return;
            }

            _view.AbrirVentanaModificar(puesto);
            CargarPuestos();
        }

        public void CargarPuestos()
        {
            var puestos = _databaseService.ObtenerTodosLosPuestos();
            _view.MostrarPuestos(puestos);
        }

        public void CambiarEstadoPuesto()
        {
            var puesto = _view.ObtenerPuestoSeleccionado();
            if (puesto == null)
            {
                _view.MostrarMensaje("Seleccioná un puesto para cambiar su estado.");
                return;
            }

            _databaseService.TogglePositionState(puesto.PuestoId, puesto.Activo);
            _view.MostrarMensaje(puesto.Activo ? "Puesto activado correctamente." : "Puesto desactivado correctamente.");
            CargarPuestos();
        }

    }
}
