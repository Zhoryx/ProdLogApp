﻿using ProdLogApp.Models;
using ProdLogApp.Presenters;
using ProdLogApp.Services;
using ProdLogApp.Views;
using ProdLogApp.Interfaces;
using System;
using System.Windows;

namespace ProdLogApp.Views
{
    public partial class UserManagement : Window, IUserManagementView
    {
        private readonly UserManagementPresenter _presenter;
        private readonly User _activeUser;
        private readonly IDatabaseService _databaseService;
        public event Action OnAddUser;
        public event Action OnDeleteUser;
        public event Action OnModifyUser;
        public event Action OnReturn;

        public UserManagement(User activeUser, IDatabaseService databaseService)
        {
            InitializeComponent();
            _activeUser = activeUser;
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));

            _presenter = new UserManagementPresenter(this);
        }

        // Event handlers for user management actions
        private void AddUser(object sender, RoutedEventArgs e) => OnAddUser?.Invoke();
        private void DeleteUser(object sender, RoutedEventArgs e) => OnDeleteUser?.Invoke();
        private void ModifyUser(object sender, RoutedEventArgs e) => OnModifyUser?.Invoke();
        private void ReturnToMenu(object sender, RoutedEventArgs e) => OnReturn?.Invoke();

        // Method to navigate back to the main menu
        public void NavigateToMenu()
        {
            ManagerMenu menu = new ManagerMenu(_activeUser);
            menu.Show();
            this.Close();
        }
    }
}
