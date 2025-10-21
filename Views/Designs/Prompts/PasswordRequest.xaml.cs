using System;
using System.Windows;
using ProdLogApp.Models;
using ProdLogApp.Presenters;
using ProdLogApp.Interfaces;
using ProdLogApp.Servicios;

namespace ProdLogApp.Views
{
    public partial class PasswordRequest : Window, ISolicitudPasswordVista
    {
        private readonly Usuario _activeUser;
        private readonly IServicioUsuarios _svcUsuarios;
        private readonly PasswordRequestPresenter _presenter;

        public event Action OnConfirmarSolicitud;
        public event Action OnCancelar;

        public PasswordRequest(Usuario activeUser)
        {
            InitializeComponent();

            _activeUser = activeUser ?? throw new ArgumentNullException(nameof(activeUser));
            _svcUsuarios = new ServicioUsuariosMySql(new ProveedorConexionMySql("ProdLogDb"));

            _presenter = new PasswordRequestPresenter(
                vista: this,
                svcUsuarios: _svcUsuarios,
                usuario: _activeUser,
                onSuccess: AbrirMenuGerente
            );
        }

        public string ObtenerPasswordIngresada() => txtPassword?.Password ?? string.Empty;

        public void MostrarMensaje(string mensaje)
            => MessageBox.Show(mensaje, "Información", MessageBoxButton.OK, MessageBoxImage.Information);

        public void Cerrar() => Close();

        private void AbrirMenuGerente()
        {
            var menu = new ManagerMenu(_activeUser);

            // 1) Mostrar el menú
            menu.Show();

            // 2) Hacerlo MainWindow ANTES de cerrar el owner (evita que la app se cierre)
            Application.Current.MainWindow = menu;

            // 3) Cerrar el owner (Login) y este popup
            this.Owner?.Close();
            this.Close();
        }

        private void Confirmar(object sender, RoutedEventArgs e) => OnConfirmarSolicitud?.Invoke();
        private void Cancelar(object sender, RoutedEventArgs e) => OnCancelar?.Invoke();
    }
}
