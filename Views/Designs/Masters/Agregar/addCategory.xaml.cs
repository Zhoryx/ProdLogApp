using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Presenters;
using ProdLogApp.Services;
using ProdLogApp.Views.Interfaces;
using System;
using System.Windows;

namespace ProdLogApp.Views
{
    public partial class AddCategory : Window, IAddCategoryView
    {
        private readonly IDatabaseService _databaseService;
        private readonly AddCategoryPresenter _presenter;

        public AddCategory(IDatabaseService databaseService, Categoria categoria = null)
        {
            InitializeComponent();
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
            _presenter = new AddCategoryPresenter(this, _databaseService, categoria);
        }

        public void CargarDatosCategoria(Categoria categoria)
        {
            if (categoria != null)
            {
                NombreCategoriaTextBox.Text = categoria.Nombre;
            }
        }

        public void MostrarMensaje(string mensaje)
        {
            MessageBox.Show(mensaje, "Información", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void CerrarVentana()
        {
            this.Close();
        }

  
        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            string nombreCategoria = NombreCategoriaTextBox.Text.Trim();

            if (string.IsNullOrEmpty(nombreCategoria))
            {
                MostrarMensaje("El nombre de la categoría no puede estar vacío.");
                return;
            }

            _presenter.GuardarCategoria(nombreCategoria);
        }

     
        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            CerrarVentana();
        }
    }
}
