using System;

namespace ProdLogApp.Views.Interfaces
{
    public interface IManagerMenuView
    {
        event Action OnAbrirPartesDiarios;
        event Action OnMaestroProducto;
        event Action OnMaestroCategoria;
        event Action OnMaestroPuesto;
        event Action OnMaestroUsuario;
        event Action OnDesconectar;

        void NavegarAMenu();
    }
}
