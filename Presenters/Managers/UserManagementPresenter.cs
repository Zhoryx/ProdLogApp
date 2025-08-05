using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Services;
using System;
using System.Windows;

namespace ProdLogApp.Presenters
{
    public class UserManagementPresenter
    {
        private readonly IUserManagementView _view;
        private readonly IDatabaseService _databaseService;

        public UserManagementPresenter(IDatabaseService databaseService, IUserManagementView view)
        {
            _view = view;
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));


          
            _view.OnAddUser += AddUser;
            _view.OnToggleUserStatus += ToggleUserStatus;
            _view.OnModifyUser += ModifyUser;
            UsersGet();
        }

        private async void UsersGet()
        {
            var users = await _databaseService.UsersGet(); 
            _view.ShowUsers(users);
        }


        private void AddUser()
        {
            if (_view.NewUser()) 
                UsersGet();
        }


        private void ToggleUserStatus()
        {
            User user_selected = _view.SelectedUser();

            if (user_selected == null)
            {
                _view.ShowMessage("Seleccione un Usuario para cambiar su estado.");
                return;
            }

            user_selected.Active = !user_selected.Active;
            _databaseService.ToggleCategoryStatus(user_selected.Id, user_selected.Active);
            _view.ShowMessage(user_selected.Active ? "Usuario desactivado correctamente." : "Usuario activado correctamente.");
            UsersGet();
        }

        private void ModifyUser()
        {
            User user_selected = _view.SelectedUser();
            if (user_selected == null)
            {
                _view.ShowMessage("Seleccione una Usuario para modificar.");
                return;
            }

            _view.ModifyUser(user_selected);
            UsersGet();
        }

    }
}
