using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Presenters.Prompts_PopUps;
using ProdLogApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ProdLogApp.Views.Designs.Prompts
{
    public partial class PromptProduct : Window, IPromptProductView
    {
        private readonly PromptProductPresenter _presenter;
        private readonly User _activeUser;
        private readonly IDatabaseService _databaseService;
        private List<Producto> _todosLosProductos;
        private GridViewColumnHeader _lastHeaderClicked = null;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;

        public PromptProduct(User activeUser, IDatabaseService databaseService)
        {
            InitializeComponent();
            _activeUser = activeUser;
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));

            _presenter = new PromptProductPresenter(this, _databaseService);

            _presenter.CargarProductos();
            _presenter.CargarCategorias();

            // Hook de eventos para el filtrado en vivo
            NombreTextBox.TextChanged += (s, e) => AplicarFiltros();
           
        }

        public void MostrarProductos(List<Producto> productos)
        {
            _todosLosProductos = productos ?? new List<Producto>();
            ProductList.ItemsSource = _todosLosProductos;
        }

        public Producto ObtenerProductoSeleccionado()
        {
            return ProductList.SelectedItem as Producto;
        }

        public void MostrarMensaje(string mensaje)
        {
            MessageBox.Show(mensaje, "Información", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void MostrarCategorias(List<Categoria> categorias)
        {
            if (categorias == null) return;

            // Opción "Todos"
            categorias.Insert(0, new Categoria { CategoryId = 0, Nombre = "Todos" });
        }

        private void AplicarFiltros()
        {
            if (_todosLosProductos == null) return;

            string texto = NombreTextBox.Text.Trim().ToLower();

            var filtrados = _todosLosProductos
                .Where(p =>
                    (string.IsNullOrWhiteSpace(texto) || p.Nombre.ToLower().Contains(texto)));

            ProductList.ItemsSource = filtrados;
        }
        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            var headerClicked = sender as GridViewColumnHeader;
            var sortBy = headerClicked?.Tag?.ToString();
            if (string.IsNullOrEmpty(sortBy)) return;

            ListSortDirection direction;

            if (headerClicked != _lastHeaderClicked)
            {
                direction = ListSortDirection.Ascending;
            }
            else
            {
                direction = _lastDirection == ListSortDirection.Ascending
                    ? ListSortDirection.Descending
                    : ListSortDirection.Ascending;
            }

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

    }
}
