using System;
using System.Windows;
using ProdLogApp.Models;
using ProdLogApp.Presenters;
using ProdLogApp.Views.Interfaces;

namespace ProdLogApp.Views
{
    public partial class ProductManagement : Window, IProductManagementView
    {
        private readonly ProductManagementPresenter _presenter;
        private readonly User _activeUser;

        public event Action OnAddProduct;
        public event Action OnDeleteProduct;
        public event Action OnModifyProduct;
        public event Action OnReturn;

        public ProductManagement(User activeUser)
        {
            InitializeComponent();
            _activeUser = activeUser;
            _presenter = new ProductManagementPresenter(this);
        }

        // Method to navigate back to the main menu
        public void NavigateToMenu()
        {
            ManagerMenu menu = new ManagerMenu(_activeUser);
            menu.Show();
            this.Close();
        }

        // Event handlers for product management actions
        private void AddProduct(object sender, RoutedEventArgs e) => OnAddProduct?.Invoke();
        private void DeleteProduct(object sender, RoutedEventArgs e) => OnDeleteProduct?.Invoke();
        private void ModifyProduct(object sender, RoutedEventArgs e) => OnModifyProduct?.Invoke();
        private void ReturnToMenu(object sender, RoutedEventArgs e) => OnReturn?.Invoke();
    }
}
