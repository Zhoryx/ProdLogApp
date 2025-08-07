using ProdLogApp.Models;
using ProdLogApp.Views;

namespace ProdLogApp.Interfaces
{
    public interface IPasswordRequestView
    {
        string EnteredPassword { get; } 
        void ShowMessage(string message);
        void CloseWindow();
        void ShowAdminMenu(User activeUser);
    }
}
