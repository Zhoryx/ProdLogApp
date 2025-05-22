using System;
using System.Windows;
using ProdLogApp.Views.Interfaces;

namespace ProdLogApp.Presenters
{
    public class PositionManagementPresenter
    {
        private readonly IPositionManagementView _view;

        public PositionManagementPresenter(IPositionManagementView view)
        {
            _view = view;

            _view.OnAgregarPuesto += AgregarPuesto;
            _view.OnEliminarPuesto += EliminarPuesto;
            _view.OnModificarPuesto += ModificarPuesto;
            _view.OnVolver += Volver;
        }

        private void AgregarPuesto() => MessageBox.Show("Agregar puesto...");
        private void EliminarPuesto() => MessageBox.Show("Eliminar puesto...");
        private void ModificarPuesto() => MessageBox.Show("Modificar puesto...");
        private void Volver() => _view.NavegarAMenu();
    }
}
