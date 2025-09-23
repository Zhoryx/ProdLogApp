using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Presenters;
using ProdLogApp.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ProdLogApp.Views.Designs.Prompts
{
    public partial class PromptCategory : Window, IPromptCategoryView
    {
        private readonly PromptCategoryPresenter _presenter;
        private Categoria _categoriaSeleccionada;
        private GridViewColumnHeader _lastHeaderClicked = null;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;
        public PromptCategory(IDatabaseService databaseService)
        {
            InitializeComponent();
            _presenter = new PromptCategoryPresenter(this, databaseService);

            SearchBox.TextChanged += (s, e) => _presenter.FiltrarCategorias(SearchBox.Text, SearchBoxID.Text);
        }

        public void MostrarCategorias(List<Categoria> categorias)
        {
            CategoryList.ItemsSource = categorias;
        }

        public void ObtenerSeleccion(out int categoriaId, out string descripcion)
        {
            categoriaId = _categoriaSeleccionada?.CategoryId ?? 0;
            descripcion = _categoriaSeleccionada?.Nombre ?? "Sin selección";
        }

       

        private void CategoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryList.SelectedItem is Categoria seleccion)
            {
                _categoriaSeleccionada = seleccion;
                 
            }
        }


        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            ObtenerSeleccion(out int id, out string descripcion);
            DialogResult = true; 
            Close();
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
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
            ICollectionView dataView = CollectionViewSource.GetDefaultView(CategoryList.ItemsSource);
            if (dataView == null) return;

            dataView.SortDescriptions.Clear();
            dataView.SortDescriptions.Add(new SortDescription(sortBy, direction));
            dataView.Refresh();
        }

        private void Categories_list_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Evita disparar si el doble-click fue en un hueco de la lista
            var clicked = e.OriginalSource as DependencyObject;
            var container = ItemsControl.ContainerFromElement(CategoryList, clicked) as ListViewItem;
            if (container == null) return;

            ConfirmButton_Click(sender, new RoutedEventArgs());
        }

        // Enter = confirmar
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
