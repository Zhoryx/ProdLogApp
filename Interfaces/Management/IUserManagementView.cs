using ProdLogApp.Models;
using System;

namespace ProdLogApp.Interfaces
{
    public interface IUserManagementView
    {
        event Action OnAddUser;
        event Action OnModifyUser;
        event Action OnToggleUserStatus;
        event Action OnReturn;
        void ShowUsers(List<User> Users);
        User SelectedUser();
        bool NewUser();
        void ModifyUser(User user);

        void ShowMessage(string msg);

        void NavigateToMenu();
    }
}
