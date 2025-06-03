using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ProdLogApp.Models;
using ProdLogApp.Services;

namespace ProdLogApp.Views.Designs.Prompts
{
    public partial class PromptCategory : Window, IPromptCategoryView
    {
        private readonly PromptCategoryPresenter _presenter;
        private Categoria _categoriaSeleccionada;

        public PromptCategory(IDatabaseService databaseService)
        {
            InitializeComponent();
            _presenter = new PromptCategoryPresenter(this, databaseService);

            SearchBox.TextChanged += (s, e) => _presenter.FiltrarCategorias(SearchBox.Text, SearchBoxID.Text);
        }

        public void MostrarCategorias(List<Categoria> categorias)
        {
            ProductList.ItemsSource = categorias;
        }

        public void ObtenerSeleccion(out int categoriaId, out string descripcion)
        {
            categoriaId = _categoriaSeleccionada?.Id ?? 0;
            descripcion = _categoriaSeleccionada?.Nombre ?? "Sin selección";
        }



        private void ProductList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductList.SelectedItem is Categoria seleccion)
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

    }
}
