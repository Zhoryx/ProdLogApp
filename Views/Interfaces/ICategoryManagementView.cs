using System;

namespace ProdLogApp.Views.Interfaces
{
    public interface ICategoryManagementView
    {
        // Event triggered when the "Add Category" action is requested
        event Action OnAddCategory;

        // Event triggered when the "Delete Category" action is requested
        event Action OnDeleteCategory;

        // Event triggered when the "Modify Category" action is requested
        event Action OnModifyCategory;

        // Event triggered when returning to the previous menu
        event Action OnReturn;

        // Method to close the current window
        void CloseWindow();

        // Method to navigate back to the main menu
        void NavigateToMenu();
    }
}
