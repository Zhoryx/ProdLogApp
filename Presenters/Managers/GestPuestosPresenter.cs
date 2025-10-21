using System;
using System.Linq;                    // ← necesario para .ToList()
using ProdLogApp.Interfaces;
using ProdLogApp.Servicios;

namespace ProdLogApp.Presenters
{
    // Presenter de gestión de puestos.
    // Controla alta, edición, alternancia de estado y eliminación con refresco del listado.
    public sealed class GestPuestosPresenter
    {
        private readonly IGestPuestosVista _vista;
        private readonly IServicioPuestos _svc;

        public GestPuestosPresenter(IGestPuestosVista vista, IServicioPuestos svc)
        {
            _vista = vista ?? throw new ArgumentNullException(nameof(vista));
            _svc = svc ?? throw new ArgumentNullException(nameof(svc));

            // Suscripciones a eventos de la vista
            _vista.OnAgregar += Agregar;
            _vista.OnModificar += Modificar;
            _vista.OnAlternarEstado += Alternar;
            _vista.OnEliminar += Eliminar;
            _vista.OnVolverMenu += VolverMenu;

            // Carga inicial del listado
            Cargar();
        }

        // Carga/recarga de puestos
        private async void Cargar()
        {
            try
            {
                var lista = await _svc.ListarAsync();   // o ListarActivosAsync() según necesidad
                _vista.MostrarPuestos(lista.ToList());
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"Error al cargar puestos: {ex.Message}");
            }
        }

        // Abre diálogo de alta y luego refresca
        private void Agregar()
        {
            _vista.AbrirVentanaAgregarPuesto();
            Cargar(); // refrescar al cerrar el diálogo de alta
        }

        // Abre diálogo de edición para el elemento seleccionado
        private void Modificar()
        {
            var sel = _vista.ObtenerPuestoSeleccionado();
            if (sel == null) { _vista.MostrarMensaje("Seleccioná un puesto."); return; }

            _vista.AbrirVentanaModificarPuesto(sel);
            Cargar(); // refrescar al cerrar el diálogo de edición
        }

        // Alterna estado Activo del puesto seleccionado
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

        // Eliminación del puesto seleccionado
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

        // Solicita a la vista la navegación correspondiente
        private void VolverMenu()
        {
            _vista.NavegarAMenu();  // la vista se encarga del cierre y retorno
        }
    }
}
