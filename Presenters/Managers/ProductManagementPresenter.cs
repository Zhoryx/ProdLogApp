using ProdLogApp.Models;
using ProdLogApp.Services;
using ProdLogApp.Views;
using ProdLogApp.Views;
using ProdLogApp.Interfaces;
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
            _view.OnAddProduct += AgregarProducto;
            _view.OnModifyProduct += ModifyProduct;
            _view.OnReturn += ReturnToMenu;
        }

        // Manejo de agregar producto
        public void AgregarProducto()
        {
            _view.Addview(); 
            CargarProductos();
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

            _view.Modifyview(productoSeleccionado); 
            CargarProductos(); 
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

            _databaseService.ToggleProductState(productoSeleccionado.Id, productoSeleccionado.Activo); 
            CargarProductos();

            _view.MostrarMensaje(productoSeleccionado.Activo ? "Puesto activado correctamente." : "Puesto desactivado correctamente.");
            
        }


        // Navega de regreso al menú
        private void ReturnToMenu() => _view.NavigateToMenu();
    }
}
