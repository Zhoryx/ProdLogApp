using System;
using System.Windows;
using ProdLogApp.Presenters;
using ProdLogApp.Models;
using ProdLogApp.Views.Interfaces;

namespace ProdLogApp.Views
{
    public partial class Login : Window, ILoginView
    {
        private readonly LoginPresenter _presenter;

        public Login()
        {
            InitializeComponent();
            _presenter = new LoginPresenter(this);
        }

        public string Dni => DniTextBox.Text;

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public void ShowAdminWindow(User user)
        {
            // Open the authentication window as a popup
            //var passwordRequest = new PasswordRequest(user);
            //passwordRequest.Owner = this; // Set this as parent window
            //passwordRequest.ShowDialog(); // Show as popup
        }

        public void ShowMainWindow(User activeUser)
        {
            // Open the main window and close the login
            //var mainWindow = new OperatorMenu(user);
            //mainWindow.Show();
            //this.Close(); // Close the login window
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            _presenter.ValidateLogin();
        }
    }
}
