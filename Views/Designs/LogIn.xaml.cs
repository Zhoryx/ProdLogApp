using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Presenters;
using ProdLogApp.Servicios;
using System;
using System.Windows;

namespace ProdLogApp.Views
{
    public partial class Login : Window, ILoginVista
    {
        private readonly LoginPresenter _presenter;
        private readonly IServicioUsuarios _svcUsuarios;

        public event Action OnIntentarLogin;
        public event Action OnAbrirSolicitudPassword;

        public Login()
        {
            InitializeComponent();

            var proveedor = new ProveedorConexionMySql("ProdLogDb");
            _svcUsuarios = new ServicioUsuariosMySql(proveedor);

            _presenter = new LoginPresenter(this, _svcUsuarios);
        }

        // === ILoginVista ===
        public string ObtenerDni() => DniTextBox.Text?.Trim() ?? string.Empty;
        public string ObtenerPassword() => string.Empty; // no se usa en esta pantalla

        public void MostrarMensaje(string mensaje) =>
            MessageBox.Show(mensaje, "Información", MessageBoxButton.OK, MessageBoxImage.Information);

        public void LimpiarCampos()
        {
            DniTextBox.Text = string.Empty;
            DniTextBox.Focus();
        }

        public void NavegarAMenuOperario()
        {
            var main = new OperadorMenu();
            main.Show();
            Close();
        }

        public void NavegarAMenuGerente()
        {
            // Tomamos el usuario activo ya seteado por el presenter
            var usuarioActivo = UserSession.GetInstance().ActiveUser;
            var dlg = new PasswordRequest(usuarioActivo)   // ⬅️ pasa el servicio AQUÍ
            {
                Owner = this
            };
            dlg.ShowDialog();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e) => OnIntentarLogin?.Invoke();
    }
}
