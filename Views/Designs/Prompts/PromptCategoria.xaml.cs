using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Servicios;
using ProdLogApp.Presenters;

namespace ProdLogApp.Views.Designs.Prompts
{
    public partial class PromptCategoria : Window, IPromptCategoriaVista
    {
        // Eventos interfaz
        public event Action OnConfirmarSeleccion;
        public event Action OnCancelar;

        private readonly PromptCategoriasPresenter _presenter;
        private Categoria _categoriaSeleccionada;

        // 👉 Propiedades que necesitas leer desde afuera
        public int CategoriaSeleccionadaId { get; private set; }
        public string CategoriaSeleccionadaNombre { get; private set; }

        private GridViewColumnHeader _lastHeaderClicked = null;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;

        public PromptCategoria(IServicioCategorias servicioCategorias)
        {
            InitializeComponent();

            _presenter = new PromptCategoriasPresenter(this, servicioCategorias);

            Loaded += async (_, __) => await _presenter.CargarCategoriasAsync();

            SearchBox.TextChanged += (s, e) => _presenter.FiltrarCategorias(SearchBox.Text, SearchBoxID.Text);
            SearchBoxID.TextChanged += (s, e) => _presenter.FiltrarCategorias(SearchBox.Text, SearchBoxID.Text);
        }

        // ==== Implementación vista ====
        public void CargarCategorias(List<Categoria> categorias)
        {
            CategoryList.ItemsSource = categorias ?? new List<Categoria>();
        }

        public Categoria ObtenerCategoriaSeleccionada() => CategoryList?.SelectedItem as Categoria;

        public void MostrarMensaje(string mensaje) =>
            MessageBox.Show(mensaje, "Información", MessageBoxButton.OK, MessageBoxImage.Information);

        public void Cerrar() => Close();

        // ==== Handlers UI (sin tocar tu diseño) ====
        private void CategoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _categoriaSeleccionada = CategoryList.SelectedItem as Categoria;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (_categoriaSeleccionada == null)
            {
                MostrarMensaje("Seleccioná una categoría antes de confirmar.");
                return;
            }

            // 👉 llenar las propiedades públicas antes de cerrar
            CategoriaSeleccionadaId = _categoriaSeleccionada.CategoriaId;
            CategoriaSeleccionadaNombre = _categoriaSeleccionada.Nombre;

            OnConfirmarSeleccion?.Invoke();
            DialogResult = true;   // para que ShowDialog() devuelva true
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            OnCancelar?.Invoke();
            DialogResult = false;
            Close();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _presenter.FiltrarCategorias(SearchBox.Text, SearchBoxID.Text);
        }

        private void SearchBoxID_TextChanged(object sender, TextChangedEventArgs e)
        {
            _presenter.FiltrarCategorias(SearchBox.Text, SearchBoxID.Text);
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            var headerClicked = sender as GridViewColumnHeader;
            var sortBy = headerClicked?.Tag?.ToString();
            if (string.IsNullOrEmpty(sortBy)) return;

            var direction = headerClicked != _lastHeaderClicked
                ? ListSortDirection.Ascending
                : (_lastDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending);

            Sort(sortBy, direction);
            _lastHeaderClicked = headerClicked;
            _lastDirection = direction;

            _presenter.OrdenarPor(sortBy, direction == ListSortDirection.Descending);
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
            if (container == null || _categoriaSeleccionada == null) return;

            // Doble click confirma igual que el botón
            ConfirmButton_Click(sender, new RoutedEventArgs());
        }

        private void Categories_list_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && CategoryList?.SelectedItem is Categoria)
            {
                ConfirmButton_Click(sender, new RoutedEventArgs());
                e.Handled = true;
            }
        }
    }
}
