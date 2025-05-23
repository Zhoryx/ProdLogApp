using System;

namespace ProdLogApp.Views.Interfaces
{
    public interface IManagerMenuView
    {
        // Event triggered when the "Daily Reports" button is clicked
        event Action OnOpenDailyReports;

        // Event triggered when the "Manage Products" button is clicked
        event Action OnManageProducts;

        // Event triggered when the "Manage Categories" button is clicked
        event Action OnManageCategories;

        // Event triggered when the "Manage Positions" button is clicked
        event Action OnManagePositions;

        // Event triggered when the "Manage Users" button is clicked
        event Action OnManageUsers;

        // Event triggered when the "Disconnect" button is clicked
        event Action OnDisconnect;

        // Method to close the current window
        void CloseWindow();

        // Method to navigate to the login screen
        void NavigateToLogin();
    }
}
