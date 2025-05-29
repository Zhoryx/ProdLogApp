using ProdLogApp.Models;
using ProdLogApp.Presenters;
using ProdLogApp.Services;
using ProdLogApp.Views.Interfaces;
using System;
using System.Windows;

namespace ProdLogApp.Views
{
    public partial class PositionManagement : Window, IPositionManagementView
    {
        private readonly PositionManagementPresenter _presenter;
        private readonly User _activeUser;
        private readonly IDatabaseService _databaseService;
        public event Action OnAddPosition;
        public event Action OnDeletePosition;
        public event Action OnModifyPosition;
        public event Action OnReturn;

        public PositionManagement(User activeUser, IDatabaseService databaseService)
        {
            InitializeComponent();
            _activeUser = activeUser;
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
            _presenter = new PositionManagementPresenter(this);
        }

        // Event handlers for position management actions
        private void AddPosition(object sender, RoutedEventArgs e) => OnAddPosition?.Invoke();
        private void DeletePosition(object sender, RoutedEventArgs e) => OnDeletePosition?.Invoke();
        private void ModifyPosition(object sender, RoutedEventArgs e) => OnModifyPosition?.Invoke();
        private void ReturnToMenu(object sender, RoutedEventArgs e) => OnReturn?.Invoke();

        // Method to close the current window
        public void CloseWindow()
        {
            this.Close();
        }

        // Method to navigate back to the main menu
        public void NavigateToMenu()
        {
            ManagerMenu menu = new ManagerMenu(_activeUser);
            menu.Show();
            CloseWindow();
        }
    }
}
