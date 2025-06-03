using ProdLogApp.Models;
using ProdLogApp.Services;
using ProdLogApp.Views;
using ProdLogApp.Views.Interfaces;
using System;

namespace ProdLogApp.Presenters
{
    public class ProductManagementPresenter
    {
        private readonly IProductManagementView _view;
        private readonly IDatabaseService _databaseService;

        public ProductManagementPresenter(IProductManagementView view, IDatabaseService databaseService)
        {
            _view = view;
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));

            _view.OnModifyProduct += ModifyProduct;
            _view.OnReturn += ReturnToMenu;
        }

        // Manejo de agregar producto
        public void AgregarProducto()
        {
            _view.Addview(); // ✅ Usa el método de la Vista para abrir la ventana
            CargarProductos(); // ✅ Refresca la lista tras agregar
        }

        // Manejo de modificación de producto
        private void ModifyProduct()
        {
            Producto productoSeleccionado = _view.ObtenerProductoSeleccionado();

            if (productoSeleccionado == null)
            {
                _view.MostrarMensaje("Seleccione un producto para modificar.");
                return;
            }

            _view.Modifyview(productoSeleccionado); // ✅ Usa el método de la Vista para abrir la ventana
            CargarProductos(); // ✅ Refresca la lista tras modificar
        }

        // Cargar la lista actualizada de productos desde la BD
        public void CargarProductos()
        {
            var productos = _databaseService.ObtenerTodosLosProductos();
            _view.MostrarProductos(productos);
        }

        public void ToggleProductState()
        {
            Producto productoSeleccionado = _view.ObtenerProductoSeleccionado();

            if (productoSeleccionado == null)
            {
                _view.MostrarMensaje("Seleccione un producto para cambiar su estado.");
                return;
            }

            _databaseService.ToggleProductState(productoSeleccionado.Id, productoSeleccionado.Activo); // ✅ Alterna el estado en la BD
            CargarProductos(); // ✅ Refresca la lista

            string mensaje = productoSeleccionado.Activo ? "Producto desactivado correctamente." : "Producto activado correctamente.";
            _view.MostrarMensaje(mensaje);
        }


        // Navega de regreso al menú
        private void ReturnToMenu() => _view.NavigateToMenu();
    }
}
