using System;
using System.Windows;
using ProdLogApp.Models;
using ProdLogApp.Presenters;
using ProdLogApp.Views.Interfaces;

namespace ProdLogApp.Views
{
    public partial class UserManagement : Window, IUserManagementView
    {
        private UserManagementPresenter _presenter;
        private readonly User _activeUser;
        public event Action OnAgregarUsuario;
        public event Action OnEliminarUsuario;
        public event Action OnModificarUsuario;
        public event Action OnVolver;

        public UserManagement(User activeUser)
        {
            InitializeComponent();
            _activeUser = activeUser;
            _presenter = new UserManagementPresenter(this);
        }

        private void UsuarioAgregar(object sender, RoutedEventArgs e) => OnAgregarUsuario?.Invoke();
        private void EliminarUsuario(object sender, RoutedEventArgs e) => OnEliminarUsuario?.Invoke();
        private void ModificarUsuario(object sender, RoutedEventArgs e) => OnModificarUsuario?.Invoke();
        private void Volver(object sender, RoutedEventArgs e) => OnVolver?.Invoke();

        

        public void NavegarAMenu()
        {
            ManagerMenu menu = new ManagerMenu(_activeUser);
            menu.Show();
            this.Close();
        }
    }
}
