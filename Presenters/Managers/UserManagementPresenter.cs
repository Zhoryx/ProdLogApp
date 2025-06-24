using System;
using System.Windows;
using ProdLogApp.Interfaces;

namespace ProdLogApp.Presenters
{
    public class UserManagementPresenter
    {
        private readonly IUserManagementView _view;

        public UserManagementPresenter(IUserManagementView view)
        {
            _view = view;

            // Subscribe events from the view
            _view.OnAddUser += AddUser;
            _view.OnDeleteUser += DeleteUser;
            _view.OnModifyUser += ModifyUser;
            _view.OnReturn += ReturnToMenu;
        }

        // Handles adding a new user
        private void AddUser() => MessageBox.Show("Add user...");

        // Handles deleting a user
        private void DeleteUser() => MessageBox.Show("Delete user...");

        // Handles modifying an existing user
        private void ModifyUser() => MessageBox.Show("Modify user...");

        // Navigates back to the main menu
        private void ReturnToMenu() => _view.NavigateToMenu();
    }
}
