using System;
using System.Windows;
using ProdLogApp.Models;
using ProdLogApp.Presenters;
using ProdLogApp.Interfaces;
using ProdLogApp.Services;

namespace ProdLogApp.Views
{
    public partial class ManagerMenu : Window, IManagerMenuView
    {
        private readonly ManagerMenuPresenter _presenter;
        private readonly User _activeUser;
        private readonly IDatabaseService _databaseService; // ✅ ahora sí se usa

        public event Action OnOpenDailyReports;
        public event Action OnManageProducts;
        public event Action OnManageCategories;
        public event Action OnManagePositions;
        public event Action OnManageUsers;
        public event Action OnDisconnect;

        public ManagerMenu(User activeUser)
        {
            InitializeComponent();

            _activeUser = activeUser ?? throw new ArgumentNullException(nameof(activeUser));
            _databaseService = new DatabaseService();                   // ✅ inicializar el CAMPO
            _presenter = new ManagerMenuPresenter(this, _activeUser, _databaseService);
        }

        private void OpenDailyReports(object sender, RoutedEventArgs e) => OnOpenDailyReports?.Invoke();
        private void ManageProducts(object sender, RoutedEventArgs e) => OnManageProducts?.Invoke();
        private void ManageCategories(object sender, RoutedEventArgs e) => OnManageCategories?.Invoke();
        private void ManagePositions(object sender, RoutedEventArgs e) => OnManagePositions?.Invoke();
        private void ManageUsers(object sender, RoutedEventArgs e) => OnManageUsers?.Invoke();
        private void Disconnect(object sender, RoutedEventArgs e) => OnDisconnect?.Invoke();

        public void CloseWindow() => Close();

        public void NavigateToLogin()
        {
            var login = new Login(_databaseService);                    // ✅ usa la MISMA instancia
            Application.Current.MainWindow = login;                     // (opcional) reasignar MainWindow
            login.Show();
            CloseWindow();
        }
    }
}
