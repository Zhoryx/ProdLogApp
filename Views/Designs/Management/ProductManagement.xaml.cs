using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Presenters;
using ProdLogApp.Services;
using ProdLogApp.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ProdLogApp.Views
{
    public partial class ProductManagement : Window, IProductManagementView
    {
        private readonly ProductManagementPresenter _presenter;
        private readonly User _activeUser;
        private readonly IDatabaseService _databaseService;
        private GridViewColumnHeader _lastHeaderClicked = null;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;

        public event Action OnModifyProduct;
        public event Action OnReturn;
        public event Action OnAddProduct;

        public ProductManagement(User activeUser, IDatabaseService databaseService)
        {
            InitializeComponent();
            _activeUser = activeUser;
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));

            _presenter = new ProductManagementPresenter(this, _databaseService);

            _presenter.CargarProductos();
        } 

        // Método para navegar al menú principal
        public void NavigateToMenu()
        {
            ManagerMenu menu = new ManagerMenu(_activeUser);
            menu.Show();
            Close();
        }

        private void Products_list_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var clicked = e.OriginalSource as DependencyObject;
            var container = ItemsControl.ContainerFromElement(Products_list, clicked) as ListViewItem;
            if (container == null) return; // doble click en área vacía

            if (Products_list?.SelectedItem is Producto)
                OnModifyProduct?.Invoke();
        }

        private void Products_list_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter || e.Key == Key.F2) && Products_list?.SelectedItem is Producto)
            {
                OnModifyProduct?.Invoke();
                e.Handled = true;
            }
        }

        // Si tenés botón Modificar, que llame al mismo evento:
        private void ModifyProduct_Click(object sender, RoutedEventArgs e) => OnModifyProduct?.Invoke();

        // Métodos de la vista (MVP)
        public Producto SelectedProduct() => Products_list?.SelectedItem as Producto;
        public void ShowProducts(List<Producto> items) => Products_list.ItemsSource = items ?? new List<Producto>();
        public bool NewProduct()
        {
            var dlg = new AddProduct(_databaseService) { Owner = this };
            return dlg.ShowDialog() == true;
        }
        public void ModifyProduct(Producto p)
        {
            var dlg = new AddProduct(_databaseService, p) { Owner = this };
            dlg.ShowDialog();
        }
    

        public void Addview()
        {
            AddProduct ventanaAgregar = new AddProduct(_databaseService);
            ventanaAgregar.ShowDialog();
        }

        public void Modifyview(Producto producto)
        {
            AddProduct ventanaModificar = new AddProduct(_databaseService, producto);
            ventanaModificar.ShowDialog();
        }
        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            _presenter.AgregarProducto(); 
        }
        private void ToggleProductState(object sender, RoutedEventArgs e)
        {
            _presenter.ToggleProductState();
        }


        

        // Método para volver al menú desde el botón en XAML
        private void ReturnToMenu_Click(object sender, RoutedEventArgs e) => OnReturn?.Invoke();

        // Método para mostrar productos en la lista
        public void MostrarProductos(List<Producto> productos)
        {
            Products_list.ItemsSource = productos ?? new List<Producto>();
        }

        // Método para mostrar mensajes en la UI
        public void MostrarMensaje(string mensaje)
        {
            MessageBox.Show(mensaje, "Información", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Obtener el producto seleccionado en la lista
        public Producto ObtenerProductoSeleccionado()
        {
            return Products_list.SelectedItem as Producto;
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            var headerClicked = sender as GridViewColumnHeader;
            var sortBy = headerClicked.Tag.ToString();

            ListSortDirection direction;

            if (headerClicked != _lastHeaderClicked)
            {
                direction = ListSortDirection.Ascending;
            }
            else
            {
                direction = _lastDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
            }

            Sort(sortBy, direction);

            _lastHeaderClicked = headerClicked;
            _lastDirection = direction;
        }
        private void Sort(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView = CollectionViewSource.GetDefaultView(Products_list.ItemsSource);

            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }
    }
}
