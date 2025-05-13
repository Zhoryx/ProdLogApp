using ProdLogApp.Models;

namespace ProdLogApp.Views.Interfaces
{
    public interface ILoginView
    {
        string Dni { get; }
        void ShowMessage(string message);
        void ShowAdminWindow(User ActiveUser);
        void ShowMainWindow(User ActiveUser);
    }
}
