using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Presenters.Prompts_PopUps;
using ProdLogApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ProdLogApp.Views.Designs.Prompts
{
    public partial class PromptProduct : Window, IPromptProductView
    {
        private readonly PromptProductPresenter _presenter;
        private readonly User _activeUser;
        private readonly IDatabaseService _databaseService;
        private GridViewColumnHeader _lastHeaderClicked = null;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;

        public PromptProduct(User activeUser, IDatabaseService databaseService)
        {
            InitializeComponent();
            _activeUser = activeUser;
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));

            _presenter = new PromptProductPresenter(this, _databaseService);

            _presenter.CargarProductos();

            // Hook de eventos para el filtrado en vivo
            NombreTextBox.TextChanged += SearchBox_TextChanged;
            SearchBoxID.TextChanged += SearchBox_TextChanged;
            CategoriaTextBox.TextChanged += SearchBox_TextChanged;
        }

        public void MostrarProductos(List<Producto> productos)
        {
            ProductList.ItemsSource = productos ?? new List<Producto>();
        }

        public Producto ObtenerProductoSeleccionado()
        {
            return ProductList.SelectedItem as Producto;
        }

        public void MostrarMensaje(string mensaje)
        {
            MessageBox.Show(mensaje, "Información", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _presenter.FiltrarProductos(NombreTextBox.Text, SearchBoxID.Text, CategoriaTextBox.Text);
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            var headerClicked = sender as GridViewColumnHeader;
            var sortBy = headerClicked?.Tag?.ToString();
            if (string.IsNullOrEmpty(sortBy)) return;

            ListSortDirection direction = headerClicked != _lastHeaderClicked
                ? ListSortDirection.Ascending
                : (_lastDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending);

            Sort(sortBy, direction);
            _lastHeaderClicked = headerClicked;
            _lastDirection = direction;
        }

        private void Sort(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView = CollectionViewSource.GetDefaultView(ProductList.ItemsSource);
            if (dataView == null) return;

            dataView.SortDescriptions.Clear();
            dataView.SortDescriptions.Add(new SortDescription(sortBy, direction));
            dataView.Refresh();
        }


        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var productoSeleccionado = ObtenerProductoSeleccionado();

            if (productoSeleccionado == null)
            {
                MostrarMensaje("Por favor, seleccioná un producto antes de confirmar.");
                return;
            }

            DialogResult = true; 
            Close(); 
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
        private void Product_list_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Evita disparar si el doble-click fue en un hueco de la lista
            var clicked = e.OriginalSource as DependencyObject;
            var container = ItemsControl.ContainerFromElement(ProductList, clicked) as ListViewItem;
            if (container == null) return;

            ConfirmButton_Click(sender, new RoutedEventArgs());
        }

        // Enter = confirmar
        private void Product_list_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && ProductList?.SelectedItem is Categoria)
            {
                ConfirmButton_Click(sender, new RoutedEventArgs());
                e.Handled = true;
            }
        }
    }
}
