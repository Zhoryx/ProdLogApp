using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ProdLogApp.Interfaces;                 // IPromptProductoVista
using ProdLogApp.Models;                     // Usuario, Producto
using ProdLogApp.Presenters.Prompts_PopUps;  // PromptProductPresenter
using ProdLogApp.Servicios;                  // IServicioProductos, ServicioProductosMySql, ProveedorConexionMySql

namespace ProdLogApp.Views.Designs.Prompts
{
    public partial class PromptProducto : Window, IPromptProductoVista
    {
        private readonly PromptProductPresenter _presenter;
        private readonly Usuario _activeUser;

        private GridViewColumnHeader _lastHeaderClicked = null;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;

        public event Action OnConfirmarSeleccion;
        public event Action OnCancelar;

        public PromptProducto(Usuario activeUser)
        {
            InitializeComponent();
            _activeUser = activeUser ?? throw new ArgumentNullException(nameof(activeUser));

            var proveedor = new ProveedorConexionMySql("ProdLogDb");
            IServicioProductos servicioProductos = new ServicioProductosMySql(proveedor);

            _presenter = new PromptProductPresenter(this, servicioProductos);

            Loaded += async (_, __) => await _presenter.CargarProductosAsync();
        }

        // ===== Implementación de la vista =====
        public void CargarProductos(List<Producto> productos)
        {
            ProductList.ItemsSource = productos ?? new List<Producto>();
        }

        public Producto ObtenerProductoSeleccionado() => ProductList.SelectedItem as Producto;

        public void MostrarMensaje(string mensaje)
            => MessageBox.Show(mensaje, "Información", MessageBoxButton.OK, MessageBoxImage.Information);

        public void Cerrar() => Close();

        // ===== UI Handlers =====
        private void NombreTextBox_TextChanged(object sender, TextChangedEventArgs e)
            => _presenter.FiltrarProductos(NombreTextBox.Text, SearchBoxID.Text, CategoriaTextBox.Text);

        private void SearchBoxID_TextChanged(object sender, TextChangedEventArgs e)
            => _presenter.FiltrarProductos(NombreTextBox.Text, SearchBoxID.Text, CategoriaTextBox.Text);

        private void CategoriaTextBox_TextChanged(object sender, TextChangedEventArgs e)
            => _presenter.FiltrarProductos(NombreTextBox.Text, SearchBoxID.Text, CategoriaTextBox.Text);

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            var headerClicked = sender as GridViewColumnHeader;
            var sortBy = headerClicked?.Tag?.ToString();
            if (string.IsNullOrEmpty(sortBy)) return;

            var direction = headerClicked != _lastHeaderClicked
                ? ListSortDirection.Ascending
                : (_lastDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending);

            var dataView = CollectionViewSource.GetDefaultView(ProductList.ItemsSource);
            if (dataView == null) return;

            dataView.SortDescriptions.Clear();
            dataView.SortDescriptions.Add(new SortDescription(sortBy, direction));
            dataView.Refresh();

            _lastHeaderClicked = headerClicked;
            _lastDirection = direction;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (ObtenerProductoSeleccionado() == null)
            {
                MostrarMensaje("Seleccioná un producto antes de confirmar.");
                return;
            }
            OnConfirmarSeleccion?.Invoke();
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            OnCancelar?.Invoke();
            DialogResult = false;
            Close();
        }

        private void Product_list_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var clicked = e.OriginalSource as DependencyObject;
            var container = ItemsControl.ContainerFromElement(ProductList, clicked) as ListViewItem;
            if (container == null) return;

            ConfirmButton_Click(sender, new RoutedEventArgs());
        }

        private void Product_list_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && ProductList?.SelectedItem is Producto)
            {
                ConfirmButton_Click(sender, new RoutedEventArgs());
                e.Handled = true;
            }
        }
    }
}
