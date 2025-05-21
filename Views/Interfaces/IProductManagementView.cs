using System;

namespace ProdLogApp.Views
{
    public interface IProductManagementView
    {
        event Action OnAgregarProducto;
        event Action OnEliminarProducto;
        event Action OnModificarProducto;
        event Action OnVolver;

        void NavegarAMenu();
    }
}
