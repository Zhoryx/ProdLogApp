using System;
using System.Linq;                   // <- necesario para .ToList()
using ProdLogApp.Interfaces;
using ProdLogApp.Servicios;

namespace ProdLogApp.Presenters
{
    // Presenter de gestión de productos.
    // Maneja ABM, alternancia de estado y refresca el listado según acciones.
    public sealed class GestProductosPresenter
    {
        private readonly IGestProductosVista _vista;
        private readonly IServicioProductos _svc;

        public GestProductosPresenter(IGestProductosVista vista, IServicioProductos svc)
        {
            _vista = vista ?? throw new ArgumentNullException(nameof(vista));
            _svc = svc ?? throw new ArgumentNullException(nameof(svc));

            // Suscripciones a eventos
            _vista.OnAgregar += Agregar;
            _vista.OnModificar += Modificar;
            _vista.OnAlternarEstado += Alternar;
            _vista.OnEliminar += Eliminar;
            _vista.OnVolverMenu += _vista.NavegarAMenu;   // navegación directa expuesta por la vista

            // Carga inicial
            Cargar();
        }

        // Carga/recarga de productos
        private async void Cargar()
        {
            try
            {
                var lista = await _svc.ListarAsync();
                _vista.MostrarProductos(lista.ToList());
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"Error al cargar productos: {ex.Message}");
            }
        }

        // Alta de producto y refresco de listado
        private void Agregar()
        {
            _vista.AbrirVentanaAgregarProducto();
            Cargar(); // refresca luego de cerrar el diálogo
        }

        // Edición del producto seleccionado
        private void Modificar()
        {
            var sel = _vista.ObtenerProductoSeleccionado();
            if (sel == null)
            {
                _vista.MostrarMensaje("Seleccioná un producto.");
                return;
            }

            _vista.AbrirVentanaModificarProducto(sel);
            Cargar(); // refresca luego de cerrar el diálogo
        }

        // Alterna estado Activo del producto seleccionado
        private async void Alternar()
        {
            var sel = _vista.ObtenerProductoSeleccionado();
            if (sel == null)
            {
                _vista.MostrarMensaje("Seleccioná un producto.");
                return;
            }

            try
            {
                await _svc.AlternarEstadoAsync(sel.Id, !sel.Activo);
                Cargar();
                _vista.MostrarMensaje(sel.Activo ? "Producto desactivado." : "Producto activado.");
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"Error al alternar estado: {ex.Message}");
            }
        }

        // Eliminación del producto seleccionado
        private async void Eliminar()
        {
            var sel = _vista.ObtenerProductoSeleccionado();
            if (sel == null)
            {
                _vista.MostrarMensaje("Seleccioná un producto.");
                return;
            }

            try
            {
                await _svc.EliminarAsync(sel.Id);
                Cargar();
                _vista.MostrarMensaje("Producto eliminado.");
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"Error al eliminar: {ex.Message}");
            }
        }
    }
}
