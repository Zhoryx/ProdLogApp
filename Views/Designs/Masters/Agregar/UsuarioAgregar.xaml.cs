using System;
using System.Windows;

namespace ProdLogApp.Views
{
    public partial class UsuarioAgregar : Window
    {
        public UsuarioAgregar()
        {
            InitializeComponent();
        }

        // Evento para el botón "Seleccionar"
        private void Seleccionar_Click(object sender, RoutedEventArgs e)
        {
        }

        // Evento para el botón "Confirmar"
        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {}

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
