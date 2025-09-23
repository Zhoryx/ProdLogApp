// Presenters/UserManagementPresenter.cs
using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProdLogApp.Presenters
{
    public class UserManagementPresenter
    {
        private readonly IDatabaseService _db;
        private readonly IUserManagementView _view;

        public UserManagementPresenter(IDatabaseService db, IUserManagementView view)
        {
            _db = db;
            _view = view;

            _view.OnAddUser += OnAddUser;
            _view.OnModifyUser += OnModifyUser;
            _view.OnToggleUserStatus += OnToggleUserStatus;
            _view.OnReturn += () => _view.NavigateToMenu();

            _ = LoadUsersAsync();
        }

        private async Task LoadUsersAsync()
        {
            try
            {
                var users = await _db.UsersGet(soloActivos: false);
                _view.ShowUsers(users);
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"Error al cargar usuarios: {ex.Message}");
            }
        }

        private async void OnAddUser()
        {
            try
            {
                var ok = _view.NewUser();   // abre AddUser sin objeto => alta
                if (ok) await LoadUsersAsync();
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"Error al agregar: {ex.Message}");
            }
        }

        private async void OnModifyUser()
        {
            var user = _view.SelectedUser();
            if (user == null)
            {
                _view.ShowMessage("Seleccioná un usuario para modificar.");
                return;
            }

            try
            {
                _view.ModifyUser(user);     // abre AddUser con el objeto => edición
                await LoadUsersAsync();
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"Error al modificar: {ex.Message}");
            }
        }

        private async void OnToggleUserStatus()
        {
            var user = _view.SelectedUser();
            if (user == null)
            {
                _view.ShowMessage("Seleccioná un usuario.");
                return;
            }

            try
            {
                await _db.ToggleUserStatusAsync(user.Id, user.Active);
                await LoadUsersAsync();
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"No se pudo cambiar el estado: {ex.Message}");
            }
        }
    }
}
