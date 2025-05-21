using System;
using System.Windows;
using ProdLogApp.Views.Interfaces;

namespace ProdLogApp.Presenters
{
    public class UserManagementPresenter
    {
        private readonly IUserManagementView _view;

        public UserManagementPresenter(IUserManagementView view)
        {
            _view = view;

            _view.OnAgregarUsuario += AgregarUsuario;
            _view.OnEliminarUsuario += EliminarUsuario;
            _view.OnModificarUsuario += ModificarUsuario;
            _view.OnVolver += Volver;
        }

        private void AgregarUsuario() => MessageBox.Show("Agregar usuario...");
        private void EliminarUsuario() => MessageBox.Show("Eliminar usuario...");
        private void ModificarUsuario() => MessageBox.Show("Modificar usuario...");
        private void Volver() => _view.NavegarAMenu(); // ✅ Ahora la vista maneja la navegación
    }
}
