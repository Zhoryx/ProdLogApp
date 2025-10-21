using System;
using System.Linq;                   // <- Necesario para .ToList()
using ProdLogApp.Interfaces;
using ProdLogApp.Servicios;

namespace ProdLogApp.Presenters
{
    public sealed class GestProductosPresenter
    {
        private readonly IGestProductosVista _vista;
        private readonly IServicioProductos _svc;

        public GestProductosPresenter(IGestProductosVista vista, IServicioProductos svc)
        {
            _vista = vista ?? throw new ArgumentNullException(nameof(vista));
            _svc = svc ?? throw new ArgumentNullException(nameof(svc));

            _vista.OnAgregar += Agregar;
            _vista.OnModificar += Modificar;
            _vista.OnAlternarEstado += Alternar;
            _vista.OnEliminar += Eliminar;
            _vista.OnVolverMenu += _vista.NavegarAMenu;   // <- ahora vuelve al menú

            Cargar();
        }

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

        private void Agregar()
        {
            _vista.AbrirVentanaAgregarProducto();
            Cargar(); // <- refresca luego de cerrar el diálogo
        }

        private void Modificar()
        {
            var sel = _vista.ObtenerProductoSeleccionado();
            if (sel == null)
            {
                _vista.MostrarMensaje("Seleccioná un producto.");
                return;
            }

            _vista.AbrirVentanaModificarProducto(sel);
            Cargar(); // <- refresca luego de cerrar el diálogo
        }

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
