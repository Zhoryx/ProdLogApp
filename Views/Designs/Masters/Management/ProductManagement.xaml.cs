using ProdLogApp.Models;
using ProdLogApp.Presenters;
using ProdLogApp.Services;
using ProdLogApp.Views.Interfaces;
using System;
using System.Windows;

namespace ProdLogApp.Views
{
    public partial class ProductManagement : Window, IProductManagementView
    {
        private readonly ProductManagementPresenter _presenter;
        private readonly User _activeUser;
        private readonly IDatabaseService _databaseService;

        public event Action OnDeleteProduct;
        public event Action OnModifyProduct;
        public event Action OnReturn;

        public ProductManagement(User activeUser, IDatabaseService databaseService)
        {
            InitializeComponent();

            _activeUser = activeUser;
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService)); // Validación
            _presenter = new ProductManagementPresenter(this, _databaseService);
        }


        // Método para navegar al menú principal
        public void NavigateToMenu()
        {
            ManagerMenu menu = new ManagerMenu(_activeUser);
            menu.Show();
            this.Close();
        }

        // Eventos para gestión de productos
        private void DeleteProduct(object sender, RoutedEventArgs e) => OnDeleteProduct?.Invoke();
        private void ModifyProduct(object sender, RoutedEventArgs e) => OnModifyProduct?.Invoke();
        private void ReturnToMenu(object sender, RoutedEventArgs e) => OnReturn?.Invoke();

        // Método para abrir la ventana de agregar producto
        private void AddProduct(object sender, RoutedEventArgs e)
        {
            AbrirVentanaAgregarProducto();
        }

        private void AbrirVentanaAgregarProducto()
        {
            ProductoAgregar ventanaAgregar = new ProductoAgregar(_databaseService);
            ventanaAgregar.ShowDialog(); // Abre la ventana como modal
        }
    }
}
