using System;

namespace ProdLogApp.Interfaces
{
    public interface IPositionManagementView
    {
        // Event triggered when the "Add Position" action is requested
        event Action OnAddPosition;

        // Event triggered when the "Delete Position" action is requested
        event Action OnDeletePosition;

        // Event triggered when the "Modify Position" action is requested
        event Action OnModifyPosition;

        // Event triggered when returning to the previous menu
        event Action OnReturn;

        // Method to close the current window
        void CloseWindow();

        // Method to navigate back to the main menu
        void NavigateToMenu();
    }
}
