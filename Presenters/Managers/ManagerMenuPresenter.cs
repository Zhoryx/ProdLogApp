using System;
using System.Windows;
using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Services;
using ProdLogApp.Views;

namespace ProdLogApp.Presenters
{
    public class ManagerMenuPresenter
    {
        private readonly IManagerMenuView _view;
        private readonly User _activeUser;
        private readonly IDatabaseService _databaseService;

        public ManagerMenuPresenter(IManagerMenuView view, User activeUser, IDatabaseService databaseService)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _activeUser = activeUser ?? throw new ArgumentNullException(nameof(activeUser));
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));

            // Suscripción a eventos
            _view.OnOpenDailyReports += OpenDailyReports;
            _view.OnManageProducts += OpenProductManagement;
            _view.OnManageCategories += OpenCategoryManagement;
            _view.OnManagePositions += OpenPositionManagement;
            _view.OnManageUsers += OpenUserManagement;
            _view.OnDisconnect += CloseMenu;
        }

        // --- Navegación ---


        private void OpenDailyReports()
        {
            try
            {
                var owner = _view as Window;
                owner?.Hide(); // “cierra” visualmente

                var win = new ManagerProduction(_activeUser, _databaseService)
                {
                    Owner = owner,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };

                win.ShowDialog(); // bloquea mientras está abierto
                owner?.Show();    // al cerrar el modal, vuelve el menú
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir Partes Diarios: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        // Estos módulos pueden seguir reemplazando el menú (no modales)
        private void OpenProductManagement() => NavigateToAndClose(new ProductManagement(_activeUser, _databaseService));
        private void OpenCategoryManagement() => NavigateToAndClose(new CategoryManagement(_activeUser, _databaseService));
        private void OpenPositionManagement() => NavigateToAndClose(new PositionManagement(_activeUser, _databaseService));
        private void OpenUserManagement() => NavigateToAndClose(new UserManagement(_activeUser, _databaseService));

        
        private void CloseMenu()
        {
            try
            {
                _view.NavigateToLogin();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cerrar sesión: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Helper para pantallas no modales (reemplazan el menú)
        private void NavigateToAndClose(Window window)
        {
            try
            {
                window.Show();
                _view.CloseWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir la ventana: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
