using ProdLogApp.Models;
using ProdLogApp.Views;
using ProdLogApp.Views;
using ProdLogApp.Interfaces;
using ProdLogApp.Services;
using System;
using System.Windows;

namespace ProdLogApp.Presenters
{
    public class ManagerMenuPresenter
    {
        private readonly IManagerMenuView _view;
        private readonly User _activeUser;
        private readonly IDatabaseService _databaseService; // Nuevo campo

        public ManagerMenuPresenter(IManagerMenuView view, User activeUser, IDatabaseService databaseService)
        {
            _view = view;
            _activeUser = activeUser;
            _databaseService = databaseService; 

            // Suscripción a los eventos de la vista
            _view.OnOpenDailyReports += OpenDailyReports;
            _view.OnManageProducts += OpenProductManagement;
            _view.OnManageCategories += OpenCategoryManagement;
            _view.OnManagePositions += OpenPositionManagement;
            _view.OnManageUsers += OpenUserManagement;
            _view.OnDisconnect += CloseMenu;
        }

        // Métodos para navegación
        private void OpenDailyReports() => NavigateTo(new ManagerProduction(_activeUser, _databaseService));
        private void OpenProductManagement() => NavigateTo(new ProductManagement(_activeUser, _databaseService)); 
        private void OpenCategoryManagement() => NavigateTo(new CategoryManagement( _databaseService));
        private void OpenPositionManagement() => NavigateTo(new PositionManagement(_activeUser, _databaseService));
        private void OpenUserManagement() => NavigateTo(new UserManagement(_activeUser, _databaseService));

        // Cierra el menú y navega al login
        private void CloseMenu()
        {
            try
            {
                _view.NavigateToLogin();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cerrar sesión: {ex.Message}");
            }
        }

        // Maneja la navegación a otra ventana y cierra la actual
        private void NavigateTo(Window window)
        {
            try
            {
                window.Show();
                _view.CloseWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir la ventana: {ex.Message}");
            }
        }


    }
}
