using System;
using System.Windows;
using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Presenters;
using ProdLogApp.Servicios;  // <-- capa modularizada

namespace ProdLogApp.Views
{
    public partial class AgregarUsuario : Window, IAgregarUsuarioVista
    {
        private readonly AgregarUsuarioPresenter _presenter;

        // IMPORTANTE: mantené este ctor con parámetro opcional para poder abrir "Agregar" o "Modificar"
        public AgregarUsuario(Usuario? usuarioAEditar = null)
        {
            InitializeComponent();

            // Inyección simple de servicios (si tenés contenedor DI, reemplazalo acá)
            var proveedor = new ProveedorConexionMySql("ProdLogDb");
            var servicioUsuarios = new ServicioUsuariosMySql(proveedor);

            _presenter = new AgregarUsuarioPresenter(this, servicioUsuarios, usuarioAEditar);
        }

        // ========= Eventos que escucha el Presenter =========
        public event Action AlAceptar;
        public event Action AlCancelar;

        // ========= Lectura de datos desde la UI =========
        public string ObtenerNombre() => NombreTextBox.Text?.Trim() ?? string.Empty;
        public string ObtenerDni() => DniTextBox.Text?.Trim() ?? string.Empty;

        public bool ObtenerEsGerente() => EsGerencialCheckBox.IsChecked == true;

        // Si no tenés selector de fecha, devuelvo hoy (o null si preferís)
        public DateTime? ObtenerFechaIngreso() => DateTime.Today;

        // Si no tenés checkbox “Activo” en este diálogo, asumimos true en el alta.
        public bool ObtenerActivo() => true;

        // Si la cuenta es gerencial y cargaste password, la retorno; si no, null
        public string? ObtenerPasswordInicial()
        {
            if (EsGerencialCheckBox.IsChecked != true) return null;
            return PasswordGerenteBox.Password?.Trim() is string p && p.Length > 0 ? p : null;
        }

        // ========= Escritura/feedback en la UI =========
        public void CargarDatosIniciales(string nombre, string dni, bool esGerente, DateTime? fechaIngreso, bool activo)
        {
            NombreTextBox.Text = nombre ?? string.Empty;
            DniTextBox.Text = dni ?? string.Empty;
            EsGerencialCheckBox.IsChecked = esGerente;
            PasswordGerenteBox.Password = string.Empty; // no se muestra por seguridad
            // fechaIngreso/activo no se editan acá (si luego los agregás a la UI, los seteás)
        }

        public void MostrarMensaje(string mensaje) =>
            MessageBox.Show(mensaje, "Información", MessageBoxButton.OK, MessageBoxImage.Information);

        public void Cerrar() => Close();

        // ========= Handlers de botones =========
        private void Confirmar_Click(object sender, RoutedEventArgs e) => AlAceptar?.Invoke();
        private void Cancelar_Click(object sender, RoutedEventArgs e) => AlCancelar?.Invoke();
    }
}
