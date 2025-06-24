using System;
using System.Windows;
using ProdLogApp.Views;
using ProdLogApp.Interfaces;

namespace ProdLogApp.Presenters
{
    public class PositionManagementPresenter
    {
        private readonly IPositionManagementView _view;

        public PositionManagementPresenter(IPositionManagementView view)
        {
            _view = view;

            // Subscribe events from the view
            _view.OnAddPosition += AddPosition;
            _view.OnDeletePosition += DeletePosition;
            _view.OnModifyPosition += ModifyPosition;
            _view.OnReturn += ReturnToMenu;
        }

        // Handles adding a new position
        private void AddPosition() => MessageBox.Show("Add position...");

        // Handles deleting a position
        private void DeletePosition() => MessageBox.Show("Delete position...");

        // Handles modifying an existing position
        private void ModifyPosition() => MessageBox.Show("Modify position...");

        // Navigates back to the main menu
        private void ReturnToMenu() => _view.NavigateToMenu();
    }
}
