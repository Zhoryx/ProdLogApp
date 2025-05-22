using System;
using System.Windows;
using ProdLogApp.Models;
using ProdLogApp.Presenters;
using ProdLogApp.Views.Interfaces;

namespace ProdLogApp.Views
{
    public partial class CategoryManagement : Window, ICategoryManagementView
    {
        private readonly CategoryManagementPresenter _presenter;
        private readonly User _activeUser;
        public event Action OnAgregarCategoria;
        public event Action OnEliminarCategoria;
        public event Action OnModificarCategoria;
        public event Action OnVolver;

        public CategoryManagement(User activeuser)
        {
            InitializeComponent();
            _activeUser = activeuser;
            _presenter = new CategoryManagementPresenter(this);
        }

        private void AgregarCategoria(object sender, RoutedEventArgs e) => OnAgregarCategoria?.Invoke();
        private void EliminarCategoria(object sender, RoutedEventArgs e) => OnEliminarCategoria?.Invoke();
        private void ModificarCategoria(object sender, RoutedEventArgs e) => OnModificarCategoria?.Invoke();
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
