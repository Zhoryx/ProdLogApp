using System;
using System.Windows;
using ProdLogApp.Views;

namespace ProdLogApp.Presenters
{
    public class ProductManagementPresenter
    {
        private readonly IProductManagementView _view;

        public ProductManagementPresenter(IProductManagementView view)
        {
            _view = view;

            _view.OnAgregarProducto += AgregarProducto;
            _view.OnEliminarProducto += EliminarProducto;
            _view.OnModificarProducto += ModificarProducto;
            _view.OnVolver += Volver;
        }

        private void AgregarProducto() => MessageBox.Show("Agregar producto...");
        private void EliminarProducto() => MessageBox.Show("Eliminar producto...");
        private void ModificarProducto() => MessageBox.Show("Modificar producto...");
        private void Volver() => _view.NavegarAMenu();
    }
}
