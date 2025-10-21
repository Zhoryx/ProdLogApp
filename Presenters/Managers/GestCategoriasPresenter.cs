using System;
using System.Linq;
using ProdLogApp.Interfaces;
using ProdLogApp.Servicios;

namespace ProdLogApp.Presenters
{
    public sealed class GestCategoriasPresenter
    {
        private readonly IGestCategoriasVista _vista;
        private readonly IServicioCategorias _svc;

        public GestCategoriasPresenter(IGestCategoriasVista vista, IServicioCategorias svc)
        {
            _vista = vista;
            _svc = svc;

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
                var lista = await _svc.ListarAsync();
                _vista.MostrarCategorias(lista.ToList());
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"Error al cargar categorías: {ex.Message}");
            }
        }

        private void Agregar()
        {
            _vista.AbrirVentanaAgregarCategoria();
            Cargar(); // refresca siempre tras cerrar el diálogo
        }

        private void Modificar()
        {
            var sel = _vista.ObtenerCategoriaSeleccionada();
            if (sel == null) { _vista.MostrarMensaje("Seleccioná una categoría."); return; }

            _vista.AbrirVentanaModificarCategoria(sel);
            Cargar(); // refresca al volver del diálogo
        }

        private async void Alternar()
        {
            var sel = _vista.ObtenerCategoriaSeleccionada();
            if (sel == null) { _vista.MostrarMensaje("Seleccioná una categoría."); return; }

            try
            {
                await _svc.AlternarEstadoAsync(sel.CategoriaId, !sel.Activo);
                Cargar();
                // Mensajes de éxito opcionales: comentados para no molestar
                // _vista.MostrarMensaje(sel.Activo ? "Categoría desactivada." : "Categoría activada.");
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"Error al alternar estado: {ex.Message}");
            }
        }

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

        private void VolverMenu()
        {
            // El presenter no cierra ventanas; delega en la vista
            _vista.NavegarAMenu();
        }
    }
}
