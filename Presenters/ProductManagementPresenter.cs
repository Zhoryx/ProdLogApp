using System;
using System.Windows;
using ProdLogApp.Views;
using ProdLogApp.Views.Interfaces;
using ProdLogApp.Services;

namespace ProdLogApp.Presenters
{
    public class ProductManagementPresenter
    {
        private readonly IProductManagementView _view;
        private readonly IDatabaseService _databaseService;

        public ProductManagementPresenter(IProductManagementView view, IDatabaseService databaseService)
        {
            _view = view;
            _databaseService = databaseService;

            // Subscribe events from the view
            _view.OnDeleteProduct += DeleteProduct;
            _view.OnModifyProduct += ModifyProduct;
            _view.OnReturn += ReturnToMenu;
        }

        // Método para abrir la ventana de agregar producto
        public void AbrirVentanaAgregarProducto()
        {
            ProductoAgregar ventanaAgregar = new ProductoAgregar(_databaseService);
            ventanaAgregar.ShowDialog(); // Se abre como modal
        }

        // Manejo de eliminación de producto
        private void DeleteProduct() => MessageBox.Show("Eliminar producto...");

        // Manejo de modificación de producto
        private void ModifyProduct() => MessageBox.Show("Modificar producto...");

        // Navega de regreso al menú
        private void ReturnToMenu() => _view.NavigateToMenu();
    }
}
