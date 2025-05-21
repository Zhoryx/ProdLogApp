using System;
using System.Windows;
using ProdLogApp.Models;
using ProdLogApp.Presenters;
using ProdLogApp.Views.Interfaces;

namespace ProdLogApp.Views
{
    public partial class ProductManagement : Window, IProductManagementView
    {
        private ProductManagementPresenter _presenter;
        private readonly User _activeUser;
        public event Action OnAgregarProducto;
        public event Action OnEliminarProducto;
        public event Action OnModificarProducto;
        public event Action OnVolver;

        public ProductManagement(User activeUser)
        {
            InitializeComponent();
            _activeUser = activeUser;
            _presenter = new ProductManagementPresenter(this);
        }
        public void NavegarAMenu()
        {
            ManagerMenu menu = new ManagerMenu(_activeUser);
            menu.Show();
            this.Close();
        }
        private void AgregarProducto(object sender, RoutedEventArgs e) => OnAgregarProducto?.Invoke();
        private void EliminarProducto(object sender, RoutedEventArgs e) => OnEliminarProducto?.Invoke();
        private void ModificarProducto(object sender, RoutedEventArgs e) => OnModificarProducto?.Invoke();
        private void Volver(object sender, RoutedEventArgs e) => OnVolver?.Invoke();

       
    }
}
