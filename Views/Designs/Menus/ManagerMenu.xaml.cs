using System;
using System.Windows;
using ProdLogApp.Models;
using ProdLogApp.Presenters;
using ProdLogApp.Interfaces;
using ProdLogApp.Services; // Agregamos la referencia al servicio de base de datos

namespace ProdLogApp.Views
{
    public partial class ManagerMenu : Window, IManagerMenuView
    {
        private readonly ManagerMenuPresenter _presenter;
        private readonly User _activeUser;
        private readonly IDatabaseService _databaseService; // Agregamos esta variable

        public event Action OnOpenDailyReports;
        public event Action OnManageProducts;
        public event Action OnManageCategories;
        public event Action OnManagePositions;
        public event Action OnManageUsers;
        public event Action OnDisconnect;

        // Modificamos el constructor para recibir IDatabaseService
        public ManagerMenu(User activeUser)
        {
            InitializeComponent();

            IDatabaseService databaseService = new DatabaseService(); // Instancia única en el menú
            _activeUser = activeUser;
            _presenter = new ManagerMenuPresenter(this, _activeUser, databaseService);
        }


        private void OpenDailyReports(object sender, RoutedEventArgs e) => OnOpenDailyReports?.Invoke();
        private void ManageProducts(object sender, RoutedEventArgs e) => OnManageProducts?.Invoke();
        private void ManageCategories(object sender, RoutedEventArgs e) => OnManageCategories?.Invoke();
        private void ManagePositions(object sender, RoutedEventArgs e) => OnManagePositions?.Invoke();
        private void ManageUsers(object sender, RoutedEventArgs e) => OnManageUsers?.Invoke();
        private void Disconnect(object sender, RoutedEventArgs e) => OnDisconnect?.Invoke();

        public void CloseWindow()
        {
            this.Close(); // Cerrar ManagerMenu
        }

        public void NavigateToLogin()
        {
            Login login = new Login(_databaseService);
            login.Show(); // Mostrar la ventana de Login
            CloseWindow(); // Cerrar ManagerMenu
        }
    }
}
