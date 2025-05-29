using ProdLogApp.Models;
using ProdLogApp.Presenters;
using ProdLogApp.Services;
using ProdLogApp.Views.Interfaces;
using System;
using System.Windows;

namespace ProdLogApp.Views
{
    public partial class CategoryManagement : Window, ICategoryManagementView
    {
        private readonly CategoryManagementPresenter _presenter;
        private readonly User _activeUser;
        private readonly IDatabaseService _databaseService;
        public event Action OnAddCategory;
        public event Action OnDeleteCategory;
        public event Action OnModifyCategory;
        public event Action OnReturn;

        public CategoryManagement(User activeUser, IDatabaseService databaseService)
        {
            InitializeComponent();
            _activeUser = activeUser;
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
            _presenter = new CategoryManagementPresenter(this);
        }

        // Event handlers for category management actions
        private void AddCategory(object sender, RoutedEventArgs e) => OnAddCategory?.Invoke();
        private void DeleteCategory(object sender, RoutedEventArgs e) => OnDeleteCategory?.Invoke();
        private void ModifyCategory(object sender, RoutedEventArgs e) => OnModifyCategory?.Invoke();
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
