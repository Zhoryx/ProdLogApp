using ProdLogApp.Models;
using ProdLogApp.Views;
using ProdLogApp.Views.Interfaces;
using System;
using System.Windows;

namespace ProdLogApp.Presenters
{
    public class ManagerMenuPresenter
    {
        private readonly IManagerMenuView _view;
        private readonly User _activeUser;

        public ManagerMenuPresenter(IManagerMenuView view, User activeUser)
        {
            _view = view;
            _activeUser = activeUser;

            // Subscribe events from the view
            _view.OnOpenDailyReports += OpenDailyReports;
            _view.OnManageProducts += OpenProductManagement;
            _view.OnManageCategories += OpenCategoryManagement;
            _view.OnManagePositions += OpenPositionManagement;
            _view.OnManageUsers += OpenUserManagement;
            _view.OnDisconnect += CloseMenu;
        }

        // Methods to navigate to different management screens
        private void OpenDailyReports() => NavigateTo(new ManagerProduction(_activeUser));
        private void OpenProductManagement() => NavigateTo(new ProductManagement(_activeUser));
        private void OpenCategoryManagement() => NavigateTo(new CategoryManagement(_activeUser));
        private void OpenPositionManagement() => NavigateTo(new PositionManagement(_activeUser));
        private void OpenUserManagement() => NavigateTo(new UserManagement(_activeUser));

        // Closes the current menu and navigates to the login screen
        private void CloseMenu()
        {
            try
            {
                _view.NavigateToLogin();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while logging out: {ex.Message}");
            }
        }

        // Handles navigation to another window and closes the current one
        private void NavigateTo(Window window)
        {
            try
            {
                window.Show();
                _view.CloseWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while opening the window: {ex.Message}");
            }
        }
    }
}
