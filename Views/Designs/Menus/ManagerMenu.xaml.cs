using System;
using System.Windows;
using ProdLogApp.Models;
using ProdLogApp.Presenters;
using ProdLogApp.Views.Interfaces;

namespace ProdLogApp.Views
{
    public partial class ManagerMenu : Window, IManagerMenuView
    {
        private readonly ManagerMenuPresenter _presenter;
        private readonly User _activeUser; // Store the active user

        public event Action OnOpenDailyReports;
        public event Action OnManageProducts;
        public event Action OnManageCategories;
        public event Action OnManagePositions;
        public event Action OnManageUsers;
        public event Action OnDisconnect;

        public ManagerMenu(User activeUser)
        {
            InitializeComponent();
            _activeUser = activeUser; // Save the active user
            _presenter = new ManagerMenuPresenter(this, _activeUser); // Pass the user to the presenter
        }

        private void OpenDailyReports(object sender, RoutedEventArgs e) => OnOpenDailyReports?.Invoke();
        private void ManageProducts(object sender, RoutedEventArgs e) => OnManageProducts?.Invoke();
        private void ManageCategories(object sender, RoutedEventArgs e) => OnManageCategories?.Invoke();
        private void ManagePositions(object sender, RoutedEventArgs e) => OnManagePositions?.Invoke();
        private void ManageUsers(object sender, RoutedEventArgs e) => OnManageUsers?.Invoke();
        private void Disconnect(object sender, RoutedEventArgs e) => OnDisconnect?.Invoke();

        public void CloseWindow()
        {
            this.Close(); // Close ManagerMenu
        }

        public void NavigateToLogin()
        {
            Login login = new Login();
            login.Show(); // Show the Login window correctly
            CloseWindow(); // Close ManagerMenu
        }
    }
}
