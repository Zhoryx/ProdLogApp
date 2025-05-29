using System;

namespace ProdLogApp.Views
{
    public interface IProductManagementView
    {
        
        

        // Event triggered when the "Delete Product" action is requested
        event Action OnDeleteProduct;

        // Event triggered when the "Modify Product" action is requested
        event Action OnModifyProduct;

        // Event triggered when returning to the previous menu
        event Action OnReturn;

        // Method to navigate back to the main menu
        void NavigateToMenu();
    }
}
