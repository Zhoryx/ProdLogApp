using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Presenters;
using ProdLogApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ProdLogApp.Views
{
    public partial class PositionManagement : Window, IPositionManagementView
    {
        private readonly PositionManagementPresenter _presenter;
        private readonly User _activeUser;
        private readonly IDatabaseService _databaseService;
        private GridViewColumnHeader _lastHeaderClicked = null;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;

        public event Action OnAddPosition;
        public event Action OnDeletePosition;
        public event Action OnModifyPosition;
        public event Action OnReturn;

        public PositionManagement(User activeUser, IDatabaseService databaseService)
        {
            InitializeComponent();
            _activeUser = activeUser;
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
            _presenter = new PositionManagementPresenter(this, _databaseService);
            _presenter.CargarPuestos();
        }

        private void AddPosition(object sender, RoutedEventArgs e) => OnAddPosition?.Invoke();
        private void DeletePosition(object sender, RoutedEventArgs e) => OnDeletePosition?.Invoke();
        private void ModifyPosition(object sender, RoutedEventArgs e) => OnModifyPosition?.Invoke();
        private void ReturnToMenu(object sender, RoutedEventArgs e) => OnReturn?.Invoke();

        public void CloseWindow() => this.Close();

        public void NavigateToMenu()
        {
            ManagerMenu menu = new ManagerMenu(_activeUser);
            menu.Show();
            CloseWindow();

        }

        public void AbrirVentanaAgregar()
        {
            var ventana = new PositionAdd(_databaseService);
            if (ventana.ShowDialog() == true)
            {
                _presenter.CargarPuestos();
            }
        }

        public void AbrirVentanaModificar(Position puesto)
        {
            var ventana = new PositionAdd(_databaseService, puesto);
            if (ventana.ShowDialog() == true)
            {
                _presenter.CargarPuestos();
            }
        }

        public Position ObtenerPuestoSeleccionado()
        {
            return Positions.SelectedItem as Position;
        }

        public void MostrarMensaje(string mensaje)
        {
            MessageBox.Show(mensaje, "Información", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void MostrarPuestos(List<Position> puestos)
        {
            Positions.ItemsSource = puestos;
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            var headerClicked = sender as GridViewColumnHeader;
            var sortBy = headerClicked?.Tag?.ToString();
            if (string.IsNullOrEmpty(sortBy)) return;

            ListSortDirection direction = (headerClicked != _lastHeaderClicked)
                ? ListSortDirection.Ascending
                : (_lastDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending);

            Sort(sortBy, direction);
            _lastHeaderClicked = headerClicked;
            _lastDirection = direction;
        }

        private void Sort(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView = CollectionViewSource.GetDefaultView(Positions.ItemsSource);
            if (dataView == null) return;

            dataView.SortDescriptions.Clear();
            dataView.SortDescriptions.Add(new SortDescription(sortBy, direction));
            dataView.Refresh();
        }
       

        

        


    }
}
