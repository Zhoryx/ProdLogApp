using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Presenters;
using ProdLogApp.Servicios;

namespace ProdLogApp.Views
{
    public partial class GestionUsuariosWindow : Window, IGestUsuariosVista
    {
        private readonly GestUsuariosPresenter _presenter;
        private readonly IServicioUsuarios _svcUsuarios;
        private readonly Usuario _usuarioActivo;

        public event Action OnAgregar;
        public event Action OnModificar;
        public event Action OnAlternarEstado;
        public event Action OnResetearPassword;
        public event Action OnEliminar;
        public event Action OnVolverMenu;

        private GridViewColumnHeader _lastHeaderClicked = null;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;

        public GestionUsuariosWindow(Usuario usuarioActivo)
        {
            InitializeComponent();
            _usuarioActivo = usuarioActivo ?? throw new ArgumentNullException(nameof(usuarioActivo));

            var proveedor = new ProveedorConexionMySql("ProdLogDb");
            _svcUsuarios = new ServicioUsuariosMySql(proveedor);

            _presenter = new GestUsuariosPresenter(this, _svcUsuarios);
        }

        public void MostrarUsuarios(List<Usuario> usuarios)
        {
            Users_list.ItemsSource = usuarios ?? new List<Usuario>();
        }

        public Usuario ObtenerUsuarioSeleccionado() => Users_list.SelectedItem as Usuario;

        public void AbrirVentanaAgregarUsuario()
        {
            var win = new AgregarUsuario
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            win.ShowDialog();
        }

        public void AbrirVentanaModificarUsuario(Usuario usuario)
        {
            if (usuario == null) return;
            var win = new AgregarUsuario(usuario)
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            win.ShowDialog();
        }

        public void MostrarMensaje(string mensaje)
            => MessageBox.Show(mensaje, "Información", MessageBoxButton.OK, MessageBoxImage.Information);

        public void NavegarAMenu()
        {
            // Si esta ventana se abrió con ShowDialog() desde ManagerMenu,
            // solo cerrar es suficiente para “volver” al menú.
            Close();

            // Si la hubieras abierto con Show() y tenés Owner:
            // Owner?.Show();
            // Owner?.Activate();
        }

        // ==== Handlers UI ====
        private void Users_list_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

        private void Users_list_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var clicked = e.OriginalSource as DependencyObject;
            var container = ItemsControl.ContainerFromElement(Users_list, clicked) as ListViewItem;
            if (container == null) return;

            if (Users_list?.SelectedItem is Usuario)
                OnModificar?.Invoke();
        }

        private void Users_list_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter || e.Key == Key.F2) && Users_list?.SelectedItem is Usuario)
            {
                OnModificar?.Invoke();
                e.Handled = true;
            }
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            var headerClicked = sender as GridViewColumnHeader;
            var sortBy = headerClicked?.Tag?.ToString();
            if (string.IsNullOrWhiteSpace(sortBy)) return;

            var direction = headerClicked != _lastHeaderClicked
                ? ListSortDirection.Ascending
                : (_lastDirection == ListSortDirection.Ascending
                    ? ListSortDirection.Descending
                    : ListSortDirection.Ascending);

            Sort(sortBy, direction);
            _lastHeaderClicked = headerClicked;
            _lastDirection = direction;
        }

        private void Sort(string sortBy, ListSortDirection direction)
        {
            var dataView = CollectionViewSource.GetDefaultView(Users_list.ItemsSource);
            if (dataView == null) return;

            dataView.SortDescriptions.Clear();
            dataView.SortDescriptions.Add(new SortDescription(sortBy, direction));
            dataView.Refresh();
        }

        // Botones
        private void BtnVolver_Click(object sender, RoutedEventArgs e) => OnVolverMenu?.Invoke();
        private void BtnAgregar_Click(object sender, RoutedEventArgs e) => OnAgregar?.Invoke();
        private void BtnModificar_Click(object sender, RoutedEventArgs e) => OnModificar?.Invoke();
        private void BtnAlternarEstado_Click(object sender, RoutedEventArgs e) => OnAlternarEstado?.Invoke();
        // Si agregás botones extra:
        // private void BtnResetearPass_Click(object s, RoutedEventArgs e) => OnResetearPassword?.Invoke();
        // private void BtnEliminar_Click(object s, RoutedEventArgs e) => OnEliminar?.Invoke();
    }
}
