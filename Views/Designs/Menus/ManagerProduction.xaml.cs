using ProdLogApp.Models;
using ProdLogApp.Presenters;
using ProdLogApp.Views.Interfaces;
using System;
using System.Windows;

namespace ProdLogApp.Views
{
    public partial class ManagerProduction : Window, IManagerProductionView
    {
        private readonly ManagerProductionPresenter _presenter;
        private readonly User _activeUser;
        public event Action OnAgregarProduccion;
        public event Action OnEliminarProduccion;
        public event Action OnModificarProduccion;
        public event Action OnVolver;

        public ManagerProduction(User activeUser)
        {
            InitializeComponent();
            _activeUser= activeUser;
            _presenter = new ManagerProductionPresenter(this, _activeUser);
        }

        private void AgregarProduccion(object sender, RoutedEventArgs e) => OnAgregarProduccion?.Invoke();
        private void EliminarProduccion(object sender, RoutedEventArgs e) => OnEliminarProduccion?.Invoke();
        private void ModificarProduccion(object sender, RoutedEventArgs e) => OnModificarProduccion?.Invoke();
        private void Volver(object sender, RoutedEventArgs e) => OnVolver?.Invoke();

        public void CerrarVentana()
        {
            this.Close(); // ✅ Se cierra la ventana actual
        }

        public void NavegarAMenu()
        {
            ManagerMenu menu = new ManagerMenu(_activeUser);
            menu.Show();
            CerrarVentana();
        }
    }
}
