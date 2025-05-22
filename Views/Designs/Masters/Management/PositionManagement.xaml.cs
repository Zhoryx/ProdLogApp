using System;
using System.Windows;
using ProdLogApp.Models;
using ProdLogApp.Presenters;
using ProdLogApp.Views.Interfaces;

namespace ProdLogApp.Views
{
    public partial class PositionManagement : Window, IPositionManagementView
    {
        private readonly PositionManagementPresenter _presenter;
        private readonly User _activeUser;
        public event Action OnAgregarPuesto;
        public event Action OnEliminarPuesto;
        public event Action OnModificarPuesto;
        public event Action OnVolver;

        public PositionManagement(User activeUser)
        {
            InitializeComponent();
            _activeUser = activeUser;
            _presenter = new PositionManagementPresenter(this);
        }

        private void AgregarPuesto(object sender, RoutedEventArgs e) => OnAgregarPuesto?.Invoke();
        private void EliminarPuesto(object sender, RoutedEventArgs e) => OnEliminarPuesto?.Invoke();
        private void ModificarPuesto(object sender, RoutedEventArgs e) => OnModificarPuesto?.Invoke();
        private void Volver(object sender, RoutedEventArgs e) => OnVolver?.Invoke();

        public void CerrarVentana()
        {
            this.Close(); 
        }

        public void NavegarAMenu()
        {
            ManagerMenu menu = new ManagerMenu(_activeUser);
            menu.Show();
            CerrarVentana(); 
        }
    }
}
