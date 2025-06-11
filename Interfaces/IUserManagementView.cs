using System;

namespace ProdLogApp.Interfaces
{
    public interface IUserManagementView
    {
        // Event triggered when the "Add User" action is requested
        event Action OnAddUser;

        // Event triggered when the "Delete User" action is requested
        event Action OnDeleteUser;

        // Event triggered when the "Modify User" action is requested
        event Action OnModifyUser;

        // Event triggered when returning to the previous menu
        event Action OnReturn;

        // Method to navigate back to the main menu
        void NavigateToMenu();
    }
}
