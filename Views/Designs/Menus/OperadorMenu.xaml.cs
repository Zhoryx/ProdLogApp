using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Presenters;
using ProdLogApp.Servicios;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ProdLogApp.Views
{
    public partial class OperadorMenu : Window, IMenuOperarioVista
    {
        private readonly MenuOperarioPresenter _presenter;
        private readonly IServicioProducciones _servicioProducciones;
        private readonly int _usuarioIdActual;

        private readonly bool _modoGerente;
        private readonly DateTime? _fechaParte;

        public event Func<Task> OnCargarProduccion;
        public event Func<Task> OnEliminarProduccionSeleccionada;
        public event Func<Task> OnRefrescarListado;
        public event Action OnSalir;

        // ===== MODO OPERARIO =====
        public OperadorMenu()
        {
            InitializeComponent();

            var activeUser = UserSession.GetInstance().ActiveUser
                ?? throw new InvalidOperationException("No hay usuario activo en sesión.");
            _usuarioIdActual = activeUser.Id;

            var proveedor = new ProveedorConexionMySql("ProdLogDb");
            _servicioProducciones = new ServicioProduccionesMySql(proveedor);

            // 👇 cambio 1: ser explícitos pasando fechaFija = null
            _presenter = new MenuOperarioPresenter(this, _servicioProducciones, _usuarioIdActual, fechaFija: null);

            OnRefrescarListado += RefrescarListado_UI;
            OnSalir += () => Close();

            _modoGerente = false;
            _fechaParte = null;

            Loaded += async (_, __) =>
            {
                txtFechaHoy.Text = $"Hoy: {DateTime.Today:dd/MM/yyyy}";
                if (OnRefrescarListado != null)
                    await OnRefrescarListado.Invoke();
            };
        }

        // ===== MODO GERENTE (modal) =====
        public OperadorMenu(IProveedorConexion proveedorConexion,
                            int usuarioId,
                            DateTime fecha,
                            string operarioNombre = null,
                            bool asManagerModal = true)
        {
            InitializeComponent();

            _usuarioIdActual = usuarioId;
            _servicioProducciones = new ServicioProduccionesMySql(proveedorConexion);

            // 👇 cambio 2: pasar la fecha al presenter
            _presenter = new MenuOperarioPresenter(this, _servicioProducciones, _usuarioIdActual, fechaFija: fecha);

            OnRefrescarListado += RefrescarListado_UI;
            OnSalir += () => Close();

            _modoGerente = asManagerModal;
            _fechaParte = fecha.Date;

            Title = "Detalle de Parte (Gerencia)";
            Loaded += async (_, __) =>
            {
                txtFechaHoy.Text = $"Operario: {operarioNombre ?? usuarioId.ToString()} | Fecha: {fecha:dd/MM/yyyy}";
                if (OnRefrescarListado != null)
                    await OnRefrescarListado.Invoke();

                btnDisconnect.Content = "Volver";
                btnDisconnect.ToolTip = "Cerrar y volver a Partes Diarios";
            };
        }

        public void MostrarMensaje(string mensaje)
            => MessageBox.Show(mensaje, "Información", MessageBoxButton.OK, MessageBoxImage.Information);

        public void ActualizarListadoProducciones(List<Produccion> items)
        {
            Productions_list.ItemsSource = null;
            Productions_list.ItemsSource = items ?? new List<Produccion>();
        }

        public void AgregarProduccionAListado(Produccion produccion)
        {
            if (Productions_list?.ItemsSource is List<Produccion> current)
            {
                current.Add(produccion);
                Productions_list.Items.Refresh();
            }
            else
            {
                ActualizarListadoProducciones(new List<Produccion> { produccion });
            }
        }

        public Produccion ObtenerProduccionSeleccionada()
            => Productions_list?.SelectedItem as Produccion;

        public Produccion ObtenerProduccionIngresada()
        {
            var activeUser = UserSession.GetInstance().ActiveUser;
            var dlg = new FormularioProduccion(activeUser, _servicioProducciones)
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            bool? ok = dlg.ShowDialog();
            return ok == true ? dlg.ProduccionCreada : null;
        }

        private Task RefrescarListado_UI() => Task.CompletedTask;

        private async Task EditarSeleccionadaAsync()
        {
            if (Productions_list?.SelectedItem is not Produccion sel) return;

            var activeUser = UserSession.GetInstance().ActiveUser;
            var dlg = new FormularioProduccion(
                activeUser,
                _servicioProducciones,
                isEdit: true,
                editId: sel.ProduccionId)
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            bool? ok = dlg.ShowDialog();
            if (ok == true && OnRefrescarListado != null)
                await OnRefrescarListado.Invoke();
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            if (OnCargarProduccion != null)
                await OnCargarProduccion.Invoke();
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (OnEliminarProduccionSeleccionada != null)
                await OnEliminarProduccionSeleccionada.Invoke();
        }

        private async void Modify_Click(object sender, RoutedEventArgs e)
            => await EditarSeleccionadaAsync();

        private async void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (_modoGerente)
            {
                DialogResult = true;
                Close();
            }
            else
            {
                var login = new Login();
                Application.Current.MainWindow = login;
                login.Show();
                Close();
            }
        }

        private void Disconnect_Click(object sender, RoutedEventArgs e)
        {
            if (_modoGerente)
            {
                DialogResult = false;
                Close();
                return;
            }

            var login = new Login();
            Application.Current.MainWindow = login;
            login.Show();
            Close();
        }

        private async void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
            => await EditarSeleccionadaAsync();
    }
}
