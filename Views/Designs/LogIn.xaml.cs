using System;
using System.Windows;
using ProdLogApp.Presenters;
using ProdLogApp.Models;
using ProdLogApp.Views.Interfaces;
using ProdLogApp.Services; // ✅ Added missing namespace for UserSession

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

        public void ShowAdminWindow(User activeUser)
        {
            // ✅ Store user in session before opening new window
            UserSession.GetInstance().SetUser(activeUser);

            // Open the authentication window as a popup
            var passwordRequest = new PasswordRequest(activeUser);
            passwordRequest.Owner = this;
            passwordRequest.ShowDialog();
        }

        public void ShowMainWindow(User activeUser)
        {
            // ✅ Store user in session before opening OperatorMenu
            UserSession.GetInstance().SetUser(activeUser);

            // Open the main window and close the login
            var mainWindow = new OperatorMenu();
            mainWindow.Show();
            this.Close();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            _presenter.ValidateLogin();
        }
    }
}
