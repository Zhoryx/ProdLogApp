using System;
using System.Windows;
using ProdLogApp.Views;

namespace ProdLogApp.Presenters
{
    public class ProductManagementPresenter
    {
        private readonly IProductManagementView _view;

        public ProductManagementPresenter(IProductManagementView view)
        {
            _view = view;

            // Subscribe events from the view
            _view.OnAddProduct += AddProduct;
            _view.OnDeleteProduct += DeleteProduct;
            _view.OnModifyProduct += ModifyProduct;
            _view.OnReturn += ReturnToMenu;
        }

        // Handles adding a new product
        private void AddProduct() => MessageBox.Show("Add product...");

        // Handles deleting a product
        private void DeleteProduct() => MessageBox.Show("Delete product...");

        // Handles modifying an existing product
        private void ModifyProduct() => MessageBox.Show("Modify product...");

        // Navigates back to the main menu
        private void ReturnToMenu() => _view.NavigateToMenu();
    }
}
