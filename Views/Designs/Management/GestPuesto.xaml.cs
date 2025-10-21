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
    public partial class GestPuesto : Window, IGestPuestosVista
    {
        private readonly GestPuestosPresenter _presenter;
        private readonly IServicioPuestos _svcPuestos;
        private readonly Usuario _usuarioActivo;

        public event Action OnAgregar;
        public event Action OnModificar;
        public event Action OnAlternarEstado;
        public event Action OnEliminar;
        public event Action OnVolverMenu;

        private GridViewColumnHeader _lastHeaderClicked = null;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;

        public GestPuesto(Usuario activeUser)
        {
            InitializeComponent();
            _usuarioActivo = activeUser ?? throw new ArgumentNullException(nameof(activeUser));

            var proveedor = new ProveedorConexionMySql("ProdLogDb");
            _svcPuestos = new ServicioPuestosMySql(proveedor);

            _presenter = new GestPuestosPresenter(this, _svcPuestos);
        }

        // ===== IGestPuestosVista =====
        public void MostrarPuestos(List<Puesto> puestos) => Positions.ItemsSource = puestos ?? new List<Puesto>();
        public Puesto ObtenerPuestoSeleccionado() => Positions?.SelectedItem as Puesto;

        public void AbrirVentanaAgregarPuesto()
        {
            var win = new AgregarPuesto(_svcPuestos, null)
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            win.ShowDialog();
        }

        public void AbrirVentanaModificarPuesto(Puesto puesto)
        {
            if (puesto == null) return;

            var win = new AgregarPuesto(_svcPuestos, puesto)
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
            // no crear otro ManagerMenu: reactivar el que te abrió (Owner) y cerrar
            Owner?.Activate();
            Close();
        }

        // ===== Handlers UI =====
        private void Positions_list_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var clicked = e.OriginalSource as DependencyObject;
            var container = ItemsControl.ContainerFromElement(Positions, clicked) as ListViewItem;
            if (container == null) return;

            if (Positions?.SelectedItem is Puesto) OnModificar?.Invoke();
        }

        private void Positions_list_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter || e.Key == Key.F2) && Positions?.SelectedItem is Puesto)
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
            var dataView = CollectionViewSource.GetDefaultView(Positions.ItemsSource);
            if (dataView == null) return;

            dataView.SortDescriptions.Clear();
            dataView.SortDescriptions.Add(new SortDescription(sortBy, direction));
            dataView.Refresh();
        }

        // Botones
        private void AddPosition(object sender, RoutedEventArgs e) => OnAgregar?.Invoke();
        private void ModifyPosition(object sender, RoutedEventArgs e) => OnModificar?.Invoke();
        private void DeletePosition(object sender, RoutedEventArgs e) => OnAlternarEstado?.Invoke(); // activar/desactivar
        private void ReturnToMenu(object sender, RoutedEventArgs e) => OnVolverMenu?.Invoke();
    }
}
