using System;
using System.Windows;
using ProdLogApp.Presenters;
using ProdLogApp.Models;
using ProdLogApp.Views.Interfaces;

namespace ProdLogApp.Views
{
    public partial class PasswordRequest : Window, IPasswordRequestView
    {
        private readonly User _activeUser;
        private readonly PasswordRequestPresenter _presenter;

        public PasswordRequest(User activeUser)
        {
            InitializeComponent();
            _activeUser = activeUser ?? throw new ArgumentNullException(nameof(activeUser));
            _presenter = new PasswordRequestPresenter(this);
        }

       
        public string EnteredPassword => txtPassword.Password; 

        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void CloseWindow()
        {
            this.Close();
        }

        public void ShowAdminMenu(User activeUser)
        {
            var managerMenu = new ManagerMenu(activeUser);
            managerMenu.Show();
            CloseWindow();
        }

        private void Confirmar(object sender, RoutedEventArgs e)
        {
            _presenter.ValidatePassword(_activeUser, EnteredPassword);
        }

        private void Cancelar(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }
    }
}
