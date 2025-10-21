using System;
using System.Linq;                    // ← necesario para .ToList()
using ProdLogApp.Interfaces;
using ProdLogApp.Servicios;

namespace ProdLogApp.Presenters
{
    public sealed class GestPuestosPresenter
    {
        private readonly IGestPuestosVista _vista;
        private readonly IServicioPuestos _svc;

        public GestPuestosPresenter(IGestPuestosVista vista, IServicioPuestos svc)
        {
            _vista = vista ?? throw new ArgumentNullException(nameof(vista));
            _svc = svc ?? throw new ArgumentNullException(nameof(svc));

            _vista.OnAgregar += Agregar;
            _vista.OnModificar += Modificar;
            _vista.OnAlternarEstado += Alternar;
            _vista.OnEliminar += Eliminar;
            _vista.OnVolverMenu += VolverMenu;

            Cargar();
        }

        private async void Cargar()
        {
            try
            {
                var lista = await _svc.ListarAsync();   // o ListarActivosAsync() si preferís
                _vista.MostrarPuestos(lista.ToList());
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"Error al cargar puestos: {ex.Message}");
            }
        }

        private void Agregar()
        {
            _vista.AbrirVentanaAgregarPuesto();
            Cargar(); // refrescar al cerrar el diálogo de alta
        }

        private void Modificar()
        {
            var sel = _vista.ObtenerPuestoSeleccionado();
            if (sel == null) { _vista.MostrarMensaje("Seleccioná un puesto."); return; }

            _vista.AbrirVentanaModificarPuesto(sel);
            Cargar(); // refrescar al cerrar el diálogo de edición
        }

        private async void Alternar()
        {
            var sel = _vista.ObtenerPuestoSeleccionado();
            if (sel == null) { _vista.MostrarMensaje("Seleccioná un puesto."); return; }

            try
            {
                await _svc.AlternarEstadoAsync(sel.PuestoId, !sel.Activo);
                Cargar();
                // _vista.MostrarMensaje(sel.Activo ? "Puesto desactivado." : "Puesto activado."); // opcional
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"Error al alternar estado: {ex.Message}");
            }
        }

        private async void Eliminar()
        {
            var sel = _vista.ObtenerPuestoSeleccionado();
            if (sel == null) { _vista.MostrarMensaje("Seleccioná un puesto."); return; }

            try
            {
                await _svc.EliminarAsync(sel.PuestoId);
                Cargar();
                // _vista.MostrarMensaje("Puesto eliminado."); // opcional
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"Error al eliminar: {ex.Message}");
            }
        }

        private void VolverMenu()
        {
            _vista.NavegarAMenu();  // la vista se encarga de reactivar Owner y Close()
        }
    }
}
