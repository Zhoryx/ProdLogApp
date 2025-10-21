using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Presenters;
using ProdLogApp.Servicios;

namespace ProdLogApp.Views
{
    public partial class GestProducto : Window, IGestProductosVista
    {
        private readonly GestProductosPresenter _presenter;
        private readonly IServicioProductos _svcProductos;
        private readonly IServicioCategorias _svcCategorias;
        private readonly Usuario _usuarioActivo;

        public event Action OnAgregar;
        public event Action OnModificar;
        public event Action OnAlternarEstado;
        public event Action OnEliminar;
        public event Action OnVolverMenu;

        private GridViewColumnHeader _lastHeaderClicked = null;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;

        public GestProducto(Usuario activeUser)
        {
            InitializeComponent();
            _usuarioActivo = activeUser ?? throw new ArgumentNullException(nameof(activeUser));

            var proveedor = new ProveedorConexionMySql("ProdLogDb");
            _svcProductos = new ServicioProductosMySql(proveedor);
            _svcCategorias = new ServicioCategoriasMySql(proveedor);

            _presenter = new GestProductosPresenter(this, _svcProductos);

            Loaded += async (_, __) => await RefreshListAsync();
        }

        // ===== IGestProductosVista =====
        public void MostrarProductos(List<Producto> productos)
        {
            Products_list.ItemsSource = productos ?? new List<Producto>();
        }

        public Producto ObtenerProductoSeleccionado() => Products_list.SelectedItem as Producto;

        public async void AbrirVentanaAgregarProducto()
        {
            var dlg = new AgregarProducto(_svcProductos, _svcCategorias)
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            dlg.ShowDialog();
            await RefreshListAsync();
        }

        public async void AbrirVentanaModificarProducto(Producto producto)
        {
            if (producto == null) return;

            var dlg = new AgregarProducto(_svcProductos, _svcCategorias, producto)
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            dlg.ShowDialog();
            await RefreshListAsync();
        }

        public void MostrarMensaje(string mensaje)
        {
            MessageBox.Show(mensaje, "Información", MessageBoxButton.OK, MessageBoxImage.Information);
        }

       

        // ===== Handlers UI =====
        private void Products_list_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var clicked = e.OriginalSource as DependencyObject;
            var container = ItemsControl.ContainerFromElement(Products_list, clicked) as ListViewItem;
            if (container == null) return;

            if (Products_list?.SelectedItem is Producto)
                OnModificar?.Invoke();
        }

        private void Products_list_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter || e.Key == Key.F2) && Products_list?.SelectedItem is Producto)
            {
                OnModificar?.Invoke();
                e.Handled = true;
            }
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            var headerClicked = sender as GridViewColumnHeader;
            var sortBy = headerClicked?.Tag?.ToString();
            if (string.IsNullOrWhiteSpace(sortBy)) return;

            var direction = headerClicked != _lastHeaderClicked
                ? ListSortDirection.Ascending
                : (_lastDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending);

            Sort(sortBy, direction);
            _lastHeaderClicked = headerClicked;
            _lastDirection = direction;
        }

        private void Sort(string sortBy, ListSortDirection direction)
        {
            var dataView = CollectionViewSource.GetDefaultView(Products_list.ItemsSource);
            if (dataView == null) return;

            dataView.SortDescriptions.Clear();
            dataView.SortDescriptions.Add(new SortDescription(sortBy, direction));
            dataView.Refresh();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e) => OnAgregar?.Invoke();
        private void ModifyProduct_Click(object sender, RoutedEventArgs e) => OnModificar?.Invoke();
        private void ToggleProductState(object sender, RoutedEventArgs e) => OnAlternarEstado?.Invoke();
        private void ReturnToMenu_Click(object sender, RoutedEventArgs e) => OnVolverMenu?.Invoke();

        // ===== Helpers =====
        private async Task RefreshListAsync()
        {
            try
            {
                var items = await _svcProductos.ListarAsync();
                MostrarProductos(new List<Producto>(items));
            }
            catch (Exception ex)
            {
                MostrarMensaje($"No se pudo actualizar la lista: {ex.Message}");
            }
        }

        public void NavegarAMenu()
        {
            
            Close();
        }

    }
}
