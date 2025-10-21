using System;
using System.Windows;
using ProdLogApp.Models;
using ProdLogApp.Servicios;
using ProdLogApp.Views.Designs.Prompts;

namespace ProdLogApp.Views
{
    public partial class AgregarProducto : Window
    {
        private readonly IServicioProductos _svcProductos;
        private readonly IServicioCategorias _svcCategorias;
        private readonly Producto _producto;          // null = alta
        private int _categoriaIdSeleccionada = 0;

        // Alta
        public AgregarProducto(IServicioProductos svcProductos, IServicioCategorias svcCategorias)
        {
            InitializeComponent();
            _svcProductos = svcProductos ?? throw new ArgumentNullException(nameof(svcProductos));
            _svcCategorias = svcCategorias ?? throw new ArgumentNullException(nameof(svcCategorias));
        }

        // Edición
        public AgregarProducto(IServicioProductos svcProductos, IServicioCategorias svcCategorias, Producto producto)
            : this(svcProductos, svcCategorias)
        {
            _producto = producto;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_producto != null)
            {
                NombreProducto.Text = _producto.Nombre;
                _categoriaIdSeleccionada = _producto.CategoriaId;

                // Si querés mostrar el nombre de la categoría:
                var cat = await _svcCategorias.ObtenerPorIdAsync(_producto.CategoriaId);
                CategoriaTextBox.Text = cat != null
                    ? $"Seleccionada: {cat.Nombre}"
                    : $"Seleccionada (Id): {_producto.CategoriaId}";
            }
        }

        private void SeleccionarProducto_Click(object sender, RoutedEventArgs e)
        {
            // Abrimos el prompt modular (ver más abajo)
            var prompt = new PromptCategoria(_svcCategorias);
            if (prompt.ShowDialog() == true)
            {
                _categoriaIdSeleccionada = prompt.CategoriaSeleccionadaId;
                CategoriaTextBox.Text = $"Seleccionada: {prompt.CategoriaSeleccionadaNombre}";
            }
        }

        private async void Confirmar_Click(object sender, RoutedEventArgs e)
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

            try
            {
                if (_producto == null)
                {
                    var nuevo = new Producto
                    {
                        Nombre = NombreProducto.Text.Trim(),
                        CategoriaId = _categoriaIdSeleccionada,
                        Activo = true
                    };
                    await _svcProductos.CrearAsync(nuevo);
                }
                else
                {
                    _producto.Nombre = NombreProducto.Text.Trim();
                    _producto.CategoriaId = _categoriaIdSeleccionada;
                    await _svcProductos.ActualizarAsync(_producto);
                }

                MessageBox.Show("Operación realizada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo guardar: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e) => Close();
    }
}
