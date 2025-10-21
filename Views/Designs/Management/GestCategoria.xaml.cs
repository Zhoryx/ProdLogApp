using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class GestCategoria : Window, IGestCategoriasVista
    {
        private readonly GestCategoriasPresenter _presenter;
        private readonly IServicioCategorias _svcCategorias;
        private readonly Usuario _usuarioActivo;

        public event Action OnAgregar;
        public event Action OnModificar;
        public event Action OnAlternarEstado;
        public event Action OnEliminar;
        public event Action OnVolverMenu;

        private GridViewColumnHeader _lastHeaderClicked = null;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;

        public GestCategoria(Usuario activeUser)
        {
            InitializeComponent();
            _usuarioActivo = activeUser ?? throw new ArgumentNullException(nameof(activeUser));

            var proveedor = new ProveedorConexionMySql("ProdLogDb");
            _svcCategorias = new ServicioCategoriasMySql(proveedor);

            _presenter = new GestCategoriasPresenter(this, _svcCategorias);
        }

        public void MostrarMensaje(string mensaje)
            => MessageBox.Show(mensaje, "Información", MessageBoxButton.OK, MessageBoxImage.Information);

        public void MostrarCategorias(List<Categoria> categorias)
            => CategoryList.ItemsSource = categorias ?? new List<Categoria>();

        public Categoria ObtenerCategoriaSeleccionada()
            => CategoryList.SelectedItem as Categoria;

        public void AbrirVentanaAgregarCategoria()
        {
            var win = new AgregarCategoria
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            win.ShowDialog();
        }

        public void AbrirVentanaModificarCategoria(Categoria categoria)
        {
            if (categoria == null) return;

            var win = new AgregarCategoria(categoria)
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            win.ShowDialog();
        }

        public void NavegarAMenu()
        {
            // No crear otra instancia del menú: reactivar el Owner si existe y cerrar
            Owner?.Activate();
            Close();
        }

        private void CategoryList_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

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
            var dataView = CollectionViewSource.GetDefaultView(CategoryList.ItemsSource);
            if (dataView == null) return;

            dataView.SortDescriptions.Clear();
            dataView.SortDescriptions.Add(new SortDescription(sortBy, direction));
            dataView.Refresh();
        }

        private void Categories_list_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var clicked = e.OriginalSource as DependencyObject;
            var container = ItemsControl.ContainerFromElement(CategoryList, clicked) as ListViewItem;
            if (container == null) return;

            if (CategoryList?.SelectedItem is Categoria) OnModificar?.Invoke();
        }

        private void Categories_list_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter || e.Key == Key.F2) && CategoryList?.SelectedItem is Categoria)
            {
                OnModificar?.Invoke();
                e.Handled = true;
            }
        }

        private void ToggleCategoryStatus_Click(object sender, RoutedEventArgs e) => OnAlternarEstado?.Invoke();
        private void AddCategory_Click(object sender, RoutedEventArgs e) => OnAgregar?.Invoke();
        private void ModifyCategory_Click(object sender, RoutedEventArgs e) => OnModificar?.Invoke();
        private void ReturnToMenu_Click(object sender, RoutedEventArgs e) => OnVolverMenu?.Invoke();
    }
}
