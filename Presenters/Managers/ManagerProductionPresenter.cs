using ProdLogApp.Models;
using ProdLogApp.Views;
using ProdLogApp.Interfaces;
using System;
using System.Windows;

namespace ProdLogApp.Presenters
{
    public class ManagerProductionPresenter
    {
        private readonly IManagerProductionView _view;
        private readonly User _activeUser;

        public ManagerProductionPresenter(IManagerProductionView view, User activeUser)
        {
            _view = view;
            _activeUser = activeUser;
            _view.OnAgregarProduccion += AgregarProduccion;
            _view.OnEliminarProduccion += EliminarProduccion;
            _view.OnModificarProduccion += ModificarProduccion;
            _view.OnVolver += Volver;
        }

        private void AgregarProduccion() => MessageBox.Show("Agregar producción...");
        private void EliminarProduccion() => MessageBox.Show("Eliminar producción...");
        private void ModificarProduccion() => MessageBox.Show("Modificar producción...");
        private void Volver() => _view.NavegarAMenu(); 
    }
}
