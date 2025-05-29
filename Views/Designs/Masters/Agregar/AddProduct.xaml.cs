using System.Collections.Generic;
using System.Windows;
using ProdLogApp.Services;
using ProdLogApp.Models;

namespace ProdLogApp.Views
{
    public partial class ProductoAgregar : Window
    {
        private readonly IDatabaseService _databaseService;

        public ProductoAgregar(IDatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService;

            CargarCategorias();
        }

        private async void CargarCategorias()
        {
            var categorias = await _databaseService.ObtenerCategoriasDesdeDBAsync();

            if (categorias == null || categorias.Count == 0)
            {
                MessageBox.Show("No se encontraron categorías en la base de datos.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show($"Categorías cargadas: {categorias.Count}", "Carga Exitosa", MessageBoxButton.OK, MessageBoxImage.Information);

            ComboBoxCategorias.ItemsSource = categorias;
            ComboBoxCategorias.DisplayMemberPath = "Nombre";
            ComboBoxCategorias.SelectedValuePath = "Id";
        }



        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            string nombreProducto = NombreProducto.Text;
            Categoria categoriaSeleccionada = (Categoria)ComboBoxCategorias.SelectedItem;

            if (categoriaSeleccionada != null && !string.IsNullOrWhiteSpace(nombreProducto))
            {
                Producto nuevoProducto = new Producto
                {
                    Nombre = nombreProducto,
                    CategoriaId = categoriaSeleccionada.Id
                };

                _databaseService.AgregarProductoEnDB(nuevoProducto);
                MessageBox.Show("Producto agregado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            else
            {
                MessageBox.Show("Debe ingresar un nombre y seleccionar una categoría.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
