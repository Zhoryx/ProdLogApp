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

        private readonly IDatabaseService _databaseService;

        public Login(IDatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
            _presenter = new LoginPresenter(this);
        }

        public string Dni => DniTextBox.Text;

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public void ShowAdminWindow(User activeUser)
        {
          
            UserSession.GetInstance().SetUser(activeUser);

            
            var passwordRequest = new PasswordRequest(activeUser, _databaseService);
            passwordRequest.Owner = this;
            passwordRequest.ShowDialog();
        }

        public void ShowMainWindow(User activeUser)
        {
            
            UserSession.GetInstance().SetUser(activeUser);

            
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
