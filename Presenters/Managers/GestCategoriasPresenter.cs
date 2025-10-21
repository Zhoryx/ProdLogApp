using System;
using System.Linq;
using ProdLogApp.Interfaces;
using ProdLogApp.Servicios;

namespace ProdLogApp.Presenters
{
    // Presenter de gestión de categorías.
    // Coordina operaciones de ABM y mantiene actualizado el listado en pantalla.
    public sealed class GestCategoriasPresenter
    {
        private readonly IGestCategoriasVista _vista;
        private readonly IServicioCategorias _svc;

        public GestCategoriasPresenter(IGestCategoriasVista vista, IServicioCategorias svc)
        {
            _vista = vista ?? throw new ArgumentNullException(nameof(vista));
            _svc = svc ?? throw new ArgumentNullException(nameof(svc));

            // Suscripción a eventos de la vista
            _vista.OnAgregar += Agregar;
            _vista.OnModificar += Modificar;
            _vista.OnAlternarEstado += Alternar;
            _vista.OnEliminar += Eliminar;
            _vista.OnVolverMenu += VolverMenu;

            // Carga inicial
            Cargar();
        }

        // Carga/recarga del listado de categorías
        private async void Cargar()
        {
            try
            {
                var lista = await _svc.ListarAsync();
                _vista.MostrarCategorias(lista.ToList());
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"Error al cargar categorías: {ex.Message}");
            }
        }

        // Abre diálogo de alta y refresca al cerrar
        private void Agregar()
        {
            _vista.AbrirVentanaAgregarCategoria();
            Cargar(); // refresca siempre tras cerrar el diálogo
        }

        // Abre diálogo de edición para la categoría seleccionada
        private void Modificar()
        {
            var sel = _vista.ObtenerCategoriaSeleccionada();
            if (sel == null) { _vista.MostrarMensaje("Seleccioná una categoría."); return; }

            _vista.AbrirVentanaModificarCategoria(sel);
            Cargar(); // refresca al volver del diálogo
        }

        // Alterna estado Activo de la categoría seleccionada
        private async void Alternar()
        {
            var sel = _vista.ObtenerCategoriaSeleccionada();
            if (sel == null) { _vista.MostrarMensaje("Seleccioná una categoría."); return; }

            try
            {
                await _svc.AlternarEstadoAsync(sel.CategoriaId, !sel.Activo);
                Cargar();
                // Mensaje de éxito opcional:
                // _vista.MostrarMensaje(sel.Activo ? "Categoría desactivada." : "Categoría activada.");
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"Error al alternar estado: {ex.Message}");
            }
        }

        // Eliminación de la categoría seleccionada (o inactivación según política)
        private async void Eliminar()
        {
            var sel = _vista.ObtenerCategoriaSeleccionada();
            if (sel == null) { _vista.MostrarMensaje("Seleccioná una categoría."); return; }

            try
            {
                await _svc.EliminarAsync(sel.CategoriaId);
                Cargar();
                // _vista.MostrarMensaje("Categoría eliminada."); // opcional
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"Error al eliminar: {ex.Message}");
            }
        }

        // Solicita a la vista la navegación correspondiente
        private void VolverMenu()
        {
            _vista.NavegarAMenu();
        }
    }
}
