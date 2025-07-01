using ProdLogApp.Models;
using ProdLogApp.Services;
using ProdLogApp.Views.Designs.Prompts;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ProdLogApp.Interfaces;


namespace ProdLogApp.Views
{


    public partial class AddProduct : Window
    {
        private int _categoriaIdSeleccionada = 0;
        private readonly IDatabaseService _databaseService;

        public AddProduct(IDatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService;

           
        }

        private readonly Producto _producto;

        public AddProduct(IDatabaseService databaseService, Producto producto) : this(databaseService)
        {
            _producto = producto;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_producto != null)
            {
                NombreProducto.Text = _producto.Nombre;
                _categoriaIdSeleccionada = _producto.CategoriaId;
                SeleccionCategoria.Content = $"Seleccionado: {_producto.CategoriaNombre}";
            }
        }



        private void SeleccionarProducto_Click(object sender, RoutedEventArgs e)
        {
            var prompt = new PromptCategory(_databaseService);
            bool? resultado = prompt.ShowDialog();

            if (resultado == true)
            {
                prompt.ObtenerSeleccion(out _categoriaIdSeleccionada, out string descripcion);
                SeleccionCategoria.Content = $"Seleccionada: {descripcion}"; 
            }
        }


        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NombreProducto.Text))
            {
                MessageBox.Show("El nombre del producto no puede estar vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_categoriaIdSeleccionada == 0)
            {
                MessageBox.Show("Debe seleccionar una categoría.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_producto == null) 
            {
                Producto nuevoProducto = new Producto
                {
                    Nombre = NombreProducto.Text,
                    CategoriaId = _categoriaIdSeleccionada
                };

                _databaseService.AgregarProductoEnDB(nuevoProducto);
            }
            else 
            {
                _producto.Nombre = NombreProducto.Text;
                _producto.CategoriaId = _categoriaIdSeleccionada;

                _databaseService.ModificarProductoEnDB(_producto);
            }

            MessageBox.Show("Operación realizada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            DialogResult = true;
            Close();
        }



        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

       
    }
}
