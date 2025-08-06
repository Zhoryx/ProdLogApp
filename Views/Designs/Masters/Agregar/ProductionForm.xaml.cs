using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Services;
using ProdLogApp.Views.Designs.Prompts;
using System;
using System.Windows;

namespace ProdLogApp.Views
{
    public partial class ProductionForm : Window
    {
        private readonly User _activeUser;
        private readonly IDatabaseService _databaseService;
        public ProductionForm(List<Production> productionList, int productionIndex, User activeuser, IDatabaseService databaseService)
        {
            InitializeComponent();
            _activeUser = activeuser;
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
        } 

        // Evento para el botón "Confirmar"
        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
           
        }

        // Evento para el botón "Cancelar"
        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            // Cierra la ventana sin guardar cambios
            this.Close();
        }

        // Método para validar el formato de hora (hh:mm)
        private bool IsValidTimeFormat(string timeInput)
        {
            TimeSpan time;
            return TimeSpan.TryParseExact(timeInput, "hh\\:mm", null, out time);
        }

        private void SeleccionarProducto_Click(object sender, RoutedEventArgs e)
        {


            var prompt = new PromptProduct( _activeUser, _databaseService);
            bool? resultado = prompt.ShowDialog();

            if (resultado == true)
            {
                var producto = prompt.ObtenerProductoSeleccionado();
                ProductoTextBox.Text = producto.Nombre;
            }
            else
            {
                // El usuario canceló
            }

        }

    }
}
