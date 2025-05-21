using System;
using System.Windows;
using ProdLogApp.Models;

namespace ProdLogApp.Views
{
    public partial class ManagerMenu : Window
    {
        private readonly User _activeUser;
        public ManagerMenu(User activeUser)
        {
            InitializeComponent();
            _activeUser = activeUser;
        }

        public ManagerMenu()
        {
        }

        //// Método para abrir la ventana de Partes Diarios
        private void AbrirPartesDiarios(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuProduccionGerente partesDiarios = new MenuProduccionGerente();
                partesDiarios.Show();
                this.Close(); // Cierra la ventana actual
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir Partes Diarios: {ex.Message}");
            }
        }

        //// Método para abrir la ventana de GestionProducto
        private void MaestroProducto(object sender, RoutedEventArgs e)
        {
            try
            {
                GestionProducto maestroProducto = new GestionProducto();
                maestroProducto.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir Maestro Producto: {ex.Message}");
            }
        }

        //// Método para abrir la ventana de GestionCategoria
        private void MaestroCategoria(object sender, RoutedEventArgs e)
        {
            try
            {
                GestionCategoria maestroCategoria = new GestionCategoria();
                maestroCategoria.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir Maestro Categoria: {ex.Message}");
            }
        }

        //// Método para abrir la ventana de GestionPuesto
        private void MaestroPuesto(object sender, RoutedEventArgs e)
        {
            try
            {
                GestionPuesto maestroPuesto = new GestionPuesto();
                maestroPuesto.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir Maestro Puesto: {ex.Message}");
            }
        }

        //// Método para abrir la ventana de GestionUsuario
        private void MaestroUsuario(object sender, RoutedEventArgs e)
        {
            try
            {
                GestionUsuario maestroUsuario = new GestionUsuario();
                maestroUsuario.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir Maestro Usuario: {ex.Message}");
            }
        }
    }
}
