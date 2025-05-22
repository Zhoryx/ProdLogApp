using System;
using System.Windows;
using ProdLogApp.Views.Interfaces;

namespace ProdLogApp.Presenters
{
    public class CategoryManagementPresenter
    {
        private readonly ICategoryManagementView _view;

        public CategoryManagementPresenter(ICategoryManagementView view)
        {
            _view = view;

            _view.OnAgregarCategoria += AgregarCategoria;
            _view.OnEliminarCategoria += EliminarCategoria;
            _view.OnModificarCategoria += ModificarCategoria;
            _view.OnVolver += Volver;
        }

        private void AgregarCategoria() => MessageBox.Show("Agregar categoría...");
        private void EliminarCategoria() => MessageBox.Show("Eliminar categoría...");
        private void ModificarCategoria() => MessageBox.Show("Modificar categoría...");
        private void Volver() => _view.NavegarAMenu(); // ✅ Ahora la vista maneja la navegación
    }
}
