using System;
using System.Windows;
using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Servicios;
using ProdLogApp.Views;

namespace ProdLogApp.Views
{
    public partial class ManagerMenu : Window, IMenuGerenteVista
    {
        private readonly Usuario _activeUser;

        public event Action OnAbrirGestionCategorias;
        public event Action OnAbrirGestionProductos;
        public event Action OnAbrirGestionPuestos;
        public event Action OnAbrirGestionUsuarios;
        public event Action OnAbrirPanelProduccion;
        public event Action OnSalir;

        public ManagerMenu(Usuario activeUser)
        {
            InitializeComponent();
            _activeUser = activeUser ?? throw new ArgumentNullException(nameof(activeUser));
        }

        // Partes diarios
        private void OpenDailyReports(object sender, RoutedEventArgs e)
        {
            var prov = new ProveedorConexionMySql("ProdLogDb");
            var svc = new ServicioProduccionesMySql(prov);

            var win = new ProduccionesGerente(_activeUser, svc)
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            win.ShowDialog();
            OnAbrirPanelProduccion?.Invoke();
        }

        // Gestión de Productos
        private void ManageProducts(object sender, RoutedEventArgs e)
        {
            var win = new GestProducto(_activeUser)
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            win.ShowDialog();
            OnAbrirGestionProductos?.Invoke();
        }

        // Gestión de Categorías
        private void ManageCategories(object sender, RoutedEventArgs e)
        {
            var win = new GestCategoria(_activeUser)
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            win.ShowDialog();
            OnAbrirGestionCategorias?.Invoke();
        }

        // Gestión de Puestos
        private void ManagePositions(object sender, RoutedEventArgs e)
        {
            var win = new GestPuesto(_activeUser)
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            win.ShowDialog();
            OnAbrirGestionPuestos?.Invoke();
        }

        // Gestión de Usuarios
        private void ManageUsers(object sender, RoutedEventArgs e)
        {
            var win = new GestionUsuariosWindow(_activeUser)
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            win.ShowDialog();
            OnAbrirGestionUsuarios?.Invoke();
        }

        // Desconectar → Login
        private void Disconnect(object sender, RoutedEventArgs e)
        {
            UserSession.GetInstance().SignOut();

            var login = new Login();
            Application.Current.MainWindow = login;
            login.Show();
            Close();
            OnSalir?.Invoke();
        }

        public void MostrarMensaje(string mensaje)
            => MessageBox.Show(mensaje, "Información", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
