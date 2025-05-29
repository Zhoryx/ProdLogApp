using ProdLogApp.Models;
using ProdLogApp.Presenters;
using ProdLogApp.Services;
using ProdLogApp.Views.Interfaces;
using System;
using System.Windows;

namespace ProdLogApp.Views
{
    public partial class PasswordRequest : Window, IPasswordRequestView
    {
        private readonly User _activeUser;
        private readonly PasswordRequestPresenter _presenter;
        private readonly IDatabaseService _databaseService;
        public PasswordRequest(User activeUser, IDatabaseService databaseService)
        {
            InitializeComponent();
            _activeUser = activeUser ?? throw new ArgumentNullException(nameof(activeUser));
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));

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
            this.Owner.Close();
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
