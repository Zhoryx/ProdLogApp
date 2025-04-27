using System;
using System.Windows;

namespace ProdLogApp.Views
{
    public partial class PuestoAgregar : Window
    {
        public PuestoAgregar()
        {
            InitializeComponent();
        }

        // Evento para el botón "Seleccionar"
        private void Seleccionar_Click(object sender, RoutedEventArgs e)
        {
            // Simulación de selección del producto a través de un prompt
            string productoSeleccionado = "Producto Ejemplo"; // Puedes reemplazarlo con tu lógica de selección
            ProductoLabel.Content = productoSeleccionado; // Actualiza el Label con el nombre del producto
        }

        // Evento para el botón "Confirmar"
        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            // Obtener los datos ingresados por el usuario
            string horaInicio = HoraInicioTextBox.Text; // TextBox para la Hora Inicio
            string horaFin = HoraFinTextBox.Text;       // TextBox para la Hora Fin
            string cantidad = CantidadTextBox.Text;     // TextBox para la Cantidad Producida

            // Validar el formato de las horas (hh:mm)
            if (!IsValidTimeFormat(horaInicio) || !IsValidTimeFormat(horaFin))
            {
                MessageBox.Show("Por favor, ingrese las horas en formato hh:mm.", "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Mensaje de confirmación con los datos ingresados
            MessageBox.Show($"Producto: {ProductoLabel.Content}\nHora Inicio: {horaInicio}\nHora Fin: {horaFin}\nCantidad: {cantidad}",
                            "Producción Confirmada", MessageBoxButton.OK, MessageBoxImage.Information);
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
    }
}
