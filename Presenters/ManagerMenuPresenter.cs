using System;
using System.Windows;
using ProdLogApp.Models;
using ProdLogApp.Views;

namespace ProdLogApp.Presenters
{
    public class ManagerMenuPresenter
    {
        private readonly IManagerMenuView _view;
        private readonly User _activeUser;

        public ManagerMenuPresenter(IManagerMenuView view, User activeUser)
        {
            _view = view;
            _activeUser = activeUser;

            _view.OnAbrirPartesDiarios += AbrirPartesDiarios;
            _view.OnMaestroProducto += AbrirGestionProducto;
            _view.OnMaestroCategoria += AbrirGestionCategoria;
            _view.OnMaestroPuesto += AbrirGestionPuesto;
            _view.OnMaestroUsuario += AbrirGestionUsuario;
            _view.OnDesconectar += CerrarMenu;
        }

        private void AbrirPartesDiarios() => NavegarA(new ManagerProduction(_activeUser));
        private void AbrirGestionProducto() => NavegarA(new ProductManagement(_activeUser));
        private void AbrirGestionCategoria() => NavegarA(new CategoryManagement());
        private void AbrirGestionPuesto() => NavegarA(new PositionManagement(_activeUser));
        private void AbrirGestionUsuario() => NavegarA(new UserManagement(_activeUser));
        private void CerrarMenu() => _view.CerrarVentana();

        private void NavegarA(Window ventana)
        {
            try
            {
                ventana.Show();
                _view.CerrarVentana();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir la ventana: {ex.Message}");
            }
        }
    }

    public interface IManagerMenuView
    {
        event Action OnAbrirPartesDiarios;
        event Action OnMaestroProducto;
        event Action OnMaestroCategoria;
        event Action OnMaestroPuesto;
        event Action OnMaestroUsuario;
        event Action OnDesconectar;

        void CerrarVentana();
    }
}
