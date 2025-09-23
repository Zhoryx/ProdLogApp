// Presenters/AddUserPresenter.cs
using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProdLogApp.Presenters
{
    public class AddUserPresenter
    {
        private readonly IDatabaseService _db;
        private readonly IAddUserView _view;
        private readonly User? _editingUser;

        public AddUserPresenter(IDatabaseService db, IAddUserView view, User? user = null)
        {
            _db = db;
            _view = view;
            _editingUser = user;

            _view.OnConfirm += Confirm;
            _view.OnCancel += () => _view.CloseDialogCancel();

            if (user != null)
            {
                _view.LoadUser(user);
                _view.SetEditMode(true);
            }
            else
            {
                _view.SetEditMode(false);
            }
        }

        private async void Confirm()
        {
            // Validaciones simples
            if (string.IsNullOrWhiteSpace(_view.Nombre))
            {
                _view.ShowWarn("Ingresá el nombre.");
                return;
            }

            if (string.IsNullOrWhiteSpace(_view.DNI) || !Regex.IsMatch(_view.DNI, @"^\d{7,10}$"))
            {
                _view.ShowWarn("Ingresá un DNI válido (solo números).");
                return;
            }

            try
            {
                if (_editingUser == null)
                {
                    // Alta
                    var newUser = new User
                    {
                        Name = _view.Nombre,
                        Dni = _view.DNI,
                        // IsGerente / IsManager según tu modelo
                        // IsGerente = _view.EsGerencial
                    };

                    // Si es gerencial, pasá el pass (si tu API lo requiere)
                    // await _db.Users.CreateAsync(newUser, _view.EsGerencial ? _view.PasswordGerencial : null);

                    _view.ShowInfo("Usuario guardado correctamente.");
                }
                else
                {
                    // Edición
                    var toUpdate = new User
                    {
                        // Id = _editingUser.Id,
                        Name = _view.Nombre,
                        Dni = _view.DNI,
                        // IsGerente = _view.EsGerencial
                    };

                    // Si hay password nuevo y aplica, pasalo
                    // await _db.Users.UpdateAsync(toUpdate, !string.IsNullOrWhiteSpace(_view.PasswordGerencial) ? _view.PasswordGerencial : null);

                    _view.ShowInfo("Usuario actualizado correctamente.");
                }

                _view.CloseDialogOK();
            }
            catch (Exception ex)
            {
                _view.ShowWarn($"Error al guardar el usuario: {ex.Message}");
            }
        }
    }
}
