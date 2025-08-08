using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Services;
using ProdLogApp.Views.Designs.Prompts;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProdLogApp.Views
{
    public partial class ProductionForm : Window, IAddProductionUserView
    {
        private readonly AddProductionUserPresenter _presenter;
        private readonly User _activeUser; 
        private readonly IDatabaseService _databaseService;
        public ProductionForm( User activeuser, IDatabaseService databaseService)
        {
            InitializeComponent(); // Esto primero
            _activeUser = activeuser; 
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
            _presenter = new AddProductionUserPresenter(this); // Luego el resto
        }


        // Propiedades requeridas por la interfaz
        public string HoraInicio => HoraInicioTextBox.Text;
        public string HoraFin => HoraFinTextBox.Text;
        public string Cantidad => CantidadTextBox.Text;

        public void MostrarError(TextBox campo, string mensaje)
        {
            campo.Tag = "Error"; // activa el trigger visual
            campo.ToolTip = mensaje;
        }

        public void LimpiarError(TextBox campo)
        {
            campo.ClearValue(Control.TagProperty); // desactiva el trigger
            campo.ToolTip = null;
        }


        public void ActualizarHora(TextBox campo, string horaNormalizada)
        {
            campo.Text = horaNormalizada;
        }

        // Eventos
        private void HoraInicioTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
                _presenter.ValidarHora(tb, tb.Text);
        }

        private void HoraFinTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
                _presenter.ValidarHora(tb, tb.Text);
        }

        private void CantidadTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
                _presenter.ValidarCantidad(tb, tb.Text);
        }

        private void SeleccionarProducto_Click(object sender, RoutedEventArgs e)
        {
            var prompt = new PromptProduct(_activeUser, _databaseService); bool? resultado = prompt.ShowDialog(); if (resultado == true) { var producto = prompt.ObtenerProductoSeleccionado(); ProductoTextBox.Text = producto.Nombre; }
            else
            { // El usuario canceló } }
            }
        }

        private void SeleccionarPuesto_Click(object sender, RoutedEventArgs e)
        {
            var prompt = new PromptPosition(_activeUser, _databaseService);
            bool? resultado = prompt.ShowDialog();

            if (resultado == true)
            {
                int puestoId;
                string descripcion;
                prompt.ObtenerPuestoSeleccionado(out puestoId, out descripcion);
                PuestoTextBox.Text = descripcion;
            }
            else
            {
                // El usuario canceló
            }
        }






        // Evento para el botón "Confirmar"
        private void Confirmar_Click(object sender, RoutedEventArgs e) { 
        } 
        // Evento para el botón "Cancelar"
        private void Cancelar_Click(object sender, RoutedEventArgs e) 
        {   this.Close(); }

        private void PuestoTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}